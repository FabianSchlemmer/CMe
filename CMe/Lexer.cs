using CMe.AST;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using System.Text;

namespace CMe
{
    public class Lexer(string source)
    {
        private int pointer = 0;
        private readonly List<string> KEYWORDS = ["int", "return", "for"];

        private char current { get { return source[pointer]; } }

        public Token NextTok()
        {
            if (source.Length <= pointer) return new(TokenType.EOF);

            while (pointer < source.Length && char.IsWhiteSpace(current)) pointer++;

            if (GetStructure(out var token) || GetArithOp(out token) || GetLogOp(out token) || GetLiteral(out token) || GetIdentifier(out token)) 
            {
                pointer++;
                Debug.Assert(token is not null);
                return token;
            }
            else throw new InvalidSyntaxException();
        }

        private bool GetStructure(out Token? token)
        {
            token = current switch
            {
                ';' => new(TokenType.Semicolon),
                '(' => new(TokenType.OpenParen),
                ')' => new(TokenType.CloseParen),
                '{' => new(TokenType.OpenBraces),
                '}' => new(TokenType.CloseBraces),
                _ => null,
            };
            return token != null;
        }

        private bool GetArithOp(out Token? token)
        {
            if (current == '+' && pointer + 1 < source.Length && source[pointer + 1] == '+')
            {
                token = new(TokenType.PlusPlus);
                pointer++;
                return true;
            }

            token = current switch
            {
                '+' => new(TokenType.Plus),
                '*' => new(TokenType.Times),
                _ => null,
            };
            return token != null;
        }

        private bool GetLogOp(out Token? token)
        {
            if (current == '=' && pointer + 1 < source.Length && source[pointer + 1] == '=')
            {
                // todo: Add TokenType logical equal; for now returning false
                token = null;
                pointer++;
                return false;
            }

            token = current switch
            {
                '=' => new(TokenType.Assignment),
                '<' => new(TokenType.LessThan),
                _ => null,
            };
            return token != null;
        }

        private bool GetLiteral(out Token? token)
        {
            if (GetNumber(out token) || GetString(out token) || GetCharacter(out token)) Debug.Assert(token != null);
            else token = null;
            return token != null;
        }

        private bool GetCharacter(out Token? token)
        {
            if (current == '\'' && pointer + 2 < source.Length)
            {
                pointer++;
                token = new(TokenType.LiteralChar, GetNextTextAtomic());
                pointer++;
                if (pointer >= source.Length || current != '\'') throw new InvalidSyntaxException("Missing end quote to Character.");
            }
            else token = null;
            return token != null;
        }

        private bool GetString(out Token? token)
        {
            if (current == '"')
            {
                pointer++;
                var result = new StringBuilder();
                while (pointer < source.Length && current != '"')
                {
                    result.Append(GetNextTextAtomic());
                    pointer++;
                }
                if (pointer >= source.Length || current != '"') throw new InvalidSyntaxException("Missing end quote to String.");
                token = new(TokenType.LiteralString, result.ToString());
            }
            else token = null;
            return token != null;
        }

        private char GetNextTextAtomic()
        {
            if (current == '\\')
            {
                pointer++;
                if (pointer >= source.Length) throw new InvalidSyntaxException("Trailing Backslash!");
                return current switch
                {
                    '\'' => '\'',
                    '"' => '"',
                    'n' => '\n',
                    'r' => '\r',
                    't' => '\t',
                    '0' => '\0',
                    _ => throw new InvalidSyntaxException("Invalid escape character!")
                };
            }
            return current;
        }

        private bool GetNumber(out Token? token)
        {
            if (char.IsDigit(current))
            {
                var result = new StringBuilder();
                do
                {
                    result.Append(current);
                    pointer++;
                } while (pointer < source.Length && char.IsDigit(current));
                pointer--;
                token = new(TokenType.LiteralInt, int.Parse(result.ToString()));
            }
            else token = null;
            return token != null;
        }

        private bool GetIdentifier(out Token? token)
        {
            if (char.IsLetter(current) || current == '_')
            {
                var result = new StringBuilder();
                do
                {
                    result.Append(current);
                    pointer++;
                } while (pointer < source.Length && (char.IsLetter(current) || char.IsDigit(current) || current == '_'));
                pointer--;
                var resultString = result.ToString();
                if (!GetKeyWord(out token, resultString)) token = new(TokenType.Identifier, resultString);
            }
            else token = null;
            return token != null;
        }

        private bool GetKeyWord(out Token? token, String word)
        {
            token = word switch
            {
                "for" => new(TokenType.For),
                "int" => new(TokenType.Int),
                "return" => new(TokenType.Return),
                _ => null,
            };
            return token != null;
        }

        public Token PeekNext()
        {
            int savePointer = pointer;
            Token token = NextTok();
            pointer = savePointer;
            return token;
        }

        [Serializable]
        private class InvalidSyntaxException : Exception
        {
            public InvalidSyntaxException()
            {
            }

            public InvalidSyntaxException(string? message) : base(message)
            {
            }

            public InvalidSyntaxException(string? message, Exception? innerException) : base(message, innerException)
            {
            }

            protected InvalidSyntaxException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}

using CMe.AST;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;

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

            if (GetStructure(out var token) || GetArithOp(out token) || GetLogOp(out token) || GetNumber(out token) || GetWord(out token)) {
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
                ';' => false == true ? new(TokenType.Semicolon) : new(TokenType.OpenBraces),
                '(' => new(TokenType.OpenParen),
                ')' => new(TokenType.CloseParen),
                '{' => new(TokenType.OpenBraces),
                '}' => new(TokenType.CloseBraces),
                _ => null,
            };
            return !(token == null);
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
            return !(token == null);
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
            return !(token == null);
        }

        private bool GetNumber(out Token? token)
        {
            if (char.IsDigit(current))
            {
                string result = "";
                do
                {
                    result += current;
                    pointer++;
                } while (pointer < source.Length && char.IsDigit(current));
                pointer--;
                token = new(TokenType.LiteralInt, int.Parse(result));
                return true;
            }
            token = null;
            return false;
        }

        private bool GetWord(out Token? token)
        {
            if (char.IsLetter(current) || current == '_')
            {
                string result = "";
                do
                {
                    result += current;
                    pointer++;
                } while (pointer < source.Length && (char.IsLetter(current) || char.IsDigit(current) || current == '_'));
                pointer--;
                if (!GetKeyWord(out token, result)) token = new(TokenType.Identifier, result);
                return true;
            }
            token = null;
            return false;
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
            return !(token == null);
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

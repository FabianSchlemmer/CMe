using System.Diagnostics;
using System.Text;

namespace CMe
{
    public class Lexer(string source)
    {
        private int pointer = 0;

        private char current { get { return source[pointer]; } }

        public Token NextTok()
        {
            // char.IsWhiteSpace() returns true for new line character (\n)
            while (pointer < source.Length && char.IsWhiteSpace(current)) pointer++;

            if (source.Length <= pointer) return new(TokenType.EOF);

            // Using the fact, that upon finding the first True, the rest of the condition is not checked, to only run as much code as necessary.
            // Could use a big switch statement instead, but better overview this way.
            if (GetStructure(out var tok) || GetArithOp(out tok) || GetLogOp(out tok) || GetLiteral(out tok) || GetIdentifier(out tok)) 
            {
                pointer++;
                Debug.Assert(tok is not null);
                return tok;
            }
            else throw new InvalidSyntaxException();
        }

        private bool GetStructure(out Token? tok)
        {
            tok = current switch
            {
                ';' => new(TokenType.Semicolon),
                ',' => new(TokenType.Comma),
                '(' => new(TokenType.OpenParen),
                ')' => new(TokenType.CloseParen),
                '{' => new(TokenType.OpenBraces),
                '}' => new(TokenType.CloseBraces),
                _ => null,
            };
            return tok != null;
        }

        private bool GetArithOp(out Token? tok)
        {
            // Handle ++
            if (current == '+' && pointer + 1 < source.Length && source[pointer + 1] == '+')
            {
                tok = new(TokenType.PlusPlus);
                pointer++;
                return true;
            }

            tok = current switch
            {
                '+' => new(TokenType.Plus),
                '*' => new(TokenType.Times),
                _ => null,
            };
            return tok != null;
        }

        private bool GetLogOp(out Token? tok)
        {
            // Handle ==
            if (current == '=' && pointer + 1 < source.Length && source[pointer + 1] == '=')
            {
                tok = new(TokenType.EqualEqual);
                pointer++;
                return true;
            }

            tok = current switch
            {
                '=' => new(TokenType.Assignment),
                '<' => new(TokenType.LessThan),
                _ => null,
            };
            return tok != null;
        }

        private bool GetLiteral(out Token? tok)
        {
            // Same Principle as above. Exit condition as soon as one is true.
            if (GetNumber(out tok) || GetString(out tok) || GetCharacter(out tok)) Debug.Assert(tok != null);
            else tok = null;
            return tok != null;
        }

        private bool GetCharacter(out Token? tok)
        {
            // Make sure there is enough left of source for this to be a valid character.
            if (current == '\'' && pointer + 2 < source.Length)
            {
                pointer++;
                tok = new(TokenType.LiteralChar, GetNextTextAtomic());
                pointer++;
                if (pointer >= source.Length || current != '\'') throw new InvalidSyntaxException("Missing end quote to Character.");
            }
            else tok = null;
            return tok != null;
        }

        private bool GetString(out Token? tok)
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
                tok = new(TokenType.LiteralString, result.ToString());
            }
            else tok = null;
            return tok != null;
        }

        // Handles escape characters and returns current otherwise
        private char GetNextTextAtomic()
        {
            if (current == '\\')
            {
                pointer++;
                if (pointer >= source.Length) throw new InvalidSyntaxException("Trailing Backslash!");
                return current switch
                {
                    '\\' => '\\',
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

        private bool GetNumber(out Token? tok)
        {
            if (char.IsDigit(current))
            {
                var result = new StringBuilder();
                // Using a do-while instead of a while, because while condition includes char.IsDigit()-check, and we just checked that already
                do
                {
                    result.Append(current);
                    pointer++;
                } while (pointer < source.Length && char.IsDigit(current));
                // Decrement pointer because it will be incremented to correct spot in NextTok().
                pointer--;
                tok = new(TokenType.LiteralInt, int.Parse(result.ToString()));
            }
            else tok = null;
            return tok != null;
        }

        private bool GetIdentifier(out Token? tok)
        {
            // Identifiers may start with letters or underscores (_), but not digits or special characters
            if (char.IsLetter(current) || current == '_')
            {
                var result = new StringBuilder();
                // Using a do-while because of duplicate condition check
                do
                {
                    result.Append(current);
                    pointer++;
                // Identifiers may contain numbers as long as they are not the first character
                } while (pointer < source.Length && (char.IsLetter(current) || char.IsDigit(current) || current == '_'));
                // Decrement pointer because it will be incremented to correct spot in NextTok().
                pointer--;
                var resultString = result.ToString();
                // Check if our identifier is a KeyWord
                if (!GetKeyWord(out tok, resultString)) tok = new(TokenType.Identifier, resultString);
            }
            else tok = null;
            return tok != null;
        }

        private bool GetKeyWord(out Token? tok, String str)
        {
            tok = str switch
            {
                // TODO: Currently Missing (from existing TokenTypes): Struct, Enum, TypeDef
                "int" => new(TokenType.Int),
                "char" => new(TokenType.Char),
                "void" => new(TokenType.Void),
                "float" => new(TokenType.Float),
                "double" => new(TokenType.Double),
                "return" => new(TokenType.Return),
                "for" => new(TokenType.For),
                _ => null,
            };
            return tok != null;
        }

        public Token PeekNext()
        {
            int savePointer = pointer;
            Token token = NextTok();
            pointer = savePointer;
            return token;
        }
    }
}

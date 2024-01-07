using System.Diagnostics;
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
            switch (current)
            {
                case ';':
                    token = new(TokenType.Semicolon);
                    break;
                case '(':
                    token = new(TokenType.OpenParen);
                    break;
                case ')':
                    token = new(TokenType.CloseParen);
                    break;
                case '{':
                    token = new(TokenType.OpenBraces);
                    break;
                case '}':
                    token = new(TokenType.CloseBraces);
                    break;
                default:
                    token = null;
                    return false;
            }
            return true;            
        }

        private bool GetArithOp(out Token? token)
        {
            switch (current)
            {
                case '+':
                    if (pointer + 1 < source.Length && source[pointer + 1] == '+')
                    {
                        token = new(TokenType.PlusPlus);
                        pointer++;
                    }
                    else token = new(TokenType.Plus);
                    break;
                case '*':
                    token = new(TokenType.Times);
                    break;
                default:
                    token = null;
                    return false;
            }
            return true;
        }

        private bool GetLogOp(out Token? token)
        {
            switch (current)
            {
                case '=':
                    if (pointer + 1 < source.Length && source[pointer + 1] == '=')
                    {
                        // todo: Add TokenType logical equal; for now returning false
                        token = null;
                        return false;
                    }
                    else token = new(TokenType.Assignment);
                    break;
                case '<':
                    token = new(TokenType.LessThan);
                    break;
                default:
                    token = null;
                    return false;
            }
            return true;
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
            switch (word)
            {
                case "for":
                    token = new(TokenType.For);
                    break;
                case "int":
                    token = new(TokenType.Int);
                    break;
                case "return":
                    token = new(TokenType.Return);
                    break;
                default:
                    token = null;
                    return false;
            }
            return true;
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

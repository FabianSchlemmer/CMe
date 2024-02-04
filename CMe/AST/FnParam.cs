using System.Diagnostics;

namespace CMe.AST
{
    // Function Parameters, used in Function Definitions
    public class FnParam(TypeKind type, string name)
    {
        public static List<FnParam> ParseAll(Lexer lexer)
        {
            var braces = lexer.NextTok();
            if (braces.Type != TokenType.OpenParen) throw new ParseException($"Expected Function Args Definition. Got {braces}");
            var parameters = new List<FnParam>();
            var tok = lexer.NextTok();
            var previousTokT = TokenType.OpenParen;

            while (tok.Type != TokenType.CloseParen)
            {
                if (((previousTokT == TokenType.OpenParen || previousTokT == TokenType.Comma) && Parser.HasReturnableType(tok.Type, out var tk))
                    || (previousTokT == TokenType.Identifier && tok.Type == TokenType.Comma))
                {
                }
                else if (Parser.HasReturnableType(previousTokT, out tk) && tok.Type == TokenType.Identifier)
                {
                    Debug.Assert(tk != null);
                    parameters.Add(new FnParam((TypeKind)tk, tok.ValueAs<String>()));
                }
                else
                {
                    throw new ParseException($"Expected Function Parameters. Got {tok}.");
                }
                previousTokT = tok.Type;
                tok = lexer.NextTok();
            }
            return parameters;
        }
    }
}

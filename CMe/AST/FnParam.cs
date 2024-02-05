using System.Diagnostics;

namespace CMe.AST
{
    // Function Parameters, used in Function Definitions
    public class FnParam(TypeKind type, string name)
    {
        public static FnParam Parse(Lexer lexer)
        {
            if (!Parser.HasReturnableType(out TypeKind? paramType, lexer.NextTok().Type)) throw new ParseException($"Expected Parameter Type. Got {paramType}.");
            var ident = Parser.ExpectToken(lexer, TokenType.Identifier);

            Debug.Assert(paramType != null);
            return new FnParam((TypeKind)paramType, ident.ValueAs<string>());
        }
    }
}

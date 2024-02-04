namespace CMe.AST
{
    // parent of every expression
    public abstract class Expr : BaseNode
    {
        public static Expr Parse(Lexer lexer)
        {
            return lexer.PeekNext().Type switch
            {
                TokenType.Identifier => FunctionCall.Parse(lexer),
                TokenType.LiteralString => StringLiteral.Parse(lexer),
                _ => IntLiteral.Parse(lexer),
            };
        }
    }
}

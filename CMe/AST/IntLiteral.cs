namespace CMe.AST
{
    public class IntLiteral(int value) : Expr
    {
        public int Value { get { return value; } }

        public new static IntLiteral Parse(Lexer lexer)
        {
            var tok = Parser.ExpectToken(lexer, TokenType.LiteralInt);
            return new IntLiteral(tok.ValueAs<int>());
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

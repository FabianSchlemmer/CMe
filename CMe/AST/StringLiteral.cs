namespace CMe.AST
{
    public class StringLiteral(string value) : Expr
    {
        public string Value { get { return value; } }

        public new static StringLiteral Parse(Lexer lexer)
        {
            var tok = Parser.ExpectToken(lexer, TokenType.LiteralString);
            return new StringLiteral(tok.ValueAs<string>());
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

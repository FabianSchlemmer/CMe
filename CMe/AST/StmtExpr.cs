namespace CMe.AST
{
    // Statement Wrapper around Expressions
    public class StmtExpr(Expr expr) : Stmt
    {
        public new static StmtExpr Parse(Lexer lexer)
        {
            var expr = Expr.Parse(lexer);
            var tok = lexer.NextTok();
            if (tok.Type != TokenType.Semicolon) throw new ParseException($"Expected end of Statement (;). Got {tok}.");
            return new StmtExpr(expr);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void AcceptExpr(IVisitor visitor)
        {
            expr.Accept(visitor);
        }
    }
}

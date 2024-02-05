namespace CMe.AST
{
    // Statement Wrapper around Expressions
    public class StmtExpr(Expr expr) : Stmt
    {
        public new static StmtExpr Parse(Lexer lexer)
        {
            var expr = Expr.Parse(lexer);
            Parser.ExpectToken(lexer, TokenType.Semicolon);
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

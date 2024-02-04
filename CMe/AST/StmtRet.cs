namespace CMe.AST
{
    // Return Instruction
    public class StmtRet(Expr expr) : Stmt
    {
        public new static StmtRet Parse(Lexer lexer)
        {
            Parser.ExpectToken(lexer, TokenType.Return);
            var expr = Expr.Parse(lexer);
            Parser.ExpectToken(lexer, TokenType.Semicolon);
            return new StmtRet(expr);
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

namespace CMe.AST
{
    // function call of type expression, sometimes wrapped in a statement (if semicolon afterwards) but doesn't have to be
    public class FunctionCall(string name, List<Expr> args) : Expr
    {
        public string Name { get { return name; } }

        public new static FunctionCall Parse(Lexer lexer)
        {
            var ident = Parser.ExpectToken(lexer, TokenType.Identifier);
            Parser.ExpectToken(lexer, TokenType.OpenParen);
            var args = new List<Expr>();
            var tok = lexer.PeekNext();
            while (tok.Type != TokenType.CloseParen)
            {
                args.Add(Expr.Parse(lexer));
                tok = Parser.ExpectToken(lexer, TokenType.Comma, TokenType.CloseParen);
            }
            // if CloseParen was peeked (empty args)
            if (args.Count == 0) lexer.NextTok();
            return new FunctionCall(ident.ValueAs<string>(), args);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void AcceptArgs(IVisitor visitor)
        {
            foreach (var arg in args) arg.Accept(visitor);
        }
    }
}

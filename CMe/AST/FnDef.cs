using System.Diagnostics;

namespace CMe.AST
{
    // Function Definition
    public class FnDef(TypeKind type, string name, List<FnParam> parameters, List<Stmt> body) : TLDef
    {
        public string Name { get { return name; } }

        public new static FnDef Parse(Lexer lexer)
        {
            if (!Parser.HasReturnableType(out TypeKind? retType, lexer.NextTok().Type)) throw new ParseException($"Expected Return Type. Got {retType}.");
            var identifier = Parser.ExpectToken(lexer, TokenType.Identifier);

            Parser.ExpectToken(lexer, TokenType.OpenParen);
            var parameters = new List<FnParam>();

            // Declaration for while loop condition
            var next = lexer.PeekNext();
            while (next.Type != TokenType.CloseParen)
            {
                parameters.Add(FnParam.Parse(lexer));
                // Expect Valid next Tokens
                next = Parser.ExpectToken(lexer, TokenType.Comma, TokenType.CloseParen);
            }
            // In case the while loop condition was hit immediately, need to consume the Closing Parenthesis
            if (parameters.Count == 0) Parser.ExpectToken(lexer, TokenType.CloseParen);

            Parser.ExpectToken(lexer, TokenType.OpenBraces);
            var body = new List<Stmt>();
            next = lexer.PeekNext();
            // An empty body is a valid Function Definition
            while (next.Type != TokenType.CloseBraces)
            {
                body.Add(Stmt.Parse(lexer));
                next = lexer.PeekNext();
            }
            Parser.ExpectToken(lexer, TokenType.CloseBraces);

            Debug.Assert(retType != null);
            return new FnDef((TypeKind) retType, identifier.ValueAs<string>(), parameters, body);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void AcceptBody(IVisitor visitor)
        {
            foreach (var stmt in body) stmt.Accept(visitor);
        }
    }
}

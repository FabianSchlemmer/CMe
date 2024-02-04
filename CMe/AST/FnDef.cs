using System.Collections.Generic;

namespace CMe.AST
{
    // Function Definition
    public class FnDef(TypeKind type, string name, List<FnParam> parameters, List<Stmt> body) : TLDef
    {
        public string Name { get { return name; } }

        public static FnDef Parse(TypeKind tk, Lexer lexer)
        {
            var identifier = lexer.NextTok();
            if (identifier.Type != TokenType.Identifier) throw new ParseException($"Expected Identifier. Got {identifier}");
            var parameters = FnParam.ParseAll(lexer);
            var braces = lexer.NextTok();
            if (braces.Type != TokenType.OpenBraces) throw new ParseException($"Expected Function Body. Got {braces}.");
            var body = new List<Stmt>();
            while (lexer.PeekNext().Type != TokenType.CloseBraces)
            {
                var stmt = Stmt.Parse(lexer);
                body.Add(stmt);
            }
            Parser.ExpectToken(lexer, TokenType.CloseBraces);
            return new FnDef(tk, identifier.ValueAs<string>(), parameters, body);
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

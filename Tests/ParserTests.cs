using CMe;

namespace Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void HelloWorld()
        {
            string source =
            """
                int main()
                {
                    __stdout("Hello, World\n");
                    return 0;
                }
            """;

            var parser = new Parser(new Lexer(source));
            var ast = parser.Parse();
            var expected = new List<CMe.AST.TLDef>()
            {
                new CMe.AST.FnDef(TypeKind.Int, "main", new List<CMe.AST.FnParam>(), new List<CMe.AST.Stmt>() {
                    new CMe.AST.StmtExpr(
                        new CMe.AST.FunctionCall("__stdout", new List<CMe.AST.Expr>() {
                            new CMe.AST.StringLiteral("Hello, World\n")
                        })
                    ),
                    new CMe.AST.StmtRet(
                        new CMe.AST.IntLiteral(0)
                    )
                })
            };

            // TODO: Test cases
        }
    }
}

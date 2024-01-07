using CMe;

namespace Tests
{
    [TestClass]
    public class LexerTests
    {
        public List<Token> LexAll(string str)
        {
            var lexer = new Lexer(str);
            var lst = new List<Token>();
            Token tok;
            do
            {
                tok = lexer.NextTok();
                lst.Add(tok);
            } while (tok.Type != TokenType.EOF);
            return lst;
        }

        [TestMethod]
        public void TestTokenEquality()
        {
            Assert.IsTrue(new Token(TokenType.LiteralInt, 12).Equals(new Token(TokenType.LiteralInt, 12)));
        }

        [TestMethod]
        public void TestBinOp()
        {
            var toks = LexAll("x * 10");
            var expected = new List<Token>()
            {
                new(TokenType.Identifier, "x"),
                new(TokenType.Times),
                new(TokenType.LiteralInt, 10),
                new(TokenType.EOF)
            };
            CollectionAssert.AreEqual(expected, toks);
        }

        [TestMethod]
        public void TestFor()
        {
            var toks = LexAll("for (int i = 0; i < 10; ++i) {\n    Fun(someGlobal);\n}");
            var expected = new List<Token>()
            {
                new(TokenType.For),
                new(TokenType.OpenParen),
                new(TokenType.Int),
                new(TokenType.Identifier, "i"),
                new(TokenType.Assignment),
                new(TokenType.LiteralInt, 0),
                new(TokenType.Semicolon),
                new(TokenType.Identifier, "i"),
                new(TokenType.LessThan),
                new(TokenType.LiteralInt, 10),
                new(TokenType.Semicolon),
                new(TokenType.PlusPlus),
                new(TokenType.Identifier, "i"),
                new(TokenType.CloseParen),
                new(TokenType.OpenBraces),
                new(TokenType.Identifier, "Fun"),
                new(TokenType.OpenParen),
                new(TokenType.Identifier, "someGlobal"),
                new(TokenType.CloseParen),
                new(TokenType.Semicolon),
                new(TokenType.CloseBraces),
                new(TokenType.EOF)
            };
            CollectionAssert.AreEqual(expected, toks);
        }

        [TestMethod]
        public void TestNonsenseLex()
        {
            var toks = LexAll("Hej There return");
            var expected = new List<Token>()
            {
                new(TokenType.Identifier, "Hej"),
                new(TokenType.Identifier, "There"),
                new(TokenType.Return),
                new(TokenType.EOF)
            };
            CollectionAssert.AreEqual(expected, toks);
        }
    }
}

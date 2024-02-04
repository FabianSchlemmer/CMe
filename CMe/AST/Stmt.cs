using System.Reflection.Metadata.Ecma335;

namespace CMe.AST
{
    // Parent of all Statement Types
    public abstract class Stmt : BaseNode
    {
        public static Stmt Parse(Lexer lexer)
        {
            return lexer.PeekNext().Type switch
            {
                TokenType.Return => StmtRet.Parse(lexer),
                _ => StmtExpr.Parse(lexer),
            };
        }
    }
}

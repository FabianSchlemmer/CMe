namespace CMe.AST
{
    // function call of type expression, sometimes wrapped in a statement (if semicolon afterwards) but doesn't have to be
    public class FunctionCall(string name, List<Expr> args) : Expr
    {
    }
}

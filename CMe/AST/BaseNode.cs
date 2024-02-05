namespace CMe.AST
{
    // parent of all types of nodes in the abstract syntax tree
    public abstract class BaseNode
    {
        public abstract void Accept(IVisitor visitor);
    }
}

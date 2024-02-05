namespace CMe.AST
{
    // Holds all TLDefs; Root Node of AST
    public class Program : BaseNode
    {
        private List<TLDef> defs = new();

        public void Add(TLDef def)
        {
            defs.Add(def);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
            foreach (TLDef def in defs) def.Accept(visitor);
        }
    }
}

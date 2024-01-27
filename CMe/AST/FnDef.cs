using System.Collections.Generic;

namespace CMe.AST
{
    // Function Definition
    public class FnDef(TypeKind type, string name, List<FnParam> parameters, List<Stmt> body) : TLDef
    {
    }
}

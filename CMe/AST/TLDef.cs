using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace CMe.AST
{
    // Top Level Definition: Classes (Structs), Enums, Functions, etc.
    public abstract class TLDef : BaseNode
    {
        public static TLDef Parse(Lexer lexer)
        {
            var tokT = lexer.NextTok().Type;
            if (Parser.HasReturnableType(tokT, out var tk))
            {
                Debug.Assert(tk != null);
                return FnDef.Parse((TypeKind)tk, lexer);
            } else
            {
                throw new ParseException($"Expected Top Level Definition. Got {tokT}.");
            }
        }
    }
}

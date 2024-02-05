using System.Diagnostics;

namespace CMe.AST
{
    // Top Level Definition: Classes (Structs), Enums, Functions, etc.
    public abstract class TLDef : BaseNode
    {
        public static TLDef Parse(Lexer lexer)
        {
            var tokT = lexer.PeekNext().Type;
            if (Parser.HasReturnableType(out var tk, tokT))
            {
                Debug.Assert(tk != null);
                return FnDef.Parse(lexer);
            } else
            {
                // Only recognizing function definitions for the time being
                throw new ParseException($"Expected Top Level Definition. Got {tokT}.");
            }
        }
    }
}

using CMe.AST;
using System.Text;

namespace CMe
{
    public class Printer : IVisitor
    {
        private int indentations = 0;
        private readonly int indentationWidth = 2;
        private StringBuilder result = new();
        public string Result { get { return result.ToString(); } }

        public void Visit(FnDef fdef)
        {
            result.Append(Print($"[FnDef \"{fdef.Name}\"]\n"));
            IncIndent();
            fdef.AcceptBody(this);
            DecIndent();
        }

        public void Visit(FunctionCall fcall)
        {
            var print = new Printer();
            fcall.AcceptArgs(print);
            result.Append(Print($"[FnCall \"{fcall.Name}\", args: {print.Result}]\n"));
        }

        public void Visit(IntLiteral number)
        {
            result.Append(Print($"{number.Value}"));
        }

        public void Visit(AST.Program prog)
        {
            result.Append(Print("[Program]\n"));
            IncIndent();
        }

        public void Visit(StmtExpr stex)
        {            
            result.Append(Print("[StmtExpr]\n"));
            IncIndent();
            stex.AcceptExpr(this);
            DecIndent();
        }

        public void Visit(StmtRet stret)
        {
            var print = new Printer();
            stret.AcceptExpr(print);
            result.Append(Print($"[Return {print.Result}]\n"));
        }

        public void Visit(StringLiteral words)
        {
            result.Append(Print($"\"{EscapeString(words.Value)}\""));
        }

        private string Print(string s)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < indentations; i++) sb.Append(' ');
            return sb.Append(s).ToString();
        }

        private void IncIndent()
        {
            indentations += indentationWidth;
        }

        private void DecIndent()
        {
            indentations -= indentationWidth;
        }

        private string EscapeString(string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                sb.Append(c switch
                {
                    '\0' => "\\0",
                    '\'' => "\\'",
                    '"' => "\\\"",
                    '\\' => "\\\\",
                    '\r' => "\\r",
                    '\t' => "\\t",
                    '\n' => "\\n",
                    _ => c
                });
            }
            return sb.ToString();
        }
    }
}

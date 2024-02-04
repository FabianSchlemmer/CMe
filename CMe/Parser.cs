using CMe.AST;
using System;
using System.Runtime.Serialization;

namespace CMe
{
    public class Parser(Lexer lexer)
    {
        public AST.Program Parse()
        {
            var program = new AST.Program();
            while (lexer.PeekNext().Type != TokenType.EOF)
            {
                program.Add(TLDef.Parse(lexer));
            }
            return program;
        }

        public static bool HasReturnableType(TokenType tt, out TypeKind? tk)
        {
            tk = tt switch
            {
                TokenType.Int => TypeKind.Int,
                TokenType.Float => TypeKind.Float,
                TokenType.Double => TypeKind.Double,
                TokenType.Char => TypeKind.Char,
                TokenType.Void => TypeKind.Void,
                _ => null
            };
            return tk != null;
        }

        public static Token ExpectToken(Lexer lexer, params TokenType[] types)
        {
            var tok = lexer.NextTok();
            if (types.Contains(tok.Type)) return tok;
            throw new ParseException($"Expected {string.Join(", ", types)}. Got {tok.Type}.");
        }
    }
}

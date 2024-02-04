﻿using CMe.AST;

namespace CMe
{
    public interface IVisitor
    {
        void Visit(FnDef fdef);
        void Visit(FunctionCall fcall);
        void Visit(IntLiteral number);
        void Visit(AST.Program prog);
        void Visit(StmtExpr stex);
        void Visit(StmtRet stret);
        void Visit(StringLiteral words);
    }
}

using System;
using System.Collections.Generic;

public class OperationProcedur : Procedure
{
    Func<Element, Element, Element> EvalOp;

    public OperationProcedur(SchemeList procParams, Func<Element, Element, Element> func, SchemeEnvironment env) : base(procParams,null,env) 
    {
        EvalOp = func;
    }

    public override Element Eval(SchemeList paramsl, SchemeEnvironment env)
    {
        if (paramsl.Count == 0)
            return new NumberElement("0");

        Element result = Interpreter.Eval(paramsl.Next(), env);
        Element b;

        while((b = paramsl.Next()) != null)
        {
            b = Interpreter.Eval(b, env);
            result = EvalOp(result, b);
        }

        return result;
    }
}

using System;
using System.Collections.Generic;

public class OperationProcedur : Procedure
{
    Func<ExpressionElement, ExpressionElement,Element> EvalOp;

    public OperationProcedur(SchemeList procParams, Func<ExpressionElement, ExpressionElement, Element> func, SchemeEnvironment env) : base(procParams,null,env) 
    {
        EvalOp = func;
    }

    public new Element Eval(SchemeList paramsl, SchemeEnvironment env)
    {
        if (paramsl.Count - 1 < Params.Count)
            throw new ArgumentException("Not enough Arguments. Expecting " + Params.Count +
                " but received " + Params.Count + ".");

        Element result = null;
        
        Element a = Interpreter.Eval(new SchemeList(paramsl.NextIt()), env);
        Element b = Interpreter.Eval(new SchemeList(paramsl.NextIt()), env);

        if (paramsl.NextIt() != null)
            throw new ArgumentException("Too many arguments for Operation, expecting exactly 2.");

        result = EvalOp(a as ExpressionElement, b as ExpressionElement);

        return result;
    }
}

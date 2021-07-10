using System;
using System.Collections.Generic;

public class OperationProcedur : Procedure
{
    Func<ExpressionElement, ExpressionElement,Element> EvalOp;

    public OperationProcedur(SchemeList procParams, Func<ExpressionElement, ExpressionElement, Element> func, SchemeEnvironment env) : base(procParams,null,env) 
    {
        EvalOp = func;

        Eval = EvalProcOp;
    }

    public Element EvalProcOp(SchemeList paramsl, SchemeEnvironment env)
    {
        if (paramsl.Count < Params.Count)
            throw new ArgumentException("Not enough Arguments. Expecting 2, but received " + 
                Params.Count + ".");

        Element result = null;

        //localEnv der Operation ist global
        //da kann man nichts finden...
        //übergeben der Aufrufs env
        Element a = Interpreter.Eval(new SchemeList(paramsl.NextIt()), env);
        Element b = Interpreter.Eval(new SchemeList(paramsl.NextIt()), env);

        if (paramsl.NextIt() != null)
            throw new ArgumentException("Too many arguments for Operation, expecting 2, but received " +
                Params.Count+".");

        result = EvalOp(a as ExpressionElement, b as ExpressionElement);

        return result;
    }
}

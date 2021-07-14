using System;
using System.Collections.Generic;

public class OperationProcedur : Procedure
{
    Func<ExpressionElement, ExpressionElement,Element> EvalOp;

    public OperationProcedur(SchemeList procParams, Func<ExpressionElement, ExpressionElement, Element> func, SchemeEnvironment env) : base(procParams,null,env) 
    {
        EvalOp = func;
    }

    public override Element Eval(SchemeList paramsl, SchemeEnvironment env)
    {
        if (paramsl.Count < Params.Count)
            throw new ParameterMismatch("Not enough Arguments. Expecting 2, but received " + 
                Params.Count + ".");

        Element result = null;

        //localEnv der Operation ist global
        //da kann man nichts finden...
        //übergeben der Aufrufs env
        Element a = Interpreter.Eval(new SchemeList(paramsl.Next()), env);
        Element b = Interpreter.Eval(new SchemeList(paramsl.Next()), env);

        if (paramsl.Next() != null)
            throw new ParameterMismatch("Too many arguments for Operation, expecting 2, but received " +
                Params.Count+".");

        result = EvalOp(a as ExpressionElement, b as ExpressionElement);

        return result;
    }
}

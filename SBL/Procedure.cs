using System;
using System.Collections.Generic;

public class Procedure
{
    protected SchemeEnvironment localEnv;

    SchemeList procParams;
    protected SchemeList Params
    {
        get { return procParams; }
    }

    ExpressionElement body;

    public Procedure(SchemeList procParams, ExpressionElement body, SchemeEnvironment curEnv)
    {
        Eval = EvalProcBase;

        localEnv = new SchemeEnvironment(curEnv);
        this.procParams = procParams;
        this.body = body;
        for(int i = 0; i < procParams.Count;i++)
            localEnv.UpdateVar(procParams[i].Text, procParams[i]);
    }

    public delegate Element EvalDel(SchemeList paramsl, SchemeEnvironment env);
    public EvalDel Eval;
    Element EvalProcBase(SchemeList paramsl, SchemeEnvironment env)
    {
        if(paramsl.Count < procParams.Count)
            throw new ArgumentException("Not enough Arguments. Expecting " + procParams.Count + 
                " but received " + procParams.Count + ".");

        //Bind Params
        for (int i = 0; i < procParams.Count; i++)
            localEnv.UpdateVar(Params[i].Text, paramsl.NextIt());


        if(paramsl.NextIt() != null)
            throw new ArgumentException("Too many Arguments. Expecting "+ procParams.Count +
                " but received " + procParams.Count + ".");

        Element result = Interpreter.Eval(new SchemeList(body), localEnv);

        return result;
    }
}
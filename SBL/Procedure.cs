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
        localEnv = new SchemeEnvironment(curEnv);
        this.procParams = procParams;
        this.body = body;
        for(int i = 0; i < procParams.Count;i++)
            localEnv.UpdateVar(procParams[i].Text, procParams[i]);
    }

    public virtual Element Eval(SchemeList paramsl, SchemeEnvironment env)
    {
        if(paramsl.Count < procParams.Count)
            throw new ParameterMismatch("Not enough Arguments. Expecting " + procParams.Count + 
                " but received " + procParams.Count + ".");

        //Bind Params
        //for (int i = 0; i < procParams.Count; i++)
        //    localEnv.UpdateVar(Params[i].Text, paramsl.NextIt());
        for (int i = 0; i < procParams.Count; i++)
        {
            Element update = Interpreter.Eval((paramsl.Next() as ExpressionElement).ExprList, env);
            localEnv.UpdateVar(Params[i].Text, update);
        }
            

        if (paramsl.Next() != null)
            throw new ParameterMismatch("Too many Arguments. Expecting "+ procParams.Count +
                " but received " + procParams.Count + ".");

        Element result = Interpreter.Eval(new SchemeList(body), localEnv);

        return result;
    }
}
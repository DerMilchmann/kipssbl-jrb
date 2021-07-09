using System;
using System.Collections.Generic;

public class Procedure
{
    SchemeEnvironment localEnv;


    SchemeList procParams;
    public SchemeList Params
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

    public Element Eval(SchemeList paramsl, SchemeEnvironment env)
    {
        SchemeEnvironment paramBind = new SchemeEnvironment(localEnv);
        if(paramsl.Count - 1 < procParams.Count)
            throw new ArgumentException("Not enough Arguments. Expecting " + procParams.Count + 
                " but received " + procParams.Count + ".");

        //Bind Params
        for (int i = 0; i < procParams.Count; i++)
            paramBind.UpdateVar(paramsl[i].Text, paramsl.NextIt());


        if(paramsl.NextIt() != null)
            throw new ArgumentException("Too many Arguments. Expecting "+ procParams.Count +
                " but received " + procParams.Count + ".");

        localEnv.Parent = env;
        Element result = Interpreter.Eval(new SchemeList(body), paramBind);
        localEnv.Parent = null;

        return result;
    }
}
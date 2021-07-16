public class Procedure
{
    protected SchemeEnvironment localEnv;

    SchemeList procParams;
    protected SchemeList Params
    {
        get { return procParams; }
    }

    SchemeList body;

    public Procedure(SchemeList procParams, SchemeList body, SchemeEnvironment curEnv)
    {
        localEnv = new SchemeEnvironment(curEnv);
        this.procParams = procParams;
        this.body = body;
        for(int i = 0; i < procParams.Count;i++)
            localEnv.Update(procParams[i].Text, procParams[i]);
    }

    public virtual Element Eval(SchemeList paramsl, SchemeEnvironment env)
    {
        //Enough Arguments?
        if(paramsl.Count < procParams.Count)
            throw new ParameterMismatch("Not enough Arguments. Expecting " + procParams.Count + 
                " but received " + procParams.Count + ".");

        //Bind Params
        for (int i = 0; i < procParams.Count; i++)
        {
            Element update = Interpreter.Eval(paramsl.Next(), env);
            localEnv.Update(Params[i].Text, update);
        }
            
        //Too many Arguments?
        if (paramsl.Next() != null)
            throw new ParameterMismatch("Too many Arguments. Expecting "+ procParams.Count +
                " but received " + procParams.Count + ".");

        Element result = new EmptyElement();
        
        for(int i = 0; i < body.Count; i++)
        {
            result = Interpreter.Eval(body[i], localEnv);
        }      

        return result;
    }
}
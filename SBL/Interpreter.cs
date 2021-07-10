using System;
using System.Collections.Generic;

static class Interpreter
{
    static public Element Eval(SchemeList sl,  SchemeEnvironment env)
    {
        Element nextElement = null;
        while((nextElement = sl.NextIt()) != null)
        {
            switch (nextElement.GetType().Name)
            {
                case nameof(NumberElement):
                    return nextElement;
                case nameof(StringElement):
                    return nextElement;
                case nameof(BoolElement):
                    return nextElement;
                case nameof(OperatorElement):
                case nameof(TokenElement):
                    return EvaluateToken(nextElement as TokenElement, sl, env);
                case nameof(ExpressionElement):
                    return Eval((nextElement as ExpressionElement).exprList, env);
                case nameof(EmptyElement):
                    break;
            }
        }
        /*for(int i = 0; i < sl.Count();i++)
        {
            nextElement = sl[i];

            switch (nextElement.GetType().Name)
            {
                case nameof(NumberElement):
                    return nextElement;
                case nameof(StringElement):
                    return nextElement;
                case nameof(BoolElement):
                    return nextElement;
                case nameof(OperatorElement):
                    Element a = Eval(sl, env);
                    Element b = Eval(sl, env);
                    return Operate(nextElement, a, b);
                case nameof(TokenElement):
                    return EvaluateToken(nextElement as TokenElement, sl, env);
                case nameof(ExpressionElement):
                    return Eval((nextElement as ExpressionElement).exprList, env);
                case nameof(EmptyElement):
                    break;
            }
        }*/
        return new EmptyElement();
    }


    static Element EvaluateToken(TokenElement token, SchemeList sl, SchemeEnvironment env)
    {
        Element ret = new EmptyElement();

        switch(token.Text)
        {
            case "define":
                Element lookahead = sl.NextIt();
                if(lookahead is ExpressionElement)
                {
                    //Prozedur
                    SchemeList header = (lookahead as ExpressionElement).exprList;
                    ExpressionElement body = sl.NextIt() as ExpressionElement;

                    createProcedur(header, body, env);

                }else
                {
                    //Variable
                    string id = lookahead.Text;
                    Element Value = Eval(sl, env);
                    env.UpdateVar(id, Value);
                }
                return ret;

            //Variable Eval
            case string _ when env.ContainsVar(token.Text):
                return Eval(new SchemeList(env.GetVar(token.Text)), env);

            //Procedure Eval
            case string _ when env.ContainsProc(token.Text):
                Procedure proc = env.GetProc(token.Text);

                SchemeList parameters = new SchemeList();

                Element nextParam;
                while((nextParam = sl.NextIt()) != null)
                {
                    parameters.Append(nextParam);
                }
                
                return proc.Eval(parameters, env);

            default:
                    throw new Exception("Identifier not in Environment: " + token.Text);
        }
    }

    static void createProcedur(SchemeList header, ExpressionElement body, SchemeEnvironment env)
    {
        //Process Header
        Element id = header.NextIt();
        SchemeList parameters = new SchemeList();
        Element p;
        while ((p = header.NextIt()) != null)
            parameters.Append(p);

        Procedure neu = new Procedure(parameters, body, env);

        env.UpdateProc(id.Text, neu);
    }

    static Element Operate(Element op, Element a, Element b)
    {
        //op = + - / * = > < 
        Element ret = null;

        if (a is NumberElement && b is NumberElement)
        {
            double ad = (a as NumberElement).Value;
            double bd = (b as NumberElement).Value;

            switch (op.Text)
            {
                case ("+"):
                    ret = new NumberElement((ad + bd).ToString());
                    break;
                case ("-"):
                    ret = new NumberElement((ad - bd).ToString());
                    break;
                case ("*"):
                    ret = new NumberElement((ad * bd).ToString());
                    break;
                case ("/"):
                    ret = new NumberElement((ad / bd).ToString());
                    break;
                case ("="):
                    ret = new BoolElement(Equals(ad, bd).ToString());
                    break;
                case ("<"):
                    ret = new BoolElement((ad < bd).ToString());
                    break;
                case (">"):
                    ret = new BoolElement((ad > bd).ToString());
                    break;
            }
        }
        else throw new InvalidOperationException("Expecting two NumberElements, got "
                                    + a.GetType().Name + " and "
                                    + b.GetType().Name + " instead.");
        return ret;
    }
}

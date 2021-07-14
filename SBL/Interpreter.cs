using System;
using System.Collections.Generic;

static class Interpreter
{
    static public Element Eval(SchemeList sl,  SchemeEnvironment env)
    {
        Element nextElement = null;
        while((nextElement = sl.Next()) != null)
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
                    return Eval((nextElement as ExpressionElement).ExprList, env);
                case nameof(EmptyElement):
                    break;
            }
        }
        return new EmptyElement();
    }


    static Element EvaluateToken(TokenElement token, SchemeList sl, SchemeEnvironment env)
    {
        Element ret = new EmptyElement();

        switch(token.Text)
        {
            case "define":
                Element lookahead = sl.Next();
                if (lookahead == null)
                    throw new BadSyntaxException("Bad Syntax in define");
                if(lookahead is TokenElement)
                {
                    //Variable
                    string id = lookahead.Text;
                    Element Value = Eval(sl, env);
                    env.UpdateVar(id, Value);
                }
                else
                {
                    //Prozedur
                    SchemeList header = (lookahead as ExpressionElement).ExprList;
                    ExpressionElement body = sl.Next() as ExpressionElement;

                    createProcedur(header, body, env);
                }
                return ret;

            case "lambda":
                {
                    /* lambda
                     * (arg-id)
                     * body
                     */
                    ExpressionElement arguments = sl.Next() as ExpressionElement;
                    ExpressionElement body = sl.Next() as ExpressionElement;
                    if (sl.Next() != null ||
                        arguments == null ||
                        body == null)
                        throw new BadSyntaxException("Bad Syntax in Lambda");

                    //should return a procedure pointer
                    //no idea how to do that with my current implementation
                    //I'd have to re-design alot to accomodate for procedure pointers

                    Procedure lambda = new Procedure(arguments.ExprList, body, env);

                    return ret;
                }


            case "if":
                ExpressionElement testExpr = sl.Next() as ExpressionElement;
                ExpressionElement thenExpr = sl.Next() as ExpressionElement;
                ExpressionElement elseExpr = sl.Next() as ExpressionElement;
                if (sl.Next() != null ||
                    testExpr == null ||
                    thenExpr == null ||
                    elseExpr == null)
                    throw new BadSyntaxException("Bad Syntax in IF");
              
                if (Eval(testExpr.ExprList, env).Text != "#f")//ANY value other than #f --> then-expr               
                    ret = Eval(thenExpr.ExprList, env);
                else
                    ret = Eval(elseExpr.ExprList, env);

                return ret;

            case "cond":

                Element condClause = sl.Next();
                if (condClause == null)
                    return ret; // #void



                return ret;

            //Variable Eval
            case string _ when env.ContainsVar(token.Text):
                return Eval(new SchemeList(env.GetVar(token.Text)), env);

            //Procedure Eval
            case string _ when env.ContainsProc(token.Text):
                Procedure proc = env.GetProc(token.Text);

                SchemeList parameters = new SchemeList();

                Element nextParam;
                while((nextParam = sl.Next()) != null)
                {
                    parameters.Append(nextParam);
                }
                
                return proc.Eval(parameters, env);

            default:
                    throw new UndefinedIdentifier("Identifier not in Environment: " + token.Text);
        }
    }

    static void createProcedur(SchemeList header, ExpressionElement body, SchemeEnvironment env)
    {
        //Process Header
        Element id = header.Next();
        SchemeList parameters = new SchemeList();
        Element p;
        while ((p = header.Next()) != null)
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

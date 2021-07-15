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
                case nameof(ProcedureElement):
                    return nextElement;
                case nameof(OperatorElement):
                case nameof(TokenElement):
                    return EvaluateToken(nextElement as TokenElement, sl, env);
                case nameof(ExpressionElement):
                    return Call((nextElement as ExpressionElement).ExprList, env);
                case nameof(EmptyElement):
                    break;
            }
        }
        return new EmptyElement();
    }

    static public Element Eval(Element e, SchemeEnvironment env) 
    { 
        return Eval(new SchemeList(e), env); 
    }

    static Element Call(SchemeList sl, SchemeEnvironment env)
    {
        Element ret = new EmptyElement();

        //Eval first element
        //if first is keyword-Token, evaluateToken will return with Result
        //if result is Procedure, call it
        //rest of objects in list are arguments for call
        Element first = sl.Next();

        if (first is TokenElement)
            if (tokenKeywords.Contains(first.Text))
                return EvaluateToken(first as TokenElement, sl, env);
            else
                first = EvaluateToken(first as TokenElement, sl, env);

        if (first is ExpressionElement)
            first = Eval((first as ExpressionElement).ExprList, env);

        if (!(first is ProcedureElement))
            throw new NotAProcedureException("Expected a procedure that can be applied to arguments.");

        Procedure proc = (first as ProcedureElement).proc;

        SchemeList parameters = new SchemeList();

        Element nextParam;
        while ((nextParam = sl.Next()) != null)
        {
            parameters.Append(nextParam);
        }

        ret = proc.Eval(parameters, env);

        return ret;
    }

    static List<string> tokenKeywords = new List<string>()
    {
        "define",
        "lambda",
        "if",
        "cond",
        "let"
    };
    static Element EvaluateToken(TokenElement token, SchemeList sl, SchemeEnvironment env)
    {
        Element ret = new EmptyElement();

        switch(token.Text)
        {
            case "define":
                Element lookahead = sl.Next();
                if (lookahead == null)
                    throw new BadSyntaxException("Bad Syntax in define.");
                if (lookahead is TokenElement)
                {
                    //Variable
                    string id = lookahead.Text;
                    Element Value = Eval(sl, env);
                    env.Update(id, Value);
                }
                else if (lookahead is ExpressionElement)
                {
                    //Prozedur
                    SchemeList header = (lookahead as ExpressionElement).ExprList;
                    Element body = sl.Next();

                    Procedure neu = createProcedur(header, body, env);

                    env.Update(header[0].Text, new ProcedureElement(neu));
                }
                else
                    throw new BadSyntaxException("Bad Syntax in define.");

                return new EmptyElement();

            case "lambda":
                {
                    /* lambda
                     * (arg-id)
                     * body
                     */
                    ExpressionElement arguments = sl.Next() as ExpressionElement;
                    Element body = sl.Next();
                    if (sl.Next() != null ||
                        arguments == null ||
                        body == null)
                        throw new BadSyntaxException("Bad Syntax in Lambda.");

                    //should return a procedure pointer
                    //no idea how to do that with my current implementation
                    //I'd have to re-design alot to accomodate for procedure pointers

                    //Jesus gib mir Kraft

                    Procedure lambda = new Procedure(arguments.ExprList, body, env);

                    ret = new ProcedureElement(lambda);

                    return ret;
                }


            case "if":
                {
                    Element testExpr = sl.Next();
                    Element thenExpr = sl.Next();
                    Element elseExpr = sl.Next();
                    if (sl.Next() != null ||
                        testExpr == null ||
                        thenExpr == null ||
                        elseExpr == null)
                        throw new BadSyntaxException("Bad Syntax in IF.");

                    if (Eval(testExpr, env).Text != "#f")//ANY value other than #f --> then-expr               
                        ret = Eval(thenExpr, env);
                    else
                        ret = Eval(elseExpr, env);

                    return ret;
                }

            case "cond":
                {
                    Element condClause = sl.Next();
                    if (condClause == null)
                        return new EmptyElement(); // #void

                    //beliebig oft:
                    //test-expr then-body
                    //test-expr then-body
                    //...
                    Element testExpr;
                    Element thenBody;
                    do
                    {
                        testExpr = (condClause as ExpressionElement).ExprList[0];
                        thenBody = (condClause as ExpressionElement).ExprList[1];

                        if (Eval(testExpr, env).Text == "#t")
                            return Eval(thenBody, env);
                    } while ((condClause = sl.Next()) != null);


                    //else-clause must be last
                    return ret;
                }

            case "let":
                {
                    //(assignments)
                        //beliebig oft:
                        //(id val-expr)
                        //...
                    //body
                    //return body result
                    SchemeList assignments = (sl.Next() as ExpressionElement).ExprList;
                    Element body = sl.Next();

                    SchemeEnvironment letEnv = new SchemeEnvironment(env);

                    ExpressionElement nextAssignment;
                    string id;
                    Element value;
                    while((nextAssignment= assignments.Next() as ExpressionElement) != null)
                    {
                        id = nextAssignment.ExprList[0].Text;
                        value = Eval(nextAssignment.ExprList[1], env);
                        letEnv.Update(id, value);
                    }

                    return Eval(body, letEnv);
                }

            //Search Environment for definition
            case string _ when env.ContainsIdentifier(token.Text):
                return env.Get(token.Text);              

            default:
                    throw new UndefinedIdentifier("Identifier not in Environment: " + token.Text);
        }
    }

    static Procedure createProcedur(SchemeList header, Element body, SchemeEnvironment env)
    {
        //Process Header
        Element id = header.Next();
        SchemeList parameters = new SchemeList();
        Element p;
        while ((p = header.Next()) != null)
            parameters.Append(p);
    
        return new Procedure(parameters, body, env); ;
    }
}

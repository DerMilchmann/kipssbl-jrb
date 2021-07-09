using System;

class InterpreterTree
{
    SchemeEnvironment currentEnv;
    public Node Eval(Node sl, SchemeEnvironment env = null)
    {
        currentEnv = env;
        Node nextNode = sl;
        do
        {
            switch (nextNode.GetType().Name)
            {
                case nameof(NumberNode):
                    return nextNode;
                case nameof(StringNode):
                    return nextNode;
                case nameof(BoolNode):
                    return nextNode;
                case nameof(OperatorNode):
                    Node a = Eval(sl.Next());
                    Node b = Eval(sl.Next());
                    return Operate(nextNode, a, b);
                case nameof(TokenNode):
                    return EvaluateToken(nextNode as TokenNode);
                case nameof(ExpressionNode):
                    break;
                case nameof(EmptyNode):
                    break;
            }
        } while ((nextNode = sl.Next()) != null);
        return new EmptyNode();
    }

    Node Operate(Node op, Node a, Node b)
    {
        //op = + - / * = > < 
        Node ret = null;

        if (a is NumberNode && b is NumberNode)
        {
            double ad = (a as NumberNode).Value;
            double bd = (b as NumberNode).Value;

            switch (op.Text)
            {
                case ("+"):
                    ret = new NumberNode((ad + bd).ToString());
                    break;
                case ("-"):
                    ret = new NumberNode((ad - bd).ToString());
                    break;
                case ("*"):
                    ret = new NumberNode((ad * bd).ToString());
                    break;
                case ("/"):
                    ret = new NumberNode((ad / bd).ToString());
                    break;
                case ("="):
                    ret = new BoolNode(Equals(ad, bd).ToString());
                    break;
                case ("<"):
                    ret = new BoolNode((ad < bd).ToString());
                    break;
                case (">"):
                    ret = new BoolNode((ad > bd).ToString());
                    break;
            }
        }
        else throw new InvalidOperationException("Expecting two NumberNodes, got "
                                    + a.GetType().Name + " and "
                                    + b.GetType().Name + " instead.");
        return ret;
    }

    Node EvaluateToken(TokenNode token)
    {
        Node ret = new EmptyNode();

        /*Mögliche Tokens
         * 
         * define id expr
         * define (head args[]) (body)
         * if (test-expr) (then-expr) (opt else-expr)
         * cond
         * and expr1 expr2 ... , gibt #t oder den ersten anderen Wert
         * display
         * Variable?
         */
        switch(token.Text)
        {
            case "define":
                //(define x 10)
                //(define (summe x y) (+ x y))
                Node lookahead = token.Next();
                if (lookahead is ExpressionNode)
                {
                    //Prozedur mit Param?
                }
                else
                {
                    //Variablen define
                    //naechster Token ist id
                    string id = lookahead.Text;

                    //ExpressionToken
                    Node Value = Eval(token.Next());

                }
                return ret;

            //TODO

            //Token = Variable?
            default:
                /*if (currentEnv.Lookup(token.Text))
                    return Eval(currentEnv.Get(token.Text));
                else
                    currentEnv.Add(token.Text, token.Next());*/
                break;

        }


        return ret;
    }
}

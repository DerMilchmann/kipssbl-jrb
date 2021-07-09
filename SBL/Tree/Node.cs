using System.Collections.Generic;

public class Node
{
    string text;
    public string Text
    {
        get { return text; }
        set { text = value; }
    }
    int it = 0;

    public List<Node> children;
    public bool isLeaf;

    public Node(string text)
    {
        children = null;
        this.text = text;
    }
    public Node()
    {
        text = null;
        children = new List<Node>();
    }
    public void Append(Node n)
    {
        children.Add(n);
    }
    public Node Next()
    {
        if (children == null || it == children.Count)
            return null;
        else
        {
            Node ret = children[it];
            it++;
            return ret;
        }
    }
}
class NumberNode : Node
{
    public double Value
    {
        get { return double.Parse(Text); }
    }
    public NumberNode(string text) : base(text) {
        isLeaf = true;
    }
}

class BoolNode : Node
{
    public bool Value
    {
        get { return Text == "#t"; }
    }
    public BoolNode(string text) : base(text)
    {
        //convert to Scheme Bool format       
        if (!text.StartsWith("#"))
            if (text == bool.TrueString)
                Text = "#t";
            else
                Text = "#f";
    }
    public BoolNode(bool value) : base()
    {
        Text = value ? "#t" : "#f";
    }
}

class StringNode : Node
{
    public StringNode(string text) : base(text) { }
}
class OperatorNode : Node
{
    public OperatorNode(string text) : base(text) { }
}

class TokenNode : Node
{
    public TokenNode(string text) : base(text) { }
}

class EmptyNode : Node
{
    public EmptyNode() : base(null) { }
}

class ExpressionNode : Node
{
    public ExpressionNode() : base() { }
}
using System;

public class Element
{
    string text;

    public Element(string text)
    {
        this.text = text;
    }
    public Element() 
    { 
        text = null; 
    }

    public string Text { get => text;
        set => this.text = value; }
}

public class ExpressionElement : Element
{
    protected SchemeList exprList;
    public SchemeList ExprList
    { get { return exprList.Copy(); } }
    public ExpressionElement(SchemeList list) : base()
    {
        exprList = list;
    }

    public ExpressionElement(string text) : base(text)
    {
        exprList = new SchemeList();
    }
}

class NumberElement : ExpressionElement
{
    public double Value
    {
        get { return double.Parse(Text); }
    }
    public NumberElement(string text) : base(text) {
        exprList.Append(this);
    }
}

class BoolElement : ExpressionElement
{
    public bool Value
    {
        get { return Text == "#t"; }
    }
    public BoolElement(string text) :base (text)
    {
        //convert to Scheme Bool format       
        if (!text.StartsWith("#"))
            if(text == bool.TrueString)
                Text = "#t";
            else
                Text = "#f";
        exprList.Append(this);
    }
}

class StringElement : ExpressionElement
{
    public StringElement(string text) : base(text){
        exprList.Append(this);
    }
}

class TokenElement : ExpressionElement
{
    public TokenElement(string text) : base(text) {
        exprList.Append(this);
    }
}

class OperatorElement : TokenElement
{
    public OperatorElement(string text) : base(text) { }
}

class EmptyElement : Element
{
    public EmptyElement() : base() { }
}
public class Token
{
    private int type;
    private string text;


    public Token(int type, string text) : base()
    {
        this.text = text;
        this.type = type;
    }


    public string getText()
    {
        return text;
    }

    public int getType()
    {
        return type;
    }

    public override string ToString()
    {
        return "<'" + text + "', " + SchemeLexer.tokenNames[type] + ">";
    }
}

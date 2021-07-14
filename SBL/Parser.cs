using System;

public class Parser
{
    private Lexer input;
    protected Token lookahead;

    public Parser(Lexer input)
    {
        this.input = input;
        Consume();
    }

    public void Consume()
    {
        try
        {
            lookahead = input.nextToken();
        }catch(Exception e)
        {
            Console.Write(e.StackTrace);
        }
    }

    public void Match(int type)
    {
        if (lookahead.getType() == type)
        {
            Consume();
        }
        else throw new ParserException("Expecting " + input.getTokenName(type)
                                        + "; found " + lookahead);
    }
}
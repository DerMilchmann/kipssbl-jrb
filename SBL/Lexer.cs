using System;
using System.IO;

public abstract class Lexer
{
    private StreamReader input;
    protected int p;
    protected char c;
    protected bool eof;


    public Lexer()
    {
        p = 0;
        eof = false;
    }

    public Lexer(Stream input) : this()
    {
        this.input = new StreamReader(input);
        try
        {
            consume();
        }
        catch (IOException e)
        {
            Console.Write(e.StackTrace);
        }
    }

    protected bool isWhitespace()
    {
        return (c == '\n') || (c == '\r') || (c == '\t') || (c == ' ');
    }

    public void consume()
    {
        int i = input.Read();
		if (i == -1) {
			eof = true;
		}
        c = (char) i;
        p++;
	}
	
	public void consumeWS()
    {
        do
        {
            consume();
        } while (isWhitespace());
    }



public abstract Token nextToken();

public abstract String getTokenName(int type);

}

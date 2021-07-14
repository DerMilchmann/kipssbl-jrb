using System;
using System.IO;
using System.Text;

public class SchemeLexer : Lexer
{
    public const int EOF = 1;
    public const int ELEMENT = 2;
    public const int LPARENTHESIS = 3;
    public const int RPARENTHESIS = 4;
    public const int QUOTE = 5;
    public const int NUMBER = 6;
    public const int BOOLEAN = 7;
    public const int STRING = 8;
    public const int OPERATOR = 9;

    public static string[] tokenNames = { "n/a", "<EOF>", "ELEMENT", "LPARENTHESIS", "RPARENTHESIS", "QUOTE", "NUMBER",
            "BOOLEAN", "STRING", "OPERATOR" };

    public SchemeLexer(Stream input) : base(input)
    { 
    }

    private bool isStartLetter()
    {
        return ((c >= 'a') && (c <= 'z')) || ((c >= 'A') && (c <= 'Z'));
    }

    private bool isLetter()
    {
        return isStartLetter() || (c == '!') || (c == '?') || (c == '-');
    }

    private bool isOperator()
    {
        return (c == '+') || (c == '-') || (c == '/') || (c == '*') || (c == '=') || (c == '>') || (c == '<');
    }

    private bool isSeperator()
    {
        return (c == '(') || (c == ')') || (c == ' ');
    }

    private bool isBoolean()
    {
        return (c == '#');
    }

    private bool isString()
    {
        return (c == '\"');
    }


    protected Token nextVariable() 
    {
        StringBuilder sb = new StringBuilder();

        try { 
            do {
                sb.Append(c);
                consume();
            } while (!eof && isLetter() && !isSeperator());          
        }catch(Exception e)
        {
            Console.Write(e.StackTrace);
        }

        return new Token(ELEMENT, sb.ToString());
    }

    protected Token nextNumber()
    {
        StringBuilder sb = new StringBuilder();
        try {
            do {
                sb.Append(c);
                consume();
            } while (!eof && char.IsDigit(c));
        }catch(Exception e)
        {
            Console.Write(e.StackTrace);
        }

        return new Token(NUMBER, sb.ToString());
    }

	protected Token nextOperator()
{
        StringBuilder sb = new StringBuilder();
        try {
		    do {
			    sb.Append(c);
			    consume();
		    } while (!eof && isOperator());
        }
        catch (Exception e)
        {
            Console.Write(e.StackTrace);
        }

        return new Token(OPERATOR, sb.ToString());
    }

	protected Token nextBoolean()
{
        StringBuilder sb = new StringBuilder();
        try {
            sb.Append(c);
		    consume();
            sb.Append(c);
		    consume();
        }
        catch (Exception e)
        {
            Console.Write(e.StackTrace);
        }

        return new Token(BOOLEAN, sb.ToString());
    }
    
	protected Token nextString()
{
        StringBuilder sb = new StringBuilder();
        try { 
            consume();
		    while (!isString()) {
			    sb.Append(c);
			    consume();
		    }
		    consume();
        }
        catch (Exception e)
        {
            Console.Write(e.StackTrace);
        }

        return new Token(STRING, sb.ToString());
    }

	public override Token nextToken()
    {
        try { 
		    while (!eof) {
            switch (c)
            {
                case ' ':
                case '\n':
                case '\t':
                case '\r':
                    consumeWS();
                    continue;
                case '(':
                    consume();
                    return new Token(LPARENTHESIS, "(");
                case ')':
                    consume();
                    return new Token(RPARENTHESIS, ")");
                case '\'':
                    consume();
                    return new Token(QUOTE, "'");
                default:
                    if (isStartLetter())
                    {
                        return nextVariable();
                    }
                    if (char.IsDigit(c))
                    {
                        return nextNumber();
                    }
                    if (isOperator())
                    {
                        return nextOperator();
                    }
                    if (isBoolean())
                    {
                        return nextBoolean();
                    }
                    if (isString())
                    {
                        return nextString();
                    }
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.Write(e.StackTrace);
        }

        return new Token(EOF, "EOF");
    }

	public override string getTokenName(int type)
    {
        return tokenNames[type];
    }

}
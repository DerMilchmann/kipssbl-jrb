using System;
using System.Collections.Generic;

public class SchemeParser : Parser
{
    private List<Entry> progast;
    private Entry ast;
    private Entry currententry;

    public SchemeParser(Lexer input) : base(input)
    {
        progast = new List<Entry>();
        ast = null;
        currententry = null;
    }

    public List<Entry> Parse()
    {
        while (lookahead.getType() != SchemeLexer.EOF)
        {
            form();
            progast.Add(ast);
            ast = null;
            currententry = null;
        }
        return progast;
    }


    public new void Match(int type)
    {
        if ((type != SchemeLexer.LPARENTHESIS) && (type != SchemeLexer.RPARENTHESIS))
        {
            Entry neu = new Entry(lookahead);
            if (currententry != null)
            {
                currententry.addChildren(neu);
            }
            else
            {
                ast = neu;
            }
        }
        base.Match(type);
    }

    public void element()
    {
        switch (lookahead.getType())
        {
            case SchemeLexer.LPARENTHESIS:
                liste();
                break;
            case SchemeLexer.ELEMENT:
                Match(SchemeLexer.ELEMENT);
                break;
            case SchemeLexer.NUMBER:
                Match(SchemeLexer.NUMBER);
                break;
            case SchemeLexer.BOOLEAN:
                Match(SchemeLexer.BOOLEAN);
                break;
            case SchemeLexer.STRING:
                Match(SchemeLexer.STRING);
                break;
            case SchemeLexer.OPERATOR:
                Match(SchemeLexer.OPERATOR);
                break;
            case SchemeLexer.QUOTE:
                quoted();
                break;
            default:
                throw new ParserException("No valid element; reading " + lookahead);

        }
    }

    public void elements()
    {
        while (lookahead.getType() != SchemeLexer.RPARENTHESIS)
        {
            element();
        }
    }

    public void quoted()
    {
        Entry save = currententry;

        currententry = new Entry(new Token(SchemeLexer.LPARENTHESIS, "("));
        Match(SchemeLexer.QUOTE);
        switch (lookahead.getType())
        {
            case SchemeLexer.LPARENTHESIS: liste(); break;
            case SchemeLexer.ELEMENT: element(); break;
        }
        if (save != null)
        {
            save.addChildren(currententry);
            currententry = save;
        }
        else
        {
            ast = currententry;
        }
    }

    public void liste()
    {

        Entry save = currententry;

        currententry = new Entry(lookahead);
        Match(SchemeLexer.LPARENTHESIS);
        elements();
        Match(SchemeLexer.RPARENTHESIS);
        if (save != null)
        {
            save.addChildren(currententry);
            currententry = save;
        }
        else
        {
            ast = currententry;
        }
    }

    public void form()
    {
        switch (lookahead.getType())
        {
            case SchemeLexer.LPARENTHESIS:
                liste();
                break;
            case SchemeLexer.ELEMENT:
                element();
                break;
            case SchemeLexer.NUMBER:
                element();
                break;
            case SchemeLexer.EOF:
                break;
            default:
                throw new ParserException("Invalid form; reading " + lookahead);
        }
    }
}

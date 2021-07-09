class ASTRewriter
{
    private SchemeList sl;
    private SchemeList current;
    private bool quoted;

    public ASTRewriter()
    {
        sl = null;
        current = null;
        quoted = false;
    }

    public SchemeList toSchemeList(Entry ast)
    {
        if (ast != null)
        {
            Entry e = null;
            Element neu = null;
            SchemeList save = current;
            if (ast.isLeaf())
            {
                if ((ast.getToken().getType() == SchemeLexer.LPARENTHESIS) && quoted)
                {
                    neu = new EmptyElement();
                    quoted = false;
                }
                else
                {
                    switch (ast.getToken().getType())
                    {
                        case SchemeLexer.NUMBER:
                            neu = new NumberElement(ast.getToken().getText());
                            break;
                        case SchemeLexer.BOOLEAN:
                            neu = new BoolElement(ast.getToken().getText());
                            break;
                        case SchemeLexer.STRING:
                            neu = new StringElement(ast.getToken().getText());
                            break;
                        case SchemeLexer.ELEMENT:
                            neu = new TokenElement(ast.getToken().getText());
                            break;
                        case SchemeLexer.OPERATOR:
                            neu = new OperatorElement(ast.getToken().getText());
                            break;
                        case SchemeLexer.QUOTE:
                            neu = new TokenElement("quote");
                            quoted = true;
                            break;
                    }
                }
                if (current == null)
                {
                    return new SchemeList(neu);
                }
                else
                {
                    if (neu != null)
                    {
                        current.Append(neu);
                    }
                }
            }
            else
            {
                current = new SchemeList();
                while ((e = ast.next()) != null)
                {
                    toSchemeList(e);
                }
                if (save != null)
                {
                    save.Append(current);
                    current = save;
                }
                else
                {
                    sl = current;
                }

            }
        }
        return sl;
    }
}
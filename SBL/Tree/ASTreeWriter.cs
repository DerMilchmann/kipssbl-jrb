class ASTreeWriter
{
    private Node sl;
    private Node current;

    public ASTreeWriter()
    {
        sl = null;
        current = null;
    }

    public Node toTree(Entry ast)
    {
        if (ast != null)
        {
            Entry e = null;
            Node neu = null;
            Node save = current;

            if(ast.isLeaf())
            {
                switch (ast.getToken().getType())
                {
                    case SchemeLexer.NUMBER:
                        neu = new NumberNode(ast.getToken().getText());
                        break;
                    case SchemeLexer.BOOLEAN:
                        neu = new BoolNode(ast.getToken().getText());
                        break;
                    case SchemeLexer.STRING:
                        neu = new StringNode(ast.getToken().getText());
                        break;
                    case SchemeLexer.ELEMENT:
                        neu = new TokenNode(ast.getToken().getText());
                        break;
                    case SchemeLexer.OPERATOR:
                        neu = new OperatorNode(ast.getToken().getText());
                        break;
                }

                if (current == null)
                    return neu;
                else
                {
                    if (neu != null)
                        current.Append(neu);
                }
            }else
            {
                current = new ExpressionNode();
                while((e = ast.next()) != null)
                {
                    toTree(e);
                }
                if (save != null)
                {
                    save.Append(current);
                    current = save;
                }
                else
                    sl = current;
            }
        }
        return sl;
    }
}
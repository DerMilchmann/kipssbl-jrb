using System.Collections.Generic;

public class Entry
{
    private Token token;
    private List<Entry> children;
    private bool leaf;
    private IEnumerator<Entry> it;


    public Entry()
    {
        children = null;
        leaf = true;
        it = null;
    }


    public Entry(Token token) : this()
    {
        this.token = token;
    }

    public bool isLeaf()
    {
        return leaf;
    }

    public Token getToken()
    {
        return token;
    }

    public void addChildren(Entry child)
    {
        if (children == null)
        {
            children = new List<Entry>();
            leaf = false;
        }
        children.Add(child);
    }

    public Entry next()
    {
        if (children == null)
        {
            return null;
        }
        if (it == null)
        {
            it = children.GetEnumerator();
        }
        if (it.MoveNext())
        {
            return it.Current;
        }
        return null;
    }

}
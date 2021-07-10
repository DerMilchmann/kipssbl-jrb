using System;
using System.Collections.Generic;

public class SchemeList 
{
    protected List<Element> list;
    int it = 0;

    public SchemeList()
    {
        list = new List<Element>();
    }
    public SchemeList(Element e) : base()
    {
        list = new List<Element>();
        list.Add(e);
    }
    public void Append(Element e)
    {
        list.Add(e);
    }

    public void Append(SchemeList sl)
    {
        list.Add(new ExpressionElement(sl));
    }

    public Element this[int i] => list[i];

    Element Next()
    {
        if (list.Count == 0)
            return null;
        else
        {
            Element ret = list[0];
            list.RemoveAt(0);
            return ret;
        }
    }

    public Element NextIt()
    {
        if (list.Count == it)
        {
            it = 0;
            return null;
        }          
        else
        {
            Element ret = list[it];
            it++;
            return ret;
        }         
    }

    public SchemeList Copy()
    {
        SchemeList neu = new SchemeList();
        neu.list = list;
        return neu;
    }
    public int Count
    {
        get { return list.Count; }
    }
}
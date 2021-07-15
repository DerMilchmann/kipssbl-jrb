using System;
using System.Collections;
using System.Collections.Generic;

public class SchemeList
{
    protected List<Element> list;
    int position = 0;

    public SchemeList()
    {
        list = new List<Element>();
    }
    public SchemeList(Element e)
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

    public Element Next()
    {
        if (list.Count == position)
        {
            position = 0;
            return null;
        }          
        else
        {
            Element ret = list[position];
            position++;
            return ret;
        }         
    }
    public int Count
    {
        get { return list.Count; }
    }
    public void Reset()
    {
        position = 0;
    }
    public SchemeList Copy()
    {
        SchemeList ret = new SchemeList();
        ret.list = list;
        return ret;
    }
}
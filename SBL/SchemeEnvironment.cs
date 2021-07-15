using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SchemeEnvironment
{
    Dictionary<string, Element> entries;

    public SchemeEnvironment Parent { get; set; }

    public SchemeEnvironment(SchemeEnvironment parent)
    {
        entries = new Dictionary<string, Element>();
        this.Parent = parent;
    }

    public bool ContainsIdentifier(string id)
    {
        if (entries.ContainsKey(id))
            return true;
        else if (Parent != null)
            return Parent.ContainsIdentifier(id);
        else return false;
    }

    public Element Get(string id)
    {
        if (ContainsIdentifier(id))
        {
            if (entries.ContainsKey(id))
                return entries[id];
            else
                return Parent.Get(id);
        }
        else return null;
    }


    public void Update(string id, Element e) => entries[id] = e;
}
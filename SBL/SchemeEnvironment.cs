using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SchemeEnvironment
{
    Dictionary<string, Procedure> procedures;

    Dictionary<string, Element> variables;

    public SchemeEnvironment Parent { get; set; }

    public SchemeEnvironment(SchemeEnvironment parent)
    {
        procedures = new Dictionary<string, Procedure>();
        variables = new Dictionary<string, Element>();
        this.Parent = parent;
    }

    public bool ContainsVar(string id)
    {
        if (variables.ContainsKey(id))
            return true;
        else if (Parent != null)
            return Parent.ContainsVar(id);
        else return false;
    }

    public bool ContainsProc(string id)
    {
        if (procedures.ContainsKey(id))
            return true;
        else if (Parent != null)
            return Parent.ContainsProc(id);
        else return false;
    }

    public Element GetVar(string id)
    {
        if (ContainsVar(id))
        {
            if (variables.ContainsKey(id))
                return variables[id];
            else
                return Parent.GetVar(id);
        }
        else return null;
    }

    public Procedure GetProc(string id)
    {
        if (ContainsProc(id))
        {
            if (procedures.ContainsKey(id))
                return procedures[id];
            else
                return Parent.GetProc(id);
        }
        else return null;
    }

    ///<summary>Adds or Updates Variable</summary>
    public void UpdateVar(string id, Element e) => variables[id] = e;

    ///<summary>Adds or Updates Procedure</summary>
    public void UpdateProc(string id, Procedure clo) => procedures[id] = clo;
    
    private SchemeEnvironment DeepCopy()
    {
        using(var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, this);
            ms.Position = 0;

            return (SchemeEnvironment)formatter.Deserialize(ms);
        }
    }
}
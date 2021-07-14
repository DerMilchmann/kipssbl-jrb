using System;

public class SchemeException : Exception
{
    protected string type;
    public SchemeException(string mess) : base(mess) { type = ""; }
    public void Display()
    {
        Console.WriteLine(type);
        Console.WriteLine("\t" + Message);
    }
}

class UndefinedIdentifier : SchemeException
{
    public UndefinedIdentifier(string text) : base(text) 
    {
        type = "UndefinedIdentifier";
    }
}

class ParameterMismatch : SchemeException
{
    public ParameterMismatch(string text) : base(text)
    {
        type = "ParameterMismatch";
    }
}

class ParserException : SchemeException
{
    public ParserException(string text) : base(text)
    {
        type = "ParserException";
    }
}
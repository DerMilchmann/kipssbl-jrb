﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Lambda
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists("../../SchemeCode.rkt"))
                throw new IOException("File SchemeCode.rkt does not exist in this directory.\n");

            SchemeEnvironment global = new SchemeEnvironment(null);
            InitGlobalEnv(global);
            env = ReadFile();

            //Read single lines with operations
            StreamReader s = new StreamReader(Console.OpenStandardInput());
            while (true)
            {
                string input = s.ReadLine();              

                if (input == "reload")
                    env = ReadFile();
                else
                {
                    //Convert to Stream
                    var stream = new MemoryStream();
                    var writer = new StreamWriter(stream);
                    writer.Write(input);
                    writer.Flush();
                    stream.Position = 0;

                    //Start with stream
                    SchemeLexer sl = new SchemeLexer(stream);
                    SchemeParser sp = new SchemeParser(sl);
                    List<Entry> entrys = sp.Parse();
                    foreach (Entry e in entrys)
                    {
                        ASTRewriter astr = new ASTRewriter();
                        SchemeList exp = astr.toSchemeList(e);
                        Element result = Interpreter.Eval(exp, env);
                        if ((result.Text != null))
                        {
                            Console.WriteLine(result.Text);
                        }
                    }
                }
            }
        }


        static SchemeEnvironment ReadFile()
        {
            using (FileStream stream = File.OpenRead("../../SchemeCode.rkt"))
            {
                stream.Position = 0;

                SchemeLexer sl = new SchemeLexer(stream);
                SchemeParser sp = new SchemeParser(sl);
                List<Entry> entrys = sp.Parse();
                var env = new SchemeEnvironment(null);

                foreach (Entry e in entrys)
                {

                    ASTRewriter astr = new ASTRewriter();
                    SchemeList exp = astr.toSchemeList(e);
                    Element result = Interpreter.Eval(exp, env);
                    if (!(result is EmptyElement))
                    {
                        Console.WriteLine(result.Text);
                    }
                }
                return env;
            }
        }

        static void InitGlobalEnv(SchemeEnvironment global)
        {
            // + Operation
            {
                string id = "+";
                SchemeList procParams = new SchemeList();
                procParams.Append(new TokenElement("a"));
                procParams.Append(new TokenElement("b"));

                Func<Element, Element, Element> body = (a, b) => 
                {
                    NumberElement ret = null;
                    if (a is NumberElement && b is NumberElement)
                    {
                        double ad = (a as NumberElement).Value;
                        double bd = (b as NumberElement).Value;                       
                        ret = new NumberElement((ad + bd).ToString());
                        
                    }
                    else throw new InvalidOperationException("Expecting two NumberElements, got "
                                                + a.GetType().Name + " and "
                                                + b.GetType().Name + " instead.");
                    return ret; 
                };

                OperationProcedur plus = new OperationProcedur(procParams, body, global);

                global.UpdateProc(id, plus);
            }
        }
    }
}
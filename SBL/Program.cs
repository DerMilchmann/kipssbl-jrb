using System;
using System.Collections.Generic;
using System.IO;

namespace Lambda
{
    class Program
    {
        static SchemeEnvironment global;
        static void Main(string[] args)
        {
            if (!File.Exists("../../SchemeCode.rkt"))
                throw new IOException("File SchemeCode.rkt does not exist in this directory.\n");


            InitGlobalEnv();
            SchemeEnvironment env = ReadFile();

            //Read single lines with operations
            StreamReader s = new StreamReader(Console.OpenStandardInput());
            while (true)
            {
                string input = s.ReadLine();              

                if (input == "reload")
                    env = ReadFile();
                else
                {
                    var stream = ConvertInputToStream(input);

                    LineInput(stream, env);
                }
            }
        }

        static Stream ConvertInputToStream(string input)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(input);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        static void LineInput(Stream stream, SchemeEnvironment env)
        {
            try
            {
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
            } catch (SchemeException se) { se.Display(); }
        }

        static SchemeEnvironment ReadFile()
        {
            using (FileStream stream = File.OpenRead("../../SchemeCode.rkt"))
            {
                stream.Position = 0;

                var env = new SchemeEnvironment(global);

                try
                {
                    SchemeLexer sl = new SchemeLexer(stream);
                    SchemeParser sp = new SchemeParser(sl);
                    List<Entry> entrys = sp.Parse();


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
                }catch (SchemeException se) { se.Display(); }

                return env;
            }
        }

        static void InitGlobalEnv()
        {
            global = new SchemeEnvironment(null);
            // ++++++++++
            {
                string id = "+";
                SchemeList procParams = new SchemeList();
                procParams.Append(new TokenElement(""));
                procParams.Append(new TokenElement(""));

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

            // ----------
            {
                string id = "-";
                SchemeList procParams = new SchemeList();
                procParams.Append(new TokenElement(""));
                procParams.Append(new TokenElement(""));

                Func<Element, Element, Element> body = (a, b) =>
                {
                    NumberElement ret = null;
                    if (a is NumberElement && b is NumberElement)
                    {
                        double ad = (a as NumberElement).Value;
                        double bd = (b as NumberElement).Value;
                        ret = new NumberElement((ad - bd).ToString());

                    }
                    else throw new InvalidOperationException("Expecting two NumberElements, got "
                                                + a.GetType().Name + " and "
                                                + b.GetType().Name + " instead.");
                    return ret;
                };

                OperationProcedur minus = new OperationProcedur(procParams, body, global);

                global.UpdateProc(id, minus);
            }

            // //////////
            {
                string id = "/";
                SchemeList procParams = new SchemeList();
                procParams.Append(new TokenElement(""));
                procParams.Append(new TokenElement(""));

                Func<Element, Element, Element> body = (a, b) =>
                {
                    NumberElement ret = null;
                    if (a is NumberElement && b is NumberElement)
                    {
                        double ad = (a as NumberElement).Value;
                        double bd = (b as NumberElement).Value;
                        ret = new NumberElement((ad / bd).ToString());

                    }
                    else throw new InvalidOperationException("Expecting two NumberElements, got "
                                                + a.GetType().Name + " and "
                                                + b.GetType().Name + " instead.");
                    return ret;
                };

                OperationProcedur divide = new OperationProcedur(procParams, body, global);

                global.UpdateProc(id, divide);
            }

            // **********
            {
                string id = "*";
                SchemeList procParams = new SchemeList();
                procParams.Append(new TokenElement(""));
                procParams.Append(new TokenElement(""));

                Func<Element, Element, Element> body = (a, b) =>
                {
                    NumberElement ret = null;
                    if (a is NumberElement && b is NumberElement)
                    {
                        double ad = (a as NumberElement).Value;
                        double bd = (b as NumberElement).Value;
                        ret = new NumberElement((ad * bd).ToString());

                    }
                    else throw new InvalidOperationException("Expecting two NumberElements, got "
                                                + a.GetType().Name + " and "
                                                + b.GetType().Name + " instead.");
                    return ret;
                };

                OperationProcedur multiplication = new OperationProcedur(procParams, body, global);

                global.UpdateProc(id, multiplication);
            }

            // ==========
            {
                string id = "=";
                SchemeList procParams = new SchemeList();
                procParams.Append(new TokenElement(""));
                procParams.Append(new TokenElement(""));

                Func<Element, Element, Element> body = (a, b) =>
                {
                    BoolElement ret = null;
                    if (a is NumberElement && b is NumberElement)
                    {
                        double ad = (a as NumberElement).Value;
                        double bd = (b as NumberElement).Value;
                        ret = new BoolElement(Equals(ad, bd).ToString());

                    }
                    else throw new InvalidOperationException("Expecting two NumberElements, got "
                                                + a.GetType().Name + " and "
                                                + b.GetType().Name + " instead.");
                    return ret;
                };

                OperationProcedur equals = new OperationProcedur(procParams, body, global);

                global.UpdateProc(id, equals);
            }

            // <<<<<<<<<<
            {
                string id = "<";
                SchemeList procParams = new SchemeList();
                procParams.Append(new TokenElement(""));
                procParams.Append(new TokenElement(""));

                Func<Element, Element, Element> body = (a, b) =>
                {
                    BoolElement ret = null;
                    if (a is NumberElement && b is NumberElement)
                    {
                        double ad = (a as NumberElement).Value;
                        double bd = (b as NumberElement).Value;
                        ret = new BoolElement((ad < bd).ToString());

                    }
                    else throw new InvalidOperationException("Expecting two NumberElements, got "
                                                + a.GetType().Name + " and "
                                                + b.GetType().Name + " instead.");
                    return ret;
                };

                OperationProcedur smaller = new OperationProcedur(procParams, body, global);

                global.UpdateProc(id, smaller);
            }

            // >>>>>>>>>>
            {
                string id = ">";
                SchemeList procParams = new SchemeList();
                procParams.Append(new TokenElement(""));
                procParams.Append(new TokenElement(""));

                Func<Element, Element, Element> body = (a, b) =>
                {
                    BoolElement ret = null;
                    if (a is NumberElement && b is NumberElement)
                    {
                        double ad = (a as NumberElement).Value;
                        double bd = (b as NumberElement).Value;
                        ret = new BoolElement((ad > bd).ToString());

                    }
                    else throw new InvalidOperationException("Expecting two NumberElements, got "
                                                + a.GetType().Name + " and "
                                                + b.GetType().Name + " instead.");
                    return ret;
                };

                OperationProcedur greater = new OperationProcedur(procParams, body, global);

                global.UpdateProc(id, greater);
            }
        }
    }
}
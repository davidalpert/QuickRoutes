using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.WebPages.Compilation.Parser;
using Microsoft.WebPages.Compilation;
using Microsoft.WebPages;
using System.Web;

namespace GustavoMachado.RazorConsoleWriter
{
    public class RazorFactory
    {
        public List<String> ReferencedAssemblies
        {
            get;
            private set;
        }
        private List<string> errors = new List<string>();
        public string[] Errors
        {
            get
            {
                return errors.ToArray();
            }
        }
        public RazorFactory()
        {
            ReferencedAssemblies = new List<string>();
            ReferencedAssemblies.Add("System.dll");
            ReferencedAssemblies.Add("System.Core.dll");
            ReferencedAssemblies.Add("System.Web.dll");
            ReferencedAssemblies.Add("System.Web.Mvc.dll");
            ReferencedAssemblies.Add("System.Data.dll");
            ReferencedAssemblies.Add("System.Web.Extensions.dll");
            ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            ReferencedAssemblies.Add("Microsoft.WebPages.dll");
            ReferencedAssemblies.Add("Microsoft.WebPages.Compilation.dll");
            ReferencedAssemblies.Add("Microsoft.WebPages.Configuration.dll");
            ReferencedAssemblies.Add("Microsoft.WebPages.Helpers.dll");
            ReferencedAssemblies.Add("Microsoft.WebPages.Helpers.Toolkit.dll");
            ReferencedAssemblies.Add("Microsoft.Data.Schema.dll");
        }
        public T Create<T>(string fileName) where T : class
        {
            using (var reader = new StreamReader(fileName))
            {
                return Create<T>(reader);
            }
        }
        public T Create<T>(TextReader reader) where T : class
        {
            var codeParser = new CSharpCodeParser();
            var markupParser = new HtmlMarkupParser();
            var parser = new InlinePageParser(codeParser, markupParser);

            var listener = new CSharpCodeGeneratorParserListener(
                    "className",
                    "ASP",
                    "System.Web.HttpApplication",
                    "linePragmaFileName",
                    typeof(T).FullName);

            foreach (var n in CodeGeneratorSettings.GetGlobalImports())
                listener.AdditionalImports.Add(n);
            parser.Parse(reader, listener);

            var option = new System.CodeDom.Compiler.CompilerParameters();
            option.ReferencedAssemblies.AddRange(ReferencedAssemblies.ToArray());
            option.GenerateExecutable = false;
            option.GenerateInMemory = true;

            var options = new Dictionary<string, string>();
            options.Add("CompilerVersion", "v3.5");

            var compiler = Microsoft.CSharp.CSharpCodeProvider.CreateProvider("C#", options);

            var compiled = compiler.CompileAssemblyFromDom(
                option,
                listener.GeneratedCode);

            foreach (var e in compiled.Errors)
                errors.Add(e.ToString());

            if (Errors.Length > 0)
                return null;
            else
            {
                var page = Activator.CreateInstance(compiled.CompiledAssembly.GetTypes().FirstOrDefault()) as T;
                return page;
            }
        }
    }

    public class ConsoleWriter : WebPage
    {
        public new string Model
        {
            get;set;
        }
        public override void Execute()
        {
            this.Context = new HttpContextWrapper(new HttpContext(new HttpRequest(null, null, null), new HttpResponse(Console.Out)));
        }
        public override void WriteLiteral(object o)
        {
            if (o != null)
                Console.Write(o.ToString());
        }
        public override void Write(Microsoft.WebPages.Helpers.HelperResult result)
        {
            Write(result);
        }
        public override void Write(object value)
        {
            if (value != null)
                Console.Write(value.ToString());
        }
    }
public class FileWriter<T> : WebPage, IDisposable
    {
        private string filePath;
        public string FilePath 
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
                File.WriteAllText(FilePath, string.Empty);
                writer = new StreamWriter(FilePath);
                this.Context = new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://razor", null), new HttpResponse(writer)));
            }
        }
        private StreamWriter writer;
        public FileWriter()
        {
        }
        public new T Model
        {
            get;
            set;
        }
        public override void Execute()
        {

        }
        public override void WriteLiteral(object o)
        {
            if (o != null)
                writer.Write(o.ToString());
        }
        public override void Write(Microsoft.WebPages.Helpers.HelperResult result)
        {
            Write(result);
        }
        public override void Write(object value)
        {
            if (value != null)
                writer.Write(value.ToString());
        }
        public void Dispose()
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
                writer.Dispose();
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Test.txt");

            var factory = new RazorClient.RazorFactory();
            factory.ReferencedAssemblies.Add("ConsoleApplication1.exe");

            using (var page = factory.Create<FileWriter<string>>(filePath))
            {

                if (factory.Errors.Length > 0)
                    foreach (var e in factory.Errors)
                        Console.WriteLine(e);
                else
                {
                    page.FilePath = filePath + ".rzr.txt";
                    page.Model = "this is my model";
                    page.Execute();
                }
            }
            Console.ReadKey();

        }
    }
}

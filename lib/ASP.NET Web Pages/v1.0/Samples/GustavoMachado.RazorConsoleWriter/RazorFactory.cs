using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.WebPages.Compilation.Parser;
using Microsoft.WebPages.Compilation;
using Microsoft.WebPages;
using System.Web;

namespace RazorClient
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
}

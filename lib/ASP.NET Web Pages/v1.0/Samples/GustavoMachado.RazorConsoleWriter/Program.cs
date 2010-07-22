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
    class Program
    {
        static void Main(string[] args)
        {
            var filename = args[0];
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);
            var outputFilePath = filePath + ".htm";
            var viewmodel = "this is my model";

            var factory = new RazorClient.RazorFactory();
            factory.ReferencedAssemblies.Add("RazorConsoleWriter.exe");

            using (var page = factory.Create<FileWriter<string>>(filename))
            {
                if (factory.Errors.Length > 0)
                    foreach (var e in factory.Errors)
                        Console.WriteLine(e);
                else
                {
                    page.FilePath = outputFilePath;
                    page.Model = viewmodel;
                    page.Execute();
                }
            }

            // DA: let's cat the output to the console
            if (factory.Errors.Length == 0)
            {
                string output = File.ReadAllText(outputFilePath);
                Console.WriteLine(output);
            }

            // DA: and let's make pausing after execution 
            //     conditional on a command-line arg
            if (args.Contains("--pause"))
            {
                Console.ReadKey();
            }
        }
    }
}

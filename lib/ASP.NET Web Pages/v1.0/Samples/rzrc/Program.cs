using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WebPages.Compilation;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System.IO;
using Microsoft.WebPages.Compilation.Parser;
using CodeDom = System.CodeDom.Compiler;

namespace Razor.Compiler {
    class Program {
        private static Dictionary<string, CodeDom.CodeDomProvider> _codeLanguageGenerators = new Dictionary<string, CodeDom.CodeDomProvider>() {
            { "cshtml", new CSharpCodeProvider() },
            { "vbhtml", new VBCodeProvider() }
        };

        static void Main(string[] args) {
            // Parse command line args
            string inputFileName = args[0];

            // Determine the language
            string extension = Path.GetExtension(inputFileName);

            CodeLanguageService languageService = CodeLanguageService.GetServiceByExtension(extension);
            if (languageService == null) {
                Console.WriteLine("{0} is not a Razor code language", extension);
                return;
            }
            CodeDom.CodeDomProvider provider = _codeLanguageGenerators[extension.TrimStart('.')];

            // Determine the output file name
            string outputFile = Path.GetFileNameWithoutExtension(inputFileName) + "." + provider.FileExtension;

            // Load the parser
            InlinePageParser parser = new InlinePageParser(languageService.CreateCodeParser(), new HtmlMarkupParser());

            // Create the code generator
            string className = Path.GetFileNameWithoutExtension(outputFile);
            CodeGenerator codeGenerator =
                languageService.CreateCodeGeneratorParserListener(className,
                                                                    rootNamespaceName: "Template",
                                                                    applicationTypeName: "object",
                                                                    linePragmaFileName: inputFileName,
                                                                    baseClass: "System.Object");
            CustomParserConsumer consumer = new CustomParserConsumer() { CodeGenerator = codeGenerator };

            try {
                // Run the parser
                using (StreamReader reader = new StreamReader(inputFileName)) {
                    parser.Parse(reader, consumer);
                }

                if (consumer.Errors > 0) {
                    Console.WriteLine("Translation failed, {0} errors", consumer.Errors);
                    return;
                }

                // Clean out web stuff from the generated code
                codeGenerator.GeneratedCode.Namespaces[0].Types[0].Members.RemoveAt(0);
                // EXT: Add useful context propertie 
                codeGenerator.GeneratedCode.Namespaces[0].Types[0].BaseTypes.Clear();
                // EXT: Set your own base type if you want!
                codeGenerator.GeneratedCode.Namespaces[0].Imports.Clear();
                // EXT: You should add your own default imports here!

                // Write the code to the output file
                using (StreamWriter writer = new StreamWriter(outputFile)) {
                    provider.GenerateCodeFromCompileUnit(codeGenerator.GeneratedCode, writer, new CodeDom.CodeGeneratorOptions());
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return;
            }

            Console.WriteLine("Translation successful!");
        }

        private class CustomParserConsumer : IParserConsumer {
            public CodeGenerator CodeGenerator { get; set; }

            public bool IsCanceled {
                get { return false; }
            }

            public int Errors { get; set; }

            public void OnComplete() {
                CodeGenerator.OnComplete();
            }

            public void OnEndBlock(BlockType type) {
                CodeGenerator.OnEndBlock(type);
            }

            public void OnEndSpan(ParsedSpan span) {
                CodeGenerator.OnEndSpan(span);
            }

            public void OnError(ParserError err) {
                Errors++;
                Console.WriteLine("Error at line {0} column {1}: {2}", err.Location.LineIndex + 1, err.Location.CharacterIndex + 1, err.Message);
                CodeGenerator.OnError(err);
            }

            public void OnStartBlock(BlockType type) {
                CodeGenerator.OnStartBlock(type);
            }
        }
    }
}

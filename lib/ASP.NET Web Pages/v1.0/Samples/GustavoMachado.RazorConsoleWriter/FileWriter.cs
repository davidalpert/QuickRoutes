using System;
using System.IO;
using System.Web;
using Microsoft.WebPages;

namespace GustavoMachado.RazorConsoleWriter
{
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
}

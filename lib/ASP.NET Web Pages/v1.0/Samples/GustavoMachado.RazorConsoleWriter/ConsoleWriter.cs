using System;
using Microsoft.WebPages;
using System.Web;

namespace GustavoMachado.RazorConsoleWriter
{
    public class ConsoleWriter : WebPage
    {
        public new string Model
        {
            get;
            set;
        }
        public override void Execute()
        {
            this.Context = new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://razor", null), new HttpResponse(Console.Out)));
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
}

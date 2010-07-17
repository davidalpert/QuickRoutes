using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using System.Collections.Specialized;

namespace QuickRoutes
{
    public interface IContext
    {
        void Write(string text);
        NameValueCollection Form { get; }
    }

    public class Context : IContext
    {
        HttpContextWrapper httpContext;

        /// <summary>
        /// Initializes a new instance of the Context class.
        /// </summary>
        /// <param name="httpContext"></param>
        public Context(HttpContextWrapper httpContext)
        {
            this.httpContext = httpContext;
        }

        public void Write(string text)
        {
            this.httpContext.Response.Write(text);
        }

        public NameValueCollection Form { get { return httpContext.Request.Form; } }
    }
}

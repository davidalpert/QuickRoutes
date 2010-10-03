using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using System.Collections.Specialized;

namespace QuickRoutes
{
    public interface IQuickContext
    {
        void Write(string text);
    }

    public class QuickContext : IQuickContext
    {
        HttpContextWrapper httpContext;

        /// <summary>
        /// Initializes a new instance of the Context class.
        /// </summary>
        /// <param name="httpContext"></param>
        public QuickContext(HttpContextWrapper httpContext)
        {
            this.httpContext = httpContext;
        }

        public void Write(string param1)
        {
            this.httpContext.Response.Write(param1);
        }
    }
}

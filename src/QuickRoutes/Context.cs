using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace QuickRoutes
{
    public class Context
    {
        RequestContext requestContext;

        /// <summary>
        /// Initializes a new instance of the Context class.
        /// </summary>
        /// <param name="requestContext"></param>
        public Context(RequestContext requestContext)
        {
            this.requestContext = requestContext;
        }

        public void Write(string param1)
        {
            this.requestContext.HttpContext.Response.Write(param1);
        }
    }
}

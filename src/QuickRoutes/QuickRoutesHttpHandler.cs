using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace QuickRoutes
{
    public class QuickRoutesHttpHandler : IHttpHandler
    {
        RequestContext requestContext;

        /// <summary>
        /// Initializes a new instance of the QuickRoutesHttpHandler class.
        /// </summary>
        /// <param name="requestContext"></param>
        public QuickRoutesHttpHandler(RequestContext requestContext)
        {
            this.requestContext = requestContext;
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            App app = context.ApplicationInstance as App;
            if (app == null)
            {
                throw new InvalidOperationException("Current HttpApplication is not a QuickRoutes App!");
            }

            string method = requestContext.HttpContext.Request.HttpMethod;
            string route = requestContext.HttpContext.Request.RawUrl;
            Context quickRoutesContext = new Context(requestContext);

            app.InvokeHandlerFor(method, route, quickRoutesContext);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace QuickRoutes
{
    public class SinetraHttpHandler : IHttpHandler
    {
        RequestContext requestContext;

        /// <summary>
        /// Initializes a new instance of the QuickRoutesHttpHandler class.
        /// </summary>
        /// <param name="requestContext"></param>
        public SinetraHttpHandler(RequestContext requestContext)
        {
            this.requestContext = requestContext;
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            App app = context.ApplicationInstance as App;
            app.InvokeHandlerFor(context.Request.RequestType, context.Request.RawUrl, requestContext);
        }
    }
}

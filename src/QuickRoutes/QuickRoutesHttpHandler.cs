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
        IContext context;

        /// <summary>
        /// Initializes a new instance of the QuickRoutesHttpHandler class.
        /// </summary>
        /// <param name="contextWrapper"></param>
        public QuickRoutesHttpHandler(IContext context)
        {
            this.context = context;
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
                throw new InvalidOperationException("The current HttpApplication does not inherit from QuickRoutes.App!");
            }

            string requestedMethod = context.Request.HttpMethod;
            SupportedHttpMethod parsedMethod;

            if (Enum<SupportedHttpMethod>.TryParse(requestedMethod, out parsedMethod) == false)
            {
                throw new InvalidOperationException(String.Format("HTTP method '{0}' is not supported by QuickRoutes.", requestedMethod));
            }

            var route = context.Request.RawUrl;
            var contextWrapper = new HttpContextWrapper(context);
            var quickContext = new Context(contextWrapper);

            app.InvokeHandlerFor(parsedMethod, route, quickContext);
        }
    }
}

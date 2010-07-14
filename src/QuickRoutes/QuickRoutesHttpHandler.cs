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
        Context context;

        /// <summary>
        /// Initializes a new instance of the QuickRoutesHttpHandler class.
        /// </summary>
        /// <param name="contextWrapper"></param>
        public QuickRoutesHttpHandler(Context context)
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
                throw new InvalidOperationException("Current HttpApplication is not a QuickRoutes App!");
            }

            string method = context.Request.HttpMethod;
            string route = context.Request.RawUrl;
            var contextWrapper = new HttpContextWrapper(context);
            var quickContext = new Context(contextWrapper);

            app.InvokeHandlerFor(method, route, quickContext);
        }
    }
}

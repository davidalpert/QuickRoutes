using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace QuickRoutes
{
    public class QuickRoutesHttpModule : IHttpModule
    {
        HttpApplication application;

        public void Dispose()
        {
            application.PostMapRequestHandler -= application_PostMapRequestHandler;
        }

        public void Init(HttpApplication context)
        {
            application = context;
            application.PostMapRequestHandler += application_PostMapRequestHandler;
        }

        void application_PostMapRequestHandler(object sender, EventArgs e)
        {
            var wrapper = new HttpContextWrapper(application.Context);

            wrapper.Handler = new QuickRoutesHttpHandler(new QuickContext(wrapper));
        }
    }
}

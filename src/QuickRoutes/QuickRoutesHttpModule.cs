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
            throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            application = context;
            application.PostMapRequestHandler += new EventHandler(application_PostMapRequestHandler);
        }

        void application_PostMapRequestHandler(object sender, EventArgs e)
        {
            var wrapper = new HttpContextWrapper(application.Context);

            wrapper.Handler = new QuickRoutesHttpHandler(new Context(wrapper));
        }

        void application_BeginRequest(object sender, EventArgs e)
        {
            Console.Write("BeginRequest.");
        }

        void context_MapRequestHandler(object sender, EventArgs e)
        {
            //Console.Write("MapRequestHandler.");
        }
    }
}

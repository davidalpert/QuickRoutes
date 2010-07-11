using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace QuickRoutes
{
    public class App : HttpApplication
    {
        private Dictionary<string, List<IHandler>> _handlers;

        public App()
        {
            _handlers = new Dictionary<string, List<IHandler>>();
            _handlers["GET"] = new List<IHandler>();
            _handlers["POST"] = new List<IHandler>();
            _handlers["PUT"] = new List<IHandler>();
            _handlers["DELETE"] = new List<IHandler>();
        }

        public void get(string route, Action<Context> handler)
        {
            _handlers["GET"].Add(new GetHandler { Route = route, Handler = handler });
            RouteTable.Routes.Add(new Route(route.TrimStart('~', '/'), new QuickRoutesRouteHandler()));
        }

        public void get(Func<string> route, Action<Context> handler)
        {
            get(route(), handler);
        }

        public void InvokeHandlerFor(string method, string rawUrl, Context context)
        {
            IHandler match = _handlers[method].Where(h => h.Route.Equals(rawUrl)).FirstOrDefault();
            if (match != null)
            {
                match.Handler(context);
            }
            else
            {
                throw new InvalidOperationException(String.Format("{0} {1} has no registered handler!", method, rawUrl));
            }
        }
    }
}
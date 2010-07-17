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
        private Dictionary<SupportedHttpMethod, List<IHandler>> _handlers;

        public App()
        {
            _handlers = new Dictionary<SupportedHttpMethod, List<IHandler>>();

            foreach (var method in Enum.GetValues(typeof(SupportedHttpMethod)).Cast<SupportedHttpMethod>())
            {
                _handlers[method] = new List<IHandler>();
            }

            // ensure that the favicon has a default action.
            get("/favicon.ico", c => { });
        }

        public Func<string, Func<IHandler, bool>> RouteMatchingStrategyFactory = url =>
        {
            return h => h.Route.Equals(url);
        };

        public void get(string route, Action<IContext> handler)
        {
            _handlers[SupportedHttpMethod.GET].Add(new RouteHandler { Route = route, Handle = handler });
        }

        public void get(Func<string> route, Action<IContext> handler)
        {
            get(route(), handler);
        }

        public void InvokeHandlerFor(SupportedHttpMethod method, string rawUrl, IContext context)
        {
            if (_handlers.Keys.Contains(method) == false)
            {
                throw new InvalidOperationException(String.Format("HTTP method '{0}' is known to QuickRoutes but not supported by this build.", method));
            }

            var routeMatchesRawUrl = RouteMatchingStrategyFactory(rawUrl);
            IHandler matchedHandler = _handlers[method].Where(routeMatchesRawUrl).LastOrDefault();

            if (matchedHandler == null)
            {
                throw new InvalidOperationException(String.Format("{0} {1} has no registered handler!", method, rawUrl));
            }

            matchedHandler.Handle(context);
        }
    }
}
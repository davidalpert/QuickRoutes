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
        private Context _context = null;
        private Dictionary<string, List<IHandler>> _handlers;

        public App(Context context)
        {
            _context = context;
            _handlers = new Dictionary<string, List<IHandler>>();
            _handlers["GET"] = new List<IHandler>();
            _handlers["POST"] = new List<IHandler>();
            _handlers["PUT"] = new List<IHandler>();
            _handlers["DELETE"] = new List<IHandler>();
        }

        public Context Context { get { return _context; } }

        public void get(string route, Action<Context> handler)
        {
            _handlers["GET"].Add(new GetHandler { Route = route, Handler = handler });
        }

        public void get(Func<string> route, Action<Context> handler)
        {
            get(route(), handler);
        }

        public void InvokeHandlerFor(string method, string rawUrl, RequestContext requestContext)
        {
            IHandler match = _handlers[method].Where(h => h.Route.Equals(rawUrl)).FirstOrDefault();
            if (match != null)
            {
                match.Handler(new Context());
            }
        }

        protected void Write(string param1)
        {
            throw new NotImplementedException();
        }
    }
}
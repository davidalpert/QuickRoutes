using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRoutes
{
    public class App
    {
        private Context _context = null;
        private List<IHandler> _getHandlers = null;
        private List<IHandler> _postHandlers = null;
        private List<IHandler> _putHandlers = null;
        private List<IHandler> _deleteHandlers = null;

        public App(Context context)
        {
            _context = context;
        }

        public Context Context { get { return _context; } }

        public void get(string route, Action<Context> handler)
        {
            _getHandlers.Add(new GetHandler { Route = route, Handler = handler });
        }

        public void get(Func<string> route, Action<Context> handler)
        {
            get(route(), handler);
        }

        protected void Write(string param1)
        {
            throw new NotImplementedException();
        }
    }
}
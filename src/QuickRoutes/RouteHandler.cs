using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRoutes
{
    public class RouteHandler : IHandler
    {
        public string Route { get; set; }
        public Action<IContext> Handle { get; set; }
    }
}

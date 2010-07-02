using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRoutes
{
    public class GetHandler : IHandler
    {
        public GetHandler()
        {

        }
        public string Route { get; set; }
        public Action<Context> Handler { get; set; }
    }
}

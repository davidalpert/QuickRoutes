using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRoutes
{
    public interface IHandler
    {
        string Route { get; set; }
        Action<Context> Handle { get; set; }
    }
}

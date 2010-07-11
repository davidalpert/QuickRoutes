using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;

namespace QuickRoutes
{
    public class QuickRoutesRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new QuickRoutesHttpHandler(requestContext);
        }
    }
}

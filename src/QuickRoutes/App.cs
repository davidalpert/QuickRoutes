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
		private Dictionary<SupportedHttpMethod, List<IQuickRoute>> _routes;

		private IQuickRouteFactory _routeFactory;

		public App()
		{
			_routeFactory = new DefaultQuickRouteFactory();

			_routes = new Dictionary<SupportedHttpMethod, List<IQuickRoute>>();

			foreach (var method in Enum.GetValues(typeof(SupportedHttpMethod)).Cast<SupportedHttpMethod>())
			{
				_routes[method] = new List<IQuickRoute>();
			}

			// ensure that the favicon has a default action.
			get("/favicon.ico", c => { });
		}

		#region get

		public void get(string pattern, Action<IQuickContext> handler)
		{

			_routes[SupportedHttpMethod.GET].Add(_routeFactory.BuildRouteLinking(pattern, handler));
		}

		public void get(Func<string> buildPattern, Action<IQuickContext> handler)
		{
			get(buildPattern(), handler);
		}

		#endregion

		public IQuickRoute FindRouteFor(SupportedHttpMethod method, string rawUrl)
		{
			if (_routes.Keys.Contains(method) == false)
			{
				throw new InvalidOperationException(String.Format("HTTP method '{0}' is known to QuickRoutes but not supported by this build.", method));
			}

			IQuickRoute matchedRoute = _routes[method].Where(route => route.CanHandle(rawUrl)).LastOrDefault();

			/* a.k.a.
			IQuickRoute matchedRoute = (from route in _routes[method]
										where route.CanHandle(rawUrl)
										select route).LastOrDefault();
			 */

			if (matchedRoute == null)
			{
				throw new InvalidOperationException(String.Format("{0} '{1}' has no registered handler!", method, rawUrl));
			}

			return matchedRoute;
		}
	}
}
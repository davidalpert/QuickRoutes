using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRoutes
{
	/// <summary>
	/// Builds the most complex known <see cref="IQuickRoute"/> available to
	/// process the given route pattern.
	/// </summary>
	public class DefaultQuickRouteFactory : IQuickRouteFactory
	{
		public IQuickRoute BuildRouteLinking(string route, Action<IQuickContext> handler)
		{
			return new LiteralQuickRoute
			{
				Pattern = route,
				Handle = handler
			};
		}
	}
}

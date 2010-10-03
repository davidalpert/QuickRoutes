using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRoutes
{
	public interface IQuickRouteFactory
	{
		/// <summary>
		/// Responsible for building an <see cref="IQuickRoute"/> that links
		/// the given <paramref name="route"/> to the given <paramref name="handler"/>
		/// </summary>
		IQuickRoute BuildRouteLinking(string route, Action<IQuickContext> handler);
	}
}

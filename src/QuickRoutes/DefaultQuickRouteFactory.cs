using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRoutes
{
	public class ParameterizedQuickRoute<TInputModel> : IQuickRoute
	{
		public ParameterizedQuickRoute(Action<IQuickContext, TInputModel> innerAction)
		{
			Handle = ResovleAndHandle(innerAction);
		}

		public string Pattern { get; set; }

		public Action<IQuickContext> Handle { get; set; }

		public bool CanHandle(string rawUrl)
		{
			return false;
		}

		private Action<IQuickContext> ResovleAndHandle(Action<IQuickContext, TInputModel> innerAction)
		{
			return cxt =>
			{
				TInputModel input = default(TInputModel);
				innerAction(cxt, input);
			};
		}
	}

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

		public IQuickRoute BuildRouteLinking<TInputModel>(string route, Action<IQuickContext, TInputModel> handler)
		{
			return new ParameterizedQuickRoute<TInputModel>(handler)
			{
				Pattern = route
			};
		}
	}
}

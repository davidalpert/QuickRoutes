using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRoutes
{
	public class LiteralQuickRoute : IQuickRoute
	{
		public string Pattern { get; set; }

		public Action<IQuickContext> Handle { get; set; }

		public bool CanHandle(string rawUrl)
		{
			return rawUrl.ToLowerInvariant().Equals(Pattern.ToLowerInvariant());
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace QuickRoutes.Sample
{
	public class Minnow : App
	{
		public Minnow()
		{
			get("/", index);
			get("/hey", index);
			get("/hello", index);
			get("/hi", index);
		}

		private void index(IQuickContext context)
		{
			context.Write("Hey Skipper");
		}
	}
}
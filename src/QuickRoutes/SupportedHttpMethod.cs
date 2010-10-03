using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace QuickRoutes
{
	public enum SupportedHttpMethod
	{
		GET
		//,HEAD
		//,POST
		//,PUT
		//,DELETE
		//,TRACE
		//,OPTIONS
	}

	public static class SupportedHttpMethods
	{
		public static SupportedHttpMethod[] Get = new SupportedHttpMethod[] { SupportedHttpMethod.GET };
		//public static SupportedHttpMethod[] Head = new SupportedHttpMethod[] { SupportedHttpMethod.HEAD };
		//public static SupportedHttpMethod[] Post = new SupportedHttpMethod[] { SupportedHttpMethod.POST };
		//public static SupportedHttpMethod[] Put = new SupportedHttpMethod[] { SupportedHttpMethod.PUT };
		//public static SupportedHttpMethod[] Delete = new SupportedHttpMethod[] { SupportedHttpMethod.DELETE };
		//public static SupportedHttpMethod[] Trace = new SupportedHttpMethod[] { SupportedHttpMethod.TRACE };
		//public static SupportedHttpMethod[] Options = new SupportedHttpMethod[] { SupportedHttpMethod.OPTIONS };
		//public static SupportedHttpMethod[] All = new SupportedHttpMethod[] { SupportedHttpMethod.GET, SupportedHttpMethod.HEAD, SupportedHttpMethod.POST, SupportedHttpMethod.PUT, SupportedHttpMethod.DELETE, SupportedHttpMethod.TRACE, SupportedHttpMethod.OPTIONS };
	}

	public class MatchesHttpMethods
	{
		List<SupportedHttpMethod> supportedMethods = new List<SupportedHttpMethod>();

		public MatchesHttpMethods Get
		{
			get
			{
				supportedMethods.Add(SupportedHttpMethod.GET);
				return this;
			}
		}
	}
}

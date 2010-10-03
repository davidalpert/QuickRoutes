using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRoutes
{
	public interface IQuickRoute
	{
		string Pattern { get; set; }

		bool CanHandle(string rawUrl);

		Action<IQuickContext> Handle { get; set; }
	}

	/// <summary>
	/// This might eventually support strongly-typed
	/// input-model binding.
	/// </summary>
	/// <typeparam name="TInputModel"></typeparam>
	public interface IQuickRoute<TInputModel> : IQuickRoute
	{
		Action<IQuickContext, TInputModel> Handel { get; set; }
	}

	/// <summary>
	/// This might eventually support a FubuMVC, opinionated, 
	/// one-model-in-one-model-out kind of QuickRoute.
	/// </summary>
	/// <typeparam name="TInputModel"></typeparam>
	/// <typeparam name="TOutputModel"></typeparam>
	public interface IQuickRoute<TInputModel, TOutputModel> : IQuickRoute
	{
		Func<IQuickContext, TInputModel, TOutputModel> Handel { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using Should.Extensions;

namespace QuickRoutes.Tests
{
    public class TestSpike
    {
        protected class TestApp : App
        {
            /// <summary>
            /// Initializes a new instance of the TestApp class.
            /// </summary>
            public TestApp()
            {
                get("/hi", context => context.Write("hello"));
            }
        }

        protected TestApp app;

        /// <summary>
        /// Initializes a new instance of the TestSpike class.
        /// </summary>
        /// <param name=""></param>
        public TestSpike()
        {
            this.app = new TestApp();
        }

        [Fact]
        public void Monkey1()
        {
            var context = new QuickContextSpy();
            var route = app.FindRouteFor(SupportedHttpMethod.GET, "/hi");

            route.Handle(context);
            
            context.Response.ShouldEqual("hello");
        }

        [Fact]
        public void Monkey2()
        {
            var context = new QuickContextSpy();
            
            app.FindRouteFor(SupportedHttpMethod.GET, "/hi").Handle(context);
            
            context.Response.ShouldEqual("hello");
        }

        [Fact]
        public void Monkey3()
        {
            var context = new Mock<IQuickContext>().Object;
            
            app.FindRouteFor(SupportedHttpMethod.GET, "/hi").Handle(context);

            Mock.Get(context).Verify(cxt => cxt.Write("hello"));
        }
    }
}
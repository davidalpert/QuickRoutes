using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;

namespace QuickRoutes.Tests
{
    public class TestSpike
    {
        private class TestApp : App
        {
            /// <summary>
            /// Initializes a new instance of the TestApp class.
            /// </summary>
            public TestApp()
            {
                get("/hi", context => context.Write("hello"));
            }
        }

        [Fact]
        public void Monkey()
        {
            TestApp testApp = new TestApp();

            var route = testApp.FindRouteFor(SupportedHttpMethod.GET, "/hi");
            
            var context = new Mock<IQuickContext>();

            route.Handle(context.Object);

        }
    }
}

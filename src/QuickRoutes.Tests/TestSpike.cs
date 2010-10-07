using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using Should.Extensions;

namespace QuickRoutes.Tests
{
    public class GetWithParameters
    {
        protected class TestApp : App
        {
            public TestApp()
            {
                get<string>("/hi/{name}", (context, name) => context.Write("Hello "+name));
            }
        }

        [Fact]
        public void GET_route_with_simple_parameter_matches_when_url_component_is_missing() 
        {
            (new TestApp()).FindRouteFor(SupportedHttpMethod.GET, "/hi").Pattern
                .ShouldNotBeNull()
                .ShouldEqual("/hi/{name}");
        }

        [Fact]
        public void GET_route_with_simple_parameter_matches_when_url_component_is_empty() 
        {
            (new TestApp()).FindRouteFor(SupportedHttpMethod.GET, "/hi/").Pattern
                .ShouldNotBeNull()
                .ShouldEqual("/hi/{name}");
        }

        [Fact]
        public void GET_route_with_simple_parameter_matches_when_url_component_has_a_value() 
        {
            (new TestApp()).FindRouteFor(SupportedHttpMethod.GET, "/hi/bob").Pattern
                .ShouldNotBeNull()
                .ShouldEqual("/hi/{name}");
        }

        [Fact]
        public void GET_route_with_simple_parameter_matches_when_url_component_has_a_value_and_a_trailing_slash() 
        {
            (new TestApp()).FindRouteFor(SupportedHttpMethod.GET, "/hi/bob/").Pattern
                .ShouldNotBeNull()
                .ShouldEqual("/hi/{name}");
        }

        [Fact]
        public void GET_route_with_simple_parameter_does_not_matche_when_url_is_too_long() 
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                (new TestApp()).FindRouteFor(SupportedHttpMethod.GET, "/hi/bob/frank");
            });
        }

        [Fact]
        public void GET_route_throws_when_url_does_not_match() 
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                (new TestApp()).FindRouteFor(SupportedHttpMethod.GET, "/hi/bob/frank");
            });
        }

        [Fact]
        public void GET_route_when_url_does_not_match_throws_with_diagnostic_message() 
        {
            try
            {
                (new TestApp()).FindRouteFor(SupportedHttpMethod.GET, "/hi/bob/frank");
            }
            catch (InvalidOperationException ex)
            {
                ex.Message.ShouldEqual("GET '/hi/bob/frank' has no registered handler!");
            }
        }

        [Fact]
        public void Can_handle_GET_routes_with_a_single_parameter()
        {
            using (var app = new TestApp())
            {
                var context = new Mock<IQuickContext>().Object;

                app.FindRouteFor(SupportedHttpMethod.GET, "/hi/bob").Handle(context);

                Mock.Get(context).Verify(cxt => cxt.Write("Hello bob"));
            }
        }

        [Fact]
        public void Can_handle_GET_routes_with_a_single_parameter2()
        {
            using (var app = new TestApp())
            {
                var context = new QuickContextSpy();

                app.FindRouteFor(SupportedHttpMethod.GET, "/hi/bob").Handle(context);

                context.Response.ShouldEqual("Hello bob");
            }
        }
    }
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
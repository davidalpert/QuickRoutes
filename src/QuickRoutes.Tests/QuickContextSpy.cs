using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using Should.Extensions;

namespace QuickRoutes.Tests
{
    public class QuickContextSpy : IQuickContext
    {
        StringBuilder responseBuilder = new StringBuilder();

        public virtual void Write(string text)
        {
            responseBuilder.Append(text);
        }

        public string Response { get { return responseBuilder.ToString(); } }
    }
}

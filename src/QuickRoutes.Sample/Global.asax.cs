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
            get("/get", get);
            post("/post", post);
        }

        private void index(IContext context)
        {
            context.Write("Hey Skipper");
        }

        private void get(IContext context)
        {
            context.Write("<form method='POST' action='/post'><input type='text' id='data'/><button type='submit'>submit a post</button</form>");
        }

        private void post(IContext context)
        {
            context.Write("caught a post!<br/>data: " + context.Form["id"]);
        }
    }
}
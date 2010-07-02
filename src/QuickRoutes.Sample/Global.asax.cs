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
        public Minnow(Context context)
            : base(context)
        {
            get("/", index);
        }

        private void index(Context context)
        {
            this.Write("Hey Skipper");
        }
    }
}
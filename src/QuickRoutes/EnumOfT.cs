using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace QuickRoutes
{
    public static class Enum<T>
    {
        public static IEnumerable<T> GetValues()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static bool TryParse(string source, out T result)
        {
            result = default(T);

            if (String.IsNullOrEmpty(source))
            {
                // cannot parse a null or empty source string
                return false;
            }

            string sourcelower = source.ToLower();
            IEnumerable<T> matches = Enum<T>.GetValues().Where(value =>
            {
                return value.ToString().ToLower().Equals(sourcelower);
            });

            if (matches.Count() == 0)
            {
                // no values match the source string
                return false;
            }

            result = matches.First();

            return true;
        }
    }
}

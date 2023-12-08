
// We define our custom constraint by creating the class for it and implements interface 'IRouteConstraint'

using System.Text.RegularExpressions;

namespace RoutingExample.CustomConstraints
{
    // eg. /sales-report/2020/apr
    // 'routeKey' is month
    // 'values' are values in that key
    public class MonthsCustomConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // check whether the value exists
            if (!values.ContainsKey(routeKey))  // month
            {
                return false;
            }

            Regex regex = new("^(apr|jul|oct|jan)$");
            string? monthValue = Convert.ToString(values[routeKey]);

            if (regex.IsMatch(monthValue!))
            {
                return true;    // match
            }

            return false;       // doesn't match
        }
    }
}

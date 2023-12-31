﻿using System.Text.RegularExpressions;

namespace RoutingExample.CustomConstraints
{
    public class MonthsCustomConstraint : IRouteConstraint
    {
        //Eg: sales-report/2020/apr
        public bool Match(HttpContext? httpContext, 
            IRouter? route, string routeKey, 
            RouteValueDictionary values, 
            RouteDirection routeDirection)
        {
            if (!values.ContainsKey(routeKey)) //month
            {
                return false; //not a match
            }

            Regex regex = new Regex($"^(apr|jul|oct|jan)$");
            string? monthValue = Convert.ToString(values[routeKey]);

            if (regex.IsMatch(monthValue))
            {
                return true;
            }
            return false;
        }
    }
}

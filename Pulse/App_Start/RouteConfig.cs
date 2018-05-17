using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HealthcareAnalytics
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "ARManagement", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "executivesummarypage",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "ExecutiveSummary", id = UrlParameter.Optional }
            );

              
        }
    }
}

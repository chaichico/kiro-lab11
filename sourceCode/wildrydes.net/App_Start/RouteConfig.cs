using System.Web.Mvc;
using System.Web.Routing;

namespace wildrydes.net
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "TestImage",
                url: "Ride/TestImage/{z}/{x}/{y}",
                defaults: new { controller = "Ride", action = "TestImage" },
                constraints: new { z = @"\d+", x = @"\d+", y = @"\d+" }
            );

            routes.MapRoute(
                name: "MapTile",
                url: "Ride/GetMapTile/{z}/{x}/{y}",
                defaults: new { controller = "Ride", action = "GetMapTile" },
                constraints: new { z = @"\d+", x = @"\d+", y = @"\d+" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

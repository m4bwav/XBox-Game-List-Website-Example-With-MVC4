using System.Web.Mvc;
using System.Web.Routing;

namespace XboxGamesListAndVoting
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", "{controller}/{action}/{title}",
                new {controller = "Home", action = "Index", title = UrlParameter.Optional}
                );
        }
    }
}
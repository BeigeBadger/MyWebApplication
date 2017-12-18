using System.Web.Mvc;
using System.Web.Routing;

namespace MyWebApplication
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Home",
				url: "{controller}/{action}",
				defaults: new { controller = "Home", action = "Index" }
			);

			routes.MapRoute(
				name: "Gaming Machines",
				url: "{controller}/{action}",
				defaults: new { controller = "GamingMachines", action = "Index" }
			);

			// TODO: Add more routes for CRUD operations
		}
	}
}
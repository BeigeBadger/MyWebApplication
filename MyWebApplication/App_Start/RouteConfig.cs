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
				name: "Gaming Machines Edit",
				url: "GamingMachines/{serialNumber}/Edit",
				defaults: new { controller = "GamingMachines", action = "Edit" }
			);

			routes.MapRoute(
				name: "Gaming Machines Delete",
				url: "GamingMachines/{serialNumber}/Delete",
				defaults: new { controller = "GamingMachines", action = "Delete" }
			);

			routes.MapRoute(
				name: "Home",
				url: "{controller}/{action}",
				defaults: new { controller = "Home", action = "Index" }
			);

			// TODO: Add more routes for CRUD operations
		}
	}
}
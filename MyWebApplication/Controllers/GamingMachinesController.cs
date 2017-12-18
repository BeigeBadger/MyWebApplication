using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyWebApplication.Controllers
{
	public class GamingMachinesController : Controller
	{
		private const int MaxPositionNumber = 1000;

		private List<GamingMachine> _gamingMachines = new List<GamingMachine>();

		// GET: GamingMachines
		public ActionResult Index()
		{
			PopulateGamingMachines();

			return View(_gamingMachines);
		}

		// TODO: Add gaming machine (POST)

		// TODO: Update gaming machine (PATCH)

		// TODO: Delete gaming machine (DELETE)

		private void PopulateGamingMachines()
		{
			for (int i = 0; i <= 1000; i++)
			{
				bool deleted = i % 9 == 0;

				GamingMachine gamingMachine = new GamingMachine(i, i, $"Game{i}", DateTime.Now, deleted);

				_gamingMachines.Add(gamingMachine);
			}
		}
	}
}
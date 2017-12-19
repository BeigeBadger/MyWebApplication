using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyWebApplication.Controllers
{
	public class GamingMachinesController : Controller
	{
		private const int MaxPositionNumber = 1000;

		private List<GamingMachine> _gamingMachines = new List<GamingMachine>();

		// GET: GamingMachines
		public ActionResult Index(string sortOrder)
		{
			PopulateGamingMachines();

			HandleSortOrder(sortOrder);

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

		private void HandleSortOrder(string sortOrder)
		{
			if (string.IsNullOrWhiteSpace(sortOrder))
				sortOrder = "PositionAsc";

			ViewBag.PositionSortParam = sortOrder == "PositionAsc" ? "PositionDesc" : "PositionAsc";
			ViewBag.NameSortParam = sortOrder == "NameDesc" ? "NameAsc" : "NameDesc";
			ViewBag.SerialSortParam = sortOrder == "SerialDesc" ? "SerialAsc" : "SerialDesc";

			switch (sortOrder)
			{
				case "PositionAsc":
					_gamingMachines = _gamingMachines.OrderBy(g => g.MachinePosition).ToList();
					break;

				case "PositionDesc":
					_gamingMachines = _gamingMachines.OrderByDescending(g => g.MachinePosition).ToList();
					break;

				case "NameAsc":
					_gamingMachines = _gamingMachines.OrderBy(g => g.Name).ToList();
					break;

				case "NameDesc":
					_gamingMachines = _gamingMachines.OrderByDescending(g => g.Name).ToList();
					break;

				case "SerialAsc":
					_gamingMachines = _gamingMachines.OrderBy(g => g.SerialNumber).ToList();
					break;

				case "SerialDesc":
					_gamingMachines = _gamingMachines.OrderByDescending(g => g.SerialNumber).ToList();
					break;

				default:
					throw new NotImplementedException($"The '{sortOrder}' option for '{nameof(sortOrder)}' parameter is not implemented!");
			}
		}
	}
}
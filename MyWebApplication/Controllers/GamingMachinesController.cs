using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyWebApplication.Controllers
{
	public class GamingMachinesController : Controller
	{
		private const int PageSize = 10;
		private const int MaxPositionNumber = 1000;

		private List<GamingMachine> _gamingMachines = new List<GamingMachine>();

		// GET: GamingMachines
		/// <summary>
		/// </summary>
		/// <param name="sortBy">What column to sort on and whether we should sort by ASC or DESC</param>
		/// <param name="filterBy">What the user would like to filter the results by aka search string</param>
		/// <param name="currentFilter">A backup of the user's filter so that we can maintain it through paging</param>
		/// <param name="page">The current page we are on</param>
		/// <returns></returns>
		public ActionResult Index(string sortBy, string filterBy, string currentFilter, int? page)
		{
			PopulateGamingMachines();

			// Maintain filtering throughout paging
			if (filterBy != null)
			{
				page = 1;
			}
			else
			{
				filterBy = currentFilter;
			}

			ViewBag.CurrentFilter = filterBy;

			// Do any filtering first as it will make sorting less expensive
			FilterItems(filterBy);
			SortItems(sortBy);

			int pageNumber = (page ?? 1);

			return View(_gamingMachines.ToPagedList(pageNumber, PageSize));
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

		private void SortItems(string sortBy)
		{
			if (string.IsNullOrWhiteSpace(sortBy))
				sortBy = "PositionAsc";

			// Maintain sorting throughout paging
			ViewBag.CurrentSort = sortBy;

			// Check sorting
			ViewBag.PositionSortParam = sortBy == "PositionAsc" ? "PositionDesc" : "PositionAsc";
			ViewBag.NameSortParam = sortBy == "NameDesc" ? "NameAsc" : "NameDesc";
			ViewBag.SerialSortParam = sortBy == "SerialDesc" ? "SerialAsc" : "SerialDesc";

			switch (sortBy)
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
					throw new NotImplementedException($"The '{sortBy}' option for '{nameof(sortBy)}' parameter is not implemented!");
			}
		}

		private void FilterItems(string filterBy)
		{
			// Exit early
			if (string.IsNullOrWhiteSpace(filterBy))
				return;

			_gamingMachines = _gamingMachines.Where(g => g.Name.Contains(filterBy)).ToList();
		}
	}
}
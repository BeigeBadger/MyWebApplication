using MyWebApplication.Models;
using MyWebApplication.Repositories;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyWebApplication.Controllers
{
	public class GamingMachinesController : Controller
	{
		private static List<GamingMachine> _gamingMachines = new List<GamingMachine>();

		private readonly IGamingMachineRepository GamingMachineRepository;

		private const int PageSize = 10;
		private const int MaxPositionNumber = 1000;

		public GamingMachinesController(IGamingMachineRepository gamingMachineRepository)
		{
			GamingMachineRepository = gamingMachineRepository;

			if (!_gamingMachines.Any())
			{
				RestoreDatabaseBackup();
			}
		}

		/// <summary>
		/// Loads the list of gaming machines
		/// </summary>
		/// <param name="sortBy">What column to sort on and whether we should sort by ASC or DESC</param>
		/// <param name="filterBy">What the user would like to filter the results by aka search string</param>
		/// <param name="currentFilter">A backup of the user's filter so that we can maintain it through paging</param>
		/// <param name="page">The current page we are on</param>
		/// <returns>A new Index view with a paged list of gaming machines populated</returns>
		[HttpGet]
		public ActionResult Index(string sortBy, string filterBy, string currentFilter, int? page, bool resetList = false)
		{
			if (resetList)
			{
				RestoreDatabaseBackup();
			}

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

		/// <summary>
		/// Navigates to the Create view and optionally
		/// displays a success message
		/// </summary>
		/// <param name="machineName">[Optional]The name of the machine that was successfully created</param>
		/// <returns>A new Create view</returns>
		[HttpGet]
		public ActionResult Create(string machineName)
		{
			if (!string.IsNullOrWhiteSpace(machineName))
			{
				// Set success message
				ViewBag.CreateResultMessage = $"Successfully created machine with name '{machineName}'.";
				ViewBag.CreatedSucceeded = true;
			}

			return View();
		}

		/// <summary>
		/// Creates a new Gaming Machine using Bind to
		/// prevent overposting and antiforgery to prevent
		/// XSS attacks
		/// </summary>
		/// <param name="gamingMachine">The gaming machine to create</param>
		/// <returns>The Create view with validation errors or a success message</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "SerialNumber, MachinePosition, Name")]GamingMachine gamingMachine)
		{
			//List<ModelError> modelStateErrors = ModelState.Values.SelectMany(v => v.Errors).ToList();
			string failureMessage = $"Model validation failed.";

			if (ModelState.IsValid)
			{
				Result result = GamingMachineRepository.CreateGamingMachine(gamingMachine);

				if (result.ResultCode == ResultTypeEnum.Success)
				{
					// Update the list used by the view to include the new entry
					RestoreDatabaseBackup();

					// Use PRG pattern to prevent resubmit on page refresh
					return RedirectToAction("Create", new { machineName = gamingMachine.Name });
				}

				failureMessage = $"There was an error creating the machine: {result.ResultMessage}";
			}

			// Set failure message
			ViewBag.CreateResultMessage = failureMessage;
			ViewBag.CreatedSucceeded = false;

			return View();
		}

		/// <summary>
		/// Load a gaming machine to edit and optionally
		/// displays a success message
		/// </summary>
		/// <param name="serialNumber">The serial number of the gaming machine to update</param>
		/// <returns>A new Edit view with the details of the provided gaming machine populated</returns>
		[HttpGet]
		public ActionResult Edit(long? serialNumber, string machineName)
		{
			if (serialNumber == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			// Get by serial
			GamingMachine gamingMachine = GamingMachineRepository.Get(serialNumber.Value);

			if (gamingMachine == null)
			{
				return HttpNotFound();
			}

			if (!string.IsNullOrWhiteSpace(machineName))
			{
				// Set success message
				ViewBag.UpdateResultMessage = $"Successfully updated machine with name (old/new) '{machineName}'.";
				ViewBag.UpdateSucceeded = true;
			}

			return View(gamingMachine);
		}

		/// <summary>
		/// Update a gaming machine
		///
		/// Have to use HttpPost as HttpPut is not supported
		/// by ASP.NET MVC5.
		///
		/// Also use ActionName to get around the error about
		/// two methods with the same signature but also still
		/// call the correct method using aliasing.
		///
		/// Use antiforgery to prevent XSS attacks
		/// </summary>
		/// <param name="serialNumber">The serial number of the gaming machine to update</param>
		/// <returns>The Edit view with validation errors or the Index view with a success message</returns>
		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public ActionResult EditMachine(long? serialNumber)
		{
			if (serialNumber == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			// Get by serial
			GamingMachine gamingMachineToUpdate = GamingMachineRepository.Get(serialNumber.Value);
			string failureMessage = $"Model validation failed.";

			if (ModelState.IsValid)
			{
				string oldName = gamingMachineToUpdate.Name;
				int oldPosition = gamingMachineToUpdate.MachinePosition;

				try
				{
					// Attempt to update model - Provide a list of properties that are allowed to be updated
					UpdateModel(gamingMachineToUpdate, "", new string[] { "MachinePosition", "Name" });

					Result result = GamingMachineRepository.UpdateGamingMachine(gamingMachineToUpdate);

					if (result.ResultCode == ResultTypeEnum.Success)
					{
						// Update the list used by the view to include the new entry
						RestoreDatabaseBackup();

						// Use PRG pattern to prevent resubmit on page refresh
						return RedirectToAction("Edit", new { serialNumber, machineName = $"{oldName}/{gamingMachineToUpdate.Name}" });
					}

					failureMessage = $"There was an error updating the machine: {result.ResultMessage}";
				}
				catch (Exception)
				{
					// Reset the name and position values to their original state UpdateModel() overwrites them and updates the master
					// list somehow... this will not reinstate them inside the Edit form (good behaviour) but prevents the master list
					// from being updated with bogus values
					gamingMachineToUpdate.Name = oldName;
					gamingMachineToUpdate.MachinePosition = oldPosition;
				}
			}

			// Set failure message
			ViewBag.UpdateResultMessage = failureMessage;
			ViewBag.UpdateSucceeded = false;

			return View(gamingMachineToUpdate);
		}

		// TODO: Delete gaming machine (DELETE)

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

		private void RestoreDatabaseBackup()
		{
			_gamingMachines.Clear();
			_gamingMachines.AddRange(GamingMachineRepository.GetDatabaseBackup());
		}
	}
}
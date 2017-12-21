using MyWebApplication.Helpers;
using MyWebApplication.Models;
using MyWebApplication.Repositories;
using MyWebApplication.ViewModels;
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
		// TODO: Replace all ViewBag with TempData
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

			// TODO: Use view model here

			return View(_gamingMachines.ToPagedList(pageNumber, PageSize));
		}

		/// <summary>
		/// Navigates to the Create view and displays controls for creating
		/// a new <see cref="GamingMachine"/>
		/// </summary>
		/// <returns>A new <see cref="GamingMachineCreateViewModel"/></returns>
		[HttpGet]
		public ActionResult Create()
		{
			return View(new GamingMachineCreateViewModel());
		}

		/// <summary>
		/// Creates a new <see cref="GamingMachine"/>
		/// </summary>
		/// <param name="gamingMachine">The gaming machine to create</param>
		/// <returns>A <see cref="GamingMachineCreateViewModel"/> with validation errors or a success message</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(GamingMachineCreateViewModel createViewModel)
		{
			// TODO: Use ViewModel
			string failureMessage = $"Model validation failed.";

			if (ModelState.IsValid)
			{
				// TODO: Use extension method to do this
				GamingMachine gamingMachineToCreate = GamingMachineHelper.CreateGamingMachineModelFromCreateViewModel(createViewModel);
				Result result = GamingMachineRepository.CreateGamingMachine(gamingMachineToCreate);

				if (result.ResultCode == ResultTypeEnum.Success)
				{
					// Update the list used by the view to include the new entry
					RestoreDatabaseBackup();

					// Set success message
					TempData.Add("CreateResultMessage", $"Successfully created machine with name '{gamingMachineToCreate.Name}'.");
					TempData.Add("CreateSucceeded", true);

					// Use PRG pattern to prevent resubmit on page refresh
					return RedirectToAction("Create");
				}

				failureMessage = $"There was an error creating the machine: {result.ResultMessage}";
			}

			// Set failure message
			TempData.Add("CreateResultMessage", failureMessage);
			TempData.Add("CreateSucceeded", false);

			return View(createViewModel);
		}

		/// <summary>
		/// Navigates to the Edit view and display the details for a gaming machine
		/// specified the the serial number
		/// </summary>
		/// <param name="serialNumber">The serial number of the gaming machine to update</param>
		/// <returns>A new <see cref="GamingMachineEditViewModel"/> with the details of the <see cref="GamingMachine"/> associated with the serial number populated</returns>
		[HttpGet]
		public ActionResult Edit(long? serialNumber)
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

			GamingMachineEditViewModel editViewModel = GamingMachineHelper.CreateEditViewModelFromGamingMachineModel(gamingMachine);

			return View(editViewModel);
		}

		/// <summary>
		/// Update a <see cref="GamingMachine"/>
		///
		/// Have to use HttpPost as HttpPut is not supported
		/// by ASP.NET MVC5.
		///
		/// Also use ActionName to get around the error about
		/// two methods with the same signature but also still
		/// call the correct method using aliasing.
		/// </summary>
		/// <param name="serialNumber">The serial number of the gaming machine to update</param>
		/// <returns>A <see cref="GamingMachineEditViewModel"/> with validation errors or a success message</returns>
		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public ActionResult EditMachine(GamingMachineEditViewModel editViewModel)
		{
			long serialNumber = editViewModel.SerialNumber.Value;
			string failureMessage = $"Model validation failed.";

			if (ModelState.IsValid)
			{
				try
				{
					// Get by serial
					GamingMachine gamingMachineToUpdate = GamingMachineRepository.Get(serialNumber);

					if (gamingMachineToUpdate == null)
					{
						return HttpNotFound();
					}

					string oldName = gamingMachineToUpdate.Name;

					// Update model
					GamingMachineHelper.UpdateGamingMachineFromEditViewModel(gamingMachineToUpdate, editViewModel);

					Result result = GamingMachineRepository.UpdateGamingMachine(gamingMachineToUpdate);

					if (result.ResultCode == ResultTypeEnum.Success)
					{
						// Update the list used by the view to include the new entry
						RestoreDatabaseBackup();

						// Set success message
						TempData.Add("updateResultMessage", $"Successfully updated {oldName}/{gamingMachineToUpdate.Name}!");
						TempData.Add("updateSucceeded", true);

						// Use PRG pattern to prevent resubmit on page refresh
						return RedirectToAction("Edit", new { serialNumber = serialNumber });
					}

					failureMessage = $"There was an error updating the machine: {result.ResultMessage}";
				}
				catch (Exception)
				{
					failureMessage = "An exception occured while attempting to update the machine, please reload the page and try again";
				}
			}

			// Set failure message
			TempData.Add("updateResultMessage", failureMessage);
			TempData.Add("updateSucceeded", false);

			return View(editViewModel);
		}

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
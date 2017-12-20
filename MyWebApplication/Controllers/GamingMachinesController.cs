﻿using MyWebApplication.Models;
using MyWebApplication.Repositories;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
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

		// GET: GamingMachines
		/// <summary>
		/// </summary>
		/// <param name="sortBy">What column to sort on and whether we should sort by ASC or DESC</param>
		/// <param name="filterBy">What the user would like to filter the results by aka search string</param>
		/// <param name="currentFilter">A backup of the user's filter so that we can maintain it through paging</param>
		/// <param name="page">The current page we are on</param>
		/// <returns></returns>
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
		/// <returns>The Create view with validation errors or the Index view with a success message</returns>
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

		// TODO: Add gaming machine (POST)

		// TODO: Update gaming machine (PATCH)

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
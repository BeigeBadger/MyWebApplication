using MyWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApplication.Repositories
{
	public interface IGamingMachineRepository
	{
		List<GamingMachine> Get(int page = 0, int skip = 10, string filter = "");

		GamingMachine GetBySerial(long serialNumber);

		Result CreateGamingMachine(GamingMachine gamingMachine);

		Result UpdateGamingMachine(GamingMachine gamingMachine);

		Result DeleteGamingMachine(GamingMachine gamingMachine);

		List<GamingMachine> GetDatabaseBackup();
	}

	public class GamingMachineRepository : IGamingMachineRepository
	{
		private static List<GamingMachine> GamingMachineDatabase = new List<GamingMachine>();

		public GamingMachineRepository()
		{
			if (!GamingMachineDatabase.Any())
				PopulateGamingMachinesDatabase();
		}

		public List<GamingMachine> GetDatabaseBackup()
		{
			return GamingMachineDatabase;
		}

		public List<GamingMachine> Get(int page = 0, int skip = 10, string filter = "")
		{
			// Implemented but not used because the paging is done on the controller side using and IPagedList
			var machinesOnPage =
				GamingMachineDatabase.Where(g => g.Name.Contains(filter))
				.Skip(page * skip)
				.Take(skip).ToList();

			return machinesOnPage;
		}

		public GamingMachine GetBySerial(long serialNumber)
		{
			// TODO: long to int parsing issue here
			GamingMachine gamingMachine = GamingMachineDatabase.SingleOrDefault(g => g.SerialNumber == serialNumber);

			return gamingMachine;
		}

		public Result CreateGamingMachine(GamingMachine gamingMachine)
		{
			// Validate that the serial does not exist
			int index = GetIndexOfExistingMachine(gamingMachine.SerialNumber);

			// Machine exists
			if (index != -1)
				return new Result(ResultTypeEnum.Failure, $"A machine with the serial number '{gamingMachine.SerialNumber}' already exists, please enter a different serial number");

			// Validate that the position is between 0 and 1000 inclusive
			if (gamingMachine.MachinePosition < 0 || gamingMachine.MachinePosition > 1000)
				return new Result(ResultTypeEnum.Failure, $"Parameter '{nameof(gamingMachine.MachinePosition)}' must be between 0 and 1000 inclusive");

			PerformCreateOperation(gamingMachine);

			// TODO: check what new result property values are
			var x = new Result();

			return new Result();
		}

		public Result UpdateGamingMachine(GamingMachine gamingMachine)
		{
			long serialNumber = gamingMachine.SerialNumber;
			int index = GetIndexOfExistingMachine(serialNumber);

			// Machine doesn't exist
			if (index == -1)
				return new Result(ResultTypeEnum.Failure, $"A gaming machine with the serial number '{serialNumber}' could not be found");

			PerformUpdateOperation(gamingMachine, index);
			// TODO: check what new result property values are
			var x = new Result();

			return new Result();
		}

		public Result DeleteGamingMachine(GamingMachine gamingMachine)
		{
			long serialNumber = gamingMachine.SerialNumber;
			int index = GetIndexOfExistingMachine(serialNumber);

			// Machine doesn't exist
			if (index == -1)
				return new Result(ResultTypeEnum.Failure, $"A gaming machine with the serial number '{serialNumber}' could not be found");

			PerformDeleteOperation(index);

			// TODO: check what new result property values are
			var x = new Result();

			return new Result();
		}

		#region Private

		private void PerformDeleteOperation(int index)
		{
			GamingMachineDatabase.RemoveAt(index);
		}

		private void PerformUpdateOperation(GamingMachine gamingMachine, int index)
		{
			PerformDeleteOperation(index);
			PerformCreateOperation(gamingMachine);
		}

		private void PerformCreateOperation(GamingMachine gamingMachine)
		{
			gamingMachine.CreatedAt = DateTime.Now;

			GamingMachineDatabase.Add(gamingMachine);
		}

		private int GetIndexOfExistingMachine(long serialNumber)
		{
			// Don't bother using our Get method as we could run into casting issues from long to int
			// and we need to get the index of the element anyhow so this was means only searching once
			int index = GamingMachineDatabase.FindIndex(g => g.SerialNumber == serialNumber);

			return index;
		}

		private void PopulateGamingMachinesDatabase()
		{
			for (int i = 0; i <= 1000; i++)
			{
				bool deleted = i % 9 == 0;

				GamingMachine gamingMachine = new GamingMachine(i, i, $"Machine No. {i}", DateTime.Now, deleted);

				GamingMachineDatabase.Add(gamingMachine);
			}
		}

		#endregion Private
	}
}
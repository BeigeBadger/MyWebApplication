using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApplication.Repositories
{
	public interface IGamingMachineRepository
	{
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

		private void PopulateGamingMachinesDatabase()
		{
			for (int i = 0; i <= 1000; i++)
			{
				bool deleted = i % 9 == 0;

				GamingMachine gamingMachine = new GamingMachine(i, i, $"Game{i}", DateTime.Now, deleted);

				GamingMachineDatabase.Add(gamingMachine);
			}
		}

		public List<GamingMachine> GetDatabaseBackup()
		{
			return GamingMachineDatabase;
		}
	}
}
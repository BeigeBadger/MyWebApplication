using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication
{
	public interface IGamingMachine
	{
		List<GamingMachine> Get(int page = 0, int skip = 10, string filter = "");

		GamingMachine Get(int gamingSerialNumber);

		Result CreateGamingMachine(GamingMachine gamingMachine);

		Result UpdateGamingMachine(GamingMachine gamingMachine);

		Result DeleteGamingMachine(GamingMachine gamingMachine);
	}

	public class GamingMachine : IGamingMachine
	{
		/// <summary>
		/// unique number which will be used to identify a gaming machine
		/// </summary>
		[Display(Name = "Serial")]
		public long SerialNumber { get; private set; }

		/// <summary>
		/// range for this should be zero (default) to 1000 position
		/// </summary>
		[Display(Name = "Position")]
		public int MachinePosition { get; private set; } = 0;

		[Display(Name = "Name")]
		public string Name { get; private set; }

		[Display(Name = "Created at")]
		[DisplayFormat(DataFormatString = "{0:O}")]         // ISO 8601
		public DateTime CreatedAt { get; set; }

		[Display(Name = "Deleted")]
		public bool IsDeleted { get; private set; }

		public GamingMachine(long serialNumber, int position, string name, DateTime createdAt, bool isDeleted)
		{
			SerialNumber = serialNumber;
			MachinePosition = position;
			Name = name;
			CreatedAt = createdAt;
			IsDeleted = isDeleted;
		}

		public Result CreateGamingMachine(GamingMachine gamingMachine)
		{
			throw new NotImplementedException();
		}

		public Result DeleteGamingMachine(GamingMachine gamingMachine)
		{
			throw new NotImplementedException();
		}

		public List<GamingMachine> Get(int page = 0, int skip = 10, string filter = "")
		{
			throw new NotImplementedException();
		}

		public GamingMachine Get(int gamingSerialNumber)
		{
			throw new NotImplementedException();
		}

		public Result UpdateGamingMachine(GamingMachine gamingMachine)
		{
			throw new NotImplementedException();
		}
	}

	public class Result
	{
		private int ResultCode { get; set; } = 0;
		private string ResultMessage { get; set; } = "Success";
	}
}
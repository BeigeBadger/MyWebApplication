using System;
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication
{
	public class GamingMachine
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
		public DateTime CreatedAt { get; private set; }

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
	}
}
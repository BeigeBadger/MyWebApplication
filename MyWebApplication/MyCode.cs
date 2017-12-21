using System;
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication
{
	public class GamingMachine
	{
		/// <summary>
		/// unique number which will be used to identify a gaming machine
		/// </summary>
		[Required]
		[Range(0, long.MaxValue)]
		[Display(Name = "Serial")]
		public long SerialNumber { get; set; }

		/// <summary>
		/// range for this should be zero (default) to 1000 position
		/// </summary>
		[Required]
		[Range(0, 1000)]
		[Display(Name = "Position")]
		public int MachinePosition { get; set; } = 0;

		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Display(Name = "Created at")]
		[DisplayFormat(DataFormatString = "{0:O}")]         // ISO 8601
		public DateTime CreatedAt { get; set; }

		[Display(Name = "Deleted")]
		public bool IsDeleted { get; set; }

		public GamingMachine()
		{
		}

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
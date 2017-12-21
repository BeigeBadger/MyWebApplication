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
		public long SerialNumber { get; set; }

		/// <summary>
		/// range for this should be zero (default) to 1000 position
		/// </summary>
		[Required]
		[Range(0, 1000)]
		public int MachinePosition { get; set; } = 0;

		[Required]
		public string Name { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		[Required]
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
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication.ViewModels
{
	public class GamingMachineBaseViewModel
	{
		[Required]
		[Range(0, long.MaxValue)]
		public long? SerialNumber { get; set; }

		[Required]
		[Range(0, 1000)]
		[Display(Name = "Position")]
		public int? Position { get; set; } = 0;

		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }

		//[Required]
		//[Display(Name = "Created at")]
		//[DisplayFormat(DataFormatString = "{0:O}")]         // ISO 8601
		//public DateTime? CreatedAt { get; set; }

		//[Required]
		//[Display(Name = "Deleted")]
		//public bool? IsDeleted { get; set; }

		/// <summary>
		/// DO NOT CALL THIS DIRECTLY
		///
		/// For modelbinder
		/// </summary>
		protected GamingMachineBaseViewModel()
		{
		}

		/// <summary>
		/// DO NOT CALL THIS DIRECTLY
		/// </summary>
		protected GamingMachineBaseViewModel(long serial, int position, string name)
		{
			SerialNumber = serial;
			Position = position;
			Name = name;
		}
	}
}
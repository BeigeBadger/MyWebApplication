using System.ComponentModel.DataAnnotations;

namespace MyWebApplication.ViewModels
{
	/// <summary>
	/// Cannot be abstract otherwise we cannot instantiate instances of the child classes,
	/// cannot be an interface, protected or internal might work
	/// </summary>
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
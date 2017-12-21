using System;
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication.ViewModels
{
	public class GamingMachineDeleteViewModel : GamingMachineBaseViewModel
	{
		[Required]
		[Display(Name = "Created at")]
		[DisplayFormat(DataFormatString = "{0:O}")]         // ISO 8601
		public DateTime? CreatedAt { get; set; }

		/// <summary>
		/// For model binder
		/// </summary>
		public GamingMachineDeleteViewModel()
		{
		}

		public GamingMachineDeleteViewModel(long serial, int position, string name, DateTime? createdAt)
			: base(serial, position, name)
		{
			CreatedAt = createdAt;
		}
	}
}
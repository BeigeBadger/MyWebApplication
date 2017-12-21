using System;
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication.ViewModels
{
	public class GamingMachineListViewModel : GamingMachineBaseViewModel
	{
		[Required]
		[Display(Name = "Created at")]
		[DisplayFormat(DataFormatString = "{0:O}")]         // ISO 8601
		public DateTime? CreatedAt { get; set; }

		[Required]
		[Display(Name = "Deleted")]
		public bool? IsDeleted { get; set; }

		public GamingMachineListViewModel(long serial, int position, string name, DateTime? createdAt, bool? isDeleted = false)
			: base(serial, position, name)
		{
			CreatedAt = createdAt;
			IsDeleted = isDeleted;
		}
	}
}
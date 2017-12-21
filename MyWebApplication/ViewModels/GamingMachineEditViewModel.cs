namespace MyWebApplication.ViewModels
{
	public class GamingMachineEditViewModel : GamingMachineBaseViewModel
	{
		/// <summary>
		/// For model binder
		/// </summary>
		public GamingMachineEditViewModel()
		{
		}

		public GamingMachineEditViewModel(long serial, int position, string name)
			: base(serial, position, name)
		{
		}
	}
}
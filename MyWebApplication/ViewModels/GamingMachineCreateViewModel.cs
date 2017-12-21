namespace MyWebApplication.ViewModels
{
	public class GamingMachineCreateViewModel : GamingMachineBaseViewModel
	{
		/// <summary>
		/// For model binder
		/// </summary>
		public GamingMachineCreateViewModel()
		{
		}

		public GamingMachineCreateViewModel(long serial, int position, string name)
			: base(serial, position, name)
		{
		}
	}
}
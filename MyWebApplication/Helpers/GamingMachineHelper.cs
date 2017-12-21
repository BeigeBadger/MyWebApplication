using MyWebApplication.ViewModels;

namespace MyWebApplication.Helpers
{
	public static class GamingMachineHelper
	{
		/// <summary>
		/// Create a new <see cref="GamingMachine"/>
		/// with it's properties populated from the
		/// provided <see cref="GamingMachineCreateViewModel"/>
		/// </summary>
		/// <param name="createViewModel"></param>
		/// <returns>A new <see cref="GamingMachine"/></returns>
		public static GamingMachine CreateGamingMachineModelFromCreateViewModel(GamingMachineCreateViewModel createViewModel)
		{
			long serialNumber = createViewModel.SerialNumber.Value;
			int position = createViewModel.Position.Value;
			string name = createViewModel.Name;

			return new GamingMachine(serialNumber, position, name);
		}

		/// <summary>
		/// Create a new <see cref="GamingMachineEditViewModel" />
		/// with it's properties populated from the provided
		/// <see cref="GamingMachine"/>
		/// </summary>
		/// <param name="gamingMachine">The source object</param>
		/// <returns>A new <see cref="GamingMachineEditViewModel"/></returns>
		public static GamingMachineEditViewModel CreateEditViewModelFromGamingMachineModel(GamingMachine gamingMachine)
		{
			long serialNumber = gamingMachine.SerialNumber;
			int position = gamingMachine.MachinePosition;
			string name = gamingMachine.Name;

			return new GamingMachineEditViewModel(serialNumber, position, name);
		}

		/// <summary>
		/// Copy properties from edit view model onto the provided gaming machine model
		/// </summary>
		/// <param name="gamingMachine">The target object</param>
		/// <param name="editViewModel">The source object</param>
		public static void UpdateGamingMachineFromEditViewModel(GamingMachine gamingMachine, GamingMachineEditViewModel editViewModel)
		{
			gamingMachine.Name = editViewModel.Name;
			gamingMachine.MachinePosition = editViewModel.Position.Value;
		}
	}
}
using MyWebApplication.ViewModels;
using System;

namespace MyWebApplication.Helpers
{
	public static class GamingMachineHelper
	{
		/// <summary>
		/// Create a new <see cref="GamingMachine"/>
		/// from the provided <see cref="GamingMachineCreateViewModel"/>
		/// </summary>
		/// <param name="createViewModel"></param>
		/// <returns>A new <see cref="GamingMachine"/></returns>
		public static GamingMachine GetGamingMachineModelFromCreateViewModel(GamingMachineCreateViewModel createViewModel)
		{
			long serialNumber = createViewModel.SerialNumber.Value;
			int position = createViewModel.Position.Value;
			string name = createViewModel.Name;

			return new GamingMachine(serialNumber, position, name);
		}

		/// <summary>
		/// Create a new <see cref="GamingMachineEditViewModel" />
		/// from the provided <see cref="GamingMachine"/>
		/// </summary>
		/// <param name="gamingMachine">The source object</param>
		/// <returns>A new <see cref="GamingMachineEditViewModel"/></returns>
		public static GamingMachineEditViewModel GetEditViewModelFromGamingMachineModel(GamingMachine gamingMachine)
		{
			long serialNumber = gamingMachine.SerialNumber;
			int position = gamingMachine.MachinePosition;
			string name = gamingMachine.Name;

			return new GamingMachineEditViewModel(serialNumber, position, name);
		}

		/// <summary>
		/// Copy properties from a <see cref="GamingMachineEditViewModel"/> onto a <see cref="GamingMachine"/>
		/// </summary>
		/// <param name="gamingMachine">The target object</param>
		/// <param name="editViewModel">The source object</param>
		public static void UpdateGamingMachineFromEditViewModel(GamingMachine gamingMachine, GamingMachineEditViewModel editViewModel)
		{
			gamingMachine.Name = editViewModel.Name;
			gamingMachine.MachinePosition = editViewModel.Position.Value;
		}

		/// <summary>
		/// Create a new <see cref="GamingMachineDeleteViewModel"/>
		/// from the provided <see cref="GamingMachine"/>
		/// </summary>
		/// <param name="gamingMachine"></param>
		/// <returns></returns>
		public static GamingMachineDeleteViewModel GetDeleteViewModelFromGamingMachineModel(GamingMachine gamingMachine)
		{
			long serialNumber = gamingMachine.SerialNumber;
			int position = gamingMachine.MachinePosition;
			string name = gamingMachine.Name;
			DateTime createdAt = gamingMachine.CreatedAt;

			return new GamingMachineDeleteViewModel(serialNumber, position, name, createdAt);
		}
	}
}
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using MyWebApplication.App_Start;
using MyWebApplication.Repositories;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace MyWebApplication.App_Start
{
	public static class NinjectWebCommon
	{
		private static readonly Bootstrapper bootstrapper = new Bootstrapper();

		public static void Start()
		{
			DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
			DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
			bootstrapper.Initialize(CreateKernel);
		}

		public static void Stop()
		{
			bootstrapper.ShutDown();
		}

		private static IKernel CreateKernel()
		{
			var kernel = new StandardKernel();

			RegisterServices(kernel);

			return kernel;
		}

		private static void RegisterServices(IKernel kernel)
		{
			kernel.Bind<IGamingMachineRepository>().To<GamingMachineRepository>().InSingletonScope();
		}
	}
}
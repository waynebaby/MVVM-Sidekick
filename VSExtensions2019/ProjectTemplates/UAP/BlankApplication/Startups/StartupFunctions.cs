using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Services;
using $safeprojectname$;
using $safeprojectname$.ViewModels;

using System.Threading;

namespace MVVMSidekick.Startups
{
	internal static partial class StartupFunctions
	{

		#region RunAllConfigures

		static List<Action> AllConfig;
		public static Action CreateAndAddToAllConfig(this Action action)
		{
			if (AllConfig == null)
			{
				AllConfig = new List<Action>();
			}
			AllConfig.Add(action);
			return action;
		}

		static int _IsRunAllConfigCalled = 0;
		public static void RunAllConfig()
		{
			ServiceLocator.Instance.Register<ITellDesignTimeService>(new InRuntime());
			ServiceLocator.Instance.Register<IStageManager, StageManager>();

			if (AllConfig == null) return;
			if (Interlocked.Exchange(ref _IsRunAllConfigCalled, 1) == 0)
			{
				foreach (var item in AllConfig)
				{
					item();
				}
			}

		}
		#endregion

		#region MainPage

		static Action MainPageConfig =
			CreateAndAddToAllConfig(ConfigMainPage);

		public static void ConfigMainPage()
		{
			ViewModelLocator<MainPage_Model>
				.Instance
				.Register(
					context =>
						new MainPage_Model(),
					false)          //Main Page View Model Should be Singleton
				.GetViewMapper()
				.MapToDefault<MainPage>();

		}
		#endregion


	}
}

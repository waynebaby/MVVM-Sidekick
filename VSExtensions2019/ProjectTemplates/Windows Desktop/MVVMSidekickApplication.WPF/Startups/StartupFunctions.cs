using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Services;

namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
     	static List<Action> AllConfig ;

		public static Action CreateAndAddToAllConfig(this Action action)
		{
			if (AllConfig == null)
			{
				AllConfig = new List<Action>();
			}
			AllConfig.Add(action);
			return action;
		}
		public static void RunAllConfig()
		{
			ServiceLocator.Instance.Register<ITellDesignTimeService>(new InRuntime());
			ServiceLocator.Instance.Register<IStageManager, StageManager>();
			if (AllConfig==null) return;
			foreach (var item in AllConfig)
			{
				item();
			}

		}

    }
}

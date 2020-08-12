using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection ;
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
			if (AllConfig==null) return;
			foreach (var item in AllConfig)
			{
				item();
			}

		}


    }
}

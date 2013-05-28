using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace JointCharsToOne.WPF.Startups
{
    public static partial class StartupFunctions
    {

        public static void RunAllConfig()
        {
            MVVMSidekick.Services.ServiceLocator.Instance.Register<ITextToPathService>(new TextToPathService());


            typeof(StartupFunctions)
                    .GetMethods()
                    .Where(m => m.Name.StartsWith("Config") && m.IsStatic)
                    .AsParallel()
                    .ForAll(m => m.Invoke(null, Enumerable.Empty<object>().ToArray()));


        }

    }
}

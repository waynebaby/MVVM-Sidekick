using MVVMSidekick.Views;
using Samples.ViewModels;
using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Reflection;

namespace Samples.Startups
{
    public static partial class StartupFunctions
    {

        public static void RunAllConfig()
        {
            var methods =
             typeof(StartupFunctions)
#if NETFX_CORE
.GetRuntimeMethods()
#else
                .GetMethods()
#endif

.Where(m => m.Name.StartsWith("Config") && m.IsStatic);

            foreach (var m in methods)
            {
                m.Invoke(null, Enumerable.Empty<object>().ToArray());
            }
        }

        public static void ConfigOldViews()
        {
            ViewModelLocator<Calculator_Model>
                .Instance
                .Register(new Calculator_Model())
                .GetViewMapper()
                .MapToDefault<Calculator>();

#if !(NETFX_CORE||WINDOWS_PHONE_8||WINDOWS_PHONE_7)

            ViewModelLocator<Tree_Model>
                .Instance
                .Register(new Tree_Model())
                .GetViewMapper()
                .MapToDefault<Tree>();
#endif

#if ! SILVERLIGHT_5
            ViewModelLocator<Index_Model>
                .Instance
                .Register(new Index_Model())
                .GetViewMapper()
                .MapToDefault<Index>();
#else
            ViewModelLocator<Index_Model>
                .Instance
                .Register(new Index_Model())
                .GetViewMapper()
                .MapToDefaultControl<Index>();
#endif
        }


    }
}

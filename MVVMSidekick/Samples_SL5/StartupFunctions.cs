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
            typeof(StartupFunctions)
#if NETFX_CORE
.GetRuntimeMethods()
#else
                .GetMethods()
#endif

.Where(m => m.Name.StartsWith("Config") && m.IsStatic)
#if !(WINDOWS_PHONE_8||SILVERLIGHT_5||WINDOWS_PHONE_7)
.AsParallel()
                .ForAll(
#else
                .ToList ()
                .ForEach (           

#endif
m => m.Invoke(null, Enumerable.Empty<object>().ToArray()));


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

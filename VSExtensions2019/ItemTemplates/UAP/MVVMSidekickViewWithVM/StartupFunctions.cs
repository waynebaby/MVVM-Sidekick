using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using $rootnamespace$;
using $rootnamespace$.ViewModels;  
using System;
using System.Net;
using System.Windows;


namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
        static Action $safeitemname$Config =
			CreateAndAddToAllConfig(Config$safeitemname$);

        public static void Config$safeitemname$()
        {
            ViewModelLocator<$safeitemname$_Model>
                .Instance
                .Register(context=>
                    new $safeitemname$_Model())
                .GetViewMapper()
                .MapToDefault<$safeitemname$>();

        }
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        internal static Action<MVVMSidekickOptions> $safeitemname$ConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<$safeitemname$, $safeitemname$_Model>());
    }
}
}

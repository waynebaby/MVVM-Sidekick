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
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        internal Action<MVVMSidekickOptions> $safeitemname$ConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<$safeitemname$, $safeitemname$_Model>());
    }

}

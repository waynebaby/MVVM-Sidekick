using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using Samples.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace Samples.Startups
{
    public static partial class StartupFunctions
    {
        public static void ConfigCommandBindingsSample()
        {
            ViewModelLocator<CommandBindingsSample_Model>
                .Instance
                .Register(context=>
                    new CommandBindingsSample_Model())
                .GetViewMapper()
                .MapToDefault<CommandBindingsSample>(); ;

        }
    }
}

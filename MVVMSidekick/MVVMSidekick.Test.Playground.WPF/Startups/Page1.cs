using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using MVVMSidekick.Test.Playground.WPF;
using MVVMSidekick.Test.Playground.WPF.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
        static Action Page1Config =
            CreateAndAddToAllConfig(ConfigPage1);

        public static void ConfigPage1()
        {
            ViewModelLocator<Page1_Model>
                .Instance
                .Register(context =>
                    new Page1_Model())
                .GetViewMapper()
                .MapToDefault<Page1>();

        }
    }
}

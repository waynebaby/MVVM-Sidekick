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
        static Action Window1Config =
            CreateAndAddToAllConfig(ConfigWindow1);

        public static void ConfigWindow1()
        {
            ViewModelLocator<Window1_Model>
                .Instance
                .Register(context =>
                    new Window1_Model())
                .GetViewMapper()
                .MapToDefault<Window1>();

        }
    }
}

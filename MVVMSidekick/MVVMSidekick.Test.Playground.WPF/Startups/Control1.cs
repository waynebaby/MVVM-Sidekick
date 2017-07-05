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
        static Action Control1Config =
            CreateAndAddToAllConfig(ConfigControl1);

        public static void ConfigControl1()
        {
            ViewModelLocator<Control1_Model>
                .Instance
                .Register(context =>
                    new Control1_Model())
                .GetViewMapper()
                .MapToDefault<Control1>();

        }
    }
}

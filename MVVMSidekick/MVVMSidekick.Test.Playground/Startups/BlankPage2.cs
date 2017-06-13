using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using MVVMSidekick.Test.Playground;
using MVVMSidekick.Test.Playground.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
        static Action BlankPage2Config =
           CreateAndAddToAllConfig(ConfigBlankPage2);

        public static void ConfigBlankPage2()
        {
            ViewModelLocator<BlankPage2_Model>
                .Instance
                .Register(context =>
                    new BlankPage2_Model())
                .GetViewMapper()
                .MapToDefault<BlankPage2>();

        }
    }
}

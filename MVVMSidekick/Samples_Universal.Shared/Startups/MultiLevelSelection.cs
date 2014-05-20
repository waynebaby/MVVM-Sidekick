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
        public static void ConfigMultiLevelSelection()
        {
            ViewModelLocator<MultiLevelSelection_Model>
                .Instance
                .Register(_ => new MultiLevelSelection_Model())
                .GetViewMapper()
                .MapToDefault<MultiLevelSelection>();

        }
    }
}

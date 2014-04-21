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
        public static void ConfigListsAndItemsPattern()
        {
            ViewModelLocator<ListsAndItemsPattern_Model>
                .Instance
                .Register(_ => new ListsAndItemsPattern_Model())
                .GetViewMapper()
                .MapToDefault<ListsAndItemsPattern>(); ;

        }
    }
}

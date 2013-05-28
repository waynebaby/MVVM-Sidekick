using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using JointCharsToOne.WPF.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace JointCharsToOne.WPF.Startups
{
    public static partial class StartupFunctions
    {
        public static void ConfigWorkspace()
        {
            ViewModelLocator<Workspace_Model>
                .Instance
                .Register(new Workspace_Model())
                .GetViewMapper()
                .MapToDefault<Workspace>();

        }
    }
}

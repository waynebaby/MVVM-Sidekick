using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using MVVMSidekickUWPDemo;
using MVVMSidekickUWPDemo.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace MVVMSidekick.Startups
{
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        internal Action<MVVMSidekickOptions> LoginDemoConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<LoginDemo, LoginDemo_Model>());
    }

}

using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using MVVMSidekickWPFDemo;
using MVVMSidekickWPFDemo.ViewModels;
using System;
using System.Net;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace MVVMSidekick.Startups
{
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        internal Action<MVVMSidekickOptions> FetchDataConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<FetchData, FetchData_Model>());
    }
}

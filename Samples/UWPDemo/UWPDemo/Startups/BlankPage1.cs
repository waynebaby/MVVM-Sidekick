using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using UWPDemo;
using UWPDemo.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace MVVMSidekick.Startups
{
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        internal  Action<MVVMSidekickOptions> BlankPage1ConfigEntry =
            AddConfigure(opt =>
                opt.RegisterViewAndModelMapping<BlankPage1, BlankPage1_Model>());
    }

}

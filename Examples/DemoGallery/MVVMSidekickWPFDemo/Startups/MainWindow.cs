using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Services;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekickWPFDemo;
using MVVMSidekickWPFDemo.ViewModels;

namespace MVVMSidekick.Startups
{
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        internal  Action<MVVMSidekickOptions> MainWindowConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<MainWindow, MainWindow_Model>());
    }
}

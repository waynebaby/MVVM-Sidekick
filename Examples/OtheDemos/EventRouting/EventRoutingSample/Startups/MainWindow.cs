using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventRoutingSample.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick;
using MVVMSidekick.ViewModels;

namespace EventRoutingSample.Startups
{
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        internal  Action<MVVMSidekickOptions> MainWindowConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<MainWindow,MainWindow_Model>());
    }
}

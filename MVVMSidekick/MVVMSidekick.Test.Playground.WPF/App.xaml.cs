using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMSidekick.Test.Playground.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void InitNavigationConfigurationInThisAssembly()
        {
            ServiceLocator.Instance.Register<ITellDesignTimeService>(new InRuntime());
            ServiceLocator.Instance.Register<IStageManager,StageManager>();
            Startups.StartupFunctions.RunAllConfig();

            #region Debug Trace
#if DEBUG
            EventRouting.EventRouter.Instance.GetEventChannel<Exception>()
                .Subscribe(e =>
                {
                    System.Diagnostics.Debug.WriteLine($@"
==============={DateTime.Now}
EventName:
    {e.EventName}
Exception Detail:
    {e.EventData}
==============
");
                });
#endif
            #endregion
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            InitNavigationConfigurationInThisAssembly();
            base.OnStartup(e);
        }
    }
}

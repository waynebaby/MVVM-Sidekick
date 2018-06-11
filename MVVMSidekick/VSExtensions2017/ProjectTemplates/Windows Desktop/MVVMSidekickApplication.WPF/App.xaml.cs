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
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$$if$ ($targetframeworkversion$ >= 4.5)using System.Threading.Tasks;
$endif$using System.Windows;

namespace $safeprojectname$
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void InitNavigationConfigurationInThisAssembly()
		{
			MVVMSidekick.Startups.StartupFunctions.RunAllConfig();
		}

  		protected override void OnStartup(StartupEventArgs e)
		{
			InitNavigationConfigurationInThisAssembly();
            ConfigureCommandAndCommandExceptionHandler();
            base.OnStartup(e);
		}
        /// <summary>
        /// Configure event handler when command executed or exception happens
        /// </summary>
        private static void ConfigureCommandAndCommandExceptionHandler()
        {
            ////Command Firing Messages 
            //EventRouter.Instance.GetEventChannel<(EventPattern<EventCommandEventArgs> InputContext, CancellationTokenSource CancellationTokenSource)>()
            //    .ObserveOnDispatcher()
            //    .Subscribe(
            //        e =>
            //        {
            //            ////You can cancel it by:
            //            //e.EventData.CancellationTokenSource.Cancel();
            //        }
            //    );

            ////Command Executed Messages
            //EventRouter.Instance.GetEventChannel<(EventPattern<EventCommandEventArgs> InputContext, Task Task)>()
            //    .ObserveOnDispatcher()
            //    .Subscribe(
            //        e =>
            //        {
            //           
            //        }
            //    );

            //Exception Monitoring
            EventRouter.Instance.GetEventChannel<Exception>()
                .ObserveOnDispatcher()
                .Subscribe(
                    e =>
                    {
                            //Exceptions Messages 
                            if (Exceptions.Count >= 20)
                        {
                            Exceptions.RemoveAt(0);
                        }
                        Exceptions.Add(Tuple.Create(DateTime.Now, e.EventData));
                        Debug.WriteLine(e.EventData);
                    }
                );
        }
        /// <summary>
        /// Exception lists
        /// </summary>
        public static ObservableCollection<Tuple<DateTime, Exception>> Exceptions { get; set; } = new ObservableCollection<Tuple<DateTime, Exception>>();
}
}

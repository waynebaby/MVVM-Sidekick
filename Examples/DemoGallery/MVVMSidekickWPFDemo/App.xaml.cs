using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.Commands;
using MVVMSidekick.EventRouting;
using MVVMSidekick.Startups;
using MVVMSidekickWPFDemo.ViewModels;

namespace MVVMSidekickWPFDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitNavigationConfigurationInThisAssembly();
        }
        public static void InitNavigationConfigurationInThisAssembly()
        {

            ServiceCollection services = new ServiceCollection();
            services.AddMVVMSidekick(new ViewModelRegistry());
            services.BuildServiceProvider().PushToMVVMSidekickRoot();
            var s = ServiceProviderLocator.RootServiceProvider.GetRequiredService<MainWindow_Model>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

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

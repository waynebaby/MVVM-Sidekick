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
    /// App.xaml的交互逻辑，WPF应用程序的入口点
    /// Interaction logic for App.xaml, entry point for WPF application
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 初始化App的新实例，配置导航
        /// Initializes a new instance of App, configures navigation
        /// </summary>
        public App()
        {
            InitNavigationConfigurationInThisAssembly();
        }
        
        /// <summary>
        /// 初始化当前程序集中的导航配置
        /// Initializes navigation configuration in this assembly
        /// </summary>
        public static void InitNavigationConfigurationInThisAssembly()
        {

            ServiceCollection services = new ServiceCollection();
            services.AddMVVMSidekick(new ViewModelRegistry());
            services.BuildServiceProvider().PushToMVVMSidekickRoot();
            var s = ServiceProviderLocator.RootServiceProvider.GetRequiredService<MainWindow_Model>();
        }

        /// <summary>
        /// 应用程序启动时的处理
        /// Handles application startup
        /// </summary>
        /// <param name="e">启动事件参数 / Startup event arguments</param>
        protected override void OnStartup(StartupEventArgs e)
        {

            ConfigureCommandAndCommandExceptionHandler();
            base.OnStartup(e);
        }
        
        /// <summary>
        /// 配置命令执行和异常处理的事件处理器
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

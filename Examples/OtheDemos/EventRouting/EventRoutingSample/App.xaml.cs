using System.Reactive.Linq;
using System;
using System.Windows;
using MVVMSidekick.EventRouting;
using MVVMSidekick.Services;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using Microsoft.Extensions.DependencyInjection;
using EventRoutingSample.ViewModels;
using Microsoft.Extensions.DependencyInjection.Extensions;
using EventRoutingSample.Startups;

namespace EventRoutingSample
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static void InitNavigationConfigurationInThisAssembly()
		{
			ServiceCollection services = new ServiceCollection();
			services.AddMVVMSidekick<ViewModelRegistry>();
			services.BuildServiceProvider().PushToMVVMSidekickRoot();
			
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			InitNavigationConfigurationInThisAssembly();

			//Register Global Event Heartbeat Stream. Event data is string of datetime .
			//注册全局心跳事件流
			Observable.Timer(
				TimeSpan.FromSeconds(1),
				TimeSpan.FromSeconds(1), 
				System.Reactive.Concurrency.DispatcherScheduler.Current)
				.Subscribe(
					_ =>
					{
						EventRouter.Instance
							.GetEventChannel<string>()
							.RaiseEvent(
								this,
								"Global HeartBeat",
								DateTime.Now.ToString("Global Stream yyMMdd HHmmss"),
								true,
								true);
					});


		}

		private void Application_Activated(object sender, EventArgs e)
		{

		}
	}
}

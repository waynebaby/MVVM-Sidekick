using System.Reactive.Linq;
using System;
using System.Windows;
using MVVMSidekick.EventRouting;

namespace EventRoutingSample
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

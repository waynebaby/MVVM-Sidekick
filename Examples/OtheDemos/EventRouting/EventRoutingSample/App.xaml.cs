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
	/// 应用程序主类，负责初始化和配置事件路由演示
	/// Application main class responsible for initializing and configuring event routing demo
	/// </summary>
	public partial class App : Application
	{
		/// <summary>
		/// 初始化程序集中的导航配置
		/// Initializes navigation configuration in this assembly
		/// </summary>
		public static void InitNavigationConfigurationInThisAssembly()
		{
			ServiceCollection services = new ServiceCollection();
			services.AddMVVMSidekick<ViewModelRegistry>();
			services.BuildServiceProvider().PushToMVVMSidekickRoot();
			
		}

		/// <summary>
		/// 应用程序启动事件处理器，设置全局事件心跳流
		/// Application startup event handler, sets up global event heartbeat stream
		/// </summary>
		/// <param name="sender">事件发送者 / Event sender</param>
		/// <param name="e">启动事件参数 / Startup event args</param>
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

		/// <summary>
		/// 应用程序激活事件处理器
		/// Application activated event handler
		/// </summary>
		/// <param name="sender">事件发送者 / Event sender</param>
		/// <param name="e">事件参数 / Event args</param>
		private void Application_Activated(object sender, EventArgs e)
		{

		}
	}
}

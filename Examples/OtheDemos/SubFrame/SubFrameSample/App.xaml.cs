using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409

namespace SubFrameSample
{
    /// <summary>
    /// <para>提供应用程序特定的行为以补充默认的Application类</para>
    /// <para>Provides application-specific behavior to supplement the default Application class</para>
    /// </summary>
    sealed partial class App : Application
	{
		/// <summary>
		/// <para>允许通过Microsoft Application Insights服务跟踪页面视图、异常和其他遥测数据</para>
		/// <para>Allows tracking page views, exceptions and other telemetry through the Microsoft Application Insights service</para>
		/// </summary>
		public static Microsoft.ApplicationInsights.TelemetryClient TelemetryClient;

		/// <summary>
		/// <para>初始化单例应用程序对象。这是编写代码执行的第一行，因此是main()或WinMain()的逻辑等效项</para>
		/// <para>Initializes the singleton application object. This is the first line of authored code executed, and as such is the logical equivalent of main() or WinMain()</para>
		/// </summary>
		public App()
        {
			TelemetryClient = new Microsoft.ApplicationInsights.TelemetryClient();

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

		public static void InitNavigationConfigurationInThisAssembly()
		{
			MVVMSidekick.Startups.StartupFunctions.RunAllConfig();
		}
        /// <summary>
        /// <para>当应用程序由最终用户正常启动时调用。当应用程序启动以打开特定文件时，将使用其他入口点</para>
        /// <para>Invoked when the application is launched normally by the end user. Other entry points will be used such as when the application is launched to open a specific file</para>
        /// </summary>
        /// <param name="e">有关启动请求和进程的详细信息 / Details about the launch request and process</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            //Init MVVM-Sidekick Navigations:
            InitNavigationConfigurationInThisAssembly();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// <para>导航到某个页面失败时调用</para>
        /// <para>Invoked when Navigation to a certain page fails</para>
        /// </summary>
        /// <param name="sender">导航失败的Frame / The Frame which failed navigation</param>
        /// <param name="e">有关导航失败的详细信息 / Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// <para>应用程序执行暂停时调用。在不知道应用程序是否会被终止或恢复且内存内容仍完整的情况下保存应用程序状态</para>
        /// <para>Invoked when application execution is being suspended. Application state is saved without knowing whether the application will be terminated or resumed with the contents of memory still intact</para>
        /// </summary>
        /// <param name="sender">暂停请求的源 / The source of the suspend request</param>
        /// <param name="e">有关暂停请求的详细信息 / Details about the suspend request</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}

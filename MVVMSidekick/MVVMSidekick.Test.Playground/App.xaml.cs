using MVVMSidekick.Commands;
using MVVMSidekick.EventRouting;
using MVVMSidekick.Reactive;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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

namespace MVVMSidekick.Test.Playground
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }


		static bool _inited = false;
		public static void InitConfigurationInThisAssembly()
		{
			if (!_inited)
			{
				MVVMSidekick.Startups.StartupFunctions.RunAllConfig();
				ConfigureCommandAndCommandExceptionHandler();
				//You can init you Dependency Injection Here:
				//ServiceLocator.Instance.Register<IDrawingService, DrawingService>();
				_inited = true;	  
			}
		}
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
			//Init MVVM-Sidekick Navigations and Dependency Injections
			InitConfigurationInThisAssembly();

			Frame rootFrame = CreateOrSetupRootFrame(e);

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
		/// Create Or setup Root Frame
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		/// <returns>Root Frame</returns>
		private Frame CreateOrSetupRootFrame(LaunchActivatedEventArgs e)
		{
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

			return rootFrame;
		}
		/// <summary>
		/// Invoked when Navigation to a certain page fails
		/// </summary>
		/// <param name="sender">The Frame which failed navigation</param>
		/// <param name="e">Details about the navigation failure</param>
		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

		/// <summary>
		/// Configure event handler when command executed or exception happens
		/// </summary>
		private static void ConfigureCommandAndCommandExceptionHandler()
		{
            EventRouter.Instance.GetEventChannel<(EventPattern<EventCommandEventArgs> InputContext, CancellationTokenSource CancellationTokenSource)>()
                .ObserveOnDispatcher()
				.Subscribe(
					e =>
					{
                        //Command Firing Messages
                        e.EventData.CancellationTokenSource.Cancel();
					}
				);
            EventRouter.Instance.GetEventChannel<(EventPattern<EventCommandEventArgs> InputContext, Task Task)>()
                .ObserveOnDispatcher()
                .Subscribe(
                    e =>
                    {
                                    //Command Executed Messages
                    }
                );

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
		public static ObservableCollection<Tuple<DateTime,Exception>> Exceptions { get; set; } = new ObservableCollection<Tuple<DateTime, Exception>>();
}

}

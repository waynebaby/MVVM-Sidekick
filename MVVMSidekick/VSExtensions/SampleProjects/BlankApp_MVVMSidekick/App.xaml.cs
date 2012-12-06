using BlankApp_MVVMSidekick.ViewModels;
using MVVMSidekick.EventRouter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using MVVMSidekick.Views;
// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace BlankApp_MVVMSidekick
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        /// <summary>
        /// 本App的主Frame
        /// </summary>
        public static Frame MainFrame
        {
            get { return m_MainFrame; }
            set { m_MainFrame = value; }
        }
        static Frame m_MainFrame;

        /// <summary>
        /// 本App的主事件路由
        /// </summary>
        public static EventRouter MainEventRouter = EventRouter.Instance;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {

            this.InitializeComponent();
            this.Suspending += OnSuspending;


            MainFrame = Window.Current.Content as Frame;

            MainEventRouter.InitFrameNavigator(ref m_MainFrame);

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active

            MainFrame.GetFrameNavigator ().PageInitActions
                .Add(
                    typeof(MainPage),
                    (p, dic) =>
                    {
                        //  p.DefaultViewModel["Title"] = "Ok!,String Index Property Access!";
                        ((MainPage_Model)p.DefaultViewModel).Title   = "Ok!,Strong Type Property Access!";
                    }

                );

        }




        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            // Place the frame in the current Window
            Window.Current.Content = MainFrame;
            if (MainFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                var par = new Dictionary<string, object> 
                { 
                    {
                        MVVMSidekick.Views.NavigateParameterKeys.ViewInitActionName,
                        args.Arguments
                    }
                };
                if (!MainFrame.Navigate(typeof(MainPage), par))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
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
    }
}

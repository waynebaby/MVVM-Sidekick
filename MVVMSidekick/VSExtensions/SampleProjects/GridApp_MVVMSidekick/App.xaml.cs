using GridApp_MVVMSidekick.Common;
using GridApp_MVVMSidekick.Data;

using MVVMSidekick.EventRouter;
using MVVMSidekick.Views;
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

// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace GridApp_MVVMSidekick
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
            get { return App.m_MainFrame; }
            set { App.m_MainFrame = value; }
        }

        static Frame m_MainFrame;


        /// <summary>
        /// 本App的主事件路由
        /// </summary>
        public static EventRouter MainEventRouter = EventRouter.Instance;

        public const string Parameter_ViewNameKey = "ViewName";

        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            Frame rootFrame = Window.Current.Content as Frame;

            MainEventRouter.InitFrameNavigator(ref m_MainFrame);

            MVVMSidekick.Views.SuspensionManager.RegisterFrame(MainFrame, "AppFrame");

            MainFrame.GetFrameNavigator().PageInitActions.Add
            (
                typeof(GroupedItemsPage),
                (p, dic) =>
                {
                    var sampleDataGroups = SampleDataSource.GetGroups((String)dic[Parameter_ViewNameKey]);
                    p.DefaultViewModel = new GroupModel { Groups = sampleDataGroups };
                }
            );

            MainFrame.GetFrameNavigator().PageInitActions.Add
            (
                typeof(ItemDetailPage),
                (p, dic) =>
                {
                    var item = SampleDataSource.GetItem((String)dic[Parameter_ViewNameKey]);
                    //var item = SampleDataSource.GetItem((String)navigationParameter);
                    //this.DefaultViewModel["Group"] = item.Group;
                    // this.DefaultViewModel["Items"] = item.Group.Items;
                    p.DefaultViewModel = new GroupModel { Group = item.Group, Items = item.Group.Items };
                    ((ItemDetailPage)p).flipView.SelectedItem = item;
                }
            );
            MainFrame.GetFrameNavigator().PageInitActions.Add
            (
                typeof(GroupDetailPage),
                (p, dic) =>
                {
                    var group = SampleDataSource.GetGroup((String)dic[Parameter_ViewNameKey]);
                    // this.DefaultViewModel["Group"] = group;
                    //this.DefaultViewModel["Items"] = group.Items;
                    //var sampleDataGroups = SampleDataSource.GetGroups((String)dic[Parameter_ViewNameKey]);
                    p.DefaultViewModel = new GroupModel { Group = group, Items = group.Items };
                }
            );



        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {



            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active


            // Create a Frame to act as the navigation context and navigate to the first page

            //Associate the frame with a SuspensionManager key                                

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state only when appropriate
                try
                {
                    await SuspensionManager.RestoreAsync();
                }
                catch (Exception)
                {
                    //Something went wrong restoring state.
                    //Assume there is no state and continue
                }
            }

            // Place the frame in the current Window
            Window.Current.Content = MainFrame;

            if (MainFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!MainFrame.Navigate(typeof(GroupedItemsPage), new Dictionary<string, object>() { { Parameter_ViewNameKey, "AllGroups" } }))
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
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
           // await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}

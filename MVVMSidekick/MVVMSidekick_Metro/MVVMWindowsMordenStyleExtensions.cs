using MVVMSidekick.EventRouter;
using MVVMSidekick.Reactive;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
namespace MVVMSidekick
{
    namespace Storages
    {
        public class Storage<T> : BindableBase<Storage<T>>, Storages.IStorage<T> where T : new()
        {
            public Storage(string fileName = null, StorageFolder folder = null, Type[] knownTypes = null)
            {
                knownTypes = knownTypes ?? new Type[0];
                _BusyWait = new System.Threading.AutoResetEvent(true)
                    .DisposeWith(this);
                _Folder = folder ?? Windows.Storage.ApplicationData.Current.LocalFolder;
                _FileName = fileName ?? typeof(T).FullName;
                _Ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T), knownTypes);

            }

            protected string _FileName;
            private System.Runtime.Serialization.Json.DataContractJsonSerializer _Ser;



            StorageFolder _Folder;

            protected System.Threading.AutoResetEvent _BusyWait;

            protected IDisposable CreateBusyLock()
            {
                _BusyWait.WaitOne();

                var dis = System.Reactive.Disposables.Disposable.Create(() => _BusyWait.Set());

                return dis;
            }


            public virtual async Task Save()
            {
                using (CreateBusyLock())
                {
                    var folder = _Folder;
                    var file = await GetFileIfExists(folder);


                    if (file == null)
                    {
                        file = await folder.CreateFileAsync(_FileName);
                    }

                    var ms = new MemoryStream();
                    _Ser.WriteObject(ms, this.Value);
                    ms.Position = 0;
                    await Windows.Storage.FileIO.WriteBytesAsync(file, ms.ToArray());




                }

            }

            private async Task<Windows.Storage.StorageFile> GetFileIfExists(Windows.Storage.StorageFolder folder)
            {

                try
                {
                    return await _Folder.GetFileAsync(_FileName);
                }
                catch (FileNotFoundException)
                {

                    return null;
                }
            }

            public async Task Refresh()
            {
                using (CreateBusyLock())
                {
                    var folder = _Folder;
                    var file = await GetFileIfExists(folder);
                    if (file != null)
                    {
                        using (var stream = await file.OpenSequentialReadAsync())
                        {
                            var ms = new MemoryStream();
                            await stream.AsStreamForRead().CopyToAsync(ms);
                            ms.Position = 0;

                            try
                            {
                                var lst = _Ser.ReadObject(ms);

                                this.Value = (T)(lst);
                            }
                            catch (System.Runtime.Serialization.SerializationException)
                            {


                            }



                        }

                    }



                }

            }




            public T Value
            {
                get
                {

                    var vc = _ValueLocator(this);
                    var v = vc.Value;
                    //if (v == null || v.Equals(default(T)))
                    //{

                    //    vc.Value = v = new T();
                    //}

                    return v;

                }
                set { _ValueLocator(this).SetValueAndTryNotify(value); }
            }


            #region Property T Value Setup

            protected Property<T> _Value =
              new Property<T> { LocatorFunc = _ValueLocator };
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            static Func<BindableBase, ValueContainer<T>> _ValueLocator =
                RegisterContainerLocator<T>(
                "Value",
                model =>
                {
                    model._Value =
                        model._Value
                        ??
                        new Property<T> { LocatorFunc = _ValueLocator };
                    return model._Value.Container =
                        model._Value.Container
                        ??
                        new ValueContainer<T>("Value", model);
                });

            #endregion


        }



    }

    namespace ViewModels
    {


        public struct NavigateResult<TViewModel, TResult>
        {
            public Task<TViewModel> ViewModel { get; set; }
            public Task<TResult> Result { get; set; }

        }



        public partial interface IViewModelBase
        {
            FrameNavigator Navigator { get; set; }

        }
        public abstract partial class ViewModelBase<TViewModel>
        {
            public FrameNavigator Navigator { get; set; }
        }
    }

    namespace Views
    {
        public static class NavigateParameterKeys
        {
            public static readonly string ViewInitActionName = "InitAction";
            public static readonly string GameInfomation_ChosenGame = "GameInfomation_ChosenGame";
            public static readonly string NavigateToCallback = "NavigateToCallback";
            public static readonly string FinishedCallback = "FinishedCallback";
            public static readonly string AppliactionActiveArgs = "AppliactionActiveArgs";

        }

        /// <summary>
        /// Typical implementation of Page that provides several important conveniences:
        /// <list type="bullet">
        /// <item>
        /// <description>Application view state to visual state mapping</description>
        /// </item>
        /// <item>
        /// <description>GoBack, GoForward, and GoHome event handlers</description>
        /// </item>
        /// <item>
        /// <description>Mouse and keyboard shortcuts for navigation</description>
        /// </item>
        /// <item>
        /// <description>State management for navigation and process lifetime management</description>
        /// </item>
        /// <item>
        /// <description>A default view model</description>
        /// </item>
        /// </list>
        /// </summary>
        [Windows.Foundation.Metadata.WebHostHidden]
        public class LayoutAwarePage : Page
        {
            /// <summary>
            /// Identifies the <see cref="DefaultViewModel"/> dependency property.
            /// </summary>
            public static readonly DependencyProperty DefaultViewModelProperty =
                DependencyProperty.Register("DefaultViewModel", typeof(IViewModelBase),
                typeof(LayoutAwarePage), new PropertyMetadata(new DefaultViewModel()));

            private List<Control> _layoutAwareControls;

            /// <summary>
            /// Initializes a new instance of the <see cref="LayoutAwarePage"/> class.
            /// </summary>
            public LayoutAwarePage()
            {
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;

                // Create an empty default view model
                //this.DefaultViewModel = new ObservableDictionary<String, Object>();

                // When this page is part of the visual tree make two changes:
                // 1) Map application view state to visual state for the page
                // 2) Handle keyboard and mouse navigation requests
                this.Loaded += (sender, e) =>
                {
                    this.StartLayoutUpdates(sender, e);

                    // Keyboard and mouse navigation only apply when occupying the entire window
                    if (this.ActualHeight == Window.Current.Bounds.Height &&
                        this.ActualWidth == Window.Current.Bounds.Width)
                    {
                        // Listen to the window directly so focus isn't required
                        Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated +=
                            CoreDispatcher_AcceleratorKeyActivated;
                        Window.Current.CoreWindow.PointerPressed +=
                            this.CoreWindow_PointerPressed;
                    }
                };

                // Undo the same changes when the page is no longer visible
                this.Unloaded += (sender, e) =>
                {
                    this.StopLayoutUpdates(sender, e);
                    Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated -=
                        CoreDispatcher_AcceleratorKeyActivated;
                    Window.Current.CoreWindow.PointerPressed -=
                        this.CoreWindow_PointerPressed;
                };

                //var bEnable = new Binding()
                //{
                //    Source = this,
                //    Path = new PropertyPath("DefaultViewModel.IsUIBusy"),
                //    Mode = BindingMode.OneWay,
                //    Converter = BooleanNotConverter.Instance,                
                //};

                //this.SetBinding(IsEnabledProperty, bEnable);

            }



            public IViewModelBase DefaultViewModel
            {
                get
                {
                    return this.GetValue(DefaultViewModelProperty) as IViewModelBase;
                }

                set
                {
                    DisposeViewModel();
                    this.SetValue(DefaultViewModelProperty, value);
                }
            }

            protected void DisposeViewModel()
            {
                var oldv = this.GetValue(DefaultViewModelProperty) as BindableBase;
                if (oldv != null)
                {
                    try
                    {
                        oldv.Dispose();
                    }
                    catch (Exception)
                    {


                    }
                }
            }

            #region Navigation support

            /// <summary>
            /// Invoked as an event handler to navigate backward in the page's associated
            /// <see cref="Frame"/> until it reaches the top of the navigation stack.
            /// </summary>
            /// <param name="sender">Instance that triggered the event.</param>
            /// <param name="e">Event data describing the conditions that led to the event.</param>
            protected virtual void GoHome(object sender, RoutedEventArgs e)
            {
                // Use the navigation frame to return to the topmost page
                if (this.Frame != null)
                {
                    while (this.Frame.CanGoBack) this.Frame.GoBack();
                }
            }

            /// <summary>
            /// Invoked as an event handler to navigate backward in the navigation stack
            /// associated with this page's <see cref="Frame"/>.
            /// </summary>
            /// <param name="sender">Instance that triggered the event.</param>
            /// <param name="e">Event data describing the conditions that led to the
            /// event.</param>
            protected virtual void GoBack(object sender, RoutedEventArgs e)
            {
                DefaultViewModel.Close();



            }

            /// <summary>
            /// Invoked as an event handler to navigate forward in the navigation stack
            /// associated with this page's <see cref="Frame"/>.
            /// </summary>
            /// <param name="sender">Instance that triggered the event.</param>
            /// <param name="e">Event data describing the conditions that led to the
            /// event.</param>
            protected virtual void GoForward(object sender, RoutedEventArgs e)
            {
                //// Use the navigation frame to move to the next page
                //if (this.Frame != null && this.Frame.CanGoForward) this.Frame.GoForward();
            }

            /// <summary>
            /// Invoked on every keystroke, including system keys such as Alt key combinations, when
            /// this page is active and occupies the entire window.  Used to detect keyboard navigation
            /// between pages even when the page itself doesn't have focus.
            /// </summary>
            /// <param name="sender">Instance that triggered the event.</param>
            /// <param name="args">Event data describing the conditions that led to the event.</param>
            private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender,
                AcceleratorKeyEventArgs args)
            {
                var virtualKey = args.VirtualKey;

                // Only investigate further when Left, Right, or the dedicated Previous or Next keys
                // are pressed
                if ((args.EventType == CoreAcceleratorKeyEventType.SystemKeyDown ||
                    args.EventType == CoreAcceleratorKeyEventType.KeyDown) &&
                    (virtualKey == VirtualKey.Left || virtualKey == VirtualKey.Right ||
                    (int)virtualKey == 166 || (int)virtualKey == 167))
                {
                    var coreWindow = Window.Current.CoreWindow;
                    var downState = CoreVirtualKeyStates.Down;
                    bool menuKey = (coreWindow.GetKeyState(VirtualKey.Menu) & downState) == downState;
                    bool controlKey = (coreWindow.GetKeyState(VirtualKey.Control) & downState) == downState;
                    bool shiftKey = (coreWindow.GetKeyState(VirtualKey.Shift) & downState) == downState;
                    bool noModifiers = !menuKey && !controlKey && !shiftKey;
                    bool onlyAlt = menuKey && !controlKey && !shiftKey;

                    if (((int)virtualKey == 166 && noModifiers) ||
                        (virtualKey == VirtualKey.Left && onlyAlt))
                    {
                        // When the previous key or Alt+Left are pressed navigate back
                        args.Handled = true;
                        this.GoBack(this, new RoutedEventArgs());
                    }
                    else if (((int)virtualKey == 167 && noModifiers) ||
                        (virtualKey == VirtualKey.Right && onlyAlt))
                    {
                        // When the next key or Alt+Right are pressed navigate forward
                        args.Handled = true;
                        this.GoForward(this, new RoutedEventArgs());
                    }
                }
            }

            /// <summary>
            /// Invoked on every mouse click, touch screen tap, or equivalent interaction when this
            /// page is active and occupies the entire window.  Used to detect browser-style next and
            /// previous mouse button clicks to navigate between pages.
            /// </summary>
            /// <param name="sender">Instance that triggered the event.</param>
            /// <param name="args">Event data describing the conditions that led to the event.</param>
            private void CoreWindow_PointerPressed(CoreWindow sender,
                PointerEventArgs args)
            {
                var properties = args.CurrentPoint.Properties;

                // Ignore button chords with the left, right, and middle buttons
                if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed ||
                    properties.IsMiddleButtonPressed) return;

                // If back or foward are pressed (but not both) navigate appropriately
                bool backPressed = properties.IsXButton1Pressed;
                bool forwardPressed = properties.IsXButton2Pressed;
                if (backPressed ^ forwardPressed)
                {
                    args.Handled = true;
                    if (backPressed) this.GoBack(this, new RoutedEventArgs());
                    if (forwardPressed) this.GoForward(this, new RoutedEventArgs());
                }
            }

            #endregion

            #region Visual state switching

            /// <summary>
            /// Invoked as an event handler, typically on the <see cref="FrameworkElement.Loaded"/>
            /// event of a <see cref="Control"/> within the page, to indicate that the sender should
            /// start receiving visual state management changes that correspond to application view
            /// state changes.
            /// </summary>
            /// <param name="sender">Instance of <see cref="Control"/> that supports visual state
            /// management corresponding to view states.</param>
            /// <param name="e">Event data that describes how the request was made.</param>
            /// <remarks>The current view state will immediately be used to set the corresponding
            /// visual state when layout updates are requested.  A corresponding
            /// <see cref="FrameworkElement.Unloaded"/> event handler connected to
            /// <see cref="StopLayoutUpdates"/> is strongly encouraged.  Instances of
            /// <see cref="LayoutAwarePage"/> automatically invoke these handlers in their Loaded and
            /// Unloaded events.</remarks>
            /// <seealso cref="DetermineVisualState"/>
            /// <seealso cref="InvalidateVisualState"/>
            public void StartLayoutUpdates(object sender, RoutedEventArgs e)
            {
                var control = sender as Control;
                if (control == null) return;
                if (this._layoutAwareControls == null)
                {
                    // Start listening to view state changes when there are controls interested in updates
                    Window.Current.SizeChanged += this.WindowSizeChanged;
                    this._layoutAwareControls = new List<Control>();
                }
                this._layoutAwareControls.Add(control);

                // Set the initial visual state of the control
                VisualStateManager.GoToState(control, DetermineVisualState(ApplicationView.Value), false);
            }

            private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
            {
                this.InvalidateVisualState();
            }

            /// <summary>
            /// Invoked as an event handler, typically on the <see cref="FrameworkElement.Unloaded"/>
            /// event of a <see cref="Control"/>, to indicate that the sender should start receiving
            /// visual state management changes that correspond to application view state changes.
            /// </summary>
            /// <param name="sender">Instance of <see cref="Control"/> that supports visual state
            /// management corresponding to view states.</param>
            /// <param name="e">Event data that describes how the request was made.</param>
            /// <remarks>The current view state will immediately be used to set the corresponding
            /// visual state when layout updates are requested.</remarks>
            /// <seealso cref="StartLayoutUpdates"/>
            public void StopLayoutUpdates(object sender, RoutedEventArgs e)
            {
                var control = sender as Control;
                if (control == null || this._layoutAwareControls == null) return;
                this._layoutAwareControls.Remove(control);
                if (this._layoutAwareControls.Count == 0)
                {
                    // Stop listening to view state changes when no controls are interested in updates
                    this._layoutAwareControls = null;
                    Window.Current.SizeChanged -= this.WindowSizeChanged;
                }
            }

            /// <summary>
            /// Translates <see cref="ApplicationViewState"/> values into strings for visual state
            /// management within the page.  The default implementation uses the names of enum values.
            /// Subclasses may override this method to control the mapping scheme used.
            /// </summary>
            /// <param name="viewState">View state for which a visual state is desired.</param>
            /// <returns>Visual state name used to drive the
            /// <see cref="VisualStateManager"/></returns>
            /// <seealso cref="InvalidateVisualState"/>
            protected virtual string DetermineVisualState(ApplicationViewState viewState)
            {
                return viewState.ToString();
            }

            /// <summary>
            /// Updates all controls that are listening for visual state changes with the correct
            /// visual state.
            /// </summary>
            /// <remarks>
            /// Typically used in conjunction with overriding <see cref="DetermineVisualState"/> to
            /// signal that a different value may be returned even though the view state has not
            /// changed.
            /// </remarks>
            public void InvalidateVisualState()
            {
                if (this._layoutAwareControls != null)
                {
                    string visualState = DetermineVisualState(ApplicationView.Value);
                    foreach (var layoutAwareControl in this._layoutAwareControls)
                    {
                        VisualStateManager.GoToState(layoutAwareControl, visualState, false);
                    }
                }
            }

            #endregion

            #region Process lifetime management

            private String _pageKey;

            /// <summary>
            /// Invoked when this page is about to be displayed in a Frame.
            /// </summary>
            /// <param name="e">Event data that describes how this page was reached.  The Parameter
            /// property provides the group to be displayed.</param>
            protected override void OnNavigatedTo(NavigationEventArgs e)
            {
                base.OnNavigatedTo(e);
                // Returning to a cached page through navigation shouldn't trigger state loading



                if (this._pageKey != null) return;

                var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
                this._pageKey = "Page-" + this.Frame.BackStackDepth;
                var dic = e.Parameter as Dictionary<string, object>;
                if (e.NavigationMode == NavigationMode.New)
                {
                    // Clear existing state for forward navigation when adding a new page to the
                    // navigation stack
                    var nextPageKey = this._pageKey;
                    int nextPageIndex = this.Frame.BackStackDepth;
                    while (frameState.Remove(nextPageKey))
                    {
                        nextPageIndex++;
                        nextPageKey = "Page-" + nextPageIndex;
                    }






                    // Pass the navigation parameter to the new page
                    //  this.LoadState(e.Parameter, dic);
                }
                else
                {
                    // Pass the navigation parameter and preserved page state to the page, using
                    // the same strategy for loading suspended state and recreating pages discarded
                    // from cache
                    // this.LoadState(e.Parameter, (Dictionary<String, Object>)frameState[this._pageKey]);
                }

                Action<LayoutAwarePage, IDictionary<string, object>> init = null;
                if (Frame.GetFrameNavigator().PageInitActions.TryGetValue(this.GetType(), out init))
                {
                    init(this, dic);
                }
                this.DefaultViewModel.Navigator = this.Frame.GetFrameNavigator();
                object fin = null;
                if (dic != null)
                {



                    if (dic.TryGetValue(NavigateParameterKeys.FinishedCallback, out fin))
                    {
                        Action<LayoutAwarePage> finishNavCallback = fin as Action<LayoutAwarePage>;
                        DefaultViewModel.AddDisposeAction(() =>
                        {
                            finishNavCallback(this);
                            // if (this.Frame != null && this.Frame.CanGoBack) this.Frame.GoBack();
                            //GoBack(this, null);

                            DefaultViewModel.Navigator.GoBack();
                        }
                    );

                    }
                    fin = null;
                    if (dic.TryGetValue(NavigateParameterKeys.NavigateToCallback, out fin))
                    {
                        Action<LayoutAwarePage> navigateToCallback = fin as Action<LayoutAwarePage>;
                        navigateToCallback(this);
                    }
                }



            }

            /// <summary>
            /// Invoked when this page will no longer be displayed in a Frame.
            /// </summary>
            /// <param name="e">Event data that describes how this page was reached.  The Parameter
            /// property provides the group to be displayed.</param>
            protected override void OnNavigatedFrom(NavigationEventArgs e)
            {
                //var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
                var pageState = new Dictionary<string, object> { { "", DefaultViewModel } };
                this.SaveState(pageState);
                //frameState[_pageKey] = pageState;
            }

            /// <summary>
            /// Populates the page with content passed during navigation.  Any saved state is also
            /// provided when recreating a page from a prior session.
            /// </summary>
            /// <param name="navigationParameter">The parameter value passed to
            /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
            /// </param>
            /// <param name="pageState">A dictionary of state preserved by this page during an earlier
            /// session.  This will be null the first time a page is visited.</param>
            protected virtual void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
            {


            }

            /// <summary>
            /// Preserves state associated with this page in case the application is suspended or the
            /// page is discarded from the navigation cache.  Values must conform to the serialization
            /// requirements of <see cref="SuspensionManager.SessionState"/>.
            /// </summary>
            /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
            protected virtual void SaveState(Dictionary<String, Object> pageState)
            {

            }

            #endregion



            protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
            {
                //  DisposeViewModel();
                base.OnNavigatingFrom(e);

            }




            public TResult GetResult<TResult>()
            {
                if (DefaultViewModel["Result"] is TResult)
                    return (TResult)DefaultViewModel["Result"];
                else
                    return default(TResult);
            }


        }

        public class SuspensionManager
        {
            private static Dictionary<string, object> _sessionState = new Dictionary<string, object>();
            private static List<Type> _knownTypes = new List<Type>();
            private const string sessionStateFilename = "_sessionState.xml";

            /// <summary>
            /// Provides access to global session state for the current session.  This state is
            /// serialized by <see cref="SaveAsync"/> and restored by
            /// <see cref="RestoreAsync"/>, so values must be serializable by
            /// <see cref="DataContractSerializer"/> and should be as compact as possible.  Strings
            /// and other self-contained data types are strongly recommended.
            /// </summary>
            public static Dictionary<string, object> SessionState
            {
                get { return _sessionState; }
            }

            /// <summary>
            /// List of custom types provided to the <see cref="DataContractSerializer"/> when
            /// reading and writing session state.  Initially empty, additional types may be
            /// added to customize the serialization process.
            /// </summary>
            public static List<Type> KnownTypes
            {
                get { return _knownTypes; }
            }

            /// <summary>
            /// Save the current <see cref="SessionState"/>.  Any <see cref="Frame"/> instances
            /// registered with <see cref="RegisterFrame"/> will also preserve their current
            /// navigation stack, which in turn gives their active <see cref="Page"/> an opportunity
            /// to save its state.
            /// </summary>
            /// <returns>An asynchronous task that reflects when session state has been saved.</returns>
            public static async Task SaveAsync()
            {
                // Save the navigation state for all registered frames
                foreach (var weakFrameReference in _registeredFrames)
                {
                    Frame frame;
                    if (weakFrameReference.TryGetTarget(out frame))
                    {
                        SaveFrameNavigationState(frame);
                    }
                }

                // Serialize the session state synchronously to avoid asynchronous access to shared
                // state
                MemoryStream sessionData = new MemoryStream();
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>), _knownTypes);
                serializer.WriteObject(sessionData, _sessionState);

                // Get an output stream for the SessionState file and write the state asynchronously
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(sessionStateFilename, CreationCollisionOption.ReplaceExisting);
                using (Stream fileStream = await file.OpenStreamForWriteAsync())
                {
                    sessionData.Seek(0, SeekOrigin.Begin);
                    await sessionData.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }

            /// <summary>
            /// Restores previously saved <see cref="SessionState"/>.  Any <see cref="Frame"/> instances
            /// registered with <see cref="RegisterFrame"/> will also restore their prior navigation
            /// state, which in turn gives their active <see cref="Page"/> an opportunity restore its
            /// state.
            /// </summary>
            /// <returns>An asynchronous task that reflects when session state has been read.  The
            /// content of <see cref="SessionState"/> should not be relied upon until this task
            /// completes.</returns>
            public static async Task RestoreAsync()
            {
                _sessionState = new Dictionary<String, Object>();

                // Get the input stream for the SessionState file
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(sessionStateFilename);
                using (IInputStream inStream = await file.OpenSequentialReadAsync())
                {
                    // Deserialize the Session State
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>), _knownTypes);
                    _sessionState = (Dictionary<string, object>)serializer.ReadObject(inStream.AsStreamForRead());
                }

                // Restore any registered frames to their saved state
                foreach (var weakFrameReference in _registeredFrames)
                {
                    Frame frame;
                    if (weakFrameReference.TryGetTarget(out frame))
                    {
                        frame.ClearValue(FrameSessionStateProperty);
                        RestoreFrameNavigationState(frame);
                    }
                }
            }

            private static DependencyProperty FrameSessionStateKeyProperty =
                DependencyProperty.RegisterAttached("_FrameSessionStateKey", typeof(String), typeof(SuspensionManager), null);
            private static DependencyProperty FrameSessionStateProperty =
                DependencyProperty.RegisterAttached("_FrameSessionState", typeof(Dictionary<String, Object>), typeof(SuspensionManager), null);
            private static List<WeakReference<Frame>> _registeredFrames = new List<WeakReference<Frame>>();

            /// <summary>
            /// Registers a <see cref="Frame"/> instance to allow its navigation history to be saved to
            /// and restored from <see cref="SessionState"/>.  Frames should be registered once
            /// immediately after creation if they will participate in session state management.  Upon
            /// registration if state has already been restored for the specified key
            /// the navigation history will immediately be restored.  Subsequent invocations of
            /// <see cref="RestoreAsync"/> will also restore navigation history.
            /// </summary>
            /// <param name="frame">An instance whose navigation history should be managed by
            /// <see cref="SuspensionManager"/></param>
            /// <param name="sessionStateKey">A unique key into <see cref="SessionState"/> used to
            /// store navigation-related information.</param>
            public static void RegisterFrame(Frame frame, String sessionStateKey)
            {
                if (frame.GetValue(FrameSessionStateKeyProperty) != null)
                {
                    throw new InvalidOperationException("Frames can only be registered to one session state key");
                }

                if (frame.GetValue(FrameSessionStateProperty) != null)
                {
                    throw new InvalidOperationException("Frames must be either be registered before accessing frame session state, or not registered at all");
                }

                // Use a dependency property to associate the session key with a frame, and keep a list of frames whose
                // navigation state should be managed
                frame.SetValue(FrameSessionStateKeyProperty, sessionStateKey);
                _registeredFrames.Add(new WeakReference<Frame>(frame));

                // Check to see if navigation state can be restored
                RestoreFrameNavigationState(frame);
            }

            /// <summary>
            /// Disassociates a <see cref="Frame"/> previously registered by <see cref="RegisterFrame"/>
            /// from <see cref="SessionState"/>.  Any navigation state previously captured will be
            /// removed.
            /// </summary>
            /// <param name="frame">An instance whose navigation history should no longer be
            /// managed.</param>
            public static void UnregisterFrame(Frame frame)
            {
                // Remove session state and remove the frame from the list of frames whose navigation
                // state will be saved (along with any weak references that are no longer reachable)
                SessionState.Remove((String)frame.GetValue(FrameSessionStateKeyProperty));
                _registeredFrames.RemoveAll((weakFrameReference) =>
                {
                    Frame testFrame;
                    return !weakFrameReference.TryGetTarget(out testFrame) || testFrame == frame;
                });
            }

            /// <summary>
            /// Provides storage for session state associated with the specified <see cref="Frame"/>.
            /// Frames that have been previously registered with <see cref="RegisterFrame"/> have
            /// their session state saved and restored automatically as a part of the global
            /// <see cref="SessionState"/>.  Frames that are not registered have transient state
            /// that can still be useful when restoring pages that have been discarded from the
            /// navigation cache.
            /// </summary>
            /// <remarks>Apps may choose to rely on <see cref="LayoutAwarePage"/> to manage
            /// page-specific state instead of working with frame session state directly.</remarks>
            /// <param name="frame">The instance for which session state is desired.</param>
            /// <returns>A collection of state subject to the same serialization mechanism as
            /// <see cref="SessionState"/>.</returns>
            public static Dictionary<String, Object> SessionStateForFrame(Frame frame)
            {
                var frameState = (Dictionary<String, Object>)frame.GetValue(FrameSessionStateProperty);

                if (frameState == null)
                {
                    var frameSessionKey = (String)frame.GetValue(FrameSessionStateKeyProperty);
                    if (frameSessionKey != null)
                    {
                        // Registered frames reflect the corresponding session state
                        if (!_sessionState.ContainsKey(frameSessionKey))
                        {
                            _sessionState[frameSessionKey] = new Dictionary<String, Object>();
                        }
                        frameState = (Dictionary<String, Object>)_sessionState[frameSessionKey];
                    }
                    else
                    {
                        // Frames that aren't registered have transient state
                        frameState = new Dictionary<String, Object>();
                    }
                    frame.SetValue(FrameSessionStateProperty, frameState);
                }
                return frameState;
            }

            private static void RestoreFrameNavigationState(Frame frame)
            {
                var frameState = SessionStateForFrame(frame);
                if (frameState.ContainsKey("Navigation"))
                {
                    frame.SetNavigationState((String)frameState["Navigation"]);
                }
            }

            private static void SaveFrameNavigationState(Frame frame)
            {
                var frameState = SessionStateForFrame(frame);
                frameState["Navigation"] = frame.GetNavigationState();
            }
        }
        ///// <summary>
        ///// 用于控制Frame 浏览的控制器
        ///// </summary>
        //public interface IFrameNavigator
        //{
        //    ///// <summary>
        //    ///// 用async 工作流的方式浏览一个View
        //    ///// </summary>
        //    ///// <param name="targetViewName">View名</param>
        //    ///// <param name="parameters">参数</param>
        //    ///// <returns>返回Task</returns>
        //    //Task FrameNavigate(string targetViewName, System.Collections.Generic.Dictionary<string, object> parameters = null);

        //    ///// <summary>
        //    ///// 用async 工作流的方式浏览一个View 并且返回结果
        //    ///// </summary>
        //    ///// <param name="targetViewName">View名</param>
        //    ///// <param name="parameters">参数</param>
        //    ///// <returns>返回结果</returns>
        //    //Task<TResult> FrameNavigate<TResult>(string targetViewName, System.Collections.Generic.Dictionary<string, object> parameters = null);

        //    ///// <summary>
        //    ///// 用async 工作流的方式浏览一个View 返回VM和
        //    ///// </summary>
        //    ///// <param name="targetViewName">View名</param>
        //    ///// <param name="parameters">参数</param>
        //    ///// <returns>返回VM</returns>
        //    //Task<TViewModel> FrameNavigateAndGetViewModel<TViewModel>(string targetViewName, System.Collections.Generic.Dictionary<string, object> parameters = null)
        //    //    where TViewModel : IViewModelBase;



        //    ///// <summary>
        //    ///// 用async 工作流的方式浏览一个View 并且返回VM和结果
        //    ///// </summary>
        //    ///// <param name="targetViewName">View名</param>
        //    ///// <param name="parameters">参数</param>
        //    ///// <returns>返回两个Task 一个是VM一个是结果</returns>
        //    //NavigateResult<TViewModel, TResult> FrameNavigateAndGetViewModel<TViewModel, TResult>(string targetViewName, System.Collections.Generic.Dictionary<string, object> parameters = null);

        //    /// <summary>
        //    /// 页面初始化时注入逻辑
        //    /// </summary>
        //    Dictionary<Type, Action<LayoutAwarePage, IDictionary<string, object>>> PageInitActions
        //    { get; set; }

        //    /// <summary>
        //    /// 前进
        //    /// </summary>
        //    /// <returns>是否成功</returns>
        //    bool GoForward();

        //    /// <summary>
        //    /// 后退
        //    /// </summary>
        //    /// <returns>是否成功</returns>
        //    bool GoBack();

        //    #region ViewAccessor



        //    /// <summary>
        //    /// 用async 工作流的方式浏览一个View
        //    /// </summary>
        //    /// <param name="targetViewType">View类型</param>
        //    /// <param name="parameters">参数</param>
        //    /// <returns>返回Task</returns>
        //    Task FrameNavigate(Type targetViewType, IViewModelBase sourceVm, System.Collections.Generic.Dictionary<string, object> parameters = null);

        //    /// <summary>
        //    /// 用async 工作流的方式浏览一个View 并且返回结果
        //    /// </summary>
        //    /// <param name="targetViewType">View类型</param>
        //    /// <param name="parameters">参数</param>
        //    /// <returns>返回结果</returns>
        //    Task<TResult> FrameNavigate<TResult>(Type targetViewType, IViewModelBase sourceVm, System.Collections.Generic.Dictionary<string, object> parameters = null);

        //    /// <summary>
        //    /// 用async 工作流的方式浏览一个View 返回VM和
        //    /// </summary>
        //    /// <param name="targetViewType">View类型</param>
        //    /// <param name="parameters">参数</param>
        //    /// <returns>返回VM</returns>
        //    Task<TViewModel> FrameNavigateAndGetViewModel<TViewModel>(Type targetViewType, IViewModelBase sourceVm, System.Collections.Generic.Dictionary<string, object> parameters = null)
        //        where TViewModel : IViewModelBase;



        //    /// <summary>
        //    /// 用async 工作流的方式浏览一个View 并且返回VM和结果
        //    /// </summary>
        //    /// <param name="targetViewType">View类型</param>
        //    /// <param name="parameters">参数</param>
        //    /// <returns>返回两个Task 一个是VM一个是结果</returns>
        //    NavigateResult<TViewModel, TResult> FrameNavigateAndGetViewModel<TViewModel, TResult>(Type targetViewType, IViewModelBase sourceVm, System.Collections.Generic.Dictionary<string, object> parameters = null);

        //    #endregion


        //}

        /// <summary>
        /// 用于控制Frame 浏览的控制器
        /// </summary>
        public class FrameNavigator
        {
            public FrameNavigator(Frame frame, EventRouter.EventRouter eventRouter)
            {
                _Frame = frame;
                _EventRouter = eventRouter;
                this.PageInitActions = new Dictionary<Type, Action<LayoutAwarePage, IDictionary<string, object>>>();
            }


            Frame _Frame;
            EventRouter.EventRouter _EventRouter;

            void CheckParametersNull(ref  Dictionary<string, object> parameters)
            {
                if (parameters == null)
                {
                    parameters = new Dictionary<string, object>();
                }

            }

            public Task FrameNavigate(Type targetViewType, IViewModelBase sourceVm, Dictionary<string, object> parameters = null)
            {
                CheckParametersNull(ref parameters);
                var arg = CreateArgs(targetViewType, sourceVm, parameters);

                Task task = new Task(() => { });
                Action<LayoutAwarePage> finishNavigateAction =
                    page =>
                    {

                        task.Start();
                    };
                arg.ParameterDictionary[NavigateParameterKeys.FinishedCallback] = finishNavigateAction;
                _EventRouter.RaiseEvent<NavigateCommandEventArgs>(arg.ViewModel, arg);

                return task;

            }


            public Task<TResult> FrameNavigate<TResult>(Type targetViewType, IViewModelBase sourceVm, Dictionary<string, object> parameters = null)
            {
                CheckParametersNull(ref parameters);

                var arg = CreateArgs(targetViewType, sourceVm, parameters);

                TResult result = default(TResult);

                Task<TResult> taskR = new Task<TResult>(() => result);
                Action<LayoutAwarePage> finishNavigateAction =
                    page =>
                    {
                        result = page.GetResult<TResult>();
                        taskR.Start();
                    };
                arg.ParameterDictionary[NavigateParameterKeys.FinishedCallback] = finishNavigateAction;



                _EventRouter.RaiseEvent<NavigateCommandEventArgs>(arg.ViewModel, arg);

                return taskR;

            }


            public Task<TViewModel> FrameNavigateAndGetViewModel<TViewModel>(Type targetViewType, IViewModelBase sourceVm, Dictionary<string, object> parameters = null) where TViewModel : IViewModelBase
            {
                CheckParametersNull(ref parameters);

                var arg = CreateArgs(targetViewType, sourceVm, parameters);


                TViewModel viewModel = default(TViewModel);


                Task<TViewModel> taskVm = new Task<TViewModel>(() => viewModel);
                Action<LayoutAwarePage> navigateToAction =
                    page =>
                    {
                        viewModel = (TViewModel)page.DefaultViewModel;
                        taskVm.Start();
                    };
                arg.ParameterDictionary[NavigateParameterKeys.NavigateToCallback] = navigateToAction;




                _EventRouter.RaiseEvent<NavigateCommandEventArgs>(arg.ViewModel, arg);
                return taskVm;
            }


            public NavigateResult<TViewModel, TResult> FrameNavigateAndGetViewModel<TViewModel, TResult>(Type targetViewType, IViewModelBase sourceVm, Dictionary<string, object> parameters = null)
            {
                CheckParametersNull(ref parameters);
                var arg = CreateArgs(targetViewType, sourceVm, parameters);


                TViewModel viewModel = default(TViewModel);
                Task<TViewModel> taskVm = new Task<TViewModel>(() => viewModel);
                Action<LayoutAwarePage> navigateToAction =
                    page =>
                    {
                        viewModel = (TViewModel)page.DefaultViewModel;
                        taskVm.Start();
                    };
                arg.ParameterDictionary[NavigateParameterKeys.NavigateToCallback] = navigateToAction;


                TResult result = default(TResult);
                Task<TResult> taskR = new Task<TResult>(() => result);
                Action<LayoutAwarePage> finishNavigateAction =
                    page =>
                    {
                        result = page.GetResult<TResult>();
                        taskR.Start();
                    };
                arg.ParameterDictionary[NavigateParameterKeys.FinishedCallback] = finishNavigateAction;



                _EventRouter.RaiseEvent<NavigateCommandEventArgs>(arg.ViewModel, arg);
                return new NavigateResult<TViewModel, TResult>
                {
                    ViewModel = taskVm,
                    Result = taskR
                };
            }


            //public Task FrameNavigate(string targetViewName, IViewModelBase sourceVm, Dictionary<string, object> parameters = null)
            //{
            //    CheckParametersNull(ref parameters);
            //    var arg = CreateArgs(targetViewName, sourceVm, parameters);

            //    Task task = new Task(() => { });
            //    Action<LayoutAwarePage> finishNavigateAction =
            //        page =>
            //        {

            //            task.Start();
            //        };
            //    arg.ParameterDictionary[NavigateParameterKeys.FinishedCallback] = finishNavigateAction;
            //    _EventRouter.RaiseEvent<NavigateCommandEventArgs>(arg.ViewModel, arg);

            //    return task;

            //}


            //public Task<TResult> FrameNavigate<TResult>(string targetViewName, IViewModelBase sourceVm, Dictionary<string, object> parameters = null)
            //{
            //    CheckParametersNull(ref parameters);

            //    var arg = CreateArgs(targetViewName, sourceVm, parameters);

            //    TResult result = default(TResult);

            //    Task<TResult> taskR = new Task<TResult>(() => result);
            //    Action<LayoutAwarePage> finishNavigateAction =
            //        page =>
            //        {
            //            result = page.GetResult<TResult>();
            //            taskR.Start();
            //        };
            //    arg.ParameterDictionary[NavigateParameterKeys.FinishedCallback] = finishNavigateAction;



            //    _EventRouter.RaiseEvent<NavigateCommandEventArgs>(arg.ViewModel, arg);

            //    return taskR;

            //}


            //public Task<TViewModel> FrameNavigateAndGetViewModel<TViewModel>(string targetViewName, IViewModelBase sourceVm, Dictionary<string, object> parameters = null) where TViewModel : IViewModelBase
            //{
            //    CheckParametersNull(ref parameters);

            //    var arg = CreateArgs(targetViewName, sourceVm, parameters);


            //    TViewModel viewModel = default(TViewModel);


            //    Task<TViewModel> taskVm = new Task<TViewModel>(() => viewModel);
            //    Action<LayoutAwarePage> navigateToAction =
            //        page =>
            //        {
            //            viewModel = (TViewModel)page.DefaultViewModel;
            //            taskVm.Start();
            //        };
            //    arg.ParameterDictionary[NavigateParameterKeys.NavigateToCallback] = navigateToAction;




            //    _EventRouter.RaiseEvent<NavigateCommandEventArgs>(arg.ViewModel, arg);
            //    return taskVm;
            //}


            //public NavigateResult<TViewModel, TResult> FrameNavigateAndGetViewModel<TViewModel, TResult>(string targetViewName, IViewModelBase sourceVm, Dictionary<string, object> parameters = null)
            //{
            //    CheckParametersNull(ref parameters);
            //    var arg = CreateArgs(targetViewName, sourceVm, parameters);


            //    TViewModel viewModel = default(TViewModel);
            //    Task<TViewModel> taskVm = new Task<TViewModel>(() => viewModel);
            //    Action<LayoutAwarePage> navigateToAction =
            //        page =>
            //        {
            //            viewModel = (TViewModel)page.DefaultViewModel;
            //            taskVm.Start();
            //        };
            //    arg.ParameterDictionary[NavigateParameterKeys.NavigateToCallback] = navigateToAction;


            //    TResult result = default(TResult);
            //    Task<TResult> taskR = new Task<TResult>(() => result);
            //    Action<LayoutAwarePage> finishNavigateAction =
            //        page =>
            //        {
            //            result = page.GetResult<TResult>();
            //            taskR.Start();
            //        };
            //    arg.ParameterDictionary[NavigateParameterKeys.FinishedCallback] = finishNavigateAction;



            //    _EventRouter.RaiseEvent<NavigateCommandEventArgs>(arg.ViewModel, arg);
            //    return new NavigateResult<TViewModel, TResult>
            //    {
            //        ViewModel = taskVm,
            //        Result = taskR
            //    };
            //}




            //private NavigateCommandEventArgs CreateArgs(string viewName, IViewModelBase sourceVm, Dictionary<string, object> parameters)
            //{
            //    var arg = new NavigateCommandEventArgs()
            //    {
            //        ParameterDictionary = parameters,
            //        TargetViewType =  ,
            //        TargetFrame = _Frame,
            //        ViewModel = sourceVm
            //    };
            //    return arg;
            //}
            private NavigateCommandEventArgs CreateArgs(Type viewType, IViewModelBase sourceVm, Dictionary<string, object> parameters)
            {
                if (sourceVm == null)
                {
                    throw new ArgumentNullException("sourceVm cannot be null");
                }
                var arg = new NavigateCommandEventArgs()
                {
                    ParameterDictionary = parameters,
                    TargetViewType = viewType,
                    TargetFrame = _Frame,
                    ViewModel = sourceVm
                };
                return arg;
            }



            public bool GoForward()
            {
                var can = _Frame.CanGoForward;
                if (can)
                {
                    _Frame.GoForward();
                }
                return can;
            }

            public bool GoBack()
            {
                var can = _Frame.CanGoBack;
                if (can)
                {
                    _Frame.GoBack();
                }
                return can;
            }






            public Dictionary<Type, Action<LayoutAwarePage, IDictionary<string, object>>> PageInitActions
            {
                get;
                set;
            }
        }

        public static class NavigationHelper
        {

            public static void InitFrameNavigator(this EventRouter.EventRouter router, ref   Frame frame)
            {
                FrameNavigator fn;
                if (frame == null)
                {
                    frame = new Frame();
                }

                if ((fn = frame.GetFrameNavigator()) == null)
                {
                    fn = new FrameNavigator(frame, router);
                    frame.SetFrameNavigator(fn);

                }


                var noneedDispose = router.GetEventObject<NavigateCommandEventArgs>().GetRouterEventObservable()
                   .Where(
                   a =>
                   {
                       var vm = a.Sender as ViewModels.IViewModelBase;
                       if (vm == null)
                       {
                           return false;
                       }
                       return vm.Navigator == fn;
                   })
                .Subscribe(
                  async ep =>
                  {
                      await Task.Delay(100);
                      ((Frame)ep.EventArgs.TargetFrame).Navigate(ep.EventArgs.TargetViewType, ep.EventArgs.ParameterDictionary);
                  }
                );


            }


            public static FrameNavigator GetFrameNavigator(this Frame obj)
            {
                return (FrameNavigator)obj.GetValue(FrameNavigatorProperty);
            }

            public static void SetFrameNavigator(this Frame obj, FrameNavigator value)
            {
                obj.SetValue(FrameNavigatorProperty, value);
            }

            // Using a DependencyProperty as the backing store for FrameNavigator.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty FrameNavigatorProperty =
                DependencyProperty.RegisterAttached("FrameNavigator", typeof(FrameNavigator), typeof(Frame), new PropertyMetadata(null));


        }



    }

    namespace Commands
    {


        public class CommandBinderParameter
        {
            public DependencyObject SourceObject { get; set; }
            public Object Paremeter { get; set; }
            public string EventName { get; set; }
            public object EventArgs { get; set; }

        }

        public static class TypeEventHelper
        {
            private static MethodInfo AddEventHandlerMethodInfo = typeof(TypeEventHelper)
                .GetRuntimeMethods().Where(x => x.Name == "AddEventHandler").First();

            public static void AddEventHandlerByType(
                this FrameworkElement target,
                Type eventHandlerType,
                EventInfo eventInfo,
                Action<object, object> handler)
            {
                var eventArgsHandlerType = eventHandlerType
                    .GetRuntimeMethods()
                    .Where(x => x.Name == "Invoke")
                    .First()
                    .GetParameters()
                    .Last()
                    .ParameterType;

                AddEventHandlerMethodInfo
                    .MakeGenericMethod(
                        eventHandlerType,
                        eventArgsHandlerType)
                    .Invoke(
                        null,
                        new Object[] { target, eventInfo, handler });
            }
            public static void AddEventHandler<THandler, TEventArgs>(
                this FrameworkElement target,
                EventInfo eventInfo,
                Action<object, object> handler)
                where THandler : class
            {

                Expression<Action<object, object>> bind = (o, e) => handler(o, e);
                var compiled = CreateHandler<THandler, TEventArgs>(bind);

                WindowsRuntimeMarshal.AddEventHandler<THandler>(
                    ev => (EventRegistrationToken)eventInfo.AddMethod.Invoke(target, new object[] { ev }),
                    et => eventInfo.RemoveMethod.Invoke(target, new object[] { et }),
                    compiled);

            }

            private static THandler CreateHandler<THandler, TEventArgs>(
                Expression<Action<object, object>> bind)
                where THandler : class
            {
                var par1 = Expression.Parameter(typeof(Object));
                var par2 = Expression.Parameter(typeof(TEventArgs));
                var expr = Expression.Invoke(bind, par1, par2);
                var lambda = Expression.Lambda<THandler>(expr, par1, par2);
                var compiled = lambda.Compile();
                return compiled;
            }

            public readonly static Dictionary<Type, Dictionary<string, EventInfo>> TypeEventDic
                    = new Dictionary<Type, Dictionary<string, EventInfo>>();

            public static Dictionary<string, EventInfo> GetOrCreateEventCache(this Type t)
            {
                Dictionary<string, EventInfo> es;
                if (!TypeEventDic.TryGetValue(t, out es))
                {
                    es = t.GetRuntimeEvents()
                        .ToDictionary(x => x.Name, x => x);
                    TypeEventDic[t] = es;
                }
                return es;
            }
        }
        public class CommandBinder : DependencyObject
        {


            public ICommand Command
            {
                get
                {
                    return (ICommand)GetValue(CommandProperty);
                }
                set
                {
                    SetValue(CommandProperty, value);
                }
            }

            // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty CommandProperty =
                DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandBinder), new PropertyMetadata(null));




            public Object Parameter
            {
                get { return (Object)GetValue(ParameterProperty); }
                set { SetValue(ParameterProperty, value); }
            }

            // Using a DependencyProperty as the backing store for Parameter.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty ParameterProperty =
                DependencyProperty.Register("Parameter", typeof(Object), typeof(CommandBinder), new PropertyMetadata(null));



            public string EventName
            {
                get { return (string)GetValue(EventNameProperty); }
                set { SetValue(EventNameProperty, value); }
            }

            // Using a DependencyProperty as the backing store for EventName.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty EventNameProperty =
                DependencyProperty.Register("EventName", typeof(string), typeof(CommandBinder), new PropertyMetadata(""));



            public static CommandBinder GetCommandBinder(DependencyObject obj)
            {
                return (CommandBinder)obj.GetValue(CommandBinderProperty);
            }

            public static void SetCommandBinder(DependencyObject obj, CommandBinder value)
            {
                obj.SetValue(CommandBinderProperty, value);
            }

            // Using a DependencyProperty as the backing store for CommandBinder.  This enables animation, styling, binding, etc...


            public static readonly DependencyProperty CommandBinderProperty =
                DependencyProperty.RegisterAttached("CommandBinder", typeof(CommandBinder), typeof(DependencyObject), new PropertyMetadata(
                    null,
                        (d, v) =>
                        {

                            TryBind(d as FrameworkElement);
                        }
                    ));


            public static void TryBind(FrameworkElement element)
            {
                var t = element.GetType();
                var cb = (CommandBinder)element.GetValue(CommandBinderProperty);
                while (t != null)
                {
                    Dictionary<string, EventInfo> es;
                    es = t.GetOrCreateEventCache();
                    EventInfo eventInfo;
                    if (es.TryGetValue(cb.EventName, out eventInfo))
                    {

                        var handlerType = eventInfo.EventHandlerType;



                        element.AddEventHandlerByType(handlerType, eventInfo,
                            (o, e) =>
                            {
                                var cmd = ((ICommand)cb.GetValue(CommandProperty));
                                cmd.Execute(new CommandBinderParameter { EventArgs = e, EventName = cb.EventName, Paremeter = cb.Parameter, SourceObject = element });
                            }
                            );

                        return;
                    }
                    t = t.GetTypeInfo().BaseType;
                }



            }


        }

    }

    namespace Common
    {
        [DataContract]
        public class ObservableDictionary<K, V> : IObservableMap<K, V>
        {

            public ObservableDictionary(IDictionary<K, V> coreDic)
            {
                _dictionary = coreDic;
            }
            private class ObservableDictionaryChangedEventArgs : IMapChangedEventArgs<K>
            {
                public ObservableDictionaryChangedEventArgs(CollectionChange change, K key)
                {
                    this.CollectionChange = change;
                    this.Key = key;
                }

                public CollectionChange CollectionChange { get; private set; }
                public K Key { get; private set; }
            }


            private IDictionary<K, V> _dictionary = new Dictionary<K, V>();
            [DataMember]
            public IDictionary<K, V> Dictionary
            {
                get { return _dictionary; }
                set { _dictionary = value; }
            }
            public event MapChangedEventHandler<K, V> MapChanged;

            private void InvokeMapChanged(CollectionChange change, K key)
            {
                var eventHandler = MapChanged;
                if (eventHandler != null)
                {
                    eventHandler(this, new ObservableDictionaryChangedEventArgs(CollectionChange.ItemInserted, key));
                }
            }

            public void Add(K key, V value)
            {
                this._dictionary.Add(key, value);
                this.InvokeMapChanged(CollectionChange.ItemInserted, key);
            }

            public void Add(KeyValuePair<K, V> item)
            {
                this.Add(item.Key, item.Value);
            }

            public bool Remove(K key)
            {
                if (this._dictionary.Remove(key))
                {
                    this.InvokeMapChanged(CollectionChange.ItemRemoved, key);
                    return true;
                }
                return false;
            }

            public bool Remove(KeyValuePair<K, V> item)
            {
                V currentValue;
                if (this._dictionary.TryGetValue(item.Key, out currentValue) &&
                    Object.Equals(item.Value, currentValue) && this._dictionary.Remove(item.Key))
                {
                    this.InvokeMapChanged(CollectionChange.ItemRemoved, item.Key);
                    return true;
                }
                return false;
            }

            public V this[K key]
            {
                get
                {
                    return this._dictionary[key];
                }
                set
                {
                    this._dictionary[key] = value;
                    this.InvokeMapChanged(CollectionChange.ItemChanged, key);
                }
            }

            public void Clear()
            {
                var priorKeys = this._dictionary.Keys.ToArray();
                this._dictionary.Clear();
                foreach (var key in priorKeys)
                {
                    this.InvokeMapChanged(CollectionChange.ItemRemoved, key);
                }
            }

            public ICollection<K> Keys
            {
                get { return this._dictionary.Keys; }
            }

            public bool ContainsKey(K key)
            {
                return this._dictionary.ContainsKey(key);
            }

            public bool TryGetValue(K key, out V value)
            {
                return this._dictionary.TryGetValue(key, out value);
            }

            public ICollection<V> Values
            {
                get { return this._dictionary.Values; }
            }

            public bool Contains(KeyValuePair<K, V> item)
            {
                return this._dictionary.Contains(item);
            }

            public int Count
            {
                get { return this._dictionary.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
            {
                return this._dictionary.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this._dictionary.GetEnumerator();
            }

            public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
            {
                int arraySize = array.Length;
                foreach (var pair in this._dictionary)
                {
                    if (arrayIndex >= arraySize) break;
                    array[arrayIndex++] = pair;
                }
            }
        }

    }


    namespace ValueConverters
    {
        /// <summary>
        /// Value converter that translates true to false and vice versa.
        /// </summary>
        public sealed class BooleanNegationConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, string language)
            {
                return !(value is bool && (bool)value);
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                return !(value is bool && (bool)value);
            }
        }

        public sealed class StringDoubleFormatConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, string language)
            {
                string arg = System.Convert.ToString(value);

                return string.Format((string)parameter, arg);
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                return double.Parse(value.ToString());
            }
        }

        /// <summary>
        /// Value converter that translates true to false and vice versa.
        /// </summary>
        public sealed class DoubleStringConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, string language)
            {
                return double.Parse(value.ToString());
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                return ((double)value).ToString("#.##");
            }
        }

        /// <summary>
        /// Value converter that translates true to <see cref="Visibility.Visible"/> and false to
        /// <see cref="Visibility.Collapsed"/>.
        /// </summary>
        public sealed class BooleanNotConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, string language)
            {
                return !(bool)value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                return !(bool)value;
            }

            public static BooleanNotConverter Instance = new BooleanNotConverter();
        }

        /// <summary>
        /// Value converter that translates true to <see cref="Visibility.Visible"/> and false to
        /// <see cref="Visibility.Collapsed"/>.
        /// </summary>
        public sealed class BooleanToVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, string language)
            {
                return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                return value is Visibility && (Visibility)value == Visibility.Visible;
            }
        }

    }
}

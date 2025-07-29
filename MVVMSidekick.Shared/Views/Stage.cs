#if !BLAZOR

/// <summary>
/// <para>Stage类 - 负责管理视图显示和导航的核心类</para>
/// <para>Stage class - Core class responsible for managing view display and navigation</para>
/// </summary>
/// <remarks>
/// <para>Stage类继承自DependencyObject并实现IStage接口，提供视图和视图模型的显示管理功能。</para>
/// <para>这个类支持多种平台的导航模式，包括Frame导航、窗口显示、内容控件显示等。</para>
/// <para>Stage class inherits from DependencyObject and implements IStage interface, providing view and view model display management functionality.</para>
/// <para>This class supports multiple platform navigation modes including Frame navigation, window display, content control display, etc.</para>
/// </remarks>

namespace MVVMSidekick.Views
{
    /// <summary>
    /// <para>Stage类 - MVVM-Sidekick框架中的视图舞台管理器</para>
    /// <para>Stage class - View stage manager in MVVM-Sidekick framework</para>
    /// </summary>
    /// <remarks>
    /// <para>Stage类负责管理视图的生命周期、导航和显示。它支持多种目标控件类型，如Frame、ContentControl、Panel等。</para>
    /// <para>这个类根据不同的编译条件（WPF、UWP、WinUI3）提供相应的平台特定功能。</para>
    /// <para>Stage class manages the lifecycle, navigation and display of views. It supports various target control types such as Frame, ContentControl, Panel, etc.</para>
    /// <para>This class provides corresponding platform-specific functionality based on different compilation conditions (WPF, UWP, WinUI3).</para>
    /// </remarks>
    public class Stage : DependencyObject, IStage
    {
        /// <summary>
        /// <para>初始化Stage类的新实例</para>
        /// <para>Initializes a new instance of the Stage class</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>服务提供程序，用于解析依赖项和视图模型</para>
        /// <para>Service provider for resolving dependencies and view models</para>
        /// </param>
        /// <remarks>
        /// <para>构造函数只设置服务提供程序，其他属性需要通过InitStage方法进行初始化。</para>
        /// <para>Constructor only sets the service provider, other properties need to be initialized through InitStage method.</para>
        /// </remarks>
        public Stage(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// <para>初始化Stage的目标、信标键和舞台管理器</para>
        /// <para>Initialize Stage's target, beacon key and stage manager</para>
        /// </summary>
        /// <param name="target">
        /// <para>目标框架元素，作为视图显示的容器</para>
        /// <para>Target framework element that serves as container for view display</para>
        /// </param>
        /// <param name="beaconKey">
        /// <para>信标键，用于标识此Stage实例</para>
        /// <para>Beacon key for identifying this Stage instance</para>
        /// </param>
        /// <param name="stageManager">
        /// <para>舞台管理器，负责管理多个Stage实例</para>
        /// <para>Stage manager responsible for managing multiple Stage instances</para>
        /// </param>
        public void InitStage(FrameworkElement target, string beaconKey, StageManager stageManager)
        {
            Target = target;
            _stageManager = stageManager;
            BeaconKey = beaconKey;
        }

        /// <summary>
        /// <para>私有舞台管理器字段</para>
        /// <para>Private stage manager field</para>
        /// </summary>
        private StageManager _stageManager;

        /// <summary>
        /// <para>私有目标框架元素字段</para>
        /// <para>Private target framework element field</para>
        /// </summary>
        private FrameworkElement _target;




        /// <summary>
        /// <para>获取关联的Frame控件</para>
        /// <para>Gets the associated Frame control</para>
        /// </summary>
        /// <value>
        /// <para>Frame控件实例，用于页面导航</para>
        /// <para>Frame control instance for page navigation</para>
        /// </value>
        /// <remarks>
        /// <para>此属性使用依赖属性实现，支持数据绑定、动画等WPF/UWP功能。</para>
        /// <para>This property is implemented using dependency property, supporting WPF/UWP features like data binding, animation, etc.</para>
        /// </remarks>
        public Frame Frame
        {
            get => (Frame)GetValue(FrameProperty);
            private set => SetValue(FrameProperty, value);
        }

        /// <summary>
        /// <para>Frame属性的依赖属性标识符</para>
        /// <para>Dependency property identifier for Frame property</para>
        /// </summary>
        /// <remarks>
        /// <para>使用依赖属性作为Frame的后备存储，支持动画、样式、绑定等功能。</para>
        /// <para>Uses dependency property as backing store for Frame, enabling animation, styling, binding, etc.</para>
        /// </remarks>
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register("Frame", typeof(Frame), typeof(Stage), new PropertyMetadata(null));



        /// <summary>
        /// <para>获取目标控件对象</para>
        /// <para>Gets the target control object</para>
        /// </summary>
        /// <value>
        /// <para>目标对象，通常是FrameworkElement类型的控件</para>
        /// <para>Target object, typically a control of FrameworkElement type</para>
        /// </value>
        /// <remarks>
        /// <para>此属性设置时会自动转换为FrameworkElement类型。如果转换失败，则为null。</para>
        /// <para>This property automatically converts to FrameworkElement type when set. If conversion fails, it will be null.</para>
        /// </remarks>
        public Object Target
        {
            get => _target;
            private set
            {
                _target = value as FrameworkElement;



            }
        }
#if WPF
        /// <summary>
        /// <para>获取一个值，指示是否支持前进导航</para>
        /// <para>Gets a value indicating whether forward navigation is supported</para>
        /// </summary>
        /// <value>
        /// <para>如果Frame不为空则支持前进导航，否则不支持</para>
        /// <para>Returns true if Frame is not null (forward navigation supported), otherwise false</para>
        /// </value>
        /// <remarks>
        /// <para>此属性仅在WPF平台下可用，用于判断当前是否具备前进导航的能力。</para>
        /// <para>This property is only available on WPF platform, used to determine if forward navigation capability exists.</para>
        /// </remarks>
        public bool IsGoForwardSupported
        {
            get
            {
                return Frame != null;
            }
        }

        /// <summary>
        /// <para>获取一个值，指示是否可以前进</para>
        /// <para>Gets a value indicating whether it can go forward</para>
        /// </summary>
        /// <value>
        /// <para>如果支持前进导航且Frame可以前进，则返回true；否则返回false</para>
        /// <para>Returns true if forward navigation is supported and Frame can go forward; otherwise false</para>
        /// </value>
        /// <remarks>
        /// <para>此属性依赖于Frame控件的CanGoForward属性，仅在WPF平台下可用。</para>
        /// <para>This property depends on Frame control's CanGoForward property, only available on WPF platform.</para>
        /// </remarks>
        public bool CanGoForward
        {
            get
            {
                return IsGoForwardSupported ? Frame.CanGoForward : false;
            }

        }
#elif WINDOWS_UWP || WinUI3

        /// <summary>
        /// <para>获取一个值，指示是否支持前进导航（UWP/WinUI3平台）</para>
        /// <para>Gets a value indicating whether forward navigation is supported (UWP/WinUI3 platform)</para>
        /// </summary>
        /// <value>
        /// <para>在UWP/WinUI3平台下始终返回false</para>
        /// <para>Always returns false on UWP/WinUI3 platform</para>
        /// </value>
        /// <remarks>
        /// <para>UWP和WinUI3平台不支持前进导航功能。</para>
        /// <para>UWP and WinUI3 platforms do not support forward navigation functionality.</para>
        /// </remarks>
        public bool IsGoForwardSupported => false;

        /// <summary>
        /// <para>获取一个值，指示是否可以前进（UWP/WinUI3平台）</para>
        /// <para>Gets a value indicating whether it can go forward (UWP/WinUI3 platform)</para>
        /// </summary>
        /// <value>
        /// <para>在UWP/WinUI3平台下始终返回false</para>
        /// <para>Always returns false on UWP/WinUI3 platform</para>
        /// </value>
        /// <remarks>
        /// <para>UWP和WinUI3平台不支持前进导航功能。</para>
        /// <para>UWP and WinUI3 platforms do not support forward navigation functionality.</para>
        /// </remarks>
        public bool CanGoForward => false;
#endif

        /// <summary>
        /// <para>获取一个值，指示是否支持后退导航</para>
        /// <para>Gets a value indicating whether back navigation is supported</para>
        /// </summary>
        /// <value>
        /// <para>如果Frame不为空则支持后退导航，否则不支持</para>
        /// <para>Returns true if Frame is not null (back navigation supported), otherwise false</para>
        /// </value>
        /// <remarks>
        /// <para>此属性用于判断当前是否具备后退导航的能力，适用于所有支持的平台。</para>
        /// <para>This property is used to determine if back navigation capability exists, applicable to all supported platforms.</para>
        /// </remarks>
        public bool IsGoBackSupported => Frame != null;


        /// <summary>
        /// <para>获取一个值，指示是否可以后退</para>
        /// <para>Gets a value indicating whether it can go back</para>
        /// </summary>
        /// <value>
        /// <para>如果支持后退导航且Frame可以后退，则返回true；否则返回false</para>
        /// <para>Returns true if back navigation is supported and Frame can go back; otherwise false</para>
        /// </value>
        /// <remarks>
        /// <para>此属性依赖于Frame控件的CanGoBack属性，适用于所有支持的平台。</para>
        /// <para>This property depends on Frame control's CanGoBack property, applicable to all supported platforms.</para>
        /// </remarks>
        public bool CanGoBack => IsGoBackSupported ? Frame.CanGoBack : false;



        /// <summary>
        /// <para>获取信标键</para>
        /// <para>Gets the beacon key</para>
        /// </summary>
        /// <value>
        /// <para>用于标识此Stage实例的字符串键</para>
        /// <para>String key used to identify this Stage instance</para>
        /// </value>
        /// <remarks>
        /// <para>信标键用于在舞台管理器中唯一标识此Stage实例，支持通过键值查找特定的Stage。</para>
        /// <para>Beacon key is used to uniquely identify this Stage instance in stage manager, supports finding specific Stage by key value.</para>
        /// </remarks>
        public string BeaconKey
        {
            get => (string)GetValue(BeaconKeyProperty);
            private set => SetValue(BeaconKeyProperty, value);
        }

        /// <summary>
        /// <para>服务提供程序接口</para>
        /// <para>Service provider interface</para>
        /// </summary>
        /// <value>
        /// <para>用于解析依赖项和服务的服务提供程序</para>
        /// <para>Service provider for resolving dependencies and services</para>
        /// </value>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// <para>BeaconKey属性的依赖属性标识符</para>
        /// <para>Dependency property identifier for BeaconKey property</para>
        /// </summary>
        /// <remarks>
        /// <para>使用依赖属性作为BeaconKey的后备存储，支持动画、样式、绑定等功能。</para>
        /// <para>Uses dependency property as backing store for BeaconKey, enabling animation, styling, binding, etc.</para>
        /// </remarks>
        public static readonly DependencyProperty BeaconKeyProperty =
            DependencyProperty.Register("BeaconKey", typeof(string), typeof(Stage), new PropertyMetadata(""));


#if WPF

        /// <summary>
        /// <para>内部方法：如果视图未设置则定位视图</para>
        /// <para>Internal method: Locate view if not set</para>
        /// </summary>
        /// <typeparam name="TTarget">
        /// <para>目标视图模型类型，必须实现IViewModel接口</para>
        /// <para>Target view model type that must implement IViewModel interface</para>
        /// </typeparam>
        /// <param name="targetViewModel">
        /// <para>目标视图模型实例</para>
        /// <para>Target view model instance</para>
        /// </param>
        /// <param name="viewMappingKey">
        /// <para>视图映射键，用于查找对应的视图类型</para>
        /// <para>View mapping key for finding corresponding view type</para>
        /// </param>
        /// <param name="view">
        /// <para>当前视图实例，如果为null则需要创建新实例</para>
        /// <para>Current view instance, if null then need to create new instance</para>
        /// </param>
        /// <returns>
        /// <para>定位到的视图实例</para>
        /// <para>Located view instance</para>
        /// </returns>
        /// <remarks>
        /// <para>此方法首先尝试从视图模型的舞台管理器获取当前绑定视图，然后通过映射关系查找视图类型并创建实例。</para>
        /// <para>This method first tries to get current binding view from view model's stage manager, then finds view type through mapping and creates instance.</para>
        /// </remarks>
        private IView InternalLocateViewIfNotSet<TTarget>(TTarget targetViewModel, string viewMappingKey, IView view) where TTarget : class, IViewModel
        {
            if (targetViewModel != null && targetViewModel.StageManager != null)
            {
                view = targetViewModel.StageManager.CurrentBindingView;
            }

            var (_, viewType) = ViewAndModelMappingsHelper.DefaultVMToViewMapping[(viewMappingKey, typeof(TTarget))];


            var tempControl = ServiceProvider.GetService(viewType) as FrameworkElement;

            view = view ?? tempControl as IView;
            view = view ?? (tempControl as FrameworkElement)?.GetOrCreateViewDisguise();
            return view;
        }




        /// <summary>
        /// <para>显示具有映射视图的视图模型（WPF平台）</para>
        /// <para>Show a view model with mapped view (WPF platform)</para>
        /// </summary>
        /// <typeparam name="TTarget">
        /// <para>目标视图模型类型，必须实现IViewModel接口</para>
        /// <para>Target view model type that must implement IViewModel interface</para>
        /// </typeparam>
        /// <param name="viewMappingKey">
        /// <para>视图映射键，用于查找对应的视图类型</para>
        /// <para>View mapping key for finding corresponding view type</para>
        /// </param>
        /// <param name="additionalViewModelConfig">
        /// <para>额外的视图模型配置委托</para>
        /// <para>Additional view model configuration delegate</para>
        /// </param>
        /// <param name="isWaitingForDispose">
        /// <para>是否等待视图模型被释放</para>
        /// <para>Whether to wait for view model to be disposed</para>
        /// </param>
        /// <param name="autoDisposeWhenViewUnload">
        /// <para>视图卸载时是否自动释放视图模型</para>
        /// <para>Whether to auto dispose view model when view unloads</para>
        /// </param>
        /// <returns>
        /// <para>显示的视图模型实例</para>
        /// <para>Displayed view model instance</para>
        /// </returns>
        /// <remarks>
        /// <para>此方法是WPF平台的Show方法实现，支持通过映射键查找视图并显示。支持同步和异步显示模式。</para>
        /// <para>This method is the WPF platform Show method implementation, supports finding views through mapping keys and displaying them. Supports both synchronous and asynchronous display modes.</para>
        /// </remarks>
        public async Task<TTarget> Show<TTarget>(string viewMappingKey, Action<(IServiceProvider serviceProvider, TTarget viewModel)> additionalViewModelConfig, bool isWaitingForDispose, bool autoDisposeWhenViewUnload) where TTarget : class, IViewModel
        {

            IView view = null;
            var targetViewModel = ServiceProvider.GetService<TTarget>();

            view = InternalLocateViewIfNotSet<TTarget>(targetViewModel, viewMappingKey, view);
            targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
            targetViewModel = targetViewModel ?? view.GetDefaultViewModel(viewMappingKey) as TTarget;

            targetViewModel.IsDisposingWhenUnloadRequired = autoDisposeWhenViewUnload;
            additionalViewModelConfig?.Invoke((ServiceProvider, targetViewModel));

            SetVMAfterLoad(targetViewModel, view);
            InternalShowView(view, Target as FrameworkElement, _stageManager.CurrentBindingView.ViewModel);

            if (isWaitingForDispose)
            {
                await targetViewModel.WaitForClose().ConfigureAwait(true);
            }
            return targetViewModel;
        }



#endif

        /// <summary>
        /// <para>加载后设置视图模型的静态方法</para>
        /// <para>Static method to set view model after loading</para>
        /// </summary>
        /// <typeparam name="TTarget">
        /// <para>目标视图模型类型，必须实现IViewModel接口</para>
        /// <para>Target view model type that must implement IViewModel interface</para>
        /// </typeparam>
        /// <param name="targetViewModel">
        /// <para>要设置的目标视图模型实例</para>
        /// <para>Target view model instance to be set</para>
        /// </param>
        /// <param name="view">
        /// <para>要关联的视图实例</para>
        /// <para>View instance to be associated</para>
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <para>当无法找到从视图模型到视图的任何映射时抛出</para>
        /// <para>Thrown when no mapping from view model to view can be found</para>
        /// </exception>
        /// <remarks>
        /// <para>此方法验证视图是否为null，如果为null则抛出异常。然后将视图模型设置给视图。</para>
        /// <para>特别处理IPageView类型的视图，确保视图模型正确关联。</para>
        /// <para>This method validates if view is null and throws exception if it is. Then sets the view model to the view.</para>
        /// <para>Specially handles IPageView type views to ensure view model is correctly associated.</para>
        /// </remarks>
        private static void SetVMAfterLoad<TTarget>(TTarget targetViewModel, IView view) where TTarget : class, IViewModel
        {
            if (view == null)
            {
                throw new InvalidOperationException(
                    $@"
Cannot find ANY mapping from View Model [{targetViewModel.GetType().ToString()}] to ANY View.
Please check startup function of this mapping is well configured and be proper called while application starting");
            }


            if (view is IPageView)
            {

                view.ViewModel = targetViewModel;
            }
            view.ViewModel = targetViewModel;


        }

#if WINDOWS_UWP || WinUI3



        /// <summary>
        /// <para>显示具有映射视图的视图模型（UWP/WinUI3平台）</para>
        /// <para>Show a view model with mapped view (UWP/WinUI3 platform)</para>
        /// </summary>
        /// <typeparam name="TTarget">
        /// <para>目标视图模型类型，必须实现IViewModel接口</para>
        /// <para>Target view model type that must implement IViewModel interface</para>
        /// </typeparam>
        /// <param name="viewMappingKey">
        /// <para>视图映射键，用于查找对应的视图类型</para>
        /// <para>View mapping key for finding corresponding view type</para>
        /// </param>
        /// <param name="additionalViewModelConfig">
        /// <para>额外的视图模型配置委托</para>
        /// <para>Additional view model configuration delegate</para>
        /// </param>
        /// <param name="isWaitingForDispose">
        /// <para>是否等待视图模型被释放</para>
        /// <para>Whether to wait for view model to be disposed</para>
        /// </param>
        /// <param name="autoDisposeWhenViewUnload">
        /// <para>视图卸载时是否自动释放视图模型</para>
        /// <para>Whether to auto dispose view model when view unloads</para>
        /// </param>
        /// <returns>
        /// <para>显示的视图模型实例</para>
        /// <para>Displayed view model instance</para>
        /// </returns>
        /// <remarks>
        /// <para>此方法是UWP/WinUI3平台的Show方法实现。支持Page类型的Frame导航和其他控件类型的直接显示。</para>
        /// <para>对于Page类型会使用Frame进行导航，对于其他类型则直接显示在目标容器中。</para>
        /// <para>This method is the UWP/WinUI3 platform Show method implementation. Supports Frame navigation for Page types and direct display for other control types.</para>
        /// <para>For Page types it uses Frame for navigation, for other types it displays directly in target container.</para>
        /// </remarks>
        public async Task<TTarget> Show<TTarget>(string viewMappingKey, Action<(IServiceProvider serviceProvider, TTarget viewModel)> additionalViewModelConfig, bool isWaitingForDispose, bool autoDisposeWhenViewUnload) where TTarget : class, IViewModel
        {
            var targetViewModel = ServiceProvider.GetService<TTarget>();

            //bool isInstancePassedThough = targetViewModel == null;
            // object item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewMappingKey, targetViewModel);


            var (_, registeredViewType) = ViewAndModelMappingsHelper.DefaultVMToViewMapping[(viewMappingKey, typeof(TTarget))];


            IView view;
            if (typeof(Page).IsAssignableFrom(registeredViewType))
            {
                Frame frame = Target as Frame;
                if (frame != null)
                {
                    targetViewModel = await FrameNavigate<TTarget>(targetViewModel, viewMappingKey, registeredViewType, frame, ServiceProvider).ConfigureAwait(true);

                    await targetViewModel.WaitForClose().ConfigureAwait(true);
                    return targetViewModel;
                }

                view = (frame.Content as DependencyObject)?.GetOrCreateViewDisguise();
            }
            else
            {
                var fe = ServiceProvider.GetService(viewMappingKey,registeredViewType) as FrameworkElement;
                view = fe.GetOrCreateViewDisguise();
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
            }

            targetViewModel.IsDisposingWhenUnloadRequired = autoDisposeWhenViewUnload;
            additionalViewModelConfig?.Invoke((ServiceProvider, targetViewModel));

            SetVMAfterLoad(targetViewModel, view);
            InternalShowView(view, Target as FrameworkElement, _stageManager.CurrentBindingView.ViewModel);

            if (isWaitingForDispose)
            {
                await targetViewModel.WaitForClose().ConfigureAwait(true);
            }
            return targetViewModel;

        }

        /// <summary>
        /// <para>Frame导航的静态异步方法</para>
        /// <para>Static asynchronous method for Frame navigation</para>
        /// </summary>
        /// <typeparam name="TTarget">
        /// <para>目标视图模型类型，必须实现IViewModel接口</para>
        /// <para>Target view model type that must implement IViewModel interface</para>
        /// </typeparam>
        /// <param name="targetViewModel">
        /// <para>目标视图模型实例</para>
        /// <para>Target view model instance</para>
        /// </param>
        /// <param name="mappingKey">
        /// <para>映射键，用于查找视图模型</para>
        /// <para>Mapping key for finding view model</para>
        /// </param>
        /// <param name="viewType">
        /// <para>视图类型</para>
        /// <para>View type</para>
        /// </param>
        /// <param name="frame">
        /// <para>用于导航的Frame控件</para>
        /// <para>Frame control for navigation</para>
        /// </param>
        /// <param name="serviceProvider">
        /// <para>服务提供程序</para>
        /// <para>Service provider</para>
        /// </param>
        /// <returns>
        /// <para>导航完成后的视图模型实例</para>
        /// <para>View model instance after navigation completion</para>
        /// </returns>
        /// <remarks>
        /// <para>此方法处理UWP/WinUI3平台的Frame导航逻辑，使用事件路由器监听导航事件并配置视图。</para>
        /// <para>导航完成后会自动设置视图模型并调用平台特定的生命周期方法。</para>
        /// <para>This method handles UWP/WinUI3 platform Frame navigation logic, uses event router to listen navigation events and configure views.</para>
        /// <para>After navigation completion, it automatically sets view model and calls platform-specific lifecycle methods.</para>
        /// </remarks>
        private static async Task<TTarget> FrameNavigate<TTarget>(TTarget targetViewModel, string mappingKey, Type viewType,   Frame frame, IServiceProvider serviceProvider) where TTarget : class, IViewModel
        {
            StageNavigationContext<TTarget> parameter = new StageNavigationContext<TTarget>() { ViewModel = targetViewModel };
            TaskCompletionSource<object> t = new TaskCompletionSource<object>();
            IDisposable dip = EventRouting.EventRouter.Instance
                 .GetEventChannel<NavigationEventArgs>()

                 .Where(e =>
                         object.ReferenceEquals(e.EventData.Parameter, parameter))
                 .Subscribe(e =>
                 {
                     Page page = null;
                     IView view = null;
                     switch (e.Sender)
                     {
                         case PageViewDisguise disguise:
                             page = disguise.AssocatedObject;
                             view = disguise;
                             break;
                         default:
                             break;
                     }

                     var viewInstance = view.ViewContentObject;
       
                     var configOfView = serviceProvider.GetService(typeof(ViewContentConfigurator<>).MakeGenericType(viewType)) as IViewContentConfigurator;
                     configOfView.Config(viewInstance);


                     if (parameter.ViewModel != null)
                     {
                         view.ViewModel = parameter.ViewModel;
                     }
                     else
                     {
                         IViewModel solveV = view.GetDefaultViewModel(mappingKey);
                         if (solveV != null)
                         {
                             targetViewModel = parameter.ViewModel = (TTarget)solveV;
                         }
                     }

                     if (targetViewModel == null)
                     {
                         targetViewModel = (TTarget)view.ViewModel;
                     }

                     view.ViewModel = parameter.ViewModel = targetViewModel;
                     (targetViewModel as IViewModelWithPlatformService)?.OnPageNavigatedTo(e.EventData);
                     t.TrySetResult(null);
                 });

            frame.Navigate(viewType, parameter);
            await t.Task.ConfigureAwait(true);
            dip.DisposeWith(targetViewModel);
            return targetViewModel;
        }

        /// <summary>
        /// <para>舞台导航上下文的私有类</para>
        /// <para>Private class for stage navigation context</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>视图模型类型，必须实现IViewModel接口</para>
        /// <para>View model type that must implement IViewModel interface</para>
        /// </typeparam>
        /// <remarks>
        /// <para>此类用于在Frame导航过程中传递视图模型实例，作为导航参数的容器。</para>
        /// <para>This class is used to pass view model instances during Frame navigation, serving as a container for navigation parameters.</para>
        /// </remarks>
        private class StageNavigationContext<T> where T : IViewModel
        {
            /// <summary>
            /// <para>获取或设置视图模型实例</para>
            /// <para>Gets or sets the view model instance</para>
            /// </summary>
            /// <value>
            /// <para>视图模型实例</para>
            /// <para>View model instance</para>
            /// </value>
            public T ViewModel { get; set; }

        }

        ///// <summary>
        ///// Show a viewmodel and return a result when leave.
        ///// </summary>
        ///// <typeparam name="TTarget"></typeparam>
        ///// <typeparam name="TResult"></typeparam>
        ///// <param name="targetViewModel"></param>
        ///// <param name="viewMappingKey"></param>

        ///// <returns></returns>
        //public async Task<TResult> Show<TTarget, TResult>(TTarget targetViewModel = null, string viewMappingKey = null, bool isWaitingForDispose = false)
        //    where TTarget : class, IViewModel<TResult>
        //{
        //    object item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewMappingKey, targetViewModel);
        //    Type type;
        //    if ((type = item as Type) != null) //only MVVMPage Can be registered as Type
        //    {
        //        Frame frame;
        //        if ((frame = Target as Frame) != null)
        //        {
        //            targetViewModel = await FrameNavigate<TTarget>(targetViewModel, type, frame);

        //            return await targetViewModel.WaitForCloseWithResult();
        //        }

        //    }


        //    IView view = item as IView;
        //    targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
        //    SetVMAfterLoad(targetViewModel, view);
        //    InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
        //    if (isWaitingForDispose)
        //    {
        //        return await targetViewModel.WaitForCloseWithResult();
        //    }
        //    else
        //    {
        //        return targetViewModel.Result;
        //    }

        //}

        ///// <summary>
        ///// show a view model mapped view and return the viewmodel 
        ///// </summary>
        ///// <typeparam name="TTarget"></typeparam>
        ///// <param name="targetViewModel"></param>
        ///// <param name="viewMappingKey"></param>
        ///// <returns></returns>

        //public async Task<ShowAwaitableResult<TTarget>> ShowAndGetViewModel<TTarget>(TTarget targetViewModel = null, string viewMappingKey = null)
        //    where TTarget : class, IViewModel
        //{
        //    object item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewMappingKey, targetViewModel);
        //    Type type;
        //    if ((type = item as Type) != null) //only MVVMPage Can be registered as Type
        //    {
        //        Frame frame;
        //        if ((frame = Target as Frame) != null)
        //        {
        //            targetViewModel = await FrameNavigate<TTarget>(targetViewModel, type, frame);

        //            return new ShowAwaitableResult<TTarget>
        //            {
        //                Closing = targetViewModel.WaitForClose(),
        //                ViewModel = targetViewModel
        //            };
        //        }

        //    }


        //    IView view = item as IView;

        //    targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
        //    SetVMAfterLoad(targetViewModel, view);
        //    InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);

        //    Task tr = targetViewModel.WaitForClose();
        //    return new ShowAwaitableResult<TTarget> { Closing = tr, ViewModel = targetViewModel };
        //}
#endif



        /// <summary>
        /// <para>内部显示视图的私有方法</para>
        /// <para>Private method for internal view display</para>
        /// </summary>
        /// <param name="view">
        /// <para>要显示的视图实例</para>
        /// <para>View instance to be displayed</para>
        /// </param>
        /// <param name="target">
        /// <para>目标框架元素，作为视图的容器</para>
        /// <para>Target framework element serving as container for the view</para>
        /// </param>
        /// <param name="sourceVM">
        /// <para>源视图模型</para>
        /// <para>Source view model</para>
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <para>当视图类型不支持在指定目标中显示时抛出</para>
        /// <para>Thrown when view type is not supported for display in specified target</para>
        /// </exception>
        /// <remarks>
        /// <para>此方法根据目标控件类型和视图类型选择适当的显示方式：</para>
        /// <para>- 对于Window类型的IWindowView，直接显示窗口</para>
        /// <para>- 对于Frame目标，进行页面导航</para>
        /// <para>- 对于ContentControl，设置Content属性</para>
        /// <para>- 对于Panel，添加到Children集合</para>
        /// <para>This method selects appropriate display method based on target control type and view type:</para>
        /// <para>- For Window type IWindowView, displays window directly</para>
        /// <para>- For Frame target, performs page navigation</para>
        /// <para>- For ContentControl, sets Content property</para>
        /// <para>- For Panel, adds to Children collection</para>
        /// </remarks>
        private void InternalShowView(IView view, FrameworkElement target, IViewModel sourceVM)
        {

            switch (target)
            {
#if WPF
                case FrameworkElement targetWindow when view is IWindowView:
                    var viewWindow = view.ViewObject as Window;
                    IWindowView iviewWindow = view as IWindowView;

                    if (targetWindow == null)
                    {
                        targetWindow = sourceVM.StageManager.CurrentBindingView as Window;
                    }
                    if (iviewWindow.IsAutoOwnerSetNeeded)
                    {
                        //viewWindow.Owner = targetWindow;
                    }
                    viewWindow.Show();
                    break;
                case Frame targetFrame:

                    var ipv = (view as IPageView);

                    if (ipv != null)
                    {
                        ipv.FrameObject = this.Target;
                    }
                    targetFrame.Navigate(view.ViewObject);

                    break;
#endif
#if WINDOWS_UWP
                case ContentDialog targetCDControl:
                    targetCDControl.Content = view.ViewObject;
                    IViewModel viewModel = view.ViewModel ?? sourceVM.StageManager.ViewModel;
                    IDisposable closeFromViewModel = null;
                    closeFromViewModel = Observable
                        .FromAsync(x => viewModel.WaitForClose())
                        
                        .Subscribe(_ =>
                            {
                                viewModel.IsDisposingWhenUnloadRequired = true;
                                targetCDControl.Hide();
                                targetCDControl.Content = null;
                                closeFromViewModel?.Dispose();
                                closeFromViewModel = null;
                            });
                    IAsyncOperation<ContentDialogResult> t = targetCDControl.ShowAsync();
                    break;
#endif
                case ContentControl targetCControl:
                    targetCControl.Content = view.ViewObject;
                    break;
                case Panel targetPanelControl:
                    targetPanelControl.Children.Add(view.ViewObject as UIElement);
                    break;

                default:
                    throw new InvalidOperationException($"This view {view.GetType()} is not support show in {target.GetType()} ");
            }


        }


    }
}
#endif

/// <summary>
/// <para>StageManager类的using声明和条件编译指令</para>
/// <para>Using declarations and conditional compilation directives for StageManager class</para>
/// </summary>
/// <remarks>
/// <para>此文件包含多个平台的using声明，通过条件编译支持WPF、UWP、WinUI3、Silverlight等平台。</para>
/// <para>This file contains using declarations for multiple platforms, supporting WPF, UWP, WinUI3, Silverlight, etc. through conditional compilation.</para>
/// </remarks>

using System;
using System.Collections.Generic;
using MVVMSidekick.ViewModels;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;


#if !BLAZOR

#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;

#elif WinUI3
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media;
#elif WPF
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Concurrent;
using System.Windows.Navigation;
using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;
#elif SILVERLIGHT_5 || SILVERLIGHT_4
						   using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif



namespace MVVMSidekick.Views
{

    /// <summary>
    /// <para>舞台管理器类 - 用于Frame/ContentControl的抽象管理器</para>
    /// <para>Stage manager class - Abstract manager for Frame/ContentControl</para>
    /// </summary>
    /// <remarks>
    /// <para>StageManager是MVVM-Sidekick框架中的核心类，负责管理视图和视图模型之间的导航和显示。</para>
    /// <para>视图模型可以通过此类访问和显示其他视图模型及其映射的视图。</para>
    /// <para>支持信标(Beacon)系统，允许在视图中注册命名的容器，然后通过名称进行定位和导航。</para>
    /// <para>StageManager is a core class in MVVM-Sidekick framework, responsible for managing navigation and display between views and view models.</para>
    /// <para>View models can access and display other view models and their mapped views through this class.</para>
    /// <para>Supports beacon system that allows registering named containers in views, then locating and navigating through names.</para>
    /// </remarks>
    public class StageManager : DependencyObject, IStageManager
    {

        /// <summary>
        /// <para>使用服务提供程序初始化StageManager类的新实例</para>
        /// <para>Initializes a new instance of the StageManager class with service provider</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>服务提供程序，用于解析依赖项和服务</para>
        /// <para>Service provider for resolving dependencies and services</para>
        /// </param>
        /// <remarks>
        /// <para>此构造函数设置服务提供程序，用于后续的依赖注入和服务解析。</para>
        /// <para>This constructor sets the service provider for subsequent dependency injection and service resolution.</para>
        /// </remarks>
        public  StageManager(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// <para>初始化StageManager类的新实例（已注释的构造函数）</para>
        /// <para>Initializes a new instance of the StageManager class (commented constructor)</para>
        /// </summary>
        /// <param name="viewModel">
        /// <para>关联的视图模型</para>
        /// <para>Associated view model</para>
        /// </param>
        /// <remarks>
        /// <para>此构造函数已被注释，不再使用。保留用于向后兼容性参考。</para>
        /// <para>This constructor is commented out and no longer used. Kept for backward compatibility reference.</para>
        /// </remarks>
        //public StageManager(IViewModel viewModel)
        //{
        //    ViewModel = viewModel;
        //}

        /// <summary>
        /// <para>初始化StageManager类的新实例（无参数构造函数）</para>
        /// <para>Initializes a new instance of the StageManager class (parameterless constructor)</para>
        /// </summary>
        /// <remarks>
        /// <para>无参数构造函数，需要后续手动设置必要的属性和依赖项。</para>
        /// <para>Parameterless constructor, requires manual setting of necessary properties and dependencies afterward.</para>
        /// </remarks>
        public StageManager()
        { }

        /// <summary>
        /// <para>获取或设置关联的视图模型</para>
        /// <para>Gets or sets the associated view model</para>
        /// </summary>
        /// <value>
        /// <para>与此舞台管理器关联的视图模型实例</para>
        /// <para>View model instance associated with this stage manager</para>
        /// </value>
        /// <remarks>
        /// <para>此属性建立舞台管理器与特定视图模型之间的关联关系。</para>
        /// <para>This property establishes the association between stage manager and specific view model.</para>
        /// </remarks>
        public IViewModel ViewModel { get; set; }

        /// <summary>
        /// <para>导航信标键的常量</para>
        /// <para>Constant for navigator beacons key</para>
        /// </summary>
        /// <remarks>
        /// <para>此键用作注册键的前缀。舞台注册将字符串-元素映射存储在视图的资源字典中。</para>
        /// <para>这有助于避免覆盖已定义的资源。</para>
        /// <para>This key is used as a prefix for register keys. Stage registration stores String-Element-Mapping in view's Resource Dictionary.</para>
        /// <para>This helps not to overwrite the resources already defined.</para>
        /// </remarks>
        public const string NavigatorBeaconsKey= nameof(NavigatorBeaconsKey);

        /// <summary>
        /// <para>当前绑定视图的弱引用</para>
        /// <para>Weak reference to current binding view</para>
        /// </summary>
        private WeakReference _CurrentBindingView = new WeakReference(null);

        /// <summary>
        /// <para>获取此舞台管理器当前绑定的视图</para>
        /// <para>Get the currently bound view of this stage manager</para>
        /// </summary>
        /// <value>
        /// <para>当前绑定的视图，如果视图已被垃圾回收则返回null</para>
        /// <para>Currently bound view, returns null if view has been garbage collected</para>
        /// </value>
        /// <remarks>
        /// <para>舞台管理器为特定的视图服务。如果视图模型未绑定到视图，整个系统无法正常工作。</para>
        /// <para>使用弱引用避免内存泄漏，当视图被垃圾回收时自动释放引用。</para>
        /// <para>A stage manager serves a specific view. If view model is not bound to a view, the whole system cannot work.</para>
        /// <para>Uses weak reference to avoid memory leaks, automatically releases reference when view is garbage collected.</para>
        /// </remarks>
        public IView CurrentBindingView
        {
            get => _CurrentBindingView.IsAlive ? _CurrentBindingView.Target as IView : null;
            set => _CurrentBindingView = new WeakReference(value);
        }






        /// <summary>
        /// <para>初始化父定位器</para>
        /// <para>Initialize parent locator</para>
        /// </summary>
        /// <param name="parentLocator">
        /// <para>父定位器委托，用于查找父容器</para>
        /// <para>Parent locator delegate for finding parent container</para>
        /// </param>
        /// <remarks>
        /// <para>父定位器用于在无法通过信标系统找到目标容器时，提供备用的定位方法。</para>
        /// <para>Parent locator provides alternative location method when target container cannot be found through beacon system.</para>
        /// </remarks>
        public void InitParent(Func<object> parentLocator)
        {
            _parentLocator = parentLocator;

        }

        /// <summary>
        /// <para>私有父定位器字段</para>
        /// <para>Private parent locator field</para>
        /// </summary>
        private Func<object> _parentLocator;


#region Attached Property

        /// <summary>
        /// <para>获取指定依赖对象的信标值</para>
        /// <para>Gets the beacon value of specified dependency object</para>
        /// </summary>
        /// <param name="dobj">
        /// <para>要获取信标值的依赖对象</para>
        /// <para>Dependency object to get beacon value from</para>
        /// </param>
        /// <returns>
        /// <para>信标字符串值</para>
        /// <para>Beacon string value</para>
        /// </returns>
        /// <remarks>
        /// <para>此方法用于获取附加到UI元素上的信标标识符。信标用于在舞台管理器中标识和定位特定的容器。</para>
        /// <para>在WPF平台下，此方法仅对ContentControl、Frame和Window类型可见。</para>
        /// <para>This method is used to get beacon identifier attached to UI elements. Beacons are used to identify and locate specific containers in stage manager.</para>
        /// <para>On WPF platform, this method is only visible for ContentControl, Frame and Window types.</para>
        /// </remarks>
#if WPF
        [AttachedPropertyBrowsableForType(typeof(ContentControl))]
        [AttachedPropertyBrowsableForType(typeof(Frame))]
        [AttachedPropertyBrowsableForType(typeof(Window))]
#endif



        public static string GetBeacon(DependencyObject dobj)
        {
            return (string)dobj?.GetValue(BeaconProperty);
        }

        /// <summary>
        /// <para>设置指定依赖对象的信标值</para>
        /// <para>Sets the beacon value of specified dependency object</para>
        /// </summary>
        /// <param name="dobj">
        /// <para>要设置信标值的依赖对象</para>
        /// <para>Dependency object to set beacon value for</para>
        /// </param>
        /// <param name="value">
        /// <para>要设置的信标字符串值</para>
        /// <para>Beacon string value to set</para>
        /// </param>
        /// <remarks>
        /// <para>此方法用于为UI元素设置信标标识符。设置信标后，元素可以在舞台管理器中被识别和定位。</para>
        /// <para>仅在非设计模式下执行设置操作，避免在设计时产生副作用。</para>
        /// <para>在WPF平台下，此方法仅对ContentControl、Frame和Window类型可见。</para>
        /// <para>This method is used to set beacon identifier for UI elements. After setting beacon, elements can be identified and located in stage manager.</para>
        /// <para>Only performs setting operation in non-design mode to avoid side effects at design time.</para>
        /// <para>On WPF platform, this method is only visible for ContentControl, Frame and Window types.</para>
        /// </remarks>
#if WPF
        [AttachedPropertyBrowsableForType(typeof(ContentControl))]
        [AttachedPropertyBrowsableForType(typeof(Frame))]
        [AttachedPropertyBrowsableForType(typeof(Window))]
#endif


        public static void SetBeacon(DependencyObject dobj, string value)
        {
            if (!ViewModels.ViewModel.IsInDesignMode)
            {

                dobj?.SetValue(BeaconProperty, value);
            }
        }

        /// <summary>
        /// <para>信标附加属性的依赖属性标识符</para>
        /// <para>Dependency property identifier for beacon attached property</para>
        /// </summary>
        /// <remarks>
        /// <para>此依赖属性用于在UI元素上附加信标标识符。当属性值改变时，会自动注册目标信标。</para>
        /// <para>属性变更回调函数处理元素的Loaded事件，确保在元素完全加载后进行信标注册。</para>
        /// <para>This dependency property is used to attach beacon identifiers on UI elements. When property value changes, target beacon is automatically registered.</para>
        /// <para>Property change callback handles element's Loaded event to ensure beacon registration after element is fully loaded.</para>
        /// </remarks>
        public static readonly DependencyProperty BeaconProperty =
            DependencyProperty.RegisterAttached("Beacon", typeof(string), typeof(StageManager), new PropertyMetadata(null,
                   (o, p) =>
                   {
                       string name = (p.NewValue as string);
                       FrameworkElement target = o as FrameworkElement;

                       target.Loaded +=
                           (_1, _2)
                           =>
                           {
                               StageManager.RegisterTargetBeacon(name, target);
                           };
                   }

                   ));





#endregion

        /// <summary>
        /// <para>内部方法：定位目标容器</para>
        /// <para>Internal method: Locate target container</para>
        /// </summary>
        /// <param name="view">
        /// <para>要显示的视图</para>
        /// <para>View to be displayed</para>
        /// </param>
        /// <param name="targetContainerName">
        /// <para>目标容器名称（引用参数）</para>
        /// <para>Target container name (ref parameter)</para>
        /// </param>
        /// <param name="sourceVM">
        /// <para>源视图模型</para>
        /// <para>Source view model</para>
        /// </param>
        /// <returns>
        /// <para>找到的目标容器元素</para>
        /// <para>Found target container element</para>
        /// </returns>
        /// <remarks>
        /// <para>此方法按以下顺序查找目标容器：</para>
        /// <para>1. 通过信标字典查找命名容器</para>
        /// <para>2. 使用父定位器查找父容器</para>
        /// <para>3. 对于WPF平台的窗口视图，使用当前绑定视图</para>
        /// <para>4. 使用视图的Content作为容器</para>
        /// <para>This method finds target container in following order:</para>
        /// <para>1. Find named container through beacon dictionary</para>
        /// <para>2. Use parent locator to find parent container</para>
        /// <para>3. For WPF platform window views, use current binding view</para>
        /// <para>4. Use view's Content as container</para>
        /// </remarks>
        internal FrameworkElement LocateTargetContainer(IView view, ref string targetContainerName, IViewModel sourceVM)
        {


            targetContainerName = targetContainerName ?? "";
            FrameworkElement viewele = view.ViewObject as FrameworkElement;



            Dictionary<string, FrameworkElement> dic = GetOrCreateBeacons(sourceVM.StageManager.CurrentBindingView.ViewObject as FrameworkElement);
            dic.TryGetValue(targetContainerName, out FrameworkElement target);


            if (target == null)
            {
                target = _parentLocator() as FrameworkElement;
            }

            if (target == null)
            {
#if WPF
                if (string.IsNullOrWhiteSpace(targetContainerName) && view is IWindowView)
                {

                    target = sourceVM.StageManager.CurrentBindingView.ViewObject as FrameworkElement;

                }
                else
#endif
                {
                    ContentControl vieweleCt = viewele as ContentControl;
                    if (vieweleCt != null)
                    {
                        target = vieweleCt.Content as FrameworkElement;
                    }
                }

            }
            return target;
        }




        /// <summary>
        /// <para>获取或创建信标字典的私有静态方法</para>
        /// <para>Private static method to get or create beacons dictionary</para>
        /// </summary>
        /// <param name="view">
        /// <para>视图框架元素</para>
        /// <para>View framework element</para>
        /// </param>
        /// <returns>
        /// <para>信标字典，包含名称到框架元素的映射</para>
        /// <para>Beacons dictionary containing name to framework element mappings</para>
        /// </returns>
        /// <remarks>
        /// <para>此方法在视图的资源字典中查找或创建信标映射字典。</para>
        /// <para>如果字典不存在，则创建新的字典并添加到视图资源中。</para>
        /// <para>不同平台使用不同的检查方法：UWP/WinUI3使用ContainsKey，WPF使用Contains。</para>
        /// <para>This method finds or creates beacon mapping dictionary in view's resource dictionary.</para>
        /// <para>If dictionary doesn't exist, creates new dictionary and adds to view resources.</para>
        /// <para>Different platforms use different checking methods: UWP/WinUI3 use ContainsKey, WPF uses Contains.</para>
        /// </remarks>
        private static Dictionary<string, FrameworkElement> GetOrCreateBeacons(FrameworkElement view)
        {
            Dictionary<string, FrameworkElement> dic;
#if WINDOWS_UWP || WinUI3
            if (!view.Resources.ContainsKey(NavigatorBeaconsKey))
#elif WPF
            if (!view.Resources.Contains(NavigatorBeaconsKey))
#endif
            {
                dic = new Dictionary<string, FrameworkElement>();
                view.Resources.Add(NavigatorBeaconsKey, dic);
            }
            else
            {
                dic = view.Resources[NavigatorBeaconsKey] as Dictionary<string, FrameworkElement>;
            }

            return dic;
        }

        /// <summary>
        /// <para>注册目标信标的静态方法</para>
        /// <para>Static method to register target beacon</para>
        /// </summary>
        /// <param name="name">
        /// <para>信标名称</para>
        /// <para>Beacon name</para>
        /// </param>
        /// <param name="target">
        /// <para>目标框架元素</para>
        /// <para>Target framework element</para>
        /// </param>
        /// <remarks>
        /// <para>此方法将指定的目标元素注册到信标字典中，使其可以通过名称进行查找。</para>
        /// <para>首先定位包含目标元素的视图，然后将目标元素添加到该视图的信标字典中。</para>
        /// <para>仅在非设计模式下执行注册操作。</para>
        /// <para>This method registers specified target element to beacon dictionary, making it findable by name.</para>
        /// <para>First locates the view containing target element, then adds target element to that view's beacon dictionary.</para>
        /// <para>Only performs registration in non-design mode.</para>
        /// </remarks>
        public static void RegisterTargetBeacon(string name, FrameworkElement target)
        {
            if (!ViewModels.ViewModel.IsInDesignMode)
            {


                FrameworkElement view = LocateIView(target);

                Dictionary<string, FrameworkElement> beacons = GetOrCreateBeacons(view);
                beacons[name] = target;
            }

        }

        /// <summary>
        /// <para>定位IView的私有静态方法</para>
        /// <para>Private static method to locate IView</para>
        /// </summary>
        /// <param name="target">
        /// <para>起始目标元素</para>
        /// <para>Starting target element</para>
        /// </param>
        /// <returns>
        /// <para>找到的视图框架元素</para>
        /// <para>Found view framework element</para>
        /// </returns>
        /// <remarks>
        /// <para>此方法从给定的目标元素开始，向上遍历可视化树和逻辑树，查找实现IView接口的元素。</para>
        /// <para>遍历顺序：首先尝试Parent属性，然后尝试VisualTreeHelper.GetParent方法。</para>
        /// <para>当找到IView类型的元素或具有ViewDisguise的元素时停止搜索。</para>
        /// <para>This method starts from given target element, traverses up the visual and logical tree to find elements implementing IView interface.</para>
        /// <para>Traversal order: first tries Parent property, then tries VisualTreeHelper.GetParent method.</para>
        /// <para>Stops searching when finds IView type element or element with ViewDisguise.</para>
        /// </remarks>
        private static FrameworkElement LocateIView(FrameworkElement target)
        {
            FrameworkElement view = target;

            while (view != null)
            {
                //	var tryView = GetViewOfStage(target) as FrameworkElement;
                //	if (tryView != null)
                //	{
                //		return tryView;
                //	}

                FrameworkElement tryView = view.Parent as FrameworkElement;

                if (tryView != null)
                {
                    view = tryView;
                }
                else
                {
                    tryView = VisualTreeHelper.GetParent(view) as FrameworkElement;
                    view = tryView;
                }
                if (view is IView || view.GetViewDisguise() != null)
                {
                    break;
                }
            }
            return view;
        }







        /// <summary>
        /// <para>获取默认舞台</para>
        /// <para>Gets the default stage</para>
        /// </summary>
        /// <value>
        /// <para>使用空字符串作为信标键的默认舞台</para>
        /// <para>Default stage using empty string as beacon key</para>
        /// </value>
        /// <remarks>
        /// <para>默认舞台是使用空字符串("")作为信标键的舞台实例。</para>
        /// <para>Default stage is a stage instance using empty string ("") as beacon key.</para>
        /// </remarks>
        public IStage DefaultStage => this[""];

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
        /// <para>通过信标键获取舞台实例的索引器</para>
        /// <para>Indexer to get stage instance by beacon key</para>
        /// </summary>
        /// <param name="beaconKey">
        /// <para>信标键，用于标识目标容器</para>
        /// <para>Beacon key for identifying target container</para>
        /// </param>
        /// <returns>
        /// <para>对应的舞台实例，如果无法找到目标容器则返回null</para>
        /// <para>Corresponding stage instance, returns null if target container cannot be found</para>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// <para>当服务提供程序返回的不是Stage类型时抛出</para>
        /// <para>Thrown when service provider returns non-Stage type</para>
        /// </exception>
        /// <remarks>
        /// <para>此索引器通过信标键定位目标容器，然后创建并初始化对应的舞台实例。</para>
        /// <para>如果找到目标容器，则从服务提供程序获取Stage实例并进行初始化。</para>
        /// <para>This indexer locates target container by beacon key, then creates and initializes corresponding stage instance.</para>
        /// <para>If target container is found, gets Stage instance from service provider and initializes it.</para>
        /// </remarks>
        public IStage this[string beaconKey]
        {
            get
            {
                FrameworkElement fr = LocateTargetContainer(CurrentBindingView, ref beaconKey, ViewModel);
                if (fr != null)
                {
                    var stage = ServiceProvider.GetRequiredService<IStage>() as  Stage;
                    if (stage ==null )
                    {
                        throw new InvalidOperationException("StageManager can only work with Stage. Please check your settings");
                    }
                    stage.InitStage(fr, beaconKey, this);

                    return stage;
                }
                else
                {
                    return null;
                }
            }
        }
    }

}
#endif
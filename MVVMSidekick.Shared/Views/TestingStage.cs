/// <summary>
/// <para>测试舞台类和测试舞台管理器的实现文件</para>
/// <para>Implementation file for testing stage classes and testing stage manager</para>
/// </summary>
/// <remarks>
/// <para>此文件包含TestingStage和TestingStageManager类，专门用于单元测试和集成测试场景。</para>
/// <para>这些类提供了模拟的舞台环境，允许在没有实际UI的情况下测试视图模型的行为。</para>
/// <para>This file contains TestingStage and TestingStageManager classes specifically designed for unit testing and integration testing scenarios.</para>
/// <para>These classes provide simulated stage environment allowing testing of view model behavior without actual UI.</para>
/// </remarks>
#if !BLAZOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVVMSidekick.ViewModels;
using System.Reactive.Linq;
using System.Windows;
using System.IO;
using MVVMSidekick.Services;
using Microsoft.Extensions.DependencyInjection;


#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;


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
    /// <para>测试舞台管理器类</para>
    /// <para>Testing stage manager class</para>
    /// </summary>
    /// <remarks>
    /// <para>TestingStageManager是IStageManager接口的测试实现，专门用于单元测试和集成测试场景。</para>
    /// <para>它提供了一个简化的舞台管理环境，不依赖实际的UI控件，便于测试视图模型的导航和显示逻辑。</para>
    /// <para>TestingStageManager is a testing implementation of IStageManager interface, specifically designed for unit testing and integration testing scenarios.</para>
    /// <para>It provides a simplified stage management environment that doesn't depend on actual UI controls, facilitating testing of view model navigation and display logic.</para>
    /// </remarks>
    public class TestingStageManager : IStageManager
    {
        /// <summary>
        /// <para>初始化TestingStageManager类的新实例</para>
        /// <para>Initializes a new instance of the TestingStageManager class</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>服务提供程序，用于解析依赖项和服务</para>
        /// <para>Service provider for resolving dependencies and services</para>
        /// </param>
        /// <remarks>
        /// <para>构造函数设置服务提供程序，用于后续的依赖注入和服务解析。</para>
        /// <para>Constructor sets the service provider for subsequent dependency injection and service resolution.</para>
        /// </remarks>
        public TestingStageManager(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// <para>当前舞台实例的字典</para>
        /// <para>Dictionary of current stage instances</para>
        /// </summary>
        /// <remarks>
        /// <para>此字典存储所有已创建的舞台实例，以信标键作为索引。</para>
        /// <para>This dictionary stores all created stage instances, indexed by beacon key.</para>
        /// </remarks>
        private Dictionary<string, IStage> _currentStages = new Dictionary<string, IStage>();

        /// <summary>
        /// <para>通过信标键获取舞台实例的索引器</para>
        /// <para>Indexer to get stage instance by beacon key</para>
        /// </summary>
        /// <param name="beaconKey">
        /// <para>信标键，用于标识目标舞台</para>
        /// <para>Beacon key for identifying target stage</para>
        /// </param>
        /// <returns>
        /// <para>对应的舞台实例</para>
        /// <para>Corresponding stage instance</para>
        /// </returns>
        /// <remarks>
        /// <para>此索引器首先尝试从缓存中获取舞台实例，如果不存在则创建新的TestingStage实例。</para>
        /// <para>新创建的舞台会被配置为支持前进和后退导航，并缓存以供后续使用。</para>
        /// <para>This indexer first tries to get stage instance from cache, creates new TestingStage instance if not exists.</para>
        /// <para>Newly created stage is configured to support forward and back navigation, and cached for subsequent use.</para>
        /// </remarks>
        public IStage this[string beaconKey]
        {
            get
            {
                _currentStages.TryGetValue(beaconKey, out IStage stage);
                if (stage == null)
                {
                    var tstage = ServiceProvider.GetRequiredService<IStage>(beaconKey) as TestingStage;
                    stage = tstage;

                    tstage.BeaconKey = beaconKey;
                    tstage.CanGoBack = true;
                    tstage.CanGoForward = true;
                    tstage.IsGoBackSupported = true;
                    tstage.IsGoForwardSupported = true;
                    tstage.Target = null;

                    _currentStages[beaconKey] = stage;
                }
                return stage;
            }
        }

        /// <summary>
        /// <para>获取或设置当前绑定的视图</para>
        /// <para>Gets or sets the current binding view</para>
        /// </summary>
        /// <value>
        /// <para>当前绑定的视图实例</para>
        /// <para>Current binding view instance</para>
        /// </value>
        /// <remarks>
        /// <para>在测试环境中，此属性可能为null，因为测试通常不涉及实际的视图。</para>
        /// <para>In testing environment, this property may be null as tests usually don't involve actual views.</para>
        /// </remarks>
        public IView CurrentBindingView
        {
            get; set;
        }

        /// <summary>
        /// <para>获取或设置默认舞台</para>
        /// <para>Gets or sets the default stage</para>
        /// </summary>
        /// <value>
        /// <para>默认舞台实例，在测试实现中始终为null</para>
        /// <para>Default stage instance, always null in testing implementation</para>
        /// </value>
        /// <remarks>
        /// <para>测试实现中不提供默认舞台，所有舞台都通过信标键进行访问。</para>
        /// <para>Testing implementation doesn't provide default stage, all stages are accessed through beacon keys.</para>
        /// </remarks>
        public IStage DefaultStage
        {
            get => null;

            set { }
        }

        /// <summary>
        /// <para>获取或设置关联的视图模型</para>
        /// <para>Gets or sets the associated view model</para>
        /// </summary>
        /// <value>
        /// <para>与此测试舞台管理器关联的视图模型实例</para>
        /// <para>View model instance associated with this testing stage manager</para>
        /// </value>
        public IViewModel ViewModel { get; set; }

        /// <summary>
        /// <para>获取服务提供程序</para>
        /// <para>Gets the service provider</para>
        /// </summary>
        /// <value>
        /// <para>用于解析依赖项和服务的服务提供程序</para>
        /// <para>Service provider for resolving dependencies and services</para>
        /// </value>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// <para>初始化父定位器</para>
        /// <para>Initialize parent locator</para>
        /// </summary>
        /// <param name="parentLocator">
        /// <para>父定位器委托</para>
        /// <para>Parent locator delegate</para>
        /// </param>
        /// <remarks>
        /// <para>测试实现中此方法为空，不执行任何操作。</para>
        /// <para>This method is empty in testing implementation and performs no operations.</para>
        /// </remarks>
        public void InitParent(Func<object> parentLocator)
        {

        }
    }

    /// <summary>
    /// <para>测试舞台类</para>
    /// <para>Testing stage class</para>
    /// </summary>
    /// <remarks>
    /// <para>TestingStage是IStage接口的测试实现，专门用于单元测试和集成测试场景。</para>
    /// <para>它提供了模拟的舞台环境，允许测试视图模型的显示逻辑而无需实际的UI控件。</para>
    /// <para>支持模拟操作，可以预设视图模型显示时的行为，便于测试各种场景。</para>
    /// <para>TestingStage is a testing implementation of IStage interface, specifically designed for unit testing and integration testing scenarios.</para>
    /// <para>It provides simulated stage environment allowing testing of view model display logic without actual UI controls.</para>
    /// <para>Supports mocking operations, allowing preset behaviors when view models are displayed, facilitating testing of various scenarios.</para>
    /// </remarks>
    public class TestingStage : IStage
    {

        /// <summary>
        /// <para>初始化TestingStage类的新实例</para>
        /// <para>Initializes a new instance of the TestingStage class</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>服务提供程序，用于解析依赖项和服务</para>
        /// <para>Service provider for resolving dependencies and services</para>
        /// </param>
        /// <remarks>
        /// <para>构造函数设置服务提供程序，用于后续的依赖注入和服务解析。</para>
        /// <para>Constructor sets the service provider for subsequent dependency injection and service resolution.</para>
        /// </remarks>
        public TestingStage(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// <para>获取或设置信标键</para>
        /// <para>Gets or sets the beacon key</para>
        /// </summary>
        /// <value>
        /// <para>用于标识此测试舞台实例的字符串键</para>
        /// <para>String key used to identify this testing stage instance</para>
        /// </value>
        public string BeaconKey
        {
            get; set;
        }

        /// <summary>
        /// <para>获取或设置是否可以后退</para>
        /// <para>Gets or sets whether it can go back</para>
        /// </summary>
        /// <value>
        /// <para>指示是否可以执行后退操作的布尔值</para>
        /// <para>Boolean value indicating whether back operation can be performed</para>
        /// </value>
        /// <remarks>
        /// <para>在测试环境中，此属性可以被设置为任意值来模拟不同的导航状态。</para>
        /// <para>In testing environment, this property can be set to any value to simulate different navigation states.</para>
        /// </remarks>
        public bool CanGoBack
        {
            get; set;
        }

        /// <summary>
        /// <para>获取或设置是否可以前进</para>
        /// <para>Gets or sets whether it can go forward</para>
        /// </summary>
        /// <value>
        /// <para>指示是否可以执行前进操作的布尔值</para>
        /// <para>Boolean value indicating whether forward operation can be performed</para>
        /// </value>
        /// <remarks>
        /// <para>在测试环境中，此属性可以被设置为任意值来模拟不同的导航状态。</para>
        /// <para>In testing environment, this property can be set to any value to simulate different navigation states.</para>
        /// </remarks>
        public bool CanGoForward
        {
            get; set;
        }

        /// <summary>
        /// <para>获取或设置Frame控件</para>
        /// <para>Gets or sets the Frame control</para>
        /// </summary>
        /// <value>
        /// <para>模拟的Frame控件实例</para>
        /// <para>Simulated Frame control instance</para>
        /// </value>
        /// <remarks>
        /// <para>在测试环境中，此属性通常为null，因为不需要实际的Frame控件。</para>
        /// <para>In testing environment, this property is usually null as actual Frame control is not needed.</para>
        /// </remarks>
        public Frame Frame
        {
            get; set;
        }

        /// <summary>
        /// <para>获取或设置是否支持后退导航</para>
        /// <para>Gets or sets whether back navigation is supported</para>
        /// </summary>
        /// <value>
        /// <para>指示是否支持后退导航的布尔值</para>
        /// <para>Boolean value indicating whether back navigation is supported</para>
        /// </value>
        /// <remarks>
        /// <para>在测试环境中，此属性可以被设置为任意值来模拟不同的导航能力。</para>
        /// <para>In testing environment, this property can be set to any value to simulate different navigation capabilities.</para>
        /// </remarks>
        public bool IsGoBackSupported
        {
            get; set;
        }

        /// <summary>
        /// <para>获取或设置是否支持前进导航</para>
        /// <para>Gets or sets whether forward navigation is supported</para>
        /// </summary>
        /// <value>
        /// <para>指示是否支持前进导航的布尔值</para>
        /// <para>Boolean value indicating whether forward navigation is supported</para>
        /// </value>
        /// <remarks>
        /// <para>在测试环境中，此属性可以被设置为任意值来模拟不同的导航能力。</para>
        /// <para>In testing environment, this property can be set to any value to simulate different navigation capabilities.</para>
        /// </remarks>
        public bool IsGoForwardSupported
        {
            get; set;
        }

        /// <summary>
        /// <para>获取或设置目标对象</para>
        /// <para>Gets or sets the target object</para>
        /// </summary>
        /// <value>
        /// <para>目标对象实例</para>
        /// <para>Target object instance</para>
        /// </value>
        /// <remarks>
        /// <para>在测试环境中，此属性通常为null，因为不需要实际的目标控件。</para>
        /// <para>In testing environment, this property is usually null as actual target control is not needed.</para>
        /// </remarks>
        public Object Target
        {
            get; set;
        }

        /// <summary>
        /// <para>获取服务提供程序</para>
        /// <para>Gets the service provider</para>
        /// </summary>
        /// <value>
        /// <para>用于解析依赖项和服务的服务提供程序</para>
        /// <para>Service provider for resolving dependencies and services</para>
        /// </value>
        IServiceProvider ServiceProvider { get; }

#if WPF
        /// <summary>
        /// <para>显示视图模型并返回结果（WPF平台）</para>
        /// <para>Show view model and return result (WPF platform)</para>
        /// </summary>
        /// <typeparam name="TTarget">
        /// <para>目标视图模型类型，必须实现IViewModel&lt;TResult&gt;接口</para>
        /// <para>Target view model type that must implement IViewModel&lt;TResult&gt; interface</para>
        /// </typeparam>
        /// <typeparam name="TResult">
        /// <para>返回结果的类型</para>
        /// <para>Type of the return result</para>
        /// </typeparam>
        /// <param name="targetViewModel">
        /// <para>目标视图模型实例，如果为null则从服务提供程序获取</para>
        /// <para>Target view model instance, if null then get from service provider</para>
        /// </param>
        /// <param name="viewMappingKey">
        /// <para>视图映射键</para>
        /// <para>View mapping key</para>
        /// </param>
        /// <returns>
        /// <para>视图模型的结果值</para>
        /// <para>Result value from view model</para>
        /// </returns>
        /// <remarks>
        /// <para>此方法用于测试需要返回结果的视图模型显示场景。在测试环境中模拟显示过程并获取结果。</para>
        /// <para>This method is used to test view model display scenarios that need to return results. Simulates display process and gets result in testing environment.</para>
        /// </remarks>
        public async Task<TResult> ShowAndReturnResult<TTarget, TResult>(TTarget targetViewModel = null, string viewMappingKey = null) where TTarget : class, IViewModel<TResult>
        {
            var vm = targetViewModel ?? ServiceProviderLocator.RootServiceProvider.GetRequiredService<TTarget>(viewMappingKey);
            vm = await InternalTestShow(vm).ConfigureAwait(true);
            return vm.Result;

        }

        /// <summary>
        /// <para>显示视图模型并立即获取视图模型实例（WPF平台）</para>
        /// <para>Show view model and immediately get view model instance (WPF platform)</para>
        /// </summary>
        /// <typeparam name="TTarget">
        /// <para>目标视图模型类型，必须实现IViewModel接口</para>
        /// <para>Target view model type that must implement IViewModel interface</para>
        /// </typeparam>
        /// <param name="targetViewModel">
        /// <para>目标视图模型实例，如果为null则从服务提供程序获取</para>
        /// <para>Target view model instance, if null then get from service provider</para>
        /// </param>
        /// <param name="viewMappingKey">
        /// <para>视图映射键</para>
        /// <para>View mapping key</para>
        /// </param>
        /// <returns>
        /// <para>包含视图模型和关闭任务的可等待结果</para>
        /// <para>Awaitable result containing view model and closing task</para>
        /// </returns>
        /// <remarks>
        /// <para>此方法立即返回视图模型实例和关闭任务，适用于需要立即访问视图模型的测试场景。</para>
        /// <para>This method immediately returns view model instance and closing task, suitable for test scenarios that need immediate access to view model.</para>
        /// </remarks>
        public async Task<ShowAwaitableResult<TTarget>> ShowAndGetViewModelImmediately<TTarget>(TTarget targetViewModel = null, string viewMappingKey = null) where TTarget : class, IViewModel
        {
            var vm = targetViewModel ?? ServiceProviderLocator.RootServiceProvider.GetRequiredService<TTarget>(viewMappingKey);
            var vmt = InternalTestShow(vm).ConfigureAwait(true);
            return await Task.FromResult(new ShowAwaitableResult<TTarget>() { Closing = vm.WaitForClose(), ViewModel = vm }).ConfigureAwait(true);

        }
#endif 

        /// <summary>
        /// <para>模拟操作字典</para>
        /// <para>Dictionary of mocking actions</para>
        /// </summary>
        /// <remarks>
        /// <para>此字典存储针对不同视图模型类型的模拟操作。当视图模型被显示时，会执行对应的模拟操作。</para>
        /// <para>这允许在测试中预设特定的行为，模拟用户交互或业务逻辑。</para>
        /// <para>This dictionary stores mocking actions for different view model types. When view model is displayed, corresponding mocking action is executed.</para>
        /// <para>This allows presetting specific behaviors in tests, simulating user interactions or business logic.</para>
        /// </remarks>
        Dictionary<Type, Func<IViewModel, Task<IViewModel>>> mockingActionsWhenShown
            = new Dictionary<Type, Func<IViewModel, Task<IViewModel>>>();

        /// <summary>
        /// <para>模拟显示逻辑</para>
        /// <para>Mock show logic</para>
        /// </summary>
        /// <typeparam name="TTarget">
        /// <para>目标视图模型类型，必须实现IViewModel接口</para>
        /// <para>Target view model type that must implement IViewModel interface</para>
        /// </typeparam>
        /// <param name="mockingActionWhenShowing">
        /// <para>视图模型显示时执行的模拟操作委托</para>
        /// <para>Mocking action delegate to execute when view model is showing</para>
        /// </param>
        /// <remarks>
        /// <para>此方法允许为特定类型的视图模型设置模拟行为。当该类型的视图模型被显示时，会执行指定的模拟操作。</para>
        /// <para>模拟操作完成后会自动调用视图模型的关闭和释放方法。</para>
        /// <para>This method allows setting mock behavior for specific view model types. When view model of that type is displayed, specified mocking action is executed.</para>
        /// <para>After mocking action completes, view model's close and dispose methods are automatically called.</para>
        /// </remarks>
        public void MockShowLogic<TTarget>(Func<TTarget, Task<TTarget>> mockingActionWhenShowing) where TTarget : class, IViewModel
        {
            Func<IViewModel, Task<IViewModel>> asyncAction = async (m) =>
              {
                  var inp = m as TTarget;
                  var rval = await mockingActionWhenShowing(inp).ConfigureAwait(true);
                  inp.CloseViewAndDispose();
                  return rval;
              };
            mockingActionsWhenShown[typeof(TTarget)] = asyncAction;

        }



        /// <summary>
        /// <para>内部测试显示方法</para>
        /// <para>Internal test show method</para>
        /// </summary>
        /// <typeparam name="TTarget">
        /// <para>目标视图模型类型，必须实现IViewModel接口</para>
        /// <para>Target view model type that must implement IViewModel interface</para>
        /// </typeparam>
        /// <param name="vm">
        /// <para>要显示的视图模型实例</para>
        /// <para>View model instance to be displayed</para>
        /// </param>
        /// <returns>
        /// <para>处理后的视图模型实例</para>
        /// <para>Processed view model instance</para>
        /// </returns>
        /// <remarks>
        /// <para>此方法执行视图模型的测试显示逻辑。首先检查是否有对应的模拟操作，如果有则执行模拟操作；</para>
        /// <para>如果没有模拟操作，则调用视图模型的OnBindedViewLoad方法模拟正常的视图加载过程。</para>
        /// <para>This method executes test display logic for view model. First checks if there's corresponding mocking action, executes it if exists;</para>
        /// <para>If no mocking action exists, calls view model's OnBindedViewLoad method to simulate normal view loading process.</para>
        /// </remarks>
        private async Task<TTarget> InternalTestShow<TTarget>(TTarget vm) where TTarget : class, IViewModel
        {

            Func<IViewModel, Task<IViewModel>> mockingAction = null;
            if (mockingActionsWhenShown.TryGetValue(typeof(TTarget), out mockingAction))
            {
                await mockingAction(vm).ConfigureAwait(true);
            }
            else
            {
                await vm.OnBindedViewLoad(null).ConfigureAwait(true);
            }
            return vm;
        }

        /// <summary>
        /// <para>IStage接口的Show方法实现</para>
        /// <para>Implementation of IStage interface Show method</para>
        /// </summary>
        /// <typeparam name="TTarget">
        /// <para>目标视图模型类型，必须实现IViewModel接口</para>
        /// <para>Target view model type that must implement IViewModel interface</para>
        /// </typeparam>
        /// <param name="viewMappingKey">
        /// <para>视图映射键</para>
        /// <para>View mapping key</para>
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
        /// <para>此方法实现了IStage接口的Show方法，提供与实际Stage相同的接口，但在测试环境中执行。</para>
        /// <para>根据isWaitingForDispose参数决定是否等待视图模型完成其生命周期。</para>
        /// <para>This method implements IStage interface's Show method, providing same interface as actual Stage but executes in testing environment.</para>
        /// <para>Decides whether to wait for view model to complete its lifecycle based on isWaitingForDispose parameter.</para>
        /// </remarks>
        async Task<TTarget> IStage.Show<TTarget>(string viewMappingKey, Action<(IServiceProvider serviceProvider, TTarget viewModel)> additionalViewModelConfig, bool isWaitingForDispose, bool autoDisposeWhenViewUnload)
        {
            var instancedViewModel = ServiceProviderLocator.RootServiceProvider.GetRequiredService<TTarget>(viewMappingKey);
            additionalViewModelConfig?.Invoke((ServiceProvider, instancedViewModel));

            var w = InternalTestShow(instancedViewModel);
            if (isWaitingForDispose)
            {
                return await w.ConfigureAwait(true);
            }
            return instancedViewModel;
        }
    }
}
#endif
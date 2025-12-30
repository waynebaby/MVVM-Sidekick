
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MVVMSidekick.Reactive;
using MVVMSidekick.Views;

namespace MVVMSidekick.ViewModels
{

#if BLAZOR
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// <para>Blazor平台的视图模型基类，为Blazor组件提供MVVM模式支持</para>
    /// <para>Base view model class for Blazor platform, providing MVVM pattern support for Blazor components</para>
    /// </summary>
    /// <typeparam name="TViewModel">
    /// <para>视图模型类型，必须继承自此类</para>
    /// <para>View model type that must inherit from this class</para>
    /// </typeparam>
    /// <typeparam name="TView">
    /// <para>视图类型，必须继承自MVVMSidekickComponentBase</para>
    /// <para>View type that must inherit from MVVMSidekickComponentBase</para>
    /// </typeparam>
    /// <remarks>
    /// <para>此类为Blazor组件提供完整的MVVM模式支持，包含组件生命周期管理、依赖注入支持、以及页面导航功能</para>
    /// <para>This class provides complete MVVM pattern support for Blazor components, including component lifecycle management, dependency injection support, and page navigation functionality</para>
    /// </remarks>
    public class ViewModel<TViewModel, TView> : ViewModelBase<TViewModel>, IViewModelWithPlatformService where TViewModel : ViewModel<TViewModel, TView>
        where TView : MVVMSidekickComponentBase<TView, TViewModel>
    {

        /// <summary>
        /// <para>初始化视图模型实例，设置服务提供者和舞台管理器</para>
        /// <para>Initializes view model instance, setting up service provider and stage manager</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>服务提供者实例，用于依赖注入</para>
        /// <para>Service provider instance for dependency injection</para>
        /// </param>
        public ViewModel(IServiceProvider serviceProvider)
        {
            BlazorServiceProvider = serviceProvider;
            StageManager = serviceProvider.GetService<IStageManager>();
        }
        
        /// <summary>
        /// <para>关联的页面视图实例</para>
        /// <para>Associated page view instance</para>
        /// </summary>
        /// <value>
        /// <para>页面视图实例，用于视图模型与视图的交互</para>
        /// <para>Page view instance for view model and view interaction</para>
        /// </value>
        public TView Page { get; set; }


        /// <summary>
        /// <para>显示Blazor页面可能具有的3种状态：渲染前、服务器预渲染或客户端渲染</para>
        /// <para>Shows 3 possible states of a Blazor page: before render, server prerender, or client render</para>
        /// </summary>
        /// <value>
        /// <para>首次渲染状态，null表示未知，true表示首次渲染，false表示非首次渲染</para>
        /// <para>First render state, null indicates unknown, true indicates first render, false indicates non-first render</para>
        /// </value>
        public bool? IsFirstRender { get => _IsFirstRenderLocator(this).Value; protected set => _IsFirstRenderLocator(this).SetValueAndTryNotify(value); }
        #region Property bool? IsFirstRender Setup        
        protected Property<bool?> _IsFirstRender = new Property<bool?>(_IsFirstRenderLocator);
        static Func<BindableBase, ValueContainer<bool?>> _IsFirstRenderLocator = RegisterContainerLocator(nameof(IsFirstRender), m => m.Initialize(nameof(IsFirstRender), ref m._IsFirstRender, ref _IsFirstRenderLocator, default));
        #endregion


        /// <summary>
        /// <para>舞台管理器属性，用于管理视图导航和显示</para>
        /// <para>Stage manager property for managing view navigation and display</para>
        /// </summary>
        /// <value>
        /// <para>舞台管理器实例，继承自基类</para>
        /// <para>Stage manager instance inherited from base class</para>
        /// </value>
        public override IStageManager StageManager { get => base.StageManager; set => base.StageManager = value; }
        
        /// <summary>
        /// <para>Blazor服务提供者属性，用于依赖注入和服务访问</para>
        /// <para>Blazor service provider property for dependency injection and service access</para>
        /// </summary>
        /// <value>
        /// <para>用于依赖注入的服务提供者实例</para>
        /// <para>Service provider instance for dependency injection</para>
        /// </value>
        public IServiceProvider BlazorServiceProvider { get; set; }
        
        /// <summary>
        /// <para>组件初始化时调用的虚方法</para>
        /// <para>Virtual method called when component is initialized</para>
        /// </summary>
        public virtual void OnInitialized() { }
        
        /// <summary>
        /// <para>组件异步初始化时调用的虚方法</para>
        /// <para>Virtual method called when component is initialized asynchronously</para>
        /// </summary>
        /// <returns>
        /// <para>已完成的任务</para>
        /// <para>Completed task</para>
        /// </returns>
        public virtual Task OnInitializedAsync() => Task.CompletedTask;
        /// <summary>
        /// <para>参数设置时调用的虚方法</para>
        /// <para>Virtual method called when parameters are set</para>
        /// </summary>
        public virtual void OnParametersSet() { }
        
        /// <summary>
        /// <para>参数异步设置时调用的虚方法</para>
        /// <para>Virtual method called when parameters are set asynchronously</para>
        /// </summary>
        /// <returns>
        /// <para>已完成的任务</para>
        /// <para>Completed task</para>
        /// </returns>
        public virtual Task OnParametersSetAsync() => Task.CompletedTask;
        
        /// <summary>
        /// <para>渲染完成后调用的虚方法，更新首次渲染状态</para>
        /// <para>Virtual method called after rendering is complete, updates first render state</para>
        /// </summary>
        /// <param name="firstRender">
        /// <para>是否为首次渲染</para>
        /// <para>Whether this is the first render</para>
        /// </param>
        public virtual void OnAfterRender(bool firstRender)
        {
            IsFirstRender = firstRender;
        }
        
        /// <summary>
        /// <para>渲染完成后调用的异步虚方法，更新首次渲染状态</para>
        /// <para>Virtual asynchronous method called after rendering is complete, updates first render state</para>
        /// </summary>
        /// <param name="firstRender">
        /// <para>是否为首次渲染</para>
        /// <para>Whether this is the first render</para>
        /// </param>
        /// <returns>
        /// <para>已完成的任务</para>
        /// <para>Completed task</para>
        /// </returns>
        public virtual Task OnAfterRenderAsync(bool firstRender)
        {
            IsFirstRender = firstRender; return Task.CompletedTask;
        }

        /// <summary>
        /// <para>请求重新渲染页面</para>
        /// <para>Requests page to re-render</para>
        /// </summary>
        public void RequestRerender()
        {
            Page?.RequestRerender();
        }

        /// <summary>
        /// <para>异步设置参数的虚方法</para>
        /// <para>Virtual method for setting parameters asynchronously</para>
        /// </summary>
        /// <param name="parameters">
        /// <para>参数视图，包含要设置的参数</para>
        /// <para>Parameter view containing parameters to be set</para>
        /// </param>
        /// <returns>
        /// <para>已完成的任务</para>
        /// <para>Completed task</para>
        /// </returns>
        public virtual Task SetParametersAsync(ParameterView parameters) => Task.CompletedTask;
#else

    /// <summary>
    /// <para>通用平台的视图模型基类，为非Blazor平台提供MVVM模式支持</para>
    /// <para>Base view model class for general platforms, providing MVVM pattern support for non-Blazor platforms</para>
    /// </summary>
    /// <typeparam name="TViewModel">
    /// <para>视图模型类型，必须继承自此类</para>
    /// <para>View model type that must inherit from this class</para>
    /// </typeparam>
    /// <remarks>
    /// <para>此类为非Blazor平台（如WPF、UWP等）提供MVVM模式支持，包含平台特定的生命周期方法</para>
    /// <para>This class provides MVVM pattern support for non-Blazor platforms (such as WPF, UWP, etc.), including platform-specific lifecycle methods</para>
    /// </remarks>
    public class ViewModel<TViewModel> : ViewModelBase<TViewModel>, IViewModelWithPlatformService where TViewModel : ViewModel<TViewModel>
    {
#endif


#if WINDOWS_UWP || WinUI3

        /// <summary>
        /// <para>加载页面状态的虚方法，在页面导航时调用</para>
        /// <para>Virtual method to load page state, called during page navigation</para>
        /// </summary>
        /// <param name="navigationParameter">
        /// <para>导航参数，包含传递给页面的数据</para>
        /// <para>Navigation parameter containing data passed to the page</para>
        /// </param>
        /// <param name="pageState">
        /// <para>页面状态字典，包含之前保存的状态数据</para>
        /// <para>Page state dictionary containing previously saved state data</para>
        /// </param>
        public virtual void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {

        }

        /// <summary>
        /// <para>页面导航离开时调用的虚方法</para>
        /// <para>Virtual method called when page is navigated from</para>
        /// </summary>
        /// <param name="e">
        /// <para>导航事件参数</para>
        /// <para>Navigation event arguments</para>
        /// </param>
        public virtual void OnPageNavigatedFrom( NavigationEventArgs e)
        {

        }

        /// <summary>
        /// <para>页面导航到此页面时调用的虚方法</para>
        /// <para>Virtual method called when page is navigated to</para>
        /// </summary>
        /// <param name="e">
        /// <para>导航事件参数</para>
        /// <para>Navigation event arguments</para>
        /// </param>
        public virtual void OnPageNavigatedTo( NavigationEventArgs e)
        {

        }

        /// <summary>
        /// <para>页面即将导航离开时调用的虚方法，可用于取消导航</para>
        /// <para>Virtual method called when page is about to navigate away, can be used to cancel navigation</para>
        /// </summary>
        /// <param name="e">
        /// <para>可取消的导航事件参数</para>
        /// <para>Cancellable navigation event arguments</para>
        /// </param>
        public virtual void OnPageNavigatingFrom( NavigatingCancelEventArgs e)
        {
    
        }

        /// <summary>
        /// <para>保存页面状态的虚方法，在应用程序挂起或页面被丢弃时调用</para>
        /// <para>Virtual method to save page state, called when application is suspended or page is discarded</para>
        /// </summary>
        /// <param name="pageState">
        /// <para>用于保存可序列化状态的字典</para>
        /// <para>Dictionary for saving serializable state</para>
        /// </param>
        public virtual void SaveState(Dictionary<string, object> pageState)
        {
       
        }
#elif WPF
        /// <summary>
        /// <para>框架对象属性，用于WPF平台的页面导航</para>
        /// <para>Frame object property for WPF platform page navigation</para>
        /// </summary>
        /// <value>
        /// <para>WPF框架控件实例</para>
        /// <para>WPF Frame control instance</para>
        /// </value>
        public System.Windows.Controls.Frame FrameObject { get; set; }
#elif BLAZOR

#endif



    }
}

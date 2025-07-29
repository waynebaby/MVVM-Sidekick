using MVVMSidekick.Services;
using MVVMSidekick.Views;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
 

namespace MVVMSidekick.ViewModels
{



#if BLAZOR
    using Microsoft.AspNetCore.Components;

#endif
    /// <summary>
    /// <para>具有平台服务的视图模型接口，扩展基本的IViewModel接口以提供特定平台的服务和生命周期方法</para>
    /// <para>Interface for view model with platform services, extending the basic IViewModel interface to provide platform-specific services and lifecycle methods</para>
    /// </summary>
    /// <remarks>
    /// <para>此接口根据不同的编译条件提供不同平台的特定功能，包括UWP/WinUI3的导航事件处理、WPF的框架对象访问、以及Blazor的组件生命周期管理</para>
    /// <para>This interface provides platform-specific functionality based on different compilation conditions, including navigation event handling for UWP/WinUI3, frame object access for WPF, and component lifecycle management for Blazor</para>
    /// </remarks>
    public interface IViewModelWithPlatformService : IViewModel
    {
#if WINDOWS_UWP || WinUI3

        /// <summary>
        /// <para>当页面导航到此页面时调用的方法</para>
        /// <para>Method called when page is navigated to</para>
        /// </summary>
        /// <param name="e">
        /// <para>导航事件参数，包含导航相关信息</para>
        /// <para>Navigation event arguments containing navigation-related information</para>
        /// </param>
        void OnPageNavigatedTo(NavigationEventArgs e);
        
        /// <summary>
        /// <para>当页面导航离开此页面时调用的方法</para>
        /// <para>Method called when page is navigated from</para>
        /// </summary>
        /// <param name="e">
        /// <para>导航事件参数，包含导航相关信息</para>
        /// <para>Navigation event arguments containing navigation-related information</para>
        /// </param>
        void OnPageNavigatedFrom(NavigationEventArgs e);
        
        /// <summary>
        /// <para>当页面即将导航离开时调用的方法，可用于取消导航</para>
        /// <para>Method called when page is about to navigate away, can be used to cancel navigation</para>
        /// </summary>
        /// <param name="e">
        /// <para>可取消的导航事件参数，允许阻止导航操作</para>
        /// <para>Cancellable navigation event arguments allowing to prevent navigation operation</para>
        /// </param>
        void OnPageNavigatingFrom(NavigatingCancelEventArgs e);
        /// <summary>
        /// <para>使用导航期间传递的内容填充页面，在从先前会话重新创建页面时也会提供任何保存的状态</para>
        /// <para>Populates the page with content passed during navigation. Any saved state is also provided when recreating a page from a prior session</para>
        /// </summary>
        /// <param name="navigationParameter">
        /// <para>最初请求此页面时传递给Frame.Navigate(Type, Object)的参数值</para>
        /// <para>The parameter value passed to Frame.Navigate(Type, Object) when this page was initially requested</para>
        /// </param>
        /// <param name="pageState">
        /// <para>此页面在早期会话期间保留的状态字典，首次访问页面时这将为null</para>
        /// <para>A dictionary of state preserved by this page during an earlier session. This will be null the first time a page is visited</para>
        /// </param>
        void LoadState(Object navigationParameter, Dictionary<String, Object> pageState);

        /// <summary>
        /// <para>保留与此页面关联的状态，以防应用程序被挂起或页面从导航缓存中丢弃</para>
        /// <para>Preserves state associated with this page in case the application is suspended or the page is discarded from the navigation cache</para>
        /// </summary>
        /// <param name="pageState">
        /// <para>要填充可序列化状态的空字典</para>
        /// <para>An empty dictionary to be populated with serializable state</para>
        /// </param>
        void SaveState(Dictionary<String, Object> pageState);

#elif WPF
        /// <summary>
        /// <para>框架对象属性，用于访问WPF的Frame控件</para>
        /// <para>Frame object property for accessing WPF Frame control</para>
        /// </summary>
        /// <value>
        /// <para>WPF框架控件实例，用于页面导航</para>
        /// <para>WPF Frame control instance for page navigation</para>
        /// </value>
        System.Windows.Controls.Frame FrameObject { get; set; }
#elif BLAZOR
        /// <summary>
        /// <para>Blazor服务提供者，用于依赖注入和服务访问</para>
        /// <para>Blazor service provider for dependency injection and service access</para>
        /// </summary>
        /// <value>
        /// <para>用于依赖注入的服务提供者实例</para>
        /// <para>Service provider instance for dependency injection</para>
        /// </value>
        IServiceProvider BlazorServiceProvider { get; set; }
        
        /// <summary>
        /// <para>组件初始化时调用的同步方法</para>
        /// <para>Synchronous method called when component is initialized</para>
        /// </summary>
        void OnInitialized();
        
        /// <summary>
        /// <para>组件异步初始化时调用的方法</para>
        /// <para>Method called when component is initialized asynchronously</para>
        /// </summary>
        /// <returns>
        /// <para>表示异步操作的任务</para>
        /// <para>Task representing the asynchronous operation</para>
        /// </returns>
        Task OnInitializedAsync();
        
        /// <summary>
        /// <para>参数设置时调用的同步方法</para>
        /// <para>Synchronous method called when parameters are set</para>
        /// </summary>
        void OnParametersSet();
        /// <summary>
        /// <para>参数异步设置时调用的方法</para>
        /// <para>Method called when parameters are set asynchronously</para>
        /// </summary>
        /// <returns>
        /// <para>表示异步操作的任务</para>
        /// <para>Task representing the asynchronous operation</para>
        /// </returns>
        Task OnParametersSetAsync();
        
        /// <summary>
        /// <para>渲染完成后调用的同步方法</para>
        /// <para>Synchronous method called after rendering is complete</para>
        /// </summary>
        /// <param name="firstRender">
        /// <para>是否为首次渲染</para>
        /// <para>Whether this is the first render</para>
        /// </param>
        void OnAfterRender(bool firstRender);
        
        /// <summary>
        /// <para>渲染完成后调用的异步方法</para>
        /// <para>Asynchronous method called after rendering is complete</para>
        /// </summary>
        /// <param name="firstRender">
        /// <para>是否为首次渲染</para>
        /// <para>Whether this is the first render</para>
        /// </param>
        /// <returns>
        /// <para>表示异步操作的任务</para>
        /// <para>Task representing the asynchronous operation</para>
        /// </returns>
        Task OnAfterRenderAsync(bool firstRender);
        
        /// <summary>
        /// <para>异步设置参数的方法</para>
        /// <para>Method for setting parameters asynchronously</para>
        /// </summary>
        /// <param name="parameters">
        /// <para>参数视图，包含要设置的参数</para>
        /// <para>Parameter view containing parameters to be set</para>
        /// </param>
        /// <returns>
        /// <para>表示异步操作的任务</para>
        /// <para>Task representing the asynchronous operation</para>
        /// </returns>
        Task SetParametersAsync(ParameterView parameters);
        
        /// <summary>
        /// <para>请求重新渲染组件</para>
        /// <para>Requests component to re-render</para>
        /// </summary>
        void RequestRerender();
        
        /// <summary>
        /// <para>是否为首次渲染的状态属性</para>
        /// <para>Property indicating whether this is the first render</para>
        /// </summary>
        /// <value>
        /// <para>首次渲染状态，null表示未知</para>
        /// <para>First render state, null indicates unknown</para>
        /// </value>
        bool? IsFirstRender { get; }
#endif


    }

}

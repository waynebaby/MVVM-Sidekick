using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MVVMSidekick.Views
{
    /// <summary>
    /// <para>空舞台实现，提供最基本的舞台功能</para>
    /// <para>Empty stage implementation that provides basic stage functionality</para>
    /// </summary>
    internal class EmptyStage : IStage
    {
        /// <summary>
        /// <para>初始化EmptyStage的新实例</para>
        /// <para>Initializes a new instance of the EmptyStage class</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>服务提供者</para>
        /// <para>The service provider</para>
        /// </param>
        public EmptyStage(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        
        /// <summary>
        /// <para>获取或设置信标键</para>
        /// <para>Gets or sets the beacon key</para>
        /// </summary>
        public string BeaconKey { get; set; }
        
        /// <summary>
        /// <para>获取或设置是否可以后退</para>
        /// <para>Gets or sets whether can go back</para>
        /// </summary>
        public bool CanGoBack { get; set; }
        
        /// <summary>
        /// <para>获取或设置是否可以前进</para>
        /// <para>Gets or sets whether can go forward</para>
        /// </summary>
        public bool CanGoForward { get; set; }
        
        /// <summary>
        /// <para>获取或设置是否支持后退</para>
        /// <para>Gets or sets whether go back is supported</para>
        /// </summary>
        public bool IsGoBackSupported { get; set; }
        
        /// <summary>
        /// <para>获取或设置是否支持前进</para>
        /// <para>Gets or sets whether go forward is supported</para>
        /// </summary>
        public bool IsGoForwardSupported { get; set; }
        
        /// <summary>
        /// <para>获取或设置目标对象</para>
        /// <para>Gets or sets the target object</para>
        /// </summary>
        public object Target { get; set; }
        
        /// <summary>
        /// <para>获取服务提供者</para>
        /// <para>Gets the service provider</para>
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// <para>显示指定类型的视图模型</para>
        /// <para>Shows a view model of the specified type</para>
        /// </summary>
        /// <typeparam name="TTarget">
        /// <para>目标视图模型类型</para>
        /// <para>The type of the target view model</para>
        /// </typeparam>
        /// <param name="viewMappingKey">
        /// <para>视图映射键</para>
        /// <para>The view mapping key</para>
        /// </param>
        /// <param name="additionalViewModelConfig">
        /// <para>附加视图模型配置</para>
        /// <para>Additional view model configuration</para>
        /// </param>
        /// <param name="isWaitingForDispose">
        /// <para>是否等待销毁</para>
        /// <para>Whether to wait for dispose</para>
        /// </param>
        /// <param name="autoDisposeWhenViewUnload">
        /// <para>视图卸载时是否自动销毁</para>
        /// <para>Whether to auto dispose when view unloads</para>
        /// </param>
        /// <returns>
        /// <para>目标视图模型的异步任务</para>
        /// <para>Asynchronous task of the target view model</para>
        /// </returns>
        Task<TTarget> IStage.Show<TTarget>(string viewMappingKey, Action<(IServiceProvider serviceProvider, TTarget viewModel)> additionalViewModelConfig, bool isWaitingForDispose, bool autoDisposeWhenViewUnload)
        {
            var vm = ServiceProvider.GetService<TTarget>(viewMappingKey);
            additionalViewModelConfig?.Invoke((ServiceProvider, vm));
            return Task.FromResult(vm);
        }
    }
}
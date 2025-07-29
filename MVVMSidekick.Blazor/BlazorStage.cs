
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MVVMSidekick.Views
{
    /// <summary>
    /// Blazor阶段类，在Blazor中应该作为每个信标键/名称的单例存在
    /// Stage class, in blazor it should be singleton with each beacon key/name.
    /// </summary>
    public class BlazorStage : IStage
    {
        private readonly MVVMSidekickOptions pageViewModelCacheOptions;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// 初始化BlazorStage的新实例
        /// Initializes a new instance of BlazorStage
        /// </summary>
        /// <param name="navigationManager">导航管理器 / Navigation manager</param>
        /// <param name="pageViewModelCacheOptions">页面视图模型缓存选项 / Page view model cache options</param>
        /// <param name="serviceProvider">服务提供程序 / Service provider</param>
        public BlazorStage(NavigationManager navigationManager, MVVMSidekickOptions pageViewModelCacheOptions, IServiceProvider serviceProvider)
        {
            NavigationManager = navigationManager;
            this.pageViewModelCacheOptions = pageViewModelCacheOptions;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 获取信标键，始终返回空字符串
        /// Gets the beacon key, always returns empty string
        /// </summary>
        public string BeaconKey => string.Empty;

        /// <summary>
        /// 获取是否可以后退，始终返回false
        /// Gets whether can go back, always returns false
        /// </summary>
        public bool CanGoBack => false;

        /// <summary>
        /// 获取是否可以前进，始终返回false
        /// Gets whether can go forward, always returns false
        /// </summary>
        public bool CanGoForward => false;

        /// <summary>
        /// 获取是否支持后退，始终返回false
        /// Gets whether go back is supported, always returns false
        /// </summary>
        public bool IsGoBackSupported => false;

        /// <summary>
        /// 获取是否支持前进，始终返回false
        /// Gets whether go forward is supported, always returns false
        /// </summary>
        public bool IsGoForwardSupported => false;

        /// <summary>
        /// 获取目标对象，始终返回null
        /// Gets the target object, always returns null
        /// </summary>
        public object Target => null;

        /// <summary>
        /// 获取导航管理器实例
        /// Gets the navigation manager instance
        /// </summary>
        public NavigationManager NavigationManager { get; }

        /// <summary>
        /// 显示指定类型的视图模型并导航到相应页面
        /// Shows the specified type of view model and navigates to the corresponding page
        /// </summary>
        /// <typeparam name="TTarget">目标视图模型类型 / Target view model type</typeparam>
        /// <param name="viewMappingKey">视图映射键 / View mapping key</param>
        /// <param name="additionalViewModelConfig">附加视图模型配置 / Additional view model configuration</param>
        /// <param name="isWaitingForDispose">是否等待释放 / Whether waiting for dispose</param>
        /// <param name="autoDisposeWhenViewUnload">视图卸载时是否自动释放 / Whether auto dispose when view unload</param>
        /// <returns>返回视图模型实例 / Returns the view model instance</returns>
        async Task<TTarget> IStage.Show<TTarget>(string viewMappingKey, Action<(IServiceProvider serviceProvider, TTarget viewModel)> additionalViewModelConfig, bool isWaitingForDispose, bool autoDisposeWhenViewUnload)
        {
            //if ( isWaitingForDispose ||autoDisposeWhenViewUnload)
            //{
            //    throw new PlatformNotSupportedException("Platform not support 'isWaitingForDispose' or 'autoDisposeWhenViewUnload' features ");
            //}

            var instancedViewModel = serviceProvider.GetService<TTarget>(viewMappingKey)?? serviceProvider.GetRequiredService<TTarget>();
            
            additionalViewModelConfig?.Invoke((serviceProvider, instancedViewModel));

            if (pageViewModelCacheOptions.ViewModelRoutingTable.TryGetValue(typeof(TTarget), out string template))
            {
                NavigationManager.NavigateTo(template);
            }
            await Task.CompletedTask;
            return instancedViewModel;

        }
    }
}
#if BLAZOR
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
    /// <para>Blazor舞台类，在Blazor中每个信标键/名称应为单例</para>
    /// <para>Blazor stage class, in blazor it should be singleton with each beacon key/name</para>
    /// </summary>
    public class BlazorStage : IStage
    {
        private readonly MVVMSidekickOptions pageViewModelCacheOptions;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// <para>初始化BlazorStage类的新实例</para>
        /// <para>Initializes a new instance of the BlazorStage class</para>
        /// </summary>
        /// <param name="navigationManager">导航管理器 / Navigation manager</param>
        /// <param name="pageViewModelCacheOptions">页面视图模型缓存选项 / Page view model cache options</param>
        /// <param name="serviceProvider">服务提供者 / Service provider</param>
        public BlazorStage(NavigationManager navigationManager, MVVMSidekickOptions pageViewModelCacheOptions, IServiceProvider serviceProvider)
        {
            NavigationManager = navigationManager;
            this.pageViewModelCacheOptions = pageViewModelCacheOptions;
            this.serviceProvider = serviceProvider;
        }



        public string BeaconKey => string.Empty;

        public bool CanGoBack => false;

        public bool CanGoForward => false;

        public bool IsGoBackSupported => false;

        public bool IsGoForwardSupported => false;

        public object Target => null;

        public NavigationManager NavigationManager { get; }



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
#endif

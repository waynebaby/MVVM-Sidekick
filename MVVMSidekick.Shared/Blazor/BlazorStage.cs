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
    /// Stage class, in blazor it should be singleton with each beacon key/name.
    /// </summary>
    public class BlazorStage : IStage
    {
        private readonly MVVMSidekickOptions pageViewModelCacheOptions;
        private readonly IServiceProvider serviceProvider;

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

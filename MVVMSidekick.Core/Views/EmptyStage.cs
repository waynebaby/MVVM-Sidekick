using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MVVMSidekick.Views
{
    internal class EmptyStage : IStage
    {
        public EmptyStage(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        public string BeaconKey { get; set; }
        public bool CanGoBack { get; set; }
        public bool CanGoForward { get; set; }
        public bool IsGoBackSupported { get; set; }
        public bool IsGoForwardSupported { get; set; }
        public object Target { get; set; }
        public IServiceProvider ServiceProvider { get; }

        Task<TTarget> IStage.Show<TTarget>(string viewMappingKey, Action<(IServiceProvider serviceProvider, TTarget viewModel)> additionalViewModelConfig, bool isWaitingForDispose, bool autoDisposeWhenViewUnload)
        {
            var vm = ServiceProvider.GetService<TTarget>(viewMappingKey);
            additionalViewModelConfig?.Invoke((ServiceProvider, vm));
            return Task.FromResult(vm);
        }
    }
}
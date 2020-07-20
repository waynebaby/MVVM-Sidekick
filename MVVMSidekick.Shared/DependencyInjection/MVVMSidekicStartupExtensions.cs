#if BLAZOR

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

#endif
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick;
using MVVMSidekick.Services;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MVVMSidekicStartupExtensions
    {


        public static IServiceCollection AddMVVMSidekick<TViewModelRegistry>(this IServiceCollection services,
        Action<MVVMSidekickOptions> optionalConfiguration = null)
            where TViewModelRegistry : MVVMSidekickStartupBase, new()
        {
            var opt = new MVVMSidekickOptions(services);
#if BLAZOR
            services.AddTransient<IStage, BlazorStage>();
            services.AddTransient<IStageManager, BlazorStageManager>();

            services.AddSingleton(opt);
#else
            services.AddTransient<IStage, Stage>();
            services.AddTransient<IStageManager, StageManager>();
#endif
            services.AddSingleton<ITellDesignTimeService, InRuntime>();

            var buildervm = new TViewModelRegistry();
            buildervm.ConfigureInstance(opt);
            optionalConfiguration?.Invoke(opt);
            return services;
        }


#if BLAZOR
        public static WebAssemblyHost PushToMVVMSidekickRoot(this WebAssemblyHost host)
        {
            ServiceProviderLocator.RootServiceProvider = host.Services;
            return host;
        }
#else
        public static IServiceProvider PushToMVVMSidekickRoot(this IServiceProvider serviceProvider)
        {
            ServiceProviderLocator.RootServiceProvider = serviceProvider;
            return serviceProvider;
        }
#endif
    }
}

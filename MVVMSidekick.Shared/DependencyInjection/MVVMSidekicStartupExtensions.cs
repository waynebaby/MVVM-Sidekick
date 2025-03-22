
#if WEBASM
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


        public static IServiceCollection AddMVVMSidekick<TViewModelRegistry>(this IServiceCollection services, TViewModelRegistry instance,
        Action<MVVMSidekickOptions> optionalConfiguration = null)
            where TViewModelRegistry : MVVMSidekickStartupBase 
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

            var buildervm =  instance;
            buildervm.ConfigureInstance(opt);
            optionalConfiguration?.Invoke(opt);
            return services;
        }


#if WEBASM
 
        public static WebAssemblyHost PushToMVVMSidekickRoot(this WebAssemblyHost host)
        {
            ServiceProviderLocator.RootServiceProvider = host.Services;
            return host;
        }
#elif MAUI_BLAZOR
        public static MauiApp PushToMVVMSidekickRoot(this MauiApp  host)
        {
            ServiceProviderLocator.RootServiceProvider = host.Services ;
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

using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MVVMSidekickBlazorHelper
    {



        public static IServiceCollection AddMVVMSidekick<TViewModelRegistry>(this IServiceCollection services,
            Action<IServiceProvider, MVVMSidekickOptions> optionsConfiguration = null) where TViewModelRegistry : MVVMSidekickViewModelRegistryBase, new()
        {
            services.AddTransient<IStage, BlazorStage>();
            services.AddTransient<IStageManager, BlazorStageManager>();



            var builder = new TViewModelRegistry();
            var opt = new MVVMSidekickOptions(services);
            builder.ConfigureInstance(opt);

            services.AddSingleton(sp =>
            {
                optionsConfiguration?.Invoke(sp, opt);
                return opt;
            });



            return services;
        }
    }
}

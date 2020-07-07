using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMSidekick.Blazor
{
  public   static class  MVVMSidekickBlazorHelper
    {

        public static IServiceCollection AddMVVMSidekick(this IServiceCollection services, Action<ViewModelOptions> registerViewModels )
        {
            services.AddSingleton<IStage,Stage>();
            services.AddSingleton<IStageManager,StageManager>();
            var opt = new ViewModelOptions(services);
            registerViewModels?.Invoke(opt);
            services.AddSingleton(opt);
            return services;
        }
    }
}

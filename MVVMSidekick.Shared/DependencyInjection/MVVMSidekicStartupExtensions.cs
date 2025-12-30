
/// <summary>
/// <para>MVVM-Sidekick启动扩展，提供依赖注入容器配置的扩展方法</para>
/// <para>MVVM-Sidekick startup extensions, providing extension methods for dependency injection container configuration</para>
/// </summary>

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
    /// <summary>
    /// <para>MVVM-Sidekick启动扩展类，提供服务容器配置的静态扩展方法</para>
    /// <para>MVVM-Sidekick startup extensions class, providing static extension methods for service container configuration</para>
    /// </summary>
    public static class MVVMSidekicStartupExtensions
    {
        /// <summary>
        /// <para>将MVVM-Sidekick服务添加到依赖注入容器</para>
        /// <para>Adds MVVM-Sidekick services to dependency injection container</para>
        /// </summary>
        /// <typeparam name="TViewModelRegistry">视图模型注册表类型 / View model registry type</typeparam>
        /// <param name="services">服务集合 / Service collection</param>
        /// <param name="instance">视图模型注册表实例 / View model registry instance</param>
        /// <param name="optionalConfiguration">可选配置委托 / Optional configuration delegate</param>
        /// <returns>服务集合 / Service collection</returns>
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
        /// <summary>
        /// <para>将WebAssembly主机推送到MVVM-Sidekick根服务提供者</para>
        /// <para>Pushes WebAssembly host to MVVM-Sidekick root service provider</para>
        /// </summary>
        /// <param name="host">WebAssembly主机 / WebAssembly host</param>
        /// <returns>WebAssembly主机 / WebAssembly host</returns>
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

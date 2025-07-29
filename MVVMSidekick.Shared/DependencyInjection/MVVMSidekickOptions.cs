
// MVVM-Sidekick选项配置，提供依赖注入和配置功能
// MVVM-Sidekick options configuration, providing dependency injection and configuration functionality

using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Windows;
#if WINDOWS_UWP
using Windows.UI.Xaml;
#endif
#if BLAZOR
using Microsoft.AspNetCore.Components;

#endif
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <para>MVVM-Sidekick选项类，用于配置MVVM框架的依赖注入和服务注册</para>
    /// <para>MVVM-Sidekick options class, used for configuring dependency injection and service registration of MVVM framework</para>
    /// </summary>
    public class MVVMSidekickOptions
    {
        private readonly IServiceCollection services;
#if BLAZOR
        /// <summary>
        /// <para>视图模型路由表，用于Blazor平台的视图模型路由映射</para>
        /// <para>View model routing table, used for view model routing mapping in Blazor platform</para>
        /// </summary>
        internal Dictionary<Type, string> ViewModelRoutingTable = new Dictionary<Type, string>();
#endif
        /// <summary>
        /// <para>初始化MVVMSidekickOptions类的新实例</para>
        /// <para>Initializes a new instance of the MVVMSidekickOptions class</para>
        /// </summary>
        /// <param name="services">服务集合 / Service collection</param>
        public MVVMSidekickOptions(IServiceCollection services)
        {
            this.services = services;
        }
        
        /// <summary>
        /// <para>注册视图模型到依赖注入容器</para>
        /// <para>Registers view model to dependency injection container</para>
        /// </summary>
        /// <typeparam name="TViewModel">视图模型类型 / View model type</typeparam>
        /// <param name="name">服务名称 / Service name</param>
        /// <param name="viewModelConfig">视图模型配置委托 / View model configuration delegate</param>
        /// <returns>MVVMSidekickOptions实例 / MVVMSidekickOptions instance</returns>
        public MVVMSidekickOptions RegisterViewModel<TViewModel>(string name = default, Action<IServiceProvider, TViewModel> viewModelConfig = default) where TViewModel : class, IViewModelWithPlatformService, IViewModel
        {

#if BLAZOR
            services.ConfigNamed().AddSingleton<TViewModel>(name);
            // 获取基类型枚举序列 / Gets base type enumerable sequence
            IEnumerable<Type> GetBaseTypeEnumerable(Type start)
            {
                var current = start.BaseType;
                while (current != null)
                {
                    yield return current;
                    current = current.BaseType;
                }
            }

            var ag = GetBaseTypeEnumerable(typeof(TViewModel)).First(x => x.GetGenericTypeDefinition() == typeof(ViewModel<,>)).GetGenericArguments()[1];
            var route = ag.GetCustomAttribute(typeof(RouteAttribute), true) as RouteAttribute;
            ViewModelRoutingTable[typeof(TViewModel)] = route.Template;
#else
            if (viewModelConfig == null)
            {
                services.ConfigNamed().AddTransient<TViewModel>(name);

            }
            else
            {

                services.ConfigNamed().AddTransient<TViewModel>(name, sp =>
                 {
                     var item = sp.GetRequiredService<TViewModel>();
                     viewModelConfig.Invoke(sp, item);
                     return item;
                 });


            }
#endif

            return this;
        }

#if !BLAZOR
        public MVVMSidekickOptions RegisterView<TView>(string name = default, Action<IServiceProvider, TView> viewConfig = default) where TView : FrameworkElement
        {


#if WPF
            if (viewConfig == null)
            {
                services.AddTransient<TView>();
            }
            else
            {
                if (!services.Any(x => x.ServiceType.Equals(typeof(TView))))
                {
                    services.AddTransient<TView>();
                }
                services.ConfigNamed().AddTransient<TView>(name, sp =>
                {
                    var item = sp.GetRequiredService<TView>();
                    viewConfig.Invoke(sp, item);
                    return item;
                });
            }

#elif WINDOWS_UWP
            services.ConfigNamed().AddSingleton(name, sp => new ViewContentConfigurator<TView>(sp) { Action = viewConfig });

#endif
            return this;
        }


        public MVVMSidekickOptions RegisterViewAndModelMapping<TView, TViewModel>(string mappingKey = default, Action<IServiceProvider, TView> viewConfig = default, Action<IServiceProvider, TViewModel> viewModelConfig = default)
             where TView : FrameworkElement where TViewModel : class, IViewModelWithPlatformService
        {
            this.RegisterView<TView>(mappingKey, viewConfig)
               .RegisterViewModel<TViewModel>(mappingKey, viewModelConfig);
            ViewAndModelMappingsHelper.DefaultViewToVMMapping.Add((mappingKey, typeof(TView)), (mappingKey, typeof(TViewModel)));
            ViewAndModelMappingsHelper.DefaultVMToViewMapping.Add((mappingKey, typeof(TViewModel)), (mappingKey, typeof(TView)));
            return this;
        }
#endif
    }
#if WINDOWS_UWP || WinUI3
    public interface IViewContentConfigurator
    {
        void Config(object viewContent);
    }

    public class ViewContentConfigurator<TView> : IViewContentConfigurator where TView : FrameworkElement
    {


        public ViewContentConfigurator(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

        }

        public IServiceProvider ServiceProvider { get; }
        public Action<IServiceProvider, TView> Action { get; set; }

        public void Config(object viewContent)
        {
            var vc = (FrameworkElement)viewContent;
            Action?.Invoke(ServiceProvider, vc.Parent as TView);
        }
    }

#endif
}
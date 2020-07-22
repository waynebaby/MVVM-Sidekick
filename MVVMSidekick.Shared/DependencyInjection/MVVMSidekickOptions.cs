

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
    public class MVVMSidekickOptions
    {
        private readonly IServiceCollection services;
#if BLAZOR
        internal Dictionary<Type, string> ViewModelRoutingTable = new Dictionary<Type, string>();
#endif
        public MVVMSidekickOptions(IServiceCollection services)
        {
            this.services = services;
        }
        public MVVMSidekickOptions RegisterViewModel<TViewModel>(string name = default, Action<IServiceProvider, TViewModel> viewModelConfig = default) where TViewModel : class, IViewModelWithPlatformService, IViewModel
        {


#if BLAZOR
            services.ConfigNamed().AddSingleton<TViewModel>(name);
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
#if WINDOWS_UWP
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
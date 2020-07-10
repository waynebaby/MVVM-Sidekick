

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace MVVMSidekick.ViewModels
{
    public class MVVMSidekickOptions
    {
        private readonly IServiceCollection services;
        internal Dictionary<Type, string> ViewModelRoutingTable = new Dictionary<Type, string>();
        public MVVMSidekickOptions(IServiceCollection services)
        {
            this.services = services;
        }
        public MVVMSidekickOptions RegisterViewModel<TViewModel>() where TViewModel : class, IViewModelWithPlatformService
        {
            IEnumerable<Type> GetBaseTypeEnumerable(Type start)
            {
                var current = start.BaseType;
                while (current != null)
                {
                    yield return current;
                    current = current.BaseType;
                }
            }

            services.AddTransient<TViewModel>();

            var ag = GetBaseTypeEnumerable(typeof(TViewModel)).First(x => x.GetGenericTypeDefinition() == typeof(ViewModel<,>)).GetGenericArguments()[1];
            var route = ag.GetCustomAttribute(typeof(RouteAttribute), true) as RouteAttribute;
            ViewModelRoutingTable[typeof(TViewModel)] = route.Template;


            return this;
        }
    }
}
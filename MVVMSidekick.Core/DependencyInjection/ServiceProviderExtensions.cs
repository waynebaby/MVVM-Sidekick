// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Services.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Concurrent;
using MVVMSidekick.Services;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        static ConcurrentDictionary<IServiceCollection, INamedServiceCollection> NamedServiceSupportAdded
                    = new System.Collections.Concurrent.ConcurrentDictionary<IServiceCollection, INamedServiceCollection>();


        public static INamedServiceCollection ConfigNamed(this IServiceCollection services)
        {
            return NamedServiceSupportAdded.GetOrAdd(services,
                svs =>
                {
                    ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>> factoryDataCore
                            = new ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>>();

                    services.AddSingleton(factoryDataCore);

                    return new NamedServiceCollection(services, factoryDataCore);
                });
        }

        public static INamedServiceCollection AddSingleton<TService>(this INamedServiceCollection services, string name, TService instance) where TService : class
        {
            if (String.IsNullOrEmpty(name))
            {
                services.Core.AddSingleton(instance);

            }
            else
            {
                Func<IServiceProvider, TService> factoryEntry = sp => instance;
                services.FactoryData.AddOrUpdate((name, typeof(TService)), factoryEntry, (k, oldv) => factoryEntry);

            }
            return services;
        }

        public static INamedServiceCollection AddSingleton<TService>(this INamedServiceCollection services, string name, Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            if (String.IsNullOrEmpty(name))
            {
                services.Core.AddSingleton(implementationFactory);

            }
            else
            {

                var lazyBox = new LateLazyBox<TService>();
                Func<IServiceProvider, TService> factoryEntry = sp =>
                {
                    var lazy = lazyBox.SetLazy(() => implementationFactory(sp)).Lazy;
                    return lazy.Value;
                };
                services.FactoryData.AddOrUpdate((name, typeof(TService)), factoryEntry, (k, oldv) => factoryEntry);

            }
            return services;

        }
        public static INamedServiceCollection AddSingleton<TService>(this INamedServiceCollection services, string name) where TService : class
        {
            if (String.IsNullOrEmpty(name))
            {
                services.Core.AddSingleton<TService>();
            }
            else
            {
                var lazyBox = new LateLazyBox<TService>();
                Func<IServiceProvider, TService> factoryEntry = sp =>
                {
                    var lazy = lazyBox.SetLazy(() => sp.GetService<TService>()).Lazy;
                    return lazy.Value;
                };
                services.FactoryData.AddOrUpdate((name, typeof(TService)), sp => factoryEntry, (k, oldv) => factoryEntry);
            }

            return services;
        }


        public static INamedServiceCollection AddTransient<TService>(this INamedServiceCollection services, string name, Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            if (String.IsNullOrEmpty(name))
            {
                services.Core.AddTransient(implementationFactory);
            }
            else
            {
                services.FactoryData.AddOrUpdate((name, typeof(TService)), implementationFactory, (k, oldv) => implementationFactory);
            }
            return services;
        }

        public static INamedServiceCollection AddTransient<TService>(this INamedServiceCollection services, string name) where TService : class
        {


            if (String.IsNullOrEmpty(name))
            {
                services.Core.AddTransient<TService>();

            }
            else
            {
                Func<IServiceProvider, TService> factoryEntry = sp => sp.GetService<TService>();
                services.FactoryData.AddOrUpdate((name, typeof(TService)), factoryEntry, (k, oldv) => factoryEntry);
            }
            return services;
        }
        public static INamedServiceCollection AddTransient<TService, TImplementation>(this INamedServiceCollection services, string name)
            where TService : class
            where TImplementation : class, TService
        {

            if (String.IsNullOrEmpty(name))
            {
                services.Core.AddTransient<TService, TImplementation>();

            }
            else
            {
                Func<IServiceProvider, TService> factoryEntry = sp => sp.GetService<TImplementation>();
                services.FactoryData.AddOrUpdate((name, typeof(TService)), factoryEntry, (k, oldv) => factoryEntry);
            }
            return services;
        }

        public static object GetService(this IServiceProvider serviceProvider,  string name  , Type serviceType)
        {

            if (String.IsNullOrEmpty(name))
            {
                return serviceProvider.GetService(serviceType);

            }
            else
            {
                var factoryData = serviceProvider.GetRequiredService<ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>>>();

                if (factoryData.TryGetValue((name, serviceType), out var factory))
                {
                    return factory(serviceProvider);
                }
                else
                {
                    return null;
                }
            }

        }
        public static TService GetService<TService>(this IServiceProvider serviceProvider, string name)
        {

            if (String.IsNullOrEmpty(name))
            {
                return serviceProvider.GetService<TService>();

            }
            else
            {
                var factoryData = serviceProvider.GetRequiredService<ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>>>();

                if (factoryData.TryGetValue((name, typeof(TService)), out var factory))
                {
                    return (TService)factory(serviceProvider);
                }
                else
                {
                    return default(TService);
                }
            }
        }
        public static TService GetRequiredService<TService>(this IServiceProvider serviceProvider, string name)
        {

            if (String.IsNullOrEmpty(name))
            {
                return serviceProvider.GetService<TService>();

            }
            else
            {
                var factoryData = serviceProvider.GetRequiredService<ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>>>();

                if (factoryData.TryGetValue((name, typeof(TService)), out var factory))
                {
                    return (TService)factory(serviceProvider);


                }
                else
                {
                    throw new InvalidOperationException($"This named service -{typeof(TService)},'{ name }' - is not registered correctly in Service Collection");
                }
            }
        }

        private class LateLazyBox<TService>
        {
            public Lazy<TService> Lazy { get; private set; }

            public LateLazyBox<TService> SetLazy(Func<TService> factory)
            {
                lock (this)
                {
                    if (Lazy != null)
                    {
                        Lazy = new Lazy<TService>(factory, true);
                    }

                }
                return this;

            }
        }
    }

}
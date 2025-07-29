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
    /// <summary>
    /// 服务提供程序扩展类，为依赖注入容器提供命名服务支持
    /// Service provider extensions class providing named service support for dependency injection container
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// 已添加命名服务支持的服务集合缓存
        /// Cache of service collections that have named service support added
        /// </summary>
        static ConcurrentDictionary<IServiceCollection, INamedServiceCollection> NamedServiceSupportAdded
                    = new System.Collections.Concurrent.ConcurrentDictionary<IServiceCollection, INamedServiceCollection>();

        /// <summary>
        /// 为服务集合配置命名服务支持
        /// Configures named service support for service collection
        /// </summary>
        /// <param name="services">服务集合 / Service collection</param>
        /// <returns>命名服务集合 / Named service collection</returns>
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

        /// <summary>
        /// 添加单例服务实例到命名服务集合
        /// Adds singleton service instance to named service collection
        /// </summary>
        /// <typeparam name="TService">服务类型 / Service type</typeparam>
        /// <param name="services">命名服务集合 / Named service collection</param>
        /// <param name="name">服务名称 / Service name</param>
        /// <param name="instance">服务实例 / Service instance</param>
        /// <returns>命名服务集合 / Named service collection</returns>
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

        /// <summary>
        /// 使用工厂方法添加单例服务到命名服务集合
        /// Adds singleton service using factory method to named service collection
        /// </summary>
        /// <typeparam name="TService">服务类型 / Service type</typeparam>
        /// <param name="services">命名服务集合 / Named service collection</param>
        /// <param name="name">服务名称 / Service name</param>
        /// <param name="implementationFactory">实现工厂方法 / Implementation factory method</param>
        /// <returns>命名服务集合 / Named service collection</returns>
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
        
        /// <summary>
        /// 按类型添加单例服务到命名服务集合
        /// Adds singleton service by type to named service collection
        /// </summary>
        /// <typeparam name="TService">服务类型 / Service type</typeparam>
        /// <param name="services">命名服务集合 / Named service collection</param>
        /// <param name="name">服务名称 / Service name</param>
        /// <returns>命名服务集合 / Named service collection</returns>
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
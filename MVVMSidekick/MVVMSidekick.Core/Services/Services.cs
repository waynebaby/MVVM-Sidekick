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
using System.Collections.Generic;
using System.Threading.Tasks;
using MVVMSidekick.Utilities;
using Unity;
using MVVMSidekick.Views;
using System.Globalization;

namespace MVVMSidekick
{


    namespace Services
    {

        /// <summary>
        /// Class TypeSpecifiedServiceLocatorBase.
        /// </summary>
        /// <typeparam name="TSubClass">The type of the t sub class.</typeparam>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        public class TypeSpecifiedServiceLocatorBase<TSubClass, TService> : ITypeSpecifiedServiceLocator<TService>
            where TSubClass : TypeSpecifiedServiceLocatorBase<TSubClass, TService>
        {
            /// <summary>
            /// Registers the specified instance.
            /// </summary>
            /// <param name="instance">The instance.</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            public ServiceLocatorEntryStruct<TService> Register(TService instance)
            {

                return Register(null, instance);
            }

            /// <summary>
            /// Registers the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="instance">The instance.</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            public ServiceLocatorEntryStruct<TService> Register(string name, TService instance)
            {
                name = name ?? "";
                return dic[name] =
                        new ServiceLocatorEntryStruct<TService>(name)
                        {
                            CacheType = CacheType.Instance,
                            ServiceInstance = instance,
                        };
            }

            /// <summary>
            /// Registers the specified factory.
            /// </summary>
            /// <param name="factory">The factory.</param>
            /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            public ServiceLocatorEntryStruct<TService> Register(Func<object, TService> factory, bool isAlwaysCreatingNew = true)
            {
                return Register(null, factory, isAlwaysCreatingNew);
            }

            /// <summary>
            /// Registers the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="factory">The factory.</param>
            /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            public ServiceLocatorEntryStruct<TService> Register(string name, Func<object, TService> factory, bool isAlwaysCreatingNew = true)
            {
                name = name ?? "";

                if (isAlwaysCreatingNew)
                {
                    return dic[name] = new ServiceLocatorEntryStruct<TService>(name)
                    {
                        CacheType = CacheType.Factory,
                        ServiceFactory = factory
                    };

                }
                else
                {

                    return dic[name] = new ServiceLocatorEntryStruct<TService>(name)
                    {
                        CacheType = CacheType.LazyInstance,
                        ServiceFactory = factory
                    };

                }


            }

            /// <summary>
            /// Resolves the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="parameters">The parameters.</param>
            /// <returns>TService.</returns>
            public TService Resolve(string name = null, object parameters = null)
            {
                name = name ?? "";
                var subdic = dic;
                ServiceLocatorEntryStruct<TService> entry = null;
                if (subdic.TryGetValue(name, out entry))
                {
                    return entry.GetService(parameters);
                }
                else
                    return default(TService);
            }




            /// <summary>
            /// The dic
            /// </summary>
            static Dictionary<string, ServiceLocatorEntryStruct<TService>> dic
               = new Dictionary<string, ServiceLocatorEntryStruct<TService>>();




            /// <summary>
            /// Determines whether the specified name has instance.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns><c>true</c> if the specified name has instance; otherwise, <c>false</c>.</returns>
            public bool HasInstance(string name = null)
            {
                name = name ?? "";
                ServiceLocatorEntryStruct<TService> entry = null;
                if (dic.TryGetValue(name, out entry))
                {
                    return
                       entry.GetIsValueCreated();

                }
                else
                {
                    return false;
                }
            }


            /// <summary>
            /// Registers the specified asynchronous factory.
            /// </summary>
            /// <param name="asyncFactory">The asynchronous factory.</param>
            /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            public ServiceLocatorEntryStruct<TService> Register(Func<object, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true)
            {
                return Register(null, asyncFactory, isAlwaysCreatingNew);
            }

            /// <summary>
            /// Registers the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="asyncFactory">The asynchronous factory.</param>
            /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            public ServiceLocatorEntryStruct<TService> Register(string name, Func<object, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true)
            {
                name = name ?? "";

                if (isAlwaysCreatingNew)
                {
                    return dic[name] = new ServiceLocatorEntryStruct<TService>(name)
                    {
                        CacheType = CacheType.AsyncFactory,
                        AsyncServiceFactory = asyncFactory
                    };

                }
                else
                {
                    return dic[name] = new ServiceLocatorEntryStruct<TService>(name)
                    {
                        CacheType = CacheType.AsyncLazyInstance,
                        AsyncServiceFactory = asyncFactory
                    };

                }

            }

            /// <summary>
            /// resolve as an asynchronous operation.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="parameter">The parameter.</param>
            /// <returns>Task&lt;TService&gt;.</returns>
            public async Task<TService> ResolveAsync(string name = null, object parameter = null)
            {
                name = name ?? "";
                var subdic = dic;
                ServiceLocatorEntryStruct<TService> entry = null;
                if (subdic.TryGetValue(name, out entry))
                {
                    return await entry.GetServiceAsync();
                }
                else
                    return await Task.FromResult(default(TService));

            }


            /// <summary>
            /// Determines whether the specified name is asynchronous.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns><c>true</c> if the specified name is asynchronous; otherwise, <c>false</c>.</returns>
            /// <exception cref="System.ArgumentException">No such key</exception>
            public bool IsAsync(string name = null)
            {
                name = name ?? "";
                ServiceLocatorEntryStruct<TService> entry = null;
                if (dic.TryGetValue(name, out entry))
                {
                    return
                       entry.GetIsValueCreated();

                }
                else
                {
                    throw new ArgumentException("No such key");
                }
            }
        }

        /// <summary>
        /// Enum CacheType
        /// </summary>
        public enum CacheType
        {
            /// <summary>
            /// The instance
            /// </summary>
            Instance,
            /// <summary>
            /// The factory
            /// </summary>
            Factory,
            /// <summary>
            /// The lazy instance
            /// </summary>
            LazyInstance,
            /// <summary>
            /// The asynchronous factory
            /// </summary>
            AsyncFactory,
            /// <summary>
            /// The asynchronous lazy instance
            /// </summary>
            AsyncLazyInstance
        }


        /// <summary>
        /// Interface IServiceLocator
        /// </summary>
        public interface IServiceLocator
        {
            /// <summary>
            /// Determines whether the specified name has instance.
            /// </summary>
            /// <typeparam name="TService">The type of the t service.</typeparam>
            /// <param name="name">The name.</param>
            /// <returns><c>true</c> if the specified name has instance; otherwise, <c>false</c>.</returns>
            bool HasInstance<TService>(string name =null);

            TService TryResolve<TService>(Func<TService> backupPlan, String name = null);

            /// <summary>
            /// Registers the typeMapping
            /// </summary>
            IServiceLocator Register<TFrom, TTo>(string name =null)
                where TTo : TFrom;

            /// <summary>
            /// Registers the specified instance.
            /// </summary>
            /// <typeparam name="TService">The type of the t service.</typeparam>
            /// <param name="instance">The instance.</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            IServiceLocator Register<TService>(TService instance);
            /// <summary>
            /// Registers the specified name.
            /// </summary>
            /// <typeparam name="TService">The type of the t service.</typeparam>
            /// <param name="name">The name.</param>
            /// <param name="instance">The instance.</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            IServiceLocator Register<TService>(string name, TService instance);

            /// <summary>
            /// Registers the specified name.
            /// </summary>
            /// <typeparam name="TService">The type of the t service.</typeparam>
            /// <param name="name">The name.</param>
            /// <param name="factory">The factory.</param>
            /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
            /// <returns>
            /// ServiceLocatorEntryStruct&lt;TService&gt;.
            /// </returns>
            IServiceLocator RegisterFactory<TService>(string name, Func<object, IServiceLocator, TService> factory, bool isAlwaysCreatingNew = true);
            /// <summary>
            /// Resolves the specified name.
            /// </summary>
            /// <typeparam name="TService">The type of the service.</typeparam>
            /// <param name="name">The name.</param>
            /// <param name="parameter">The parameter.</param>
            /// <returns>
            /// TService.
            /// </returns>
            TService Resolve<TService>(string name = null);

            /// <summary>
            /// Registers the specified asynchronous factory.
            /// </summary>
            /// <typeparam name="TService">The type of the t service.</typeparam>
            /// <param name="asyncFactory">The asynchronous factory.</param>
            /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            IServiceLocator RegisterAsyncFactory<TService>(Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
            /// <summary>
            /// Registers the specified name.
            /// </summary>
            /// <typeparam name="TService">The type of the t service.</typeparam>
            /// <param name="name">The name.</param>
            /// <param name="asyncFactory">The asynchronous factory.</param>
            /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            IServiceLocator RegisterAsyncFactory<TService>(string name, Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);


            /// <summary>
            /// Resolves the asynchronous.
            /// </summary>
            /// <typeparam name="TService"></typeparam>
            /// <param name="name">The name.</param>
            /// <param name="parameter">The parameter.</param>
            /// <returns>Task&lt;TService&gt;.</returns>
            TService ResolveFactory<TService>(string name = null, object parameter = null);

            /// <summary>
            /// Resolves the asynchronous.
            /// </summary>
            /// <typeparam name="TService"></typeparam>
            /// <param name="name">The name.</param>
            /// <param name="parameter">The parameter.</param>
            /// <returns>Task&lt;TService&gt;.</returns>
            Task<TService> ResolveAsyncFactoryAsync<TService>(string name = null, object parameter = null);
        }

        public class ResolveFactory<TService>
        {
            public ResolveFactory(Func<object, IServiceLocator, TService> factory, bool isAlwaysCreatingNew)
            {
                IsAlwaysCreatingNew = isAlwaysCreatingNew;
                _factory = factory;
            }

            public bool IsAlwaysCreatingNew { get; private set; }

            private TService _instance;
            private bool _executed;
            private object _lock = new object();
            private Func<object, IServiceLocator, TService> _factory;

            public virtual TService GetInstance(object parameter, IServiceLocator locator)
            {
                if (IsAlwaysCreatingNew)
                {
                    return _factory(parameter, locator);
                }
                else
                {
                    lock (_lock)
                    {
                        if (_executed)
                        {
                            return _instance;
                        }
                        else
                        {
                            _instance = _factory(parameter, locator);
                            _executed = true;
                            return _instance;
                        }
                    }
                }
            }


        }


        public static class ServiceLocatorExtensions
        {

            public static TServiceLocator Configure<TServiceLocator>(this TServiceLocator instance, Action<TServiceLocator> configureBody) where TServiceLocator : ServiceLocator
            {
                configureBody(instance);
                return instance;
            }
            public static async Task<TServiceLocator> ConfigureAsync<TServiceLocator>(this Task<TServiceLocator> instanceTask, Action<TServiceLocator> configureBody) where TServiceLocator : ServiceLocator
            {
                var instance = await instanceTask;
                instance.Configure(configureBody);
                return instance;
            }
            public static async Task<TServiceLocator> ConfigureAsync<TServiceLocator>(this TServiceLocator instance, AsyncAction<TServiceLocator> configureBody) where TServiceLocator : ServiceLocator
            {
                await configureBody(instance);
                return instance;
            }
            public static async Task<TServiceLocator> ConfigureAsync<TServiceLocator>(this Task<TServiceLocator> instanceTask, AsyncAction<TServiceLocator> configureBody) where TServiceLocator : ServiceLocator
            {
                var instance = await instanceTask;
                await instance.ConfigureAsync(configureBody);
                return instance;
            }
        }



        public delegate Task AsyncAction<T>(T input);

        public abstract class ServiceLocator : IServiceLocator
        {

            static internal Lazy<ServiceLocator> _defaultInstance
                = new Lazy<ServiceLocator>(() => new UnityServiceLocator(), true);

            static IServiceLocator _instance;

            public static IServiceLocator Instance
            {
                get
                {
                    lock (_defaultInstance)
                    {
                        if (_instance == null)
                        {
                            _instance = _defaultInstance.Value;
                        }

                    }
                    return _instance;
                }
            }


            /// <summary>
            ///  SetInstance
            /// </summary>
            /// <typeparam name="TServiceLocator"></typeparam>
            /// <param name="instance"></param>
            /// <returns>IServiceLocator because we don't want the instance be configured after had been set to instance. </returns>
            public static IServiceLocator SetInstance<TServiceLocator>(TServiceLocator instance) where TServiceLocator : ServiceLocator
            {

                lock (_defaultInstance)
                {
                    _instance = instance;
                }
                return instance;
            }






            public abstract bool HasInstance<TService>(string name = null);
            public abstract IServiceLocator Register<TService>(TService instance);
            public abstract IServiceLocator Register<TService>(string name, TService instance);
            public abstract IServiceLocator Register<TFrom, TTo>(string name = null)
                where TTo : TFrom;
            public abstract IServiceLocator RegisterAsyncFactory<TService>(Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
            public abstract IServiceLocator RegisterAsyncFactory<TService>(string name, Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
            public abstract IServiceLocator RegisterFactory<TService>(string name, Func<object, IServiceLocator, TService> factory, bool isAlwaysCreatingNew = true);
            public abstract TService Resolve<TService>(string name = null);
            public abstract Task<TService> ResolveAsyncFactoryAsync<TService>(string name = null, object parameter = null);
            public abstract TService ResolveFactory<TService>(string name = null, object parameter = null);

            public  TService TryResolve<TService>(Func<TService> backupPlan, string name = null)
            {
                return HasInstance<TService>(name) ? Resolve<TService>(name) : backupPlan();
            }
        }


        public class UnityServiceLocator : ServiceLocator
        {
            public UnityServiceLocator() : this(new UnityContainer())
            {
            }


            //public UnityServiceLocator(bool isForUnitTesting) : this(new UnityContainer())
            //{
            //}

            public UnityServiceLocator(IUnityContainer coreContainer)
            {
                UnityContainer = coreContainer;
            }


            public IUnityContainer UnityContainer { get; protected set; }

            public override bool HasInstance<TService>(string name = null)
            {
                return name == null ? UnityContainer.IsRegistered(typeof(TService)) : UnityContainer.IsRegistered(typeof(TService), name);
            }

            public override IServiceLocator Register<TService>(TService instance)
            {
                UnityContainer.RegisterInstance(instance);
                return this;
            }

            public override IServiceLocator Register<TService>(string name, TService instance)
            {
                UnityContainer.RegisterInstance(name, instance);
                return this;

            }

            public override IServiceLocator Register<TFrom, TTo>(string name = null)

            {
                if (name == null)
                {
                    UnityContainer.RegisterType<TFrom, TTo>();

                }
                else
                {
                    UnityContainer.RegisterType<TFrom, TTo>(name);
                }

                return this;
            }

            public override IServiceLocator RegisterAsyncFactory<TService>(Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true)
            {
                var fac = new ResolveFactory<Task<TService>>(asyncFactory, isAlwaysCreatingNew);
                UnityContainer.RegisterInstance(fac);
                return this;
            }

            public override IServiceLocator RegisterAsyncFactory<TService>(string name, Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true)
            {
                var fac = new ResolveFactory<Task<TService>>(asyncFactory, isAlwaysCreatingNew);
                UnityContainer.RegisterInstance(name, fac);
                return this;
            }

            public override IServiceLocator RegisterFactory<TService>(string name, Func<object, IServiceLocator, TService> factory, bool isAlwaysCreatingNew = true)
            {
                var fac = new ResolveFactory<TService>(factory, isAlwaysCreatingNew);
                UnityContainer.RegisterInstance(name, fac);
                return this;
            }

            public override TService Resolve<TService>(string name = null)
            {
                return name == null ? UnityContainer.Resolve<TService>() : UnityContainer.Resolve<TService>(name);
            }

            public override async Task<TService> ResolveAsyncFactoryAsync<TService>(string name = null, object parameter = null)
            {
                return await ResolveFactory<Task<TService>>(name);
            }

            public override TService ResolveFactory<TService>(string name = null, object parameter = null)
            {
                var fac = Resolve<ResolveFactory<TService>>(name);
                if (fac != null)
                {
                    return fac.GetInstance(parameter, this);
                }
                else
                {
                    return default(TService);
                }
            }


        }



    }
}

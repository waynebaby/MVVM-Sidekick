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
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Commands;
using System.Runtime.CompilerServices;
using MVVMSidekick.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using MVVMSidekick.EventRouting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Collections;
using MVVMSidekick.Utilities;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using System.Collections.Concurrent;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Xaml.Controls.Primitives;


#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;


#elif SILVERLIGHT_5||SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8||WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;

#endif


namespace MVVMSidekick
{

	
    namespace Services
    {
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
            bool HasInstance<TService>(string name = "");
			/// <summary>
			/// Determines whether the specified name is asynchronous.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="name">The name.</param>
			/// <returns><c>true</c> if the specified name is asynchronous; otherwise, <c>false</c>.</returns>
            bool IsAsync<TService>(string name = "");
			/// <summary>
			/// Registers the specified instance.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="instance">The instance.</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            ServiceLocatorEntryStruct<TService> Register<TService>(TService instance);
			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="instance">The instance.</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            ServiceLocatorEntryStruct<TService> Register<TService>(string name, TService instance);
			/// <summary>
			/// Registers the specified factory.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="factory">The factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, TService> factory, bool alwaysNew = true);
			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="factory">The factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>
			/// ServiceLocatorEntryStruct&lt;TService&gt;.
			/// </returns>
            ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, TService> factory, bool alwaysNew = true);
			/// <summary>
			/// Resolves the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="paremeter">The paremeter.</param>
			/// <returns>
			/// TService.
			/// </returns>
            TService Resolve<TService>(string name = null, object paremeter = null);

			/// <summary>
			/// Registers the specified asynchronous factory.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="asyncFactory">The asynchronous factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, Task<TService>> asyncFactory, bool alwaysNew = true);
			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="asyncFactory">The asynchronous factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, Task<TService>> asyncFactory, bool alwaysNew = true);
			/// <summary>
			/// Resolves the asynchronous.
			/// </summary>
			/// <typeparam name="TService"></typeparam>
			/// <param name="name">The name.</param>
			/// <param name="paremeter">The paremeter.</param>
			/// <returns>Task&lt;TService&gt;.</returns>
            Task<TService> ResolveAsync<TService>(string name = null, object paremeter = null);

        }

		/// <summary>
		/// Interface IServiceLocator
		/// </summary>
		/// <typeparam name="TService">The type of the t service.</typeparam>
        public interface IServiceLocator<TService> 
        {
			/// <summary>
			/// Determines whether the specified name has instance.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <returns><c>true</c> if the specified name has instance; otherwise, <c>false</c>.</returns>
            bool HasInstance(string name = "");
			/// <summary>
			/// Determines whether the specified name is asynchronous.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <returns><c>true</c> if the specified name is asynchronous; otherwise, <c>false</c>.</returns>
            bool IsAsync(string name = "");
			/// <summary>
			/// Registers the specified instance.
			/// </summary>
			/// <param name="instance">The instance.</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            ServiceLocatorEntryStruct<TService> Register(TService instance);
			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="instance">The instance.</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            ServiceLocatorEntryStruct<TService> Register(string name, TService instance);
			/// <summary>
			/// Registers the specified factory.
			/// </summary>
			/// <param name="factory">The factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            ServiceLocatorEntryStruct<TService> Register(Func<object, TService> factory, bool alwaysNew = true);
			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="factory">The factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            ServiceLocatorEntryStruct<TService> Register(string name, Func<object, TService> factory, bool alwaysNew = true);
			/// <summary>
			/// Resolves the specified name.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="paremeter">The paremeter.</param>
			/// <returns>TService.</returns>
            TService Resolve(string name = null, object paremeter = null);
			/// <summary>
			/// Registers the specified asynchronous factory.
			/// </summary>
			/// <param name="asyncFactory">The asynchronous factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            ServiceLocatorEntryStruct<TService> Register(Func<object, Task<TService>> asyncFactory, bool alwaysNew = true);
			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="asyncFactory">The asynchronous factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            ServiceLocatorEntryStruct<TService> Register(string name, Func<object, Task<TService>> asyncFactory, bool alwaysNew = true);
			/// <summary>
			/// Resolves the asynchronous.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="paremeter">The paremeter.</param>
			/// <returns>Task&lt;TService&gt;.</returns>
            Task<TService> ResolveAsync(string name = null, object paremeter = null);
        }

		/// <summary>
		/// Class ServiceLocatorEntryStruct.
		/// </summary>
		/// <typeparam name="TService">The type of the t service.</typeparam>
        public class ServiceLocatorEntryStruct<TService>
        {
			/// <summary>
			/// Initializes a new instance of the <see cref="ServiceLocatorEntryStruct{TService}"/> class.
			/// </summary>
			/// <param name="name">The name.</param>
            public ServiceLocatorEntryStruct(string name)
            {
                Name = name;
            }
			/// <summary>
			/// Gets or sets the name.
			/// </summary>
			/// <value>The name.</value>
            public string Name { get; set; }
			/// <summary>
			/// Gets or sets the type of the cache.
			/// </summary>
			/// <value>The type of the cache.</value>
            public CacheType CacheType { get; set; }
			/// <summary>
			/// Gets or sets the service instance.
			/// </summary>
			/// <value>The service instance.</value>
            public TService ServiceInstance { private get; set; }
			/// <summary>
			/// Gets or sets the service factory.
			/// </summary>
			/// <value>The service factory.</value>
            public Func<object, TService> ServiceFactory { private get; set; }
			/// <summary>
			/// Gets or sets the asynchronous service factory.
			/// </summary>
			/// <value>The asynchronous service factory.</value>
            public Func<object, Task<TService>> AsyncServiceFactory { private get; set; }
			/// <summary>
			/// Gets the is value created.
			/// </summary>
			/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
            public bool GetIsValueCreated()
            {

                return (ServiceInstance != null && (!ServiceInstance.Equals(default(TService))));

            }

			/// <summary>
			/// Gets the service.
			/// </summary>
			/// <param name="paremeter">The paremeter.</param>
			/// <returns>TService.</returns>
			/// <exception cref="System.ArgumentException">No such value supported in enum  + typeof(CacheType).ToString()</exception>
            public TService GetService(object paremeter = null)
            {
                switch (CacheType)
                {
                    case CacheType.Instance:
                        return ServiceInstance;
                    case CacheType.Factory:
                        return ServiceFactory(paremeter);
                    case CacheType.LazyInstance:
                        var rval = ServiceInstance;
                        if (rval == null || rval.Equals(default(TService)))
                        {
                            lock (this)
                            {
                                if (ServiceInstance == null || ServiceInstance.Equals(default(TService)))
                                {
                                    return ServiceInstance = ServiceFactory(paremeter);
                                }
                            }
                        }
                        return rval;
                    case CacheType.AsyncFactory:            //  not really suguessed to acces async factory in sync method cos may lead to deadlock ,
                    case CacheType.AsyncLazyInstance:       // but still can do.
                        Task<TService> t = GetServiceAsync(paremeter);
                        return t.Result;
                    default:
                        throw new ArgumentException("No such value supported in enum " + typeof(CacheType).ToString());
                }
            }

			/// <summary>
			/// The _ not finished service task
			/// </summary>
            Task<TService> _NotFinishedServiceTask;

			/// <summary>
			/// get service as an asynchronous operation.
			/// </summary>
			/// <param name="paremeter">The paremeter.</param>
			/// <returns>Task&lt;TService&gt;.</returns>
            public async Task<TService> GetServiceAsync(object paremeter = null)
            {
                switch (CacheType)
                {
                    case CacheType.AsyncFactory:
                        return await AsyncServiceFactory(paremeter);
                    case CacheType.AsyncLazyInstance:
                        if (GetIsValueCreated())
                        {
                            return ServiceInstance;
                        }
                        else
                        {
                            TService rval;
                            Task<TService> rawait;
                            lock (this)
                            {
                                if (GetIsValueCreated())
                                {
                                    return ServiceInstance;
                                }

                                rawait = _NotFinishedServiceTask;
                                if (rawait == null)
                                {
                                    rawait = _NotFinishedServiceTask = AsyncServiceFactory(paremeter);
                                }

                            }

                            rval = await rawait;
                            lock (this)
                            {
                                ServiceInstance = rval;
                                _NotFinishedServiceTask = null;
                            }

                            return rval;
                        }


                    default:
#if SILVERLIGHT_5||WINDOWS_PHONE_7
                        return GetService(paremeter);


#else
                        return GetService(paremeter);
#endif

                }

            }
        }

		/// <summary>
		/// Class TypeSpecifiedServiceLocatorBase.
		/// </summary>
		/// <typeparam name="TSubClass">The type of the t sub class.</typeparam>
		/// <typeparam name="TService">The type of the t service.</typeparam>
        public class TypeSpecifiedServiceLocatorBase<TSubClass, TService> : IServiceLocator<TService>
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
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            public ServiceLocatorEntryStruct<TService> Register(Func<object, TService> factory, bool alwaysNew = true)
            {
                return Register(null, factory, alwaysNew);
            }

			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="factory">The factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            public ServiceLocatorEntryStruct<TService> Register(string name, Func<object, TService> factory, bool alwaysNew = true)
            {
                name = name ?? "";

                if (alwaysNew)
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
            public bool HasInstance(string name = "")
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
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            public ServiceLocatorEntryStruct<TService> Register(Func<object, Task<TService>> asyncFactory, bool alwaysNew = true)
            {
                return Register(null, asyncFactory, alwaysNew);
            }

			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="asyncFactory">The asynchronous factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            public ServiceLocatorEntryStruct<TService> Register(string name, Func<object, Task<TService>> asyncFactory, bool alwaysNew = true)
            {
                name = name ?? "";

                if (alwaysNew)
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
			/// <param name="paremeter">The paremeter.</param>
			/// <returns>Task&lt;TService&gt;.</returns>
            public async Task<TService> ResolveAsync(string name = null, object paremeter = null)
            {
                name = name ?? "";
                var subdic = dic;
                ServiceLocatorEntryStruct<TService> entry = null;
                if (subdic.TryGetValue(name, out entry))
                {
                    return await entry.GetServiceAsync();
                }
                else
                    //#if SILVERLIGHT_5||WINDOWS_PHONE_7
                    //                    return await T.askEx.FromResult(default(TService));
                    //#else
                    //                    return await T.ask.FromResult(default(TService));
                    //#endif
                    return await TaskExHelper.FromResult(default(TService));

            }


			/// <summary>
			/// Determines whether the specified name is asynchronous.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <returns><c>true</c> if the specified name is asynchronous; otherwise, <c>false</c>.</returns>
			/// <exception cref="System.ArgumentException">No such key</exception>
            public bool IsAsync(string name = "")
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
		/// Class ServiceLocatorBase.
		/// </summary>
		/// <typeparam name="TSubClass">The type of the t sub class.</typeparam>
        public class ServiceLocatorBase<TSubClass> : IServiceLocator
            where TSubClass : ServiceLocatorBase<TSubClass>
        {
			/// <summary>
			/// The dispose actions
			/// </summary>
            Dictionary<Type, Action> disposeActions = new Dictionary<Type, Action>();




			/// <summary>
			/// Registers the specified instance.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="instance">The instance.</param>
			/// <returns>
			/// ServiceLocatorEntryStruct&lt;TService&gt;.
			/// </returns>
            public ServiceLocatorEntryStruct<TService> Register<TService>(TService instance)
            {

                return Register<TService>(null, instance);
            }

			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="instance">The instance.</param>
			/// <returns>
			/// ServiceLocatorEntryStruct&lt;TService&gt;.
			/// </returns>
            public ServiceLocatorEntryStruct<TService> Register<TService>(string name, TService instance)
            {
                name = name ?? "";


                if (!disposeActions.ContainsKey(typeof(TService)))
                    disposeActions[typeof(TService)] = () => ServiceTypedCache<TService>.dic.Clear();
                return ServiceTypedCache<TService>.dic[name] =
                     new ServiceLocatorEntryStruct<TService>(name)
                     {
                         ServiceInstance = instance,
                         CacheType = CacheType.Instance
                     };
            }

			/// <summary>
			/// Registers the specified factory.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="factory">The factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>
			/// ServiceLocatorEntryStruct&lt;TService&gt;.
			/// </returns>
            public ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, TService> factory, bool alwaysNew = true)
            {
                return Register<TService>(null, factory, alwaysNew);
            }

			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="factory">The factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>
			/// ServiceLocatorEntryStruct&lt;TService&gt;.
			/// </returns>
            public ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, TService> factory, bool alwaysNew = true)
            {
                name = name ?? "";
                ServiceLocatorEntryStruct<TService> rval;
                if (alwaysNew)
                {
                    ServiceTypedCache<TService>.dic[name] =
                       rval =
                  new ServiceLocatorEntryStruct<TService>(name)
                  {
                      ServiceFactory = factory,
                      CacheType = CacheType.Factory
                  };
                }
                else
                {

                    ServiceTypedCache<TService>.dic[name] =
                        rval =
                       new ServiceLocatorEntryStruct<TService>(name)
                       {
                           ServiceFactory = factory,
                           CacheType = CacheType.LazyInstance
                       };
                }
                if (!disposeActions.ContainsKey(typeof(TService)))
                    disposeActions[typeof(TService)] = () => ServiceTypedCache<TService>.dic.Clear();

                return rval;
            }

			/// <summary>
			/// Resolves the specified name.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="paremeters">The paremeters.</param>
			/// <returns>
			/// TService.
			/// </returns>
            public TService Resolve<TService>(string name = null, object paremeters = null)
            {
                name = name ?? "";
                var subdic = ServiceTypedCache<TService>.dic;
                ServiceLocatorEntryStruct<TService> entry = null;
                if (subdic.TryGetValue(name, out entry))
                {
                    return entry.GetService(paremeters);
                }
                else
                    return default(TService);
            }

			/// <summary>
			/// Disposes this instance.
			/// </summary>
            public void Dispose()
            {
                foreach (var act in disposeActions.Values)
                {
                    try
                    {
                        act();
                    }
                    catch (Exception)
                    {

                    }

                }
            }

			/// <summary>
			/// Class ServiceTypedCache.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
            static class ServiceTypedCache<TService>
            {
				/// <summary>
				/// The dic
				/// </summary>
                public static Dictionary<string, ServiceLocatorEntryStruct<TService>> dic
                    = new Dictionary<string, ServiceLocatorEntryStruct<TService>>();
            }



			/// <summary>
			/// Determines whether the specified name has instance.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="name">The name.</param>
			/// <returns>
			///   <c>true</c> if the specified name has instance; otherwise, <c>false</c>.
			/// </returns>
            public bool HasInstance<TService>(string name = "")
            {
                name = name ?? "";
                ServiceLocatorEntryStruct<TService> entry = null;
                if (ServiceTypedCache<TService>.dic.TryGetValue(name, out entry))
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
			/// Determines whether the specified name is asynchronous.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="name">The name.</param>
			/// <returns>
			///   <c>true</c> if the specified name is asynchronous; otherwise, <c>false</c>.
			/// </returns>
			/// <exception cref="System.ArgumentException">No such key</exception>
            public bool IsAsync<TService>(string name = "")
            {
                name = name ?? "";
                ServiceLocatorEntryStruct<TService> entry = null;
                if (ServiceTypedCache<TService>.dic.TryGetValue(name, out entry))
                {
                    return
                       entry.GetIsValueCreated();

                }
                else
                {
                    throw new ArgumentException("No such key");
                }
            }

			/// <summary>
			/// Registers the specified asynchronous factory.
			/// </summary>
			/// <typeparam name="TService">The type of the service.</typeparam>
			/// <param name="asyncFactory">The asynchronous factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>
			/// ServiceLocatorEntryStruct&lt;TService&gt;.
			/// </returns>
            public ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, Task<TService>> asyncFactory, bool alwaysNew = true)
            {
                return Register(null, asyncFactory, alwaysNew);
            }

			/// <summary>
			/// Registers the specified name.
			/// </summary>
			/// <typeparam name="TService"></typeparam>
			/// <param name="name">The name.</param>
			/// <param name="asyncFactory">The asynchronous factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            public ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, Task<TService>> asyncFactory, bool alwaysNew = true)
            {
                name = name ?? "";
                ServiceLocatorEntryStruct<TService> rval;
                if (alwaysNew)
                {
                    ServiceTypedCache<TService>.dic[name] = rval = new ServiceLocatorEntryStruct<TService>(name)
                    {
                        CacheType = CacheType.AsyncFactory,
                        AsyncServiceFactory = asyncFactory
                    };

                }
                else
                {
                    ServiceTypedCache<TService>.dic[name] = rval = new ServiceLocatorEntryStruct<TService>(name)
                    {
                        CacheType = CacheType.AsyncLazyInstance,
                        AsyncServiceFactory = asyncFactory
                    };

                }
                if (!disposeActions.ContainsKey(typeof(TService)))
                    disposeActions[typeof(TService)] = () => ServiceTypedCache<TService>.dic.Clear();
                return rval;
            }


			/// <summary>
			/// resolve as an asynchronous operation.
			/// </summary>
			/// <typeparam name="TService">The type of the t service.</typeparam>
			/// <param name="name">The name.</param>
			/// <param name="paremeter">The paremeter.</param>
			/// <returns>Task&lt;TService&gt;.</returns>
            public async Task<TService> ResolveAsync<TService>(string name = null, object paremeter = null)
            {
                name = name ?? "";
                var subdic = ServiceTypedCache<TService>.dic;
                ServiceLocatorEntryStruct<TService> entry = null;
                if (subdic.TryGetValue(name, out entry))
                {
                    return await entry.GetServiceAsync();
                }
                else
                    //#if SILVERLIGHT_5||WINDOWS_PHONE_7
                    //                    return await T.askEx.FromResult(default(TService));
                    //#else
                    //                    return await T.ask.FromResult(default(TService));
                    //#endif
                    return await TaskExHelper.FromResult(default(TService));
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

        //public class DictionaryServiceLocator : IServiceLocator
        //{
        //    Dictionary<Type, Dictionary<string, Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>>>
        //       dic = new Dictionary<Type, Dictionary<string, Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>>>();
        //    public static IServiceLocator Instance { get; set; }


        //    public ServiceLocatorEntryStruct<TService> Register<TService>(TService instance)
        //    {

        //        Register<TService>(null, instance);
        //    }

        //    public ServiceLocatorEntryStruct<TService> Register<TService>(string name, TService instance)
        //    {
        //        name = name ?? "";
        //        dic[typeof(TService)] = dic[typeof(TService)] ?? new Dictionary<string, Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>>();
        //        dic[typeof(TService)][name] =
        //             new Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>(
        //                 null,
        //                 instance,
        //                 null,
        //                 CacheType.Instance);



        //    }

        //    public ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, TService> factory, bool alwaysNew = true)
        //    {
        //        Register<TService>(null, factory, alwaysNew);
        //    }

        //    public ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, TService> factory, bool alwaysNew = true)
        //    {
        //        name = name ?? "";
        //        dic[typeof(TService)] = dic[typeof(TService)] ?? new Dictionary<string, Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>>();

        //        if (alwaysNew)
        //        {
        //            dic[typeof(TService)][name] =
        //                new Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>(
        //                    null,
        //                    default(TService),
        //                    d => factory(d) as object,
        //                    CacheType.Factory);
        //        }
        //        else
        //        {

        //            dic[typeof(TService)][name] =
        //                new Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>(
        //                    new Lazy<object>(() => factory(null)),
        //                    default(TService),
        //                    null,
        //                    CacheType.LazyInstance);
        //        }


        //    }

        //    public TService Resolve<TService>(string name = null, object parameters = null)
        //    {
        //        name = name ?? "";
        //        var subdic = dic[typeof(TService)];
        //        if (subdic != null)
        //        {

        //            Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType> entry = null;
        //            if (subdic.TryGetValue(name, out entry))
        //            {
        //                switch (entry.Item4)
        //                {
        //                    case CacheType.Instance:
        //                        return (TService)entry.Item2;
        //                    case CacheType.Factory:
        //                        return (TService)entry.Item3(parameters);
        //                    case CacheType.LazyInstance:
        //                        return (TService)entry.Item1.Value;
        //                    default:
        //                        break;
        //                }
        //                return default(TService);
        //            }
        //            else
        //                return default(TService);
        //        }
        //        else
        //        {
        //            return default(TService);
        //        }
        //    }

        //    public void Dispose()
        //    {
        //        dic.Clear();
        //    }

        //    public bool HasInstance<TService>(string name = "")
        //    {
        //        name = name ?? "";
        //        Tuple<Lazy<Object>, object, Func<object, Object>, CacheType> entry = null;
        //        if (dic[typeof(TService)].TryGetValue(name, out entry))
        //        {
        //            return
        //                (entry.Item4 == CacheType.Instance
        //                ||
        //                (
        //                    entry.Item4 == CacheType.LazyInstance
        //                    &&
        //                    entry.Item1.IsValueCreated)
        //                );


        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}
		/// <summary>
		/// Class ServiceLocator. This class cannot be inherited.
		/// </summary>
        public sealed class ServiceLocator : ServiceLocatorBase<ServiceLocator>
        {
			/// <summary>
			/// Initializes static members of the <see cref="ServiceLocator"/> class.
			/// </summary>
            static ServiceLocator()
            {
                Instance = new ServiceLocator();
            }

			/// <summary>
			/// Prevents a default instance of the <see cref="ServiceLocator"/> class from being created.
			/// </summary>
            private ServiceLocator()
            {

            }

			/// <summary>
			/// Gets or sets the instance.
			/// </summary>
			/// <value>The instance.</value>
            public static IServiceLocator Instance { get; set; }
        }

    }
}

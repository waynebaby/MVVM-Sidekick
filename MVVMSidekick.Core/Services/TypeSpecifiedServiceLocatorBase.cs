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




    }
}

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
using System.Threading.Tasks;
#if NETFX_CORE



#elif WPF



#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
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
            /// <param name="parameter">The parameter.</param>
            /// <returns>TService.</returns>
            /// <exception cref="System.ArgumentException">No such value supported in enum  + typeof(CacheType).ToString()</exception>
            public TService GetService(object parameter = null)
            {
                switch (CacheType)
                {
                    case CacheType.Instance:
                        return ServiceInstance;
                    case CacheType.Factory:
                        return ServiceFactory(parameter);
                    case CacheType.LazyInstance:
                        var rval = ServiceInstance;
                        if (rval == null || rval.Equals(default(TService)))
                        {
                            lock (this)
                            {
                                if (ServiceInstance == null || ServiceInstance.Equals(default(TService)))
                                {
                                    return ServiceInstance = ServiceFactory(parameter);
                                }
                            }
                        }
                        return rval;
                    case CacheType.AsyncFactory:            //  not really suguessed to acces async factory in sync method cos may lead to deadlock ,
                    case CacheType.AsyncLazyInstance:       // but still can do.
                        Task<TService> t = GetServiceAsync(parameter);
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
            /// <param name="parameter">The parameter.</param>
            /// <returns>Task&lt;TService&gt;.</returns>
            public async Task<TService> GetServiceAsync(object parameter = null)
            {
                switch (CacheType)
                {
                    case CacheType.AsyncFactory:
                        return await AsyncServiceFactory(parameter);
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
                                    rawait = _NotFinishedServiceTask = AsyncServiceFactory(parameter);
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
#if SILVERLIGHT_5 || WINDOWS_PHONE_7
						return GetService(parameter);


#else
                        return GetService(parameter);
#endif

                }

            }
        }




    }
}

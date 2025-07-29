/// <summary>
/// <para>服务定位器条目结构，定义服务定位器中条目的数据结构</para>
/// <para>Service locator entry structure, defining the data structure of entries in service locator</para>
/// </summary>

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

namespace MVVMSidekick
{
    namespace Services
    {
        /// <summary>
        /// <para>服务定位器条目结构类，表示服务定位器中的单个条目</para>
        /// <para>Service locator entry structure class, representing a single entry in service locator</para>
        /// </summary>
        /// Class ServiceLocatorEntryStruct.
        /// </summary>
        /// <typeparam name="TService">服务类型 / The type of the service.</typeparam>
        public class ServiceLocatorEntryStruct<TService>
        {
            /// <summary>
            /// 初始化 <see cref="ServiceLocatorEntryStruct{TService}"/> 类的新实例
            /// Initializes a new instance of the <see cref="ServiceLocatorEntryStruct{TService}"/> class.
            /// </summary>
            /// <param name="name">名称 / The name.</param>
            public ServiceLocatorEntryStruct(string name)
            {
                Name = name;
            }
            /// <summary>
            /// 获取或设置名称
            /// Gets or sets the name.
            /// </summary>
            /// <value>名称 / The name.</value>
            public string Name { get; set; }
            /// <summary>
            /// 获取或设置缓存类型
            /// Gets or sets the type of the cache.
            /// </summary>
            /// <value>缓存类型 / The type of the cache.</value>
            public CacheType CacheType { get; set; }
            /// <summary>
            /// 获取或设置服务实例
            /// Gets or sets the service instance.
            /// </summary>
            /// <value>服务实例 / The service instance.</value>
            public TService ServiceInstance { private get; set; }
            /// <summary>
            /// 获取或设置服务工厂
            /// Gets or sets the service factory.
            /// </summary>
            /// <value>服务工厂 / The service factory.</value>
            public Func<object, TService> ServiceFactory { private get; set; }
            /// <summary>
            /// 获取或设置异步服务工厂
            /// Gets or sets the asynchronous service factory.
            /// </summary>
            /// <value>异步服务工厂 / The asynchronous service factory.</value>
            public Func<object, Task<TService>> AsyncServiceFactory { private get; set; }
            /// <summary>
            /// 获取值是否已创建
            /// Gets the is value created.
            /// </summary>
            /// <returns><c>true</c> 如果值已创建，否则 <c>false</c> / if value is created, <c>false</c> otherwise.</returns>
            public bool GetIsValueCreated()
            {

                return (ServiceInstance != null && (!ServiceInstance.Equals(default(TService))));

            }

            /// <summary>
            /// 获取服务
            /// Gets the service.
            /// </summary>
            /// <param name="parameter">参数 / The parameter.</param>
            /// <returns>TService.</returns>
            /// <exception cref="System.ArgumentException">枚举中不支持此值 / No such value supported in enum  + typeof(CacheType).ToString()</exception>
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
            /// 未完成的服务任务字段
            /// The _ not finished service task
            /// </summary>
            Task<TService> _NotFinishedServiceTask;

            /// <summary>
            /// 异步获取服务
            /// get service as an asynchronous operation.
            /// </summary>
            /// <param name="parameter">参数 / The parameter.</param>
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

                        return GetService(parameter);

                }

            }
        }




    }
}

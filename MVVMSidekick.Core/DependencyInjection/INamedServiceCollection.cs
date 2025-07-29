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
using System.Collections.Concurrent;
using System;


namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 命名服务集合接口，扩展标准服务集合以支持命名服务注册
    /// Named service collection interface that extends standard service collection to support named service registration
    /// </summary>
    public interface INamedServiceCollection : IServiceCollection
    {
        /// <summary>
        /// 获取工厂数据字典，存储命名服务的工厂方法
        /// Gets the factory data dictionary that stores factory methods for named services
        /// </summary>
        ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>> FactoryData { get; }
        
        /// <summary>
        /// 获取核心服务集合
        /// Gets the core service collection
        /// </summary>
        IServiceCollection Core { get; }
    }


}


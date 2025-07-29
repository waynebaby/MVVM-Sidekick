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
        /// 类型指定的服务定位器接口，提供按名称注册和解析特定类型服务的功能
        /// Type-specified service locator interface providing functionality to register and resolve specific type services by name
        /// </summary>
        /// <typeparam name="TService">服务类型 / Service type</typeparam>
        public interface ITypeSpecifiedServiceLocator<TService>
        {
            /// <summary>
            /// 确定指定名称是否有实例
            /// Determines whether the specified name has instance
            /// </summary>
            /// <param name="name">服务名称 / Service name</param>
            /// <returns>如果有实例返回true，否则返回false / Returns true if has instance, otherwise false</returns>
            bool HasInstance(string name = "");
            
            /// <summary>
            /// 确定指定名称的服务是否为异步
            /// Determines whether the specified name service is asynchronous
            /// </summary>
            /// <param name="name">服务名称 / Service name</param>
            /// <returns>如果是异步返回true，否则返回false / Returns true if asynchronous, otherwise false</returns>
            bool IsAsync(string name = "");
            
            /// <summary>
            /// 注册指定的实例
            /// Registers the specified instance
            /// </summary>
            /// <param name="instance">服务实例 / Service instance</param>
            /// <returns>服务定位器条目结构 / Service locator entry structure</returns>
            ServiceLocatorEntryStruct<TService> Register(TService instance);
            
            /// <summary>
            /// 按名称注册指定的实例
            /// Registers the specified instance with name
            /// </summary>
            /// <param name="name">服务名称 / Service name</param>
            /// <param name="instance">服务实例 / Service instance</param>
            /// <returns>服务定位器条目结构 / Service locator entry structure</returns>
            ServiceLocatorEntryStruct<TService> Register(string name, TService instance);
            
            /// <summary>
            /// 注册指定的工厂方法
            /// Registers the specified factory method
            /// </summary>
            /// <param name="factory">工厂方法 / Factory method</param>
            /// <param name="isAlwaysCreatingNew">是否总是创建新实例 / Whether always creating new instance</param>
            /// <returns>服务定位器条目结构 / Service locator entry structure</returns>
            ServiceLocatorEntryStruct<TService> Register(Func<object, TService> factory, bool isAlwaysCreatingNew = true);
            
            /// <summary>
            /// 按名称注册指定的工厂方法
            /// Registers the specified factory method with name
            /// </summary>
            /// <param name="name">服务名称 / Service name</param>
            /// <param name="factory">工厂方法 / Factory method</param>
            /// <param name="isAlwaysCreatingNew">是否总是创建新实例 / Whether always creating new instance</param>
            /// <returns>服务定位器条目结构 / Service locator entry structure</returns>
            ServiceLocatorEntryStruct<TService> Register(string name, Func<object, TService> factory, bool isAlwaysCreatingNew = true);
            
            /// <summary>
            /// 解析指定名称的服务
            /// Resolves service with specified name
            /// </summary>
            /// <param name="name">服务名称 / Service name</param>
            /// <param name="parameter">解析参数 / Resolution parameter</param>
            /// <returns>服务实例 / Service instance</returns>
            TService Resolve(string name = null, object parameter = null);
            
            /// <summary>
            /// 注册指定的异步工厂方法
            /// Registers the specified asynchronous factory method
            /// </summary>
            /// <param name="asyncFactory">异步工厂方法 / Asynchronous factory method</param>
            /// <param name="isAlwaysCreatingNew">是否总是创建新实例 / Whether always creating new instance</param>
            /// <returns>服务定位器条目结构 / Service locator entry structure</returns>
            ServiceLocatorEntryStruct<TService> Register(Func<object, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
            
            /// <summary>
            /// 按名称注册指定的异步工厂方法
            /// Registers the specified asynchronous factory method with name
            /// </summary>
            /// <param name="name">服务名称 / Service name</param>
            /// <param name="asyncFactory">异步工厂方法 / Asynchronous factory method</param>
            /// <param name="isAlwaysCreatingNew">是否总是创建新实例 / Whether always creating new instance</param>
            /// <returns>服务定位器条目结构 / Service locator entry structure</returns>
            ServiceLocatorEntryStruct<TService> Register(string name, Func<object, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
            
            /// <summary>
            /// 异步解析指定名称的服务
            /// Resolves service asynchronously with specified name
            /// </summary>
            /// <param name="name">服务名称 / Service name</param>
            /// <param name="parameter">解析参数 / Resolution parameter</param>
            /// <returns>表示异步操作的任务 / Task representing the asynchronous operation</returns>
            Task<TService> ResolveAsync(string name = null, object parameter = null);
        }




    }
}

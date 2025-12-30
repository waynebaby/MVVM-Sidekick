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
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Concurrent;
using System;


namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 命名服务集合实现，包装标准服务集合并添加命名服务支持
    /// Named service collection implementation that wraps standard service collection and adds named service support
    /// </summary>
    public class NamedServiceCollection : INamedServiceCollection
    {
        /// <summary>
        /// 初始化NamedServiceCollection类的新实例
        /// Initializes a new instance of the NamedServiceCollection class
        /// </summary>
        /// <param name="innerserviceCollection">内部服务集合 / The inner service collection</param>
        /// <param name="factoryData">工厂数据字典 / The factory data dictionary</param>
        public NamedServiceCollection(IServiceCollection innerserviceCollection, ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>> factoryData)
        {
            this.Core = innerserviceCollection;
            this.FactoryData = factoryData;
        }

        /// <summary>
        /// 获取或设置指定索引处的服务描述符
        /// Gets or sets the service descriptor at the specified index
        /// </summary>
        /// <param name="index">索引 / The index</param>
        /// <returns>服务描述符 / The service descriptor</returns>
        public ServiceDescriptor this[int index] { get => Core[index]; set => Core[index] = value; }

        /// <summary>
        /// 获取服务集合中的元素数量
        /// Gets the number of elements in the service collection
        /// </summary>
        public int Count => Core.Count;

        /// <summary>
        /// 获取一个值，该值指示服务集合是否为只读
        /// Gets a value indicating whether the service collection is read-only
        /// </summary>
        public bool IsReadOnly => Core.IsReadOnly;

        /// <summary>
        /// 获取核心服务集合
        /// Gets the core service collection
        /// </summary>
        public IServiceCollection Core { get; }

        /// <summary>
        /// 获取工厂数据字典，存储命名服务的工厂方法
        /// Gets the factory data dictionary that stores factory methods for named services
        /// </summary>
        public ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>> FactoryData { get; }

        /// <summary>
        /// 将服务描述符添加到集合中
        /// Adds a service descriptor to the collection
        /// </summary>
        /// <param name="item">要添加的服务描述符 / The service descriptor to add</param>
        public void Add(ServiceDescriptor item)
        {
            Core.Add(item);
        }

        /// <summary>
        /// 清空服务集合中的所有元素
        /// Clears all elements from the service collection
        /// </summary>
        public void Clear()
        {
            Core.Clear();
        }

        /// <summary>
        /// 确定服务集合是否包含指定的服务描述符
        /// Determines whether the service collection contains the specified service descriptor
        /// </summary>
        /// <param name="item">要查找的服务描述符 / The service descriptor to locate</param>
        /// <returns>如果找到则为true，否则为false / True if found, otherwise false</returns>
        public bool Contains(ServiceDescriptor item)
        {
            return Core.Contains(item);
        }

        /// <summary>
        /// 将服务集合的元素复制到数组中
        /// Copies the elements of the service collection to an array
        /// </summary>
        /// <param name="array">目标数组 / The destination array</param>
        /// <param name="arrayIndex">起始索引 / The starting index</param>
        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            Core.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 返回遍历服务集合的枚举器
        /// Returns an enumerator that iterates through the service collection
        /// </summary>
        /// <returns>枚举器 / The enumerator</returns>
        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return Core.GetEnumerator();
        }

        /// <summary>
        /// 确定指定服务描述符在集合中的索引
        /// Determines the index of the specified service descriptor in the collection
        /// </summary>
        /// <param name="item">要查找的服务描述符 / The service descriptor to locate</param>
        /// <returns>索引位置 / The index position</returns>
        public int IndexOf(ServiceDescriptor item)
        {
            return Core.IndexOf(item);
        }

        /// <summary>
        /// 在指定索引处插入服务描述符
        /// Inserts a service descriptor at the specified index
        /// </summary>
        /// <param name="index">插入位置 / The insertion index</param>
        /// <param name="item">要插入的服务描述符 / The service descriptor to insert</param>
        public void Insert(int index, ServiceDescriptor item)
        {
            Core.Insert(index, item);
        }

        /// <summary>
        /// 从服务集合中移除指定的服务描述符
        /// Removes the specified service descriptor from the service collection
        /// </summary>
        /// <param name="item">要移除的服务描述符 / The service descriptor to remove</param>
        /// <returns>如果成功移除则为true / True if successfully removed</returns>
        public bool Remove(ServiceDescriptor item)
        {
            return Core.Remove(item);
        }

        /// <summary>
        /// 移除指定索引处的服务描述符
        /// Removes the service descriptor at the specified index
        /// </summary>
        /// <param name="index">要移除的索引 / The index to remove</param>
        public void RemoveAt(int index)
        {
            Core.RemoveAt(index);
        }

        /// <summary>
        /// 返回遍历集合的非泛型枚举器
        /// Returns a non-generic enumerator that iterates through the collection
        /// </summary>
        /// <returns>非泛型枚举器 / The non-generic enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Core).GetEnumerator();
        }
    }


}


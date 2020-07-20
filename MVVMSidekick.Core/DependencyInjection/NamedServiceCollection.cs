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

    public class NamedServiceCollection : INamedServiceCollection
    {
        public NamedServiceCollection(IServiceCollection innerserviceCollection, ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>> factoryData)
        {
            this.Core = innerserviceCollection;
            this.FactoryData = factoryData;
        }

        public ServiceDescriptor this[int index] { get => Core[index]; set => Core[index] = value; }

        public int Count => Core.Count;

        public bool IsReadOnly => Core.IsReadOnly;

        public IServiceCollection Core { get; }

        public ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>> FactoryData { get; }

        public void Add(ServiceDescriptor item)
        {
            Core.Add(item);
        }

        public void Clear()
        {
            Core.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
            return Core.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            Core.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return Core.GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            return Core.IndexOf(item);
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            Core.Insert(index, item);
        }

        public bool Remove(ServiceDescriptor item)
        {
            return Core.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Core.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Core).GetEnumerator();
        }
    }


}


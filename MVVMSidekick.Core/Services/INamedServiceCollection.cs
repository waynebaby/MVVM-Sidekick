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
using System;
using System.Collections.Concurrent;

namespace MVVMSidekick
{


    namespace Services
    {
        public interface INamedServiceCollection : IServiceCollection
        {

             ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>> FactoryData { get; }
            IServiceCollection Core { get; }
        }


    }
}

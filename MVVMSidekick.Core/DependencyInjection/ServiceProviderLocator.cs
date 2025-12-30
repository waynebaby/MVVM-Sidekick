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


namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 服务提供程序定位器，提供全局服务提供程序访问
    /// Service provider locator providing global service provider access
    /// </summary>
    public static class ServiceProviderLocator
    {
        /// <summary>
        /// 获取或设置根服务提供程序
        /// Gets or sets the root service provider
        /// </summary>
        public static IServiceProvider RootServiceProvider { get; set; }


    }

}
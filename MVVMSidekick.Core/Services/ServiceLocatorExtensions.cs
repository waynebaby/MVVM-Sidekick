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
        public static class ServiceLocatorExtensions
        {

            public static TServiceLocator Configure<TServiceLocator>(this TServiceLocator instance, Action<TServiceLocator> configureBody) where TServiceLocator : ServiceLocator
            {
                configureBody(instance);
                return instance;
            }
            public static async Task<TServiceLocator> ConfigureAsync<TServiceLocator>(this Task<TServiceLocator> instanceTask, Action<TServiceLocator> configureBody) where TServiceLocator : ServiceLocator
            {
                var instance = await instanceTask;
                instance.Configure(configureBody);
                return instance;
            }
            public static async Task<TServiceLocator> ConfigureAsync<TServiceLocator>(this TServiceLocator instance, AsyncAction<TServiceLocator> configureBody) where TServiceLocator : ServiceLocator
            {
                await configureBody(instance);
                return instance;
            }
            public static async Task<TServiceLocator> ConfigureAsync<TServiceLocator>(this Task<TServiceLocator> instanceTask, AsyncAction<TServiceLocator> configureBody) where TServiceLocator : ServiceLocator
            {
                var instance = await instanceTask;
                await instance.ConfigureAsync(configureBody);
                return instance;
            }
        }




    }
}

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
        /// Interface IServiceLocator
        /// </summary>
        public interface IServiceLocator
        {
            /// <summary>
            /// Determines whether the specified name has instance.
            /// </summary>
            /// <typeparam name="TService">The type of the t service.</typeparam>
            /// <param name="name">The name.</param>
            /// <returns><c>true</c> if the specified name has instance; otherwise, <c>false</c>.</returns>
            bool HasInstance<TService>(string name = null);

            TService TryResolve<TService>(Func<TService> backupPlan, String name = null);

            /// <summary>
            /// Registers the typeMapping
            /// </summary>
            IServiceLocator Register<TFrom, TTo>(string name = null)
                where TTo : TFrom;

            /// <summary>
            /// Registers the specified instance.
            /// </summary>
            /// <typeparam name="TService">The type of the t service.</typeparam>
            /// <param name="instance">The instance.</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            IServiceLocator Register<TService>(TService instance);
            /// <summary>
            /// Registers the specified name.
            /// </summary>
            /// <typeparam name="TService">The type of the t service.</typeparam>
            /// <param name="name">The name.</param>
            /// <param name="instance">The instance.</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            IServiceLocator Register<TService>(string name, TService instance);

            /// <summary>
            /// Registers the specified name.
            /// </summary>
            /// <typeparam name="TService">The type of the t service.</typeparam>
            /// <param name="name">The name.</param>
            /// <param name="factory">The factory.</param>
            /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
            /// <returns>
            /// ServiceLocatorEntryStruct&lt;TService&gt;.
            /// </returns>
            IServiceLocator RegisterFactory<TService>(string name, Func<object, IServiceLocator, TService> factory, bool isAlwaysCreatingNew = true);
            /// <summary>
            /// Resolves the specified name.
            /// </summary>
            /// <typeparam name="TService">The type of the service.</typeparam>
            /// <param name="name">The name.</param>
            /// <param name="parameter">The parameter.</param>
            /// <returns>
            /// TService.
            /// </returns>
            TService Resolve<TService>(string name = null);

            /// <summary>
            /// Registers the specified asynchronous factory.
            /// </summary>
            /// <typeparam name="TService">The type of the t service.</typeparam>
            /// <param name="asyncFactory">The asynchronous factory.</param>
            /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            IServiceLocator RegisterAsyncFactory<TService>(Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
            /// <summary>
            /// Registers the specified name.
            /// </summary>
            /// <typeparam name="TService">The type of the t service.</typeparam>
            /// <param name="name">The name.</param>
            /// <param name="asyncFactory">The asynchronous factory.</param>
            /// <param name="isAlwaysCreatingNew">if set to <c>true</c> [always new].</param>
            /// <returns>ServiceLocatorEntryStruct&lt;TService&gt;.</returns>
            IServiceLocator RegisterAsyncFactory<TService>(string name, Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);


            /// <summary>
            /// Resolves the asynchronous.
            /// </summary>
            /// <typeparam name="TService"></typeparam>
            /// <param name="name">The name.</param>
            /// <param name="parameter">The parameter.</param>
            /// <returns>Task&lt;TService&gt;.</returns>
            TService ResolveFactory<TService>(string name = null, object parameter = null);

            /// <summary>
            /// Resolves the asynchronous.
            /// </summary>
            /// <typeparam name="TService"></typeparam>
            /// <param name="name">The name.</param>
            /// <param name="parameter">The parameter.</param>
            /// <returns>Task&lt;TService&gt;.</returns>
            Task<TService> ResolveAsyncFactoryAsync<TService>(string name = null, object parameter = null);
        }




    }
}

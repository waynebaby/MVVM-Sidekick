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

namespace MVVMSidekick
{


    namespace Services
    {
        /// <summary>
        /// <para>Cache type for service registration.</para>
        /// <para>服务注册的缓存类型。</para>
        /// </summary>
        public enum CacheType
        {
            /// <summary>
            /// <para>Instance cache.</para>
            /// <para>实例缓存。</para>
            /// </summary>
            Instance,
            /// <summary>
            /// <para>Factory cache.</para>
            /// <para>工厂缓存。</para>
            /// </summary>
            Factory,
            /// <summary>
            /// <para>Lazy instance cache.</para>
            /// <para>延迟实例缓存。</para>
            /// </summary>
            LazyInstance,
            /// <summary>
            /// <para>Asynchronous factory cache.</para>
            /// <para>异步工厂缓存。</para>
            /// </summary>
            AsyncFactory,
            /// <summary>
            /// <para>Asynchronous lazy instance cache.</para>
            /// <para>异步延迟实例缓存。</para>
            /// </summary>
            AsyncLazyInstance
        }




    }
}

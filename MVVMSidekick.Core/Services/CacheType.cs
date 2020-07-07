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
        /// Enum CacheType
        /// </summary>
        public enum CacheType
        {
            /// <summary>
            /// The instance
            /// </summary>
            Instance,
            /// <summary>
            /// The factory
            /// </summary>
            Factory,
            /// <summary>
            /// The lazy instance
            /// </summary>
            LazyInstance,
            /// <summary>
            /// The asynchronous factory
            /// </summary>
            AsyncFactory,
            /// <summary>
            /// The asynchronous lazy instance
            /// </summary>
            AsyncLazyInstance
        }




    }
}

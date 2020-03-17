// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Utilities.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reactive.Linq;
using System.Collections.Concurrent;



namespace MVVMSidekick
{

    namespace Utilities
    {
        /// <summary>
        /// Class ReflectionCache.
        /// </summary>
        public static class ReflectionCache
        {
            /// <summary>
            /// Class ReflectInfoCache.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            static class ReflectInfoCache<T> where T : MemberInfo
            {
                /// <summary>
                /// The cache
                /// </summary>
                static ConcurrentDictionary<Type, Dictionary<string, T>> cache
                    = new ConcurrentDictionary<Type, Dictionary<string, T>>();

                /// <summary>
                /// Gets the cache.
                /// </summary>
                /// <param name="type">The type.</param>
                /// <param name="dataGetter">The data getter.</param>
                /// <returns>Dictionary&lt;System.String, T&gt;.</returns>
                static public Dictionary<string, T> GetCache(Type type, Func<Type, T[]> dataGetter)
                {
                    return cache.GetOrAdd(type, s => dataGetter(s).ToDictionary(x => x.Name, x => x));
                }
            }


            /// <summary>
            /// Gets the methods from cache.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <returns>Dictionary&lt;System.String, MethodInfo&gt;.</returns>
            public static Dictionary<string, MethodInfo> GetMethodsFromCache(this Type type)
            {

                return ReflectInfoCache<MethodInfo>.GetCache(type, x => x.GetRuntimeMethods().ToArray());

            }

            /// <summary>
            /// Gets the events from cache.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <returns>Dictionary&lt;System.String, EventInfo&gt;.</returns>
            public static Dictionary<string, EventInfo> GetEventsFromCache(this Type type)
            {

                return ReflectInfoCache<EventInfo>.GetCache(type, x => x.GetEvents());

            }



        }

    }

}


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
        /// <para>反射缓存类，提供反射信息的缓存功能</para>
        /// <para>Reflection cache class, providing caching functionality for reflection information</para>
        /// </summary>
        public static class ReflectionCache
        {
            /// <summary>
            /// <para>反射信息缓存类，泛型约束为成员信息类型</para>
            /// <para>Reflection information cache class with generic constraint for member information types</para>
            /// </summary>
            /// <typeparam name="T">成员信息类型 / Member information type</typeparam>
            static class ReflectInfoCache<T> where T : MemberInfo
            {
                /// <summary>
                /// <para>缓存字典，存储类型与其成员信息的映射</para>
                /// <para>Cache dictionary storing mappings between types and their member information</para>
                /// </summary>
                static ConcurrentDictionary<Type, Dictionary<string, T>> cache
                    = new ConcurrentDictionary<Type, Dictionary<string, T>>();

                /// <summary>
                /// <para>获取指定类型的缓存信息，如果不存在则使用数据获取器创建</para>
                /// <para>Gets cached information for the specified type, creates using data getter if not exists</para>
                /// </summary>
                /// <param name="type">要获取缓存的类型 / The type to get cache for</param>
                /// <param name="dataGetter">数据获取器函数 / The data getter function</param>
                /// <returns>包含成员信息的字典 / Dictionary containing member information</returns>
                static public Dictionary<string, T> GetCache(Type type, Func<Type, T[]> dataGetter)
                {
                    return cache.GetOrAdd(type, s => dataGetter(s).ToDictionary(x => x.Name, x => x));
                }
            }

            /// <summary>
            /// <para>从缓存中获取指定类型的所有方法信息</para>
            /// <para>Gets all method information for the specified type from cache</para>
            /// </summary>
            /// <param name="type">要获取方法的类型 / The type to get methods for</param>
            /// <returns>包含方法信息的字典 / Dictionary containing method information</returns>
            public static Dictionary<string, MethodInfo> GetMethodsFromCache(this Type type)
            {

                return ReflectInfoCache<MethodInfo>.GetCache(type, x => x.GetRuntimeMethods().ToArray());

            }

            /// <summary>
            /// <para>从缓存中获取指定类型的所有事件信息</para>
            /// <para>Gets all event information for the specified type from cache</para>
            /// </summary>
            /// <param name="type">要获取事件的类型 / The type to get events for</param>
            /// <returns>包含事件信息的字典 / Dictionary containing event information</returns>
            public static Dictionary<string, EventInfo> GetEventsFromCache(this Type type)
            {

                return ReflectInfoCache<EventInfo>.GetCache(type, x => x.GetEvents());

            }

        }

    }

}


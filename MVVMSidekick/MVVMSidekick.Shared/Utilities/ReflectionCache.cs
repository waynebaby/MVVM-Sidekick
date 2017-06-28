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
#if NETFX_CORE
using System.Collections.Concurrent;

#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Services;
using System.Reactive.Disposables;


#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Reactive.Disposables;

#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Reactive;
#endif



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
#if NETFX_CORE
				return ReflectInfoCache<MethodInfo>.GetCache(type, x => x.GetRuntimeMethods().ToArray());
#else
				return ReflectInfoCache<MethodInfo>.GetCache(type, x => x.GetMethods());
#endif
			}

			/// <summary>
			/// Gets the events from cache.
			/// </summary>
			/// <param name="type">The type.</param>
			/// <returns>Dictionary&lt;System.String, EventInfo&gt;.</returns>
			public static Dictionary<string, EventInfo> GetEventsFromCache(this Type type)
			{
#if NETFX_CORE
				return ReflectInfoCache<EventInfo>.GetCache(type, x => x.GetRuntimeEvents().ToArray());
#else
				return ReflectInfoCache<EventInfo>.GetCache(type, x => x.GetEvents());
#endif
			}



		}

	}

}


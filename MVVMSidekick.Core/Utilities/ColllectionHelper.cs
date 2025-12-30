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
using System.Collections.Generic;
using System.Collections.ObjectModel;
#if WINDOWS_UWP

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
        /// 集合助手类，提供集合操作的扩展方法
        /// Collection helper class providing extension methods for collection operations
        /// </summary>
        public static class CollectionHelper
		{

			/// <summary>
			/// 将可枚举对象转换为可观察集合
			/// Converts enumerable to observable collection
			/// </summary>
			/// <typeparam name="T">元素类型 / Element type</typeparam>
			/// <param name="items">源项目集合 / Source items collection</param>
			/// <returns>可观察集合 / Observable collection</returns>
			public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
			{

				return new ObservableCollection<T>(items);
			}

			/// <summary>
			/// 匹配或返回默认值
			/// Matches or returns default value
			/// </summary>
			/// <typeparam name="TKey">键类型 / Key type</typeparam>
			/// <typeparam name="TValue">值类型 / Value type</typeparam>
			/// <param name="dic">The dic.</param>
			/// <param name="key">The key.</param>
			/// <returns>
			/// TValue.
			/// </returns>
			public static TValue MatchOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
			{
				TValue val = default(TValue);
				dic.TryGetValue(key, out val);
				return val;
			}
		}

	}

}


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
#if NETFX_CORE

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
        /// Class ColllectionHelper.
        /// </summary>
        public static class ColllectionHelper
		{


			/// <summary>
			/// To the observable collection.
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="items">The items.</param>
			/// <returns>ObservableCollection&lt;T&gt;.</returns>
			public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
			{

				return new ObservableCollection<T>(items);
			}


			/// <summary>
			/// Matches the or default.
			/// </summary>
			/// <typeparam name="TKey">The type of the key.</typeparam>
			/// <typeparam name="TValue">The type of the value.</typeparam>
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


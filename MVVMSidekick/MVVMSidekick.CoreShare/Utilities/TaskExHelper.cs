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
using System.Threading.Tasks;
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
        /// Unify Task(4.5) and TaskEx (SL5) method in this helper
        /// </summary>
        public static class TaskExHelper
		{



			/// <summary>
			/// Yields this instance.
			/// </summary>
			/// <returns>Task.</returns>
			public static async Task Yield()
			{
#if SILVERLIGHT_5 || WINDOWS_PHONE_7 || NET40
				await TaskEx.Yield();

#elif NETFX_CORE
                await Task.CompletedTask;

#else
                await Task.Yield();
#endif

            }

            /// <summary>
            /// Froms the result.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="result">The result.</param>
            /// <returns>
            /// Task&lt;T&gt;.
            /// </returns>
            public static async Task<T> FromResult<T>(T result)
			{
#if SILVERLIGHT_5||WINDOWS_PHONE_7||NET40
				return await TaskEx.FromResult(result);

#else
				return await Task.FromResult(result);
#endif

			}

			/// <summary>
			/// Delays the specified ms.
			/// </summary>
			/// <param name="ms">The ms.</param>
			/// <returns>Task.</returns>
			public static async Task Delay(int ms)
			{

#if SILVERLIGHT_5||WINDOWS_PHONE_7||NET40
				await TaskEx.Delay(ms);


#else

				await Task.Delay(ms);
#endif

			}

		}

	}

}


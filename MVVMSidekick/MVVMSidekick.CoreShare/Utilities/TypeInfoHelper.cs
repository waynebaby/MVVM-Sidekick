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
using System.Reflection;
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
        /// Class TypeInfoHelper.
        /// </summary>
        public static class TypeInfoHelper
		{
#if NETFX_CORE
			/// <summary>
			/// Gets the type or type information.
			/// </summary>
			/// <param name="type">The type.</param>
			/// <returns>TypeInfo.</returns>
			public static TypeInfo GetTypeOrTypeInfo(this Type type)
			{
				return type.GetTypeInfo();

			}
#else
			/// <summary>
			/// Gets the type or type information.
			/// </summary>
			/// <param name="type">The type.</param>
			/// <returns>Type.</returns>
			public static Type GetTypeOrTypeInfo(this Type type)
			{
				return type;

			}
#endif

		}

	}

}


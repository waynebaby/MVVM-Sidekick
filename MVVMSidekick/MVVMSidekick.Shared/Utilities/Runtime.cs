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
        /// Class Runtime.
        /// </summary>
        public static class Runtime
		{

			/// <summary>
			/// The _ is in design mode
			/// </summary>
			static bool? _IsInDesignMode;


			/// <summary>
			/// <para>Gets if the code is running in design time. </para>
			/// <para>读取目前是否在设计时状态。</para>
			/// </summary>
			/// <value><c>true</c> if this instance is in design mode; otherwise, <c>false</c>.</value>
			public static bool IsInDesignMode
			{
				get
				{

					return (
						_IsInDesignMode
						??
						(

							_IsInDesignMode =
#if SILVERLIGHT_5||WINDOWS_PHONE_8||WINDOWS_PHONE_7
 DesignerProperties.IsInDesignTool
#elif NETFX_CORE
 Windows.ApplicationModel.DesignMode.DesignModeEnabled
#else
 (bool)System.ComponentModel.DependencyPropertyDescriptor
                                .FromProperty(
                                    System.ComponentModel.DesignerProperties.IsInDesignModeProperty,
                                    typeof(System.Windows.FrameworkElement))
                                .Metadata
                                .DefaultValue
#endif
))
						.Value;
				}

			}

		}

	}

}


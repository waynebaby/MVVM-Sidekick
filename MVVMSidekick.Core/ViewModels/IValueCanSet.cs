// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="ViewModels.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
#if WINDOWS_UWP


#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;
using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;
using System.Windows.Threading;
#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
#endif






namespace MVVMSidekick
{

    namespace ViewModels
    {
        /// <summary>
        /// <para>定义可以设置值的接口</para>
        /// <para>Interface that defines the ability to set a value</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>值的类型</para>
        /// <para>The type of the value</para>
        /// </typeparam>
        public interface IValueCanSet<in T>
        {
            /// <summary>
            /// <para>设置值</para>
            /// <para>Sets the value</para>
            /// </summary>
            /// <value>
            /// <para>类型为 T 的值</para>
            /// <para>The value of type T</para>
            /// </value>
            T Value { set; }
        }





    }

}

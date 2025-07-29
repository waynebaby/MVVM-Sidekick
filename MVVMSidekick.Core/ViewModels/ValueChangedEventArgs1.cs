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
using System.ComponentModel;
using System;
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
        /// <para>属性值已变化事件参数基类，包含旧值和新值字段</para>
        /// <para>Base event args that fired when property changed, with old value and new value field</para>
        /// </summary>
        public abstract class ValueChangedEventArgs : PropertyChangedEventArgs
        {
            /// <summary>
            /// <para>ValueChangedEventArgs的构造函数</para>
            /// <para>Constructor of ValueChangedEventArgs</para>
            /// </summary>
            /// <param name="propertyName">
            /// <para>属性名称</para>
            /// <para>Name of the property</para>
            /// </param>
            /// <param name="originalValue">
            /// <para>原始值</para>
            /// <para>The original value</para>
            /// </param>
            /// <param name="newValue">
            /// <para>新值</para>
            /// <para>The new value</para>
            /// </param>
            public ValueChangedEventArgs(string propertyName, object originalValue, object newValue)
                : base(propertyName)
            {

            }

            /// <summary>
            /// <para>获取新值的对象形式</para>
            /// <para>Gets the new value as an object</para>
            /// </summary>
            /// <value>
            /// <para>新值的对象表示</para>
            /// <para>The new value as object</para>
            /// </value>
            public abstract object GetNewValueObject();
            
            /// <summary>
            /// <para>获取原始值的对象形式</para>
            /// <para>Gets the original value as an object</para>
            /// </summary>
            /// <value>
            /// <para>原始值的对象表示</para>
            /// <para>The original value as object</para>
            /// </value>
            public abstract object GetOriginalValueObject();
        }



    }

}

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
#if NETFX_CORE


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
        /// <para>Event args that fired when property changed, with old value and new value field.</para>
        /// <para>值变化事件参数</para>
        /// </summary>
        /// <typeparam name="TProperty">Type of propery/变化属性的类型</typeparam>
        public abstract class ValueChangedEventArgs : PropertyChangedEventArgs
        {
            /// <summary>
            /// Constructor of ValueChangedEventArgs
            /// </summary>
            /// <param name="propertyName">Name of the property.</param>
            /// <param name="originalValue">The old value.</param>
            /// <param name="newValue">The new value.</param>
            public ValueChangedEventArgs(string propertyName, object originalValue, object newValue)
                : base(propertyName)
            {

            }

            /// <summary>
            /// New Value
            /// </summary>
            /// <value>The new value.</value>
            public abstract object GetNewValueObject();
            /// <summary>
            /// Old Value
            /// </summary>
            /// <value>The old value.</value>
            public abstract object GetOriginalValueObject();
        }



    }

}

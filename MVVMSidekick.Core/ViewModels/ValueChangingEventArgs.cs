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
using System.Threading;
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
        /// <para>属性值正在变化事件参数基类</para>
        /// <para>Base event args that fired when property is changing</para>
        /// </summary>
        public abstract class ValueChangingEventArgs : PropertyChangingEventArgs
        {
            /// <summary>
            /// <para>ValueChangingEventArgs的构造函数</para>
            /// <para>Constructor of ValueChangingEventArgs</para>
            /// </summary>
            /// <param name="propertyName">
            /// <para>属性名称</para>
            /// <para>Name of the property</para>
            /// </param>
            /// <param name="originalValue">
            /// <para>当前值</para>
            /// <para>The current value</para>
            /// </param>
            /// <param name="newValue">
            /// <para>新值</para>
            /// <para>The new value</para>
            /// </param>
            public ValueChangingEventArgs(string propertyName, object originalValue, object newValue)
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
            /// <para>获取当前值的对象形式</para>
            /// <para>Gets the current value as an object</para>
            /// </summary>
            /// <value>
            /// <para>当前值的对象表示</para>
            /// <para>The current value as object</para>
            /// </value>
            public abstract object GetCurrentValueObject();

            /// <summary>
            /// <para>取消令牌源，用于取消属性变化操作</para>
            /// <para>Cancellation token source for cancelling the property change operation</para>
            /// </summary>
            public CancellationTokenSource Cancellation { get; } = new CancellationTokenSource();
        }


        /// <summary>
        /// <para>属性值正在变化事件参数，包含当前值和新值字段的泛型版本</para>
        /// <para>Event args that fired when property is changing, with current value and new value field (generic version)</para>
        /// </summary>
        /// <typeparam name="TProperty">
        /// <para>变化属性的类型</para>
        /// <para>Type of the property that is changing</para>
        /// </typeparam>
        public class ValueChangingEventArgs<TProperty> : ValueChangingEventArgs
        {
            /// <summary>
            /// <para>ValueChangingEventArgs的构造函数</para>
            /// <para>Constructor of ValueChangingEventArgs</para>
            /// </summary>
            /// <param name="propertyName">
            /// <para>属性名称</para>
            /// <para>Name of the property</para>
            /// </param>
            /// <param name="currentValue">
            /// <para>当前值</para>
            /// <para>The current value</para>
            /// </param>
            /// <param name="newValue">
            /// <para>新值</para>
            /// <para>The new value</para>
            /// </param>
            public ValueChangingEventArgs(string propertyName, TProperty currentValue, TProperty newValue)
            : base(propertyName, currentValue, newValue)
            {
                NewValue = newValue;
                CurrentValue = currentValue;
            }

            /// <summary>
            /// <para>新值</para>
            /// <para>The new value</para>
            /// </summary>
            /// <value>
            /// <para>类型为 TProperty 的新值</para>
            /// <para>The new value of type TProperty</para>
            /// </value>
            public TProperty NewValue { get; private set; }
            
            /// <summary>
            /// <para>当前值</para>
            /// <para>The current value</para>
            /// </summary>
            /// <value>
            /// <para>类型为 TProperty 的当前值</para>
            /// <para>The current value of type TProperty</para>
            /// </value>
            public TProperty CurrentValue { get; private set; }

            /// <summary>
            /// <para>获取新值的对象形式</para>
            /// <para>Gets the new value as an object</para>
            /// </summary>
            /// <returns>
            /// <para>新值的对象表示</para>
            /// <para>The new value as object</para>
            /// </returns>
            public override object GetNewValueObject()
            {
                return NewValue;
            }

            /// <summary>
            /// <para>获取当前值的对象形式</para>
            /// <para>Gets the current value as an object</para>
            /// </summary>
            /// <returns>
            /// <para>当前值的对象表示</para>
            /// <para>The current value as object</para>
            /// </returns>
            public override object GetCurrentValueObject()
            {
                return CurrentValue;
            }
        }

    }

}

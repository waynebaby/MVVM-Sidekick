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
        /// <para>A slot to place the value container field and value container locator.</para>
        /// <para>属性定义。一个属性定义包括一个创建/定位属性“值容器”的静态方法引用，和一个缓存该方法执行结果“值容器”的槽位</para>
        /// </summary>
        /// <typeparam name="TProperty">Type of the property value /属性的类型</typeparam>
        public class Property<TProperty>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Property{TProperty}"/> class.
            /// </summary>
            public Property()
            {

            }
            public Property(Func<BindableBase, ValueContainer<TProperty>> locatorFunc)
            {
                LocatorFunc = locatorFunc;
            }

            /// <summary>
            /// <para>Locate or create the value container of this model intances</para>
            /// <para>通过定位方法定位本Model实例中的值容器</para>
            /// </summary>
            /// <param name="model">Model intances/model 实例</param>
            /// <returns>Value Container of this property/值容器</returns>
            public ValueContainer<TProperty> LocateValueContainer(BindableBase model)
            {
                return LocatorFunc(model);
            }


            /// <summary>
            /// <para>Gets sets the factory to locate/create value container of this model instance</para>
            /// <para>读取/设置定位值容器用的方法。</para>
            /// </summary>
            /// <value>The locator function.</value>
            public Func<BindableBase, ValueContainer<TProperty>> LocatorFunc
            {
               internal get;
                set;
            }

            /// <summary>
            /// <para>Gets or sets Value Container, it can be recently create and cached here，by LocatorFunc </para>
            /// <para>读取/设置值容器,这事值容器LocatorFunc创建值容器并且缓存的位置 </para>
            /// </summary>
            /// <value>The container.</value>
            public ValueContainer<TProperty> Container
            {
                get;
                set;
            }


        }




    }

}

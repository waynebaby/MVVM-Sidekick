// ***************************************************        /// <summary>
        /// <para>属性定义类。用于封装属性值容器的创建和定位逻辑</para>
        /// <para>Property definition class. Used to encapsulate value container creation and location logic</para>
        /// </summary>
        /// <typeparam name="TProperty">
        /// <para>属性值的类型</para>
        /// <para>Type of the property value</para>
        /// </typeparam>***************
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
            /// <para>初始化 <see cref="Property{TProperty}"/> 类的新实例</para>
            /// <para>Initializes a new instance of the <see cref="Property{TProperty}"/> class</para>
            /// </summary>
            public Property()
            {

            }
            
            /// <summary>
            /// <para>使用指定的定位器函数初始化 <see cref="Property{TProperty}"/> 类的新实例</para>
            /// <para>Initializes a new instance of the <see cref="Property{TProperty}"/> class with specified locator function</para>
            /// </summary>
            /// <param name="locatorFunc">
            /// <para>值容器定位器函数</para>
            /// <para>Value container locator function</para>
            /// </param>
            public Property(Func<BindableBase, ValueContainer<TProperty>> locatorFunc)
            {
                LocatorFunc = locatorFunc;
            }

            /// <summary>
            /// <para>定位或创建指定模型实例的值容器</para>
            /// <para>Locates or creates the value container for the specified model instance</para>
            /// </summary>
            /// <param name="model">
            /// <para>模型实例</para>
            /// <para>Model instance</para>
            /// </param>
            /// <returns>
            /// <para>此属性的值容器</para>
            /// <para>Value container of this property</para>
            /// </returns>
            public ValueContainer<TProperty> LocateValueContainer(BindableBase model)
            {
                return LocatorFunc(model);
            }


            /// <summary>
            /// <para>获取或设置用于定位/创建模型实例值容器的工厂方法</para>
            /// <para>Gets or sets the factory method to locate/create value container of model instance</para>
            /// </summary>
            /// <value>
            /// <para>定位器函数</para>
            /// <para>The locator function</para>
            /// </value>
            public Func<BindableBase, ValueContainer<TProperty>> LocatorFunc
            {
               internal get;
                set;
            }

            /// <summary>
            /// <para>获取或设置值容器。可以通过 LocatorFunc 创建并缓存在此处</para>
            /// <para>Gets or sets the value container. It can be created by LocatorFunc and cached here</para>
            /// </summary>
            /// <value>
            /// <para>值容器实例</para>
            /// <para>The value container instance</para>
            /// </value>
            public ValueContainer<TProperty> Container
            {
                get;
                set;
            }


        }




    }

}

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
using System.ComponentModel;






namespace MVVMSidekick
{

    namespace ViewModels
    {
        using EventRouting;
        using MVVMSidekick.Common;
        /// <summary>
        /// 可绑定接口，MVVM框架中的核心接口，定义了数据绑定和事件路由的基本功能
        /// Interface IBindable - Core interface in MVVM framework that defines basic functionality for data binding and event routing
        /// </summary>
        public interface IBindable : INotifyPropertyChanged, IDisposable, IDisposeGroup
        {
            /// <summary>
            /// 获取一个值，该值指示当前是否处于设计模式
            /// Gets a value indicating whether the current instance is in design mode
            /// </summary>
            bool IsInDesignMode { get; }
            
            /// <summary>
            /// 获取全局事件路由器
            /// Gets the Global event router.
            /// </summary>
            /// <value>全局事件路由器 / The event router.</value>
            EventRouter GlobalEventRouter { get; }

            /// <summary>
            /// 获取或设置本地事件路由器
            /// Gets or sets the event router.
            /// </summary>
            /// <value>本地事件路由器 / The event router.</value>
            EventRouter LocalEventRouter { get; set; }
            
            /// <summary>
            /// 获取可绑定实例的唯一标识符
            /// Gets the bindable instance identifier.
            /// </summary>
            /// <value>可绑定实例标识符 / The bindable instance identifier.</value>
            string BindableInstanceId { get; }

            /// <summary>
            /// 获取错误消息
            /// Gets the error.
            /// </summary>
            /// <value>错误消息 / The error.</value>
            string ErrorMessage { get; }

            /// <summary>
            /// 获取字段名称数组
            /// Gets the field names.
            /// </summary>
            /// <returns>字段名称数组 / System.String[].</returns>
            string[] GetFieldNames();
            
            /// <summary>
            /// 获取或设置具有指定名称的对象值
            /// Gets or sets the <see cref="System.Object"/> with the specified name.
            /// </summary>
            /// <param name="name">属性名称 / The name.</param>
            /// <returns>属性值对象 / System.Object.</returns>
            object this[string name] { get; set; }

            /// <summary>
            /// 获取指定属性名的值容器
            /// Gets the value container for the specified property name
            /// </summary>
            /// <param name="propertyName">属性名称 / The property name</param>
            /// <returns>值容器 / The value container</returns>
            IValueContainer GetValueContainer(string propertyName);
        }





    }

}

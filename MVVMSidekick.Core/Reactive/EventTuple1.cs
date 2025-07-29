// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Reactive.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************




namespace MVVMSidekick
{

    namespace Reactive
    {
        /// <summary>
        /// 事件元组结构，包含事件源和事件参数
        /// Event tuple structure containing event source and event arguments
        /// </summary>
        /// <typeparam name="TSource">源类型 / The type of the source.</typeparam>
        /// <typeparam name="TEventArgs">事件参数类型 / The type of the event arguments.</typeparam>
        public struct EventTuple<TSource, TEventArgs>
        {
            /// <summary>
            /// 获取或设置事件源
            /// Gets or sets the source.
            /// </summary>
            /// <value>事件源 / The source.</value>
            public TSource Source { get; set; }
            /// <summary>
            /// 获取或设置事件参数
            /// Gets or sets the event arguments.
            /// </summary>
            /// <value>事件参数 / The event arguments.</value>
            public TEventArgs EventArgs { get; set; }
        }

    }
}

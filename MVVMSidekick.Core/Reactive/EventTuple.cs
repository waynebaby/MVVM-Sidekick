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
        /// 事件元组静态工厂类
        /// Class EventTuple.
        /// </summary>
        public static class EventTuple
        {
            /// <summary>
            /// 创建指定的事件元组
            /// Creates the specified event tuple.
            /// </summary>
            /// <typeparam name="TSource">源类型 / The type of the source.</typeparam>
            /// <typeparam name="TEventArgs">事件参数类型 / The type of the event arguments.</typeparam>
            /// <param name="source">源对象 / The source.</param>
            /// <param name="eventArgs">包含事件数据的实例 / The instance containing the event data.</param>
            /// <returns>
            /// 事件元组实例 / EventTuple&lt;TSource, TEventArgs&gt;.
            /// </returns>
            public static EventTuple<TSource, TEventArgs> Create<TSource, TEventArgs>(TSource source, TEventArgs eventArgs)
            {
                return new EventTuple<TSource, TEventArgs> { Source = source, EventArgs = eventArgs };
            }

        }

    }
}

// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="EventRouting.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using MVVMSidekick.ViewModels;
using System.Reactive.Subjects;
using MVVMSidekick.Utilities;
using MVVMSidekick.Common;
using System.Runtime.CompilerServices;






namespace MVVMSidekick
{

    namespace EventRouting
    {




        /// <summary>
        /// 路由器事件数据结构
        /// Router event data structure
        /// </summary>
        /// <typeparam name="TEventArgs">事件参数类型 / Event arguments type</typeparam>
        public struct RouterEventData<TEventArgs>


		{
			/// <summary>
			/// 初始化 <see cref="RouterEventData{TEventArgs}" /> 结构的新实例
			/// Initializes a new instance of the <see cref="RouterEventData{TEventArgs}" /> struct.
			/// </summary>
			/// <param name="sender">发送者 / The sender.</param>
			/// <param name="eventName">事件名称 / Name of the event.</param>
			/// <param name="eventArgs">包含事件数据的实例 / The instance containing the event data.</param>
			public RouterEventData(object sender, string eventName, TEventArgs eventArgs)
			{

				_Sender = sender;
				_EventName = eventName;
				_EventArgs = eventArgs;
			}

			/// <summary>
			/// 发送者字段
			/// The sender field
			/// </summary>
			private Object _Sender;
			/// <summary>
			/// 获取事件发送者
			/// Gets the event sender
			/// </summary>
			/// <value>发送者 / The sender.</value>
			public Object Sender
			{
				get { return _Sender; }

			}

			/// <summary>
			/// 事件名称字段
			/// The event name field
			/// </summary>
			private string _EventName;

			/// <summary>
			/// 获取事件名称
			/// Gets the event name
			/// </summary>
			/// <value>事件名称 / The name of the event.</value>
			public string EventName
			{
				get { return _EventName; }
			}

			/// <summary>
			/// 事件参数字段
			/// The event arguments field
			/// </summary>
			private TEventArgs _EventArgs;
			/// <summary>
			/// 获取事件数据
			/// Gets the event data
			/// </summary>
			/// <value>事件数据 / The event arguments.</value>
			public TEventArgs EventData
			{
				get { return _EventArgs; }
			}
		}

	}


}

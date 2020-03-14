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
        /// 事件信息
        /// </summary>
        /// <typeparam name="TEventArgs">事件数据类型</typeparam>
        public struct RouterEventData<TEventArgs>


		{
			/// <summary>
			/// Initializes a new instance of the <see cref="RouterEventData{TEventArgs}" /> struct.
			/// </summary>
			/// <param name="sender">The sender.</param>
			/// <param name="eventName">Name of the event.</param>
			/// <param name="eventArgs">The instance containing the event data.</param>
			public RouterEventData(object sender, string eventName, TEventArgs eventArgs)
			{

				_Sender = sender;
				_EventName = eventName;
				_EventArgs = eventArgs;
			}

			/// <summary>
			/// The _ sender
			/// </summary>
			private Object _Sender;
			/// <summary>
			/// 事件发送者
			/// </summary>
			/// <value>The sender.</value>
			public Object Sender
			{
				get { return _Sender; }

			}

			/// <summary>
			/// The _ event name
			/// </summary>
			private string _EventName;

			/// <summary>
			/// 事件名
			/// </summary>
			/// <value>The name of the event.</value>
			public string EventName
			{
				get { return _EventName; }
			}

			/// <summary>
			/// The _ event arguments
			/// </summary>
			private TEventArgs _EventArgs;
			/// <summary>
			/// 事件数据
			/// </summary>
			/// <value>The event arguments.</value>
			public TEventArgs EventData
			{
				get { return _EventArgs; }
			}
		}

	}


}

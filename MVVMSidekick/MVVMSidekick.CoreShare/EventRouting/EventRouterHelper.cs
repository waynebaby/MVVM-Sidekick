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
using MVVMSidekick.ViewModels;

#if NETFX_CORE
using System.Collections.Concurrent;

#elif WPF



#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif





namespace MVVMSidekick
{

    namespace EventRouting
    {
        ///// <summary>
        ///// 保存状态事件数据
        ///// </summary>
        //public class SaveStateEventArgs : EventArgs
        //{
        //	/// <summary>
        //	/// Gets or sets the view key identifier.
        //	/// </summary>
        //	/// <value>The view key identifier.</value>
        //	public string ViewKeyId { get; set; }
        //	/// <summary>
        //	/// Gets or sets the state.
        //	/// </summary>
        //	/// <value>The state.</value>
        //	public Dictionary<string, object> State { get; set; }
        //}

        /// <summary>
        /// 事件路由的扩展方法集合
        /// </summary>
        public static class EventRouterHelper
		{
			/// <summary>
			/// 触发事件
			/// </summary>
			/// <typeparam name="TEventArgs">The type of the t event arguments.</typeparam>
			/// <param name="source">事件来源</param>
			/// <param name="eventArgs">事件数据</param>
			/// <param name="callerMemberName">事件名</param>
			public static void RaiseEvent<TEventArgs>(this BindableBase source, TEventArgs eventArgs, string callerMemberName = "")
#if !NETFX_CORE
			// where TEventArgs : EventArgs
#endif

			{
				EventRouter.Instance.RaiseEvent(source, eventArgs, callerMemberName);
			}

		}



		//#if !NETFX_CORE

		//		/// <summary>
		//		/// Class DataEventArgs.
		//		/// </summary>
		//		/// <typeparam name="TData">The type of the t data.</typeparam>
		//		public class DataEventArgs<TData> : EventArgs
		//		{
		//			/// <summary>
		//			/// Initializes a new instance of the <see cref="DataEventArgs{TData}" /> class.
		//			/// </summary>
		//			/// <param name="data">The data.</param>
		//			public DataEventArgs(TData data)
		//			{

		//				Data = data;
		//			}

		//			/// <summary>
		//			/// Gets or sets the data.
		//			/// </summary>
		//			/// <value>The data.</value>
		//			public TData Data { get; protected set; }

		//		}
		//#endif
	}


}

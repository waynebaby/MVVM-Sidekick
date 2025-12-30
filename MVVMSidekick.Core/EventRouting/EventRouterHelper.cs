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





namespace MVVMSidekick
{

    namespace EventRouting
    {
       
        /// <summary>
        /// 事件路由的扩展方法集合
        /// Event routing extension methods collection
        /// </summary>
        public static class EventRouterHelper
        {
            /// <summary>
            /// 触发事件，通过事件路由器分发事件
            /// Triggers event and dispatches through event router
            /// </summary>
            /// <typeparam name="TEventArgs">事件参数类型 / Event arguments type</typeparam>
            /// <param name="source">事件来源 / Event source</param>
            /// <param name="eventArgs">事件数据 / Event data</param>
            /// <param name="callerMemberName">事件名称 / Event name</param>
            public static void RaiseEvent<TEventArgs>(this BindableBase source, TEventArgs eventArgs, string callerMemberName = "")


            {
                EventRouter.Instance.RaiseEvent(source, eventArgs, callerMemberName);
            }

        }


    }


}

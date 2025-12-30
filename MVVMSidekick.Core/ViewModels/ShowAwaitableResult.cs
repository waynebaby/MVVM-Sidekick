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
using System.Threading.Tasks;
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
        /// <para>显示可等待结果结构体</para>
        /// <para>Structure for show awaitable result</para>
        /// </summary>
        /// <typeparam name="TViewModel">
        /// <para>视图模型的类型</para>
        /// <para>The type of the view model</para>
        /// </typeparam>
        public struct ShowAwaitableResult<TViewModel>
        {
            /// <summary>
            /// <para>获取或设置视图模型</para>
            /// <para>Gets or sets the view model</para>
            /// </summary>
            /// <value>
            /// <para>视图模型实例</para>
            /// <para>The view model instance</para>
            /// </value>
            public TViewModel ViewModel { get; set; }
            
            /// <summary>
            /// <para>获取或设置关闭任务</para>
            /// <para>Gets or sets the closing task</para>
            /// </summary>
            /// <value>
            /// <para>表示关闭操作的异步任务</para>
            /// <para>The asynchronous task representing the closing operation</para>
            /// </value>
            public Task Closing { get; set; }

        }





    }

}

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
using System.Threading.Tasks;





namespace MVVMSidekick
{

    namespace ViewModels
    {
        /// <summary>
        /// <para>具有结果类型的视图模型接口</para>
        /// <para>Interface for ViewModels with a specific result type</para>
        /// </summary>
        /// <typeparam name="TResult">
        /// <para>结果类型</para>
        /// <para>The type of the result</para>
        /// </typeparam>
        public partial interface IViewModel<TResult> : IViewModel
        {
            /// <summary>
            /// <para>等待关闭并返回结果</para>
            /// <para>Waits for the ViewModel to close and returns the result</para>
            /// </summary>
            /// <param name="closingCallback">
            /// <para>关闭时的回调函数</para>
            /// <para>Optional callback function to execute when closing</para>
            /// </param>
            /// <returns>
            /// <para>返回包含结果的异步任务</para>
            /// <para>A task that returns the result when the ViewModel closes</para>
            /// </returns>
            Task<TResult> WaitForCloseWithResult(Action closingCallback = null);
            
            /// <summary>
            /// <para>获取或设置结果值</para>
            /// <para>Gets or sets the result value</para>
            /// </summary>
            /// <value>
            /// <para>类型为 TResult 的结果值</para>
            /// <para>The result value of type TResult</para>
            /// </value>
            TResult Result { get; set; }
        }
    }
}

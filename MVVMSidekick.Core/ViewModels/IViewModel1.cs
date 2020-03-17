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
        /// Interface IViewModel
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        public partial interface IViewModel<TResult> : IViewModel
        {
            /// <summary>
            /// Waits for close with result.
            /// </summary>
            /// <param name="closingCallback">The closing callback.</param>
            /// <returns>Task&lt;TResult&gt;.</returns>
            Task<TResult> WaitForCloseWithResult(Action closingCallback = null);
            /// <summary>
            /// Gets or sets the result.
            /// </summary>
            /// <value>The result.</value>
            TResult Result { get; set; }
        }
    }
}

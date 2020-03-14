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






namespace MVVMSidekick
{

    namespace ViewModels
    {
        using MVVMSidekick.Common;

        /// <summary>
        /// Interface IViewModelLifetime
        /// </summary>
        public interface IViewModelLifetime : IDisposeGroup
        {
            /// <summary>
            /// Called when [binded to view].
            /// </summary>
            /// <param name="view">The view.</param>
            /// <param name="oldValue">The old value.</param>
            /// <returns>Task.</returns>
            Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue);
            /// <summary>
            /// Called when [unbinded from view].
            /// </summary>
            /// <param name="view">The view.</param>
            /// <param name="newValue">The new value.</param>
            /// <returns>Task.</returns>
            Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue);
            /// <summary>
            /// Called when [binded view load].
            /// </summary>
            /// <param name="view">The view.</param>
            /// <returns>Task.</returns>
            Task OnBindedViewLoad(MVVMSidekick.Views.IView view);
            /// <summary>
            /// Called when [binded view unload].
            /// </summary>
            /// <param name="view">The view.</param>
            /// <returns>Task.</returns>
            Task OnBindedViewUnload(MVVMSidekick.Views.IView view);


            /// <summary>
            /// the group that would dispose when Unbind
            /// </summary>
            IDisposeGroup UnbindDisposeGroup { get; }
            /// <summary>
            ///  the group that would dispose when Unload
            /// </summary>
            IDisposeGroup UnloadDisposeGroup { get; }


        }





    }

}

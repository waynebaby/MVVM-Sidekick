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
        /// <para>视图模型生命周期管理接口</para>
        /// <para>Interface for managing ViewModel lifecycle events</para>
        /// </summary>
        public interface IViewModelLifetime : IDisposeGroup
        {
            /// <summary>
            /// <para>当视图模型绑定到视图时调用</para>
            /// <para>Called when the ViewModel is bound to a view</para>
            /// </summary>
            /// <param name="view">
            /// <para>绑定的视图</para>
            /// <para>The view being bound to</para>
            /// </param>
            /// <param name="oldValue">
            /// <para>之前绑定的视图模型</para>
            /// <para>The previously bound ViewModel</para>
            /// </param>
            /// <returns>
            /// <para>异步任务</para>
            /// <para>Asynchronous task</para>
            /// </returns>
            Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue);
            
            /// <summary>
            /// <para>当视图模型从视图解绑时调用</para>
            /// <para>Called when the ViewModel is unbound from a view</para>
            /// </summary>
            /// <param name="view">
            /// <para>解绑的视图</para>
            /// <para>The view being unbound from</para>
            /// </param>
            /// <param name="newValue">
            /// <para>新绑定的视图模型</para>
            /// <para>The newly bound ViewModel</para>
            /// </param>
            /// <returns>
            /// <para>异步任务</para>
            /// <para>Asynchronous task</para>
            /// </returns>
            Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue);
            
            /// <summary>
            /// <para>当绑定的视图加载时调用</para>
            /// <para>Called when the bound view is loaded</para>
            /// </summary>
            /// <param name="view">
            /// <para>加载的视图</para>
            /// <para>The view being loaded</para>
            /// </param>
            /// <returns>
            /// <para>异步任务</para>
            /// <para>Asynchronous task</para>
            /// </returns>
            Task OnBindedViewLoad(MVVMSidekick.Views.IView view);
            
            /// <summary>
            /// <para>当绑定的视图卸载时调用</para>
            /// <para>Called when the bound view is unloaded</para>
            /// </summary>
            /// <param name="view">
            /// <para>卸载的视图</para>
            /// <para>The view being unloaded</para>
            /// </param>
            /// <returns>
            /// <para>异步任务</para>
            /// <para>Asynchronous task</para>
            /// </returns>
            Task OnBindedViewUnload(MVVMSidekick.Views.IView view);

            /// <summary>
            /// <para>解绑时会释放的资源组</para>
            /// <para>The group of resources that will be disposed when unbinding</para>
            /// </summary>
            IDisposeGroup UnbindDisposeGroup { get; }
            
            /// <summary>
            /// <para>卸载时会释放的资源组</para>
            /// <para>The group of resources that will be disposed when unloading</para>
            /// </summary>
            IDisposeGroup UnloadDisposeGroup { get; }
        }





    }

}

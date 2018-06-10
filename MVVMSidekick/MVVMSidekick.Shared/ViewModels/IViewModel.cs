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
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
#if NETFX_CORE


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
        /// Interface IViewModel
        /// </summary>
        public partial interface IViewModel : IBindable, INotifyPropertyChanged,INotifyPropertyChanging, IViewModelLifetime
        {
#if NETFX_CORE
            /// <summary>
            /// Gets the dispatcher of view.
            /// </summary>
            Windows.UI.Core.CoreDispatcher Dispatcher { get; }
#else
            /// <summary>
            /// Gets the dispatcher of view.
            /// </summary>
            /// <value>The dispatcher.</value>
            Dispatcher Dispatcher { get; }

#endif
            /// <summary>
            /// Waits for close.
            /// </summary>
            /// <param name="closingCallback">The closing callback.</param>
            /// <returns>Task.</returns>
            Task WaitForClose(Action closingCallback = null);
            /// <summary>
            /// Gets a value indicating whether this instance is UI busy.
            /// </summary>
            /// <value><c>true</c> if this instance is UI busy; otherwise, <c>false</c>.</value>
            bool IsUIBusy { get; }
            /// <summary>
            /// Gets a value indicating whether [have return value].
            /// </summary>
            /// <value><c>true</c> if [have return value]; otherwise, <c>false</c>.</value>
            bool HaveReturnValue { get; }
            /// <summary>
            /// Closes the view and dispose.
            /// </summary>
            void CloseViewAndDispose();
            /// <summary>
            /// Gets or sets the stage manager.
            /// </summary>
            /// <value>The stage manager.</value>
            MVVMSidekick.Views.IStageManager StageManager { get; set; }


            /// <summary>
            /// Executes the task.
            /// </summary>
            /// <typeparam name="Tin">The type of the in.</typeparam>
            /// <typeparam name="Tout">The type of the out.</typeparam>
            /// <param name="taskBody">The task body.</param>
            /// <param name="inputContext">The input context.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
            /// <returns></returns>
            Task<Tout> ExecuteFunctionTask<Tin, Tout>(Func<Tin, CancellationToken, Task<Tout>> taskBody, Tin inputContext, CancellationToken cancellationToken, bool UIBusyWhenExecuting = true);

            /// <summary>
            /// Executes the task.
            /// </summary>
            /// <typeparam name="Tin">The type of the in.</typeparam>
            /// <param name="taskBody">The task body.</param>
            /// <param name="inputContext">The input context.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
            /// <returns>in value</returns>
            Task ExecuteTask<Tin>(Func<Tin, CancellationToken, Task> taskBody, Tin inputContext, CancellationToken cancellationToken, bool UIBusyWhenExecuting = true);



            /// <summary>
            /// Executes the task.
            /// </summary>
            /// <typeparam name="Tin">The type of the in.</typeparam>
            /// <param name="taskBody">The task body.</param>
            /// <param name="inputContext">The input context.</param>
            /// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
            /// <returns></returns>
            Task ExecuteTask<Tin>(Func<Tin, Task> taskBody, Tin inputContext, bool UIBusyWhenExecuting = true);

            /// <summary>
            /// Executes the task.
            /// </summary>
            /// <typeparam name="Tout">The type of the out.</typeparam>
            /// <param name="taskBody">The task body.</param>
            /// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
            /// <returns></returns>
            Task<Tout> ExecuteCaculation<Tout>(Func<Task<Tout>> taskBody, bool UIBusyWhenExecuting = true);

            /// <summary>
            /// Executes the task.
            /// </summary>
            /// <param name="taskBody">The task body.</param>
            /// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
            /// <returns>Task.</returns>
            Task ExecuteTask(Func<Task> taskBody, bool UIBusyWhenExecuting = true);

            //IObservable<Task<Tout>> DoExecuteUIBusyTask<Tin, Tout>(this IObservable<Tin> sequence,IViewModel , Func<Tin, Task<Tout>> taskBody);
            //IObservable<Task<Tout>> DoExecuteUIBusyTask<Tin, Tout>(this IObservable<Tin> sequence, Func<Tin,Task<Tout>> taskBody, TaskScheduler scheduler);


            /// <summary>
            /// Set: Will VM be Disposed when unbind from View.
            /// </summary>
            /// <value><c>true</c> if this instance is disposing when unbind required; otherwise, <c>false</c>.</value>
            bool IsDisposingWhenUnbindRequired { get; set; }

            /// <summary>
            /// Set: Will VM be Disposed when unload from View.
            /// </summary>
            /// <value><c>true</c> if this instance is disposing when unload required; otherwise, <c>false</c>.</value>
            bool IsDisposingWhenUnloadRequired { get; set; }

#if NETFX_CORE

            /// <summary>
            /// Load state of this view
            /// </summary>
            /// <param name="navigationParameter"></param>
            /// <param name="pageState"></param>
            void LoadState(Object navigationParameter, Dictionary<String, Object> pageState);

            /// <summary>
            /// Save state of this view
            /// </summary>
            /// <param name="pageState"></param>
            void SaveState(Dictionary<String, Object> pageState);
#endif
        }





    }

}

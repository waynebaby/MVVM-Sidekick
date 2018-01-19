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
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using MVVMSidekick.Commands;
using System.Runtime.CompilerServices;
using MVVMSidekick.Reactive;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
#if NETFX_CORE
using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;


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

namespace MVVMSidekick.ViewModels
{
    using EventRouting;
    using System.Reactive.Disposables;
    using Utilities;
    using Views;
    using MVVMSidekick.Common;
    using System.Reactive;
    using System.Diagnostics;

    /// <summary>
    /// Class ViewModelBase.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
    /// <typeparam name="TResult">The type of the t result.</typeparam>
    public partial class ViewModelBase<TViewModel, TResult> : ViewModelBase<TViewModel>, IViewModel<TResult>
        where TViewModel : ViewModelBase<TViewModel, TResult>, IViewModel<TResult>
    {

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets a value indicating whether [have return value].
        /// </summary>
        /// <value><c>true</c> if [have return value]; otherwise, <c>false</c>.</value>
        public override bool HaveReturnValue { get { return true; } }

        /// <summary>
        /// Waits for close with result.
        /// </summary>
        /// <param name="closingCallback">The closing callback.</param>
        /// <returns>Task&lt;TResult&gt;.</returns>
        public async Task<TResult> WaitForCloseWithResult(Action closingCallback = null)
        {
            var t = new TaskCompletionSource<TResult>();

            this.AddDisposeAction(
                () =>
                {
                    if (closingCallback != null)
                    {
                        closingCallback();
                    }
                    t.SetResult(Result);
                }
                );


            await t.Task;
            return Result;
        }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public TResult Result
        {
            get { return _ResultLocator(this).Value; }
            set { _ResultLocator(this).SetValueAndTryNotify(value); }
        }

        #region Property TResult Result Setup
        /// <summary>
        /// The _ result
        /// </summary>
        protected Property<TResult> _Result =
          new Property<TResult>( _ResultLocator);
        /// <summary>
        /// The _ result locator
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<TResult>> _ResultLocator =
            RegisterContainerLocator<TResult>(
                "Result",
                model =>
                {
                    model._Result =
                        model._Result
                        ??
                        new Property<TResult>( _ResultLocator);
                    return model._Result.Container =
                        model._Result.Container
                        ??
                        new ValueContainer<TResult>("Result", model);
                });
        #endregion


    }


    /// <summary>
    /// 一个VM,带有若干界面特性
    /// </summary>
    /// <typeparam name="TViewModel">本身的类型</typeparam>
    [DataContract]
    public abstract partial class ViewModelBase<TViewModel> : BindableBase<TViewModel>, IViewModel where TViewModel : ViewModelBase<TViewModel>
    {
        IDisposeGroup _UnbindDisposeGroup = new DisposeGroup();

        IDisposeGroup _UnloadDisposeGroup = new DisposeGroup();
        /// <summary>
        /// Resource Group that need dispose when Unbind from UI;
        /// </summary>

        public IDisposeGroup UnbindDisposeGroup
        {
            get { return _UnbindDisposeGroup; }
            //set { _UnbindDisposeGroup = value; }
        }

        /// <summary>
        /// Resource Group that need dispose when Unload from UI;
        /// </summary>											   

        public IDisposeGroup UnloadDisposeGroup
        {
            get { return _UnloadDisposeGroup; }
            //set { _UnloadDisposeGroup = value; }
        }


        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase{TViewModel}" /> class.
        /// </summary>
        public ViewModelBase()
        {

            this.IsDisposingWhenUnloadRequired = false;
            this.IsDisposingWhenUnbindRequired = false;


            GetValueContainer(x => x.UIBusyTaskCount)
                .GetNewValueObservable()
                .Select(e =>
                    e.EventArgs != 0)
                .DistinctUntilChanged()
                .Subscribe(isBusy =>
                    IsUIBusy = isBusy)
                .DisposeWith(this);
        }





        /// <summary>
        /// Called when [binded to view].
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="oldValue">The old value.</param>
        /// <returns>Task.</returns>
        Task IViewModelLifetime.OnBindedToView(IView view, IViewModel oldValue)
        {

            return OnBindedToView(view, oldValue);
        }

        /// <summary>
        /// Called when [unbinded from view].
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>Task.</returns>
        Task IViewModelLifetime.OnUnbindedFromView(IView view, IViewModel newValue)
        {
            return OnUnbindedFromView(view, newValue);
        }

        /// <summary>
        /// Called when [binded view load].
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>Task.</returns>
        Task IViewModelLifetime.OnBindedViewLoad(IView view)
        {
            foreach (var item in GetFieldNames())
            {
                RaisePropertyChanged(() => new PropertyChangedEventArgs(item));
            }
            return OnBindedViewLoad(view);
        }
        /// <summary>
        /// Called when [binded view unload].
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>Task.</returns>
        Task IViewModelLifetime.OnBindedViewUnload(IView view)
        {
            return OnBindedViewUnload(view);
        }


        /// <summary>
        /// This will be invoked by view when this viewmodel is set to view's ViewModel property.
        /// </summary>
        /// <param name="view">Set target view</param>
        /// <param name="oldValue">Value before set.</param>
        /// <returns>Task awaiter</returns>
        protected virtual async Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
        {
            //#if SILVERLIGHT_5
            //                await T.askEx.Yield();
            //#else
            //                await T.ask.Yield();
            //#endif
            if (view != null)
            {
                StageManager = new StageManager(this) { CurrentBindingView = view };
                StageManager.InitParent(()
                    => view.Parent);
            }
            //StageManager.DisposeWith(this);
            await TaskExHelper.Yield();
        }


        /// <summary>
        /// This will be invoked by view when this instance of viewmodel in ViewModel property is overwritten.
        /// </summary>
        /// <param name="view">Overwrite target view.</param>
        /// <param name="newValue">The value replacing</param>
        /// <returns>Task awaiter</returns>
        protected virtual async Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue)
        {
            try
            {
                UnbindDisposeGroup.Dispose();
                if (IsDisposingWhenUnbindRequired)
                {
                    Dispose();
                }
                await TaskExHelper.Yield();
            }
            catch (Exception)
            {
                StageManager = null;
            }
        }

        /// <summary>
        /// This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property
        /// </summary>
        /// <param name="view">View that firing Load event</param>
        /// <returns>Task awaiter</returns>
        protected virtual async Task OnBindedViewLoad(IView view)
        {
            if (view != null)
            {
                StageManager = new StageManager(this) { CurrentBindingView = view };
                StageManager.InitParent(() => 
                    view.Parent);
            }

            await TaskExHelper.Yield();
        }

        /// <summary>
        /// This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's  ViewModel property
        /// </summary>
        /// <param name="view">View that firing Unload event</param>
        /// <returns>Task awaiter</returns>
        protected virtual async Task OnBindedViewUnload(IView view)
        {

            try
            {
                UnloadDisposeGroup.Dispose();
                if (IsDisposingWhenUnloadRequired)
                {
                    this.Dispose();
                }
                await TaskExHelper.Yield();
            }

            finally
            {
                StageManager = null;
            }
        }

        /// <summary>
        /// Set: Will VM be Disposed when unbind from View.
        /// </summary>
        /// <value><c>true</c> if this instance is disposing when unbind required; otherwise, <c>false</c>.</value>
        public bool IsDisposingWhenUnbindRequired { get; set; }

        /// <summary>
        /// Set: Will VM be Disposed when unload from View.
        /// </summary>
        /// <value><c>true</c> if this instance is disposing when unload required; otherwise, <c>false</c>.</value>
        public bool IsDisposingWhenUnloadRequired { get; set; }


#if NETFX_CORE
        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        public virtual void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {

        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. 
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        public virtual void SaveState(Dictionary<String, Object> pageState)
        {

        }
#endif

        /// <summary>
        /// The _ stage manager
        /// </summary>
        MVVMSidekick.Views.IStageManager _StageManager = new TestingStageManager();

        /// <summary>
        /// Gets or sets the stage manager.												 I
        /// </summary>
        /// <value>The stage manager.</value>
        //[Microsoft.Practices.Unity.Dependency(Testing.Constants.DependencyKeyForTesting)]
        public MVVMSidekick.Views.IStageManager StageManager
        {
            get { return _StageManager; }
            set { _StageManager = value; }
        }

        /// <summary>
        /// 是否有返回值
        /// </summary>
        /// <value><c>true</c> if [have return value]; otherwise, <c>false</c>.</value>
        public virtual bool HaveReturnValue { get { return false; } }
        /// <summary>
        /// 本UI是否处于忙状态
        /// </summary>
        /// <value><c>true</c> if this instance is UI busy; otherwise, <c>false</c>.</value>

        public bool IsUIBusy
        {
            get { return _IsUIBusyLocator(this).Value; }
            set { _IsUIBusyLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsUIBusy Setup
        /// <summary>
        /// The _ is UI busy
        /// </summary>
        protected Property<bool> _IsUIBusy = new Property<bool>( _IsUIBusyLocator);
        /// <summary>
        /// The _ is UI busy locator
        /// </summary>
        static Func<BindableBase, ValueContainer<bool>> _IsUIBusyLocator = RegisterContainerLocator<bool>("IsUIBusy", model => model.Initialize("IsUIBusy", ref model._IsUIBusy, ref _IsUIBusyLocator, _IsUIBusyDefaultValueFactory));
        /// <summary>
        /// The _ is UI busy default value factory
        /// </summary>
        static Func<bool> _IsUIBusyDefaultValueFactory = null;
        #endregion



        /// <summary>
        /// Gets or sets the UI busy task count.
        /// </summary>
        /// <value>The UI busy task count.</value>
        private int UIBusyTaskCount
        {
            get { return _UIBusyTaskCountLocator(this).Value; }
            set { _UIBusyTaskCountLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property int UIBusyTaskCount Setup
        private Property<int> _UIBusyTaskCount = new Property<int>( _UIBusyTaskCountLocator);
        private static Func<BindableBase, ValueContainer<int>> _UIBusyTaskCountLocator = RegisterContainerLocator<int>("UIBusyTaskCount", model => model.Initialize("UIBusyTaskCount", ref model._UIBusyTaskCount, ref _UIBusyTaskCountLocator, _UIBusyTaskCountDefaultValueFactory));
        private static Func<int> _UIBusyTaskCountDefaultValueFactory = null;
        #endregion


        /// <summary>
        /// Waits for close.
        /// </summary>
        /// <param name="closingCallback">The closing callback.</param>
        /// <returns>Task.</returns>
        public async Task WaitForClose(Action closingCallback = null)
        {
            var t = new TaskCompletionSource<object>();

            this.AddDisposeAction(
                () =>
                {
                    if (closingCallback != null)
                    {
                        closingCallback();
                    }
                    t.SetResult(null);
                }
                );


            await t.Task;
        }
        /// <summary>
        /// Closes the view and dispose.
        /// </summary>
        public void CloseViewAndDispose()
        {
            this.StageManager?.CurrentBindingView?.SelfClose();
            Dispose();
        }



        /// <summary>
        /// Runs the on dispatcher.
        /// </summary>
        /// <param name="action">The action.</param>
        private async void RunOnDispatcher(Action action)
        {


#if NETFX_CORE
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(action));

#elif NET45

            await Dispatcher.BeginInvoke(action).Task;
#else
			await TaskExHelper.Yield();
			Dispatcher.BeginInvoke(action);

#endif

        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <typeparam name="Tin">The type of the tin.</typeparam>
        /// <typeparam name="Tout">The type of the tout.</typeparam>
        /// <param name="taskBody">The task body.</param>
        /// <param name="inputContext">The input context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
        /// <returns>Task&lt;Tout&gt;.</returns>
        public virtual async Task<Tout> ExecuteTask<Tin, Tout>(Func<Tin, CancellationToken, Task<Tout>> taskBody, Tin inputContext, CancellationToken cancellationToken, bool UIBusyWhenExecuting = true)
        {


            var cmdarh = inputContext as EventPattern<EventCommandEventArgs>;
            if (cmdarh != null)
            {
                var oldBody = taskBody;
                taskBody = async (i, c) =>
                {
                    try
                    {

                        var rval = await oldBody(i, c);


                        if (c.IsCancellationRequested)
                        {
                            cmdarh.EventArgs.Completion.TrySetCanceled();
                        }
                        else
                        {
                            cmdarh.EventArgs.Completion.TrySetResult(cmdarh.EventArgs);
                        }
                        return rval;
                    }
                    catch (Exception ex)
                    {
                        cmdarh.EventArgs.Completion.SetException(ex);
                        EventRouter.Instance.RaiseEvent(this, ex);
                        throw;
                    }

                };
            }

            if (UIBusyWhenExecuting)
            {
                using (
                    Disposable.Create(
                        () =>
                            UIBusyTaskCount--))
                {
                    UIBusyTaskCount++;

                    return await taskBody(inputContext, cancellationToken);

                }
            }
            else
            {
                return await taskBody(inputContext, cancellationToken);
            }



        }


        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <typeparam name="Tin">The type of the in.</typeparam>
        /// <param name="taskBody">The task body.</param>
        /// <param name="inputContext">The input context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
        /// <returns>
        /// Task.
        /// </returns>
        public virtual async Task ExecuteTask<Tin>(Func<Tin, CancellationToken, Task> taskBody, Tin inputContext, CancellationToken cancellationToken, bool UIBusyWhenExecuting = true)
        {

            await ExecuteTask<Tin, object>(async (i, c) => { await taskBody(i, c); return null; }, inputContext, cancellationToken, UIBusyWhenExecuting);
        }


        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <typeparam name="Tin">The type of the tin.</typeparam>
        /// <typeparam name="Tout">The type of the tout.</typeparam>
        /// <param name="taskBody">The task body.</param>
        /// <param name="inputContext">The input context.</param>
        /// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
        /// <returns>Task&lt;Tout&gt;.</returns>
        public virtual async Task<Tout> ExecuteTask<Tin, Tout>(Func<Tin, Task<Tout>> taskBody, Tin inputContext, bool UIBusyWhenExecuting = true)
        {
            return await ExecuteTask<Tin, Tout>(async (i, c) => await taskBody(i), inputContext, CancellationToken.None, UIBusyWhenExecuting);

        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <typeparam name="Tin">The type of the in.</typeparam>
        /// <param name="taskBody">The task body.</param>
        /// <param name="inputContext">The input context.</param>
        /// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
        /// <returns>
        /// Task.
        /// </returns>
        public virtual async Task ExecuteTask<Tin>(Func<Tin, Task> taskBody, Tin inputContext, bool UIBusyWhenExecuting = true)
        {
            await ExecuteTask<Tin, object>(async (i, c) => { await taskBody(i); return null; }, inputContext, CancellationToken.None, UIBusyWhenExecuting);

        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <typeparam name="Tout">The type of the out.</typeparam>
        /// <param name="taskBody">The task body.</param>
        /// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
        /// <returns>
        /// Task&lt;Tout&gt;.
        /// </returns>
        public virtual async Task<Tout> ExecuteTask<Tout>(Func<Task<Tout>> taskBody, bool UIBusyWhenExecuting = true)
        {
            return await ExecuteTask<object, Tout>(
                async (i, c) =>
                {
                    return await taskBody();
                },
                null,
                CancellationToken.None,
                UIBusyWhenExecuting);

        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="taskBody">The task body.</param>
        /// <param name="UIBusyWhenExecuting">if set to <c>true</c> [UI busy when executing].</param>
        /// <returns>Task.</returns>
        public virtual async Task ExecuteTask(Func<Task> taskBody, bool UIBusyWhenExecuting = true)
        {
            await ExecuteTask<object, object>(
                async (i, c) =>
                {
                    await taskBody();
                    return null;
                },
                null,
                CancellationToken.None, UIBusyWhenExecuting);

        }

#if NETFX_CORE
        private Windows.UI.Core.CoreDispatcher GetCurrentViewDispatcher()
        {
            Windows.UI.Xaml.DependencyObject dp = null;
            if (this.StageManager == null)
            {
                return null;
            }
            else if ((dp = (this.StageManager.CurrentBindingView as Windows.UI.Xaml.DependencyObject)) == null)
            {
                return null;
            }
            return dp.Dispatcher;

        }

        public virtual void OnPageNavigatedTo(NavigationEventArgs e)
        {
            
        }
        public virtual void OnPageNavigatedFrom(NavigationEventArgs e)
        {

        }
        public virtual void OnPageNavigatingFrom(NavigatingCancelEventArgs e)
        {
           
        }
#else

        /// <summary>
        /// Gets the current view dispatcher.
        /// </summary>
        /// <returns>Dispatcher.</returns>
        private Dispatcher GetCurrentViewDispatcher()
        {
            DependencyObject dp = null;
            if (this.StageManager == null)
            {
                return null;
            }
            else if ((dp = (this.StageManager.CurrentBindingView as DependencyObject)) == null)
            {
                return null;
            }
            return dp.Dispatcher;

        }
#endif
#if NETFX_CORE
        /// <summary>
        /// Gets the current view dispatcher.	
        /// </summary>
        public Windows.UI.Core.CoreDispatcher Dispatcher
        {
            get
            {

                var current = GetCurrentViewDispatcher();
                if (current != null)
                {
                    return current;
                }
                if (Windows.UI.Xaml.Window.Current == null)
                {
                    return null;
                }
                return Windows.UI.Xaml.Window.Current.Dispatcher;
            }


        }



#elif WPF
        /// <summary>
        /// Explode the dispatcher of current view.
        /// </summary>
        public Dispatcher Dispatcher
        {
            get
            {
                var current = GetCurrentViewDispatcher();
                if (current != null)
                {
                    return current;
                }
                if (Application.Current == null)
                {
                    return null;
                }

                return Application.Current.Dispatcher;
            }


        }
#elif SILVERLIGHT_5 || WINDOWS_PHONE_8
		/// <summary>
		/// Gets the dispatcher.
		/// </summary>
		/// <value>The dispatcher.</value>
		public Dispatcher Dispatcher
		{
			get
			{
				var current = GetCurrentViewDispatcher();
				if (current != null)
				{
					return current;
				}
				if (Application.Current == null)
				{
					return null;
				}
				else if (Application.Current.RootVisual == null)
				{
					return null;
				}
				else return Application.Current.RootVisual.Dispatcher;
			}


		}
#endif
    }


}
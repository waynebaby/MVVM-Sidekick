
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Threading;
using MVVMSidekick.Commands;
using MVVMSidekick.Reactive;
using System.Reactive.Linq;


namespace MVVMSidekick.ViewModels
{
    using EventRouting;
    using MVVMSidekick.Common;
    using System.Reactive;
    using System.Reactive.Disposables;
    using Utilities;
    using Views;

    /// <summary>
    /// Class ViewModelBase.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
    /// <typeparam name="TResult">The type of the t result.</typeparam>
    public  abstract partial class ViewModelBase<TViewModel, TResult> : ViewModelBase<TViewModel>, IViewModel<TResult>
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
        public override bool HaveReturnValue => true;

        /// <summary>
        /// Waits for close with result.
        /// </summary>
        /// <param name="closingCallback">The closing callback.</param>
        /// <returns>Task&lt;TResult&gt;.</returns>
        public async Task<TResult> WaitForCloseWithResult(Action closingCallback = null)
        {
            TaskCompletionSource<TResult> t = new TaskCompletionSource<TResult>();

            AddDisposeAction(
                () =>
                {
                    closingCallback?.Invoke();
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
            get => _ResultLocator(this).Value;
            set => _ResultLocator(this).SetValueAndTryNotify(value);
        }



        #region Property TResult Result Setup
        /// <summary>
        /// The _ result
        /// </summary>
        protected Property<TResult> _Result =
          new Property<TResult>(_ResultLocator);
        /// <summary>
        /// The _ result locator
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private static Func<BindableBase, ValueContainer<TResult>> _ResultLocator =
            RegisterContainerLocator<TResult>(
                "Result",
                model =>
                {
                    model._Result =
                        model._Result
                        ??
                        new Property<TResult>(_ResultLocator);
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
        private IDisposeGroup _UnbindDisposeGroup = new DisposeGroup();
        private IDisposeGroup _UnloadDisposeGroup = new DisposeGroup();
        /// <summary>
        /// Resource Group that need dispose when Unbind from UI;
        /// </summary>

        public IDisposeGroup UnbindDisposeGroup => _UnbindDisposeGroup;

        /// <summary>
        /// Resource Group that need dispose when Unload from UI;
        /// </summary>											   

        public IDisposeGroup UnloadDisposeGroup => _UnloadDisposeGroup;


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

            IsDisposingWhenUnloadRequired = false;
            IsDisposingWhenUnbindRequired = false;


            GetValueContainer(x => x.UIBusyTaskCount)
                .GetValueChangedEventObservable()
                .Select(e =>
                    e.EventArgs.NewValue != 0)
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
            foreach (string item in GetFieldNames())
            {
                RaisePropertyChanged(new PropertyChangedEventArgs(item));
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
         
            InitStageManager(view);

            await Task.Yield();
        }

        private void InitStageManager(IView view)
        {
            if (view != null)
            {
                StageManager = Services.ServiceLocator.Instance.Resolve<IStageManager>();
                StageManager.CurrentBindingView = view;
                StageManager.ViewModel = this;
                StageManager.InitParent(()
                    => view.Parent);
            }
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
                await Task.Yield();
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
            InitStageManager(view);
            await Task.Yield();
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
                    Dispose();
                }
                await Task.Yield();
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
        public bool IsDisposingWhenUnloadRequired { get;  set; }




        /// <summary>
        /// The _ stage manager
        /// </summary>
        private MVVMSidekick.Views.IStageManager _StageManager = new EmptyStageManager();

        /// <summary>
        /// Gets or sets the stage manager.												 I
        /// </summary>
        /// <value>The stage manager.</value>
        //[Microsoft.Practices.Unity.Dependency(Testing.Constants.DependencyKeyForTesting)]
        public MVVMSidekick.Views.IStageManager StageManager
        {
            get => _StageManager;
            set => _StageManager = value;
        }

        /// <summary>
        /// 是否有返回值
        /// </summary>
        /// <value><c>true</c> if [have return value]; otherwise, <c>false</c>.</value>
        public virtual bool HaveReturnValue => false;
        /// <summary>
        /// 本UI是否处于忙状态
        /// </summary>
        /// <value><c>true</c> if this instance is UI busy; otherwise, <c>false</c>.</value>

        public bool IsUIBusy
        {
            get => _IsUIBusyLocator(this).Value;
            set => _IsUIBusyLocator(this).SetValueAndTryNotify(value);
        }
        #region Property bool IsUIBusy Setup
        /// <summary>
        /// The _ is UI busy
        /// </summary>
        protected Property<bool> _IsUIBusy = new Property<bool>(_IsUIBusyLocator);

        /// <summary>
        /// The _ is UI busy locator
        /// </summary>
        private static Func<BindableBase, ValueContainer<bool>> _IsUIBusyLocator = RegisterContainerLocator<bool>("IsUIBusy", model => model.Initialize("IsUIBusy", ref model._IsUIBusy, ref _IsUIBusyLocator, _IsUIBusyDefaultValueFactory));

        /// <summary>
        /// The _ is UI busy default value factory
        /// </summary>
        private static Func<bool> _IsUIBusyDefaultValueFactory = null;
        #endregion



        /// <summary>
        /// Gets or sets the UI busy task count.
        /// </summary>
        /// <value>The UI busy task count.</value>
        private int UIBusyTaskCount
        {
            get => _UIBusyTaskCountLocator(this).Value;
            set => _UIBusyTaskCountLocator(this).SetValueAndTryNotify(value);
        }
        #region Property int UIBusyTaskCount Setup
        private Property<int> _UIBusyTaskCount = new Property<int>(_UIBusyTaskCountLocator);
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
            TaskCompletionSource<object> t = new TaskCompletionSource<object>();

            UnloadDisposeGroup.AddDisposeAction(
                () =>
                {
                    closingCallback?.Invoke();
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
            StageManager?.CurrentBindingView?.SelfClose();
            Dispose();
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
        public virtual async Task<Tout> ExecuteFunctionTask<Tin, Tout>(Func<Tin, CancellationToken, Task<Tout>> taskBody, Tin inputContext, CancellationToken cancellationToken, bool UIBusyWhenExecuting = true)
        {

            CancellationTokenSource tempCSource;

            EventPattern<EventCommandEventArgs> cmdarh = inputContext as EventPattern<EventCommandEventArgs>;
            if (cmdarh != null)
            {
                tempCSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cmdarh.EventArgs.Cancellation.Token);
                Func<Tin, CancellationToken, Task<Tout>> oldBody = taskBody;
                taskBody = async (i, c) =>
                {
                    try
                    {

                        if (c.IsCancellationRequested)
                        {
                            cmdarh.EventArgs.Completion.TrySetCanceled();
                            return default(Tout);
                        }

                        Tout rval = await oldBody(i, c);

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
            else
            {
                tempCSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                Func<Tin, CancellationToken, Task<Tout>> oldBody = taskBody;
                taskBody = async (i, c) =>
                {
                    try
                    {

                        if (c.IsCancellationRequested)
                        {
                            return default(Tout);
                        }

                        Tout rval = await oldBody(i, c);


                        return rval;
                    }
                    catch (Exception ex)
                    {
                        EventRouter.Instance.RaiseEvent(this, ex);
                        throw;
                    }

                };
            }
            //Add Executing and executed events
            {
                Func<Tin, CancellationToken, Task<Tout>> oldBody = taskBody;
                taskBody = async (i, c) =>
                {
                    (Tin InputContext, CancellationTokenSource CancellationSource) TaskExecuting = (inputContext, tempCSource);

                    LocalEventRouter.RaiseEvent(this, TaskExecuting, nameof(TaskExecuting));
                    await Task.Yield();

                    if (tempCSource.IsCancellationRequested)
                    {
                        return default(Tout);
                    }

                    GlobalEventRouter.RaiseEvent(this, TaskExecuting, nameof(TaskExecuting));
                    await Task.Yield();

                    if (tempCSource.IsCancellationRequested)
                    {
                        return default(Tout);
                    }

                    Task<Tout> valueTask = oldBody(inputContext, tempCSource.Token);
                    Tout value = await valueTask;

                    (Tin InputContext, Task Task) TaskExecuted = (InputContext: inputContext, Task: valueTask as Task);

                    LocalEventRouter.RaiseEvent(this, TaskExecuted, nameof(TaskExecuted));
                    await Task.Yield();

                    GlobalEventRouter.RaiseEvent(this, TaskExecuted, nameof(TaskExecuted));
                    await Task.Yield();

                    return value;

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

            await ExecuteFunctionTask<Tin, object>(async (i, c) => { await taskBody(i, c); return null; }, inputContext, cancellationToken, UIBusyWhenExecuting);
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
            await ExecuteFunctionTask<Tin, object>(async (i, c) => { await taskBody(i); return null; }, inputContext, CancellationToken.None, UIBusyWhenExecuting);

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
        public virtual async Task<Tout> ExecuteCaculation<Tout>(Func<Task<Tout>> taskBody, bool UIBusyWhenExecuting = true)
        {
            return await ExecuteFunctionTask<object, Tout>(
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
            await ExecuteFunctionTask<object, object>(
                async (i, c) =>
                {
                    await taskBody();
                    return null;
                },
                null,
                CancellationToken.None, UIBusyWhenExecuting);

        }


    }


}
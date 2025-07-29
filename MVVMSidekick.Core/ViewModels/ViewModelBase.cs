
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
    using Microsoft.Extensions.DependencyInjection;
    using MVVMSidekick.Common;
    using MVVMSidekick.Services;
    using System.Reactive;
    using System.Reactive.Disposables;
    using Utilities;
    using Views;

    /// <summary>
    /// 带有返回值的视图模型基类
    /// View model base class with return value
    /// </summary>
    /// <typeparam name="TViewModel">视图模型类型 / The type of the view model</typeparam>
    /// <typeparam name="TResult">返回值类型 / The type of the result</typeparam>
    public abstract partial class ViewModelBase<TViewModel, TResult> : ViewModelBase<TViewModel>, IViewModel<TResult>
        where TViewModel : ViewModelBase<TViewModel, TResult>, IViewModel<TResult>
    {
        /// <summary>
        /// 释放非托管资源和（可选的）托管资源
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">如果为 true，则释放托管和非托管资源；如果为 false，则仅释放非托管资源 / true to release both managed and unmanaged resources; false to release only unmanaged resources</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// 获取一个值，指示是否有返回值
        /// Gets a value indicating whether there is a return value
        /// </summary>
        /// <value>如果有返回值则为 true，否则为 false / true if there is a return value; otherwise, false</value>
        public override bool HaveReturnValue => true;

        /// <summary>
        /// 等待关闭并返回结果
        /// Waits for close and returns the result
        /// </summary>
        /// <param name="closingCallback">关闭回调 / The closing callback</param>
        /// <returns>结果任务 / The result task</returns>
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
        /// 获取或设置结果
        /// Gets or sets the result
        /// </summary>
        /// <value>结果值 / The result value</value>
        public TResult Result
        {
            get => _ResultLocator(this).Value;
            set => _ResultLocator(this).SetValueAndTryNotify(value);
        }

        #region Property TResult Result Setup
        /// <summary>
        /// 结果属性的内部字段
        /// Internal field for the result property
        /// </summary>
        protected Property<TResult> _Result =
          new Property<TResult>(_ResultLocator);
        
        /// <summary>
        /// 结果属性的定位器
        /// Locator for the result property
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
    /// 带有界面特性的视图模型基类
    /// View model base class with UI characteristics
    /// </summary>
    /// <typeparam name="TViewModel">视图模型本身的类型 / The type of the view model itself</typeparam>
    [DataContract]
    public abstract partial class ViewModelBase<TViewModel> : BindableBase<TViewModel>, IViewModel where TViewModel : ViewModelBase<TViewModel>
    {
        /// <summary>
        /// 解绑时需要释放的资源组的私有字段
        /// Private field for resource group that needs disposal when unbinding
        /// </summary>
        private IDisposeGroup _UnbindDisposeGroup = new DisposeGroup();
        
        /// <summary>
        /// 卸载时需要释放的资源组的私有字段
        /// Private field for resource group that needs disposal when unloading
        /// </summary>
        private IDisposeGroup _UnloadDisposeGroup = new DisposeGroup();
        
        /// <summary>
        /// 从UI解绑时需要释放的资源组
        /// Resource group that needs disposal when unbinding from UI
        /// </summary>
        public IDisposeGroup UnbindDisposeGroup => _UnbindDisposeGroup;

        /// <summary>
        /// 从UI卸载时需要释放的资源组
        /// Resource group that needs disposal when unloading from UI
        /// </summary>											   
        public IDisposeGroup UnloadDisposeGroup => _UnloadDisposeGroup;

        /// <summary>
        /// 释放非托管资源和（可选的）托管资源
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">如果为 true，则释放托管和非托管资源；如果为 false，则仅释放非托管资源 / true to release both managed and unmanaged resources; false to release only unmanaged resources</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// 初始化 ViewModelBase 类的新实例
        /// Initializes a new instance of the ViewModelBase class
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
        /// 当绑定到视图时调用
        /// Called when binding to view
        /// </summary>
        /// <param name="view">视图实例 / The view instance</param>
        /// <param name="oldValue">原视图模型值 / The old view model value</param>
        /// <returns>异步任务 / Async task</returns>
        Task IViewModelLifetime.OnBindedToView(IView view, IViewModel oldValue)
        {
            return OnBindedToView(view, oldValue);
        }

        /// <summary>
        /// 当从视图解绑时调用
        /// Called when unbinding from view
        /// </summary>
        /// <param name="view">视图实例 / The view instance</param>
        /// <param name="newValue">新视图模型值 / The new view model value</param>
        /// <returns>异步任务 / Async task</returns>
        Task IViewModelLifetime.OnUnbindedFromView(IView view, IViewModel newValue)
        {
            return OnUnbindedFromView(view, newValue);
        }

        /// <summary>
        /// 当绑定的视图加载时调用
        /// Called when the bound view loads
        /// </summary>
        /// <param name="view">视图实例 / The view instance</param>
        /// <returns>异步任务 / Async task</returns>
        Task IViewModelLifetime.OnBindedViewLoad(IView view)
        {
            foreach (string item in GetFieldNames())
            {
                RaisePropertyChanged(new PropertyChangedEventArgs(item));
            }
            return OnBindedViewLoad(view);
        }
        
        /// <summary>
        /// 当绑定的视图卸载时调用
        /// Called when the bound view unloads
        /// </summary>
        /// <param name="view">视图实例 / The view instance</param>
        /// <returns>异步任务 / Async task</returns>
        Task IViewModelLifetime.OnBindedViewUnload(IView view)
        {
            return OnBindedViewUnload(view);
        }

        /// <summary>
        /// 当此视图模型被设置到视图的ViewModel属性时，将由视图调用
        /// This will be invoked by view when this viewmodel is set to view's ViewModel property
        /// </summary>
        /// <param name="view">设置目标视图 / Set target view</param>
        /// <param name="oldValue">设置前的值 / Value before set</param>
        /// <returns>任务等待器 / Task awaiter</returns>
        protected virtual async Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
        {
            InitStageManager(view);
            await Task.Yield();
        }

        /// <summary>
        /// 初始化舞台管理器
        /// Initializes the stage manager
        /// </summary>
        /// <param name="view">视图实例 / View instance</param>
        private void InitStageManager(IView view)
        {
            if (view != null)
            {
                StageManager = ServiceProviderLocator.RootServiceProvider?.GetService<IStageManager>();
                StageManager.CurrentBindingView = view;
                StageManager.ViewModel = this;
                StageManager.InitParent(()
                    => view.Parent);
            }
        }

        /// <summary>
        /// 当ViewModel属性中的此视图模型实例被覆盖时，将由视图调用
        /// This will be invoked by view when this instance of viewmodel in ViewModel property is overwritten
        /// </summary>
        /// <param name="view">覆盖目标视图 / Overwrite target view</param>
        /// <param name="newValue">替换的值 / The value replacing</param>
        /// <returns>任务等待器 / Task awaiter</returns>
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
        /// 当视图触发Load事件且此视图模型实例已在视图的ViewModel属性中时，将由视图调用
        /// This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property
        /// </summary>
        /// <param name="view">触发Load事件的视图 / View that firing Load event</param>
        /// <returns>任务等待器 / Task awaiter</returns>
        protected virtual async Task OnBindedViewLoad(IView view)
        {
            InitStageManager(view);
            await Task.Yield();
        }

        /// <summary>
        /// 当视图触发Unload事件且此视图模型实例仍在视图的ViewModel属性中时，将由视图调用
        /// This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's ViewModel property
        /// </summary>
        /// <param name="view">触发Unload事件的视图 / View that firing Unload event</param>
        /// <returns>任务等待器 / Task awaiter</returns>
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
        /// 设置：当从视图解绑时是否释放VM
        /// Set: Will VM be disposed when unbind from View
        /// </summary>
        /// <value>如果此实例在解绑时需要释放则为 true，否则为 false / true if this instance is disposing when unbind required; otherwise, false</value>
        public bool IsDisposingWhenUnbindRequired { get; set; }

        /// <summary>
        /// 设置：当从视图卸载时是否释放VM
        /// Set: Will VM be disposed when unload from View
        /// </summary>
        /// <value>如果此实例在卸载时需要释放则为 true，否则为 false / true if this instance is disposing when unload required; otherwise, false</value>
        public bool IsDisposingWhenUnloadRequired { get; set; }

        /// <summary>
        /// 获取或设置舞台管理器
        /// Gets or sets the stage manager
        /// </summary>
        /// <value>舞台管理器 / The stage manager</value>
        //[Microsoft.Practices.Unity.Dependency(Testing.Constants.DependencyKeyForTesting)]
        public virtual MVVMSidekick.Views.IStageManager StageManager { get; set; } = new EmptyStageManager();

        /// <summary>
        /// 获取一个值，指示是否有返回值
        /// Gets a value indicating whether there is a return value
        /// </summary>
        /// <value>如果有返回值则为 true，否则为 false / true if there is a return value; otherwise, false</value>
        public virtual bool HaveReturnValue => false;

        /// <summary>
        /// 获取或设置此UI是否处于忙状态
        /// Gets or sets whether this UI is in a busy state
        /// </summary>
        /// <value>如果此实例的UI处于忙状态则为 true，否则为 false / true if this instance's UI is busy; otherwise, false</value>
        public bool IsUIBusy { get => _IsUIBusyLocator(this).Value; set => _IsUIBusyLocator(this).SetValueAndTryNotify(value); }
        #region Property bool IsUIBusy Setup
        /// <summary>
        /// <para>IsUIBusy 属性的内部 Property 实例</para>
        /// <para>Internal Property instance for IsUIBusy property</para>
        /// </summary>        
        protected Property<bool> _IsUIBusy = new Property<bool>(_IsUIBusyLocator);
        /// <summary>
        /// <para>IsUIBusy 属性的值容器定位器</para>
        /// <para>Value container locator for IsUIBusy property</para>
        /// </summary>
        static Func<BindableBase, ValueContainer<bool>> _IsUIBusyLocator = RegisterContainerLocator(nameof(IsUIBusy), m => m.Initialize(nameof(IsUIBusy), ref m._IsUIBusy, ref _IsUIBusyLocator, () => default(bool)));
        #endregion

        /// <summary>
        /// 获取或设置UI忙状态任务计数
        /// Gets or sets the UI busy task count
        /// </summary>
        /// <value>UI忙状态任务计数 / The UI busy task count</value>
        public int UIBusyTaskCount { get => _UIBusyTaskCountLocator(this).Value; set => _UIBusyTaskCountLocator(this).SetValueAndTryNotify(value); }
        #region Property int UIBusyTaskCount Setup
        /// <summary>
        /// <para>UIBusyTaskCount 属性的内部 Property 实例</para>
        /// <para>Internal Property instance for UIBusyTaskCount property</para>
        /// </summary>        
        protected Property<int> _UIBusyTaskCount = new Property<int>(_UIBusyTaskCountLocator);
        /// <summary>
        /// <para>UIBusyTaskCount 属性的值容器定位器</para>
        /// <para>Value container locator for UIBusyTaskCount property</para>
        /// </summary>
        static Func<BindableBase, ValueContainer<int>> _UIBusyTaskCountLocator = RegisterContainerLocator(nameof(UIBusyTaskCount), m => m.Initialize(nameof(UIBusyTaskCount), ref m._UIBusyTaskCount, ref _UIBusyTaskCountLocator, () => default(int)));
        #endregion

        /// <summary>
        /// 等待关闭
        /// Waits for close
        /// </summary>
        /// <param name="closingCallback">关闭回调 / The closing callback</param>
        /// <returns>异步任务 / Async task</returns>
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
        /// 关闭视图并释放资源
        /// Closes the view and dispose
        /// </summary>
        public void CloseViewAndDispose()
        {
            StageManager?.CurrentBindingView?.SelfClose();
            Dispose();
        }





        /// <summary>
        /// 执行函数任务
        /// Executes the function task
        /// </summary>
        /// <typeparam name="Tin">输入参数类型 / The type of the input parameter</typeparam>
        /// <typeparam name="Tout">输出结果类型 / The type of the output result</typeparam>
        /// <param name="taskBody">任务主体 / The task body</param>
        /// <param name="inputContext">输入上下文 / The input context</param>
        /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
        /// <param name="UIBusyWhenExecuting">执行时是否设置UI为忙状态 / if set to true UI busy when executing</param>
        /// <returns>异步任务结果 / Task result</returns>
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
            
            //添加执行中和已执行事件
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
        /// 执行任务（无返回值）
        /// Executes the task (no return value)
        /// </summary>
        /// <typeparam name="Tin">输入参数类型 / The type of the input parameter</typeparam>
        /// <param name="taskBody">任务主体 / The task body</param>
        /// <param name="inputContext">输入上下文 / The input context</param>
        /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
        /// <param name="UIBusyWhenExecuting">执行时是否设置UI为忙状态 / if set to true UI busy when executing</param>
        /// <returns>异步任务 / Async task</returns>
        public virtual async Task ExecuteTask<Tin>(Func<Tin, CancellationToken, Task> taskBody, Tin inputContext, CancellationToken cancellationToken, bool UIBusyWhenExecuting = true)
        {
            await ExecuteFunctionTask<Tin, object>(async (i, c) => { await taskBody(i, c); return null; }, inputContext, cancellationToken, UIBusyWhenExecuting);
        }

        /// <summary>
        /// 执行任务（简化版本，无取消令牌）
        /// Executes the task (simplified version, no cancellation token)
        /// </summary>
        /// <typeparam name="Tin">输入参数类型 / The type of the input parameter</typeparam>
        /// <param name="taskBody">任务主体 / The task body</param>
        /// <param name="inputContext">输入上下文 / The input context</param>
        /// <param name="UIBusyWhenExecuting">执行时是否设置UI为忙状态 / if set to true UI busy when executing</param>
        /// <returns>异步任务 / Async task</returns>
        public virtual async Task ExecuteTask<Tin>(Func<Tin, Task> taskBody, Tin inputContext, bool UIBusyWhenExecuting = true)
        {
            await ExecuteFunctionTask<Tin, object>(async (i, c) => { await taskBody(i); return null; }, inputContext, CancellationToken.None, UIBusyWhenExecuting);
        }

        /// <summary>
        /// 执行计算任务
        /// Executes the calculation task
        /// </summary>
        /// <typeparam name="Tout">输出结果类型 / The type of the output result</typeparam>
        /// <param name="taskBody">任务主体 / The task body</param>
        /// <param name="UIBusyWhenExecuting">执行时是否设置UI为忙状态 / if set to true UI busy when executing</param>
        /// <returns>异步任务结果 / Async task result</returns>
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
        /// 执行任务（最简化版本）
        /// Executes the task (most simplified version)
        /// </summary>
        /// <param name="taskBody">任务主体 / The task body</param>
        /// <param name="UIBusyWhenExecuting">执行时是否设置UI为忙状态 / if set to true UI busy when executing</param>
        /// <returns>异步任务 / Async task</returns>
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
using Microsoft.AspNetCore.Components;
using MVVMSidekick.Common;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSidekick.Views
{

    /// <summary>
    /// MVVMSidekick组件基类，提供视图和视图模型之间的绑定功能
    /// MVVMSidekick component base class that provides binding functionality between view and view model
    /// </summary>
    /// <typeparam name="TView">视图类型 / View type</typeparam>
    /// <typeparam name="TViewModel">视图模型类型 / View model type</typeparam>
    public class MVVMSidekickComponentBase<TView, TViewModel> : ComponentBase, IDisposable, IAsyncDisposable, IDisposeGroup
        where TView : MVVMSidekickComponentBase<TView, TViewModel>
        where TViewModel : ViewModel<TViewModel, TView>
    {

        /// <summary>
        /// 参数设置器列表，用于自动映射视图参数到视图模型属性
        /// Parameter setters list for automatically mapping view parameters to view model properties
        /// </summary>
        private static IList<Action<TView, TViewModel>> parameterSetters = typeof(TView).GetProperties()
                .Select(x =>
                    (Property: x,
                    Attribute: x.GetCustomAttribute(typeof(ModelMappingAttribute), true) ?? x.GetCustomAttribute(typeof(ParameterAttribute), true)?? x.GetCustomAttribute(typeof(CascadingParameterAttribute), true)))
                .Where(x => x.Attribute != null)
                .Select(x => (x.Property, Attribute: (x.Attribute as ModelMappingAttribute)))
                .Where(x => !(x.Attribute?.Ignore ?? false))
                .Select(x => (SourceProperty: x.Property, TargetProperty: typeof(TViewModel).GetProperty(x.Attribute?.MapToProperty ?? x.Property.Name)))
                .Where(x => x.TargetProperty != null)
                //.Where(x => x.TargetProperty.PropertyType.IsAssignableFrom(x.SourceProperty.PropertyType))
                .Select(x =>
                {
                    var expPS = Expression.Parameter(x.SourceProperty.DeclaringType);
                    var expPSMA = Expression.MakeMemberAccess(expPS, x.SourceProperty);
                    var expPT = Expression.Parameter(x.TargetProperty.DeclaringType);
                    var expPTMA = Expression.MakeMemberAccess(expPT, x.TargetProperty);
                    var expAssign = Expression.Assign(expPTMA, Expression.Convert(expPSMA, x.TargetProperty.PropertyType));
                    var expLambda = Expression.Lambda<Action<TView, TViewModel>>(expAssign, expPS, expPT);
                    return expLambda.Compile();
                })
                .ToList();

        /// <summary>
        /// 释放条目正在释放时触发的事件
        /// Event triggered when dispose entry is disposing
        /// </summary>
        public event EventHandler<DisposeEventArgs> DisposeEntryDisposing;
        
        /// <summary>
        /// 释放条目已释放时触发的事件
        /// Event triggered when dispose entry is disposed
        /// </summary>
        public event EventHandler<DisposeEventArgs> DisposeEntryDisposed;

        /// <summary>
        /// 初始化MVVMSidekickComponentBase的新实例
        /// Initializes a new instance of MVVMSidekickComponentBase
        /// </summary>
        public MVVMSidekickComponentBase()
        {
        }

        /// <summary>
        /// 获取或设置注入的视图模型实例
        /// Gets or sets the injected view model instance
        /// </summary>
        [Inject]
        public TViewModel ViewModel { get => viewModel; set => viewModel = value; }

        /// <summary>
        /// 视图模型的快捷方式属性
        /// Shortcut property for ViewModel
        /// </summary>
        protected TViewModel M { get => viewModel; }

        /// <summary>
        /// 获取释放信息列表，暂未实现
        /// Gets the dispose information list, not implemented yet
        /// </summary>
        public IList<DisposeEntry> DisposeInfoList => throw new NotImplementedException();

        /// <summary>
        /// 异步设置参数，并建立视图与视图模型的绑定关系
        /// Sets parameters asynchronously and establishes binding between view and view model
        /// </summary>
        /// <param name="parameters">参数视图 / Parameter view</param>
        /// <returns>表示异步操作的任务 / Task representing the asynchronous operation</returns>
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
            if (M == null)
            {
                throw new InvalidOperationException("View Model is null, please make sure a instance was injected or passed as constructor paramerter");
            }
            M.Page = this as TView;

            foreach (var setter in parameterSetters)
            {
                setter(M.Page, M);
            }

        }

        /// <summary>
        /// 参数设置完成时调用，同时通知视图模型
        /// Called when parameters are set, also notifies the view model
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            ViewModel?.OnParametersSet();
        }

        /// <summary>
        /// 异步参数设置完成时调用，同时通知视图模型
        /// Called when parameters are set asynchronously, also notifies the view model
        /// </summary>
        /// <returns>表示异步操作的任务 / Task representing the asynchronous operation</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await ViewModel?.OnParametersSetAsync();
        }
        
        /// <summary>
        /// 组件初始化时调用，设置属性变更通知
        /// Called when component is initialized, sets up property change notification
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            ViewModel?.OnInitialized();
            if (ViewModel != null)
            {
                ViewModel.PropertyChanged += (o, a) => StateHasChanged();

            }
        }
        
        /// <summary>
        /// 异步组件初始化时调用
        /// Called when component is initialized asynchronously
        /// </summary>
        /// <returns>表示异步操作的任务 / Task representing the asynchronous operation</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await ViewModel?.OnInitializedAsync();
        }
        
        /// <summary>
        /// 渲染完成后调用
        /// Called after rendering is completed
        /// </summary>
        /// <param name="firstRender">是否首次渲染 / Whether this is the first render</param>
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            ViewModel?.OnAfterRender(firstRender);
        }

        /// <summary>
        /// 异步渲染完成后调用
        /// Called after rendering is completed asynchronously
        /// </summary>
        /// <param name="firstRender">是否首次渲染 / Whether this is the first render</param>
        /// <returns>表示异步操作的任务 / Task representing the asynchronous operation</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await ViewModel?.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// 请求重新渲染组件
        /// Requests component to re-render
        /// </summary>
        public void RequestRerender()
        {
            base.StateHasChanged();
        }

        /// <summary>
        /// 释放组件资源
        /// Disposes component resources
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            //Dispose(disposing: true);
            dg.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 异步释放组件资源
        /// Disposes component resources asynchronously
        /// </summary>
        /// <returns>表示异步释放操作的ValueTask / ValueTask representing the asynchronous dispose operation</returns>
        public virtual ValueTask DisposeAsync()
        {
            return default;
        }

        /// <summary>
        /// 添加可释放的对象到释放组
        /// Adds disposable object to dispose group
        /// </summary>
        /// <param name="item">可释放对象 / Disposable object</param>
        /// <param name="needCheckInFinalizer">是否需要在终结器中检查 / Whether need check in finalizer</param>
        /// <param name="comment">注释 / Comment</param>
        /// <param name="member">成员名称 / Member name</param>
        /// <param name="file">文件名 / File name</param>
        /// <param name="line">行号 / Line number</param>
        public void AddDisposable(IDisposable item, bool needCheckInFinalizer = false, string comment = "", string member = "", string file = "", int line = -1)
        {
            dg.AddDisposable(item, needCheckInFinalizer, comment, member, file, line);
        }

        /// <summary>
        /// 添加释放动作到释放组
        /// Adds dispose action to dispose group
        /// </summary>
        /// <param name="action">释放动作 / Dispose action</param>
        /// <param name="needCheckInFinalizer">是否需要在终结器中检查 / Whether need check in finalizer</param>
        /// <param name="comment">注释 / Comment</param>
        /// <param name="member">成员名称 / Member name</param>
        /// <param name="file">文件名 / File name</param>
        /// <param name="line">行号 / Line number</param>
        public void AddDisposeAction(Action action, bool needCheckInFinalizer = false, string comment = "", string member = "", string file = "", int line = -1)
        {
            dg.AddDisposeAction(action, needCheckInFinalizer, comment, member, file, line);
        }

        /// <summary>
        /// 释放组实例，用于管理资源释放
        /// Dispose group instance for managing resource disposal
        /// </summary>
        private IDisposeGroup dg = new DisposeGroup();
        
        /// <summary>
        /// 视图模型实例
        /// View model instance
        /// </summary>
        private TViewModel viewModel;
    }
}

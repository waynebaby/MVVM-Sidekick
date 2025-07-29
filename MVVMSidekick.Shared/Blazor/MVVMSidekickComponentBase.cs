#if BLAZOR

using Microsoft.AspNetCore.Components;
using MVVMSidekick.Common;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Reactive;
using MVVMSidekick.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.ComponentModel;
namespace MVVMSidekick.Views
{

    /// <summary>
    /// <para>MVVMSidekick组件基类，为Blazor组件提供MVVM支持</para>
    /// <para>MVVMSidekick component base class, providing MVVM support for Blazor components</para>
    /// </summary>
    /// <typeparam name="TView">视图类型 / View type</typeparam>
    /// <typeparam name="TViewModel">视图模型类型 / View model type</typeparam>
    public class MVVMSidekickComponentBase<TView, TViewModel> : ComponentBase, IDisposable, IAsyncDisposable, IDisposeGroup
        where TView : MVVMSidekickComponentBase<TView, TViewModel>
        where TViewModel : ViewModel<TViewModel, TView>
    {

        /// <summary>
        /// <para>参数设置器列表，用于自动映射视图参数到视图模型属性</para>
        /// <para>Parameter setters list for automatically mapping view parameters to view model properties</para>
        /// </summary>
        private static IList<Action<TView, TViewModel>> parameterSetters = typeof(TView).GetProperties()
                .Select(x =>
                    (Property: x,
                    Attribute: x.GetCustomAttribute(typeof(ModelMappingAttribute), true) ?? x.GetCustomAttribute(typeof(ParameterAttribute), true) ?? x.GetCustomAttribute(typeof(CascadingParameterAttribute), true)))
                .Where(x => x.Attribute != null)
                .Select(x => (x.Property, Attribute: (x.Attribute as ModelMappingAttribute)))
                .Where(x => !(x.Attribute?.Ignore ?? false))
                .Select(x => (SourceProperty: x.Property, TargetProperty: typeof(TViewModel).GetProperty(x.Attribute?.MapToProperty ?? x.Property.Name)))
                .Where(x => x.TargetProperty != null)
                //.Where(x => x.TargetProperty.PropertyType.IsAssignableFrom(x.SourceProperty.PropertyType))
                .Select(x =>
                {
                    var expPS = System.Linq.Expressions.  Expression.Parameter(x.SourceProperty.DeclaringType);
                    var expPSMA = System.Linq.Expressions.Expression.MakeMemberAccess(expPS, x.SourceProperty);
                    var expPT = System.Linq.Expressions.Expression.Parameter(x.TargetProperty.DeclaringType);
                    var expPTMA = System.Linq.Expressions.Expression.MakeMemberAccess(expPT, x.TargetProperty);
                    var expAssign = System.Linq.Expressions.Expression.Assign(expPTMA, System.Linq.Expressions.Expression.Convert(expPSMA, x.TargetProperty.PropertyType));
                    var expLambda = System.Linq.Expressions.Expression.Lambda<Action<TView, TViewModel>>(expAssign, expPS, expPT);
                    return expLambda.Compile();
                })
                .ToList();

        /// <summary>
        /// <para>销毁条目正在销毁事件</para>
        /// <para>Dispose entry disposing event</para>
        /// </summary>
        public event EventHandler<DisposeEventArgs> DisposeEntryDisposing;
        
        /// <summary>
        /// <para>销毁条目已销毁事件</para>
        /// <para>Dispose entry disposed event</para>
        /// </summary>
        public event EventHandler<DisposeEventArgs> DisposeEntryDisposed;

        /// <summary>
        /// <para>初始化MVVMSidekickComponentBase类的新实例</para>
        /// <para>Initializes a new instance of the MVVMSidekickComponentBase class</para>
        /// </summary>
        public MVVMSidekickComponentBase()
        {
        }

        /// <summary>
        /// <para>获取或设置视图模型</para>
        /// <para>Gets or sets the view model</para>
        /// </summary>
        [Inject]
        [DefaultValue(null)]
        public TViewModel ViewModel { get => viewModel; set => viewModel = value; }

        /// <summary>
        /// <para>视图模型的快捷方式</para>
        /// <para>Shortcut for ViewModel</para>
        /// </summary>
        protected TViewModel M { get => viewModel; }

        /// <summary>
        /// <para>获取销毁信息列表</para>
        /// <para>Gets the dispose info list</para>
        /// </summary>
        public IList<DisposeEntry> DisposeInfoList => throw new NotImplementedException();






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

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            ViewModel?.OnParametersSet();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await ViewModel?.OnParametersSetAsync();
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (ViewModel != null)
            {
                ViewModel.OnInitialized();
                //ViewModel.PropertyChanged += (o, a) => StateHasChanged();
                ViewModel.CreatePropertyChangedObservable()
                    .Throttle(TimeSpan.FromSeconds(1d / 60))
                    .Subscribe(async _=> await InvokeAsync(()=> StateHasChanged()))
                    .DisposeWith(ViewModel);
            }

        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await ViewModel?.OnInitializedAsync();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            ViewModel?.OnAfterRender(firstRender);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await ViewModel?.OnAfterRenderAsync(firstRender);
        }


        public void RequestRerender()
        {
            base.StateHasChanged();
        }



        ////private bool disposedValue;
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!disposedValue)
        //    {
        //        if (disposing)
        //        {
        //            // TODO: dispose managed state (managed objects)
        //        }

        //        // TODO: free unmanaged resources (unmanaged objects) and override finalizer
        //        // TODO: set large fields to null
        //        disposedValue = true;
        //    }

        //}
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            //Dispose(disposing: true);
            dg.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual ValueTask DisposeAsync()
        {
            return default;
        }

        public void AddDisposable(IDisposable item, bool needCheckInFinalizer = false, string comment = "", string member = "", string file = "", int line = -1)
        {
            dg.AddDisposable(item, needCheckInFinalizer, comment, member, file, line);
        }

        public void AddDisposeAction(Action action, bool needCheckInFinalizer = false, string comment = "", string member = "", string file = "", int line = -1)
        {
            dg.AddDisposeAction(action, needCheckInFinalizer, comment, member, file, line);
        }

        private IDisposeGroup dg = new DisposeGroup();
        private TViewModel viewModel;
    }
}
#endif
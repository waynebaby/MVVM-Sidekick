using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using MVVMSidekick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MVVMSidekickUWPDemo.ViewModels
{

    /// <summary>
    /// 计数器视图模型，提供计数功能的演示
    /// Counter view model providing counter functionality demonstration
    /// </summary>
    [DataContract]
    public class Counter_Model : ViewModel<Counter_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property

        /// <summary>
        /// 初始化Counter_Model的新实例
        /// Initializes a new instance of Counter_Model
        /// </summary>
        public Counter_Model()
        {
        }
        
        /// <summary>
        /// 使用服务提供程序初始化Counter_Model的新实例
        /// Initializes a new instance of Counter_Model with service provider
        /// </summary>
        /// <param name="serviceProvider">服务提供程序 / Service provider</param>
        public Counter_Model(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        
        /// <summary>
        /// 获取服务提供程序
        /// Gets the service provider
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 获取或设置当前计数值
        /// Gets or sets the current count value
        /// </summary>
        public int CurrentCount { get => _CurrentCountLocator(this).Value; set => _CurrentCountLocator(this).SetValueAndTryNotify(value); }
        
        #region Property int CurrentCount Setup        
        /// <summary>
        /// CurrentCount属性的内部存储和定位器设置
        /// Internal storage and locator setup for CurrentCount property
        /// </summary>
        protected Property<int> _CurrentCount = new Property<int>(_CurrentCountLocator);
        static Func<BindableBase, ValueContainer<int>> _CurrentCountLocator = RegisterContainerLocator(nameof(CurrentCount), m => m.Initialize(nameof(CurrentCount), ref m._CurrentCount, ref _CurrentCountLocator, () => default(int)));
        #endregion

        /// <summary>
        /// 获取递增计数命令模型
        /// Gets the increment count command model
        /// </summary>
        public CommandModel CommandIncrementCount => _CommandIncrementCountLocator(this).Value;
        
        #region Property CommandModel CommandIncrementCount Setup                
        /// <summary>
        /// CommandIncrementCount命令属性的内部存储和定位器设置，包含递增计数的业务逻辑
        /// Internal storage and locator setup for CommandIncrementCount command property, containing increment count business logic
        /// </summary>
        protected Property<CommandModel> _CommandIncrementCount = new Property<CommandModel>(_CommandIncrementCountLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandIncrementCountLocator = RegisterContainerLocator(nameof(CommandIncrementCount), m => m.Initialize(nameof(CommandIncrementCount), ref m._CommandIncrementCount, ref _CommandIncrementCountLocator,
              model =>
              {
                  object state = nameof(CommandIncrementCount);
                  var commandId = nameof(CommandIncrementCount);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {
                              vm.CurrentCount++;
                              await Task.CompletedTask;
                          })
                      .Subscribe()
                      .DisposeWith(vm);

                  var cmdmdl = cmd.CreateCommandModel(state);

                  return cmdmdl;
              }));
        #endregion


        #region Life Time Event Handling

        #region OnBindedToView
        ///// <summary>
        ///// This will be invoked by view when this viewmodel instance is set to view's ViewModel property. 
        ///// </summary>
        ///// <param name="view">Set target</param>
        ///// <param name="oldValue">Value before set.</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
        //{
        //    return base.OnBindedToView(view, oldValue);
        //}
        #endregion

        #region OnUnbindedFromView
        ///// <summary>
        ///// This will be invoked by view when this instance of viewmodel in ViewModel property is overwritten.
        ///// </summary>
        ///// <param name="view">Overwrite target view.</param>
        ///// <param name="newValue">The value replacing </param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue)
        //{
        //    return base.OnUnbindedFromView(view, newValue);
        //}
        #endregion

        #region OnBindedViewLoad

        ///// <summary>
        ///// This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Load event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewLoad(view);
        //}
        #endregion

        #region OnBindedViewUnload

        ///// <summary>
        ///// This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's  ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Unload event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewUnload(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewUnload(view);
        //}
        #endregion

        #region OnDisposeExceptions

        ///// <summary>
        ///// <para>If dispose actions got exceptions, will handled here. </para>
        ///// </summary>
        ///// <param name="exceptions">
        ///// <para>The exception and dispose information</para>
        ///// </param>
        //protected override async void OnDisposeExceptions(IList<DisposeEntry> exceptions)
        //{
        //    base.OnDisposeExceptions(exceptions);
        //    await Task.Yield();
        //}
        #endregion

        #endregion

    }
}


using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;


namespace MVVMSidekickWPFDemo.ViewModels
{
    /// <summary>
    /// 计数器视图模型类，演示基本的MVVM计数功能
    /// Counter view model class demonstrating basic MVVM counting functionality
    /// </summary>
    public class Counter_Model : ViewModel<Counter_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

        /// <summary>
        /// 初始化Counter_Model实例
        /// Initializes Counter_Model instance
        /// </summary>
        public Counter_Model()
        {
          

        }
        
        /// <summary>
        /// 使用服务提供者初始化Counter_Model实例
        /// Initializes Counter_Model instance with service provider
        /// </summary>
        /// <param name="serviceProvider">服务提供者 / Service provider</param>
        public Counter_Model(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        
        /// <summary>
        /// 获取服务提供者
        /// Gets the service provider
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 获取或设置当前计数值
        /// Gets or sets the current count value
        /// </summary>
        /// <summary>
        /// 获取或设置当前计数值
        /// Gets or sets the current count value
        /// </summary>
        public int CurrentCount { get => _CurrentCountLocator(this).Value; set => _CurrentCountLocator(this).SetValueAndTryNotify(value); }
        #region Property int CurrentCount Setup        
        protected Property<int> _CurrentCount = new Property<int>(_CurrentCountLocator);
        static Func<BindableBase, ValueContainer<int>> _CurrentCountLocator = RegisterContainerLocator(nameof(CurrentCount), m => m.Initialize(nameof(CurrentCount), ref m._CurrentCount, ref _CurrentCountLocator, () =>0));
        #endregion

        /// <summary>
        /// 获取递增计数命令模型
        /// Gets the increment count command model
        /// </summary>
        public CommandModel CommandIncrementCount => _CommandIncrementCountLocator(this).Value;
        #region Property CommandModel CommandIncrementCount Setup                
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

        ///// <summary>
        ///// This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Load event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewLoad(view);
        //}

        ///// <summary>
        ///// This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's  ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Unload event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewUnload(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewUnload(view);
        //}

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

    }

}


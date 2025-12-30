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
    /// 主窗口视图模型类，提供导航和演示功能
    /// Main window view model class providing navigation and demo functionality
    /// </summary>
    public class MainWindow_Model : ViewModel<MainWindow_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

        /// <summary>
        /// 初始化MainWindow_Model实例，在设计模式下设置特殊标题
        /// Initializes MainWindow_Model instance with special title in design mode
        /// </summary>
        public MainWindow_Model()
        {
            if (IsInDesignMode)
            {
                Title = "Title is a little different in Design mode";
            }

        }

        /// <summary>
        /// 获取或设置窗口标题
        /// Gets or sets the window title
        /// </summary>
        public string Title { get => _TitleLocator(this).Value; set => _TitleLocator(this).SetValueAndTryNotify(value); }
        #region Property string Title Setup        
        protected Property<string> _Title = new Property<string>(_TitleLocator);
        static Func<BindableBase, ValueContainer<string>> _TitleLocator = RegisterContainerLocator(nameof(Title), m => m.Initialize(nameof(Title), ref m._Title, ref _TitleLocator, () => "MVVM-Sidekick Demos"));
        #endregion

        /// <summary>
        /// 获取导航框架阶段
        /// Gets the navigation frame stage
        /// </summary>
        public IStage NavigationFrame => this.StageManager[nameof(NavigationFrame)];

        /// <summary>
        /// 获取导航到计数器页面的命令模型
        /// Gets the command model for navigating to counter page
        /// </summary>
        public CommandModel CommandNaviToCounter => _CommandNaviToCounterLocator(this).Value;
        #region Property CommandModel CommandNaviToCounter Setup                
        protected Property<CommandModel> _CommandNaviToCounter = new Property<CommandModel>(_CommandNaviToCounterLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandNaviToCounterLocator = RegisterContainerLocator(nameof(CommandNaviToCounter), m => m.Initialize(nameof(CommandNaviToCounter), ref m._CommandNaviToCounter, ref _CommandNaviToCounterLocator,
              model =>
              {
                  object state = nameof(CommandNaviToCounter);
                  var commandId = nameof(CommandNaviToCounter);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {
                              await vm.NavigationFrame.Show<Counter_Model>();
                          })
                      .Subscribe()
                      .DisposeWith(vm);

                  var cmdmdl = cmd.CreateCommandModel(state);

                  return cmdmdl;
              }));
        #endregion


        /// <summary>
        /// 获取导航到数据获取页面的命令模型
        /// Gets the command model for navigating to fetch data page
        /// </summary>
        public CommandModel CommandNaviToFetchData => _CommandNaviToFetchDataLocator(this).Value;
        #region Property CommandModel CommandNaviToFetchData Setup                
        protected Property<CommandModel> _CommandNaviToFetchData = new Property<CommandModel>(_CommandNaviToFetchDataLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandNaviToFetchDataLocator = RegisterContainerLocator(nameof(CommandNaviToFetchData), m => m.Initialize(nameof(CommandNaviToFetchData), ref m._CommandNaviToFetchData, ref _CommandNaviToFetchDataLocator,
              model =>
              {
                  object state = nameof(CommandNaviToFetchData);
                  var commandId = nameof(CommandNaviToFetchData);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {
                              await vm.NavigationFrame.Show<FetchData_Model>();

                          })
                      .Subscribe()
                      .DisposeWith(vm);

                  var cmdmdl = cmd.CreateCommandModel(state);

                  return cmdmdl;
              }));
        #endregion



        /// <summary>
        /// 获取导航到登录演示页面的命令模型
        /// Gets the command model for navigating to login demo page
        /// </summary>
        public CommandModel CommandNaviToLoginDemo => _CommandNaviToLoginDemoLocator(this).Value;
        #region Property CommandModel CommandNaviToLoginDemo Setup                
        protected Property<CommandModel> _CommandNaviToLoginDemo = new Property<CommandModel>(_CommandNaviToLoginDemoLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandNaviToLoginDemoLocator = RegisterContainerLocator(nameof(CommandNaviToLoginDemo), m => m.Initialize(nameof(CommandNaviToLoginDemo), ref m._CommandNaviToLoginDemo, ref _CommandNaviToLoginDemoLocator,
              model =>
              {
                  object state = nameof(CommandNaviToLoginDemo);
                  var commandId = nameof(CommandNaviToLoginDemo);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {
                              await vm.NavigationFrame.Show<LoginDemo_Model>();
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


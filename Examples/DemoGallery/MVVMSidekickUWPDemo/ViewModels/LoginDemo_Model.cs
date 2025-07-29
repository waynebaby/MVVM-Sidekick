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
    /// 登录演示视图模型，提供登录功能的演示
    /// Login demo view model providing login functionality demonstration
    /// </summary>
    [DataContract]
    public class LoginDemo_Model : ViewModel<LoginDemo_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        /// <summary>
        /// 初始化LoginDemo_Model的新实例，在设计模式下设置不同的标题
        /// Initializes a new instance of LoginDemo_Model with different title in design mode
        /// </summary>
        public LoginDemo_Model()
        {
            if (IsInDesignMode)
            {
                Title = "Title is a little different in Design mode";
            }

        }
        
        /// <summary>
        /// 使用服务提供程序初始化LoginDemo_Model的新实例
        /// Initializes a new instance of LoginDemo_Model with service provider
        /// </summary>
        /// <param name="serviceProvider">服务提供程序 / Service provider</param>
        public LoginDemo_Model(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        
        /// <summary>
        /// 获取服务提供程序
        /// Gets the service provider
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 获取或设置页面标题
        /// Gets or sets the page title
        /// </summary>
        public string Title { get => _TitleLocator(this).Value; set => _TitleLocator(this).SetValueAndTryNotify(value); }
        
        #region Property string Title Setup        
        /// <summary>
        /// Title属性的内部存储和定位器设置
        /// Internal storage and locator setup for Title property
        /// </summary>
        protected Property<string> _Title = new Property<string>(_TitleLocator);
        static Func<BindableBase, ValueContainer<string>> _TitleLocator = RegisterContainerLocator(nameof(Title), m => m.Initialize(nameof(Title), ref m._Title, ref _TitleLocator, () => nameof(LoginDemo_Model)));
        #endregion

        /// <summary>
        /// 获取某个命令的命令模型
        /// Gets the command model for some command
        /// </summary>
        public CommandModel CommandSomeCommand => _CommandSomeCommandLocator(this).Value;
        
        #region Property CommandModel CommandSomeCommand Setup                
        /// <summary>
        /// CommandSomeCommand命令属性的内部存储和定位器设置，包含通用命令逻辑
        /// Internal storage and locator setup for CommandSomeCommand command property, containing generic command logic
        /// </summary>
        protected Property<CommandModel> _CommandSomeCommand = new Property<CommandModel>(_CommandSomeCommandLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandSomeCommandLocator = RegisterContainerLocator(nameof(CommandSomeCommand), m => m.Initialize(nameof(CommandSomeCommand), ref m._CommandSomeCommand, ref _CommandSomeCommandLocator,
              model =>
              {
                  object state = nameof(CommandSomeCommand);
                  var commandId = nameof(CommandSomeCommand);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {
                            //Todo: Add SomeCommand logic here  
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


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

namespace SubFrameSample.ViewModels
{

    [DataContract]
    public class MainPage_Model : ViewModelBase<MainPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

        public MainPage_Model()
        {
            if (IsInDesignMode )
            {
                Title = "Title is a little different in Design mode";
            }
        
        }

        //propvm tab tab string tab Title
        /// <summary>
        /// <para>获取或设置页面标题</para>
        /// <para>Gets or sets the page title</para>
        /// </summary>
        public String Title
        {
            get { return _TitleLocator(this).Value; }
            set { _TitleLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String Title Setup
        protected Property<String> _Title = new Property<String> { LocatorFunc = _TitleLocator };
        static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator<String>("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
        static Func<String> _TitleDefaultValueFactory = ()=>"Title is Here";
		#endregion



		/// <summary>
		/// <para>获取或设置导航命令</para>
		/// <para>Gets or sets the navigation command</para>
		/// </summary>
		public CommandModel<ReactiveCommand, String> CommandNavi
		{
			get { return _CommandNaviLocator(this).Value; }
			set { _CommandNaviLocator(this).SetValueAndTryNotify(value); }
		}
		#region Property CommandModel<ReactiveCommand, String> CommandNavi Setup        
		protected Property<CommandModel<ReactiveCommand, String>> _CommandNavi = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandNaviLocator };
		static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandNaviLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandNavi", model => model.Initialize("CommandNavi", ref model._CommandNavi, ref _CommandNaviLocator, _CommandNaviDefaultValueFactory));
		static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandNaviDefaultValueFactory =
			model =>
			{
				var resource = "Navi";           // Command resource  
				var commandId = "Navi";
				var vm = CastToCurrentType(model);
				var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
				cmd
					.DoExecuteUIBusyTask(
						vm,
						async e =>
						{
							await vm.StageManager["WOKAO"].Show<BlankPage1_Model>(); 
							//Todo: Add Navi logic here, or
							await MVVMSidekick.Utilities.TaskExHelper.Yield();
						}
					)
					.DoNotifyDefaultEventRouter(vm, commandId)
					.Subscribe()
					.DisposeWith(vm);

				var cmdmdl = cmd.CreateCommandModel(resource);
				cmdmdl.ListenToIsUIBusy(model: vm, canExecuteWhenBusy: false);
				return cmdmdl;
			};
		#endregion


		#region Life Time Event Handling

		///// <summary>
		///// <para>当此视图模型实例被设置到视图的ViewModel属性时，将由视图调用</para>
		///// <para>This will be invoked by view when this viewmodel instance is set to view's ViewModel property</para>
		///// </summary>
		///// <param name="view">设置目标 / Set target</param>
		///// <param name="oldValue">设置前的值 / Value before set</param>
		///// <returns>任务等待器 / Task awaiter</returns>
		//protected override Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
		//{
		//    return base.OnBindedToView(view, oldValue);
		//}

		///// <summary>
		///// <para>当ViewModel属性中的此视图模型实例被覆盖时，将由视图调用</para>
		///// <para>This will be invoked by view when this instance of viewmodel in ViewModel property is overwritten</para>
		///// </summary>
		///// <param name="view">覆盖目标视图 / Overwrite target view</param>
		///// <param name="newValue">替换的值 / The value replacing</param>
		///// <returns>任务等待器 / Task awaiter</returns>
		//protected override Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue)
		//{
		//    return base.OnUnbindedFromView(view, newValue);
		//}

		///// <summary>
		///// <para>当视图触发Load事件且此视图模型实例已在视图的ViewModel属性中时，将由视图调用</para>
		///// <para>This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property</para>
		///// </summary>
		///// <param name="view">触发Load事件的视图 / View that firing Load event</param>
		///// <returns>任务等待器 / Task awaiter</returns>
		//protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
		//{
		//    return base.OnBindedViewLoad(view);
		//}

		///// <summary>
		///// <para>当视图触发Unload事件且此视图模型实例仍在视图的ViewModel属性中时，将由视图调用</para>
		///// <para>This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's ViewModel property</para>
		///// </summary>
		///// <param name="view">触发Unload事件的视图 / View that firing Unload event</param>
		///// <returns>任务等待器 / Task awaiter</returns>
		//protected override Task OnBindedViewUnload(MVVMSidekick.Views.IView view)
		//{
		//    return base.OnBindedViewUnload(view);
		//}

		///// <summary>
		///// <para>如果释放操作发生异常，将在此处理</para>
		///// <para>If dispose actions got exceptions, will handled here</para>
		///// </summary>
		///// <param name="exceptions">
		///// <para>异常和释放信息</para>
		///// <para>The exception and dispose information</para>
		///// </param>
		//protected override async void OnDisposeExceptions(IList<DisposeInfo> exceptions)
		//{
		//    base.OnDisposeExceptions(exceptions);
		//    await TaskExHelper.Yield();
		//}

		#endregion


	}

}


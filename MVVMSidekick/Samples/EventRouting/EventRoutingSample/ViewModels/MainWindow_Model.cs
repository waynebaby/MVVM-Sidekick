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
using MVVMSidekick.EventRouting;

namespace EventRoutingSample.ViewModels
{

	public class MainWindow_Model : ViewModelBase<MainWindow_Model>
	{
		// If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
		// 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

		public MainWindow_Model()
		{
			if (IsInDesignMode)
			{
				Title = "Title is a little different in Design mode";
			}

		}



		//propvm tab tab string tab Title
		public String Title
		{
			get { return _TitleLocator(this).Value; }
			set { _TitleLocator(this).SetValueAndTryNotify(value); }
		}
		#region Property String Title Setup
		protected Property<String> _Title = new Property<String> { LocatorFunc = _TitleLocator };
		static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator<String>("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
		static Func<String> _TitleDefaultValueFactory = () => "Title is Here";
		#endregion


		public string LastHeartBeat
		{
			get { return _LastHeartBeatLocator(this).Value; }
			set { _LastHeartBeatLocator(this).SetValueAndTryNotify(value); }
		}
		#region Property string LastHeartBeat Setup        
		protected Property<string> _LastHeartBeat = new Property<string> { LocatorFunc = _LastHeartBeatLocator };
		static Func<BindableBase, ValueContainer<string>> _LastHeartBeatLocator = RegisterContainerLocator<string>("LastHeartBeat", model => model.Initialize("LastHeartBeat", ref model._LastHeartBeat, ref _LastHeartBeatLocator, _LastHeartBeatDefaultValueFactory));
		static Func<string> _LastHeartBeatDefaultValueFactory = () => { return default(string); };
		#endregion

		protected override Task OnBindedViewLoad(IView view)
		{
			EventRouter.Instance.GetEventChannel<Object>()
				.Where(e => 
					e.EventName == "Global HeartBeat")
				.Subscribe(
					e =>
						LastHeartBeat = e.EventData.ToString ()
				)
				.DisposeWhenUnload(this);

			return base.OnBindedViewLoad(view);
		}





		public CommandModel<ReactiveCommand, String> CommandNavigateToDisposeBehaviorTest
		{
			get { return _CommandNavigateToDisposeBehaviorTestLocator(this).Value; }
			set { _CommandNavigateToDisposeBehaviorTestLocator(this).SetValueAndTryNotify(value); }
		}
		#region Property CommandModel<ReactiveCommand, String> CommandNavigateToDisposeBehaviorTest Setup        

		protected Property<CommandModel<ReactiveCommand, String>> _CommandNavigateToDisposeBehaviorTest = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandNavigateToDisposeBehaviorTestLocator };
		static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandNavigateToDisposeBehaviorTestLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandNavigateToDisposeBehaviorTest), model => model.Initialize(nameof(CommandNavigateToDisposeBehaviorTest), ref model._CommandNavigateToDisposeBehaviorTest, ref _CommandNavigateToDisposeBehaviorTestLocator, _CommandNavigateToDisposeBehaviorTestDefaultValueFactory));
		static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandNavigateToDisposeBehaviorTestDefaultValueFactory =
			model =>
			{
				var resource = nameof(CommandNavigateToDisposeBehaviorTest);           // Command resource  
				var commandId = nameof(CommandNavigateToDisposeBehaviorTest);
				var vm = CastToCurrentType(model);
				var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

				cmd.DoExecuteUIBusyTask(
						vm,
						async e =>
						{
							//Todo: Add NavigateToDisposeBehaviorTest logic here, or
							await vm.StageManager.DefaultStage.Show<DisopseTestForBehaviors_Model>();
							await MVVMSidekick.Utilities.TaskExHelper.Yield();
						})
					.DoNotifyDefaultEventRouter(vm, commandId)
					.Subscribe()
					.DisposeWith(vm);

				var cmdmdl = cmd.CreateCommandModel(resource);

				cmdmdl.ListenToIsUIBusy(
					model: vm,
					canExecuteWhenBusy: false);
				return cmdmdl;
			};

		#endregion



	}

}


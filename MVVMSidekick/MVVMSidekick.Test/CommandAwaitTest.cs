using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVMSidekick.Reactive;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSidekick.Test
{
	[TestClass]

	public class CommandAwaitTest
	{
		[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
		public async Task CommandAwaitTestTestCommand()
		{
			var vm = new CommandAsyncTestVM();
			await vm.Command1.ExecuteAsync(null);
			Assert.AreEqual(vm.Value, 4);
		}
	}



	//[DataContract() ] //if you want
	public class CommandAsyncTestVM : ViewModelBase<CommandAsyncTestVM>
	{
		public CommandAsyncTestVM()
		{

			if (IsInDesignMode)
			{
				//Add design time mock data init here. These will not execute in runtime.
			}
		}

		//Use propvm + tab +tab  to create a new property of vm here:
		//Use propcvm + tab +tab  to create a new property of vm  with complex default value factory here:
		//Use propcmd + tab +tab  to create a new command of vm here:




		public int Value
		{
			get { return _ValueLocator(this).Value; }
			set { _ValueLocator(this).SetValueAndTryNotify(value); }
		}
		#region Property int Value Setup        
		protected Property<int> _Value = new Property<int> { LocatorFunc = _ValueLocator };
		static Func<BindableBase, ValueContainer<int>> _ValueLocator = RegisterContainerLocator<int>(nameof(Value), model => model.Initialize(nameof(Value), ref model._Value, ref _ValueLocator, _ValueDefaultValueFactory));
		static Func<int> _ValueDefaultValueFactory = () => default(int);
		#endregion

		public CommandModel<ReactiveCommand, String> Command1
		{
			get { return _Command1Locator(this).Value; }
			set { _Command1Locator(this).SetValueAndTryNotify(value); }
		}
		#region Property CommandModel<ReactiveCommand, String> Command1 Setup        

		protected Property<CommandModel<ReactiveCommand, String>> _Command1 = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _Command1Locator };
		static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _Command1Locator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(Command1), model => model.Initialize(nameof(Command1), ref model._Command1, ref _Command1Locator, _Command1DefaultValueFactory));
		static Func<BindableBase, CommandModel<ReactiveCommand, String>> _Command1DefaultValueFactory =
			model =>
			{
				var resource = nameof(Command1);           // Command resource  
				var commandId = nameof(Command1);
				var vm = CastToCurrentType(model);
				var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

				cmd.DoExecuteUIBusyTask(
						vm,
						async e =>
						{

							vm.Value = 1;
							await Task.Delay(3000);
							await vm.Command2.ExecuteAsync(e);							
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


		public CommandModel<ReactiveCommand, String> Command2
		{
			get { return _Command2Locator(this).Value; }
			set { _Command2Locator(this).SetValueAndTryNotify(value); }
		}
		#region Property CommandModel<ReactiveCommand, String> Command2 Setup        

		protected Property<CommandModel<ReactiveCommand, String>> _Command2 = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _Command2Locator };
		static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _Command2Locator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("Command2", model => model.Initialize("Command2", ref model._Command2, ref _Command2Locator, _Command2DefaultValueFactory));
		static Func<BindableBase, CommandModel<ReactiveCommand, String>> _Command2DefaultValueFactory =
			model =>
			{
				var resource = "Command2";           // Command resource  
				var commandId = "Command2";
				var vm = CastToCurrentType(model);
				var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

				cmd.DoExecuteUIBusyTask(
						vm,
						async e =>
						{
							//Todo: Add 2 logic here, or
							await Task.Delay(3000);
							vm.Value = 4;

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

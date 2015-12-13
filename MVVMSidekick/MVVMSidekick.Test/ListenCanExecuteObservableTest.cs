using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Reactive;
using System.Reactive.Linq;


namespace MVVMSidekick.Test
{
	/// <summary>
	/// Summary description for ListenCanExecuteObservableTest
	/// </summary>
	[TestClass]
	public class ListenCanExecuteObservableTest
	{
		public ListenCanExecuteObservableTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void TestSingleListenCanExecuteObservable()
		{
			//
			// TODO: Add test logic here
			//

			using (var vm = new TestSingleListenCanExecuteObservableViewModel())
			{
				vm.CanExecute = true;
				Assert.IsTrue(vm.CommandSomeCommand.CanExecute(null));
				vm.CanExecute = false;
				Assert.IsFalse (vm.CommandSomeCommand.CanExecute(null));
				vm.CanExecute = true;
				Assert.IsTrue(vm.CommandSomeCommand.CanExecute(null));
				vm.CanExecute = false;
				Assert.IsFalse(vm.CommandSomeCommand.CanExecute(null));
				vm.CanExecute = true;
				Assert.IsTrue(vm.CommandSomeCommand.CanExecute(null));
				vm.CanExecute = false;
				Assert.IsFalse(vm.CommandSomeCommand.CanExecute(null));
			}


		}


		public class TestSingleListenCanExecuteObservableViewModel : ViewModelBase<TestSingleListenCanExecuteObservableViewModel>
		{


			public TestSingleListenCanExecuteObservableViewModel()
			{
				this.CommandSomeCommand.CommandCore.ListenCanExecuteObservable(
					this.GetValueContainer(x => x.CanExecute).GetNewValueObservable().Select(x =>
					this.CanExecute))
				.DisposeWith(this);


			}
			public bool CanExecute
			{
				get { return _CanExecuteLocator(this).Value; }
				set { _CanExecuteLocator(this).SetValueAndTryNotify(value); }
			}
			#region Property bool CanExecute Setup        
			protected Property<bool> _CanExecute = new Property<bool> { LocatorFunc = _CanExecuteLocator };
			static Func<BindableBase, ValueContainer<bool>> _CanExecuteLocator = RegisterContainerLocator<bool>(nameof(CanExecute), model => model.Initialize(nameof(CanExecute), ref model._CanExecute, ref _CanExecuteLocator, _CanExecuteDefaultValueFactory));
			static Func<bool> _CanExecuteDefaultValueFactory = () => default(bool);
			#endregion

			public CommandModel<ReactiveCommand, String> CommandSomeCommand
			{
				get { return _CommandSomeCommandLocator(this).Value; }
				set { _CommandSomeCommandLocator(this).SetValueAndTryNotify(value); }
			}
			#region Property CommandModel<ReactiveCommand, String> CommandSomeCommand Setup        

			protected Property<CommandModel<ReactiveCommand, String>> _CommandSomeCommand = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandSomeCommandLocator };
			static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandSomeCommandLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandSomeCommand), model => model.Initialize(nameof(CommandSomeCommand), ref model._CommandSomeCommand, ref _CommandSomeCommandLocator, _CommandSomeCommandDefaultValueFactory));
			static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandSomeCommandDefaultValueFactory =
				model =>
				{
					var resource = nameof(CommandSomeCommand);           // Command resource  
					var commandId = nameof(CommandSomeCommand);
					var vm = CastToCurrentType(model);
					var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

					cmd.DoExecuteUIBusyTask(
								vm,
								async e =>
								{
									//Todo: Add SomeCommand logic here, or
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



}

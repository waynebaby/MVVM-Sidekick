using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Reactive;
using MVVMSidekick.Views;
using System.Threading.Tasks;

namespace MVVMSidekick.Test
{
    /// <summary>
    /// Summary description for TestingTest
    /// </summary>
    [TestClass]
    public class TestingTest
    {
        public TestingTest()
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
        public async Task TestMockingViewModel()
        {
            //
            // TODO: Add test logic here
            var cm = new SomeTestingViewModel();

            var tsm = cm.StageManager.DefaultStage as TestingStage;
            tsm.MockShowLogic<SomeTestingViewModel>(async (vm) =>
            {
                var ivm = vm as IViewModel;
                await ivm.OnBindedViewLoad(null);
                vm.CommandGetNowString.Execute(null);
                await Task.Delay(200);
                return vm;
            });


            var showed = await cm.StageManager.DefaultStage.Show(cm);
            await Task.Delay(200);
            Assert.IsNotNull(cm.Result);

        }




    }



    //[DataContract(IsReference=true) ] //if you want
    public class SomeTestingViewModel : ViewModelBase<SomeTestingViewModel>
    {
        public SomeTestingViewModel()
        {

            if (IsInDesignMode)
            {
                //Add design time demo data init here. These will not execute in runtime.
            }
        }

        //Use propvm + tab +tab  to create a new property of vm here:
        //Use propcvm + tab +tab  to create a new property of vm  with complex default value factory here:
        //Use propcmd + tab +tab  to create a new command of vm here:


        public string Result
        {
            get { return _ResultLocator(this).Value; }
            set { _ResultLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string Result Setup        
        protected Property<string> _Result = new Property<string> { LocatorFunc = _ResultLocator };
        static Func<BindableBase, ValueContainer<string>> _ResultLocator = RegisterContainerLocator<string>("Result", model => model.Initialize("Result", ref model._Result, ref _ResultLocator, _ResultDefaultValueFactory));
        static Func<string> _ResultDefaultValueFactory = () => default(string);
        #endregion


        public CommandModel<ReactiveCommand, String> CommandGetNowString
        {
            get { return _CommandGetNowStringLocator(this).Value; }
            set { _CommandGetNowStringLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandGetNowString Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandGetNowString = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandGetNowStringLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandGetNowStringLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandGetNowString), model => model.Initialize(nameof(CommandGetNowString), ref model._CommandGetNowString, ref _CommandGetNowStringLocator, _CommandGetNowStringDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandGetNowStringDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandGetNowString);           // Command resource  
                var commandId = nameof(CommandGetNowString);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            //Todo: Add GetNowString logic here, or

                            vm.Result = DateTime.Now.ToString();
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

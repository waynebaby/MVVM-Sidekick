using System;
#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#elif WINDOWS_PHONE_8
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using MVVMSidekick.ViewModels;
using System.Collections.ObjectModel;
using MVVMSidekick.Reactive;
using System.Threading.Tasks;

namespace MVVMSidekick.Test
{


    //[DataContract(IsReference=true) ] //if you want
    public class SomeViewModel : ViewModelBase<SomeViewModel>
    {
        public SomeViewModel()
        {
            // Use propery to init value here:
            if (IsInDesignMode)
            {
                //Add design time demo data init here. These will not execute in runtime.
            }


        }

        //Use propvm + tab +tab  to create a new property of vm here:
        //Use propcmd + tab +tab  to create a new command of vm here:

        public String Prop1
        {
            get { return _Prop1Locator(this).Value; }
            set { _Prop1Locator(this).SetValueAndTryNotify(value); }
        }
        #region Property String Prop1 Setup
        protected Property<String> _Prop1 = new Property<String> { LocatorFunc = _Prop1Locator };
        static Func<BindableBase, ValueContainer<String>> _Prop1Locator = RegisterContainerLocator<String>("Prop1", model => model.Initialize("Prop1", ref model._Prop1, ref _Prop1Locator, _Prop1DefaultValueFactory));
        static Func<String> _Prop1DefaultValueFactory = null;
        #endregion


        public int Prop2
        {
            get { return _Prop2Locator(this).Value; }
            set { _Prop2Locator(this).SetValueAndTryNotify(value); }
        }
        #region Property int Prop2 Setup
        protected Property<int> _Prop2 = new Property<int> { LocatorFunc = _Prop2Locator };
        static Func<BindableBase, ValueContainer<int>> _Prop2Locator = RegisterContainerLocator<int>("Prop2", model => model.Initialize("Prop2", ref model._Prop2, ref _Prop2Locator, _Prop2DefaultValueFactory));
        static Func<int> _Prop2DefaultValueFactory = null;
        #endregion





        public double Prop3
        {
            get { return _Prop3Locator(this).Value; }
            set { _Prop3Locator(this).SetValueAndTryNotify(value); }
        }
        #region Property double  Prop3 Setup
        protected Property<double> _Prop3 = new Property<double> { LocatorFunc = _Prop3Locator };
        static Func<BindableBase, ValueContainer<double>> _Prop3Locator = RegisterContainerLocator<double>("Prop3", model => model.Initialize("Prop3", ref model._Prop3, ref _Prop3Locator, _Prop3DefaultValueFactory));
        static Func<double> _Prop3DefaultValueFactory = null;
        #endregion


        public ObservableCollection<string> PropStringCol
        {
            get { return _PropStringColLocator(this).Value; }
            set { _PropStringColLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<string> PropStringCol Setup
        protected Property<ObservableCollection<string>> _PropStringCol = new Property<ObservableCollection<string>> { LocatorFunc = _PropStringColLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<string>>> _PropStringColLocator = RegisterContainerLocator<ObservableCollection<string>>("PropStringCol", model => model.Initialize("PropStringCol", ref model._PropStringCol, ref _PropStringColLocator, _PropStringColDefaultValueFactory));
        static Func<ObservableCollection<string>> _PropStringColDefaultValueFactory = () => new ObservableCollection<string>();
        #endregion



        public CommandModel<ReactiveCommand, String> CommandExecuteWithUIBusyOnly
        {
            get { return _CommandExecuteWithUIBusyOnlyLocator(this).Value; }
            set { _CommandExecuteWithUIBusyOnlyLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandExecuteWithUIBusyOnly Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandExecuteWithUIBusyOnly = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandExecuteWithUIBusyOnlyLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandExecuteWithUIBusyOnlyLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandExecuteWithUIBusyOnly", model => model.Initialize("CommandExecuteWithUIBusyOnly", ref model._CommandExecuteWithUIBusyOnly, ref _CommandExecuteWithUIBusyOnlyLocator, _CommandExecuteWithUIBusyOnlyDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandExecuteWithUIBusyOnlyDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //cmd.Subscribe (_=>{ } ).DisposeWith(model); //Config it if needed
                return cmd.CreateCommandModel("ExecuteWithUIBusyOnly");
            };
        #endregion


        public CommandModel<ReactiveCommand, String> CommandExecuteWhenNotBusy
        {
            get { return _CommandExecuteWhenNotBusyLocator(this).Value; }
            set { _CommandExecuteWhenNotBusyLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandExecuteWhenNotBusy Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandExecuteWhenNotBusy = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandExecuteWhenNotBusyLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandExecuteWhenNotBusyLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandExecuteWhenNotBusy", model => model.Initialize("CommandExecuteWhenNotBusy", ref model._CommandExecuteWhenNotBusy, ref _CommandExecuteWhenNotBusyLocator, _CommandExecuteWhenNotBusyDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandExecuteWhenNotBusyDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //cmd.Subscribe (_=>{ } ).DisposeWith(model); //Config it if needed
                return cmd.CreateCommandModel("ExecuteWhenNotBusy");
            };
        #endregion



    }






    [TestClass]
    public class ViewModelTests
    {
        /// <summary>
        /// Test  value copy 
        /// </summary>
        [TestMethod]
        public void CopyData_Test()
        {
            using (var vm1 = new SomeViewModel { Prop1 = "aaa", Prop2 = 1, Prop3 = 555.55, PropStringCol = new ObservableCollection<string> { "a", "b", "c" } })
            using (var vm2 = new SomeViewModel())
            {
                vm1.CopyTo(vm2);
                Assert.AreEqual(vm1.Prop1, vm2.Prop1);
                Assert.AreEqual(vm1.Prop2, vm2.Prop2);
                Assert.AreEqual(vm1.Prop3, vm2.Prop3);
                Assert.AreNotEqual(vm1.PropStringCol, vm2.PropStringCol);
                Assert.AreEqual(vm1.PropStringCol.Count, vm2.PropStringCol.Count);
                for (int i = 0; i < vm1.PropStringCol.Count; i++)
                {
                    Assert.AreEqual(vm1.PropStringCol[i], vm2.PropStringCol[i]);
                }
            }
        }

        [TestMethod]
        public void CommandListenToUIBusy_Test()
        {
            using (var vm1 = new SomeViewModel())
            {
                vm1.CommandExecuteWhenNotBusy.ListenToIsUIBusy(vm1, canExecuteWhenBusy: false);
                vm1.CommandExecuteWithUIBusyOnly.ListenToIsUIBusy(vm1, canExecuteWhenBusy: true);
                var task = new Task(() => { });
                var taskend=   vm1.ExecuteTask (async _ => await task,(object)null);
                Assert.IsFalse(vm1.CommandExecuteWhenNotBusy.CanExecute(null));
                Assert.IsTrue(vm1.CommandExecuteWithUIBusyOnly.CanExecute(null));

                task.Start();
                taskend.Wait();
                Assert.IsTrue(vm1.CommandExecuteWhenNotBusy.CanExecute(new object()));
                Assert.IsFalse(vm1.CommandExecuteWithUIBusyOnly.CanExecute(null));
            }


        }
    }
}

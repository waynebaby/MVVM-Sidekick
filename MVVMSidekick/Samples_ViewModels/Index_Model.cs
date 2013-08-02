using MVVMSidekick;
using MVVMSidekick.Reactive;
using MVVMSidekick.Utilities;
using MVVMSidekick.ViewModels;
using Samples.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Samples.ViewModels
{

    public class Index_Model : ViewModelBase<Index_Model>
    {
        public Index_Model()
        {


        }

        protected override async Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        {
            await base.OnBindedViewLoad(view);

            if (IsInDesignMode)
            {
                HelloWorld = "Hello Mvvm world, Design mode sample";
            }
            else
            {
                GetValueContainer(x => x.CountDown).GetNullObservable()
                    .Subscribe(
                        _ =>
                        {
                            HelloWorld = string.Format("Loading {0}", CountDown);
                        }
                    );
            }
            // Loading count down. You may want to replace your own logic here.
            try
            {
                IsUIBusy = true;
                for (Double i = CountDown; i > 0; i = i - 1)
                {
                    CountDown = i;
                    await TaskExHelper.Delay(500);
                }
                CountDown = 0;
                HelloWorld = "Hello Mvvm world!";
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                StateName = "Loaded";
                IsUIBusy = false;
            }



        }



        public String StateName
        {
            get { return _StateNameLocator(this).Value; }
            set { _StateNameLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String StateName Setup
        protected Property<String> _StateName = new Property<String> { LocatorFunc = _StateNameLocator };
        static Func<BindableBase, ValueContainer<String>> _StateNameLocator = RegisterContainerLocator<String>("StateName", model => model.Initialize("StateName", ref model._StateName, ref _StateNameLocator, _StateNameDefaultValueFactory));
        static Func<String> _StateNameDefaultValueFactory = () => "Loading";
        #endregion



        public Double CountDown
        {
            get { return _CountDownLocator(this).Value; }
            set { _CountDownLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property Double CountDown Setup
        protected Property<Double> _CountDown = new Property<Double> { LocatorFunc = _CountDownLocator };
        static Func<BindableBase, ValueContainer<Double>> _CountDownLocator = RegisterContainerLocator<Double>("CountDown", model => model.Initialize("CountDown", ref model._CountDown, ref _CountDownLocator, _CountDownDefaultValueFactory));
        static Func<Double> _CountDownDefaultValueFactory = () => 5;
        #endregion



        public string HelloWorld
        {
            get { return _HelloWorldLocator(this).Value; }
            set { _HelloWorldLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string HelloWorld Setup
        protected Property<string> _HelloWorld = new Property<string> { LocatorFunc = _HelloWorldLocator };
        static Func<BindableBase, ValueContainer<string>> _HelloWorldLocator = RegisterContainerLocator<string>("HelloWorld", model => model.Initialize("HelloWorld", ref model._HelloWorld, ref _HelloWorldLocator, _HelloWorldDefaultValueFactory));
        static Func<BindableBase, string> _HelloWorldDefaultValueFactory = vm => "Hello Mvvm world";
        #endregion


        public string Title
        {
            get { return _TitleLocator(this).Value; }
            set { _TitleLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string  Title Setup
        protected Property<string> _Title = new Property<string> { LocatorFunc = _TitleLocator };
        static Func<BindableBase, ValueContainer<string>> _TitleLocator = RegisterContainerLocator<string>("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
        static Func<string> _TitleDefaultValueFactory = () => "Mvvm-Sidekick samples";
        #endregion



        public CommandModel<ReactiveCommand, String> CommandStartCalculator
        {
            get { return _CommandStartCalculatorLocator(this).Value; }
            set { _CommandStartCalculatorLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandStartCalculator Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandStartCalculator = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandStartCalculatorLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandStartCalculatorLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandStartCalculator", model => model.Initialize("CommandStartCalculator", ref model._CommandStartCalculator, ref _CommandStartCalculatorLocator, _CommandStartCalculatorDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandStartCalculatorDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                var vm = CastToCurrentType(model);
                cmd
                    .Subscribe(
                    async _ =>
                    {
                        await vm.StageManager.DefaultStage.Show<Calculator_Model>();
                    })
                    .DisposeWith(model); //Config it if needed
                return cmd.CreateCommandModel("Start Calculator").ListenToIsUIBusy(vm);
            };
        #endregion


#if !(NETFX_CORE||WINDOWS_PHONE_8)

        public CommandModel<ReactiveCommand, String> CommandStartTree
        {
            get { return _CommandStartTreeLocator(this).Value; }
            set { _CommandStartTreeLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandStartTree Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandStartTree = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandStartTreeLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandStartTreeLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandStartTree", model => model.Initialize("CommandStartTree", ref model._CommandStartTree, ref _CommandStartTreeLocator, _CommandStartTreeDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandStartTreeDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                 var vm = CastToCurrentType(model);
                cmd
                    .Subscribe(
                    async _ =>
                    {
                        await vm.StageManager.DefaultStage.Show<Tree_Model >();
                    })
                    .DisposeWith(model); //Config it if needed
                return cmd.CreateCommandModel("StartTree");
            };
        #endregion
#endif
#if NETFX_CORE

        public CommandModel<ReactiveCommand, String> CommandShowGroupView
        {
            get { return _CommandShowGroupViewLocator(this).Value; }
            set { _CommandShowGroupViewLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandShowGroupView Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandShowGroupView = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandShowGroupViewLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandShowGroupViewLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandShowGroupView", model => model.Initialize("CommandShowGroupView", ref model._CommandShowGroupView, ref _CommandShowGroupViewLocator, _CommandShowGroupViewDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandShowGroupViewDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //Config it if you want
                var vm = CastToCurrentType(model); //vm instance 
                cmd.Subscribe(
                  async _ =>
                  {
                      using (var targetvm = new GroupViewSample_Model())
                      {
                          targetvm.InitData(CollectionViewType.Grouped);
                          await vm.StageManager.DefaultStage.Show<GroupViewSample_Model>(targetvm);

                      }
                  })
                  .DisposeWith(vm);
                return cmd.CreateCommandModel("ShowGroupView");
            };
        #endregion

#endif

        public CommandModel<ReactiveCommand, String> CommandListTest
        {
            get { return _CommandListTestLocator(this).Value; }
            set { _CommandListTestLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandListTest Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandListTest = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandListTestLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandListTestLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandListTest", model => model.Initialize("CommandListTest", ref model._CommandListTest, ref _CommandListTestLocator, _CommandListTestDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandListTestDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                var vm = CastToCurrentType(model);
                cmd
                    .Subscribe(
                    async _ =>
                    {

                        await vm.StageManager.DefaultStage.Show<ListsAndItemsPattern_Model>();
                    }).DisposeWith(vm);
                return cmd.CreateCommandModel("ListTest");
            };
        #endregion


    }


}

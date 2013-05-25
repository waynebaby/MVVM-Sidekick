using MVVMSidekick.Reactive;
using MVVMSidekick.ViewModels;
using Samples.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.ViewModels
{

    public class Index_Model : ViewModelBase<Index_Model>
    {
        public Index_Model()
        {
            if (IsInDesignMode)
            {
                HelloWorld = "Hello Mvvm world, Design mode sample";
            }

        }
        protected override async Task OnBindedToView(MVVMSidekick.Views.IView view,IViewModel oldValue)
        {
            await base.OnBindedToView(view, oldValue);

     

        }

        protected override async Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        {
            await base.OnBindedViewLoad(view);

            CommandStartCalculator.CommandCore.Execute(null);
        }
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
                return cmd.CreateCommandModel("StartCalculator");
            };
        #endregion



    }


}

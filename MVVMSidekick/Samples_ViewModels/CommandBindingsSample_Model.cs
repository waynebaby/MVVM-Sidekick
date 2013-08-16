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

namespace Samples.ViewModels
{

    [DataContract]
    public class CommandBindingsSample_Model : ViewModelBase<CommandBindingsSample_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        public String Title
        {
            get { return _TitleLocator(this).Value; }
            set { _TitleLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String Title Setup
        protected Property<String> _Title = new Property<String> { LocatorFunc = _TitleLocator };
        static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator<String>("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
        static Func<BindableBase, String> _TitleDefaultValueFactory = m => m.GetType().Name;
        #endregion

        protected override Task OnBindedViewLoad(IView view)
        {
            return base.OnBindedViewLoad(view);
        }

        protected override async Task OnBindedToView(IView view, IViewModel oldValue)
        {
            await base.OnBindedToView(view, oldValue);
            // This method will be called when this VM is set to a View's ViewModel property. Add Handle Logic here.
            // TODO: Add Binded Handle Logic here.
        }

        protected override async Task OnUnbindedFromView(IView view, IViewModel newValue)
        {
            await base.OnUnbindedFromView(view, newValue);
            // This method will be called when this VM is removed from a View's ViewModel property. Add Handle Logic here.
            // TODO: Add Binded Handle Logic here.
        }


        
        public string EventName
        {
            get { return _EventNameLocator(this).Value; }
            set { _EventNameLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string EventName Setup
        protected Property<string> _EventName = new Property<string> { LocatorFunc = _EventNameLocator };
        static Func<BindableBase, ValueContainer<string>> _EventNameLocator = RegisterContainerLocator<string>("EventName", model => model.Initialize("EventName", ref model._EventName, ref _EventNameLocator, _EventNameDefaultValueFactory));
        static Func<string> _EventNameDefaultValueFactory = null;
        #endregion



        
        public string Sender
        {
            get { return _SenderLocator(this).Value; }
            set { _SenderLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string Sender Setup
        protected Property<string> _Sender = new Property<string> { LocatorFunc = _SenderLocator };
        static Func<BindableBase, ValueContainer<string>> _SenderLocator = RegisterContainerLocator<string>("Sender", model => model.Initialize("Sender", ref model._Sender, ref _SenderLocator, _SenderDefaultValueFactory));
        static Func<string> _SenderDefaultValueFactory = null;
        #endregion

        
        public Type EventHandlerType
        {
            get { return _EventHandlerTypeLocator(this).Value; }
            set { _EventHandlerTypeLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property Type EventHandlerType Setup
        protected Property<Type> _EventHandlerType = new Property<Type> { LocatorFunc = _EventHandlerTypeLocator };
        static Func<BindableBase, ValueContainer<Type>> _EventHandlerTypeLocator = RegisterContainerLocator<Type>("EventHandlerType", model => model.Initialize("EventHandlerType", ref model._EventHandlerType, ref _EventHandlerTypeLocator, _EventHandlerTypeDefaultValueFactory));
        static Func<Type> _EventHandlerTypeDefaultValueFactory = null;
        #endregion


        
        public Object Parameter
        {
            get { return _ParameterLocator(this).Value; }
            set { _ParameterLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property Object Parameter Setup
        protected Property<Object> _Parameter = new Property<Object> { LocatorFunc = _ParameterLocator };
        static Func<BindableBase, ValueContainer<Object>> _ParameterLocator = RegisterContainerLocator<Object>("Parameter", model => model.Initialize("Parameter", ref model._Parameter, ref _ParameterLocator, _ParameterDefaultValueFactory));
        static Func<Object> _ParameterDefaultValueFactory = null;
        #endregion


        
        public Object EventArgs
        {
            get { return _EventArgsLocator(this).Value; }
            set { _EventArgsLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property Object EventArgs Setup
        protected Property<Object> _EventArgs = new Property<Object> { LocatorFunc = _EventArgsLocator };
        static Func<BindableBase, ValueContainer<Object>> _EventArgsLocator = RegisterContainerLocator<Object>("EventArgs", model => model.Initialize("EventArgs", ref model._EventArgs, ref _EventArgsLocator, _EventArgsDefaultValueFactory));
        static Func<Object> _EventArgsDefaultValueFactory = null;
        #endregion



        
        public CommandModel<ReactiveCommand, String> CommandShowCommandDetails
        {
            get { return _CommandShowCommandDetailsLocator(this).Value; }
            set { _CommandShowCommandDetailsLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandShowCommandDetails Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandShowCommandDetails = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandShowCommandDetailsLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandShowCommandDetailsLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandShowCommandDetails", model => model.Initialize("CommandShowCommandDetails", ref model._CommandShowCommandDetails, ref _CommandShowCommandDetailsLocator, _CommandShowCommandDetailsDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandShowCommandDetailsDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //Config it if you want
                var vm = CastToCurrentType(model); //vm instance 
                cmd.Subscribe(
                  async _ =>
                  {
                      vm.EventName = _.EventArgs.EventName;
                      vm.EventArgs  = _.EventArgs.EventArgs ;
                      vm.EventHandlerType = _.EventArgs.EventHandlerType;
                      vm.Parameter = _.EventArgs.Parameter;
                      vm.Sender = _.EventArgs.ViewSender.ToString ();
                  })
                  .DisposeWith(vm); 
                return cmd.CreateCommandModel("ShowCommandDetails");
            };
        #endregion

        
        public CommandModel<ReactiveCommand, String> CommandNavigateToListAndView
        {
            get { return _CommandNavigateToListAndViewLocator(this).Value; }
            set { _CommandNavigateToListAndViewLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandNavigateToListAndView Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandNavigateToListAndView = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandNavigateToListAndViewLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandNavigateToListAndViewLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandNavigateToListAndView", model => model.Initialize("CommandNavigateToListAndView", ref model._CommandNavigateToListAndView, ref _CommandNavigateToListAndViewLocator, _CommandNavigateToListAndViewDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandNavigateToListAndViewDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //Config it if you want
                var vm = CastToCurrentType(model); //vm instance 
                cmd.Subscribe(
                  async _ =>
                  {
                    await  vm.StageManager.DefaultStage.Show<ListsAndItemsPattern_Model >();
                  })
                  .DisposeWith(vm); 
                return cmd.CreateCommandModel("NavigateToListAndView");
            };
        #endregion


    }

}


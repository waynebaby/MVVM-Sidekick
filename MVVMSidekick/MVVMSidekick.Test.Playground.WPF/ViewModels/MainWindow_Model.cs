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

namespace MVVMSidekick.Test.Playground.WPF.ViewModels
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
        protected Property<String> _Title = new Property<String>(_TitleLocator);
        static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator<String>("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
        static Func<String> _TitleDefaultValueFactory = () => "Title is Here";
        #endregion



        #region Life Time Event Handling

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

        ///// <summary>
        ///// This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Load event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewLoad(view);
        //}

        ///// <summary>
        ///// This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's  ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Unload event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewUnload(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewUnload(view);
        //}

        ///// <summary>
        ///// <para>If dispose actions got exceptions, will handled here. </para>
        ///// </summary>
        ///// <param name="exceptions">
        ///// <para>The exception and dispose infomation</para>
        ///// </param>
        //protected override async void OnDisposeExceptions(IList<DisposeInfo> exceptions)
        //{
        //    base.OnDisposeExceptions(exceptions);
        //    await TaskExHelper.Yield();
        //}

        #endregion


        public CommandModel CommandOpenWindows => _CommandOpenWindowsLocator(this).Value;
     
        #region Property CommandModel CommandOpenWindows Setup        

        protected Property<CommandModel> _CommandOpenWindows = new Property<CommandModel>(_CommandOpenWindowsLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandOpenWindowsLocator = RegisterContainerLocator<CommandModel>(nameof(CommandOpenWindows), model => model.Initialize(nameof(CommandOpenWindows), ref model._CommandOpenWindows, ref _CommandOpenWindowsLocator, _CommandOpenWindowsDefaultValueFactory));
        static Func<BindableBase, CommandModel> _CommandOpenWindowsDefaultValueFactory =
            model =>
            {
                object state = nameof(CommandOpenWindows);           // Command state  

                var commandId = nameof(CommandOpenWindows);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            //Todo: Add OpenWindows logic here, or
                            await vm.StageManager.DefaultStage.Show<Window1_Model>();
                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(state);

                cmdmdl.ListenToIsUIBusy(
                    model: vm,
                    canExecuteWhenBusy: false);
                return cmdmdl;
            };

        #endregion


        public CommandModel CommandNavigatePage
        {
            get { return _CommandNavigatePageLocator(this).Value; }
            set { _CommandNavigatePageLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel CommandNavigatePage Setup        

        protected Property<CommandModel> _CommandNavigatePage = new Property<CommandModel>(_CommandNavigatePageLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandNavigatePageLocator = RegisterContainerLocator<CommandModel>(nameof(CommandNavigatePage), model => model.Initialize(nameof(CommandNavigatePage), ref model._CommandNavigatePage, ref _CommandNavigatePageLocator, _CommandNavigatePageDefaultValueFactory));
        static Func<BindableBase, CommandModel> _CommandNavigatePageDefaultValueFactory =
            model =>
            {
                object state = nameof(CommandNavigatePage);           // Command state  

                var commandId = nameof(CommandNavigatePage);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            await vm.StageManager["Frame"].Show<Page1_Model>();

                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(state);

                cmdmdl.ListenToIsUIBusy(
                    model: vm,
                    canExecuteWhenBusy: false);
                return cmdmdl;
            };

        #endregion


        public CommandModel CommandShowCtrol
        {
            get { return _CommandShowCtrolLocator(this).Value; }
            set { _CommandShowCtrolLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel CommandShowCtrol Setup        

        protected Property<CommandModel> _CommandShowCtrol = new Property<CommandModel>(_CommandShowCtrolLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandShowCtrolLocator = RegisterContainerLocator<CommandModel>(nameof(CommandShowCtrol), model => model.Initialize(nameof(CommandShowCtrol), ref model._CommandShowCtrol, ref _CommandShowCtrolLocator, _CommandShowCtrolDefaultValueFactory));
        static Func<BindableBase, CommandModel> _CommandShowCtrolDefaultValueFactory =
            model =>
            {
                object state = nameof(CommandShowCtrol);           // Command state  

                var commandId = nameof(CommandShowCtrol);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            //Todo: Add ShowCtrol logic here, or
                            await vm.StageManager["ContentControl"].Show<Control1_Model>();

                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(state);

                cmdmdl.ListenToIsUIBusy(
                    model: vm,
                    canExecuteWhenBusy: false);
                return cmdmdl;
            };

        #endregion

    }

}


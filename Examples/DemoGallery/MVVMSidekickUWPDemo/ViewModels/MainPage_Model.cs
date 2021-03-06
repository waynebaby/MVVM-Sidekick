﻿using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using MVVMSidekick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MVVMSidekickUWPDemo.ViewModels
{

    [DataContract]
    public class MainPage_Model : ViewModel<MainPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

        public MainPage_Model()
        {
            if (IsInDesignMode)
            {
                Title = "Title is a little different in Design mode";
            }

        }

        //propvm tab tab string tab Title

        public string Title { get => _TitleLocator(this).Value; set => _TitleLocator(this).SetValueAndTryNotify(value); }
        #region Property string Title Setup        
        protected Property<string> _Title = new Property<string>(_TitleLocator);
        static Func<BindableBase, ValueContainer<string>> _TitleLocator = RegisterContainerLocator(nameof(Title), m => m.Initialize(nameof(Title), ref m._Title, ref _TitleLocator, () => "Hello World!"));
        #endregion


        public IStage NavigationFrame => this.StageManager[nameof(NavigationFrame)];


        public CommandModel CommandNaviToCounter => _CommandNaviToCounterLocator(this).Value;
        #region Property CommandModel CommandNaviToCounter Setup                
        protected Property<CommandModel> _CommandNaviToCounter = new Property<CommandModel>(_CommandNaviToCounterLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandNaviToCounterLocator = RegisterContainerLocator(nameof(CommandNaviToCounter), m => m.Initialize(nameof(CommandNaviToCounter), ref m._CommandNaviToCounter, ref _CommandNaviToCounterLocator,
              model =>
              {
                  object state = nameof(CommandNaviToCounter);
                  var commandId = nameof(CommandNaviToCounter);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {
                              await vm.NavigationFrame.Show<Counter_Model>();
                          })
                      .Subscribe()
                      .DisposeWith(vm);

                  var cmdmdl = cmd.CreateCommandModel(state);

                  return cmdmdl;
              }));
        #endregion


        public CommandModel CommandNaviToFetchData => _CommandNaviToFetchDataLocator(this).Value;
        #region Property CommandModel CommandNaviToFetchData Setup                
        protected Property<CommandModel> _CommandNaviToFetchData = new Property<CommandModel>(_CommandNaviToFetchDataLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandNaviToFetchDataLocator = RegisterContainerLocator(nameof(CommandNaviToFetchData), m => m.Initialize(nameof(CommandNaviToFetchData), ref m._CommandNaviToFetchData, ref _CommandNaviToFetchDataLocator,
              model =>
              {
                  object state = nameof(CommandNaviToFetchData);
                  var commandId = nameof(CommandNaviToFetchData);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {
                              await vm.NavigationFrame.Show<FetchData_Model>();
                          })
                      .Subscribe()
                      .DisposeWith(vm);

                  var cmdmdl = cmd.CreateCommandModel(state);

                  return cmdmdl;
              }));
        #endregion



        public CommandModel CommandNaviToLoginDemo => _CommandNaviToLoginDemoLocator(this).Value;
        #region Property CommandModel CommandNaviToLoginDemo Setup                
        protected Property<CommandModel> _CommandNaviToLoginDemo = new Property<CommandModel>(_CommandNaviToLoginDemoLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandNaviToLoginDemoLocator = RegisterContainerLocator(nameof(CommandNaviToLoginDemo), m => m.Initialize(nameof(CommandNaviToLoginDemo), ref m._CommandNaviToLoginDemo, ref _CommandNaviToLoginDemoLocator,
              model =>
              {
                  object state = nameof(CommandNaviToLoginDemo);
                  var commandId = nameof(CommandNaviToLoginDemo);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {
                              await vm.NavigationFrame.Show<LoginDemo_Model>();
                          })
                      .Subscribe()
                      .DisposeWith(vm);

                  var cmdmdl = cmd.CreateCommandModel(state);

                  return cmdmdl;
              }));
        #endregion




        #region Life Time Event Handling

        #region OnBindedToView
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
        #endregion

        #region OnUnbindedFromView
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
        #endregion

        #region OnBindedViewLoad

        ///// <summary>
        ///// This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Load event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewLoad(view);
        //}
        #endregion

        #region OnBindedViewUnload

        ///// <summary>
        ///// This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's  ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Unload event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewUnload(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewUnload(view);
        //}
        #endregion

        #region OnDisposeExceptions

        ///// <summary>
        ///// <para>If dispose actions got exceptions, will handled here. </para>
        ///// </summary>
        ///// <param name="exceptions">
        ///// <para>The exception and dispose information</para>
        ///// </param>
        //protected override async void OnDisposeExceptions(IList<DisposeEntry> exceptions)
        //{
        //    base.OnDisposeExceptions(exceptions);
        //    await Task.Yield();
        //}
        #endregion

        #endregion


    }

}


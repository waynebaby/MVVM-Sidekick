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
using MVVMSidekick;
using Microsoft.Extensions.DependencyInjection;

namespace EventRoutingSample.ViewModels
{

    public class MainWindow_Model : ViewModel<MainWindow_Model>
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

        protected override Task OnBindedToView(IView view, IViewModel oldValue)
        {
            EventRouter.Instance.GetEventChannel<Object>()
    .Where(e =>
        e.EventName == "Global HeartBeat")
    .Subscribe(
        e =>
            LastHeartBeat = e.EventData.ToString()
    )
    .DisposeWhenUnload(this);
            return base.OnBindedToView(view, oldValue);
        }

        public CommandModel CommandNavigateToDisposeBehaviorTest => _CommandNavigateToDisposeBehaviorTestLocator(this).Value;
        #region Property CommandModel CommandNavigateToDisposeBehaviorTest Setup                
        protected Property<CommandModel> _CommandNavigateToDisposeBehaviorTest = new Property<CommandModel>(_CommandNavigateToDisposeBehaviorTestLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandNavigateToDisposeBehaviorTestLocator = RegisterContainerLocator(nameof(CommandNavigateToDisposeBehaviorTest), m => m.Initialize(nameof(CommandNavigateToDisposeBehaviorTest), ref m._CommandNavigateToDisposeBehaviorTest, ref _CommandNavigateToDisposeBehaviorTestLocator,
              model =>
              {
                  object state = nameof(CommandNavigateToDisposeBehaviorTest);
                  var commandId = nameof(CommandNavigateToDisposeBehaviorTest);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {
                              await vm.StageManager.DefaultStage.Show<DisopseTestForBehaviors_Model>(isWaitingForDispose: true);
                              await vm.StageManager.DefaultStage.Show<DisopseTestForBehaviors_Model>();
                              await vm.StageManager.DefaultStage.Show<DisopseTestForBehaviors_Model>();
                          })
                      .DoNotifyDefaultEventRouter(vm, commandId)
                      .Subscribe()
                      .DisposeWith(vm);

                  var cmdmdl = cmd.CreateCommandModel(state);
                  cmdmdl.WireExecutableToViewModelIsUIBusy(vm);
                  return cmdmdl;
              }));
        #endregion



    }

}


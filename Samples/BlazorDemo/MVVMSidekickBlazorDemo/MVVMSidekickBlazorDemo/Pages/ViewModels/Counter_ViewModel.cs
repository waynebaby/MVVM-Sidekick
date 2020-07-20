using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick;
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
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace MVVMSidekickBlazorDemo.Pages.ViewModels
{


    public class Counter_ViewModel : ViewModel<Counter_ViewModel, Counter>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。

        public Counter_ViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }


        public int CurrentCount { get => _CurrentCountLocator(this).Value; set => _CurrentCountLocator(this).SetValueAndTryNotify(value); }
        #region Property int CurrentCount Setup        
        protected Property<int> _CurrentCount = new Property<int>(_CurrentCountLocator);
        static Func<BindableBase, ValueContainer<int>> _CurrentCountLocator = RegisterContainerLocator(nameof(CurrentCount), m => m.Initialize(nameof(CurrentCount), ref m._CurrentCount, ref _CurrentCountLocator, () => default(int)));
        #endregion



        public CommandModel CommandIncrementCount => _CommandIncrementCountLocator(this).Value;
        #region Property CommandModel CommandIncrementCount Setup                
        protected Property<CommandModel> _CommandIncrementCount = new Property<CommandModel>(_CommandIncrementCountLocator);
        static Func<BindableBase, ValueContainer<CommandModel>> _CommandIncrementCountLocator = RegisterContainerLocator(nameof(CommandIncrementCount), m => m.Initialize(nameof(CommandIncrementCount), ref m._CommandIncrementCount, ref _CommandIncrementCountLocator,
              model =>
              {
                  object state = nameof(CommandIncrementCount);
                  var commandId = nameof(CommandIncrementCount);
                  var vm = CastToCurrentType(model);
                  var cmd = new ReactiveCommand(canExecute: true, commandId: commandId) { ViewModel = model };

                  cmd.DoExecuteUIBusyActionTask(
                          vm,
                          async (e, cancelToken) =>
                          {
                              vm.CurrentCount++;
                              await Task.CompletedTask;
                          })
                      .Subscribe()
                      .DisposeWith(vm);

                  var cmdmdl = cmd.CreateCommandModel(state);

                  return cmdmdl;
              }));
        #endregion

    }

    #region ViewModelRegistry
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {

        internal static Action<MVVMSidekickOptions> CounterConfigEntry = AddConfigure(opt => opt.RegisterViewModel<Counter_ViewModel>());
    }
    #endregion 
}


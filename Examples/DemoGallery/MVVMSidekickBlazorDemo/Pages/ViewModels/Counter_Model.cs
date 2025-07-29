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

    /// <summary>
    /// 计数器页面的视图模型
    /// View model for the Counter page
    /// </summary>
    public class Counter_Model : ViewModel<Counter_Model, Counter>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property
        
        /// <summary>
        /// 初始化计数器视图模型的新实例
        /// Initializes a new instance of the Counter view model
        /// </summary>
        /// <param name="serviceProvider">服务提供者 / Service provider</param>
        public Counter_Model(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        /// <summary>
        /// 获取或设置当前计数值
        /// Gets or sets the current count value
        /// </summary>
        public int CurrentCount { get => _CurrentCountLocator(this).Value; set => _CurrentCountLocator(this).SetValueAndTryNotify(value); }
        #region Property int CurrentCount Setup        
        protected Property<int> _CurrentCount = new Property<int>(_CurrentCountLocator);
        static Func<BindableBase, ValueContainer<int>> _CurrentCountLocator = RegisterContainerLocator(nameof(CurrentCount), m => m.Initialize(nameof(CurrentCount), ref m._CurrentCount, ref _CurrentCountLocator, () => default(int)));
        #endregion

        /// <summary>
        /// 获取递增计数的命令模型
        /// Gets the command model for incrementing the count
        /// </summary>
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
    /// <summary>
    /// 视图模型注册表的内部分部类
    /// Internal partial class for view model registry
    /// </summary>
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        /// <summary>
        /// 计数器配置条目的静态操作
        /// Static action for Counter configuration entry
        /// </summary>
        internal static Action<MVVMSidekickOptions> CounterConfigEntry = AddConfigure(opt => opt.RegisterViewModel<Counter_Model>());
    }
    #endregion 
}


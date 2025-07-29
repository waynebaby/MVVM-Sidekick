using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using MVVMSidekickUWPDemo;
using MVVMSidekickUWPDemo.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace MVVMSidekick.Startups
{
    /// <summary>
    /// 视图模型注册器的分部类，用于配置Counter页面的视图和视图模型映射
    /// Partial class of view model registry for configuring view and view model mapping for Counter page
    /// </summary>
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        /// <summary>
        /// Counter页面配置入口，注册Counter视图与Counter_Model视图模型的映射关系
        /// Counter page configuration entry that registers the mapping between Counter view and Counter_Model view model
        /// </summary>
        internal Action<MVVMSidekickOptions> CounterConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<Counter, Counter_Model>());
    }

}

using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using MVVMSidekickWPFDemo;
using MVVMSidekickWPFDemo.ViewModels;
using System;
using System.Net;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace MVVMSidekick.Startups
{
    /// <summary>
    /// 视图模型注册表，包含计数器页面的配置
    /// View model registry containing counter page configuration
    /// </summary>
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        /// <summary>
        /// 计数器配置条目，注册Counter视图和Counter_Model视图模型的映射
        /// Counter configuration entry that registers mapping between Counter view and Counter_Model view model
        /// </summary>
        internal  Action<MVVMSidekickOptions> CounterConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<Counter, Counter_Model>());
    }
}

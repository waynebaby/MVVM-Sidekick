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
    /// 视图模型注册表，包含登录演示的配置
    /// View model registry containing login demo configuration
    /// </summary>
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        /// <summary>
        /// 登录演示配置条目，注册LoginDemo视图和LoginDemo_Model视图模型的映射
        /// Login demo configuration entry that registers mapping between LoginDemo view and LoginDemo_Model view model
        /// </summary>
        internal Action<MVVMSidekickOptions> LoginDemoConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<LoginDemo, LoginDemo_Model>());
    }

}

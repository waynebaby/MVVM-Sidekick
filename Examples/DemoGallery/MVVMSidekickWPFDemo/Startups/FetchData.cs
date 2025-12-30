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
    /// 视图模型注册表，包含数据获取页面的配置
    /// View model registry containing fetch data page configuration
    /// </summary>
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        /// <summary>
        /// 数据获取配置条目，注册FetchData视图和FetchData_Model视图模型的映射
        /// Fetch data configuration entry that registers mapping between FetchData view and FetchData_Model view model
        /// </summary>
        internal Action<MVVMSidekickOptions> FetchDataConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<FetchData, FetchData_Model>());
    }
}

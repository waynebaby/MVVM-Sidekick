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
    /// 视图模型注册器的分部类，用于配置FetchData页面的视图和视图模型映射
    /// Partial class of view model registry for configuring view and view model mapping for FetchData page
    /// </summary>
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        /// <summary>
        /// FetchData页面配置入口，注册FetchData视图与FetchData_Model视图模型的映射关系
        /// FetchData page configuration entry that registers the mapping between FetchData view and FetchData_Model view model
        /// </summary>
        internal Action<MVVMSidekickOptions> FetchDataConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<FetchData, FetchData_Model>());
    }
}

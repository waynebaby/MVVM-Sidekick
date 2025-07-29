using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Services;
using MVVMSidekickUWPDemo;
using MVVMSidekickUWPDemo.ViewModels;

using System.Threading;


namespace MVVMSidekick.Startups
{
    /// <summary>
    /// 视图模型注册表，包含主页面的配置
    /// View model registry containing main page configuration
    /// </summary>
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {

        /// <summary>
        /// 主页面配置条目，注册MainPage视图和MainPage_Model视图模型的映射
        /// Main page configuration entry that registers mapping between MainPage view and MainPage_Model view model
        /// </summary>
        internal  Action<MVVMSidekickOptions> MainPageConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<MainPage, MainPage_Model>());
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Services;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekickWPFDemo;
using MVVMSidekickWPFDemo.ViewModels;

namespace MVVMSidekick.Startups
{
    /// <summary>
    /// 视图模型注册类，负责配置主窗口的视图和模型映射关系
    /// ViewModel registry class responsible for configuring view-model mapping for main window
    /// </summary>
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        /// <summary>
        /// 主窗口配置入口，定义MainWindow与MainWindow_Model的映射关系
        /// Main window configuration entry defining the mapping between MainWindow and MainWindow_Model
        /// </summary>
        internal  Action<MVVMSidekickOptions> MainWindowConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<MainWindow, MainWindow_Model>());
    }
}

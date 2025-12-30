using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventRoutingSample.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick;
using MVVMSidekick.ViewModels;

namespace EventRoutingSample.Startups
{
    /// <summary>
    /// <para>视图模型注册表的部分类，用于配置MainWindow的MVVM映射</para>
    /// <para>Partial class for view model registry, used to configure MVVM mapping for MainWindow</para>
    /// </summary>
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        /// <summary>
        /// <para>MainWindow配置入口，定义视图和视图模型的映射关系</para>
        /// <para>Configuration entry for MainWindow, defines the mapping between view and view model</para>
        /// </summary>
        internal Action<MVVMSidekickOptions> MainWindowConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<MainWindow, MainWindow_Model>());
    }
}

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
    /// <para>视图模型注册表的部分类，用于配置DisopseTestForBehaviors的MVVM映射</para>
    /// <para>Partial class for view model registry, used to configure MVVM mapping for DisopseTestForBehaviors</para>
    /// </summary>
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        /// <summary>
        /// <para>DisopseTestForBehaviors配置入口，定义视图和视图模型的映射关系</para>
        /// <para>Configuration entry for DisopseTestForBehaviors, defines the mapping between view and view model</para>
        /// </summary>
        internal Action<MVVMSidekickOptions> DisopseTestForBehaviorsConfigEntry =
            AddConfigure(opt => opt.RegisterViewAndModelMapping<DisopseTestForBehaviors, DisopseTestForBehaviors_Model>());
    }
}

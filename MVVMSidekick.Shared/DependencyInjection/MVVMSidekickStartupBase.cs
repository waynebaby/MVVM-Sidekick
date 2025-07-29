/// <summary>
/// <para>MVVM-Sidekick启动基类，提供依赖注入和配置的基础功能</para>
/// <para>MVVM-Sidekick startup base class, providing base functionality for dependency injection and configuration</para>
/// </summary>

using MVVMSidekick;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <para>MVVM-Sidekick启动基类，提供配置和初始化的抽象基础</para>
    /// <para>MVVM-Sidekick startup base class, providing abstract foundation for configuration and initialization</para>
    /// </summary>
    public abstract class MVVMSidekickStartupBase
    {
        #if MAUI_BLAZOR
        /// <summary>
        /// <para>初始化MVVMSidekickStartupBase类的新实例，运行类构造函数</para>
        /// <para>Initializes a new instance of the MVVMSidekickStartupBase class, runs class constructor</para>
        /// </summary>
        public MVVMSidekickStartupBase()
        {
            RuntimeHelpers.RunClassConstructor(this.GetType().TypeHandle);
        }
#endif
        /// <summary>
        /// <para>添加配置动作到配置列表</para>
        /// <para>Adds configuration action to configuration list</para>
        /// </summary>
        /// <param name="configure">配置动作委托 / Configuration action delegate</param>
        /// <returns>返回配置动作委托 / Returns configuration action delegate</returns>
        public static Action<MVVMSidekickOptions> AddConfigure(Action<MVVMSidekickOptions> configure)
        {
            ActionList.Add(configure);
            return configure;
        }

        /// <summary>
        /// <para>获取配置动作列表，用于存储所有配置操作</para>
        /// <para>Gets configuration action list, used to store all configuration operations</para>
        /// </summary>
        public static ConcurrentBag<Action<MVVMSidekickOptions>> ActionList { get; } = new ConcurrentBag<Action<MVVMSidekickOptions>>();

        /// <summary>
        /// <para>配置实例方法，执行所有注册的配置操作</para>
        /// <para>Configure instance method, executes all registered configuration operations</para>
        /// </summary>
        /// <param name="opt">MVVM-Sidekick选项 / MVVM-Sidekick options</param>
        virtual public void ConfigureInstance(MVVMSidekickOptions opt)
        {
            foreach (var item in ActionList)
            {
                item?.Invoke(opt);
            }
        }
    }

}
using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// <para>设计时服务接口，用于区分设计时和运行时环境</para>
    /// <para>Design time service interface for distinguishing between design time and runtime environments</para>
    /// </summary>
    public interface ITellDesignTimeService
    {
        /// <summary>
        /// <para>获取当前是否处于设计模式</para>
        /// <para>Gets whether the application is currently in design mode</para>
        /// </summary>
        bool IsInDesignMode
        {
            get;
        }
        
    }
    
    /// <summary>
    /// <para>设计时环境的实现</para>
    /// <para>Implementation for design time environment</para>
    /// </summary>
    public class InDesignTime : ITellDesignTimeService
    {
        /// <summary>
        /// <para>设计时返回 true</para>
        /// <para>Returns true when in design time</para>
        /// </summary>
        public bool IsInDesignMode => true;
    }

    /// <summary>
    /// <para>运行时环境的实现</para>
    /// <para>Implementation for runtime environment</para>
    /// </summary>
    public class InRuntime : ITellDesignTimeService
    {
        /// <summary>
        /// <para>运行时返回 false</para>
        /// <para>Returns false when in runtime</para>
        /// </summary>
        public bool IsInDesignMode => false;
    }
}

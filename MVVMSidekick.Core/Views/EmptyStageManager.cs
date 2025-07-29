using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;





namespace MVVMSidekick.Views
{
    /// <summary>
    /// <para>空舞台管理器实现，提供最基本的舞台管理功能</para>
    /// <para>Empty stage manager implementation that provides basic stage management functionality</para>
    /// </summary>
    public class EmptyStageManager : IStageManager
    {
        /// <summary>
        /// <para>通过信标键获取舞台（总是返回null）</para>
        /// <para>Gets the stage by beacon key (always returns null)</para>
        /// </summary>
        /// <param name="beaconKey">
        /// <para>信标键</para>
        /// <para>The beacon key</para>
        /// </param>
        /// <returns>
        /// <para>总是返回null</para>
        /// <para>Always returns null</para>
        /// </returns>
        public IStage this[string beaconKey]
        {
            get => null;
        }

        /// <summary>
        /// <para>获取或设置当前绑定的视图</para>
        /// <para>Gets or sets the currently bound view</para>
        /// </summary>
        public IView CurrentBindingView
        {
            get; set;
        }

        /// <summary>
        /// <para>获取默认舞台（总是返回null）</para>
        /// <para>Gets the default stage (always returns null)</para>
        /// </summary>
        public IStage DefaultStage
        {
            get => null;

            set { }
        }

        /// <summary>
        /// <para>获取或设置关联的视图模型</para>
        /// <para>Gets or sets the associated view model</para>
        /// </summary>
        public IViewModel ViewModel { get; set; }

        /// <summary>
        /// <para>初始化父级定位器（空实现）</para>
        /// <para>Initializes the parent locator (empty implementation)</para>
        /// </summary>
        /// <param name="parentLocator">
        /// <para>父级定位器函数</para>
        /// <para>The parent locator function</para>
        /// </param>
        public void InitParent(Func<object> parentLocator)
        {

        }
    }
}

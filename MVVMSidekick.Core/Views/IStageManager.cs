using MVVMSidekick.ViewModels;
using System;





namespace MVVMSidekick.Views
{
    /// <summary>
    /// <para>舞台管理器接口，负责管理多个舞台</para>
    /// <para>Interface for stage manager that manages multiple stages</para>
    /// </summary>
    public interface IStageManager
    {
        /// <summary>
        /// <para>通过信标键获取舞台</para>
        /// <para>Gets the stage by beacon key</para>
        /// </summary>
        /// <param name="beaconKey">
        /// <para>信标键</para>
        /// <para>The beacon key</para>
        /// </param>
        /// <returns>
        /// <para>对应的舞台实例</para>
        /// <para>The corresponding stage instance</para>
        /// </returns>
        IStage this[string beaconKey] { get; }
        
        /// <summary>
        /// <para>获取或设置关联的视图模型</para>
        /// <para>Gets or sets the associated view model</para>
        /// </summary>
        IViewModel ViewModel { get; set; }
        
        /// <summary>
        /// <para>获取或设置当前绑定的视图</para>
        /// <para>Gets or sets the currently bound view</para>
        /// </summary>
        IView CurrentBindingView { get; set; }
        
        /// <summary>
        /// <para>获取默认舞台</para>
        /// <para>Gets the default stage</para>
        /// </summary>
        IStage DefaultStage { get; }

        /// <summary>
        /// <para>初始化父级定位器</para>
        /// <para>Initializes the parent locator</para>
        /// </summary>
        /// <param name="parentLocator">
        /// <para>父级定位器函数</para>
        /// <para>The parent locator function</para>
        /// </param>
        void InitParent(Func<object> parentLocator);
    }
}
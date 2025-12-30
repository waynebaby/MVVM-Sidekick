#if BLAZOR
namespace MVVMSidekick.Views
{
    using Microsoft.AspNetCore.Components;
    using MVVMSidekick.ViewModels;
    using System;

    /// <summary>
    /// <para>Blazor舞台管理器，用于框架/内容控件的抽象。VM可以访问此类来显示其他VM和VM映射的视图</para>
    /// <para>Blazor stage manager abstract for frame/contentcontrol. VM can access this class to Show other vm and vm's mapped view</para>
    /// </summary>
    public class BlazorStageManager : IStageManager
    {
        /// <summary>
        /// <para>初始化BlazorStageManager类的新实例</para>
        /// <para>Initializes a new instance of the BlazorStageManager class</para>
        /// </summary>
        /// <param name="stage">舞台实例 / Stage instance</param>
        public BlazorStageManager(IStage stage)
        {
            DefaultStage = stage;
        }
        
        /// <summary>
        /// <para>根据信标键获取舞台</para>
        /// <para>Gets stage by beacon key</para>
        /// </summary>
        /// <param name="beaconKey">信标键 / Beacon key</param>
        /// <returns>舞台实例 / Stage instance</returns>
        public IStage this[string beaconKey] => throw new NotImplementedException();

        /// <summary>
        /// <para>获取或设置视图模型</para>
        /// <para>Gets or sets the view model</para>
        /// </summary>
        public IViewModel ViewModel { get; set; }
        
        /// <summary>
        /// <para>获取或设置当前绑定视图</para>
        /// <para>Gets or sets current binding view</para>
        /// </summary>
        public IView CurrentBindingView { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// <para>获取默认舞台</para>
        /// <para>Gets the default stage</para>
        /// </summary>
        public IStage DefaultStage { get; private set; }

        public void InitParent(Func<object> parentLocator)
        {

        }
    }
}

#endif

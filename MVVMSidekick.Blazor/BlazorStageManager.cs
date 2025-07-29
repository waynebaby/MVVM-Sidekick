
namespace MVVMSidekick.Views
{
    using Microsoft.AspNetCore.Components;
    using MVVMSidekick.ViewModels;
    using System;

    /// <summary>
    /// Blazor舞台管理器，用于框架/内容控制的抽象。VM可以通过此类访问以显示其他VM和映射的视图
    /// The abstract for frame/contentcontrol. VM can access this class to Show other vm and vm's mapped view.
    /// </summary>
    public class BlazorStageManager : IStageManager
    {

        /// <summary>
        /// 初始化BlazorStageManager的新实例
        /// Initializes a new instance of BlazorStageManager
        /// </summary>
        /// <param name="stage">舞台实例 / Stage instance</param>
        public BlazorStageManager(IStage stage)
        {
            DefaultStage = stage;
        }
        
        /// <summary>
        /// 根据信标键获取舞台，暂未实现
        /// Gets stage by beacon key, not implemented yet
        /// </summary>
        /// <param name="beaconKey">信标键 / Beacon key</param>
        /// <returns>舞台实例 / Stage instance</returns>
        public IStage this[string beaconKey] => throw new NotImplementedException();

        /// <summary>
        /// 获取或设置视图模型
        /// Gets or sets the view model
        /// </summary>
        public IViewModel ViewModel { get; set; }
        
        /// <summary>
        /// 获取或设置当前绑定视图，暂未实现
        /// Gets or sets current binding view, not implemented yet
        /// </summary>
        public IView CurrentBindingView { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// 获取默认舞台
        /// Gets the default stage
        /// </summary>
        public IStage DefaultStage { get; private set; }

        /// <summary>
        /// 初始化父对象
        /// Initializes the parent object
        /// </summary>
        /// <param name="parentLocator">父对象定位器 / Parent object locator</param>
        public void InitParent(Func<object> parentLocator)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVVMSidekick.ViewModels;
using System.Reactive.Linq;
using System.Windows;
using System.IO;
using MVVMSidekick.Services;



#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;


#elif WPF
using System.Windows.Controls;
using System.Windows.Media;

using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;
#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif




namespace MVVMSidekick.Views
{
    /// <summary>
    /// 舞台接口，定义视图导航和管理的核心功能
    /// Stage interface that defines core functionality for view navigation and management
    /// </summary>
    public interface IStage
    {
        /// <summary>
        /// 获取信标键，用于标识舞台
        /// Gets the beacon key for identifying the stage
        /// </summary>
        string BeaconKey { get; }
        
        /// <summary>
        /// 获取一个值，该值指示是否可以后退
        /// Gets a value indicating whether can go back
        /// </summary>
        bool CanGoBack { get; }
        
        /// <summary>
        /// 获取一个值，该值指示是否可以前进
        /// Gets a value indicating whether can go forward
        /// </summary>
        bool CanGoForward { get; }

        /// <summary>
        /// 获取一个值，该值指示是否支持后退操作
        /// Gets a value indicating whether go back operation is supported
        /// </summary>
        bool IsGoBackSupported { get; }
        
        /// <summary>
        /// 获取一个值，该值指示是否支持前进操作
        /// Gets a value indicating whether go forward operation is supported
        /// </summary>
        bool IsGoForwardSupported { get; }
        
        /// <summary>
        /// 获取目标对象
        /// Gets the target object
        /// </summary>
        Object Target { get; }

        /// <summary>
        /// 显示指定类型的视图模型
        /// Shows a view model of the specified type
        /// </summary>
        /// <typeparam name="TTarget">视图模型类型 / The view model type</typeparam>
        /// <param name="viewMappingKey">视图映射键 / The view mapping key</param>
        /// <param name="additionalViewModelConfig">额外的视图模型配置 / Additional view model configuration</param>
        /// <param name="isWaitingForDispose">是否等待释放 / Whether to wait for disposal</param>
        /// <param name="autoDisposeWhenViewUnload">视图卸载时是否自动释放 / Whether to auto dispose when view unloads</param>
        /// <returns>视图模型实例的任务 / Task containing the view model instance</returns>
        Task<TTarget> Show<TTarget>(
            string viewMappingKey = null, 
            Action<(IServiceProvider serviceProvider, TTarget viewModel)> additionalViewModelConfig = null, 
            bool isWaitingForDispose = false,
            bool autoDisposeWhenViewUnload=true) where TTarget : class, IViewModel;
    }



}
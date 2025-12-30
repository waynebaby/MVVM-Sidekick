using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using MVVMSidekick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace MVVMSidekickBlazorDemo.Pages.ViewModels
{

    /// <summary>
    /// 首页的视图模型
    /// View model for the Index page
    /// </summary>
    public class Index_Model : ViewModel<Index_Model, Index>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。

        /// <summary>
        /// 初始化首页视图模型的新实例
        /// Initializes a new instance of the Index view model
        /// </summary>
        /// <param name="serviceProvider">服务提供者 / Service provider</param>
        public Index_Model(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// 获取或设置页面标题
        /// Gets or sets the page title
        /// </summary>
        public string Title { get => _TitleLocator(this).Value; set => _TitleLocator(this).SetValueAndTryNotify(value); }
        #region Property string Title Setup        
        protected Property<string> _Title = new Property<string>(_TitleLocator);
        static Func<BindableBase, ValueContainer<string>> _TitleLocator = RegisterContainerLocator(nameof(Title), m => m.Initialize(nameof(Title), ref m._Title, ref _TitleLocator, () => default(string)));
        #endregion
    }

    #region ViewModelRegistry
    /// <summary>
    /// 视图模型注册表的内部分部类
    /// Internal partial class for view model registry
    /// </summary>
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        /// <summary>
        /// 首页配置条目的静态操作
        /// Static action for Index configuration entry
        /// </summary>
        internal static Action<MVVMSidekickOptions> IndexConfigEntry = AddConfigure(opt => opt.RegisterViewModel<Index_Model>());
    }
    #endregion 
}


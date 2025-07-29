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
    /// 参数演示页面的视图模型
    /// View model for the ParameterDemo page
    /// </summary>
    public class ParameterDemo_Model : ViewModel<ParameterDemo_Model, ParameterDemo>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。

        /// <summary>
        /// 初始化参数演示视图模型的新实例
        /// Initializes a new instance of the ParameterDemo view model
        /// </summary>
        /// <param name="serviceProvider">服务提供者 / Service provider</param>
        public ParameterDemo_Model(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// 渲染完成后的回调方法
        /// Callback method after rendering is complete
        /// </summary>
        /// <param name="firstRender">是否为首次渲染 / Whether it's the first render</param>
        public override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }
        
        /// <summary>
        /// 初始化时的回调方法
        /// Callback method during initialization
        /// </summary>
        public override void OnInitialized()
        {
            base.OnInitialized();
        }
        
        /// <summary>
        /// 获取或设置参数1
        /// Gets or sets parameter 1
        /// </summary>
        public string P1 { get => _P1Locator(this).Value; set => _P1Locator(this).SetValueAndTryNotify(value); }
        #region Property string P1 Setup        
        protected Property<string> _P1 = new Property<string>(_P1Locator);
        static Func<BindableBase, ValueContainer<string>> _P1Locator = RegisterContainerLocator(nameof(P1), m => m.Initialize(nameof(P1), ref m._P1, ref _P1Locator, () => default(string)));
        #endregion

        /// <summary>
        /// 获取或设置参数2
        /// Gets or sets parameter 2
        /// </summary>
        public string P2 { get => _P2Locator(this).Value; set => _P2Locator(this).SetValueAndTryNotify(value); }
        #region Property string P2 Setup        
        protected Property<string> _P2 = new Property<string>(_P2Locator);
        static Func<BindableBase, ValueContainer<string>> _P2Locator = RegisterContainerLocator(nameof(P2), m => m.Initialize(nameof(P2), ref m._P2, ref _P2Locator, () => default(string)));
        #endregion

        /// <summary>
        /// 获取或设置参数3
        /// Gets or sets parameter 3
        /// </summary>
        public string P3 { get => _P3Locator(this).Value; set => _P3Locator(this).SetValueAndTryNotify(value); }
        #region Property string P3 Setup        
        protected Property<string> _P3 = new Property<string>(_P3Locator);
        static Func<BindableBase, ValueContainer<string>> _P3Locator = RegisterContainerLocator(nameof(P3), m => m.Initialize(nameof(P3), ref m._P3, ref _P3Locator, () => default(string)));
        #endregion

        /// <summary>
        /// 获取或设置具有不同名称的属性
        /// Gets or sets the property with a different name
        /// </summary>
        public string DifferentNamedProperty { get => _DifferentNamedPropertyLocator(this).Value; set => _DifferentNamedPropertyLocator(this).SetValueAndTryNotify(value); }
        #region Property string DifferentNamedProperty Setup        
        protected Property<string> _DifferentNamedProperty = new Property<string>(_DifferentNamedPropertyLocator);
        static Func<BindableBase, ValueContainer<string>> _DifferentNamedPropertyLocator = RegisterContainerLocator(nameof(DifferentNamedProperty), m => m.Initialize(nameof(DifferentNamedProperty), ref m._DifferentNamedProperty, ref _DifferentNamedPropertyLocator, () => default(string)));
        #endregion

        /// <summary>
        /// 获取或设置可赋值类型的属性
        /// Gets or sets the assignable typed property
        /// </summary>
        public long AssignableTypedProperty { get => _AssignableTypedPropertyLocator(this).Value; set => _AssignableTypedPropertyLocator(this).SetValueAndTryNotify(value); }
        #region Property long AssignableTypedProperty Setup        
        protected Property<long> _AssignableTypedProperty = new Property<long>(_AssignableTypedPropertyLocator);
        static Func<BindableBase, ValueContainer<long>> _AssignableTypedPropertyLocator = RegisterContainerLocator(nameof(AssignableTypedProperty), m => m.Initialize(nameof(AssignableTypedProperty), ref m._AssignableTypedProperty, ref _AssignableTypedPropertyLocator, () => default(long)));
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
        /// 参数演示配置条目的静态操作
        /// Static action for ParameterDemo configuration entry
        /// </summary>
        internal static Action<MVVMSidekickOptions> ParameterDemoConfigEntry = AddConfigure(opt => opt.RegisterViewModel<ParameterDemo_Model>());
    }
    #endregion 
}


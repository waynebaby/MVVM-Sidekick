
using MVVMSidekickUWPDemo.ViewModels;
using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MVVMSidekickUWPDemo
{
    /// <summary>
    /// 数据获取页面，用于演示MVVM-Sidekick框架在UWP中的数据获取功能
    /// Data fetching page that demonstrates MVVM-Sidekick framework data fetching capabilities in UWP
    /// </summary>
    public sealed partial class FetchData : Page
    {



        /// <summary>
        /// 初始化FetchData页面实例
        /// Initializes a new instance of the FetchData page
        /// </summary>
        public FetchData()
        {
            this.InitializeComponent();
            ViewDisguise.RegisterPropertyChangedCallback(PageViewDisguise.ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = ViewDisguise.ViewModel as FetchData_Model;
            });
            StrongTypeViewModel = ViewDisguise.ViewModel as FetchData_Model;
        }

        /// <summary>
        /// 获取或设置强类型的FetchData视图模型
        /// Gets or sets the strongly typed FetchData view model
        /// </summary>
        public FetchData_Model StrongTypeViewModel
        {
            get { return (FetchData_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        /// <summary>
        /// StrongTypeViewModel属性的依赖属性定义
        /// Dependency property definition for the StrongTypeViewModel property
        /// </summary>
        public static readonly DependencyProperty StrongTypeViewModelProperty =
        DependencyProperty.Register(nameof(StrongTypeViewModel), typeof(FetchData_Model), typeof(FetchData), new PropertyMetadata(null));


        #region IView Disguise
        /// <summary>
        /// 页面视图伪装，用于MVVM-Sidekick框架的视图生命周期管理
        /// Page view disguise for MVVM-Sidekick framework view lifecycle management
        /// </summary>
        PageViewDisguise ViewDisguise { get { return this.GetOrCreateViewDisguise(); } }
        #endregion

        /// <summary>
        /// 当页面被导航到时调用
        /// Called when the page is navigated to
        /// </summary>
        /// <param name="e">导航事件参数 / Navigation event arguments</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewDisguise.OnNavigatedTo(e);
        }

        /// <summary>
        /// 当页面导航离开时调用
        /// Called when the page is navigated away from
        /// </summary>
        /// <param name="e">导航事件参数 / Navigation event arguments</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewDisguise.OnNavigatedFrom(e);
            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// 当页面即将导航离开时调用
        /// Called when the page is about to navigate away
        /// </summary>
        /// <param name="e">导航取消事件参数 / Navigation canceling event arguments</param>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewDisguise.OnNavigatingFrom(e);
            base.OnNavigatingFrom(e);
        }


    }
}

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
using MVVMSidekickUWPDemo.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.Views;
using MVVMSidekick.Services;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MVVMSidekickUWPDemo
{
    /// <summary>
    /// 应用程序主页，作为UWP应用的入口页面，演示MVVM-Sidekick框架的导航功能
    /// Main page of the application, serves as the entry page for UWP app and demonstrates MVVM-Sidekick framework navigation features
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// 初始化MainPage实例，设置视图模型和属性变更回调
        /// Initializes MainPage instance, sets up view model and property change callbacks
        /// </summary>
        public MainPage()
        {
            ViewDisguise.ViewModel = ServiceProviderLocator.RootServiceProvider.GetRequiredService<MainPage_Model>();
            this.InitializeComponent();
            ViewDisguise.RegisterPropertyChangedCallback(
                PageViewDisguise.ViewModelProperty, 
                (_, __) =>
                {
                    StrongTypeViewModel = ViewDisguise.ViewModel as MainPage_Model;
                });
        }

        /// <summary>
        /// 获取或设置强类型的MainPage视图模型
        /// Gets or sets the strongly typed MainPage view model
        /// </summary>
        public MainPage_Model StrongTypeViewModel
        {
            get { return (MainPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        /// <summary>
        /// StrongTypeViewModel属性的依赖属性定义
        /// Dependency property definition for the StrongTypeViewModel property
        /// </summary>
        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register(nameof(StrongTypeViewModel), typeof(MainPage_Model), typeof(MainPage), new PropertyMetadata(null));


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

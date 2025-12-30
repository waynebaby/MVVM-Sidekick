/// <summary>
/// <para>窗口视图伪装文件，提供WPF平台的窗口视图伪装实现</para>
/// <para>Window view disguise file that provides window view disguise implementation for WPF platform</para>
/// </summary>
/// <remarks>
/// <para>这个文件包含WindowViewDisguise类，用于为WPF Window控件提供MVVM视图伪装功能</para>
/// <para>This file contains the WindowViewDisguise class for providing MVVM view disguise functionality for WPF Window controls</para>
/// <para>仅在WPF平台可用，支持窗口的视图模型绑定和生命周期管理</para>
/// <para>Available only on WPF platform, supports view model binding and lifecycle management for windows</para>
/// </remarks>



#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;


#elif WPF
using System.Windows.Controls;
using System.Windows;
using MVVMSidekick.ViewModels;
using System;
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

#if WPF
    /// <summary>
    /// <para>窗口视图伪装类，为WPF窗口提供MVVM视图模型绑定和管理功能</para>
    /// <para>Window view disguise class that provides MVVM view model binding and management functionality for WPF windows</para>
    /// </summary>
    /// <remarks>
    /// <para>继承自ViewDisguiseBase，实现IWindowView接口，专门用于WPF Window控件</para>
    /// <para>Inherits from ViewDisguiseBase and implements IWindowView interface, specifically designed for WPF Window controls</para>
    /// <para>提供窗口生命周期管理、自动Owner设置等功能</para>
    /// <para>Provides window lifecycle management, automatic Owner setting, and other features</para>
    /// </remarks>
    public class WindowViewDisguise : ViewDisguiseBase<Window, WindowViewDisguise>, IWindowView
    {
        /// <summary>
        /// <para>初始化WindowViewDisguise实例，设置窗口生命周期回调</para>
        /// <para>Initializes a new instance of WindowViewDisguise and sets up window lifecycle callbacks</para>
        /// </summary>
        /// <param name="assocatedObject">
        /// <para>关联的窗口对象，不能为null</para>
        /// <para>The associated window object, cannot be null</para>
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <para>当关联对象为null时抛出</para>
        /// <para>Thrown when the associated object is null</para>
        /// </exception>
        public WindowViewDisguise(Window assocatedObject) : base(assocatedObject)
        {
            if (assocatedObject is null)
            {
                throw new InvalidOperationException();
            }
            assocatedObject.Loaded += ViewHelper.ViewLoadCallBack;
            assocatedObject.Unloaded += ViewHelper.ViewUnloadCallBack;

        }
        
        /// <summary>
        /// <para>获取或设置是否需要自动设置窗口Owner，如果为true，将窗口的Owner设置为父视图窗口</para>
        /// <para>Gets or sets whether auto owner setting is needed. If true, sets window's owner to parent view window</para>
        /// </summary>
        /// <value>
        /// <para>指示是否自动设置Owner的布尔值</para>
        /// <para>Boolean value indicating whether to automatically set Owner</para>
        /// </value>
        /// <remarks>
        /// <para>Is auto owner set needed. if true, set window's owner to parent view window.</para>
        /// </remarks>
        public bool IsAutoOwnerSetNeeded
        {
            get { return (bool)GetValue(IsAutoOwnerSetNeededProperty); }
            set { SetValue(IsAutoOwnerSetNeededProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAutoOwnerSetNeeded.  This enables animation, styling, binding, etc...

        /// <summary>
        /// <para>IsAutoOwnerSetNeeded依赖属性，支持动画、样式、绑定等功能</para>
        /// <para>Dependency property for IsAutoOwnerSetNeeded that enables animation, styling, binding, etc.</para>
        /// </summary>
        /// <remarks>
        /// <para>Is auto owner set needed property</para>
        /// </remarks>
        public static readonly DependencyProperty IsAutoOwnerSetNeededProperty =
            DependencyProperty.Register(nameof(IsAutoOwnerSetNeeded), typeof(bool), typeof(WindowViewDisguise), new PropertyMetadata(true));

        /// <summary>
        /// <para>重写属性变更处理方法，当视图模型属性变更时进行特殊处理</para>
        /// <para>Overrides property changed handling method to perform special processing when view model property changes</para>
        /// </summary>
        /// <param name="e">
        /// <para>依赖属性变更事件参数</para>
        /// <para>Dependency property changed event arguments</para>
        /// </param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == ViewModelProperty)
            {
                var vm = e.NewValue as IViewModel;
                if (vm != null)
                {
                    vm.IsDisposingWhenUnloadRequired = true;
                }
            }
            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// <para>获取或设置视图内容对象，对应窗口的Content属性</para>
        /// <para>Gets or sets the view content object, corresponding to the window's Content property</para>
        /// </summary>
        /// <value>
        /// <para>窗口的内容对象</para>
        /// <para>The content object of the window</para>
        /// </value>
        public override object ViewContentObject
        {
            get { return base.AssocatedObject.Content; }
            set { AssocatedObject.Content = value; }
        }

        /// <summary>
        /// <para>获取父对象，返回窗口的Parent属性</para>
        /// <para>Gets the parent object, returning the window's Parent property</para>
        /// </summary>
        /// <value>
        /// <para>窗口的父对象</para>
        /// <para>The parent object of the window</para>
        /// </value>
        public override object Parent
        {
            get
            {

                return this.AssocatedObject.Parent;

            }
        }
    }
#endif
}

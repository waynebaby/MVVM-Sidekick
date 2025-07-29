#if !BLAZOR


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
#elif WinUI3
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;

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
    /// <para>视图伪装基类，为特定视图元素类型提供视图模型绑定和管理功能</para>
    /// <para>Base class for view disguise that provides view model binding and management functionality for specific view element types</para>
    /// </summary>
    /// <typeparam name="TViewElement">
    /// <para>关联的视图元素类型，必须继承自FrameworkElement</para>
    /// <para>The associated view element type that must inherit from FrameworkElement</para>
    /// </typeparam>
    /// <typeparam name="TViewDisguise">
    /// <para>视图伪装类型，必须继承自ViewDisguiseBase</para>
    /// <para>The view disguise type that must inherit from ViewDisguiseBase</para>
    /// </typeparam>
    public abstract class ViewDisguiseBase<TViewElement, TViewDisguise> : DependencyObject, IViewDisguise
        where TViewElement : FrameworkElement
        where TViewDisguise : ViewDisguiseBase<TViewElement, TViewDisguise>
    {
        /// <summary>
        /// <para>初始化ViewDisguiseBase实例</para>
        /// <para>Initializes a new instance of ViewDisguiseBase</para>
        /// </summary>
        /// <param name="assocatedObject">
        /// <para>关联的视图元素对象</para>
        /// <para>The associated view element object</para>
        /// </param>
        public ViewDisguiseBase(TViewElement assocatedObject)
        {
            _assocatedObject = assocatedObject;
        }
        
        /// <summary>
        /// <para>获取关联的视图元素对象</para>
        /// <para>Gets the associated view element object</para>
        /// </summary>
        /// <value>
        /// <para>关联的视图元素对象</para>
        /// <para>The associated view element object</para>
        /// </value>
        public TViewElement AssocatedObject { get => _assocatedObject; }
        
        /// <summary>
        /// <para>关联的视图元素对象私有字段</para>
        /// <para>Private field for the associated view element object</para>
        /// </summary>
        TViewElement _assocatedObject;

        /// <summary>
        /// <para>获取或设置视图模型，自动处理DataContext绑定</para>
        /// <para>Gets or sets the view model and automatically handles DataContext binding</para>
        /// </summary>
        /// <value>
        /// <para>与平台服务相关联的视图模型实例</para>
        /// <para>The view model instance associated with platform services</para>
        /// </value>
        public IViewModelWithPlatformService ViewModel
        {
            get
            {
                var vm = GetValue(ViewModelProperty) as IViewModelWithPlatformService;
                var content = this.GetContentAndCreateIfNull();
                if (vm == null)
                {

                    vm = content.DataContext as IViewModelWithPlatformService;
                    SetValue(ViewModelProperty, vm);

                }
                else
                {
                    IView view = this;


                    if (!Object.ReferenceEquals(content.DataContext, vm))
                    {

                        content.DataContext = vm;
                    }
                }
                return vm;
            }
            set
            {
                SetValue(ViewModelProperty, value);
                var c = this.GetContentAndCreateIfNull();
                if (!Object.ReferenceEquals(c.DataContext, value))
                {
                    c.DataContext = value;
                }

            }
        }

        /// <summary>
        /// <para>视图模型依赖属性，用于支持数据绑定和变更通知</para>
        /// <para>Dependency property for view model that supports data binding and change notifications</para>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(IViewModel), typeof(TViewDisguise), new PropertyMetadata(null, ViewHelper.ViewModelChangedCallback));

        /// <summary>
        /// <para>获取或设置视图内容对象的抽象属性</para>
        /// <para>Abstract property to get or set the view content object</para>
        /// </summary>
        /// <value>
        /// <para>视图内容对象</para>
        /// <para>The view content object</para>
        /// </value>
        public abstract object ViewContentObject { get; set; }
        
        /// <summary>
        /// <para>获取父对象的抽象属性</para>
        /// <para>Abstract property to get the parent object</para>
        /// </summary>
        /// <value>
        /// <para>父对象实例</para>
        /// <para>The parent object instance</para>
        /// </value>
        public abstract object Parent { get; }

        /// <summary>
        /// <para>获取视图对象，返回关联的视图元素</para>
        /// <para>Gets the view object, returning the associated view element</para>
        /// </summary>
        /// <value>
        /// <para>关联的视图元素对象</para>
        /// <para>The associated view element object</para>
        /// </value>
        public object ViewObject => _assocatedObject;
        
#pragma warning disable CA1033 // Interface methods should be callable by child types
        /// <summary>
        /// <para>实现IView接口的SelfClose方法，执行视图自关闭操作</para>
        /// <para>Implements the SelfClose method of IView interface to perform view self-closing operation</para>
        /// </summary>
        void  IView.SelfClose()
#pragma warning restore CA1033 // Interface methods should be callable by child types
        {
            this.SelfClose();
        }
        
        /// <summary>
        /// <para>实现IView接口的ViewModel属性，提供类型转换</para>
        /// <para>Implements the ViewModel property of IView interface with type conversion</para>
        /// </summary>
        IViewModel IView.ViewModel { get => ViewModel; set => ViewModel = (IViewModelWithPlatformService)value; }

    }
}
#endif
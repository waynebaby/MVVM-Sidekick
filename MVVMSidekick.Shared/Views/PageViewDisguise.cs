

#if !BLAZOR

#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;


#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
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
    /// <para>页面视图伪装类，为Page控件提供IPageView接口的包装实现</para>
    /// <para>Page view disguise class that provides IPageView interface wrapper implementation for Page control</para>
    /// </summary>
    /// <remarks>
    /// <para>此类继承自ViewDisguiseBase，为Page控件提供MVVM模式的视图功能，包括页面导航、内容管理和生命周期事件处理</para>
    /// <para>This class inherits from ViewDisguiseBase and provides MVVM pattern view functionality for Page control, including page navigation, content management, and lifecycle event handling</para>
    /// </remarks>
    public class PageViewDisguise : ViewDisguiseBase<Page, PageViewDisguise>, IPageView
    {
        /// <summary>
        /// <para>初始化PageViewDisguise的新实例</para>
        /// <para>Initializes a new instance of PageViewDisguise</para>
        /// </summary>
        /// <param name="assocatedObject">
        /// <para>关联的Page对象</para>
        /// <para>Associated Page object</para>
        /// </param>
        public PageViewDisguise(Page assocatedObject) : base(assocatedObject)
        {
#if WPF
            AssocatedObject.Loaded += ViewHelper.ViewLoadCallBack;
            AssocatedObject.Unloaded += ViewHelper.ViewUnloadCallBack;
#endif

        }

        /// <summary>
        /// <para>获取或设置视图内容对象，用于管理Page的Content属性</para>
        /// <para>Gets or sets the view content object for managing Page's Content property</para>
        /// </summary>
        /// <value>
        /// <para>视图内容对象，根据平台类型进行相应转换</para>
        /// <para>View content object, converted according to platform type</para>
        /// </value>
        public override object ViewContentObject
        {
            get { return base.AssocatedObject.Content; }
            set
            {
#if WINDOWS_UWP || WinUI3
                AssocatedObject.Content = value as UIElement;
#elif WPF
                AssocatedObject.Content = value;
#endif
            }
        }

#if WPF

        public  override object Parent
        {
            get
            {
                return FrameObject;
            }
        }
        /// <summary>
        /// Frame of this view
        /// </summary>

        public object FrameObject
        {
            get { return GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }


        // Using a DependencyProperty as the backing store for Frame.  This enables animation, styling, binding, etc...
        /// <summary>
        /// Frame Property
        /// </summary>
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register(nameof(FrameObject), typeof(object), typeof(PageViewDisguise), new PropertyMetadata(null));
#elif WINDOWS_UWP || WinUI3
        public override object Parent
        {
            get
            {

                return this.AssocatedObject.Parent ?? this.AssocatedObject.Frame;

            }
        }
#endif

#if !WPF

        //WPF navigates page instances but other navgates with parameters
        /// <summary>
        /// Handles the <see cref="NavigatedTo" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs" /> instance containing the event data.</param>
        public virtual void OnNavigatedTo(NavigationEventArgs e)
        {
            RoutedEventHandler loadEvent = null;

            loadEvent = (_1, _2) =>
            {
                EventRouting.EventRouter.Instance.RaiseEvent(this, e);  //VM Is Ready after this
                AssocatedObject.Loaded -= loadEvent;

            };



            AssocatedObject.Loaded += loadEvent;
            AssocatedObject.Loaded += ViewHelper.ViewLoadCallBack;
            AssocatedObject.Unloaded += ViewHelper.ViewUnloadCallBack;

        }
        /// <summary>
        /// Handles the <see cref="NavigatedFrom" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs" /> instance containing the event data.</param>
        public virtual void OnNavigatedFrom(NavigationEventArgs e)
        {


            if (e?.NavigationMode == NavigationMode.Back)

            {

                if (ViewModel != null)
                {
                    ViewModel.OnPageNavigatedFrom(e);
                    ViewModel.Dispose();
                }


            }

        }
        public virtual void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.OnPageNavigatingFrom(e);

        }

#endif


    }
}
#endif
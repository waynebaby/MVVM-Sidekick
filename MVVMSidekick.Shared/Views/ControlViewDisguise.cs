
#if !BLAZOR


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
    /// <para>用户控件视图伪装类，为UserControl提供IView接口的包装实现</para>
    /// <para>User control view disguise class that provides IView interface wrapper implementation for UserControl</para>
    /// </summary>
    /// <remarks>
    /// <para>此类继承自ViewDisguiseBase，为UserControl控件提供MVVM模式的视图功能，包括内容管理和父级访问</para>
    /// <para>This class inherits from ViewDisguiseBase and provides MVVM pattern view functionality for UserControl, including content management and parent access</para>
    /// </remarks>
    public class ControlViewDisguise : ViewDisguiseBase<UserControl, ControlViewDisguise>, IControlView
    {
        /// <summary>
        /// <para>初始化ControlViewDisguise的新实例</para>
        /// <para>Initializes a new instance of ControlViewDisguise</para>
        /// </summary>
        /// <param name="assocatedObject">
        /// <para>关联的UserControl对象</para>
        /// <para>Associated UserControl object</para>
        /// </param>
        public ControlViewDisguise(UserControl assocatedObject) : base(assocatedObject)
        {
        }
        
        /// <summary>
        /// <para>获取或设置视图内容对象，用于管理UserControl的Content属性</para>
        /// <para>Gets or sets the view content object for managing UserControl's Content property</para>
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
#if WINDOWS_UWP
                AssocatedObject.Content = value as UIElement;
#elif WPF
                AssocatedObject.Content = value;
#endif
            }
        }

        /// <summary>
        /// <para>获取关联UserControl的父级对象</para>
        /// <para>Gets the parent object of the associated UserControl</para>
        /// </summary>
        /// <value>
        /// <para>父级对象实例</para>
        /// <para>Parent object instance</para>
        /// </value>
        public override object Parent
        {
            get
            {

                return this.AssocatedObject.Parent;

            }
        }
    }
}
#endif
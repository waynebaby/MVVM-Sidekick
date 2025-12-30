#if !BLAZOR


namespace MVVMSidekick.Views
{
    /// <summary>
    /// <para>视图伪装管理器，提供视图伪装对象的创建、获取和设置功能</para>
    /// <para>View disguise manager that provides functionality for creating, getting, and setting view disguise objects</para>
    /// </summary>
    public static class ViewDisguiseManager
    {

        /// <summary>
        /// <para>获取依赖对象的视图伪装对象</para>
        /// <para>Gets the view disguise object of the dependency object</para>
        /// </summary>
        /// <param name="dobj">
        /// <para>目标依赖对象</para>
        /// <para>The target dependency object</para>
        /// </param>
        /// <returns>
        /// <para>视图伪装对象，如果未设置则返回null</para>
        /// <para>The view disguise object, or null if not set</para>
        /// </returns>
        public static IViewDisguise GetViewDisguise(this DependencyObject dobj)
        {
            return (IViewDisguise)dobj?.GetValue(ViewDisguiseProperty);
        }

        /// <summary>
        /// <para>设置依赖对象的视图伪装对象</para>
        /// <para>Sets the view disguise object for the dependency object</para>
        /// </summary>
        /// <param name="dobj">
        /// <para>目标依赖对象</para>
        /// <para>The target dependency object</para>
        /// </param>
        /// <param name="value">
        /// <para>要设置的视图伪装对象</para>
        /// <para>The view disguise object to set</para>
        /// </param>
        public static void SetViewDisguise(this DependencyObject dobj, IViewDisguise value)
        {
            dobj?.SetValue(ViewDisguiseProperty, value);
        }

        /// <summary>
        /// <para>获取或创建依赖对象的视图伪装对象，如果不存在则根据对象类型自动创建</para>
        /// <para>Gets or creates the view disguise object for the dependency object, automatically creating one based on object type if it doesn't exist</para>
        /// </summary>
        /// <param name="dobj">
        /// <para>目标依赖对象，支持Page、UserControl和Window（仅WPF）类型</para>
        /// <para>The target dependency object, supporting Page, UserControl, and Window (WPF only) types</para>
        /// </param>
        /// <returns>
        /// <para>视图伪装对象</para>
        /// <para>The view disguise object</para>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// <para>当提供的视图类型不受支持时抛出</para>
        /// <para>Thrown when the provided view type is not supported</para>
        /// </exception>
        public static IViewDisguise GetOrCreateViewDisguise(this DependencyObject dobj)
        {
            var dis = (IViewDisguise)dobj?.GetValue(ViewDisguiseProperty);
            if (dis == null)
            {
                switch (dobj)
                {
                    case Page p:
                        dis = p.GetOrCreateViewDisguise();
                        break;

                    case UserControl c:
                        dis = c.GetOrCreateViewDisguise();
                        break;

#if WPF
                    case Window w:
                        dis = w.GetOrCreateViewDisguise();
                        break;
#endif
                    default:
                        throw new InvalidOperationException($"This kind of view ({ dobj?.GetType()?.Name ?? "null"}) is not supported");
                }
                dobj.SetValue(ViewDisguiseProperty, dis);
            }
            return dis;
        }

        /// <summary>
        /// <para>获取或创建Page对象的页面视图伪装对象</para>
        /// <para>Gets or creates the page view disguise object for a Page object</para>
        /// </summary>
        /// <param name="dobj">
        /// <para>目标Page对象</para>
        /// <para>The target Page object</para>
        /// </param>
        /// <returns>
        /// <para>页面视图伪装对象</para>
        /// <para>The page view disguise object</para>
        /// </returns>
        public static PageViewDisguise GetOrCreateViewDisguise(this Page dobj)
        {
            var dis = (PageViewDisguise)dobj?.GetValue(ViewDisguiseProperty);
            if (dis == null)
            {
                dis = new PageViewDisguise(dobj);
                dobj.SetValue(ViewDisguiseProperty, dis);
            }
            return dis;
        }

        /// <summary>
        /// <para>设置Page对象的页面视图伪装对象</para>
        /// <para>Sets the page view disguise object for a Page object</para>
        /// </summary>
        /// <param name="dobj">
        /// <para>目标Page对象</para>
        /// <para>The target Page object</para>
        /// </param>
        /// <param name="value">
        /// <para>要设置的页面视图伪装对象</para>
        /// <para>The page view disguise object to set</para>
        /// </param>
        public static void SetViewDisguise(this Page dobj, PageViewDisguise value)
        {
            dobj?.SetValue(ViewDisguiseProperty, value);
        }

        /// <summary>
        /// <para>获取或创建UserControl对象的控件视图伪装对象</para>
        /// <para>Gets or creates the control view disguise object for a UserControl object</para>
        /// </summary>
        /// <param name="dobj">
        /// <para>目标UserControl对象</para>
        /// <para>The target UserControl object</para>
        /// </param>
        /// <returns>
        /// <para>控件视图伪装对象</para>
        /// <para>The control view disguise object</para>
        /// </returns>
        public static ControlViewDisguise GetOrCreateViewDisguise(this UserControl dobj)
        {
            var dis = (ControlViewDisguise)dobj?.GetValue(ViewDisguiseProperty);
            if (dis == null)
            {
                dis = new ControlViewDisguise(dobj);
                dobj.SetValue(ViewDisguiseProperty, dis);
            }
            return dis;
        }

        /// <summary>
        /// <para>设置UserControl对象的页面视图伪装对象</para>
        /// <para>Sets the page view disguise object for a UserControl object</para>
        /// </summary>
        /// <param name="dobj">
        /// <para>目标UserControl对象</para>
        /// <para>The target UserControl object</para>
        /// </param>
        /// <param name="value">
        /// <para>要设置的页面视图伪装对象</para>
        /// <para>The page view disguise object to set</para>
        /// </param>
        public static void SetViewDisguise(this UserControl dobj, PageViewDisguise value)
        {
            dobj?.SetValue(ViewDisguiseProperty, value);
        }

#if WPF
        /// <summary>
        /// <para>获取或创建Window对象的窗口视图伪装对象（仅WPF平台）</para>
        /// <para>Gets or creates the window view disguise object for a Window object (WPF platform only)</para>
        /// </summary>
        /// <param name="dobj">
        /// <para>目标Window对象</para>
        /// <para>The target Window object</para>
        /// </param>
        /// <returns>
        /// <para>窗口视图伪装对象</para>
        /// <para>The window view disguise object</para>
        /// </returns>
        public static WindowViewDisguise GetOrCreateViewDisguise(this Window dobj)
        {
            var dis = (WindowViewDisguise)dobj?.GetValue(ViewDisguiseProperty);
            if (dis == null)
            {
                dis = new WindowViewDisguise(dobj);
                dobj?.SetValue(ViewDisguiseProperty, dis);
            }
            return dis;
        }

        /// <summary>
        /// <para>设置Window对象的窗口视图伪装对象（仅WPF平台）</para>
        /// <para>Sets the window view disguise object for a Window object (WPF platform only)</para>
        /// </summary>
        /// <param name="dobj">
        /// <para>目标Window对象</para>
        /// <para>The target Window object</para>
        /// </param>
        /// <param name="value">
        /// <para>要设置的窗口视图伪装对象</para>
        /// <para>The window view disguise object to set</para>
        /// </param>
        public static void SetViewDisguise(this Window dobj, WindowViewDisguise value)
        {
            dobj?.SetValue(ViewDisguiseProperty, value);
        }
#endif

        /// <summary>
        /// <para>视图伪装依赖属性，用于存储视图伪装对象，支持动画、样式、绑定等功能</para>
        /// <para>Dependency property for view disguise that stores view disguise objects and supports animation, styling, binding, etc.</para>
        /// </summary>
        // Using a DependencyProperty as the backing store for ViewDisguise.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewDisguiseProperty =
            DependencyProperty.RegisterAttached(nameof(GetViewDisguise).Remove(0, 3), typeof(IViewDisguise), typeof(ViewDisguiseManager), new PropertyMetadata(null));



    }
}
#endif
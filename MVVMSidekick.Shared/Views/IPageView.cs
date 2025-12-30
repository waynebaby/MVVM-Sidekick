


namespace MVVMSidekick.Views
{
    /// <summary>
    /// <para>页面视图接口，扩展基本的IView接口以提供页面特定的导航功能</para>
    /// <para>Page view interface that extends the basic IView interface to provide page-specific navigation functionality</para>
    /// </summary>
    /// <remarks>
    /// <para>此接口根据不同的编译条件提供不同平台的页面导航方法，包括UWP的导航事件处理和WPF的框架对象访问</para>
    /// <para>This interface provides platform-specific page navigation methods based on different compilation conditions, including UWP navigation event handling and WPF frame object access</para>
    /// </remarks>
    public interface IPageView :IView
    {

#if WINDOWS_UWP

        /// <summary>
        /// <para>当页面导航离开时调用的方法</para>
        /// <para>Method called when page is navigated from</para>
        /// </summary>
        /// <param name="e">
        /// <para>导航事件参数</para>
        /// <para>Navigation event arguments</para>
        /// </param>
        void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e);
        
        /// <summary>
        /// <para>当页面导航到此页面时调用的方法</para>
        /// <para>Method called when page is navigated to</para>
        /// </summary>
        /// <param name="e">
        /// <para>导航事件参数</para>
        /// <para>Navigation event arguments</para>
        /// </param>
        void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e);
        
        /// <summary>
        /// <para>当页面即将导航离开时调用的方法</para>
        /// <para>Method called when page is about to navigate away</para>
        /// </summary>
        /// <param name="e">
        /// <para>可取消的导航事件参数</para>
        /// <para>Cancellable navigation event arguments</para>
        /// </param>
        void OnNavigatingFrom(Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e);
#elif WPF
        /// <summary>
        /// <para>框架对象属性，用于WPF平台的页面导航</para>
        /// <para>Frame object property for WPF platform page navigation</para>
        /// </summary>
        /// <value>
        /// <para>框架对象实例</para>
        /// <para>Frame object instance</para>
        /// </value>
        object FrameObject { get; set; }
#endif
    }
}
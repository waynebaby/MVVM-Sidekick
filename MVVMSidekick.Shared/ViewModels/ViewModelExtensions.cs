
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.Services;
using MVVMSidekick.ViewModels;
namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// <para>视图模型扩展方法静态类，提供视图模型相关的扩展功能</para>
    /// <para>Static class for view model extension methods, providing view model related extended functionality</para>
    /// </summary>
    /// <remarks>
    /// <para>此类包含用于获取当前视图调度器、检查设计时状态等视图模型扩展方法</para>
    /// <para>This class contains extension methods for getting current view dispatcher, checking design time status, and other view model functionality</para>
    /// </remarks>
    public static class ViewModel
    {
#if WINDOWS_UWP
        /// <summary>
        /// <para>获取视图模型当前绑定视图的CoreDispatcher调度器</para>
        /// <para>Gets the CoreDispatcher of the current binding view for the view model</para>
        /// </summary>
        /// <param name="viewModel">
        /// <para>视图模型实例</para>
        /// <para>View model instance</para>
        /// </param>
        /// <returns>
        /// <para>CoreDispatcher实例，如果无法获取则返回null</para>
        /// <para>CoreDispatcher instance, returns null if unable to get</para>
        /// </returns>
        public static Windows.UI.Core.CoreDispatcher GetCurrentViewDispatcher(this IViewModel viewModel)
        {
            Windows.UI.Xaml.DependencyObject dp = null;
            if (viewModel?.StageManager == null)
            {
                return null;
            }
            else if ((dp = (viewModel?.StageManager.CurrentBindingView as Windows.UI.Xaml.DependencyObject)) == null)
            {
                return null;
            }
            return dp?.Dispatcher;

        }
#elif WPF
        /// <summary>
        /// <para>获取视图模型当前绑定视图的WPF Dispatcher调度器</para>
        /// <para>Gets the WPF Dispatcher of the current binding view for the view model</para>
        /// </summary>
        /// <param name="viewModel">
        /// <para>视图模型实例</para>
        /// <para>View model instance</para>
        /// </param>
        /// <returns>
        /// <para>WPF Dispatcher实例，如果无法获取则返回null</para>
        /// <para>WPF Dispatcher instance, returns null if unable to get</para>
        /// </returns>
        public static System.Windows.Threading.Dispatcher GetCurrentViewDispatcher(this IViewModel viewModel)
        {
            System.Windows.DependencyObject dp = null;
            if (viewModel?.StageManager == null)
            {
                return null;
            }
            else if ((dp = (viewModel?.StageManager.CurrentBindingView as System.Windows.DependencyObject)) == null)
            {
                return null;
            }
            return dp?.Dispatcher;
        }
#endif

#if !BLAZOR

        /// <summary>
        /// <para>获取代码是否在设计时状态下运行</para>
        /// <para>Gets whether the code is running in design time state</para>
        /// </summary>
        /// <value>
        /// <para>如果此实例处于设计模式下则为true，否则为false</para>
        /// <para>True if this instance is in design mode, otherwise false</para>
        /// </value>
        /// <remarks>
        /// <para>此属性用于检测当前代码是否在设计器环境中运行，如Visual Studio设计器等</para>
        /// <para>This property is used to detect whether the current code is running in a designer environment, such as Visual Studio designer</para>
        /// </remarks>
        public static bool IsInDesignMode => (ServiceProviderLocator.RootServiceProvider?.GetService<ITellDesignTimeService>() ?? new InDesignTime()).IsInDesignMode;
#endif
#if BLAZOR


  
#endif



    }
}
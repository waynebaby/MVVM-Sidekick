
using MVVMSidekick.Services;
using MVVMSidekick.ViewModels;
namespace MVVMSidekick.ViewModels
{
    public static class PlatformServiceHelper
    {
#if WINDOWS_UWP
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
        /// <summary>
        /// The _ is in design mode
        /// </summary>
        static bool? _IsInDesignMode;


        /// <summary>
        /// <para>Gets if the code is running in design time. </para>
        /// <para>读取目前是否在设计时状态。</para>
        /// </summary>
        /// <value><c>true</c> if this instance is in design mode; otherwise, <c>false</c>.</value>
        public static bool IsInDesignMode => ServiceLocator.Instance.TryResolve<ITellDesignTimeService>(() => new InDesignTime()).IsInDesignMode;

    }
}
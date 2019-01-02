


namespace MVVMSidekick.Views
{
    public interface IPageView :IView
    {

#if NETFX_CORE

        void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e);
        void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e);
        void OnNavigatingFrom(Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e);
#elif WPF
        object FrameObject { get; set; }
#endif
    }
}
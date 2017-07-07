


namespace MVVMSidekick.Views
{
    public interface IPageView :IView
    {

#if !WPF

        void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e);
        void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e);
        void OnNavigatingFrom(Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e);
#else
        object FrameObject { get; set; }
#endif
    }
}
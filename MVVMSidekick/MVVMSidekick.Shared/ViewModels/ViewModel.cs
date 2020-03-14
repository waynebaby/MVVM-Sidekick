using System;
using System.Collections.Generic;
using System.Text;


namespace MVVMSidekick.ViewModels
{
    public class ViewModel<TViewModel> : ViewModelBase<TViewModel>, IViewModelWithPlatformService where TViewModel : ViewModel<TViewModel>
    {

        public ViewModel()
        {


        }
        /// <summary>
        /// Gets a value indicating whether this instance is in design mode.
        /// </summary>
        /// <value><c>true</c> if this instance is in design mode; otherwise, <c>false</c>.</value>





#if WINDOWS_UWP

        public virtual void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {

        }

        public virtual void OnPageNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {

        }

        public virtual void OnPageNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {

        }

        public virtual void OnPageNavigatingFrom(Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
    
        }

        public virtual void SaveState(Dictionary<string, object> pageState)
        {
       
        }
#elif WPF
        public System.Windows.Controls.Frame FrameObject { get; set; }


#endif



    }
}

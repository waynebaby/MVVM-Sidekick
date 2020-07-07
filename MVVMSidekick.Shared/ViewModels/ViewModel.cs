
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MVVMSidekick.Reactive;
using MVVMSidekick.Views;

namespace MVVMSidekick.ViewModels
{

#if BLAZOR
    using Microsoft.AspNetCore.Components;
    public class ViewModel<TViewModel, TPage> : ViewModelBase<TViewModel>,IViewModelWithPlatformService where TViewModel : ViewModel<TViewModel, TPage>
        where TPage : ComponentBase
    {
        static ViewModel()
        {


        }

        [Inject]
        public override IStageManager StageManager { get => base.StageManager; set => base.StageManager = value; }
        [Inject]
        public IServiceProvider BlazorServiceProvider { get; set; }

        public virtual void OnInitialized() { }
        public virtual async Task OnInitializedAsync() => await Task.CompletedTask;
        public virtual void OnParametersSet() { }
        public virtual async Task OnParametersSetAsync() => await Task.CompletedTask;


#else
   {
    public class ViewModel<TViewModel> : ViewModelBase<TViewModel>, IViewModelWithPlatformService where TViewModel : ViewModel<TViewModel>
    {
#endif


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
#elif BLAZOR

#endif



    }
}

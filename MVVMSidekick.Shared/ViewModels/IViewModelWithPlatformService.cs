using MVVMSidekick.Services;
using MVVMSidekick.Views;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
 

namespace MVVMSidekick.ViewModels
{



#if BLAZOR
    using Microsoft.AspNetCore.Components;

#endif
    public interface IViewModelWithPlatformService : IViewModel
    {
#if WINDOWS_UWP || WinUI3

        void OnPageNavigatedTo(NavigationEventArgs e);
        void OnPageNavigatedFrom(NavigationEventArgs e);
        void OnPageNavigatingFrom(NavigatingCancelEventArgs e);
        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        void LoadState(Object navigationParameter, Dictionary<String, Object> pageState);

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. 
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        void SaveState(Dictionary<String, Object> pageState);

#elif WPF
        System.Windows.Controls.Frame FrameObject { get; set; }
#elif BLAZOR
        IServiceProvider BlazorServiceProvider { get; set; }
        void OnInitialized();
        Task OnInitializedAsync();
        void OnParametersSet();
        Task OnParametersSetAsync();
        void OnAfterRender(bool firstRender);
        Task OnAfterRenderAsync(bool firstRender);
        Task SetParametersAsync(ParameterView parameters);
        void RequestRerender();
        
        bool? IsFirstRender { get; }
#endif


    }

}

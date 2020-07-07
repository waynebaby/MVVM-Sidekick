using MVVMSidekick.Views;
using System;
using System.Threading.Tasks;

namespace MVVMSidekick.ViewModels
{
    public interface IBlazorViewModel
    {
        IServiceProvider BlazorServiceProvider { get; set; }
        IStageManager StageManager { get; set; }

        void OnInitialized();
        Task OnInitializedAsync();
        void OnParametersSet();
        Task OnParametersSetAsync();
    }
}
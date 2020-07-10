using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components
{
    public  class MVVMSidekickComponentBase<TView,TViewModel> : ComponentBase 
        where TView : MVVMSidekickComponentBase<TView,TViewModel>
        where TViewModel : ViewModel<TViewModel, TView>
    {

        [Inject]
        public TViewModel ViewModel{ get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
            ViewModel.Page = this as TView;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            ViewModel?.OnParametersSet();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await ViewModel?.OnParametersSetAsync();
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            ViewModel?.OnInitialized();
            if (ViewModel != null)
            {
                ViewModel.PropertyChanged += (o, a) => StateHasChanged();

            }
        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await ViewModel?.OnInitializedAsync();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            ViewModel?.OnAfterRender(firstRender);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await ViewModel?.OnAfterRenderAsync(firstRender);
        }

       
    }
}

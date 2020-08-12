using MVVMSidekick.Common;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components
{
    public class MVVMSidekickComponentBase<TView, TViewModel> : ComponentBase, IDisposable, IAsyncDisposable,IDisposeGroup
        where TView : MVVMSidekickComponentBase<TView, TViewModel>
        where TViewModel : ViewModel<TViewModel, TView>
    {
        private bool disposedValue;

        public event EventHandler<DisposeEventArgs> DisposeEntryDisposing;
        public event EventHandler<DisposeEventArgs> DisposeEntryDisposed;

        [Inject]
        public TViewModel ViewModel { get; set; }

        /// <summary>
        /// Shortcut for ViewModel
        /// </summary>
        protected TViewModel M { get => this.ViewModel; }

        public IList<DisposeEntry> DisposeInfoList => throw new NotImplementedException();

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

        public void RequestRender()
        {
            base.ShouldRender();
        }



        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            dg.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual ValueTask DisposeAsync()
        {
            return default;
        }

        public void AddDisposable(IDisposable item, bool needCheckInFinalizer = false, string comment = "", string member = "", string file = "", int line = -1)
        {
            dg.AddDisposable(item, needCheckInFinalizer, comment, member, file, line);
        }

        public void AddDisposeAction(Action action, bool needCheckInFinalizer = false, string comment = "", string member = "", string file = "", int line = -1)
        {
            dg.AddDisposeAction(action, needCheckInFinalizer, comment, member, file, line);
        }

        private IDisposeGroup dg = new  DisposeGroup()  ;
     
    }
}

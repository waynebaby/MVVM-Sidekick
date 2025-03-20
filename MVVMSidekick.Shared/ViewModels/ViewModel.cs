
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
    using Microsoft.Extensions.DependencyInjection;

    public class ViewModel<TViewModel, TView> : ViewModelBase<TViewModel>, IViewModelWithPlatformService where TViewModel : ViewModel<TViewModel, TView>
        where TView : MVVMSidekickComponentBase<TView, TViewModel>
    {

        public ViewModel(IServiceProvider serviceProvider)
        {
            BlazorServiceProvider = serviceProvider;
            StageManager = serviceProvider.GetService<IStageManager>();
        }
        public TView Page { get; set; }


        /// <summary>
        /// This property shows 3 status of a blazor page could have been, before render, server prerender or client render. As logic after ""
        /// </summary>
        public bool? IsFirstRender { get => _IsFirstRenderLocator(this).Value; protected set => _IsFirstRenderLocator(this).SetValueAndTryNotify(value); }
        #region Property bool? IsFirstRender Setup        
        protected Property<bool?> _IsFirstRender = new Property<bool?>(_IsFirstRenderLocator);
        static Func<BindableBase, ValueContainer<bool?>> _IsFirstRenderLocator = RegisterContainerLocator(nameof(IsFirstRender), m => m.Initialize(nameof(IsFirstRender), ref m._IsFirstRender, ref _IsFirstRenderLocator, default));
        #endregion


        public override IStageManager StageManager { get => base.StageManager; set => base.StageManager = value; }
        public IServiceProvider BlazorServiceProvider { get; set; }
        public virtual void OnInitialized() { }
        public virtual Task OnInitializedAsync() => Task.CompletedTask;
        public virtual void OnParametersSet() { }
        public virtual Task OnParametersSetAsync() => Task.CompletedTask;
        public virtual void OnAfterRender(bool firstRender)
        {
            IsFirstRender = firstRender;
        }
        public virtual Task OnAfterRenderAsync(bool firstRender)
        {
            IsFirstRender = firstRender; return Task.CompletedTask;
        }

        public void RequestRerender()
        {
            Page?.RequestRerender();
        }



        public virtual Task SetParametersAsync(ParameterView parameters) => Task.CompletedTask;
#else

    public class ViewModel<TViewModel> : ViewModelBase<TViewModel>, IViewModelWithPlatformService where TViewModel : ViewModel<TViewModel>
    {
#endif


#if WINDOWS_UWP || WinUI3

        public virtual void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {

        }

        public virtual void OnPageNavigatedFrom( NavigationEventArgs e)
        {

        }

        public virtual void OnPageNavigatedTo( NavigationEventArgs e)
        {

        }

        public virtual void OnPageNavigatingFrom( NavigatingCancelEventArgs e)
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

#if !BLAZOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVVMSidekick.ViewModels;
using System.Reactive.Linq;
using System.Windows;
using System.IO;
using MVVMSidekick.Services;
using Microsoft.Extensions.DependencyInjection;


#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;


#elif WPF
using System.Windows.Controls;
using System.Windows.Media;

using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;
#elif SILVERLIGHT_5 || SILVERLIGHT_4
						   using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif



namespace MVVMSidekick.Views
{

    public class TestingStageManager : IStageManager
    {
        public TestingStageManager(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        private Dictionary<string, IStage> _currentStages = new Dictionary<string, IStage>();
        public IStage this[string beaconKey]
        {
            get
            {
                _currentStages.TryGetValue(beaconKey, out IStage stage);
                if (stage == null)
                {
                    var tstage = ServiceProvider.GetRequiredService<IStage>(beaconKey) as TestingStage;
                    stage = tstage;

                    tstage.BeaconKey = beaconKey;
                    tstage.CanGoBack = true;
                    tstage.CanGoForward = true;
                    tstage.IsGoBackSupported = true;
                    tstage.IsGoForwardSupported = true;
                    tstage.Target = null;

                    _currentStages[beaconKey] = stage;
                }
                return stage;
            }
        }

        public IView CurrentBindingView
        {
            get; set;
        }

        public IStage DefaultStage
        {
            get => null;

            set { }
        }

        public IViewModel ViewModel { get; set; }
        public IServiceProvider ServiceProvider { get; }

        public void InitParent(Func<object> parentLocator)
        {

        }
    }
    public class TestingStage : IStage
    {


        public TestingStage(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public string BeaconKey
        {
            get; set;
        }

        public bool CanGoBack
        {
            get; set;
        }

        public bool CanGoForward
        {
            get; set;
        }

        public Frame Frame
        {
            get; set;
        }

        public bool IsGoBackSupported
        {
            get; set;
        }

        public bool IsGoForwardSupported
        {
            get; set;
        }

        public Object Target
        {
            get; set;
        }
        IServiceProvider ServiceProvider { get; }

#if WPF
        public async Task<TResult> ShowAndReturnResult<TTarget, TResult>(TTarget targetViewModel = null, string viewMappingKey = null) where TTarget : class, IViewModel<TResult>
        {
            var vm = targetViewModel ?? ServiceProviderLocator.RootServiceProvider.GetRequiredService<TTarget>(viewMappingKey);
            vm = await InternalTestShow(vm).ConfigureAwait(true);
            return vm.Result;

        }
        public async Task<ShowAwaitableResult<TTarget>> ShowAndGetViewModelImmediately<TTarget>(TTarget targetViewModel = null, string viewMappingKey = null) where TTarget : class, IViewModel
        {
            var vm = targetViewModel ?? ServiceProviderLocator.RootServiceProvider.GetRequiredService<TTarget>(viewMappingKey);
            var vmt = InternalTestShow(vm).ConfigureAwait(true);
            return await Task.FromResult(new ShowAwaitableResult<TTarget>() { Closing = vm.WaitForClose(), ViewModel = vm }).ConfigureAwait(true);

        }
#endif 

        Dictionary<Type, Func<IViewModel, Task<IViewModel>>> mockingActionsWhenShown
            = new Dictionary<Type, Func<IViewModel, Task<IViewModel>>>();
        public void MockShowLogic<TTarget>(Func<TTarget, Task<TTarget>> mockingActionWhenShowing) where TTarget : class, IViewModel
        {
            Func<IViewModel, Task<IViewModel>> asyncAction = async (m) =>
              {
                  var inp = m as TTarget;
                  var rval = await mockingActionWhenShowing(inp).ConfigureAwait(true);
                  inp.CloseViewAndDispose();
                  return rval;
              };
            mockingActionsWhenShown[typeof(TTarget)] = asyncAction;

        }



        private async Task<TTarget> InternalTestShow<TTarget>(TTarget vm) where TTarget : class, IViewModel
        {

            Func<IViewModel, Task<IViewModel>> mockingAction = null;
            if (mockingActionsWhenShown.TryGetValue(typeof(TTarget), out mockingAction))
            {
                await mockingAction(vm).ConfigureAwait(true);
            }
            else
            {
                await vm.OnBindedViewLoad(null).ConfigureAwait(true);
            }
            return vm;
        }

        async Task<TTarget> IStage.Show<TTarget>(string viewMappingKey, Action<(IServiceProvider serviceProvider, TTarget viewModel)> additionalViewModelConfig, bool isWaitingForDispose, bool autoDisposeWhenViewUnload)
        {
            var instancedViewModel = ServiceProviderLocator.RootServiceProvider.GetRequiredService<TTarget>(viewMappingKey);
            additionalViewModelConfig?.Invoke((ServiceProvider, instancedViewModel));

            var w = InternalTestShow(instancedViewModel);
            if (isWaitingForDispose)
            {
                return await w.ConfigureAwait(true);
            }
            return instancedViewModel;
        }
    }
}
#endif
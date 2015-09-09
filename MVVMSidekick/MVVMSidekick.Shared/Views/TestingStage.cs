using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVVMSidekick.ViewModels;
using System.Reactive.Linq;
using System.Windows;
using System.IO;
using MVVMSidekick.Services;



#if NETFX_CORE
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
    public class TestingStage : IStage
    {




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

        public FrameworkElement Target
        {
            get; set;
        }
#if SILVERLIGHT_5 || WINDOWS_PHONE_7 || WINDOWS_PHONE_8
        public Dictionary<string, IViewModel> NavigateRequestContexts { get; set; }
#endif
#if WPF
        public async Task<TResult> ShowAndReturnResult<TTarget, TResult>(TTarget targetViewModel = null, string viewMappingKey = null) where TTarget : class, IViewModel<TResult>
        {
            var vm = targetViewModel ?? ServiceLocator.Instance.Resolve<TTarget>(viewMappingKey);
            vm = await InternalTestShow(vm, viewMappingKey);
            return vm.Result;

        }
        public Task<ShowAwaitableResult<TTarget>> ShowAndGetViewModelImmediately<TTarget>(TTarget targetViewModel = null, string viewMappingKey = null) where TTarget : class, IViewModel
        {
            var vm = targetViewModel ?? ServiceLocator.Instance.Resolve<TTarget>(viewMappingKey);
            var vmt = InternalTestShow(vm, viewMappingKey);
            return Task.FromResult(new ShowAwaitableResult<TTarget>() { Closing = vm.WaitForClose(), ViewModel = vm });
        }


#endif

        Dictionary<Type, Func<IViewModel, Task<IViewModel>>> mockingActionsWhenShown
            = new Dictionary<Type, Func<IViewModel, Task<IViewModel>>>();
        public void MockShowLogic<TTarget>(Func<TTarget, Task<TTarget>> mockingActionWhenShowing) where TTarget : class, IViewModel
        {
            Func<IViewModel, Task<IViewModel>> asyncAction = async (m) =>
              {
                  var inp = m as TTarget;
                  var rval = await mockingActionWhenShowing(inp);
                  inp.CloseViewAndDispose();
                  return rval;
              };
            mockingActionsWhenShown[typeof(TTarget)] = asyncAction;

        }

        public async Task<TTarget> Show<TTarget>(TTarget targetViewModel = null, string viewMappingKey = null) where TTarget : class, IViewModel
        {
            var vm = targetViewModel ?? ServiceLocator.Instance.Resolve<TTarget>(viewMappingKey);
            return await InternalTestShow(vm, viewMappingKey);
        }

        private async Task<TTarget> InternalTestShow<TTarget>(TTarget vm, string viewMappingKey) where TTarget : class, IViewModel
        {

            Func<IViewModel, Task<IViewModel>> mockingAction = null;
            if (mockingActionsWhenShown.TryGetValue(typeof(TTarget), out mockingAction))
            {
                await mockingAction(vm);
            }
            else
            {
                await vm.OnBindedViewLoad(null);
            }
            return vm;
        }
    }
}

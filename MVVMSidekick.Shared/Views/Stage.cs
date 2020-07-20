#if !BLAZOR
using System;
using System.Linq;
using System.Threading.Tasks;
using MVVMSidekick.ViewModels;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;



#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Foundation;


#elif WPF
using System.Windows;

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
    /// <summary>
    /// Stage class
    /// </summary>
    public class Stage : DependencyObject, IStage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Stage"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="beaconKey">The beacon key.</param>
        /// <param name="stageManager">The stageManager.</param>
        public Stage(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        public void InitStage(FrameworkElement target, string beaconKey, StageManager stageManager)
        {
            Target = target;
            _stageManager = stageManager;
            BeaconKey = beaconKey;
        }

        private StageManager _stageManager;
        private FrameworkElement _target;




        /// <summary>
        /// Gets the frame.
        /// </summary>
        /// <value>
        /// The frame.
        /// </value>
        public Frame Frame
        {
            get => (Frame)GetValue(FrameProperty);
            private set => SetValue(FrameProperty, value);
        }

        // Using a DependencyProperty as the backing store for Frame.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The frame property
        /// </summary>
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register("Frame", typeof(Frame), typeof(Stage), new PropertyMetadata(null));



        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public Object Target
        {
            get => _target;
            private set
            {
                _target = value as FrameworkElement;



            }
        }
#if WPF
        /// <summary>
        /// Is go forward supported
        /// </summary>
        public bool IsGoForwardSupported
        {
            get
            {
                return Frame != null;
            }
        }

        /// <summary>
        /// Can go forward
        /// </summary>
        public bool CanGoForward
        {
            get
            {
                return IsGoForwardSupported ? Frame.CanGoForward : false;
            }

        }
#elif WINDOWS_UWP

        /// <summary>
        /// Is go forward supported
        /// </summary>
        public bool IsGoForwardSupported => false;
        /// <summary>
        /// Can go forward
        /// </summary>
        public bool CanGoForward => false;
#endif

        /// <summary>
        /// Gets a value indicating whether this instance is go back supported.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is go back supported; otherwise, <c>false</c>.
        /// </value>
        public bool IsGoBackSupported => Frame != null;


        /// <summary>
        /// Gets a value indicating whether this instance can go back.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can go back; otherwise, <c>false</c>.
        /// </value>
        public bool CanGoBack => IsGoBackSupported ? Frame.CanGoBack : false;



        /// <summary>
        /// Gets the beacon key.
        /// </summary>
        /// <value>
        /// The beacon key.
        /// </value>
        public string BeaconKey
        {
            get => (string)GetValue(BeaconKeyProperty);
            private set => SetValue(BeaconKeyProperty, value);
        }
        IServiceProvider ServiceProvider { get; }

        // Using a DependencyProperty as the backing store for BeaconKey.  This enables animation, styling, binding, etc...			
        /// <summary>
        /// The beacon key property
        /// </summary>
        public static readonly DependencyProperty BeaconKeyProperty =
            DependencyProperty.Register("BeaconKey", typeof(string), typeof(Stage), new PropertyMetadata(""));


#if WPF

        private IView InternalLocateViewIfNotSet<TTarget>(TTarget targetViewModel, string viewMappingKey, IView view) where TTarget : class, IViewModel
        {
            if (targetViewModel != null && targetViewModel.StageManager != null)
            {
                view = targetViewModel.StageManager.CurrentBindingView;
            }

            var (_, viewType) = ViewAndModelMappingsHelper.DefaultVMToViewMapping[(viewMappingKey, typeof(TTarget))];


            var tempControl = ServiceProvider.GetService(viewType) as FrameworkElement;

            view = view ?? tempControl as IView;
            view = view ?? (tempControl as FrameworkElement)?.GetOrCreateViewDisguise();
            return view;
        }




        /// <summary>
        /// Show a view model with mapped view
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="targetViewModel"></param>
        /// <param name="viewMappingKey"></param>
        /// <returns></returns>
        public async Task<TTarget> Show<TTarget>(string viewMappingKey, Action<(IServiceProvider serviceProvider, TTarget viewModel)> additionalViewModelConfig, bool isWaitingForDispose, bool autoDisposeWhenViewUnload) where TTarget : class, IViewModel
        {

            IView view = null;
            var targetViewModel = ServiceProvider.GetService<TTarget>();

            view = InternalLocateViewIfNotSet<TTarget>(targetViewModel, viewMappingKey, view);
            targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
            targetViewModel = targetViewModel ?? view.GetDefaultViewModel(viewMappingKey) as TTarget;

            targetViewModel.IsDisposingWhenUnloadRequired = autoDisposeWhenViewUnload;
            additionalViewModelConfig?.Invoke((ServiceProvider, targetViewModel));

            SetVMAfterLoad(targetViewModel, view);
            InternalShowView(view, Target as FrameworkElement, _stageManager.CurrentBindingView.ViewModel);

            if (isWaitingForDispose)
            {
                await targetViewModel.WaitForClose().ConfigureAwait(true);
            }
            return targetViewModel;
        }



#endif


        private static void SetVMAfterLoad<TTarget>(TTarget targetViewModel, IView view) where TTarget : class, IViewModel
        {
            if (view == null)
            {
                throw new InvalidOperationException(
                    $@"
Cannot find ANY mapping from View Model [{targetViewModel.GetType().ToString()}] to ANY View.
Please check startup function of this mapping is well configured and be proper called while application starting");
            }


            if (view is IPageView)
            {

                view.ViewModel = targetViewModel;
            }
            view.ViewModel = targetViewModel;


        }

#if WINDOWS_UWP



        /// <summary>
        /// Show a view model mapped view.
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="targetViewModel"></param>
        /// <param name="viewMappingKey"></param>
        /// <param name="isWaitingForDispose"></param>
        /// <returns></returns>
        public async Task<TTarget> Show<TTarget>(string viewMappingKey, Action<(IServiceProvider serviceProvider, TTarget viewModel)> additionalViewModelConfig, bool isWaitingForDispose, bool autoDisposeWhenViewUnload) where TTarget : class, IViewModel
        {
            var targetViewModel = ServiceProvider.GetService<TTarget>();

            //bool isInstancePassedThough = targetViewModel == null;
            // object item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewMappingKey, targetViewModel);


            var (_, registeredViewType) = ViewAndModelMappingsHelper.DefaultVMToViewMapping[(viewMappingKey, typeof(TTarget))];


            IView view;
            if (typeof(Page).IsAssignableFrom(registeredViewType))
            {
                Frame frame = Target as Frame;
                if (frame != null)
                {
                    targetViewModel = await FrameNavigate<TTarget>(targetViewModel, viewMappingKey, registeredViewType, frame, ServiceProvider).ConfigureAwait(true);

                    await targetViewModel.WaitForClose().ConfigureAwait(true);
                    return targetViewModel;
                }

                view = (frame.Content as DependencyObject)?.GetOrCreateViewDisguise();
            }
            else
            {
                var fe = ServiceProvider.GetService(viewMappingKey,registeredViewType) as FrameworkElement;
                view = fe.GetOrCreateViewDisguise();
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
            }

            targetViewModel.IsDisposingWhenUnloadRequired = autoDisposeWhenViewUnload;
            additionalViewModelConfig?.Invoke((ServiceProvider, targetViewModel));

            SetVMAfterLoad(targetViewModel, view);
            InternalShowView(view, Target as FrameworkElement, _stageManager.CurrentBindingView.ViewModel);

            if (isWaitingForDispose)
            {
                await targetViewModel.WaitForClose().ConfigureAwait(true);
            }
            return targetViewModel;

        }

        private static async Task<TTarget> FrameNavigate<TTarget>(TTarget targetViewModel, string mappingKey, Type type, Windows.UI.Xaml.Controls.Frame frame, IServiceProvider serviceProvider) where TTarget : class, IViewModel
        {
            StageNavigationContext<TTarget> parameter = new StageNavigationContext<TTarget>() { ViewModel = targetViewModel };
            TaskCompletionSource<object> t = new TaskCompletionSource<object>();
            IDisposable dip = EventRouting.EventRouter.Instance
                 .GetEventChannel<NavigationEventArgs>()

                 .Where(e =>
                         object.ReferenceEquals(e.EventData.Parameter, parameter))
                 .Subscribe(e =>
                 {
                     Page page = null;
                     IView view = null;
                     switch (e.Sender)
                     {
                         case PageViewDisguise disguise:
                             page = disguise.AssocatedObject;
                             view = disguise;
                             break;
                         default:
                             break;
                     }

                     var viewInstance = view.ViewContentObject;

                     var configOfView = serviceProvider.GetService(typeof(ViewContentConfigurator<>).MakeGenericType(viewInstance.GetType())) as IViewContentConfigurator;
                     configOfView.Config(viewInstance);


                     if (parameter.ViewModel != null)
                     {
                         view.ViewModel = parameter.ViewModel;
                     }
                     else
                     {
                         IViewModel solveV = view.GetDefaultViewModel(mappingKey);
                         if (solveV != null)
                         {
                             targetViewModel = parameter.ViewModel = (TTarget)solveV;
                         }
                     }

                     if (targetViewModel == null)
                     {
                         targetViewModel = (TTarget)view.ViewModel;
                     }

                     view.ViewModel = parameter.ViewModel = targetViewModel;
                     (targetViewModel as IViewModelWithPlatformService)?.OnPageNavigatedTo(e.EventData);
                     t.TrySetResult(null);
                 });

            frame.Navigate(type, parameter);
            await t.Task.ConfigureAwait(true);
            dip.DisposeWith(targetViewModel);
            return targetViewModel;
        }

        private class StageNavigationContext<T> where T : IViewModel
        {
            public T ViewModel { get; set; }

        }

        ///// <summary>
        ///// Show a viewmodel and return a result when leave.
        ///// </summary>
        ///// <typeparam name="TTarget"></typeparam>
        ///// <typeparam name="TResult"></typeparam>
        ///// <param name="targetViewModel"></param>
        ///// <param name="viewMappingKey"></param>

        ///// <returns></returns>
        //public async Task<TResult> Show<TTarget, TResult>(TTarget targetViewModel = null, string viewMappingKey = null, bool isWaitingForDispose = false)
        //    where TTarget : class, IViewModel<TResult>
        //{
        //    object item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewMappingKey, targetViewModel);
        //    Type type;
        //    if ((type = item as Type) != null) //only MVVMPage Can be registered as Type
        //    {
        //        Frame frame;
        //        if ((frame = Target as Frame) != null)
        //        {
        //            targetViewModel = await FrameNavigate<TTarget>(targetViewModel, type, frame);

        //            return await targetViewModel.WaitForCloseWithResult();
        //        }

        //    }


        //    IView view = item as IView;
        //    targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
        //    SetVMAfterLoad(targetViewModel, view);
        //    InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
        //    if (isWaitingForDispose)
        //    {
        //        return await targetViewModel.WaitForCloseWithResult();
        //    }
        //    else
        //    {
        //        return targetViewModel.Result;
        //    }

        //}

        ///// <summary>
        ///// show a view model mapped view and return the viewmodel 
        ///// </summary>
        ///// <typeparam name="TTarget"></typeparam>
        ///// <param name="targetViewModel"></param>
        ///// <param name="viewMappingKey"></param>
        ///// <returns></returns>

        //public async Task<ShowAwaitableResult<TTarget>> ShowAndGetViewModel<TTarget>(TTarget targetViewModel = null, string viewMappingKey = null)
        //    where TTarget : class, IViewModel
        //{
        //    object item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewMappingKey, targetViewModel);
        //    Type type;
        //    if ((type = item as Type) != null) //only MVVMPage Can be registered as Type
        //    {
        //        Frame frame;
        //        if ((frame = Target as Frame) != null)
        //        {
        //            targetViewModel = await FrameNavigate<TTarget>(targetViewModel, type, frame);

        //            return new ShowAwaitableResult<TTarget>
        //            {
        //                Closing = targetViewModel.WaitForClose(),
        //                ViewModel = targetViewModel
        //            };
        //        }

        //    }


        //    IView view = item as IView;

        //    targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
        //    SetVMAfterLoad(targetViewModel, view);
        //    InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);

        //    Task tr = targetViewModel.WaitForClose();
        //    return new ShowAwaitableResult<TTarget> { Closing = tr, ViewModel = targetViewModel };
        //}
#endif



        private void InternalShowView(IView view, FrameworkElement target, IViewModel sourceVM)
        {

            switch (target)
            {
#if WPF
                case FrameworkElement targetWindow when view is IWindowView:
                    var viewWindow = view.ViewObject as Window;
                    IWindowView iviewWindow = view as IWindowView;

                    if (targetWindow == null)
                    {
                        targetWindow = sourceVM.StageManager.CurrentBindingView as Window;
                    }
                    if (iviewWindow.IsAutoOwnerSetNeeded)
                    {
                        //viewWindow.Owner = targetWindow;
                    }
                    viewWindow.Show();
                    break;
                case Frame targetFrame:

                    var ipv = (view as IPageView);

                    if (ipv != null)
                    {
                        ipv.FrameObject = this.Target;
                    }
                    targetFrame.Navigate(view.ViewObject);

                    break;
#endif
#if WINDOWS_UWP
                case ContentDialog targetCDControl:
                    targetCDControl.Content = view.ViewObject;
                    IViewModel viewModel = view.ViewModel ?? sourceVM.StageManager.ViewModel;
                    IDisposable closeFromViewModel = null;
                    closeFromViewModel = Observable
                        .FromAsync(x => viewModel.WaitForClose())
                        .ObserveOnDispatcher()
                        .Subscribe(_ =>
                            {
                                viewModel.IsDisposingWhenUnloadRequired = true;
                                targetCDControl.Hide();
                                targetCDControl.Content = null;
                                closeFromViewModel?.Dispose();
                                closeFromViewModel = null;
                            });
                    IAsyncOperation<ContentDialogResult> t = targetCDControl.ShowAsync();
                    break;
#endif
                case ContentControl targetCControl:
                    targetCControl.Content = view.ViewObject;
                    break;
                case Panel targetPanelControl:
                    targetPanelControl.Children.Add(view.ViewObject as UIElement);
                    break;

                default:
                    throw new InvalidOperationException($"This view {view.GetType()} is not support show in {target.GetType()} ");
            }


        }


    }
}
#endif
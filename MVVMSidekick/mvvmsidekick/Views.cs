using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Commands;
using System.Runtime.CompilerServices;
using MVVMSidekick.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using MVVMSidekick.EventRouting ;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Collections;
using System.Windows;


#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using System.Collections.Concurrent;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Xaml.Controls.Primitives;

#elif WPF
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;

#elif SILVERLIGHT_5||SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8||WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif

namespace MVVMSidekick
{

    namespace Views
    {


        public static class ViewHelper
        {
            internal static PropertyChangedCallback ViewModelChangedCallback
                = (o, e) =>
                {
                    dynamic item = o;
                    ((o as IView).Content as FrameworkElement).DataContext = e.NewValue;
                    var nv = e.NewValue as IViewModel;
                    var ov = e.OldValue as IViewModel;
                    if (ov != null)
                    {
                        ov.OnUnbindedFromView(o as IView, nv);
                    }
                    if (nv != null)
                    {
                        nv.OnBindedToView(o as IView, ov);
                    }

                };

            internal static FrameworkElement CheckContent(this IView control)
            {
                var c = (control.Content as FrameworkElement);
                if (c == null)
                {
                    control.Content = c = new Grid();
                }
                return c;
            }

            public static void SelfClose(this IView view)
            {
                view.ViewModel = null;
                if (view is UserControl || view is Page)
                {
                    var viewElement = view as FrameworkElement;
                    var parent = viewElement.Parent;
                    if (parent is Panel)
                    {
                        (parent as Panel).Children.Remove(viewElement);
                    }
                    else if (parent is Frame)
                    {
                        var f = (parent as Frame);
                        if (f.CanGoBack)
                        {
                            f.GoBack();
                        }
                        else
                        {
                            f.Content = null;
                        }
                    }
                    else if (parent is ContentControl)
                    {
                        (parent as ContentControl).Content = null;
                    }
                    else if (parent is Page)
                    {
                        (parent as Page).Content = null;
                    }
                    else if (parent is UserControl)
                    {
                        (parent as UserControl).Content = null;
                    }

                }
#if WPF
                else if (view is Window)
                {
                    (view as Window).Close();
                }
#endif
                else
                {
                    view.Dispose();
                }



            }

        }
#if WPF

#elif NETCORE_FX
#elif WINDOWS_PHONE_7||WINDOWS_PHONE_8||WINDOWS_PHONE_7
#endif
#if WPF
        public class MVVMWindow : Window, IView
        {

            public MVVMWindow()
                : this(null)
            {
                //ViewModel = new DefaultViewModel ();
            }

            public MVVMWindow(IViewModel viewModel)
            {

                Loaded += async (_1, _2) =>
                {
                    if (viewModel != null)
                    {
                        if (!object.ReferenceEquals(ViewModel, viewModel))
                        {
                            ViewModel = viewModel;
                        }
                    }
                    await ViewModel.OnBindedViewLoad(this);
                };
            }



            public IViewModel ViewModel
            {
                get
                {
                    var rval = GetValue(ViewModelProperty) as IViewModel;
                    var c = this.CheckContent();
                    if (rval == null)
                    {

                        rval = c.DataContext as IViewModel;
                        SetValue(ViewModelProperty, rval);

                    }
                    else
                    {

                        if (!Object.ReferenceEquals(c.DataContext, rval))
                        {
                            c.DataContext = rval;
                        }
                    }
                    return rval;
                }
                set
                {
                    SetValue(ViewModelProperty, value);
                    var c = this.CheckContent();
                    c.DataContext = value;

                }
            }



            // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register("ViewModel", typeof(IViewModel), typeof(MVVMWindow), new PropertyMetadata(null, ViewHelper.ViewModelChangedCallback));


            public ViewType ViewType
            {
                get { return ViewType.Window; }
            }
            public void Dispose()
            {
                this.SelfClose();
            }



        }




#endif

#if WINDOWS_PHONE_7||WINDOWS_PHONE_8
        public class MVVMPage : PhoneApplicationPage, IView
#else
        public class MVVMPage : Page, IView
#endif
        {

            public MVVMPage()
                : this(null)
            {

            }

            public MVVMPage(IViewModel viewModel)
            {
                Loaded += async (_1, _2) =>
                {
                    if (viewModel != null)
                    {
                        if (!object.ReferenceEquals(ViewModel, viewModel))
                        {
                            ViewModel = viewModel;
                        }
                    }
                    await ViewModel.OnBindedViewLoad(this);
                };
            }




#if WINDOWS_PHONE_7||WINDOWS_PHONE_8||SILVERLIGHT_5||NETFX_CORE
            object IView.Content
            {
                get { return Content; }
                set { Content = value as FrameworkElement; }

            }

            protected override void OnNavigatedTo(NavigationEventArgs e)
            {
                base.OnNavigatedTo(e);
                RoutedEventHandler loadEvent = null;

                loadEvent = (_1, _2) =>
                {
                    EventRouting.EventRouter.Instance.RaiseEvent(this, e);
                    this.Loaded -= loadEvent;
                };
                this.Loaded += loadEvent;
            }
#endif






            public IViewModel ViewModel
            {
                get
                {
                    var rval = GetValue(ViewModelProperty) as IViewModel;
                    var c = this.CheckContent();
                    if (rval == null)
                    {

                        rval = c.DataContext as IViewModel;
                        SetValue(ViewModelProperty, rval);

                    }
                    else
                    {

                        if (!Object.ReferenceEquals(c.DataContext, rval))
                        {
                            c.DataContext = rval;
                        }
                    }
                    return rval;
                }
                set
                {

                    SetValue(ViewModelProperty, value);
                    var c = this.CheckContent();
                    c.DataContext = value;

                }
            }

            // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register("ViewModel", typeof(IViewModel), typeof(MVVMPage), new PropertyMetadata(null, ViewHelper.ViewModelChangedCallback));




            public ViewType ViewType
            {
                get { return ViewType.Page; }
            }

            public void Dispose()
            {

                this.SelfClose();


            }
        }



        public class MVVMControl : UserControl, IView
        {

            public MVVMControl()
                : this(null)
            {


            }
            public MVVMControl(IViewModel viewModel)
            {
                Loaded += async (_1, _2) =>
                {
                    if (viewModel != null)
                    {
                        if (!object.ReferenceEquals(ViewModel, viewModel))
                        {
                            ViewModel = viewModel;
                        }
                    }
                    await ViewModel.OnBindedViewLoad(this);
                };
            }
#if WINDOWS_PHONE_7||WINDOWS_PHONE_8||SILVERLIGHT_5||NETFX_CORE
            object IView.Content
            {
                get { return Content; }
                set { Content = value as FrameworkElement; }

            }
#endif
            //public MVVMPage WarpSelfByNewPage()
            //{
            //    lock (this)
            //    {
            //        if (this.IsWarppedToPage)
            //        {
            //            throw new InvalidOperationException("This  WarpSelfByNewPage() or WarpSelfByPage()functions can only be called by one time");
            //        }
            //        MVVMPage page = new MVVMPage(ViewModel);
            //        page.DisposeWith(ViewModel);
            //        page.Content = this;
            //        this.IsWarppedToPage = true;
            //        return page;
            //    }
            //}

            //public void WarpSelfByPage(MVVMPage page)
            //{
            //    lock (this)
            //    {
            //        if (this.IsWarppedToPage)
            //        {
            //            throw new InvalidOperationException("This WarpSelfByNewPage() or WarpSelfByPage()functions can only be called by one time");
            //        }
            //        page.ViewModel = ViewModel;
            //        page.DisposeWith(ViewModel);
            //        page.Content = this;
            //        this.IsWarppedToPage = true;

            //    }
            //}

            //public Page ParentPage
            //{
            //    get { return Parent as Page; }

            //}
            //public bool IsWarppedToPage { get; protected set; }

            public IViewModel ViewModel
            {
                get
                {
                    var rval = GetValue(ViewModelProperty) as IViewModel;
                    var c = this.CheckContent();
                    if (rval == null)
                    {

                        rval = c.DataContext as IViewModel;
                        SetValue(ViewModelProperty, rval);

                    }
                    else
                    {

                        if (!Object.ReferenceEquals(c.DataContext, rval))
                        {
                            c.DataContext = rval;
                        }
                    }
                    return rval;
                }
                set
                {
                    SetValue(ViewModelProperty, value);
                    var c = this.CheckContent();
                    c.DataContext = value;

                }
            }

            // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register("ViewModel", typeof(IViewModel), typeof(MVVMControl), new PropertyMetadata(null, ViewHelper.ViewModelChangedCallback));


            public ViewType ViewType
            {
                get { return ViewType.Control; }
            }
            public void Dispose()
            {
                this.SelfClose();
            }
        }
        public enum ViewType
        {
            Page,
            Window,
            Control
        }

        public interface IView : IDisposable
        {
            IViewModel ViewModel { get; set; }

            ViewType ViewType { get; }

            Object Content { get; set; }

            DependencyObject Parent { get; }

        }


        public interface IView<TViewModel> : IView, IDisposable where TViewModel : IViewModel
        {
            TViewModel SpecificTypedViewModel { get; set; }
        }

        public struct ViewModelToViewMapper<TModel>
            where TModel : BindableBase
        {

#if WPF
            public ViewModelToViewMapper<TModel> MapToDefault<TView>(TView instance) where TView : class,IView
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(instance);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapTo<TView>(string viewMappingKey, TView instance) where TView : class,IView
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, instance);
                return this;
            }


            public ViewModelToViewMapper<TModel> MapToDefault<TView>(bool alwaysNew = true) where TView : class,IView
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => (TView)Activator.CreateInstance(typeof(TView), d as object), alwaysNew);
                return this;
            }
            public ViewModelToViewMapper<TModel> MapTo<TView>(string viewMappingKey, bool alwaysNew = true) where TView : class,IView
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, d => (TView)Activator.CreateInstance(typeof(TView), d as object), alwaysNew);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapToDefault<TView>(Func<TModel, TView> factory, bool alwaysNew = true) where TView : class,IView
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => factory(d as TModel), alwaysNew);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapTo<TView>(string viewMappingKey, Func<TModel, TView> factory, bool alwaysNew = true) where TView : class,IView
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, d => factory(d as TModel), alwaysNew);
                return this;
            }
#else
            public ViewModelToViewMapper<TModel> MapToDefaultControl<TControl>(TControl instance) where TControl : MVVMControl
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(instance);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapToControl<TControl>(string viewMappingKey, TControl instance) where TControl : MVVMControl
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, instance);
                return this;
            }


            public ViewModelToViewMapper<TModel> MapToDefaultControl<TControl>(bool alwaysNew = true) where TControl : MVVMControl
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => (TControl)Activator.CreateInstance(typeof(TControl), d as object), alwaysNew);
                return this;
            }
            public ViewModelToViewMapper<TModel> MapToControl<TControl>(string viewMappingKey, bool alwaysNew = true) where TControl : MVVMControl
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, d => (TControl)Activator.CreateInstance(typeof(TControl), d as object), alwaysNew);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapToDefaultControl<TControl>(Func<TModel, TControl> factory, bool alwaysNew = true) where TControl : MVVMControl
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => factory(d as TModel), alwaysNew);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapToControl<TControl>(string viewMappingKey, Func<TModel, TControl> factory, bool alwaysNew = true) where TControl : MVVMControl
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, d => factory(d as TModel), alwaysNew);
                return this;
            }
#endif

#if WINDOWS_PHONE_8||WINDOWS_PHONE_7||SILVERLIGHT_5
            private static Uri GuessViewUri<TPage>(Uri baseUri) where TPage : MVVMPage
            {

                baseUri = baseUri ?? new Uri("/", UriKind.Relative);


                if (baseUri.IsAbsoluteUri)
                {
                    var path = Path.Combine(baseUri.LocalPath, typeof(TPage).Name + ".xaml");
                    UriBuilder ub = new UriBuilder(baseUri);
                    ub.Path = path;
                    return ub.Uri;
                }
                else
                {
                    var path = Path.Combine(baseUri.OriginalString, typeof(TPage).Name + ".xaml");
                    var pageUri = new Uri(path, UriKind.Relative);
                    return pageUri;
                }
            }

            public ViewModelToViewMapper<TModel> MapToDefault<TPage>(Uri baseUri = null) where TPage : MVVMPage
            {

                var pageUri = GuessViewUri<TPage>(baseUri);
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(pageUri);
                return this;
            }




            public ViewModelToViewMapper<TModel> MapTo<TPage>(string viewMappingKey, Uri baseUri = null) where TPage : MVVMPage
            {
                var pageUri = GuessViewUri<TPage>(baseUri);
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, pageUri);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapToDefault(Uri pageUri)
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(pageUri);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapTo(string viewMappingKey, Uri pageUri)
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, pageUri);
                return this;
            }
#endif
#if NETFX_CORE



            public ViewModelToViewMapper<TModel> MapToDefault<TPage>() where TPage : MVVMPage
            {


                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(typeof(TPage));
                return this;
            }


            public ViewModelToViewMapper<TModel> MapToDefault<TPage>(string viewMappingKey) where TPage : MVVMPage
            {

                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, typeof(TPage));
                return this;
            }



#endif


        }

        public static class ViewModelToViewMapperExtensions
        {

            public static ViewModelToViewMapper<TViewModel> GetViewMapper<TViewModel>(this MVVMSidekick.Services.ServiceLocatorEntryStruct<TViewModel> vmRegisterEntry)
                  where TViewModel : BindableBase
            {
                return new ViewModelToViewMapper<TViewModel>();
            }

        }
        public class ViewModelToViewMapperServiceLocator<TViewModel> : MVVMSidekick.Services.TypeSpecifiedServiceLocatorBase<ViewModelToViewMapperServiceLocator<TViewModel>, object>
        {
            static ViewModelToViewMapperServiceLocator()
            {
                Instance = new ViewModelToViewMapperServiceLocator<TViewModel>();
            }
            public static ViewModelToViewMapperServiceLocator<TViewModel> Instance { get; set; }


        }
        public class ViewModelLocator<TViewModel> : MVVMSidekick.Services.TypeSpecifiedServiceLocatorBase<ViewModelLocator<TViewModel>, TViewModel>
            where TViewModel : IViewModel
        {
            static ViewModelLocator()
            {
                Instance = new ViewModelLocator<TViewModel>();
            }
            public static ViewModelLocator<TViewModel> Instance { get; set; }

        }



        public class Stage : DependencyObject
        {
            public Stage(FrameworkElement target, string beaconKey, StageManager navigator)
            {
                Target = target;
                _navigator = navigator;
                BeaconKey = beaconKey;
                //SetupNavigateFrame();
            }

            StageManager _navigator;
            FrameworkElement _target;




            public Frame Frame
            {
                get { return (Frame)GetValue(FrameProperty); }
                private set { SetValue(FrameProperty, value); }
            }

            // Using a DependencyProperty as the backing store for Frame.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty FrameProperty =
                DependencyProperty.Register("Frame", typeof(Frame), typeof(Stage), new PropertyMetadata(null));



            public FrameworkElement Target
            {
                get { return _target; }
                private set
                {
                    _target = value;
                    Frame = _target as Frame;


                }
            }

            public bool IsGoBackSupported
            {
                get
                {
                    return Frame != null;
                }
            }


            public bool CanGoBack
            {
                get
                {
                    return IsGoBackSupported ? Frame.CanGoBack : false;
                }

            }




            public string BeaconKey
            {
                get { return (string)GetValue(BeaconKeyProperty); }
                private set { SetValue(BeaconKeyProperty, value); }
            }

            // Using a DependencyProperty as the backing store for BeaconKey.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty BeaconKeyProperty =
                DependencyProperty.Register("BeaconKey", typeof(string), typeof(Stage), new PropertyMetadata(""));


#if WPF
            private static IView InternalLocateViewIfNotSet<TTarget>(TTarget targetViewModel, string viewMappingKey, IView view) where TTarget : class, IViewModel
            {
                if (targetViewModel != null && targetViewModel.StageManager != null)
                {
                    view = targetViewModel.StageManager.CurrentBindingView as IView;

                }
                view = view ?? ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewMappingKey, targetViewModel) as IView;
                return view;
            }





            public async Task Show<TTarget>(TTarget targetViewModel = null, string viewMappingKey = null)
                 where TTarget : class,IViewModel
            {
                IView view = null;
                view = InternalLocateViewIfNotSet<TTarget>(targetViewModel, viewMappingKey, view);
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
                await targetViewModel.WaitForClose();
            }


            public async Task<TResult> Show<TTarget, TResult>(TTarget targetViewModel = null, string viewMappingKey = null)
                where TTarget : class,IViewModel<TResult>
            {
                IView view = null;
                view = InternalLocateViewIfNotSet<TTarget>(targetViewModel, viewMappingKey, view);
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
                return await targetViewModel.WaitForCloseWithResult();
            }



            public async Task<ShowAwaitableResult<TTarget>> ShowAndGetViewModel<TTarget>(TTarget targetViewModel = null, string viewMappingKey = null)
                where TTarget : class,IViewModel
            {
                IView view = null;
                view = InternalLocateViewIfNotSet<TTarget>(targetViewModel, viewMappingKey, view);

                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);


                return await TaskExHelper.FromResult(new ShowAwaitableResult<TTarget> { Closing = targetViewModel.WaitForClose(), ViewModel = targetViewModel });
            }
#endif
#if SILVERLIGHT_5||WINDOWS_PHONE_7||WINDOWS_PHONE_8

            public async Task Show<TTarget>(TTarget targetViewModel = null, string viewMappingKey = null)
                 where TTarget : class,IViewModel
            {

                var item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewMappingKey, targetViewModel);
                Uri uri;
                if ((uri = item as Uri) != null) //only sl like page Can be registered as uri
                {
                    Frame frame;
                    if ((frame = Target as Frame) != null)
                    {
                        var task = new Task(() => { });
                        var guid = Guid.NewGuid();
                        var newUriWithParameter = new Uri(uri.ToString() + "?CallBackGuid=" + guid.ToString(), UriKind.Relative);
                        using (EventRouting.EventRouter.Instance.GetEventObject<System.Windows.Navigation.NavigationEventArgs>()
                            .GetRouterEventObservable()
                            .Where(e =>
                                    e.EventArgs.Uri == newUriWithParameter)
                            .Subscribe(e =>
                            {
                                var page = e.Sender as MVVMPage;

                                if (targetViewModel != null) page.ViewModel = targetViewModel;
                                targetViewModel = (TTarget)page.ViewModel;
                                task.Start();
                            }
                            ))
                        {
                            frame.Navigate(newUriWithParameter);
                            await task;
                        }

                        await targetViewModel.WaitForClose();
                        return;
                    }

                }
                IView view = item as IView;
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
                await targetViewModel.WaitForClose();
            }

            public async Task<TResult> Show<TTarget, TResult>(TTarget targetViewModel = null, string viewMappingKey = null)
                where TTarget : class,IViewModel<TResult>
            {

                var item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewMappingKey, targetViewModel);
                Uri uri;
                if ((uri = item as Uri) != null) //only sl like page Can be registered as uri
                {
                    Frame frame;
                    if ((frame = Target as Frame) != null)
                    {
                        var task = new Task(() => { });
                        var guid = Guid.NewGuid();
                        var newUriWithParameter = new Uri(uri.ToString() + "?CallBackGuid=" + guid.ToString(), UriKind.Relative);
                        using (EventRouting.EventRouter.Instance.GetEventObject<System.Windows.Navigation.NavigationEventArgs>()
                            .GetRouterEventObservable()
                            .Where(e =>
                                    e.EventArgs.Uri == newUriWithParameter)
                            .Subscribe(e =>
                            {
                                var page = e.Sender as MVVMPage;
                                if (targetViewModel != null) page.ViewModel = targetViewModel;
                                targetViewModel = (TTarget)page.ViewModel;
                                task.Start();
                            }
                            ))
                        {
                            frame.Navigate(newUriWithParameter);
                            await task;
                        }

                        return await targetViewModel.WaitForCloseWithResult();
                    }

                }
                IView view = item as IView;
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
                return await targetViewModel.WaitForCloseWithResult();
            }



            public async Task<ShowAwaitableResult<TTarget>> ShowAndGetViewModel<TTarget>(TTarget targetViewModel = null, string viewMappingKey = null)
                where TTarget : class,IViewModel
            {
                var item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewMappingKey, targetViewModel);
                Uri uri;
                if ((uri = item as Uri) != null) //only sl like page Can be registered as uri
                {
                    Frame frame;
                    if ((frame = Target as Frame) != null)
                    {
                        var task = new Task(() => { });
                        var guid = Guid.NewGuid();
                        var newUriWithParameter = new Uri(uri.ToString() + "?CallBackGuid=" + guid.ToString(), UriKind.Relative);

                        using (EventRouting.EventRouter.Instance.GetEventObject<System.Windows.Navigation.NavigationEventArgs>()
                            .GetRouterEventObservable()
                            .Where(e =>
                                e.EventArgs.Uri.ToString().ToUpper() == newUriWithParameter.ToString().ToUpper())
                            .Subscribe(
                                e =>
                                {
                                    var page = e.Sender as MVVMPage;
                                    if (targetViewModel != null) page.ViewModel = targetViewModel;
                                    targetViewModel = (TTarget)page.ViewModel;
                                    task.Start();
                                }

                            ))
                        {
                            frame.Navigate(newUriWithParameter);
                            await task;
                        }

                        return new ShowAwaitableResult<TTarget> { Closing = targetViewModel.WaitForClose(), ViewModel = targetViewModel };

                    }

                }


                IView view = item as IView;
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);



                var tr = targetViewModel.WaitForClose();
                return new ShowAwaitableResult<TTarget> { Closing = targetViewModel.WaitForClose(), ViewModel = targetViewModel };
            }

#endif

#if NETFX_CORE


            public async Task Show<TTarget>(TTarget targetViewModel = null, string viewMappingKey = null)
                 where TTarget : class,IViewModel
            {


                var item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewMappingKey, targetViewModel);
                Type type;
                if ((type = item as Type) != null) //only MVVMPage Can be registered as Type
                {
                    Frame frame;

                    if ((frame = Target as Frame) != null)
                    {
                        Task task = new Task(() => { });
                        object paremeter = new object();


                        using (EventRouting.EventRouter.Instance.GetEventObject<NavigationEventArgs>()
                           .GetRouterEventObservable()
                           .Where(e =>
                                   object.ReferenceEquals(e.EventArgs.Parameter, paremeter))
                           .Subscribe(e =>
                           {
                               var page = e.Sender as MVVMPage;
                               if (targetViewModel != null) page.ViewModel = targetViewModel;
                               targetViewModel = (TTarget)page.ViewModel;
                               task.Start();
                           }))
                        {
                            frame.Navigate(type, paremeter);
                            await task;
                        }


                        await targetViewModel.WaitForClose();
                        return;
                    }

                }

                IView view = item as IView;
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
                await targetViewModel.WaitForClose();


            }

            public async Task<TResult> Show<TTarget, TResult>(TTarget targetViewModel = null, string viewMappingKey = null)
                where TTarget : class,IViewModel<TResult>
            {
                var item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewMappingKey, targetViewModel);
                Type type;
                if ((type = item as Type) != null) //only MVVMPage Can be registered as Type
                {
                    Frame frame;
                    if ((frame = Target as Frame) != null)
                    {
                        Task task = new Task(() => { });
                        object paremeter = new object();


                        using (EventRouting.EventRouter.Instance.GetEventObject<NavigationEventArgs>()
                           .GetRouterEventObservable()
                           .Where(e =>
                                   object.ReferenceEquals(e.EventArgs.Parameter, paremeter))
                           .Subscribe(e =>
                           {
                               var page = e.Sender as MVVMPage;
                               if (targetViewModel != null) page.ViewModel = targetViewModel;
                               targetViewModel = (TTarget)page.ViewModel;
                               task.Start();
                           }))
                        {
                            frame.Navigate(type, paremeter);
                            await task;
                        }




                        return await targetViewModel.WaitForCloseWithResult();
                    }

                }


                IView view = item as IView;
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
                return await targetViewModel.WaitForCloseWithResult();
            }



            public async Task<ShowAwaitableResult<TTarget>> ShowAndGetViewModel<TTarget>(TTarget targetViewModel = null, string viewMappingKey = null)
    where TTarget : class,IViewModel
            {
                var item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewMappingKey, targetViewModel);
                Type type;
                if ((type = item as Type) != null) //only MVVMPage Can be registered as Type
                {
                    Frame frame;
                    if ((frame = Target as Frame) != null)
                    {
                        Task task = new Task(() => { });
                        object paremeter = new object();


                        using (EventRouting.EventRouter.Instance.GetEventObject<NavigationEventArgs>()
                           .GetRouterEventObservable()
                           .Where(e =>
                                   object.ReferenceEquals(e.EventArgs.Parameter, paremeter))
                           .Subscribe(e =>
                           {
                               var page = e.Sender as MVVMPage;
                               if (targetViewModel != null) page.ViewModel = targetViewModel;
                               targetViewModel = (TTarget)page.ViewModel;
                               task.Start();
                           }))
                        {
                            frame.Navigate(type, paremeter);
                            await task;
                        }



                        return new ShowAwaitableResult<TTarget>
                        {
                            Closing = targetViewModel.WaitForClose(),
                            ViewModel = targetViewModel
                        };
                    }

                }


                IView view = item as IView;

                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);


                var tr = targetViewModel.WaitForClose();
                return new ShowAwaitableResult<TTarget> { Closing = tr, ViewModel = targetViewModel };
            }
#endif



            private void InternalShowView(IView view, FrameworkElement target, IViewModel sourceVM)
            {

                if (view is UserControl || view is Page)
                {

                    if (target is ContentControl)
                    {
                        var targetCControl = target as ContentControl;
                        var oldcontent = targetCControl.Content as IDisposable;


                        targetCControl.Content = view;
                    }
                    else if (target is Panel)
                    {
                        var targetPanelControl = target as Panel;

                        targetPanelControl.Children.Add(view as UIElement);
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("This view {0} is not support show in {1} ", view.GetType(), target.GetType()));
                    }
                }
#if WPF
                else if (view is Window)
                {
                    var viewWindow = view as Window;
                    viewWindow.HorizontalAlignment = HorizontalAlignment.Center;
                    viewWindow.VerticalAlignment = VerticalAlignment.Center;
                    var targetWindow = target as Window;
                    if (targetWindow == null)
                    {
                        targetWindow = sourceVM.StageManager.CurrentBindingView as Window;

                    }

                    viewWindow.Owner = targetWindow;
                    viewWindow.Show();

                }
#endif
            }


        }

        /// <summary>
        /// The abstract  for frame/contentcontrol. VM can access this class to Show other vm and vm's mapped view.
        /// </summary>
        public class StageManager : DependencyObject, IDisposable
        {
            static StageManager()
            {
                NavigatorBeaconsKey = "NavigatorBeaconsKey";
            }
            public StageManager(IViewModel viewModel)
            {
                _ViewModel = viewModel;


            }
            IViewModel _ViewModel;

            /// <summary>
            /// This Key is a prefix for register keys. 
            /// The stage registeration store the String-Element-Mapping in view's Resource Dictionary(Resource property). 
            /// This can help not to overwrite the resources already defined.
            /// </summary>
            public static string NavigatorBeaconsKey;



            IView _CurrentBindingView;
            /// <summary>
            /// Get the currently binded view of this stagemanager. A stagemanager is for a certain view. If viewmodel is not binded to a view, the whole thing cannot work.
            /// </summary>
            public IView CurrentBindingView
            {
                get
                {

                    return _CurrentBindingView;
                }
                internal set
                {
                    _CurrentBindingView = value;
                }
            }







            public void InitParent(Func<DependencyObject> parentLocator)
            {
                _parentLocator = parentLocator;
                DefaultStage = this[""];
            }



            Func<DependencyObject> _parentLocator;


            #region Attached Property




            public static string GetBeacon(DependencyObject obj)
            {
                return (string)obj.GetValue(BeaconProperty);
            }

            public static void SetBeacon(DependencyObject obj, string value)
            {
                obj.SetValue(BeaconProperty, value);
            }

            // Using a DependencyProperty as the backing store for Beacon.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty BeaconProperty =
                DependencyProperty.RegisterAttached("Beacon", typeof(string), typeof(StageManager), new PropertyMetadata(null,
                       (o, p) =>
                       {
                           var name = (p.NewValue as string);
                           var target = o as FrameworkElement;

                           target.Loaded +=
                               (_1, _2)
                               =>
                               {
                                   StageManager.RegisterTargetBeacon(name, target);
                               };
                       }

                       ));





            #endregion


            internal FrameworkElement LocateTargetContainer(IView view, ref string targetContainerName, IViewModel sourceVM)
            {
                targetContainerName = targetContainerName ?? "";
                var viewele = view as FrameworkElement;
                FrameworkElement target = null;



                var dic = GetOrCreateBeacons(sourceVM.StageManager.CurrentBindingView as FrameworkElement);
                dic.TryGetValue(targetContainerName, out target);


                if (target == null)
                {
                    target = _parentLocator() as FrameworkElement;
                }

                if (target == null)
                {
                    var vieweleCt = viewele as ContentControl;
                    if (vieweleCt != null)
                    {
                        target = vieweleCt.Content as FrameworkElement;
                    }
                }
                return target;
            }



            public void Dispose()
            {
                _ViewModel = null;
            }




            private static Dictionary<string, FrameworkElement> GetOrCreateBeacons(FrameworkElement view)
            {
                Dictionary<string, FrameworkElement> dic;
#if NETFX_CORE
                if (!view.Resources.ContainsKey(NavigatorBeaconsKey))
#else
                if (!view.Resources.Contains(NavigatorBeaconsKey))
#endif
                {
                    dic = new Dictionary<string, FrameworkElement>();
                    view.Resources.Add(NavigatorBeaconsKey, dic);
                }
                else
                    dic = view.Resources[NavigatorBeaconsKey] as Dictionary<string, FrameworkElement>;

                return dic;
            }

            public static void RegisterTargetBeacon(string name, FrameworkElement target)
            {
                var view = LocateIView(target);
                var beacons = GetOrCreateBeacons(view);


                beacons[name] = target;


            }

            private static FrameworkElement LocateIView(FrameworkElement target)
            {

                var view = target;

                while (view != null)
                {
                    if (view is IView)
                    {
                        break;
                    }
                    view = view.Parent as FrameworkElement;

                }
                return view;
            }







            public Stage DefaultStage
            {
                get { return (Stage)GetValue(DefaultTargetProperty); }
                set { SetValue(DefaultTargetProperty, value); }
            }

            // Using a DependencyProperty as the backing store for DefaultTarget.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty DefaultTargetProperty =
                DependencyProperty.Register("DefaultTarget", typeof(Stage), typeof(StageManager), new PropertyMetadata(null));



            public Stage this[string beaconKey]
            {
                get
                {
                    var fr = LocateTargetContainer(CurrentBindingView, ref beaconKey, _ViewModel);
                    if (fr != null)
                    {
                        return new Stage(fr, beaconKey, this);
                    }
                    else
                        return null;
                }
            }
        }



    }
}

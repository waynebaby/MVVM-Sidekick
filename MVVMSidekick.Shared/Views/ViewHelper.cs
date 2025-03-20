// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Views.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
#if !BLAZOR





namespace MVVMSidekick
{


    namespace Views
    {
        public static class ViewAndModelMappingsHelper
        {

            internal static Dictionary<(string Name, Type ViewType), (string Name, Type ViewModelType)> DefaultViewToVMMapping =
                new Dictionary<(string Name, Type ViewType), (string Name, Type ViewModelType)>();

            internal static Dictionary<(string Name, Type ViewModelType), (string Name, Type ViewType)> DefaultVMToViewMapping =
                new Dictionary<(string Name, Type ViewType), (string Name, Type ViewModelType)>();
            public static IViewModel GetDefaultViewModel(this IView view, string name = default)
            {
                IServiceProvider serviceProvider = ServiceProviderLocator.RootServiceProvider;
                Type viewType = null;
                switch (view)
                {
                    case PageViewDisguise pd:
                        viewType = pd.AssocatedObject.GetType();
                        break;
                    case ControlViewDisguise pd:
                        viewType = pd.AssocatedObject.GetType();
                        break;
#if WPF
                    case WindowViewDisguise pd:
                        viewType = pd.AssocatedObject.GetType();
                        break;
#endif
                    default:
                        viewType = view?.GetType();
                        break;
                }

                var (_, viewModelType) = DefaultViewToVMMapping[(name, viewType)];
                return serviceProvider.GetService(name, viewModelType) as IViewModel;


            }
        }
        /// <summary>
        /// Class ViewHelper.
        /// </summary>
        public static class ViewHelper
        {
            /// <summary>
            /// The default vm name
            /// </summary>
            public const string DefaultVMName = "DesignVM";
            /// <summary>
            /// Gets the default designing view model.
            /// </summary>
            /// <param name="view">The view.</param>
            /// <returns>System.Object.</returns>
            public static object GetDefaultDesigningViewModel(this IView view)
            {


                var f = view as FrameworkElement;
                object rval = null;
#if WINDOWS_UWP ||WinUI3
                if (!f?.Resources.ContainsKey(DefaultVMName) ?? false)
#elif WPF
				if (!f?.Resources.Contains(DefaultVMName)??false)
#endif
                {
                    return null;
                }
                else
                {
                    rval = f.Resources[DefaultVMName];
                }
                return rval;

            }

            /// <summary>
            /// The view unload call back
            /// </summary>
            internal static RoutedEventHandler ViewUnloadCallBack
                = async (o, e) =>
                {
                    IView v = o as IView;
                    if (v == null)
                    {
                        var dp = o as DependencyObject;
                        v = dp.GetViewDisguise();
                    }
                    if (v != null)
                    {
                        var m = v.ViewModel as IViewModelLifetime;
                        if (m != null)
                        {
                            await m.OnBindedViewUnload(v);
                        }
                    }
                };

            /// <summary>
            /// The view load call back
            /// </summary>
            internal static RoutedEventHandler ViewLoadCallBack
                = async (o, e) =>
                {
                    IView v = o as IView;
                    if (v == null)
                    {
                        var dp = o as DependencyObject;
                        v = dp.GetViewDisguise();
                    }
                    if (v != null)
                    {
                        var m = v.ViewModel as IViewModelLifetime;
                        if (m != null)
                        {
                            await m.OnBindedViewLoad(v);
                        }
                    }
                };



            /// <summary>
            /// The view model changed callback
            /// </summary>
            internal static PropertyChangedCallback ViewModelChangedCallback
                = (o, e) =>
                {
                    dynamic item = o;
                    var oiview = o as IView;
                    var fele = (oiview.ViewContentObject as FrameworkElement);
                    if (fele == null)
                    {
                        return;
                    }
                    if (object.ReferenceEquals(fele.DataContext, e.NewValue))
                    {
                        return;
                    }
                    (oiview.ViewContentObject as FrameworkElement).DataContext = e.NewValue;
                    var nv = e.NewValue as IViewModel;
                    var ov = e.OldValue as IViewModel;
                    if (ov != null)
                    {
                        ov.OnUnbindedFromView(oiview, nv);
                    }
                    if (nv != null)
                    {
                        nv.OnBindedToView(oiview, ov);
                    }

                };

            /// <summary>
            /// Gets the content and create if null.
            /// </summary>
            /// <param name="control">The control.</param>
            /// <returns>FrameworkElement.</returns>
            internal static FrameworkElement GetContentAndCreateIfNull(this IView control)
            {
                var c = (control.ViewContentObject as FrameworkElement);
                if (c == null)
                {
                    control.ViewContentObject = c = new Grid();
                }
                return c;
            }

            /// <summary>
            /// Selfs the close.
            /// </summary>
            /// <param name="view">The view.</param>
            public static void SelfClose(this IView view)
            {

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


            }

        }



    }
}
#endif
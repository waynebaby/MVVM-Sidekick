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
        /// <summary>
        /// <para>视图和模型映射帮助器，提供视图与视图模型之间的默认映射功能</para>
        /// <para>View and model mapping helper that provides default mapping functionality between views and view models</para>
        /// </summary>
        public static class ViewAndModelMappingsHelper
        {
            /// <summary>
            /// <para>默认视图到视图模型的映射字典</para>
            /// <para>Default view to view model mapping dictionary</para>
            /// </summary>
            internal static Dictionary<(string Name, Type ViewType), (string Name, Type ViewModelType)> DefaultViewToVMMapping =
                new Dictionary<(string Name, Type ViewType), (string Name, Type ViewModelType)>();

            /// <summary>
            /// <para>默认视图模型到视图的映射字典</para>
            /// <para>Default view model to view mapping dictionary</para>
            /// </summary>
            internal static Dictionary<(string Name, Type ViewModelType), (string Name, Type ViewType)> DefaultVMToViewMapping =
                new Dictionary<(string Name, Type ViewType), (string Name, Type ViewModelType)>();
                
            /// <summary>
            /// <para>获取视图的默认视图模型实例</para>
            /// <para>Gets the default view model instance for the view</para>
            /// </summary>
            /// <param name="view">
            /// <para>目标视图对象</para>
            /// <para>The target view object</para>
            /// </param>
            /// <param name="name">
            /// <para>视图模型名称，默认为null</para>
            /// <para>The view model name, default is null</para>
            /// </param>
            /// <returns>
            /// <para>视图模型实例</para>
            /// <para>The view model instance</para>
            /// </returns>
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
        /// <para>视图帮助器类，提供视图相关的实用方法和回调函数</para>
        /// <para>View helper class that provides view-related utility methods and callback functions</para>
        /// </summary>
        /// <remarks>
        /// <para>Class ViewHelper.</para>
        /// </remarks>
        public static class ViewHelper
        {
            /// <summary>
            /// <para>默认视图模型名称常量</para>
            /// <para>Default view model name constant</para>
            /// </summary>
            /// <remarks>
            /// <para>The default vm name</para>
            /// </remarks>
            public const string DefaultVMName = "DesignVM";
            
            /// <summary>
            /// <para>获取视图的默认设计时视图模型</para>
            /// <para>Gets the default designing view model for the view</para>
            /// </summary>
            /// <param name="view">
            /// <para>目标视图对象</para>
            /// <para>The view object</para>
            /// </param>
            /// <returns>
            /// <para>设计时视图模型对象，如果未找到则返回null</para>
            /// <para>The designing view model object, or null if not found</para>
            /// </returns>
            /// <remarks>
            /// <para>Gets the default designing view model.</para>
            /// </remarks>
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
            /// <para>视图卸载回调函数，处理视图模型的生命周期事件</para>
            /// <para>View unload callback function that handles view model lifecycle events</para>
            /// </summary>
            /// <remarks>
            /// <para>The view unload call back</para>
            /// </remarks>
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
            /// <para>视图加载回调函数，处理视图模型的生命周期事件</para>
            /// <para>View load callback function that handles view model lifecycle events</para>
            /// </summary>
            /// <remarks>
            /// <para>The view load call back</para>
            /// </remarks>
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
            /// <para>视图模型变更回调函数，处理DataContext和视图模型绑定关系</para>
            /// <para>View model changed callback function that handles DataContext and view model binding relationships</para>
            /// </summary>
            /// <remarks>
            /// <para>The view model changed callback</para>
            /// </remarks>
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
            /// <para>获取内容并在为空时创建新内容，确保视图有有效的内容对象</para>
            /// <para>Gets the content and creates if null, ensuring the view has a valid content object</para>
            /// </summary>
            /// <param name="control">
            /// <para>目标控件视图</para>
            /// <para>The target control view</para>
            /// </param>
            /// <returns>
            /// <para>框架元素内容对象</para>
            /// <para>The framework element content object</para>
            /// </returns>
            /// <remarks>
            /// <para>Gets the content and create if null.</para>
            /// </remarks>
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
            /// <para>执行视图的自关闭操作，根据视图类型和父容器类型处理关闭逻辑</para>
            /// <para>Performs the view's self-close operation, handling close logic based on view type and parent container type</para>
            /// </summary>
            /// <param name="view">
            /// <para>要关闭的视图对象</para>
            /// <para>The view object to close</para>
            /// </param>
            /// <remarks>
            /// <para>Selfs the close.</para>
            /// </remarks>
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
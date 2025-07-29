/// <summary>
/// <para>视图模型到视图映射器文件 - 此文件中的代码当前被注释，可能是遗留或备用实现</para>
/// <para>View model to view mapper file - The code in this file is currently commented out, possibly legacy or backup implementation</para>
/// </summary>
/// <remarks>
/// <para>这个文件包含了ViewModelToViewMapper泛型类的实现，用于建立视图模型与视图之间的映射关系</para>
/// <para>This file contains the implementation of ViewModelToViewMapper generic class for establishing mapping relationships between view models and views</para>
/// <para>支持多平台包括WPF、UWP、Silverlight、Windows Phone等</para>
/// <para>Supports multiple platforms including WPF, UWP, Silverlight, Windows Phone, etc.</para>
/// <para>由于代码被完全注释，可能表示此功能已被其他实现替代或暂时禁用</para>
/// <para>Since the code is completely commented out, it may indicate this functionality has been replaced by other implementations or temporarily disabled</para>
/// </remarks>

//#if !BLAZOR

//using System;
//using MVVMSidekick.ViewModels;



//#if WINDOWS_UWP


//#elif WPF
//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows;
//using System.Collections.Concurrent;
//using System.Windows.Navigation;

//using MVVMSidekick.Views;
//using System.Windows.Controls.Primitives;
//using MVVMSidekick.Utilities;
//#elif SILVERLIGHT_5 || SILVERLIGHT_4
//						   using System.Windows.Media;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Navigation;
//using System.Windows.Controls.Primitives;
//#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
//using System.Windows.Media;
//using System.Windows.Controls;
//using Microsoft.Phone.Controls;
//using System.Windows.Data;
//using System.Windows.Navigation;
//using System.Windows.Controls.Primitives;
//#endif




//namespace MVVMSidekick
//{


//    namespace Views
//    {
//        /// <summary>
//        /// <para>视图模型到视图映射器类，用于建立视图模型与视图之间的映射关系</para>
//        /// <para>View model to view mapper class for establishing mapping relationships between view models and views</para>
//        /// </summary>
//        /// <typeparam name="TModel">
//        /// <para>视图模型类型，必须实现IViewModel接口</para>
//        /// <para>The type of the view model that must implement IViewModel interface</para>
//        /// </typeparam>
//        /// <remarks>
//        /// <para>Struct ViewModelToViewMapper</para>
//        /// </remarks>
//        public class ViewModelToViewMapper<TModel>
//            where TModel : IViewModel
//        {
//
//			/// <summary>
//			/// <para>将视图映射到视图模型，建立类型关联</para>
//			/// <para>Maps the view to view model, establishing type association</para>
//			/// </summary>
//			/// <typeparam name="TView">
//			/// <para>视图类型</para>
//			/// <para>The type of the view</para>
//			/// </typeparam>
//			/// <remarks>
//			/// <para>Maps the view to view model.</para>
//			/// </remarks>
//#pragma warning disable CA1000 // Do not declare static members on generic types
//			public static void MapViewToViewModel<TView>()
//#pragma warning restore CA1000 // Do not declare static members on generic types
//			{
//                Func<IViewModel> func;
//                if (!ViewModelToViewMapperHelper.ViewToVMMapping.TryGetValue(typeof(TView), out func))
//                {
//                    ViewModelToViewMapperHelper.ViewToVMMapping.Add(typeof(TView), () => (ViewModelLocator<TModel>.Instance.Resolve()));
//                }

//            }
//#if WPF
//			/// <summary>
//			/// Maps to default.
//			/// </summary>
//			/// <typeparam name="TView">The type of the view.</typeparam>
//			/// <param name="instance">The instance.</param>
//			/// <returns></returns>
//			public ViewModelToViewMapper<TModel> MapToDefault<TView>(TView instance) where TView : FrameworkElement
//			{
//				MapViewToViewModel<TView>();
//				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(instance);
//				return this;
//			}

//			/// <summary>
//			/// Maps to.
//			/// </summary>
//			/// <typeparam name="TView">The type of the view.</typeparam>
//			/// <param name="viewMappingKey">The view mapping key.</param>
//			/// <param name="instance">The instance.</param>
//			/// <returns></returns>
//			public ViewModelToViewMapper<TModel> MapTo<TView>(string viewMappingKey, TView instance) where TView :  FrameworkElement
//			{
//				MapViewToViewModel<TView>();
//				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, instance);
//				return this;
//			}


//			/// <summary>
//			/// Maps to default.
//			/// </summary>
//			/// <typeparam name="TView">The type of the view.</typeparam>
//			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
//			/// <returns></returns>
//			public ViewModelToViewMapper<TModel> MapToDefault<TView>(bool alwaysNew = true) where TView :  FrameworkElement
//			{
//				MapViewToViewModel<TView>();
//				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => (TView)Activator.CreateInstance(typeof(TView)), alwaysNew);
//				return this;
//			}
//			/// <summary>
//			/// Maps to.
//			/// </summary>
//			/// <typeparam name="TView">The type of the view.</typeparam>
//			/// <param name="viewMappingKey">The view mapping key.</param>
//			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
//			/// <returns></returns>
//			public ViewModelToViewMapper<TModel> MapTo<TView>(string viewMappingKey, bool alwaysNew = true) where TView :  FrameworkElement
//			{
//				MapViewToViewModel<TView>();
//				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, d => (TView)Activator.CreateInstance(typeof(TView)), alwaysNew);
//				return this;
//			}

//			/// <summary>
//			/// Maps to default.
//			/// </summary>
//			/// <typeparam name="TView">The type of the view.</typeparam>
//			/// <param name="factory">The factory.</param>
//			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
//			/// <returns></returns>
//			public ViewModelToViewMapper<TModel> MapToDefault<TView>(Func<TModel, TView> factory, bool alwaysNew = true) where TView :  FrameworkElement
//			{
//				MapViewToViewModel<TView>();
//				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => factory((TModel)d), alwaysNew);
//				return this;
//			}

//			/// <summary>
//			/// Maps to.
//			/// </summary>
//			/// <typeparam name="TView">The type of the view.</typeparam>
//			/// <param name="viewMappingKey">The view mapping key.</param>
//			/// <param name="factory">The factory.</param>
//			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
//			/// <returns></returns>
//			public ViewModelToViewMapper<TModel> MapTo<TView>(string viewMappingKey, Func<TModel, TView> factory, bool alwaysNew = true) where TView :  Control
//			{
//				MapViewToViewModel<TView>();
//				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, d => factory((TModel)d), alwaysNew);
//				return this;
//			}
//#elif WINDOWS_UWP




//#endif

//#if WINDOWS_PHONE_8 || WINDOWS_PHONE_7 || SILVERLIGHT_5
//			private static Uri GuessViewUri<TPage>(Uri baseUri) where TPage : MVVMPage
//			{
//				MapViewToViewModel<TPage>();

//				baseUri = baseUri ?? new Uri("/", UriKind.Relative);


//				if (baseUri.IsAbsoluteUri)
//				{
//					var path = Path.Combine(baseUri.LocalPath, typeof(TPage).Name + ".xaml");
//					UriBuilder ub = new UriBuilder(baseUri);
//					ub.Path = path;
//					return ub.Uri;
//				}
//				else
//				{
//					var path = Path.Combine(baseUri.OriginalString, typeof(TPage).Name + ".xaml");
//					var pageUri = new Uri(path, UriKind.Relative);
//					return pageUri;
//				}
//			}
//			/// <summary>
//			/// Maps to default.
//			/// </summary>
//			/// <typeparam name="TPage">The type of the page.</typeparam>
//			/// <param name="baseUri">The base URI.</param>
//			/// <returns></returns>
//			public ViewModelToViewMapper<TModel> MapToDefault<TPage>(Uri baseUri = null) where TPage : MVVMPage
//			{

//				MapViewToViewModel<TPage>();
//				var pageUri = GuessViewUri<TPage>(baseUri);
//				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(
//					Tuple.Create<Uri, Func<IView>>(pageUri,
//					() => Activator.CreateInstance(typeof(TPage)) as IView));

//				return this;
//			}




//			/// <summary>
//			/// Maps to.
//			/// </summary>
//			/// <typeparam name="TPage">The type of the page.</typeparam>
//			/// <param name="viewMappingKey">The view mapping key.</param>
//			/// <param name="baseUri">The base URI.</param>
//			/// <returns></returns>
//			public ViewModelToViewMapper<TModel> MapTo<TPage>(string viewMappingKey, Uri baseUri = null) where TPage : MVVMPage
//			{
//				MapViewToViewModel<TPage>();
//				var pageUri = GuessViewUri<TPage>(baseUri);
//				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(Tuple.Create<Uri, Func<IView>>(pageUri,
//						() => Activator.CreateInstance(typeof(TPage)) as IView
//						));
//				return this;
//			}


//#endif
//#if WINDOWS_UWP


//            /// <summary>
//            ///    Map to default constructor
//            /// </summary>
//            /// <typeparam name="TPage"></typeparam>
//            /// <returns></returns>
//            public ViewModelToViewMapper<TModel> MapToDefault<TPage>() where TPage : Windows.UI.Xaml.Controls.Page
//            {

//                MapViewToViewModel<TPage>();

//                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(typeof(TPage));
//                return this;
//            }

//            /// <summary>
//            ///   Map to   default constructor with mapping key
//            /// </summary>
//            /// <param name="viewMappingKey">mapping key</param>
//            /// <returns></returns>
//            public ViewModelToViewMapper<TModel> MapToDefault<TPage>(string viewMappingKey) where TPage : Windows.UI.Xaml.Controls.Page
//            {

//                MapViewToViewModel<TPage>();
//                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, typeof(TPage));
//                return this;
//            }



//#endif


//		}



//    }
//}
//#endif
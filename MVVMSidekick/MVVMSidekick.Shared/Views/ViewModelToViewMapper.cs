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
using System;
using MVVMSidekick.ViewModels;



#if NETFX_CORE


#elif WPF
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
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




namespace MVVMSidekick
{


    namespace Views
    {
        /// <summary>
        /// Struct ViewModelToViewMapper
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        public struct ViewModelToViewMapper<TModel>
            where TModel : IViewModel
        {

            /// <summary>
            /// Maps the view to view model.
            /// </summary>
            /// <typeparam name="TView">The type of the t view.</typeparam>
            public static void MapViewToViewModel<TView>()
            {
                Func<IViewModel> func;
                if (!ViewModelToViewMapperHelper.ViewToVMMapping.TryGetValue(typeof(TView), out func))
                {
                    ViewModelToViewMapperHelper.ViewToVMMapping.Add(typeof(TView), () => (ViewModelLocator<TModel>.Instance.Resolve()));
                }

            }
#if WPF
			/// <summary>
			/// Maps to default.
			/// </summary>
			/// <typeparam name="TView">The type of the view.</typeparam>
			/// <param name="instance">The instance.</param>
			/// <returns></returns>
			public ViewModelToViewMapper<TModel> MapToDefault<TView>(TView instance) where TView : FrameworkElement
			{
				MapViewToViewModel<TView>();
				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(instance);
				return this;
			}

			/// <summary>
			/// Maps to.
			/// </summary>
			/// <typeparam name="TView">The type of the view.</typeparam>
			/// <param name="viewMappingKey">The view mapping key.</param>
			/// <param name="instance">The instance.</param>
			/// <returns></returns>
			public ViewModelToViewMapper<TModel> MapTo<TView>(string viewMappingKey, TView instance) where TView :  FrameworkElement
			{
				MapViewToViewModel<TView>();
				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, instance);
				return this;
			}


			/// <summary>
			/// Maps to default.
			/// </summary>
			/// <typeparam name="TView">The type of the view.</typeparam>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns></returns>
			public ViewModelToViewMapper<TModel> MapToDefault<TView>(bool alwaysNew = true) where TView :  FrameworkElement
			{
				MapViewToViewModel<TView>();
				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => (TView)Activator.CreateInstance(typeof(TView)), alwaysNew);
				return this;
			}
			/// <summary>
			/// Maps to.
			/// </summary>
			/// <typeparam name="TView">The type of the view.</typeparam>
			/// <param name="viewMappingKey">The view mapping key.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns></returns>
			public ViewModelToViewMapper<TModel> MapTo<TView>(string viewMappingKey, bool alwaysNew = true) where TView :  FrameworkElement
			{
				MapViewToViewModel<TView>();
				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, d => (TView)Activator.CreateInstance(typeof(TView)), alwaysNew);
				return this;
			}

			/// <summary>
			/// Maps to default.
			/// </summary>
			/// <typeparam name="TView">The type of the view.</typeparam>
			/// <param name="factory">The factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns></returns>
			public ViewModelToViewMapper<TModel> MapToDefault<TView>(Func<TModel, TView> factory, bool alwaysNew = true) where TView :  FrameworkElement
			{
				MapViewToViewModel<TView>();
				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => factory((TModel)d), alwaysNew);
				return this;
			}

			/// <summary>
			/// Maps to.
			/// </summary>
			/// <typeparam name="TView">The type of the view.</typeparam>
			/// <param name="viewMappingKey">The view mapping key.</param>
			/// <param name="factory">The factory.</param>
			/// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
			/// <returns></returns>
			public ViewModelToViewMapper<TModel> MapTo<TView>(string viewMappingKey, Func<TModel, TView> factory, bool alwaysNew = true) where TView :  Control
			{
				MapViewToViewModel<TView>();
				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, d => factory((TModel)d), alwaysNew);
				return this;
			}
#else
            /// <summary>
            /// Maps to default control.
            /// </summary>
            /// <typeparam name="TControl">The type of the control.</typeparam>
            /// <param name="instance">The instance.</param>
            /// <returns></returns>
            public ViewModelToViewMapper<TModel> MapToDefaultControl<TControl>(TControl instance) where TControl : MVVMControl
            {
                MapViewToViewModel<TControl>();
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(instance);
                return this;
            }

            /// <summary>
            /// Maps to control.
            /// </summary>
            /// <typeparam name="TControl">The type of the control.</typeparam>
            /// <param name="viewMappingKey">The view mapping key.</param>
            /// <param name="instance">The instance.</param>
            /// <returns></returns>
            public ViewModelToViewMapper<TModel> MapToControl<TControl>(string viewMappingKey, TControl instance) where TControl : MVVMControl
            {
                MapViewToViewModel<TControl>();
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, instance);
                return this;
            }


            /// <summary>
            /// Maps to default control.
            /// </summary>
            /// <typeparam name="TControl">The type of the control.</typeparam>
            /// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
            /// <returns></returns>
            public ViewModelToViewMapper<TModel> MapToDefaultControl<TControl>(bool alwaysNew = true) where TControl : MVVMControl
            {
                MapViewToViewModel<TControl>();
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => (TControl)Activator.CreateInstance(typeof(TControl)), alwaysNew);
                return this;
            }
            /// <summary>
            /// Maps to control.
            /// </summary>
            /// <typeparam name="TControl">The type of the control.</typeparam>
            /// <param name="viewMappingKey">The view mapping key.</param>
            /// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
            /// <returns></returns>
            public ViewModelToViewMapper<TModel> MapToControl<TControl>(string viewMappingKey, bool alwaysNew = true) where TControl : MVVMControl
            {
                MapViewToViewModel<TControl>();
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, d => (TControl)Activator.CreateInstance(typeof(TControl)), alwaysNew);
                return this;
            }

            /// <summary>
            /// Maps to default control.
            /// </summary>
            /// <typeparam name="TControl">The type of the control.</typeparam>
            /// <param name="factory">The factory.</param>
            /// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
            /// <returns></returns>
            public ViewModelToViewMapper<TModel> MapToDefaultControl<TControl>(Func<TModel, TControl> factory, bool alwaysNew = true) where TControl : MVVMControl
            {
                MapViewToViewModel<TControl>();
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => factory((TModel)d), alwaysNew);
                return this;
            }

            /// <summary>
            /// Maps to control.
            /// </summary>
            /// <typeparam name="TControl">The type of the control.</typeparam>
            /// <param name="viewMappingKey">The view mapping key.</param>
            /// <param name="factory">The factory.</param>
            /// <param name="alwaysNew">if set to <c>true</c> [always new].</param>
            /// <returns></returns>
            public ViewModelToViewMapper<TModel> MapToControl<TControl>(string viewMappingKey, Func<TModel, TControl> factory, bool alwaysNew = true) where TControl : MVVMControl
            {
                MapViewToViewModel<TControl>();
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, d => factory((TModel)d), alwaysNew);
                return this;
            }
#endif

#if WINDOWS_PHONE_8 || WINDOWS_PHONE_7 || SILVERLIGHT_5
			private static Uri GuessViewUri<TPage>(Uri baseUri) where TPage : MVVMPage
			{
				MapViewToViewModel<TPage>();

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
			/// <summary>
			/// Maps to default.
			/// </summary>
			/// <typeparam name="TPage">The type of the page.</typeparam>
			/// <param name="baseUri">The base URI.</param>
			/// <returns></returns>
			public ViewModelToViewMapper<TModel> MapToDefault<TPage>(Uri baseUri = null) where TPage : MVVMPage
			{

				MapViewToViewModel<TPage>();
				var pageUri = GuessViewUri<TPage>(baseUri);
				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(
					Tuple.Create<Uri, Func<IView>>(pageUri,
					() => Activator.CreateInstance(typeof(TPage)) as IView));

				return this;
			}




			/// <summary>
			/// Maps to.
			/// </summary>
			/// <typeparam name="TPage">The type of the page.</typeparam>
			/// <param name="viewMappingKey">The view mapping key.</param>
			/// <param name="baseUri">The base URI.</param>
			/// <returns></returns>
			public ViewModelToViewMapper<TModel> MapTo<TPage>(string viewMappingKey, Uri baseUri = null) where TPage : MVVMPage
			{
				MapViewToViewModel<TPage>();
				var pageUri = GuessViewUri<TPage>(baseUri);
				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(Tuple.Create<Uri, Func<IView>>(pageUri,
						() => Activator.CreateInstance(typeof(TPage)) as IView
						));
				return this;
			}


#endif
#if NETFX_CORE


            /// <summary>
            ///    Map to default constructor
            /// </summary>
            /// <typeparam name="TPage"></typeparam>
            /// <returns></returns>
            public ViewModelToViewMapper<TModel> MapToDefault<TPage>() where TPage : Windows.UI.Xaml.Controls.Page
            {

                MapViewToViewModel<TPage>();

                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(typeof(TPage));
                return this;
            }

            /// <summary>
            ///   Map to   default constructor with mapping key
            /// </summary>
            /// <param name="viewMappingKey">mapping key</param>
            /// <returns></returns>
            public ViewModelToViewMapper<TModel> MapToDefault<TPage>(string viewMappingKey) where TPage : Windows.UI.Xaml.Controls.Page
            {

                MapViewToViewModel<TPage>();
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, typeof(TPage));
                return this;
            }



#endif


        }



    }
}

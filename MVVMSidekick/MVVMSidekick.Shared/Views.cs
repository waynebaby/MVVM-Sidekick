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




namespace MVVMSidekick
{


	namespace Views
	{


		/// <summary>
		/// Class ViewHelper.
		/// </summary>
		public static class ViewHelper
		{
			/// <summary>
			/// The default vm name
			/// </summary>
			public static readonly string DEFAULT_VM_NAME = "DesignVM";
			/// <summary>
			/// Gets the default designing view model.
			/// </summary>
			/// <param name="view">The view.</param>
			/// <returns>System.Object.</returns>
			public static object GetDefaultDesigningViewModel(this IView view)
			{
				var f = view as FrameworkElement;
				object rval = null;
#if NETFX_CORE
				if (!f.Resources.ContainsKey(DEFAULT_VM_NAME))
#else
				if (!f.Resources.Contains(DEFAULT_VM_NAME))
#endif
				{
					return null;
				}
				else
				{
					rval = f.Resources[DEFAULT_VM_NAME];
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
			/// The designing view model changed call back
			/// </summary>
			internal static PropertyChangedCallback DesigningViewModelChangedCallBack
				= (o, e) =>
					  {
						  var oiview = o as IView;
						  if (Utilities.Runtime.IsInDesignMode)
						  {
							  oiview.ViewModel = e.NewValue as IViewModel;
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
					var fele = (oiview.ContentObject as FrameworkElement);
					if (fele == null)
					{
						return;
					}
					if (object.ReferenceEquals(fele.DataContext, e.NewValue))
					{
						return;
					}
					(oiview.ContentObject as FrameworkElement).DataContext = e.NewValue;
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
				var c = (control.ContentObject as FrameworkElement);
				if (c == null)
				{
					control.ContentObject = c = new Grid();
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

#if WPF
		/// <summary>
		///  MVVM Window  class
		/// </summary>
		public class MVVMWindow : Window, IView
		{

			/// <summary>
			///    MVVM Window constructor
			/// </summary>
			public MVVMWindow()
				//: this(null)
			{
				Loaded += ViewHelper.ViewLoadCallBack;
				Unloaded += ViewHelper.ViewUnloadCallBack;
			}


			/// <summary>
			/// Is auto owner set needed.  if true, set window's owner to parent view window.
			/// </summary>
			public bool IsAutoOwnerSetNeeded
			{
				get { return (bool)GetValue(IsAutoOwnerSetNeededProperty); }
				set { SetValue(IsAutoOwnerSetNeededProperty, value); }
			}

			// Using a DependencyProperty as the backing store for IsAutoOwnerSetNeeded.  This enables animation, styling, binding, etc...

			/// <summary>
			/// Is auto owner set needed property
			/// </summary>
			public static readonly DependencyProperty IsAutoOwnerSetNeededProperty =
				DependencyProperty.Register("IsAutoOwnerSetNeeded", typeof(bool), typeof(MVVMWindow), new PropertyMetadata(true));



			///// <summary>
			/////  MVVM Window constructor
			///// </summary>
			///// <param name="viewModel"> view model</param>
			//public MVVMWindow(IViewModel viewModel)
			//{
			//	ViewModel = viewModel;
			//	Unloaded += ViewHelper.ViewUnloadCallBack;
			//	Loaded += async (_1, _2) =>
			//	{

			//		if (viewModel != null)
			//		{
			//			if (!object.ReferenceEquals(ViewModel, viewModel))
			//			{
			//				ViewModel = viewModel;
			//			}
			//		}													   

			//		if (ViewModel != null)
			//		{
			//			await ViewModel.OnBindedViewLoad(this);
			//		}
			//	};
			//}

			/// <summary>
			/// the first content object of view.
			/// </summary>
			public object ContentObject
			{
				get
				{
					return Content;
				}
				set
				{
					Content = value;
				}
			}




			//public IViewModel DesigningViewModel
			//{
			//	get { return (IViewModel)GetValue(DesigningViewModelProperty); }
			//	set { SetValue(DesigningViewModelProperty, value); }
			//}

			//// Using a DependencyProperty as the backing store for DesigningViewModel.  This enables animation, styling, binding, etc...
			//public static readonly DependencyProperty DesigningViewModelProperty =
			//	DependencyProperty.Register("DesigningViewModel", typeof(IViewModel), typeof(MVVMWindow), new PropertyMetadata(null, ViewHelper.DesigningViewModelChangedCallBack));


			/// <summary>
			/// View Model
			/// </summary>
			public IViewModel ViewModel
			{
				get
				{
					var rval = GetValue(ViewModelProperty) as IViewModel;
					var c = this.GetContentAndCreateIfNull();
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
					var c = this.GetContentAndCreateIfNull();
					if (!Object.ReferenceEquals(c.DataContext, value))
					{
						c.DataContext = value;
					}

				}
			}



			// Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...

			/// <summary>
			/// View Model Property
			/// </summary>
			public static readonly DependencyProperty ViewModelProperty =
				DependencyProperty.Register("ViewModel", typeof(IViewModel), typeof(MVVMWindow), new PropertyMetadata(null, ViewHelper.ViewModelChangedCallback));

			/// <summary>
			/// Type of View
			/// </summary>
			public ViewType ViewType
			{
				get { return ViewType.Window; }
			}




		}




#endif

#if WINDOWS_PHONE_7 || WINDOWS_PHONE_8
		/// <summary>
		/// Class MVVMPage.
		/// </summary>
		public partial class MVVMPage : PhoneApplicationPage, IView
#else
		/// <summary>
		/// Class MVVMPage.
		/// </summary>
		public class MVVMPage : Page, IView
#endif
		{



			/// <summary>
			/// Initializes a new instance of the <see cref="MVVMPage" /> class.
			/// </summary>
			public MVVMPage()
				//: this(null)
			{
				Loaded += ViewHelper.ViewLoadCallBack;
				Unloaded += ViewHelper.ViewUnloadCallBack;
			}



#if WPF
			/// <summary>
			/// Frame of this view
			/// </summary>

			public Frame Frame
			{
				get { return (Frame)GetValue(FrameProperty); }
				set { SetValue(FrameProperty, value); }
			}


			// Using a DependencyProperty as the backing store for Frame.  This enables animation, styling, binding, etc...
			/// <summary>
			/// Frame Property
			/// </summary>
			public static readonly DependencyProperty FrameProperty =
				DependencyProperty.Register("Frame", typeof(Frame), typeof(MVVMPage), new PropertyMetadata(null));


			DependencyObject IView.Parent
			{
				get
				{
					return Frame;

				}
			}
#endif

//			/// <summary>
//			/// Initializes a new instance of the <see cref="MVVMPage" /> class.
//			/// </summary>
//			/// <param name="viewModel">The view model.</param>
//			public MVVMPage(IViewModel viewModel)
//			{
//				ViewModel = viewModel;
//				Unloaded += ViewHelper.ViewUnloadCallBack;
//#if WPF
//				Loaded += async (o, e) =>
//					{
//						if (viewModel != null)
//						{
//							if (!object.ReferenceEquals(ViewModel, viewModel))
//							{
//								ViewModel = viewModel;
//							}
//						}

//						if (ViewModel != null)
//						{
//							await ViewModel.OnBindedViewLoad(this);
//						}

//					};
//#endif

//			}




#if !WPF
			//WPF Pages' Content are objects but others are FE .
			/// <summary>
			/// Gets or sets the content object.
			/// </summary>
			/// <value>The content object.</value>
			public object ContentObject
			{
				get { return Content; }
				set { Content = value as FrameworkElement; }

			}

			/// <summary>
			/// The is loaded
			/// </summary>
			bool IsLoaded = false;

			//WPF navigates page instances but other navgates with parameters
			/// <summary>
			/// Handles the <see cref="E:NavigatedTo" /> event.
			/// </summary>
			/// <param name="e">The <see cref="NavigationEventArgs" /> instance containing the event data.</param>
			protected override void OnNavigatedTo(NavigationEventArgs e)
			{

				base.OnNavigatedTo(e);
				RoutedEventHandler loadEvent = null;

				loadEvent =  (_1, _2) =>
				{


					EventRouting.EventRouter.Instance.RaiseEvent(this, e);

		

					IsLoaded = true;
					this.Loaded -= loadEvent;





				};
				this.Loaded += loadEvent;

			}




			/// <summary>
			/// Handles the <see cref="E:NavigatedFrom" /> event.
			/// </summary>
			/// <param name="e">The <see cref="NavigationEventArgs" /> instance containing the event data.</param>
			protected override void OnNavigatedFrom(NavigationEventArgs e)
			{
				base.OnNavigatedFrom(e);

#if SILVERLIGHT_5
				if (ViewModel.StageManager.DefaultStage.NavigateRequestContexts.ContainsKey(e.Uri.ToString()))
#else
				if (e.NavigationMode == NavigationMode.Back)
#endif

				{

					if (ViewModel != null)
					{

						ViewModel.Dispose();
					}


				}

			}
#else
			/// <summary>
			/// the first object of view content.
			/// </summary>
			public object ContentObject
			{
				get
				{
					return Content;
				}
				set
				{
					Content = value;
				}
			}
#endif



			//public IViewModel DesigningViewModel
			//{
			//	get { return (IViewModel)GetValue(DesigningViewModelProperty); }
			//	set { SetValue(DesigningViewModelProperty, value); }
			//}

			//// Using a DependencyProperty as the backing store for DesigningViewModel.  This enables animation, styling, binding, etc...
			//public static readonly DependencyProperty DesigningViewModelProperty =
			//	DependencyProperty.Register("DesigningViewModel", typeof(IViewModel), typeof(MVVMPage), new PropertyMetadata(null, ViewHelper.DesigningViewModelChangedCallBack));


			/// <summary>
			/// Gets or sets the view model.
			/// </summary>
			/// <value>The view model.</value>
			public IViewModel ViewModel
			{
				get
				{
					var rval = GetValue(ViewModelProperty) as IViewModel;
					var c = this.GetContentAndCreateIfNull();
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
					var c = this.GetContentAndCreateIfNull();
					if (!Object.ReferenceEquals(c.DataContext, value))
					{
						c.DataContext = value;
					}

				}
			}

			// Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
			/// <summary>
			/// The view model property
			/// </summary>
			public static readonly DependencyProperty ViewModelProperty =
				DependencyProperty.Register("ViewModel", typeof(IViewModel), typeof(MVVMPage), new PropertyMetadata(null,
					(o, e) =>
					{
						var p = o as MVVMPage;
#if !WPF
						if (p.IsLoaded)
						{
							ViewHelper.ViewModelChangedCallback(o, e);
						}
#else
						ViewHelper.ViewModelChangedCallback(o, e);
#endif
					}

					));


#if NETFX_CORE

			/// <summary>
			/// Populates the page with content passed during navigation.  Any saved state is also
			/// provided when recreating a page from a prior session.
			/// </summary>
			/// <param name="navigationParameter">The parameter value passed to
			/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
			/// </param>
			/// <param name="pageState">A dictionary of state preserved by this page during an earlier
			/// session.  This will be null the first time a page is visited.</param>
			protected virtual void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
			{
				if (ViewModel != null)
				{
					ViewModel.LoadState(navigationParameter, pageState);
				}
			}

			/// <summary>
			/// Preserves state associated with this page in case the application is suspended or the
			/// page is discarded from the navigation cache.
			/// </summary>
			/// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
			protected virtual void SaveState(Dictionary<String, Object> pageState)
			{
				if (ViewModel != null)
				{
					ViewModel.SaveState(pageState);

				}
			}
#endif


			/// <summary>
			/// Gets the type of the view.
			/// </summary>
			/// <value>The type of the view.</value>
			public ViewType ViewType
			{
				get { return ViewType.Page; }
			}




		}



		/// <summary>
		/// Class MVVMControl.
		/// </summary>
		public class MVVMControl : UserControl, IView
		{

			/// <summary>
			/// Initializes a new instance of the <see cref="MVVMControl" /> class.
			/// </summary>
			public MVVMControl()
				
			{
				Loaded += ViewHelper.ViewLoadCallBack;
				Unloaded += ViewHelper.ViewUnloadCallBack;
			
			}


			///// <summary>
			///// Initializes a new instance of the <see cref="MVVMControl" /> class.
			///// </summary>
			///// <param name="viewModel">The view model.</param>
			//public MVVMControl(IViewModel viewModel)
			//{
			//	ViewModel = viewModel;
			//	Unloaded += ViewHelper.ViewUnloadCallBack;
			//	////////// Unloaded += (_1, _2) => ViewModel = null;
			//	Loaded += async (_1, _2) =>
			//	{

			//		if (viewModel != null)
			//		{
			//			//this.Resources[ViewHelper.DEFAULT_VM_NAME] = viewModel;
			//			if (!object.ReferenceEquals(ViewModel, viewModel))
			//			{
			//				ViewModel = viewModel;

			//			}
			//		}
			//		//else
			//		//{
			//		//    var solveV = this.GetDefaultViewModel();
			//		//    if (solveV != null)
			//		//    {
			//		//        ViewModel = solveV;
			//		//    }

			//		//}
			//		//ViewModel = ViewModel ?? new DefaultViewModel();

			//		if (ViewModel != null)
			//		{
			//			await ViewModel.OnBindedViewLoad(this);
			//		}
			//	};
			//}
#if !WPF
			/// <summary>
			/// Gets or sets the content object.
			/// </summary>
			/// <value>The content object.</value>
			public object ContentObject
			{
				get { return Content; }
				set { Content = value as FrameworkElement; }

			}
#else
			/// <summary>
			/// the first object of view content.
			/// </summary>
			public object ContentObject
			{
				get
				{
					return Content;
				}
				set
				{
					Content = value;
				}
			}
#endif
#if NETFX_CORE

			/// <summary>
			/// Populates the page with content passed during navigation.  Any saved state is also
			/// provided when recreating a page from a prior session.
			/// </summary>
			/// <param name="navigationParameter">The parameter value passed to
			/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
			/// </param>
			/// <param name="pageState">A dictionary of state preserved by this page during an earlier
			/// session.  This will be null the first time a page is visited.</param>
			protected virtual void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
			{
				if (ViewModel != null)
				{
					ViewModel.LoadState(navigationParameter, pageState);
				}
			}

			/// <summary>
			/// Preserves state associated with this page in case the application is suspended or the
			/// page is discarded from the navigation cache. 
			/// </summary>
			/// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
			protected virtual void SaveState(Dictionary<String, Object> pageState)
			{
				if (ViewModel != null)
				{
					ViewModel.SaveState(pageState);
				}
			}
#endif

			//public IViewModel DesigningViewModel
			//{
			//	get { return (IViewModel)GetValue(DesigningViewModelProperty); }
			//	set { SetValue(DesigningViewModelProperty, value); }
			//}

			//// Using a DependencyProperty as the backing store for DesigningViewModel.  This enables animation, styling, binding, etc...
			//public static readonly DependencyProperty DesigningViewModelProperty =
			//	DependencyProperty.Register("DesigningViewModel", typeof(IViewModel), typeof(MVVMControl), new PropertyMetadata(null, ViewHelper.DesigningViewModelChangedCallBack));
			/// <summary>
			/// Gets or sets the view model.
			/// </summary>
			/// <value>The view model.</value>
			public IViewModel ViewModel
			{
				get
				{
					var vm = GetValue(ViewModelProperty) as IViewModel;
					var content = this.GetContentAndCreateIfNull();
					if (vm == null)
					{

						vm = content.DataContext as IViewModel;
						SetValue(ViewModelProperty, vm);

					}
					else
					{
						IView view = this;


						if (!Object.ReferenceEquals(content.DataContext, vm))
						{

							content.DataContext = vm;
						}
					}
					return vm;
				}
				set
				{
					SetValue(ViewModelProperty, value);
					var c = this.GetContentAndCreateIfNull();
					if (!Object.ReferenceEquals(c.DataContext, value))
					{
						c.DataContext = value;
					}

				}
			}

			// Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
			/// <summary>
			/// The view model property
			/// </summary>
			public static readonly DependencyProperty ViewModelProperty =
				DependencyProperty.Register("ViewModel", typeof(IViewModel), typeof(MVVMControl), new PropertyMetadata(null, ViewHelper.ViewModelChangedCallback));


			/// <summary>
			/// Gets the type of the view.
			/// </summary>
			/// <value>The type of the view.</value>
			public ViewType ViewType
			{
				get { return ViewType.Control; }
			}

		}
		/// <summary>
		/// Enum ViewType
		/// </summary>
		public enum ViewType
		{
			/// <summary>
			/// The page
			/// </summary>
			/// <summary>
			/// The page
			/// </summary>
			Page,
			/// <summary>
			/// The window
			/// </summary>
			/// <summary>
			/// The window
			/// </summary>
			Window,
			/// <summary>
			/// The control
			/// </summary>
			/// <summary>
			/// The control
			/// </summary>
			Control
		}

		/// <summary>
		/// Interface IView
		/// </summary>
		public interface IView
		{
			/// <summary>
			/// Gets or sets the view model.
			/// </summary>
			/// <value>The view model.</value>
			IViewModel ViewModel { get; set; }

			/// <summary>
			/// Gets the type of the view.
			/// </summary>
			/// <value>The type of the view.</value>
			ViewType ViewType { get; }

			/// <summary>
			/// Gets or sets the content object.
			/// </summary>
			/// <value>The content object.</value>
			Object ContentObject { get; set; }

			/// <summary>
			/// Gets the parent.
			/// </summary>
			/// <value>The parent.</value>
			DependencyObject Parent { get; }


		}


		/// <summary>
		/// Interface IView
		/// </summary>
		/// <typeparam name="TViewModel">The type of the t view model.</typeparam>
		public interface IView<TViewModel> : IView, IDisposable where TViewModel : IViewModel
		{
			/// <summary>
			/// Gets or sets the specific typed view model.
			/// </summary>
			/// <value>The specific typed view model.</value>
			TViewModel SpecificTypedViewModel { get; set; }
		}

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
			public ViewModelToViewMapper<TModel> MapToDefault<TView>(TView instance) where TView : class, IView
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
			public ViewModelToViewMapper<TModel> MapTo<TView>(string viewMappingKey, TView instance) where TView : class, IView
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
			public ViewModelToViewMapper<TModel> MapToDefault<TView>(bool alwaysNew = true) where TView : class, IView
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
			public ViewModelToViewMapper<TModel> MapTo<TView>(string viewMappingKey, bool alwaysNew = true) where TView : class, IView
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
			public ViewModelToViewMapper<TModel> MapToDefault<TView>(Func<TModel, TView> factory, bool alwaysNew = true) where TView : class, IView
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
			public ViewModelToViewMapper<TModel> MapTo<TView>(string viewMappingKey, Func<TModel, TView> factory, bool alwaysNew = true) where TView : class, IView
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
			public ViewModelToViewMapper<TModel> MapToDefault<TPage>() where TPage : MVVMPage
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
			public ViewModelToViewMapper<TModel> MapToDefault<TPage>(string viewMappingKey) where TPage : MVVMPage
			{

				MapViewToViewModel<TPage>();
				ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewMappingKey, typeof(TPage));
				return this;
			}



#endif


		}

		/// <summary>
		/// 		 class ViewModelToViewMapperHelper
		/// </summary>
		public static class ViewModelToViewMapperHelper
		{

			internal static Dictionary<Type, Func<IViewModel>> ViewToVMMapping = new Dictionary<Type, Func<IViewModel>>();

			/// <summary>
			/// Gets the default view model.
			/// </summary>
			/// <param name="view">The view.</param>
			/// <returns></returns>
			public static IViewModel GetDefaultViewModel(this IView view)
			{
				Func<IViewModel> func;
				var vt = view.GetType();
				if (ViewModelToViewMapperHelper.ViewToVMMapping.TryGetValue(vt, out func))
				{

					return func();
				}
				return null;
			}

			/// <summary>
			/// Gets the view mapper.
			/// </summary>
			/// <typeparam name="TViewModel">The type of the view model.</typeparam>
			/// <param name="vmRegisterEntry">The vm register entry.</param>
			/// <returns></returns>
			public static ViewModelToViewMapper<TViewModel> GetViewMapper<TViewModel>(this MVVMSidekick.Services.ServiceLocatorEntryStruct<TViewModel> vmRegisterEntry)
				  where TViewModel : IViewModel
			{
				return new ViewModelToViewMapper<TViewModel>();
			}


		}

		public interface IViewModelToViewMapperServiceLocator : ITypeSpecifiedServiceLocator<object>
		{

		}
		/// <summary>
		/// View model to view service locator
		/// </summary>
		/// <typeparam name="TViewModel">The type of the view model.</typeparam>
		public class ViewModelToViewMapperServiceLocator<TViewModel> :
			MVVMSidekick.Services.TypeSpecifiedServiceLocatorBase<ViewModelToViewMapperServiceLocator<TViewModel>, object>,
			IViewModelToViewMapperServiceLocator
		{
			/// <summary>
			/// Constuctor
			/// </summary>
			static ViewModelToViewMapperServiceLocator()
			{
				//Instance = new ViewModelToViewMapperServiceLocator<TViewModel>();

			}

			static Lazy<IViewModelToViewMapperServiceLocator> _Instance = new Lazy<IViewModelToViewMapperServiceLocator>
				(() => new ViewModelToViewMapperServiceLocator<TViewModel>(), true);

			/// <summary>
			/// Instance
			/// </summary>

			public static IViewModelToViewMapperServiceLocator Instance
			{
				get
				{
					return _Instance.Value;
				}
				set
				{
					_Instance = new Lazy<IViewModelToViewMapperServiceLocator>(() => value);
				}
			}


		}
		/// <summary>
		/// Locator of view model class
		/// </summary>
		/// <typeparam name="TViewModel">The type of the view model.</typeparam>
		public class ViewModelLocator<TViewModel> : MVVMSidekick.Services.TypeSpecifiedServiceLocatorBase<ViewModelLocator<TViewModel>, TViewModel>
			where TViewModel : IViewModel
		{
			static ViewModelLocator()
			{
				Instance = new ViewModelLocator<TViewModel>();
			}
			/// <summary>
			/// Gets or sets the instance.
			/// </summary>
			/// <value>
			/// The instance.
			/// </value>
			public static ViewModelLocator<TViewModel> Instance { get; set; }

		}




		/// <summary>
		///  A bridge binds two Dependency property
		/// </summary>
		public class PropertyBridge : FrameworkElement
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="PropertyBridge"/> class.
			/// </summary>
			public PropertyBridge()
			{
				base.Width = 0;
				base.Height = 0;
				base.Visibility = Visibility.Collapsed;

			}



			/// <summary>
			/// Gets or sets the source.
			/// </summary>
			/// <value>
			/// The source.
			/// </value>
			public object Source
			{
				private get { return (object)GetValue(SourceProperty); }
				set { SetValue(SourceProperty, value); }
			}

			// Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...			
			/// <summary>
			/// The source property
			/// </summary>
			public static readonly DependencyProperty SourceProperty =
				DependencyProperty.Register("Source", typeof(object), typeof(PropertyBridge), new PropertyMetadata(null,

					(o, a) =>
					{
						var pb = o as PropertyBridge;
						if (pb != null)
						{
							pb.Target = a.NewValue;
						}
					}
					));



			/// <summary>
			/// Gets or sets the target.
			/// </summary>
			/// <value>
			/// The target.
			/// </value>
			public object Target
			{
				get { return (object)GetValue(TargetProperty); }
				set { SetValue(TargetProperty, value); }
			}

			// Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...


			/// <summary>
			/// The target property
			/// </summary>
			public static readonly DependencyProperty TargetProperty =
				DependencyProperty.Register("Target", typeof(object), typeof(PropertyBridge), new PropertyMetadata(null));





		}



	}
}

#if !STANDARDCORE

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

        Object IView.Parent
        {
            get
            {
                return this.Parent;

            }
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


}

#endif
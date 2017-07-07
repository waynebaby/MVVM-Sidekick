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

#if WINDOWS_PHONE_7 || WINDOWS_PHONE_8
		/// <summary>
		/// Class MVVMPage.
		/// </summary>
		public partial class MVVMPage : PhoneApplicationPage, IView
#else
    /// <summary>
    /// Class MVVMPage.
    /// </summary>
    public class MVVMPage : Page, IView, IPageView
#endif
    {



        /// <summary>
        /// Initializes a new instance of the <see cref="MVVMPage" /> class.
        /// </summary>
        public MVVMPage()
        //: this(null)
        {
#if WPF
				Loaded += ViewHelper.ViewLoadCallBack;
				Unloaded += ViewHelper.ViewUnloadCallBack;
#endif
        }



#if WPF
			/// <summary>
			/// Frame of this view
			/// </summary>

			public Object FrameObject
			{
				get { return (Frame)GetValue(FrameProperty); }
				set { SetValue(FrameProperty, value); }
			}


			// Using a DependencyProperty as the backing store for Frame.  This enables animation, styling, binding, etc...
			/// <summary>
			/// Frame Property
			/// </summary>
			public static readonly DependencyProperty FrameProperty =
				DependencyProperty.Register(nameof(FrameObject), typeof(Object), typeof(MVVMPage), new PropertyMetadata(null));


		
#endif
        Object IView.Parent
        {
            get
            {
                return FrameObject;

            }
        }

#if !WPF
        //WPF Pages' Content are objects but others are FE .
        /// <summary>
        /// Gets or sets the content object.
        /// </summary>
        /// <value>The content object.</value>
        public object ViewContentObject
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

            loadEvent = (_1, _2) =>
            {
                EventRouting.EventRouter.Instance.RaiseEvent(this, e);  //VM Is Ready after this
                IsLoaded = true;
                this.Loaded -= loadEvent;

            };



            this.Loaded += loadEvent;

            Loaded += ViewHelper.ViewLoadCallBack;
            Unloaded += ViewHelper.ViewUnloadCallBack;

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
			public object ViewContentObject
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

        void IPageView.OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        void IPageView.OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        }

        void IPageView.OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

        }
#endif


        public object ViewObject => this;
    }
#endif
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVVMSidekick.ViewModels;
using System.Reactive.Linq;
using System.Windows;
using System.IO;
 


#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using MVVMSidekick.Services;


#elif WPF
using System.Windows.Controls;
using System.Windows.Media;

using System.Collections.Concurrent;
using System.Windows.Navigation;
using MVVMSidekick.Services;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;
#elif SILVERLIGHT_5 || SILVERLIGHT_4
						   using System.Windows.Media;
using MVVMSidekick.Services;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using MVVMSidekick.Services;

using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif


namespace MVVMSidekick.Views
{
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

        Object IView.Parent
        {
            get
            {
                return this.Parent;

            }
        }


        public object ViewContentControlObject =>this;
    }




#endif
}

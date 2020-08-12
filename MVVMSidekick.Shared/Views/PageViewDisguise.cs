

#if !BLAZOR

#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;


#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
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
    public class PageViewDisguise : ViewDisguiseBase<Page, PageViewDisguise>, IPageView
    {
        public PageViewDisguise(Page assocatedObject) : base(assocatedObject)
        {
#if WPF
            AssocatedObject.Loaded += ViewHelper.ViewLoadCallBack;
            AssocatedObject.Unloaded += ViewHelper.ViewUnloadCallBack;
#endif

        }



        public override object ViewContentObject
        {
            get { return base.AssocatedObject.Content; }
            set
            {
#if WINDOWS_UWP
                AssocatedObject.Content = value as UIElement;
#elif WPF
                AssocatedObject.Content = value;
#endif
            }
        }

#if WPF

        public  override object Parent
        {
            get
            {
                return FrameObject;
            }
        }
        /// <summary>
        /// Frame of this view
        /// </summary>

        public object FrameObject
        {
            get { return GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }


        // Using a DependencyProperty as the backing store for Frame.  This enables animation, styling, binding, etc...
        /// <summary>
        /// Frame Property
        /// </summary>
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register(nameof(FrameObject), typeof(object), typeof(PageViewDisguise), new PropertyMetadata(null));
#elif WINDOWS_UWP
        public override object Parent
        {
            get
            {

                return this.AssocatedObject.Parent ?? this.AssocatedObject.Frame;

            }
        }
#endif

#if !WPF

        //WPF navigates page instances but other navgates with parameters
        /// <summary>
        /// Handles the <see cref="NavigatedTo" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs" /> instance containing the event data.</param>
        public virtual void OnNavigatedTo(NavigationEventArgs e)
        {
            RoutedEventHandler loadEvent = null;

            loadEvent = (_1, _2) =>
            {
                EventRouting.EventRouter.Instance.RaiseEvent(this, e);  //VM Is Ready after this
                AssocatedObject.Loaded -= loadEvent;

            };



            AssocatedObject.Loaded += loadEvent;
            AssocatedObject.Loaded += ViewHelper.ViewLoadCallBack;
            AssocatedObject.Unloaded += ViewHelper.ViewUnloadCallBack;

        }
        /// <summary>
        /// Handles the <see cref="NavigatedFrom" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs" /> instance containing the event data.</param>
        public virtual void OnNavigatedFrom(NavigationEventArgs e)
        {


            if (e?.NavigationMode == NavigationMode.Back)

            {

                if (ViewModel != null)
                {
                    ViewModel.OnPageNavigatedFrom(e);
                    ViewModel.Dispose();
                }


            }

        }
        public virtual void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.OnPageNavigatingFrom(e);

        }

#endif


    }
}
#endif
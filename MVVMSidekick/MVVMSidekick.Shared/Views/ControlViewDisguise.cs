


#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;


#elif WPF
using System.Windows.Controls;
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
    public class ControlViewDisguise : ViewDisguiseBase<UserControl, ControlViewDisguise>, IControlView
    {
        public ControlViewDisguise(UserControl assocatedObject) : base(assocatedObject)
        {
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


        public override object Parent
        {
            get
            {

                return this.AssocatedObject.Parent;

            }
        }
    }
}

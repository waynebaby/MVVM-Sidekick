

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
    public static class ViewDisguiseManager
    {


        public static IViewDisguise GetViewDisguise(this DependencyObject obj)
        {
            return (IViewDisguise)obj.GetValue(ViewDisguiseProperty);
        }

        public static void SetViewDisguise(this DependencyObject obj, IViewDisguise value)
        {
            obj.SetValue(ViewDisguiseProperty, value);
        }
        

        public static PageViewDisguise GetOrCreateViewDisguise(this Page obj)
        {
            var dis = (PageViewDisguise)obj.GetValue(ViewDisguiseProperty);
            if (dis == null)
            {
                dis = new PageViewDisguise(obj);
                obj.SetValue(ViewDisguiseProperty, dis);
            }
            return dis;
        }

        public static void SetViewDisguise(this Page obj, PageViewDisguise value)
        {
            obj.SetValue(ViewDisguiseProperty, value);
        }
        

        public static ControlViewDisguise GetOrCreateViewDisguise(this UserControl obj)
        {
            var dis = (ControlViewDisguise)obj.GetValue(ViewDisguiseProperty);
            if (dis == null)
            {
                dis = new ControlViewDisguise(obj);
                obj.SetValue(ViewDisguiseProperty, dis);
            }
            return dis;
        }

        public static void SetViewDisguise(this UserControl obj, PageViewDisguise value)
        {
            obj.SetValue(ViewDisguiseProperty, value);
        }

#if WPF

        public static WindowViewDisguise GetOrCreateViewDisguise(this Window obj)
        {
            var dis = (WindowViewDisguise)obj.GetValue(ViewDisguiseProperty);
            if (dis == null)
            {
                dis = new WindowViewDisguise(obj);
                obj.SetValue(ViewDisguiseProperty, dis);
            }
            return dis;
        }

        public static void SetViewDisguise(this Window obj, WindowViewDisguise value)
        {
            obj.SetValue(ViewDisguiseProperty, value);
        }
#endif

        // Using a DependencyProperty as the backing store for ViewDisguise.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewDisguiseProperty =
            DependencyProperty.RegisterAttached(nameof(GetViewDisguise).Remove(0, 3), typeof(IViewDisguise), typeof(ViewDisguiseManager), new PropertyMetadata(null));



    }
}

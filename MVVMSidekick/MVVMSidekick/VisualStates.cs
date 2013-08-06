using MVVMSidekick.Patterns;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using System.Windows;
using System.Windows.Controls;
#endif
namespace MVVMSidekick
{
    namespace VisualStates
    {


        //[DataContract(IsReference=true) ] //if you want
        public class VisualStateProxy : DependencyObject 
        {
            public VisualStateProxy()
            {
                // Use propery to init value here:

                BeaconInstance = new ElementBinder(
                     e =>
                     {

                     },

                     e =>
                     {
                         e.Element = null;
                     
                     });

            }

            //Use propvm + tab +tab  to create a new property of bindable here:




            public static ElementBinder GetBeacon(DependencyObject obj)
            {
                return (ElementBinder)obj.GetValue(BeaconProperty);
            }

            public static void SetBeacon(DependencyObject obj, ElementBinder value)
            {
                obj.SetValue(BeaconProperty, value);
            }

            // Using a DependencyProperty as the backing store for Beacon.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty BeaconProperty =
                DependencyProperty.RegisterAttached("Beacon", typeof(ElementBinder), typeof(VisualStateProxy), new PropertyMetadata(null, ElementBinder.BinderPropertyChangedCallback));





            public ElementBinder BeaconInstance
            {
                get { return (ElementBinder)GetValue(BeaconInstanceProperty); }
                set { SetValue(BeaconInstanceProperty, value); }
            }

            // Using a DependencyProperty as the backing store for BeaconInstance.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty BeaconInstanceProperty =
                DependencyProperty.Register("BeaconInstance", typeof(ElementBinder), typeof(VisualStateProxy), new PropertyMetadata(null));

            




            public bool GotoState(string stateName, bool useTransitions)
            {
                var b = BeaconInstance;
                if (b != null)
                {

                    if (b.Element != null)
                    {
#if WPF
                        return VisualStateManager.GoToElementState(b.Element, stateName, useTransitions);
#else
                        return VisualStateManager.GoToState((Control)b.Element, stateName, useTransitions);

#endif
                    }
                }

                return false;
            }

        }





    }
}

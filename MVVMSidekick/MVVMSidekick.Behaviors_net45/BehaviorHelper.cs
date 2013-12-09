using MVVMSidekick.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;

#else
using System.Windows.Controls;
using System.Windows.Data;

#endif

namespace MVVMSidekick.Behaviors
{
    public static class BehaviorHelper
    {

        internal static void OnBehaviorOnAttached(this BaeconBehavior source, ContentControl target)
        {
            if (target == null)
            {
                return;
            }
            DependencyProperty targetProperty = MVVMSidekick.Views.StageManager.BeaconProperty;
#if NETFX_CORE||SILVERLIGHT
            string path = "BaeconName";
#else
            string path = BaeconBehavior.BaeconNameProperty.Name;
#endif

            var binding = new Binding();
            binding.Source = source;
            binding.Path = new PropertyPath(path);
            binding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(target, targetProperty, binding);
        }
        internal static void OnBehaviorOnOnDetaching(this BaeconBehavior source, ContentControl target)
        {
            if (target == null)
            {
                return;
            }

            DependencyProperty targetProperty = MVVMSidekick.Views.StageManager.BeaconProperty;
#if NETFX_CORE ||SILVERLIGHT
            BindingOperations.SetBinding(target, targetProperty, null);
#else
            BindingOperations.ClearBinding(target, targetProperty);
#endif
        }
    }
}

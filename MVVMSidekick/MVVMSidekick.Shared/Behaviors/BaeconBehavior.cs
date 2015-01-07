#if ! NETFX_CORE
using Microsoft.Expression.Interactivity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

#else 
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
#endif

namespace MVVMSidekick.Behaviors
{
#if NETFX_CORE
    public class BaeconBehavior : BehaviorBase, IBehavior
#else
    public class BaeconBehavior : Behavior<ContentControl>

#endif
    {
        public string BaeconName
        {
            get { return (string)GetValue(BaeconNameProperty); }
            set { SetValue(BaeconNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BaeconName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BaeconNameProperty =
            DependencyProperty.Register("BaeconName", typeof(string), typeof(BaeconBehavior), new PropertyMetadata(""));



#if NETFX_CORE
        public override void Attach(DependencyObject associatedObject)
        {
            base.Attach(associatedObject);
            this.OnBehaviorOnAttached(associatedObject as ContentControl);
        }

        public override void Detach()
        {
            this.OnBehaviorOnOnDetaching(AssociatedObject as ContentControl);
            base.Detach();
        }

#else
        protected override void OnAttached()
        {

            this.OnBehaviorOnAttached(AssociatedObject);

            base.OnAttached();
        }



        protected override void OnDetaching()
        {
            this.OnBehaviorOnOnDetaching(AssociatedObject);
            base.OnDetaching();
        }
#endif


        internal void OnBehaviorOnAttached(ContentControl target)
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
            binding.Source = this;
            binding.Path = new PropertyPath(path);
            binding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(target, targetProperty, binding);
        }
        internal void OnBehaviorOnOnDetaching(ContentControl target)
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

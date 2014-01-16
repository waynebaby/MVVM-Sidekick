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
using MVVMSidekick.Patterns.ItemsAndSelection;
using MVVMSidekick.ViewModels;
#else 
using Microsoft.Xaml.Interactivity;
using MVVMSidekick.Patterns.ItemsAndSelection;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
#endif


namespace MVVMSidekick.Behaviors
{
#if NETFX_CORE
    public class ItemsSelectionBindingBehavior : BehaviorBase, IBehavior
#else
    public class ItemsSelectionBindingBehavior : Behavior<DependencyObject>

#endif
    {



        public IItemsAndSelectionGroupBinding BindingTarget
        {
            get { return (IItemsAndSelectionGroupBinding)GetValue(BindingTargetProperty); }
            set { SetValue(BindingTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BindingTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingTargetProperty =
            DependencyProperty.Register("BindingTarget", typeof(IItemsAndSelectionGroupBinding), typeof(ItemsSelectionBindingBehavior), new PropertyMetadata(null));




        WeakReference _associatedObject = new WeakReference(null);



#if NETFX_CORE
        public override void Attach(DependencyObject associatedObject)
        {
            base.Attach(associatedObject);
            this.OnBehaviorOnAttached(associatedObject );
        }

        public override void Detach()
        {
            this.OnBehaviorOnOnDetaching(AssociatedObject );
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

        private void OnBehaviorOnAttached(DependencyObject AssociatedObject)
        {
            _associatedObject.Target = (AssociatedObject);
            CheckBinding();
        }

        private void OnBehaviorOnOnDetaching(DependencyObject AssociatedObject)
        {
            DependencyObject ao = null;
            ao = _associatedObject.Target as DependencyObject;
            {
                if (ao == null)
                {
                    return;
                }
                DependencyProperty targetProperty = ItemsAndSelectionGroupBinder.BinderProperty;
                ao.SetValue(targetProperty, null);
            }
            _associatedObject.Target = null;


        }

        void CheckBinding()
        {
            DependencyObject ao = null;
            ao = _associatedObject.Target as DependencyObject;
            if (ao == null)
            {
                return;
            }

            DependencyProperty targetProperty = ItemsAndSelectionGroupBinder.BinderProperty;

            string path = "BindingTarget.Binder";

            var binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath(path);
            binding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(ao, targetProperty, binding);

        }


    }
}

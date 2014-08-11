using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MVVMSidekick.Behaviors
{
    public class BehaviorBase : DependencyObject, IBehavior
    {


        public DependencyObject AssociatedObject
        {
            get { return (DependencyObject)GetValue(AssociatedObjectProperty); }
            set { SetValue(AssociatedObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AssociatedObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AssociatedObjectProperty =
            DependencyProperty.Register("AssociatedObject", typeof(DependencyObject), typeof(BehaviorBase), new PropertyMetadata(null));



        public virtual void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
        }

        public virtual void Detach()
        {
            AssociatedObject = null;
        }
    }
}

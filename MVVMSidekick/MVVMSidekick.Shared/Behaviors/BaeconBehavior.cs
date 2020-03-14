#if WPF

using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


#elif WINDOWS_UWP
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
#endif

namespace MVVMSidekick.Behaviors
{

	/// <summary>
	/// Bind a beacon to a Content Control, make it work as a stage.
	/// </summary>
    public class BaeconBehavior : Behavior<ContentControl>
	{
		/// <summary>
		/// Gets or sets the name of the baecon.
		/// </summary>
		/// <value>
		/// The name of the baecon.
		/// </value>
		public string BaeconName
		{
			get { return (string)GetValue(BaeconNameProperty); }
			set { SetValue(BaeconNameProperty, value); }
		}

		// Using a DependencyProperty as the backing store for BaeconName.  This enables animation, styling, binding, etc...
		/// <summary>
		/// The baecon name property
		/// </summary>
		public static readonly DependencyProperty BaeconNameProperty =
			DependencyProperty.Register("BaeconName", typeof(string), typeof(BaeconBehavior), new PropertyMetadata(""));


		/// <summary>
		/// Called when [attached].
		/// </summary>
        protected override void OnAttached()
        {

            this.OnBehaviorOnAttached(AssociatedObject);

            base.OnAttached();
        }


		/// <summary>
		/// Called when [detaching].
		/// </summary>
        protected override void OnDetaching()
        {
            this.OnBehaviorOnOnDetaching(AssociatedObject);
            base.OnDetaching();
        }


		internal void OnBehaviorOnAttached(ContentControl target)
		{
			if (target == null)
			{
				return;
			}
			DependencyProperty targetProperty = MVVMSidekick.Views.StageManager.BeaconProperty;
#if WINDOWS_UWP 
			string path = "BaeconName";
#elif WPF
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
#if WINDOWS_UWP 
			BindingOperations.SetBinding(target, targetProperty, null);
#elif WPF
            BindingOperations.ClearBinding(target, targetProperty);
#endif

		}


	}
}

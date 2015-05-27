#if NETFX_CORE
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace MVVMSidekick.Behaviors
{

	/// <summary>
	/// 	Behavior Base
	/// </summary>
	public class BehaviorBase : DependencyObject, IBehavior
	{


		/// <summary>
		/// Gets or sets the associated object.
		/// </summary>
		/// <value>
		/// The associated object.
		/// </value>
		public DependencyObject AssociatedObject
		{
			get { return (DependencyObject)GetValue(AssociatedObjectProperty); }
			set { SetValue(AssociatedObjectProperty, value); }
		}

		// Using a DependencyProperty as the backing store for AssociatedObject.  This enables animation, styling, binding, etc...
		/// <summary>
		/// The associated object property
		/// </summary>
		public static readonly DependencyProperty AssociatedObjectProperty =
			DependencyProperty.Register("AssociatedObject", typeof(DependencyObject), typeof(BehaviorBase), new PropertyMetadata(null));



		/// <summary>
		/// Attaches the specified associated object.
		/// </summary>
		/// <param name="associatedObject">The associated object.</param>
		public virtual void Attach(DependencyObject associatedObject)
		{
			AssociatedObject = associatedObject;
		}

		/// <summary>
		/// Detaches this instance.
		/// </summary>
		public virtual void Detach()
		{
			AssociatedObject = null;
		}
	}

}
#endif
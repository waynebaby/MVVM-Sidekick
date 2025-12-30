
#if !BLAZOR

namespace MVVMSidekick.Behaviors
{
	/// <summary>
	/// <para>将信标绑定到内容控件，使其作为舞台工作</para>
	/// <para>Bind a beacon to a Content Control, make it work as a stage</para>
	/// </summary>
    public class BaeconBehavior : Behavior<ContentControl>
	{
		/// <summary>
		/// <para>获取或设置信标的名称</para>
		/// <para>Gets or sets the name of the beacon</para>
		/// </summary>
		/// <value>
		/// <para>信标的名称</para>
		/// <para>The name of the beacon</para>
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
#if WINDOWS_UWP || WinUI3
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

#endif
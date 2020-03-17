
#if WPF
using MVVMSidekick.EventRouting;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using Microsoft.Xaml.Behaviors;
#elif WINDOWS_UWP

using System.Reactive.Linq;
using System.Linq;
using System.Threading;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using System;
using MVVMSidekick.EventRouting;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;
#endif
using MVVMSidekick.Common;

namespace MVVMSidekick.Behaviors
{


#if WINDOWS_UWP

	public class TypeNameStringToTypeConverter : IValueConverter
	{
		static TypeNameStringToTypeConverter()
		{
			Instance = new TypeNameStringToTypeConverter();
		}
		public static TypeNameStringToTypeConverter Instance { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var inputString = value as string;
			var t = Type.GetType(inputString, false);
			return t ?? typeof(Object);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			var t = value as Type;
			return (t ?? typeof(object)).AssemblyQualifiedName;
		}
	}
#endif


    /// <summary>
    /// ListenToEventRouterData Behavior
    /// </summary>
    public sealed class ListenToEventRouterDataBehavior :Behavior<FrameworkElement>, IDisposable
	{


		protected override void OnAttached()
		{
			ExchangeTheSubscribedRouter(this, EventRouter);

#if WPF
		 
			Window.GetWindow(AssociatedObject).Closed += ListenToEventRouterDataBehavior_Closed;
#endif

			base.OnAttached();
		}

#if WPF
		private void ListenToEventRouterDataBehavior_Closed(object sender, EventArgs e)
		{
			this.Dispose();
		}
#elif WINDOWS_UWP

#endif

		protected override void OnDetaching()
		{
			if (_oldSubscrption != null)
			{
				_oldSubscrption.Dispose();
				_oldSubscrption = null;
			}
#if WPF
		
			Window.GetWindow(AssociatedObject).Closed -= ListenToEventRouterDataBehavior_Closed;		
#endif
			base.OnDetaching();
		}



        /// <summary>
        /// Last Data Received 
        /// </summary>
		public Object LastDataReceived
		{
			get { return (Object)this.GetValue(LastDataReceivedProperty);}
			set { SetValue(LastDataReceivedProperty, value); }
		}

        /// <summary>
        /// Last Data Received 
        /// </summary>
        public static readonly DependencyProperty LastDataReceivedProperty =
			DependencyProperty.Register("LastDataReceived", typeof(Object), typeof(ListenToEventRouterDataBehavior), new PropertyMetadata(0));


		/// <summary>
		/// Gets or sets the type of the event object.
		/// </summary>
		/// <value>
		/// The type of the event object.
		/// </value>
		public System.Type EventObjectType
		{
			get { return (Type)GetValue(EventObjectTypeProperty); }
			set { SetValue(EventObjectTypeProperty, value); }
		}
        /// <summary>
        /// Gets or sets the type of the event object.
        /// </summary>
        /// <value>
        /// The type of the event object.
        /// </value>
        public static readonly DependencyProperty EventObjectTypeProperty =
			DependencyProperty.Register("EventObjectType", typeof(Type), typeof(ListenToEventRouterDataBehavior), new PropertyMetadata(typeof(object)));


        /// <summary>
        /// Target Event Router
        /// </summary>
		public EventRouter EventRouter
		{
			get { return (EventRouter)GetValue(EventRouterProperty); }
			set { SetValue(EventRouterProperty, value); }
		}

        /// <summary>
        /// Target Event Router
        /// </summary>
        public static readonly DependencyProperty EventRouterProperty =
			DependencyProperty.Register("EventRouter", typeof(EventRouter), typeof(ListenToEventRouterDataBehavior), new PropertyMetadata(null,
				(o, e) =>
				{
					var t = o as ListenToEventRouterDataBehavior;
					ExchangeTheSubscribedRouter(t, e.NewValue as EventRouter);
				}));




		IDisposable _oldSubscrption;

		static void ExchangeTheSubscribedRouter(ListenToEventRouterDataBehavior bhv, EventRouter newRouter)
		{

			var targetEventRouter = newRouter ?? EventRouter.Instance;
			var query = targetEventRouter.GetEventChannel<object>()
				.Where(x => string.IsNullOrEmpty(bhv.EventRoutingName) || bhv.EventRoutingName == x.EventName);


			var old = Interlocked.Exchange(
				ref bhv._oldSubscrption,
				query.Subscribe(e => bhv.LastDataReceived = e.EventData).MakeFinalizableDisposable());

			old?.Dispose();

		}



        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
		{
			_oldSubscrption?.Dispose();
		}




		/// <summary>
		/// Gets or sets the name of the event routing.
		/// </summary>
		/// <value>
		/// The name of the event routing.
		/// </value>
		public string EventRoutingName
		{
			get { return (string)GetValue(EventRoutingNameProperty); }
			set { SetValue(EventRoutingNameProperty, value); }
		}


        /// <summary>
        /// Gets or sets the name of the event routing.
        /// </summary>
        /// <value>
        /// The name of the event routing.
        /// </value>
        public static readonly DependencyProperty EventRoutingNameProperty =
			DependencyProperty.Register(nameof(EventRoutingName), typeof(string), typeof(ListenToEventRouterDataBehavior), new PropertyMetadata(null));



	}

}

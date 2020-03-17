using System;
#if WPF

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Xaml.Behaviors;

#elif WINDOWS_UWP
using Windows.UI.Xaml;
using System.Reflection;
#endif
using MVVMSidekick.EventRouting;
using MVVMSidekick.Utilities;


namespace MVVMSidekick.Behaviors
{

#if WINDOWS_UWP
	public class SendToEventRouterAction : DependencyObject, Microsoft.Xaml.Interactivity.IAction
#elif WPF
    public class SendToEventRouterAction : TriggerAction<DependencyObject>
#endif
    {



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
        DependencyProperty.Register("EventRoutingName", typeof(string), typeof(SendToEventRouterAction), new PropertyMetadata(null));




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
        DependencyProperty.Register("EventRouter", typeof(EventRouter), typeof(SendToEventRouterAction), new PropertyMetadata(null));






        /// <summary>
        /// if Event data type is null, the event will fired as the EventData's GetType result.
        /// </summary>
        public Type EventDataType
        {
            get { return (Type)GetValue(EventDataTypeProperty); }
            set { SetValue(EventDataTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventDataType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventDataTypeProperty =
            DependencyProperty.Register("EventDataType", typeof(Type), typeof(SendToEventRouterAction), new PropertyMetadata(null));



        /// <summary>
        /// Is Event Firing To All Base ClassesChannels
        /// </summary>
		public bool IsEventFiringToAllBaseClassesChannels
        {
            get { return (bool)GetValue(IsEventFiringToAllBaseClassesChannelsProperty); }
            set { SetValue(IsEventFiringToAllBaseClassesChannelsProperty, value); }
        }


        /// <summary>
        /// Is Event Firing To All Base ClassesChannels
        /// </summary>
        public static readonly DependencyProperty IsEventFiringToAllBaseClassesChannelsProperty =
        DependencyProperty.Register("IsEventFiringToAllBaseClassesChannels", typeof(bool), typeof(SendToEventRouterAction), new PropertyMetadata(false));


        /// <summary>
        /// Is EventFiring To All Implemented Interfaces Channels
        /// </summary>
        public bool IsEventFiringToAllImplementedInterfacesChannels
        {
            get { return (bool)GetValue(IsEventFiringToAllImplementedInterfacesChannelsProperty); }
            set { SetValue(IsEventFiringToAllImplementedInterfacesChannelsProperty, value); }
        }

        public static readonly DependencyProperty IsEventFiringToAllImplementedInterfacesChannelsProperty =
            DependencyProperty.Register("IsEventFiringToAllImplementedInterfacesChannels", typeof(bool), typeof(SendToEventRouterAction), new PropertyMetadata(false));


        /// <summary>
        /// 
        /// </summary>
		public Object EventData
        {
            get { return (Object)GetValue(EventDataProperty); }
            set { SetValue(EventDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventDataProperty =
            DependencyProperty.Register("EventData", typeof(Object), typeof(SendToEventRouterAction), new PropertyMetadata(null));





#if WINDOWS_UWP
		public object Execute(object sender, object parameter)
		{
			var et = EventDataType;

			if (EventData != null)
			{
				if (et == null)
				{
					et = EventData.GetType();
				}
				else if (!et.GetType().GetTypeInfo().IsAssignableFrom(EventData.GetType().GetTypeInfo()))
				{
					return null;
				}
			}

			var targetEventRouter = EventRouter ?? EventRouter.Instance;

			targetEventRouter.GetEventChannel(et).RaiseEvent(sender, this.EventRoutingName, EventData, IsEventFiringToAllBaseClassesChannels, IsEventFiringToAllImplementedInterfacesChannels);
			return null;
		}
#elif WPF

        /// <summary>
        /// Invokes the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected override void Invoke(object parameter)
        {
            var et = EventDataType;

            if (EventData != null)
            {


                if (et == null)
                {
                    et = typeof(object);
                }

                if (!et.GetType().IsAssignableFrom(EventDataType.GetType()))
                {
                    return;
                }
            }

            var targetEventRouter = EventRouter ?? EventRouter.Instance;

            targetEventRouter.GetEventChannel(et).RaiseEvent(this.AssociatedObject, this.EventRoutingName, EventData, IsEventFiringToAllBaseClassesChannels, IsEventFiringToAllImplementedInterfacesChannels);

            //targetEventRouter.RaiseEvent(this, EventData, EventDataType, EventRoutingName);

        }
#endif
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSidekick_MAUI.Behaviors
{
    public class ButtonClickedEventMessageBehavior : Behavior<Button>
    {
        public ButtonClickedEventMessageBehavior()
        {

        }

        public static readonly BindableProperty EventMessageProperty =
            BindableProperty.Create(
                nameof(EventMessage),
                typeof(string),
                typeof(ButtonClickedEventMessageBehavior));

        public string EventMessage
        {
            get => (string)GetValue(EventMessageProperty);
            set => SetValue(EventMessageProperty, value);
        }

 
        public static readonly BindableProperty EventRouterProperty =
        BindableProperty.Create(
            nameof(EventRouter),
            typeof(MVVMSidekick.EventRouting.EventRouter),
            typeof(ButtonClickedEventMessageBehavior), MVVMSidekick.EventRouting.EventRouter.Instance);
        public MVVMSidekick.EventRouting.EventRouter EventRouter
        {
            get => (MVVMSidekick.EventRouting.EventRouter)GetValue(EventRouterProperty);
            set => SetValue(EventRouterProperty, value);
        }

        public Button? AttachedObject
        {
            get; private set;
        }
        protected override void OnAttachedTo(Button bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Clicked += Bindable_Clicked;
            AttachedObject = bindable;
        }
        protected override void OnDetachingFrom(Button bindable)
        {

            bindable.Clicked -= Bindable_Clicked;
            AttachedObject = null;
            base.OnDetachingFrom(bindable);
        }

        private void Bindable_Clicked(object? sender, EventArgs e)
        {
            (EventRouter?? MVVMSidekick.EventRouting.EventRouter.Instance).RaiseEvent(sender, e, typeof(EventArgs), EventMessage, true, false);
        }
    }
}

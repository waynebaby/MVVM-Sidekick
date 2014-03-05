using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Reactive.Linq;
using System.Windows.Data;
using MVVMSidekick.Utilities;
using MVVMSidekick.Commands.EventBinding;
using MVVMSidekick.Commands;
namespace MVVMSidekick.Behaviors
{

    public enum BindToApplicationBarTarget
    {
        IconButton,
        MenuItem

    }
    public class BindToAppliactionBarBehavior : Behavior<Button>
    {

        List<IApplicationBarIconButton> Buttons;
        List<IApplicationBarMenuItem> MenuItems;


        IDisposable eventSubscribe;
        public BindToAppliactionBarBehavior()
        {

        }


        private bool IsBindingActive = false;

        private static bool GetHasEventsWired(PhoneApplicationPage obj)
        {
            return (bool)obj.GetValue(HasEventsWiredProperty);
        }

        private static void SetHasEventsWired(PhoneApplicationPage obj, bool value)
        {
            obj.SetValue(HasEventsWiredProperty, value);
        }

        // Using a DependencyProperty as the backing store for HasEventsWired.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty HasEventsWiredProperty =
            DependencyProperty.RegisterAttached("HasEventsWired", typeof(bool), typeof(PhoneApplicationPage), new PropertyMetadata(false));



        public PhoneApplicationPage Page
        {
            get { return (PhoneApplicationPage)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Page.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register("Page", typeof(PhoneApplicationPage), typeof(BindToAppliactionBarBehavior), new PropertyMetadata(null, (o, v) => RefreshApplicationBar(o as BindToAppliactionBarBehavior)));

        public int IndexBindTo
        {
            get { return (int)GetValue(IndexBindToProperty); }
            set { SetValue(IndexBindToProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IndexBindTo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexBindToProperty =
            DependencyProperty.Register("IndexBindTo", typeof(int), typeof(BindToAppliactionBarBehavior), new PropertyMetadata(0));


        public BindToApplicationBarTarget TargetType
        {
            get { return (BindToApplicationBarTarget)GetValue(TargetTypeProperty); }
            set { SetValue(TargetTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetTypeProperty =
            DependencyProperty.Register("TargetType", typeof(BindToApplicationBarTarget), typeof(BindToAppliactionBarBehavior), new PropertyMetadata(BindToApplicationBarTarget.IconButton, (o, v) => RefreshApplicationBar(o as BindToAppliactionBarBehavior)));



        private bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register("IsEnabled", typeof(bool), typeof(BindToAppliactionBarBehavior), new PropertyMetadata(true, (o, v) => RefreshApplicationBar(o as BindToAppliactionBarBehavior)));




        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(BindToAppliactionBarBehavior), new PropertyMetadata(null, (o, v) => RefreshApplicationBar(o as BindToAppliactionBarBehavior)));


        public Uri IconUri
        {
            get { return (Uri)GetValue(IconUriProperty); }
            set { SetValue(IconUriProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconUriProperty =
            DependencyProperty.Register("IconUri", typeof(Uri), typeof(BindToAppliactionBarBehavior), new PropertyMetadata(null, (o, v) => RefreshApplicationBar(o as BindToAppliactionBarBehavior)));



        static readonly string EventFilterName = string.Intern("ApplicationBarItemClicked" + Guid.NewGuid().ToString());
        void WireEventToItem(IApplicationBarMenuItem item)
        {
            //if (commandSubscription != null)
            //{
            //    commandSubscription.Dispose();
            //    commandSubscription = null;
            //}

            //commandSubscription =
            Observable.FromEventPattern(
                eh => item.Click += eh,
                eh => item.Click -= eh)
            .Subscribe(
                ev =>
                {
                    var agg = EventCommandEventArgs.Create(AssociatedObject.CommandParameter, null, ev.Sender, ev.EventArgs, "Click", typeof(EventHandler));

                    EventRouting.EventRouter.Instance
                        .RaiseEvent(item, agg, EventFilterName);
                }
            );

        }

        //IDisposable commandSubscription;



        protected override void OnAttached()
        {
            base.OnAttached();
            // TryLocatePageFromButton();


            var binding = new Binding();
            binding.Source = AssociatedObject;
            binding.Path = new PropertyPath("IsEnabled");
            BindingOperations.SetBinding(this, IsEnabledProperty, binding);


            AssociatedObject.Loaded += (s, e) =>
            {

                this.IsBindingActive = true;



                TryLocatePageFromButton();
                var appb = GetApplicationBar(Page);
                if (appb.Buttons != null)
                {
                    Buttons = appb.Buttons.OfType<IApplicationBarIconButton>().ToList();
                }

                if (appb.MenuItems != null)
                {
                    MenuItems = appb.MenuItems.OfType<IApplicationBarMenuItem>().ToList();

                }

                RefreshApplicationBar(this);

                eventSubscribe = EventRouting.EventRouter.Instance
                       .GetEventObject<EventCommandEventArgs>()
                       .ObserveOn(System.Reactive.Concurrency.DispatcherScheduler.Current)
                       .Where(_ =>
                           IsBindingActive)
                       .Where(x =>
                           x.EventName == EventFilterName)
                       .Where(
                            x =>
                                (x.Sender is IApplicationBarIconButton && this.TargetType == BindToApplicationBarTarget.IconButton) || (!(x.Sender is IApplicationBarIconButton) && this.TargetType == BindToApplicationBarTarget.MenuItem))
                       .Where(ev =>
                       {

                           if (ev.EventArgs != null)
                           {

                               switch (TargetType)
                               {
                                   case BindToApplicationBarTarget.IconButton:
                                       return Page.ApplicationBar.Buttons.IndexOf(ev.Sender) == IndexBindTo;

                                   case BindToApplicationBarTarget.MenuItem:
                                       return Page.ApplicationBar.MenuItems.IndexOf(ev.Sender) == IndexBindTo;

                                   default:
                                       return false;
                               }

                           }
                           return false;
                       })

                       .Subscribe(
                           ev =>
                           {
                               var args = ev.EventArgs;
                               var cb = new CommandBinding()
                                {
                                    EventName = args.EventName,
                                    EventSource = Page,
                                    Parameter = args.Parameter,
                                    Command = AssociatedObject.Command,
                                };

                               cb.ExecuteFromEvent(args.ViewSender, args.EventArgs, args.EventName, args.EventHandlerType);

                               //if (AssociatedObject.Command != null)
                               //{
                               //    if (AssociatedObject.Command.CanExecute(AssociatedObject.CommandParameter))
                               //    {
                               //        AssociatedObject.Command.Execute(AssociatedObject.CommandParameter);
                               //    }
                               //}

                           });


                if (!GetHasEventsWired(Page))
                {
                    PageSetup();
                    SetHasEventsWired(Page, true);
                }


                return;


            };



        }

        private void PageSetup()
        {
            var appb = GetApplicationBar(Page);
            if (appb == null)
            {
                return;
            }


            if (Buttons != null)
            {

                var i = 0;
                foreach (IApplicationBarIconButton btn in appb.Buttons)
                {
                    WireEventToItem(btn);

                    i++;
                }
            }


            if (MenuItems != null)
            {

                var i = 0;
                foreach (IApplicationBarMenuItem itm in appb.MenuItems)
                {
                    WireEventToItem(itm);

                    i++;
                }
            }



        }

        private void TryLocatePageFromButton()
        {
            if (Page == null)
            {
                var p = AssociatedObject.Parent as FrameworkElement;
                if (p != null && !(p is PhoneApplicationPage))
                {
                    p = p.Parent as FrameworkElement;
                }

                if (p != null)
                {
                    Page = p as PhoneApplicationPage;
                }

            }
        }


        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (eventSubscribe != null)
            {
                eventSubscribe.Dispose();
                eventSubscribe = null;
            }
        }


        private static void RefreshApplicationBar(BindToAppliactionBarBehavior behavior)
        {
            switch (behavior.TargetType)
            {
                case BindToApplicationBarTarget.IconButton:
                    {
                        var lst = behavior.Buttons;
                        if (lst != null)
                        {
                            if (behavior.IndexBindTo < lst.Count && behavior.IndexBindTo >= 0)
                            {
                                var item = lst[behavior.IndexBindTo];
                                if (item.IconUri != behavior.IconUri)
                                {
                                    if (behavior.IconUri != null)
                                    {
                                        item.IconUri = behavior.IconUri;
                                    }

                                };

                                if (item.IsEnabled != behavior.IsEnabled)
                                {
                                    item.IsEnabled = behavior.IsEnabled;
                                };


                                if (item.Text != behavior.Text && behavior.Text != null)
                                {
                                    item.Text = behavior.Text;
                                };

                            }
                        }
                    }


                    break;
                case BindToApplicationBarTarget.MenuItem:
                    {
                        var lst = behavior.MenuItems;
                        if (lst != null)
                        {
                            if (behavior.IndexBindTo < lst.Count && behavior.IndexBindTo >= 0)
                            {
                                var item = lst[behavior.IndexBindTo];


                                if (item.IsEnabled != behavior.AssociatedObject.IsEnabled)
                                {
                                    item.IsEnabled = behavior.AssociatedObject.IsEnabled;
                                };


                                if (item.Text != behavior.Text)
                                {
                                    item.Text = behavior.Text;
                                };

                            }
                        }
                    }

                    break;
                default:
                    break;
            }
        }
        private static IApplicationBar GetApplicationBar(PhoneApplicationPage page)
        {
            IApplicationBar appb = null;
            try
            {
                appb = page.ApplicationBar;
                //if (appb == null)
                //{
                //    appb = page.ApplicationBar = new ApplicationBar();
                //}

            }
            catch (NullReferenceException ex)
            {

                throw new InvalidOperationException(
@"The Page cannot automatically found by current context. 
Please create a binding to Page property.", ex);
            }
            return appb;
        }





    }
}

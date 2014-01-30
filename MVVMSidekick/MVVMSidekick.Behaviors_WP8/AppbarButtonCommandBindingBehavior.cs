
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;


namespace MVVMSidekick.Behaviors
{

    public class ApplicationBarCommandBindingBehavior : Behavior<PhoneApplicationPage>, IApplicationBar
    {
        public ApplicationBarCommandBindingBehavior()
        {

        }

        protected override void OnAttached()
        {

            if (AssociatedObject.ApplicationBar == null)
            {
                AssociatedObject.ApplicationBar = new ApplicationBar();
            }

            base.OnAttached();
        }


        private static void PopulateAppBarIconButtons(ApplicationBarCommandBindingBehavior sender)
        {
            var page = sender.Page;
            if (page == null) return;

            var ApplicationBar = page.ApplicationBar;
            if (ApplicationBar == null) return;

            var collection = sender.Buttons;
            if (collection == null) return;


            for (int i = 0; i < ApplicationBar.Buttons.Count; i++)
            {
                ApplicationBar.Buttons.RemoveAt(ApplicationBar.Buttons.Count - i - 1);
            }

            foreach (var item in collection)
            {
                var newapbtn = new ApplicationBarIconButton(item.IconUri) { IsEnabled = item.IsEnabled, Text = item.Text };
                newapbtn.Click += (o, e) =>
                    {
                        if (item.BindingCommand !=null)
                        {
                            if (item.BindingCommand.CanExecute(null))
                            {
                                item.BindingCommand.Execute(null);
                            }
                        }

                    };
                item.Core = newapbtn;
                ApplicationBar.Buttons.Add(newapbtn);

                item.PageContext = page;
            }
        }









        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(ApplicationBarCommandBindingBehavior), new PropertyMetadata(Colors.Transparent,
                  (o, e) =>
                  {
                      PropertyChangedHelper.IfValueChangedThen<ApplicationBarCommandBindingBehavior, Color>(o, e,
                        (sender, old1, new1) =>
                        {
                            if (sender.Core != null)
                            {
                                if (!PropertyChangedHelper.ValueEquals(new1, sender.Core.BackgroundColor))
                                {
                                    sender.Core.BackgroundColor = new1;
                                }
                            }

                        }
                      );
                  }

                ));


        System.Collections.IList IApplicationBar.Buttons
        {
            get { return Buttons; }
        }

        public double DefaultSize
        {
            get { return Core.DefaultSize; }
        }



        public Color ForegroundColor
        {
            get { return (Color)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ForegroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor", typeof(Color), typeof(ApplicationBarCommandBindingBehavior), new PropertyMetadata(Colors.Transparent,
                (o, e) =>
                {
                    PropertyChangedHelper.IfValueChangedThen<ApplicationBarCommandBindingBehavior, Color>(o, e,
                      (sender, old1, new1) =>
                      {
                          if (sender.Core != null)
                          {
                              if (!PropertyChangedHelper.ValueEquals(new1, sender.Core.ForegroundColor))
                              {
                                  sender.Core.ForegroundColor = new1;
                              }
                          }

                      }
                    );
                }));




        public bool IsMenuEnabled
        {
            get { return (bool)GetValue(IsMenuEnabledProperty); }
            set { SetValue(IsMenuEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMenuEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMenuEnabledProperty =
            DependencyProperty.Register("IsMenuEnabled", typeof(bool), typeof(ApplicationBarCommandBindingBehavior), new PropertyMetadata(true,
                 (o, e) =>
                 {
                     PropertyChangedHelper.IfValueChangedThen<ApplicationBarCommandBindingBehavior, bool>(o, e,
                       (sender, old1, new1) =>
                       {
                           if (sender.Core != null)
                           {
                               if (!PropertyChangedHelper.ValueEquals(new1, sender.Core.IsMenuEnabled))
                               {
                                   sender.Core.IsMenuEnabled = new1;
                               }
                           }

                       }
                     );
                 }));




        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(ApplicationBarCommandBindingBehavior), new PropertyMetadata(true,
                    (o, e) =>
                    {
                        PropertyChangedHelper.IfValueChangedThen<ApplicationBarCommandBindingBehavior, bool>(o, e,
                          (sender, old1, new1) =>
                          {
                              if (sender.Core != null)
                              {
                                  if (!PropertyChangedHelper.ValueEquals(new1, sender.Core.IsVisible))
                                  {
                                      sender.Core.IsVisible = new1;
                                  }
                              }

                          }
                        );
                    }));



        System.Collections.IList IApplicationBar.MenuItems
        {
            get { throw new NotImplementedException(); }
        }

        public double MiniSize
        {
            get { return Core.MiniSize; }
        }



        public ApplicationBarMode Mode
        {
            get { return (ApplicationBarMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(ApplicationBarMode), typeof(ApplicationBarCommandBindingBehavior), new PropertyMetadata(ApplicationBarMode.Default,
                  (o, e) =>
                  {
                      PropertyChangedHelper.IfValueChangedThen<ApplicationBarCommandBindingBehavior, ApplicationBarMode>(o, e,
                        (sender, old1, new1) =>
                        {
                            if (sender.Core != null)
                            {
                                if (!PropertyChangedHelper.ValueEquals(new1, sender.Core.Mode))
                                {
                                    sender.Core.Mode = new1;
                                }
                            }

                        }
                      );
                  }));





        public Double Opacity
        {
            get { return (Double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Opacity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register("Opacity", typeof(Double), typeof(ApplicationBarCommandBindingBehavior), new PropertyMetadata((double)1,
                (o, e) =>
                {
                    PropertyChangedHelper.IfValueChangedThen<ApplicationBarCommandBindingBehavior, double>(o, e,
                      (sender, old1, new1) =>
                      {
                          if (sender.Core != null)
                          {
                              if (!PropertyChangedHelper.ValueEquals(new1, sender.Core.Opacity))
                              {
                                  sender.Core.Opacity = new1;
                              }
                          }

                      }
                    );
                }

                ));



        public event EventHandler<ApplicationBarStateChangedEventArgs> StateChanged
        {
            add
            {
                if (Core != null)
                {
                    Core.StateChanged += value;
                }
            }
            remove
            {

                if (Core != null)
                {
                    Core.StateChanged -= value;
                }
            }

        }
    }

    internal static class PropertyChangedHelper
    {
        internal static bool ValueEquals<T>(T x, T y)
        {
            return EqualityComparer<T>.Default.Equals(x, y);
        }

        internal static void IfValueChangedThen<TDependencyObject, TProperty>(object sender, DependencyPropertyChangedEventArgs e, Action<TDependencyObject, TProperty, TProperty> actionWithOldValueAndNewValue)
        {
            var o = (TDependencyObject)sender;
            var old1 = (TProperty)e.OldValue;
            var new1 = (TProperty)e.NewValue;
            if (!ValueEquals(old1, new1))
            {
                actionWithOldValueAndNewValue(o, old1, new1);
            }
        }

    }
    public abstract class ApplicationBarCommandBindingBase : DependencyObject
    {

        public ApplicationBarCommandBindingBase(PhoneApplicationPage page)
        {
            SetValue(PageContextProperty, page);
        }

        public ICommand BindingCommand
        {
            get { return (ICommand)GetValue(BindingCommandProperty); }
            set { SetValue(BindingCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BindingCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingCommandProperty =
            DependencyProperty.Register("BindingCommand", typeof(ICommand), typeof(ApplicationBarCommandBindingBase), new PropertyMetadata(null));




        public PhoneApplicationPage PageContext
        {
            get { return (PhoneApplicationPage)GetValue(PageContextProperty); }
            set
            {
                SetValue(PageContextProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for PageContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageContextProperty =
            DependencyProperty.Register("PageContext", typeof(PhoneApplicationPage), typeof(ApplicationBarCommandBindingBase), new PropertyMetadata(null));



    }

    public class ApplicationBarIconButtonCommandBinding : ApplicationBarCommandBindingBase, IApplicationBarIconButton, IApplicationBarMenuItem, IDisposable
    {
        public ApplicationBarIconButtonCommandBinding()
            : base(null)
        { }
        public ApplicationBarIconButtonCommandBinding(PhoneApplicationPage page, IApplicationBarIconButton core)
            : base(page)
        {
            Core = core;
        }
        public IApplicationBarIconButton Core
        {
            get { return (IApplicationBarIconButton)GetValue(CoreProperty); }
            set { SetValue(CoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Core.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoreProperty =
            DependencyProperty.Register("Core", typeof(IApplicationBarIconButton), typeof(ApplicationBarIconButtonCommandBinding), new PropertyMetadata(null,
                (o, e) =>
                {
                    PropertyChangedHelper.IfValueChangedThen<ApplicationBarIconButtonCommandBinding, IApplicationBarIconButton>(o, e,
                      (sender, old1, new1) =>
                      {
                          sender.IconUri = new1.IconUri;
                          sender.IsEnabled = new1.IsEnabled;
                          sender.Text = new1.Text;
                      }
                    );
                }
                ));





        public Uri IconUri
        {
            get { return (Uri)GetValue(IconUriProperty); }
            set { SetValue(IconUriProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconUriProperty =
            DependencyProperty.Register("IconUri", typeof(Uri), typeof(ApplicationBarIconButtonCommandBinding), new PropertyMetadata(
                new Uri("/Assets/AppBar/add.png", UriKind.Relative),
                (o, e) =>
                {
                    PropertyChangedHelper.IfValueChangedThen<ApplicationBarIconButtonCommandBinding, Uri>(o, e,
                      (sender, old1, new1) =>
                      {
                          if (sender.Core != null)
                          {
                              if (!PropertyChangedHelper.ValueEquals(new1, sender.Core.IconUri))
                              {
                                  sender.Core.IconUri = new1;
                              }
                          }

                      }
                    );
                }

                ));



        public event EventHandler Click
        {
            add
            {
                if (Core != null)
                {
                    Core.Click += value;
                }
            }
            remove
            {

                if (Core != null)
                {
                    Core.Click -= value;
                }
            }
        }


        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register("IsEnabled", typeof(bool), typeof(ApplicationBarIconButtonCommandBinding), new PropertyMetadata(true,
                (o, e) =>
                {
                    PropertyChangedHelper.IfValueChangedThen<ApplicationBarIconButtonCommandBinding, bool>(o, e,
                      (sender, old1, new1) =>
                      {
                          if (sender.Core != null)
                          {
                              if (!PropertyChangedHelper.ValueEquals(new1, sender.Core.IsEnabled))
                              {
                                  sender.Core.IsEnabled = new1;
                              }
                          }

                      }
                    );
                }));




        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ApplicationBarIconButtonCommandBinding), new PropertyMetadata(
                "Button",
                (o, e) =>
                {
                    PropertyChangedHelper.IfValueChangedThen<ApplicationBarIconButtonCommandBinding, string>(o, e,
                      (sender, old1, new1) =>
                      {
                          if (sender.Core != null)
                          {
                              if (!PropertyChangedHelper.ValueEquals(new1, sender.Core.Text))
                              {
                                  sender.Core.Text = new1;
                              }
                          }

                      });
                }));





        public void Dispose()
        {
            try
            {
                PageContext.ApplicationBar.Buttons.Remove(this.Core);
                this.Core = null;
            }
            catch (Exception)
            {


            }

        }
    }


}


using MVVMSidekick.Patterns;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using System.Windows;
using System.Windows.Controls;
#endif
namespace MVVMSidekick
{
    namespace VisualStates
    {
        public class VisualStateProxyBinder : ElementBinderBase<VisualStateProxyBinder>
        {

            //static Action <VisualStateProxyBinder > binding
            //    = binder =>
            //    {
            //        if (binder.Element != null)
            //        {
            //             act =
            //             () =>
            //             {
            //                 binder._Proxy.GotoState(binder._Proxy.CurrentState, binder._Proxy.CurrentUseTransitions);
            //             };

            //        }
            //    };


            public static VisualStateProxyBinder Create(VisualStateProxy proxy)
            {
                var binder = new VisualStateProxyBinder(proxy);
                RoutedEventHandler hdler =
                    (o, ea) =>
                    {

                        binder._Proxy.GotoState(binder._Proxy.CurrentState, binder._Proxy.CurrentUseTransitions);

                    };


                binder._bindingAction =
                    b =>
                    {
                        if (b.Element != null)
                        {
                            b.Element.Loaded += hdler;
                            b.Element.Unloaded +=
                                (_1, _2) =>
                                {
                                    b.Element.SetValue(VisualStateProxyBinder.BinderProperty, null);
                                };
                        }
                        hdler(null, null);

                    };


                binder._disposeAction =
                    b =>
                    {
                        if (b.Element != null)
                        {
                            b.Element.Loaded -= hdler;

                        }
                        b.Element = null;


                    };


                return binder;



            }

            private VisualStateProxyBinder(VisualStateProxy Proxy, Action<VisualStateProxyBinder> binding = null, Action<VisualStateProxyBinder> dispose = null)
                :
                base(
                binding,
                dispose

               )
            {
                _Proxy = Proxy;
            }



            VisualStateProxy _Proxy;

            public static VisualStateProxyBinder GetBinder(DependencyObject obj)
            {
                return (VisualStateProxyBinder)obj.GetValue(BinderProperty);
            }

            public static void SetBinder(DependencyObject obj, VisualStateProxyBinder value)
            {
                obj.SetValue(BinderProperty, value);
            }

            // Using a DependencyProperty as the backing store for Binder.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty BinderProperty =
                DependencyProperty.RegisterAttached(
                    "Binder",
                    typeof(VisualStateProxyBinder),
                    typeof(VisualStateProxyBinder),
                    new PropertyMetadata(
                        null,
                        BinderPropertyChangedCallback));



        }

        //[DataContract(IsReference=true) ] //if you want
        public class VisualStateProxy : DependencyObject
        {
            public VisualStateProxy()
            {
                // Use propery to init value here:

                Binder = VisualStateProxyBinder.Create(this);

            }

            //Use propvm + tab +tab  to create a new property of bindable here:






            public VisualStateProxyBinder Binder
            {
                get { return (VisualStateProxyBinder)GetValue(BinderProperty); }
                set { SetValue(BinderProperty, value); }
            }

            // Using a DependencyProperty as the backing store for Binder.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty BinderProperty =
                DependencyProperty.Register("Binder", typeof(VisualStateProxyBinder), typeof(VisualStateProxy), new PropertyMetadata(null));





            public string CurrentState
            {
                get { return (string)GetValue(CurrentStateProperty); }
                private set { SetValue(CurrentStateProperty, value); }
            }

            // Using a DependencyProperty as the backing store for CurrentState.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty CurrentStateProperty =
                DependencyProperty.Register("CurrentState", typeof(string), typeof(VisualStateProxyBinder), new PropertyMetadata(0));



            public bool CurrentUseTransitions
            {
                get { return (bool)GetValue(CurrentUseTransitionsProperty); }
                set { SetValue(CurrentUseTransitionsProperty, value); }
            }

            // Using a DependencyProperty as the backing store for CurrentUseTransitions.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty CurrentUseTransitionsProperty =
                DependencyProperty.Register("CurrentUseTransitions", typeof(bool), typeof(VisualStateProxyBinder), new PropertyMetadata(0));




            public bool GotoState(string stateName, bool useTransitions)
            {
                CurrentState = stateName;
                CurrentUseTransitions = useTransitions;
                var b = Binder;
                if (b != null)
                {

                    if (b.Element != null)
                    {
#if WPF
                        return VisualStateManager.GoToElementState(b.Element, stateName, useTransitions);
#else
                        return VisualStateManager.GoToState((Control)b.Element, stateName, useTransitions);

#endif
                    }
                }

                return false;
            }

        }


    }
}

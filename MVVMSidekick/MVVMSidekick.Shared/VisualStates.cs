// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="VisualStates.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using MVVMSidekick.Patterns;
using System;
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
		/// <summary>
		/// Class VisualStateProxyBinder.
		/// </summary>
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


			/// <summary>
			/// Creates the specified proxy.
			/// </summary>
			/// <param name="proxy">The proxy.</param>
			/// <returns>VisualStateProxyBinder.</returns>
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

			/// <summary>
			/// Initializes a new instance of the <see cref="VisualStateProxyBinder"/> class.
			/// </summary>
			/// <param name="Proxy">The proxy.</param>
			/// <param name="binding">The binding.</param>
			/// <param name="dispose">The dispose.</param>
            private VisualStateProxyBinder(VisualStateProxy Proxy, Action<VisualStateProxyBinder> binding = null, Action<VisualStateProxyBinder> dispose = null)
                :
                base(
                binding,
                dispose

               )
            {
                _Proxy = Proxy;
            }



			/// <summary>
			/// The _ proxy
			/// </summary>
            VisualStateProxy _Proxy;

			/// <summary>
			/// Gets the binder.
			/// </summary>
			/// <param name="obj">The object.</param>
			/// <returns>VisualStateProxyBinder.</returns>
            public static VisualStateProxyBinder GetBinder(DependencyObject obj)
            {
                return (VisualStateProxyBinder)obj.GetValue(BinderProperty);
            }

			/// <summary>
			/// Sets the binder.
			/// </summary>
			/// <param name="obj">The object.</param>
			/// <param name="value">The value.</param>
            public static void SetBinder(DependencyObject obj, VisualStateProxyBinder value)
            {
                obj.SetValue(BinderProperty, value);
            }

            // Using a DependencyProperty as the backing store for Binder.  This enables animation, styling, binding, etc...
			/// <summary>
			/// The binder property
			/// </summary>
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
		/// <summary>
		/// Class VisualStateProxy.
		/// </summary>
        public class VisualStateProxy : DependencyObject
        {
			/// <summary>
			/// Initializes a new instance of the <see cref="VisualStateProxy"/> class.
			/// </summary>
            public VisualStateProxy()
            {
                // Use propery to init value here:

                Binder = VisualStateProxyBinder.Create(this);

            }

            //Use propvm + tab +tab  to create a new property of bindable here:






			/// <summary>
			/// Gets or sets the binder.
			/// </summary>
			/// <value>The binder.</value>
            public VisualStateProxyBinder Binder
            {
                get { return (VisualStateProxyBinder)GetValue(BinderProperty); }
                set { SetValue(BinderProperty, value); }
            }

            // Using a DependencyProperty as the backing store for Binder.  This enables animation, styling, binding, etc...
			/// <summary>
			/// The binder property
			/// </summary>
            public static readonly DependencyProperty BinderProperty =
                DependencyProperty.Register("Binder", typeof(VisualStateProxyBinder), typeof(VisualStateProxy), new PropertyMetadata(null));





			/// <summary>
			/// Gets the state of the current.
			/// </summary>
			/// <value>The state of the current.</value>
            public string CurrentState
            {
                get { return (string)GetValue(CurrentStateProperty); }
                private set { SetValue(CurrentStateProperty, value); }
            }

            // Using a DependencyProperty as the backing store for CurrentState.  This enables animation, styling, binding, etc...
			/// <summary>
			/// The current state property
			/// </summary>
            public static readonly DependencyProperty CurrentStateProperty =
                DependencyProperty.Register("CurrentState", typeof(string), typeof(VisualStateProxyBinder), new PropertyMetadata(0));



			/// <summary>
			/// Gets or sets a value indicating whether [current use transitions].
			/// </summary>
			/// <value><c>true</c> if [current use transitions]; otherwise, <c>false</c>.</value>
            public bool CurrentUseTransitions
            {
                get { return (bool)GetValue(CurrentUseTransitionsProperty); }
                set { SetValue(CurrentUseTransitionsProperty, value); }
            }

            // Using a DependencyProperty as the backing store for CurrentUseTransitions.  This enables animation, styling, binding, etc...
			/// <summary>
			/// The current use transitions property
			/// </summary>
            public static readonly DependencyProperty CurrentUseTransitionsProperty =
                DependencyProperty.Register("CurrentUseTransitions", typeof(bool), typeof(VisualStateProxyBinder), new PropertyMetadata(0));




			/// <summary>
			/// Gotoes the state.
			/// </summary>
			/// <param name="stateName">Name of the state.</param>
			/// <param name="useTransitions">if set to <c>true</c> [use transitions].</param>
			/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

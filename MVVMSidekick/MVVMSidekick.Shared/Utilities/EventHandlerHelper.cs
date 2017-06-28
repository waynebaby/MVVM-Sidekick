// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Utilities.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reactive.Linq;
#if NETFX_CORE
using System.Reactive.Disposables;

#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Services;
using System.Reactive.Disposables;


#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Reactive.Disposables;

#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Reactive;
#endif



namespace MVVMSidekick
{

    namespace Utilities
    {
        /// <summary>
        /// Inveoker of event handler
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The event arguments.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="eventHandlerType">Type of the event handler.</param>
        public delegate void EventHandlerInvoker(object sender, object eventArgs, string eventName, Type eventHandlerType);
        /// <summary>
        /// Class EventHandlerHelper.
        /// </summary>
        public static class EventHandlerHelper
        {
            /// <summary>
            /// Creates the handler.
            /// </summary>
            /// <param name="bind">The bind.</param>
            /// <param name="eventName">Name of the event.</param>
            /// <param name="delegateType">Type of the delegate.</param>
            /// <param name="eventParametersTypes">The event parameters types.</param>
            /// <returns>Delegate.</returns>
            private static Delegate CreateHandler(
                Expression<EventHandlerInvoker> bind,
                string eventName,
                Type delegateType,
                Type[] eventParametersTypes
            )
            {
                var pars =
                        eventParametersTypes
                            .Select(
                                et => System.Linq.Expressions.Expression.Parameter(et))
                        .ToArray();
                var en = System.Linq.Expressions.Expression.Constant(eventName, typeof(string));
                var eht = System.Linq.Expressions.Expression.Constant(delegateType, typeof(Type));


                var expInvoke = System.Linq.Expressions.Expression.Invoke(bind, pars[0], pars[1], en, eht);
                var lambda = System.Linq.Expressions.Expression.Lambda(delegateType, expInvoke, pars);
                var compiled = lambda.Compile();
                return compiled;
            }

            /// <summary>
            /// Binds the event.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="eventName">Name of the event.</param>
            /// <param name="executeAction">The execute action.</param>
            /// <returns>IDisposable.</returns>
            public static IDisposable BindEvent(this object sender, string eventName, EventHandlerInvoker executeAction)

            {



                var t = sender.GetType();

                while (t != null)
                {

                    var es = t.GetEventsFromCache();
                    EventInfo ei = es.MatchOrDefault(eventName);


                    if (ei != null)
                    {

                        var handlerType = ei.EventHandlerType;
                        var eventMethod = handlerType.GetMethodsFromCache().MatchOrDefault("Invoke");
                        if (eventMethod != null)
                        {
                            var pts = eventMethod.GetParameters().Select(p => p.ParameterType)
                                .ToArray();
                            var newHandler = CreateHandler(
                                               (o, e, en, ehtype) => executeAction(o, e, en, ehtype),
                                               eventName,
                                               handlerType,
                                               pts
                                               );

#if NETFX_CORE || WINDOWS_PHONE_8
                            var etmodule = sender.GetType().GetTypeOrTypeInfo().Module;
                            try
                            {
                                return DoNetEventBind(sender, ei, newHandler);
                            }
                            catch (InvalidOperationException)
                            {
                                var newMI = WinRTEventBindMethodInfo.MakeGenericMethod(newHandler.GetType());

                                var rval = newMI.Invoke(null, new object[] { sender, ei, newHandler }) as IDisposable;


                                return rval;
                            }


#else

							return DoNetEventBind(sender, ei, newHandler);
#endif


                        }

                        return null;
                    }

                    t = t.GetTypeOrTypeInfo().BaseType;
                }

                return null;
            }


#if NETFX_CORE || WINDOWS_PHONE_8
            /// <summary>
            /// The win rt event bind method information
            /// </summary>
            static MethodInfo WinRTEventBindMethodInfo = typeof(EventHandlerHelper).GetTypeInfo().GetDeclaredMethod("WinRTEventBind");
            /// <summary>
            /// Wins the rt event bind.
            /// </summary>
            /// <typeparam name="THandler">The type of the t property.</typeparam>
            /// <param name="sender">The sender.</param>
            /// <param name="ei">The ei.</param>
            /// <param name="handler">The handler.</param>
            /// <returns>IDisposable.</returns>
            private static IDisposable WinRTEventBind<THandler>(object sender, EventInfo ei, object handler)
            {
                System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken tk = default(System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken);

                Action<System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken> remove
                    = et =>
                    {
                        ei.RemoveMethod.Invoke(sender, new object[] { et });
                    };

                System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal.AddEventHandler<THandler>(
                    ev =>
                    {
                        tk = (System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)ei.AddMethod.Invoke(sender, new object[] { ev });
                        return tk;
                    },
                    remove,
                    (THandler)handler);

                return Disposable.Create(() =>
                    {
                        System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal.RemoveEventHandler<THandler>(
                           remove,
                        (THandler)handler);


                    }
                );

            }
#endif
            /// <summary>
            /// Does the net event bind.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="ei">The ei.</param>
            /// <param name="newHandler">The new handler.</param>
            /// <returns>IDisposable.</returns>
            private static IDisposable DoNetEventBind(object sender, EventInfo ei, Delegate newHandler)
            {
                ei.AddEventHandler(sender, newHandler);
                return Disposable.Create(() => ei.RemoveEventHandler(sender, newHandler));
            }

        }

    }

}


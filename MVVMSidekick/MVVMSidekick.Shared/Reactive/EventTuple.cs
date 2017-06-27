// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Reactive.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
#if NETFX_CORE


#elif WPF



#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif



namespace MVVMSidekick
{

    namespace Reactive
    {
        /// <summary>
        /// Class EventTuple.
        /// </summary>
        public static class EventTuple
        {
            /// <summary>
            /// Creates the specified source.
            /// </summary>
            /// <typeparam name="TSource">The type of the t source.</typeparam>
            /// <typeparam name="TEventArgs">The type of the t event arguments.</typeparam>
            /// <param name="source">The source.</param>
            /// <param name="eventArgs">The instance containing the event data.</param>
            /// <returns>
            /// EventTuple&lt;TSource, TEventArgs&gt;.
            /// </returns>
            public static EventTuple<TSource, TEventArgs> Create<TSource, TEventArgs>(TSource source, TEventArgs eventArgs)
            {
                return new EventTuple<TSource, TEventArgs> { Source = source, EventArgs = eventArgs };
            }

        }

    }
}

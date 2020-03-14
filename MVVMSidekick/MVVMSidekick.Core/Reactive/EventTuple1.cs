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




namespace MVVMSidekick
{

    namespace Reactive
    {
        /// <summary>
        /// Struct EventTuple
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TEventArgs">The type of the t event arguments.</typeparam>
        public struct EventTuple<TSource, TEventArgs>
        {
            /// <summary>
            /// Gets or sets the source.
            /// </summary>
            /// <value>The source.</value>
            public TSource Source { get; set; }
            /// <summary>
            /// Gets or sets the event arguments.
            /// </summary>
            /// <value>The event arguments.</value>
            public TEventArgs EventArgs { get; set; }
        }

    }
}

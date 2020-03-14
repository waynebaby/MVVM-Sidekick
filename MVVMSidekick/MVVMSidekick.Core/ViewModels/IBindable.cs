// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="ViewModels.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;






namespace MVVMSidekick
{

    namespace ViewModels
    {
        using EventRouting;
        using MVVMSidekick.Common;
        /// <summary>
        /// Interface IBindable
        /// </summary>
        public interface IBindable : INotifyPropertyChanged, IDisposable, IDisposeGroup
        {
            bool IsInDesignMode { get; }
            /// <summary>
            /// Gets the Global event router.
            /// </summary>
            /// <value>The event router.</value>
            EventRouter GlobalEventRouter { get; }

            /// <summary>
            /// Gets or sets the event router.
            /// </summary>
            /// <value>The event router.</value>
            EventRouter LocalEventRouter { get; set; }
            /// <summary>
            /// Gets the bindable instance identifier.
            /// </summary>
            /// <value>The bindable instance identifier.</value>
            string BindableInstanceId { get; }

            /// <summary>
            /// Gets the error.
            /// </summary>
            /// <value>The error.</value>
            string ErrorMessage { get; }

            //IDictionary<string,object >  Values { get; }
            /// <summary>
            /// Gets the field names.
            /// </summary>
            /// <returns>System.String[].</returns>
            string[] GetFieldNames();
            /// <summary>
            /// Gets or sets the <see cref="System.Object"/> with the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>System.Object.</returns>
            object this[string name] { get; set; }

            IValueContainer GetValueContainer(string propertyName);
        }





    }

}

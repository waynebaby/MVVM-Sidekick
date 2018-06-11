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
#if NETFX_CORE


#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;
using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;
using System.Windows.Threading;
#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
#endif






namespace MVVMSidekick
{

    namespace ViewModels
    {
        /// <summary>
        /// Interface IValueContainer
        /// </summary>
        public interface IValueContainer : IErrorInfo, INotifyChanged,INotifyChanging
        {
            string PropertyName { get; }

            /// <summary>
            /// Gets the type of the property.
            /// </summary>
            /// <value>The type of the property.</value>
            Type PropertyType { get; }
            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            Object Value { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether this instance is copy to allowed.
            /// </summary>
            /// <value><c>true</c> if this instance is copy to allowed; otherwise, <c>false</c>.</value>
            bool IsCopyToAllowed { get; set; }

            void AddErrorEntry(string message, Exception exception = null);


            Object Model { get; }

        }





    }

}

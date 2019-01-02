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
        /// Class ErrorEntity.
        /// </summary>
        public class ErrorEntity
        {
            public ErrorEntity()
            {

            }


            public string PropertyName { get; set; }
            /// <summary>
            /// Gets or sets the message.
            /// </summary>
            /// <value>The message.</value>
            public string Message { get; set; }
            /// <summary>
            /// Gets or sets the exception.
            /// </summary>
            /// <value>The exception.</value>
            public Exception Exception { get; set; }
            /// <summary>
            /// Gets or sets the inner error information source.
            /// </summary>
            /// <value>The inner error information source.</value>
            public IErrorInfo InnerErrorInfoSource { get; set; }
            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {

                return null;// string.Format("{0}，{1}，{2}", Message, Exception, InnerErrorInfoSource);
            }
        }





    }

}

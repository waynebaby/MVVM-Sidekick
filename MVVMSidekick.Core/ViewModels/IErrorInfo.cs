﻿// ***********************************************************************
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
#if WINDOWS_UWP


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
        /// Interface IErrorInfo
        /// </summary>
        public interface IErrorInfo
        {
            /// <summary>
            /// Gets the errors.
            /// </summary>
            /// <value>The errors.</value>

            IReadOnlyDictionary<string, ErrorEntity> Errors { get; }
        }





    }

}

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
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using MVVMSidekick.Commands;
using System.Runtime.CompilerServices;
using MVVMSidekick.Reactive;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
#if WINDOWS_UWP
using Windows.UI.Xaml.Controls;


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
        using EventRouting;
        using System.Reactive.Disposables;
        using Utilities;
        using Views;
        using MVVMSidekick.Common;
        using System.Reactive;
        using System.Dynamic;

        /// <summary>
        /// Interface ICommandModel
        /// </summary>
        /// <typeparam name="TCommand">The type of the t command.</typeparam>
        /// <typeparam name="TResource">The type of the t resource.</typeparam>
        public interface ICommandModel<TCommand, TResource> : ICommand
        {
            /// <summary>
            /// Gets the command core.
            /// </summary>
            /// <value>The command core.</value>
            TCommand CommandCore { get; }
            /// <summary>
            /// Gets or sets a value indicating whether [last can execute value].
            /// </summary>
            /// <value><c>true</c> if [last can execute value]; otherwise, <c>false</c>.</value>
            bool CanExecuteValue { get; set; }
            /// <summary>
            /// Gets or sets the resource.
            /// </summary>
            /// <value>The resource.</value>
            TResource State { get; set; }
        }





    }

}

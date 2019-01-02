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
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MVVMSidekick.Commands;
using System.Reactive.Linq;
using System.Reactive;
using System.Reactive.Threading.Tasks;
using MVVMSidekick.Utilities;
using MVVMSidekick.Common;
#if NETFX_CORE


#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;


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
        /// Class TaskExecutionWindowEventArg. 
        /// </summary>
        public class TaskExecutionWindowEventArg : EventArgs
        {

            /// <summary>
            /// Initializes a new instance of the <see cref="TaskExecutionWindowEventArg"/> class.
            /// </summary>
            /// <param name="executedTask">The executed task.</param>
            /// <param name="callingContext">The calling context.</param>
            public TaskExecutionWindowEventArg(Task executedTask, CallingCodeContext callingContext)
            {
                TaskWindow = executedTask.ToObservable();
                CallingCodeContext = callingContext;
            }

            /// <summary>  
            /// Gets the task window.
            /// </summary>
            /// <value>The task window.</value>
            public IObservable<Unit> TaskWindow { get; private set; }
            /// <summary>
            /// Gets the calling code context.
            /// </summary>
            /// <value>The calling code context.</value>
            public CallingCodeContext CallingCodeContext { get; private set; }

        }

    }
}

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
        /// <para>命令模型接口，定义了命令包装器的基本契约</para>
        /// <para>Command model interface that defines the basic contract for command wrappers</para>
        /// </summary>
        /// <typeparam name="TCommand">
        /// <para>命令的具体类型</para>
        /// <para>The specific type of the command</para>
        /// </typeparam>
        /// <typeparam name="TResource">
        /// <para>命令状态资源的类型</para>
        /// <para>The type of the command state resource</para>
        /// </typeparam>
        public interface ICommandModel<TCommand, TResource> : ICommand
        {
            /// <summary>
            /// <para>获取包装的命令核心实例</para>
            /// <para>Gets the wrapped command core instance</para>
            /// </summary>
            /// <value>
            /// <para>命令核心实例</para>
            /// <para>The command core instance</para>
            /// </value>
            TCommand CommandCore { get; }
            /// <summary>
            /// <para>获取或设置命令是否可执行的值</para>
            /// <para>Gets or sets a value indicating whether the command can execute</para>
            /// </summary>
            /// <value>
            /// <para>如果命令可执行则为 <c>true</c>；否则为 <c>false</c></para>
            /// <para><c>true</c> if the command can execute; otherwise, <c>false</c></para>
            /// </value>
            bool CanExecuteValue { get; set; }
            /// <summary>
            /// <para>获取或设置命令的状态资源</para>
            /// <para>Gets or sets the state resource of the command</para>
            /// </summary>
            /// <value>
            /// <para>命令的状态资源</para>
            /// <para>The state resource of the command</para>
            /// </value>
            TResource State { get; set; }
        }





    }

}

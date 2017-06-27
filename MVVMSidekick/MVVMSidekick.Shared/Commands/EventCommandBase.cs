// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Commands.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using MVVMSidekick.ViewModels;
#if NETFX_CORE

#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using System.Windows.Controls.Primitives;

#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using Microsoft.Runtime.CompilerServices;

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

    namespace Commands
    {
        /// <summary>
        /// 事件Command,运行后马上触发一个事件，事件中带有Command实例和VM实例属性
        /// </summary>
        public abstract class EventCommandBase : ICommandWithViewModel
		{
			/// <summary>
			/// VM
			/// </summary>
			/// <value>The view model.</value>
			public BindableBase ViewModel { get; set; }

			/// <summary>
			/// 运行时触发的事件
			/// </summary>
			public event EventHandler<EventCommandEventArgs> CommandExecute;
			/// <summary>
			/// 执行时的逻辑
			/// </summary>
			/// <param name="args">执行时的事件数据</param>
			internal protected virtual void OnCommandExecute(EventCommandEventArgs args)
			{
				if (CommandExecute != null)
				{
					CommandExecute(this, args);
				}
			}


			/// <summary>
			/// 该Command是否能执行
			/// </summary>
			/// <param name="parameter">判断参数</param>
			/// <returns>是否</returns>
			public abstract bool CanExecute(object parameter);

			/// <summary>
			/// 是否能执行的值产生变化的事件
			/// </summary>
			public event EventHandler CanExecuteChanged;

			/// <summary>
			/// 是否能执行变化时触发事件的逻辑
			/// </summary>
			protected void OnCanExecuteChanged()
			{
				if (CanExecuteChanged != null)
				{
					CanExecuteChanged(this, EventArgs.Empty);
				}
			}

			/// <summary>
			/// 执行Command
			/// </summary>
			/// <param name="parameter">参数条件</param>
			public virtual void Execute(object parameter)
			{
				if (CanExecute(parameter))
				{
					OnCommandExecute(EventCommandEventArgs.Create(parameter, ViewModel));
				}
			}



		}


	}

}

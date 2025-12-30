// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa




using MVVMSidekick.ViewModels;
using System;

namespace MVVMSidekick
{

    namespace Commands
    {
        /// <summary>
        /// 事件Command，运行后马上触发一个事件，事件中带有Command实例和VM实例属性
        /// Event Command that immediately triggers an event after execution, with Command instance and VM instance properties in the event
        /// </summary>
        public abstract class EventCommandBase : ICommandWithViewModel
		{
			/// <summary>
			/// VM
			/// View Model
			/// </summary>
			/// <value>视图模型 / The view model.</value>
			public BindableBase ViewModel { get; set; }

			/// <summary>
			/// 运行时触发的事件
			/// Event triggered during execution
			/// </summary>
			public event EventHandler<EventCommandEventArgs> CommandExecute;
			
			/// <summary>
			/// 执行时的逻辑
			/// Logic during execution
			/// </summary>
			/// <param name="args">执行时的事件数据 / Event data during execution</param>
			internal protected virtual void OnCommandExecute(EventCommandEventArgs args)
			{
				if (CommandExecute != null)
				{
					CommandExecute(this, args);
				}
			}

			/// <summary>
			/// 该Command是否能执行
			/// Whether this Command can be executed
			/// </summary>
			/// <param name="parameter">判断参数 / Judgment parameter</param>
			/// <returns>是否 / Whether it can be executed</returns>
			public abstract bool CanExecute(object parameter);

			/// <summary>
			/// 是否能执行的值产生变化的事件
			/// Event when the value of whether it can be executed changes
			/// </summary>
			public event EventHandler CanExecuteChanged;

			/// <summary>
			/// 是否能执行变化时触发事件的逻辑
			/// Logic to trigger event when whether it can be executed changes
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
			/// Execute Command
			/// </summary>
			/// <param name="parameter">参数条件 / Parameter condition</param>
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

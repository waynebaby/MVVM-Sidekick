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
using System.Windows.Input;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Utilities;
#if NETFX_CORE
using Windows.UI.Xaml;

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
		/// Command被运行触发的事件数据类型
		/// </summary>
		public class EventCommandEventArgs : EventArgs
        {
			/// <summary>
			/// Gets or sets the parameter.
			/// </summary>
			/// <value>The parameter.</value>
            public Object Parameter { get; set; }
			/// <summary>
			/// Gets or sets the view model.
			/// </summary>
			/// <value>The view model.</value>
            public Object ViewModel { get; set; }
			/// <summary>
			/// Gets or sets the view sender.
			/// </summary>
			/// <value>The view sender.</value>
            public Object ViewSender { get; set; }
			/// <summary>
			/// Gets or sets the event arguments.
			/// </summary>
			/// <value>The event arguments.</value>
            public Object EventArgs { get; set; }
			/// <summary>
			/// Gets or sets the name of the event.
			/// </summary>
			/// <value>The name of the event.</value>
            public string EventName { get; set; }
			/// <summary>
			/// Gets or sets the type of the event handler.
			/// </summary>
			/// <value>The type of the event handler.</value>
            public Type EventHandlerType { get; set; }
			/// <summary>
			/// Creates the specified parameter.
			/// </summary>
			/// <param name="parameter">The parameter.</param>
			/// <param name="viewModel">The view model.</param>
			/// <param name="viewSender">The view sender.</param>
			/// <param name="eventArgs">The event arguments.</param>
			/// <param name="eventName">Name of the event.</param>
			/// <param name="eventHandlerType">Type of the event handler.</param>
			/// <returns>EventCommandEventArgs.</returns>
            public static EventCommandEventArgs Create(
                Object parameter = null,
                Object viewModel = null,
                object viewSender = null,
                object eventArgs = null,
               string eventName = null,
                Type eventHandlerType = null
                )
            {
                return new EventCommandEventArgs { Parameter = parameter, ViewModel = viewModel, ViewSender = viewSender, EventArgs = eventArgs, EventHandlerType = eventHandlerType, EventName = eventName };
            }
        }


		/// <summary>
		/// 带有VM的Command接口
		/// </summary>
        public interface ICommandWithViewModel : ICommand
        {
			/// <summary>
			/// Gets or sets the view model.
			/// </summary>
			/// <value>The view model.</value>
            BindableBase ViewModel { get; set; }
        }

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



		
        namespace EventBinding
		{






			/// <summary>
			/// Class CommandBinding.
			/// </summary>
			public class CommandBinding : FrameworkElement

            {

				/// <summary>
				/// Initializes a new instance of the <see cref="CommandBinding"/> class.
				/// </summary>
                public CommandBinding()
                {
                    base.Width = 0;
                    base.Height = 0;
                    base.Visibility =  Visibility.Collapsed ;
                }








				/// <summary>
				/// Gets or sets the name of the event.
				/// </summary>
				/// <value>The name of the event.</value>
                public string EventName { get; set; }


				/// <summary>
				/// Gets or sets the event source.
				/// </summary>
				/// <value>The event source.</value>
                public FrameworkElement EventSource
                {
                    get { return (FrameworkElement)GetValue(EventSourceProperty); }
                    set { SetValue(EventSourceProperty, value); }
                }

                // Using a DependencyProperty as the backing store for EventSource.  This enables animation, styling, binding, etc...
				/// <summary>
				/// The event source property
				/// </summary>
                public static readonly DependencyProperty EventSourceProperty =
                    DependencyProperty.Register("EventSource", typeof(FrameworkElement), typeof(CommandBinding), new PropertyMetadata(null,
                        (dobj, arg) =>
                        {
                            CommandBinding obj = dobj as CommandBinding;
                            if (obj == null)
                            {
                                return;
                            }
                            if (obj.oldEventDispose != null)
                            {
                                obj.oldEventDispose.Dispose();
                            }
                            var nv = arg.NewValue;
                            if (nv != null)
                            {

                                obj.oldEventDispose = nv.BindEvent(
                                    obj.EventName,
                                    (o, a, en, ehType) =>
                                    {
                                        obj.ExecuteFromEvent(o, a, en, ehType);
                                    });

                            }

                        }


                        ));


				/// <summary>
				/// The old event dispose
				/// </summary>
                IDisposable oldEventDispose;

				/// <summary>
				/// Gets or sets the command.
				/// </summary>
				/// <value>The command.</value>
                public ICommand Command
                {
                    get { return (ICommand)GetValue(CommandProperty); }
                    set { SetValue(CommandProperty, value); }
                }

                // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
				/// <summary>
				/// The command property
				/// </summary>
                public static readonly DependencyProperty CommandProperty =
                    DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandBinding), new PropertyMetadata(null));




				/// <summary>
				/// Gets or sets the parameter.
				/// </summary>
				/// <value>The parameter.</value>
                public Object Parameter
                {
                    get { return (Object)GetValue(ParameterProperty); }
                    set { SetValue(ParameterProperty, value); }
                }

                // Using a DependencyProperty as the backing store for Parameter.  This enables animation, styling, binding, etc...
				/// <summary>
				/// The parameter property
				/// </summary>
                public static readonly DependencyProperty ParameterProperty =
                    DependencyProperty.Register("Parameter", typeof(Object), typeof(CommandBinding), new PropertyMetadata(null));



				/// <summary>
				/// Executes from event.
				/// </summary>
				/// <param name="sender">The sender.</param>
				/// <param name="eventArgs">The event arguments.</param>
				/// <param name="eventName">Name of the event.</param>
				/// <param name="eventHandlerType">Type of the event handler.</param>
                public void ExecuteFromEvent(object sender, object eventArgs, string eventName, Type eventHandlerType)
                {
                    object vm = null;

                    var s = (sender as FrameworkElement);

                    if (Command == null)
                    {
                        return;
                    }
                    var cvm = Command as ICommandWithViewModel;
                    if (cvm != null)
                    {
                        vm = cvm.ViewModel;
                    }


                    var newe = EventCommandEventArgs.Create(Parameter, vm, sender, eventArgs, eventName, eventHandlerType);

                    if (Command.CanExecute(newe))
                    {
                        var spEventCommand = Command as EventCommandBase;
                        if (spEventCommand == null)
                        {
                            Command.Execute(newe);
                        }
                        else
                        {
                            spEventCommand.OnCommandExecute(newe);

                        }
                    }

                }



            }













        }
		/// <summary>
		/// 事件Command的助手类
		/// </summary>
		public static class EventCommandHelper
		{
			/// <summary>
			/// 为一个事件Command制定一个VM
			/// </summary>
			/// <typeparam name="TCommand">事件Command具体类型</typeparam>
			/// <param name="cmd">事件Command实例</param>
			/// <param name="viewModel">VM实例</param>
			/// <returns>事件Command实例本身</returns>
			public static TCommand WithViewModel<TCommand>(this TCommand cmd, BindableBase viewModel)
				where TCommand : EventCommandBase
			{
				cmd.ViewModel = viewModel;
				return cmd;
			}

		}


    }

}

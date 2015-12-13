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
        /// Class EventTuple.
        /// </summary>
        public static class EventTuple
        {
            /// <summary>
            /// Creates the specified source.
            /// </summary>
            /// <typeparam name="TSource">The type of the t source.</typeparam>
            /// <typeparam name="TEventArgs">The type of the t event arguments.</typeparam>
            /// <param name="source">The source.</param>
            /// <param name="eventArgs">The instance containing the event data.</param>
            /// <returns>
            /// EventTuple&lt;TSource, TEventArgs&gt;.
            /// </returns>
            public static EventTuple<TSource, TEventArgs> Create<TSource, TEventArgs>(TSource source, TEventArgs eventArgs)
            {
                return new EventTuple<TSource, TEventArgs> { Source = source, EventArgs = eventArgs };
            }

        }
        /// <summary>
        /// Struct EventTuple
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TEventArgs">The type of the t event arguments.</typeparam>
        public struct EventTuple<TSource, TEventArgs>
        {
            /// <summary>
            /// Gets or sets the source.
            /// </summary>
            /// <value>The source.</value>
            public TSource Source { get; set; }
            /// <summary>
            /// Gets or sets the event arguments.
            /// </summary>
            /// <value>The event arguments.</value>
            public TEventArgs EventArgs { get; set; }
        }



        /// <summary>
        /// Class ReactiveCommand.
        /// </summary>
        public class ReactiveCommand : EventCommandBase, ICommand, IObservable<EventPattern<EventCommandEventArgs>>
        {



            /// <summary>
            /// The _ lazy observable execute
            /// </summary>
            protected Lazy<IObservable<EventPattern<EventCommandEventArgs>>> _LazyObservableExecute;
            /// <summary>
            /// The _ lazy observer can execute
            /// </summary>
            protected Lazy<IObserver<bool>> _LazyObserverCanExecute;
            /// <summary>
            /// The _ current can execute observer value
            /// </summary>
            protected bool _CurrentCanExecuteObserverValue;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReactiveCommand"/> class.
            /// </summary>
            protected ReactiveCommand()
            {
                ConfigReactive();

            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ReactiveCommand"/> class.
            /// </summary>
            /// <param name="canExecute">if set to <c>true</c> [can execute].</param>
            public ReactiveCommand(bool canExecute = false)
                : this()
            {
                _CurrentCanExecuteObserverValue = canExecute;
            }


            /// <summary>
            /// Configurations the reactive.
            /// </summary>
            protected void ConfigReactive()
            {
                _LazyObservableExecute = new Lazy<IObservable<EventPattern<EventCommandEventArgs>>>
                (
                    () =>
                    {
                        var ob = Observable.FromEventPattern<EventHandler<EventCommandEventArgs>, EventCommandEventArgs>
                    (
                        eh =>
                        {
                            this.CommandExecute += eh;
                        },
                        eh =>
                        {
                            this.CommandExecute -= eh;
                        }
                    );

                        return ob;
                    }
                );

                _LazyObserverCanExecute = new Lazy<IObserver<bool>>
                (
                    () =>
                        Observer.Create<bool>(
                        canExe =>
                        {

							//var oldv = this._CurrentCanExecuteObserverValue;
							_CurrentCanExecuteObserverValue = canExe;
							//if (oldv != canExe)
							//{
							//    OnCanExecuteChanged();
							//}
							OnCanExecuteChanged();
                        }
                        )

                );
            }
            /// <summary>
            /// Gets the can execute observer.
            /// </summary>
            /// <value>The can execute observer.</value>
            private IObserver<bool> CanExecuteObserver { get { return _LazyObserverCanExecute.Value; } }

            public IDisposable ListenCanExecuteObservable(IObservable<bool> canExecuteSeq)
            {
                return Observable.Range(0, 1)
                        .Select(_ => this._CurrentCanExecuteObserverValue)
                        .Concat(canExecuteSeq)
                       .Subscribe(CanExecuteObserver);
            }

            /// <summary>
            /// Determines whether this instance can execute the specified parameter.
            /// </summary>
            /// <param name="parameter">The parameter.</param>
            /// <returns><c>true</c> if this instance can execute the specified parameter; otherwise, <c>false</c>.</returns>
            public override bool CanExecute(object parameter)
            {
                return _CurrentCanExecuteObserverValue;
            }






            /// <summary>
            /// Subscribes the specified observer.
            /// </summary>
            /// <param name="observer">The observer.</param>
            /// <returns>IDisposable.</returns>
            public IDisposable Subscribe(IObserver<EventPattern<EventCommandEventArgs>> observer)
            {
                return _LazyObservableExecute
                      .Value
                      .Subscribe(observer);
            }
        }


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

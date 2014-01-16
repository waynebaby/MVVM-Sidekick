using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Commands;
using System.Runtime.CompilerServices;
using MVVMSidekick.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using System.Reactive.Threading.Tasks;
using MVVMSidekick.EventRouting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Collections;
using MVVMSidekick.Utilities;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using System.Collections.Concurrent;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Xaml.Controls.Primitives;

#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;

#elif SILVERLIGHT_5||SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8||WINDOWS_PHONE_7
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

        public static class EventTuple
        {
            public static EventTuple<TSource, TEventArgs> Create<TSource, TEventArgs>(TSource source, TEventArgs eventArgs)
            {
                return new EventTuple<TSource, TEventArgs> { Source = source, EventArgs = eventArgs };
            }

        }
        public struct EventTuple<TSource, TEventArgs>
        {
            public TSource Source { get; set; }
            public TEventArgs EventArgs { get; set; }
        }

        public static class MVVMRxExtensions
        {

            //public static IDisposable SubscribeToRouter(this IObservable<Task> executionSequence, BindableBase model, string eventName, CallingCodeContext callingCodeContext)
            //{
            //    EventRouting.EventRouter.Instance.GetEventObject<TaskExecutionWindowEventArg>().RaiseEvent(model, eventName, callingCodeContext);
            //    return executionSequence.Subscribe();

            //}

            /// <summary>
            /// Register a Do action to the observer, Notify the value in this sequence to EventRouter
            /// </summary>
            /// <typeparam name="T">Sequence Value Type</typeparam>
            /// <param name="sequence">value sequence</param>
            /// <param name="eventRounter"> target </param>
            /// <param name="source">value source</param>
            /// <param name="registerName">log name</param>
            /// <returns>same value sequence inputed</returns>
            public static IObservable<T> DoNotifyEventRouter<T>(this IObservable<T> sequence, EventRouter eventRounter, object source = null, [CallerMemberName] string registerName = null)
            {
                return
                    sequence.Do(
                            v => eventRounter.RaiseEvent(source, v, registerName)

                        );

            }

            /// <summary>
            /// Register a Do action to the observer, Notify the value in this sequence to EventRouter
            /// </summary>
            /// <typeparam name="T">Sequence Value Type</typeparam>
            /// <param name="sequence">value sequence</param>
            /// <param name="source">value source</param>
            /// <param name="registerName">log name</param>
            /// <returns>same value sequence inputed</returns>
            public static IObservable<T> DoNotifyDefaultEventRouter<T>(this IObservable<T> sequence, object source = null, [CallerMemberName] string registerName = null)
            {
                return DoNotifyEventRouter(sequence, EventRouter.Instance, source, registerName);
            }



            public static IObservable<Task<Tout>> DoExecuteUIBusyTask<Tin, Tout>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task<Tout>> taskBody, CancellationToken cancellationToken)
            {
                return sequence.Select
                    (
                        inContext => vm.ExecuteTask(taskBody, inContext, cancellationToken, true)
                    );
            }

            public static IObservable<Task<Tout>> DoExecuteUITask<Tin, Tout>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task<Tout>> taskBody, CancellationToken cancellationToken)
            {
                return sequence.Select
                    (
                        inContext => vm.ExecuteTask(taskBody, inContext, cancellationToken, false)
                    );
            }
            public static IObservable<Task> DoExecuteUIBusyTask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task> taskBody, CancellationToken cancellationToken)
            {
                return sequence.Select
                (
                    inContext => vm.ExecuteTask(taskBody, inContext, cancellationToken, true)
                );
            }

            public static IObservable<Task> DoExecuteUITask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task> taskBody, CancellationToken cancellationToken)
            {
                return sequence.Select
               (
                   inContext => vm.ExecuteTask(taskBody, inContext, cancellationToken, false)
               );
            }





            public static IObservable<Task<Tout>> DoExecuteUIBusyTask<Tin, Tout>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task<Tout>> taskBody)
            {
                return sequence.Select
                    (
                        inContext => vm.ExecuteTask(taskBody, inContext, true)
                    );
            }

            public static IObservable<Task<Tout>> DoExecuteUITask<Tin, Tout>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task<Tout>> taskBody)
            {
                return sequence.Select
                    (
                        inContext => vm.ExecuteTask(taskBody, inContext, false)
                    );
            }
            public static IObservable<Task> DoExecuteUIBusyTask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task> taskBody)
            {
                return sequence.Select
                (
                    inContext => vm.ExecuteTask(taskBody, inContext, true)
                );
            }

            public static IObservable<Task> DoExecuteUITask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task> taskBody)
            {
                return sequence.Select
               (
                   inContext => vm.ExecuteTask(taskBody, inContext, false)
               );
            }

            /// <summary>
            /// <para>Create a instance of IObservable that fires when property changed event is raised.</para>
            /// <para>创建一个监视属性变化事件观察者IObservable实例。</para>
            /// </summary>
            /// <returns></returns>
            public static IObservable<EventPattern<PropertyChangedEventArgs>> CreatePropertyChangedObservable(this BindableBase bindable)
            {
                return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                        eh => bindable.PropertyChanged += eh,
                        eh => bindable.PropertyChanged -= eh
                    )
                    .Where(_ => bindable.IsNotificationActivated);
            }


            public static IObservable<EventPattern<NotifyCollectionChangedEventArgs>> GetEventObservable<T>(this ObservableCollection<T> source, BindableBase model)
            {
                var rval = Observable
                  .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>
                      (
                          ev => source.CollectionChanged += ev,
                          ev => source.CollectionChanged -= ev
                      ).Where(_ => model.IsNotificationActivated);
                return rval;
            }
            public static IObservable<EventTuple<ValueContainer<TValue>, TValue>> GetNewValueObservable<TValue>
                (
                    this ValueContainer<TValue> source

                )
            {

                return Observable.FromEventPattern<EventHandler<ValueChangedEventArgs<TValue>>, ValueChangedEventArgs<TValue>>(
                        eh => source.ValueChanged += eh,
                        eh => source.ValueChanged -= eh)
                        .Select(
                            x => EventTuple.Create(source, x.EventArgs.NewValue)

                        );

            }

            public static IObservable<EventTuple<ValueContainer<TValue>, ValueChangedEventArgs<TValue>>>
                GetEventObservable<TValue>(this ValueContainer<TValue> source)
            {

                var eventArgSeq = Observable.FromEventPattern<EventHandler<ValueChangedEventArgs<TValue>>, ValueChangedEventArgs<TValue>>(
                        eh => source.ValueChanged += eh,
                        eh => source.ValueChanged -= eh);
                return eventArgSeq.Select(
                            x => EventTuple.Create(source, x.EventArgs)
                        );
                ;
            }


            public static IObserver<TValue> AsObserver<TValue>(this ValueContainer<TValue> source)
            {
                return Observer.Create<TValue>(v => source.SetValueAndTryNotify(v));

            }


            public static IObservable<object> GetNullObservable<TValue>(this ValueContainer<TValue> source)
            {

                var eventArgSeq = Observable.FromEventPattern<EventHandler<ValueChangedEventArgs<TValue>>, ValueChangedEventArgs<TValue>>(
                        eh => source.ValueChanged += eh,
                        eh => source.ValueChanged -= eh);
                return eventArgSeq.Select(
                            x => null as object
                        );
                ;
            }
            /// <summary>
            /// 转化
            /// </summary>
            /// <typeparam name="TEventArgs"></typeparam>
            /// <param name="source"></param>
            /// <returns></returns>
            [Obsolete("The source is already  IObservable<RouterEventData<TEventArgs>>")]
            public static IObservable<RouterEventData<TEventArgs>>
                GetRouterEventObservable<TEventArgs>(this MVVMSidekick.EventRouting.EventRouter.EventObject<TEventArgs> source)
#if !NETFX_CORE
 where TEventArgs : EventArgs
#endif
            {


                return source;

            }
            /// <summary>
            /// Bind Command to IsUIBusy property.
            /// </summary>
            /// <typeparam name="TCommand">A sub class of ReactiveCommand</typeparam>
            /// <typeparam name="TResource">The resource type of CommandModel</typeparam>
            /// <typeparam name="TViewModel">The View Model type command wanna bind to</typeparam>
            /// <param name="command">Command itself</param>
            /// <param name="model">The View Model  command wanna bind to</param>
            /// <returns>command instance itself</returns>
            public static CommandModel<TCommand, TResource> ListenToIsUIBusy<TCommand, TResource, TViewModel>(this CommandModel<TCommand, TResource> command, ViewModelBase<TViewModel> model, bool canExecuteWhenBusy = false)
                where TViewModel : ViewModelBase<TViewModel>
                where TCommand : ReactiveCommand
            {

                //See Test  CommandListenToUIBusy_Test
                model.GetValueContainer(x => x.IsUIBusy).GetNewValueObservable()
                  .Select(e =>
                      !(canExecuteWhenBusy ^ e.EventArgs))
                  .Subscribe(command.CommandCore.CanExecuteObserver)
                  .DisposeWith(model);

                return command;
            }

        }

        public class ReactiveCommand : EventCommandBase, ICommand, IObservable<EventPattern<EventCommandEventArgs>>
        {



            protected Lazy<IObservable<EventPattern<EventCommandEventArgs>>> _LazyObservableExecute;
            protected Lazy<IObserver<bool>> _LazyObserverCanExecute;
            protected bool _CurrentCanExecuteObserverValue;

            protected ReactiveCommand()
            {
                ConfigReactive();

            }

            public ReactiveCommand(bool canExecute = false)
                : this()
            {
                _CurrentCanExecuteObserverValue = canExecute;
            }


            virtual protected void ConfigReactive()
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
                            var oldv = this._CurrentCanExecuteObserverValue;
                            _CurrentCanExecuteObserverValue = canExe;
                            if (oldv != canExe)
                            {
                                OnCanExecuteChanged();
                            }
                        }
                        )

                );
            }
            public IObserver<bool> CanExecuteObserver { get { return _LazyObserverCanExecute.Value; } }

            public override bool CanExecute(object parameter)
            {
                return _CurrentCanExecuteObserverValue;
            }






            public IDisposable Subscribe(IObserver<EventPattern<EventCommandEventArgs>> observer)
            {
                return _LazyObservableExecute
                      .Value
                      .Subscribe(observer);
            }
        }


        public class TaskExecutionWindowEventArg : EventArgs
        {

            public TaskExecutionWindowEventArg(Task executedTask, CallingCodeContext callingContext)
            {
                TaskWindow = executedTask.ToObservable();
                CallingCodeContext = callingContext;
            }

            public IObservable<Unit> TaskWindow { get; private set; }
            public CallingCodeContext CallingCodeContext { get; private set; }

        }

    }
}

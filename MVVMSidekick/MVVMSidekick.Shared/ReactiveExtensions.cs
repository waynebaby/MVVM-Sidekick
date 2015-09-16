
// ***********************************************************************
// Assembly         : MVVMSidekick
// Author           : waywa
// Created          : 01-04-2015
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="ReactiveExtensions.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Linq;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using MVVMSidekick.ViewModels;
using System.Runtime.CompilerServices;
using System.Reactive.Linq;
using System.Reactive;
using System.Reactive.Threading.Tasks;
using MVVMSidekick.EventRouting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        /// 		 MVVMRxExtensions
        /// </summary>
        public static class MVVMRxExtensions
        {


            /// <summary>
            /// Register a Do action to the observer, Notify the value in this sequence to EventRouter
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="sequence">value sequence</param>
            /// <param name="eventRouter">target</param>
            /// <param name="source">value source</param>
            /// <param name="registerName">log name</param>
            /// <returns>
            /// same value sequence inputed
            /// </returns>
            public static IObservable<T> DoNotifyEventRouter<T>(this IObservable<T> sequence, EventRouter eventRouter, object source = null, [CallerMemberName] string registerName = null)
            {
                return
                    sequence.Do(
                            v => eventRouter.RaiseEvent(source, v, registerName)

                        );

            }

            /// <summary>
            /// Register a Do action to the observer, Notify the value in this sequence to EventRouter
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="sequence">value sequence</param>
            /// <param name="source">value source</param>
            /// <param name="registerName">log name</param>
            /// <returns>
            /// same value sequence inputed
            /// </returns>
            public static IObservable<T> DoNotifyDefaultEventRouter<T>(this IObservable<T> sequence, object source = null, [CallerMemberName] string registerName = null)
            {
                return DoNotifyEventRouter(sequence, EventRouter.Instance, source, registerName);
            }



            /// <summary>
            /// Does the execute UI busy task.
            /// </summary>
            /// <typeparam name="Tin">The type of the in.</typeparam>
            /// <typeparam name="Tout">The type of the out.</typeparam>
            /// <param name="sequence">The sequence.</param>
            /// <param name="vm">The vm.</param>
            /// <param name="taskBody">The task body.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <returns>
            /// IObservable&lt;Task&lt;Tout&gt;&gt;.
            /// </returns>
            public static IObservable<Task<Tout>> DoExecuteUIBusyTask<Tin, Tout>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task<Tout>> taskBody, CancellationToken cancellationToken)
            {
                return sequence.Select
                    (
                        inContext => vm.ExecuteTask(taskBody, inContext, cancellationToken, true)
                    );
            }

            /// <summary>
            /// Does the execute UI task.
            /// </summary>
            /// <typeparam name="Tin">The type of the in.</typeparam>
            /// <typeparam name="Tout">The type of the out.</typeparam>
            /// <param name="sequence">The sequence.</param>
            /// <param name="vm">The vm.</param>
            /// <param name="taskBody">The task body.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <returns>
            /// IObservable&lt;Task&lt;Tout&gt;&gt;.
            /// </returns>
            public static IObservable<Task<Tout>> DoExecuteUITask<Tin, Tout>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task<Tout>> taskBody, CancellationToken cancellationToken)
            {
                return sequence.Select
                    (
                        inContext => vm.ExecuteTask(taskBody, inContext, cancellationToken, false)
                    );
            }
            /// <summary>
            /// Does the execute UI busy task.
            /// </summary>
            /// <typeparam name="Tin">The type of the in.</typeparam>
            /// <param name="sequence">The sequence.</param>
            /// <param name="vm">The vm.</param>
            /// <param name="taskBody">The task body.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <returns>
            /// IObservable&lt;Task&gt;.
            /// </returns>
            public static IObservable<Task> DoExecuteUIBusyTask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task> taskBody, CancellationToken cancellationToken)
            {
                return sequence.Select
                (
                    inContext => vm.ExecuteTask(taskBody, inContext, cancellationToken, true)
                );
            }

            /// <summary>
            /// Does the execute UI task.
            /// </summary>
            /// <typeparam name="Tin">The type of the in.</typeparam>
            /// <param name="sequence">The sequence.</param>
            /// <param name="vm">The vm.</param>
            /// <param name="taskBody">The task body.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <returns>
            /// IObservable&lt;Task&gt;.
            /// </returns>
            public static IObservable<Task> DoExecuteUITask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task> taskBody, CancellationToken cancellationToken)
            {
                return sequence.Select
               (
                   inContext => vm.ExecuteTask(taskBody, inContext, cancellationToken, false)
               );
            }





            /// <summary>
            /// Does the execute UI busy task.
            /// </summary>
            /// <typeparam name="Tin">The type of the tin.</typeparam>
            /// <typeparam name="Tout">The type of the tout.</typeparam>
            /// <param name="sequence">The sequence.</param>
            /// <param name="vm">The vm.</param>
            /// <param name="taskBody">The task body.</param>
            /// <returns>IObservable&lt;Task&lt;Tout&gt;&gt;.</returns>
            public static IObservable<Task<Tout>> DoExecuteUIBusyTask<Tin, Tout>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task<Tout>> taskBody)
            {
                return sequence.Select
                    (
                        inContext => vm.ExecuteTask(taskBody, inContext, true)
                    );
            }

            /// <summary>
            /// Does the execute UI task.
            /// </summary>
            /// <typeparam name="Tin">The type of the tin.</typeparam>
            /// <typeparam name="Tout">The type of the tout.</typeparam>
            /// <param name="sequence">The sequence.</param>
            /// <param name="vm">The vm.</param>
            /// <param name="taskBody">The task body.</param>
            /// <returns>IObservable&lt;Task&lt;Tout&gt;&gt;.</returns>
            public static IObservable<Task<Tout>> DoExecuteUITask<Tin, Tout>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task<Tout>> taskBody)
            {
                return sequence.Select
                    (
                        inContext => vm.ExecuteTask(taskBody, inContext, false)
                    );
            }


            /// <summary>
            /// Does the execute UI busy task.
            /// </summary>
            /// <typeparam name="Tin">The type of the tin.</typeparam>
            /// <param name="sequence">The sequence.</param>
            /// <param name="vm">The vm.</param>
            /// <param name="taskBody">The task body.</param>
            /// <returns>
            /// IObservable&lt;Task&gt;.
            /// </returns>
            public static IObservable<Task> DoExecuteUIBusyTask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task> taskBody)
            {
                return sequence.Select
                (
                    inContext => vm.ExecuteTask(taskBody, inContext, true)
                );
            }

            /// <summary>Does the execute UI task.</summary>
            /// <typeparam name="Tin">The type of the in.</typeparam>
            /// <param name="sequence">The sequence.</param>
            /// <param name="vm">The vm.</param>
            /// <param name="taskBody">The task body.</param>
            /// <returns>IObservable&lt;Task&gt;.</returns>
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
            /// <param name="bindable">The bindable.</param>
            /// <returns>IObservable&lt;EventPattern&lt;PropertyChangedEventArgs&gt;&gt;.</returns>
            public static IObservable<EventPattern<PropertyChangedEventArgs>> CreatePropertyChangedObservable(this BindableBase bindable)
            {
                return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                        eh => bindable.PropertyChanged += eh,
                        eh => bindable.PropertyChanged -= eh
                    )
                    .Where(_ => bindable.IsNotificationActivated);
            }


            /// <summary>
            /// Gets the event observable.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="source">The source.</param>
            /// <param name="model">The model.</param>
            /// <returns>
            /// IObservable&lt;EventPattern&lt;NotifyCollectionChangedEventArgs&gt;&gt;.
            /// </returns>
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
            /// <summary>Gets the new value observable.</summary>
            /// <typeparam name="TValue">The type of the value.</typeparam>
            /// <param name="source">The source.</param>
            /// <returns>
            /// IObservable&lt;EventTuple&lt;ValueContainer&lt;TValue&gt;, TValue&gt;&gt;.
            /// </returns>
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

            /// <summary>Gets the event observable.</summary>
            /// <typeparam name="TValue">The type of the t service.</typeparam>
            /// <param name="source">The source.</param>
            /// <returns>
            /// IObservable&lt;EventTuple&lt;ValueContainer&lt;TValue&gt;, ValueChangedEventArgs&lt;TValue&gt;&gt;&gt;.
            /// </returns>
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
            /// <summary>
            /// Gets the null observable.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <returns>IObservable&lt;System.Object&gt;.</returns>
            public static IObservable<object> GetNullObservable(this INotifyChanges source)
            {

                var eventArgSeq = Observable.FromEventPattern(
                        eh => source.ValueChangedWithNothing += eh,
                        eh => source.ValueChangedWithNothing -= eh);
                return eventArgSeq.Select(
                            x => null as object
                        );
                ;
            }
            /// <summary>
            /// Gets the named observable.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <returns>IObservable&lt;System.String&gt;.</returns>
            public static IObservable<string> GetNamedObservable(this INotifyChanges source)
            {

                var eventArgSeq = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                        eh => source.ValueChangedWithNameOnly += eh,
                        eh => source.ValueChangedWithNameOnly -= eh);
                return eventArgSeq.Select(
                            x => x.EventArgs.PropertyName
                        );
                ;
            }

            /// <summary>
            /// Listens to the changed properties and merge the event to a sequence.
            /// </summary>
            /// <typeparam name="TModel">The type of the source model.</typeparam>
            /// <param name="source">The source model.</param>
            /// <param name="properties">The properties expressions.</param>
            /// <returns>Event sequence</returns>
            public static IObservable<EventTuple<object, string>> ListenChanged<TModel>(this TModel source,
                    params Expression<Func<TModel, object>>[] properties
                ) where TModel : BindableBase<TModel>
            {

                return source.GetValueContainers(properties)
                    .ToObservable()
                    .SelectMany(x => x.GetNamedObservable().Select(y =>
                        new EventTuple<object, string>()
                        {
                            Source = source,
                            EventArgs = y
                        }));


            }


            /// <summary>
            /// Alsoes the listen changed with.
            /// </summary>
            /// <typeparam name="TModel">The type of the model.</typeparam>
            /// <param name="sequence">The event sequence.</param>
            /// <param name="secondSource">The second source.</param>
            /// <param name="properties">The properties.</param>
            /// <returns>Merged event Sequence</returns>
            public static IObservable<EventTuple<object, string>> AlsoListenChangedWith<TModel>(this IObservable<EventTuple<object, string>> sequence,
                TModel secondSource,
                params Expression<Func<TModel, object>>[] properties
                ) where TModel : BindableBase<TModel>
            {

                var another = ListenChanged(secondSource);
                return Observable.Merge(sequence, another);

            }

            /// <summary>
            /// Cast a value container as a observer.
            /// </summary>
            /// <typeparam name="TValue">The type of the value container's value.</typeparam>
            /// <param name="source">The source.</param>
            /// <returns></returns>
            public static IObserver<TValue> AsObserver<TValue>(this ValueContainer<TValue> source)
            {
                return Observer.Create<TValue>(v => source.SetValueAndTryNotify(v));

            }
            /// <summary>
            /// 转化
            /// </summary>
            /// <typeparam name="TEventArgs"></typeparam>
            /// <param name="source"></param>
            /// <returns></returns>
            [Obsolete("The source is already moved to  IObservable<RouterEventData<TEventArgs>>")]
            public static IObservable<RouterEventData<TEventArgs>>
                GetRouterEventObservable<TEventArgs>(this MVVMSidekick.EventRouting.EventRouter.EventChannel<TEventArgs> source)
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
            /// <param name="canExecuteWhenBusy">if can execute when ui busy , input true</param>
            /// <returns>command instance itself</returns>
            public static CommandModel<TCommand, TResource> ListenToIsUIBusy<TCommand, TResource, TViewModel>(this CommandModel<TCommand, TResource> command, ViewModelBase<TViewModel> model, bool canExecuteWhenBusy = false)
                where TViewModel : ViewModelBase<TViewModel>
                where TCommand : ReactiveCommand
            {

                //See Test  CommandListenToUIBusy_Test
                command.CommandCore.ListenCanExecuteObservable(
                Observable.Range(0, 1)
                    .Select(x => (x == 0) ? !command.LastCanExecuteValue : command.LastCanExecuteValue)
                    .Concat(
                        model.GetValueContainer(x => x.IsUIBusy)
                        .GetNewValueObservable()
                        .Select(e =>
                             canExecuteWhenBusy ? canExecuteWhenBusy : (!e.EventArgs)
                        )))
                    .DisposeWith(model);

                return command;
            }

        }
    }
}

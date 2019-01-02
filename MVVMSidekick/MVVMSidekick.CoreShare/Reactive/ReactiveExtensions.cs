
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
using MVVMSidekick.Commands;
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
            public static IObservable<T> DoNotifyEventRouter<T>(this IObservable<T> sequence, EventRouter eventRouter = null, object source = null, [CallerMemberName] string registerName = null)
            {
                return
                    sequence.Do(v => eventRouter.RaiseEvent(source, v, registerName));

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





            public static IObservable<(Task<Tout> Task, Tin InputContext, CancellationToken CancellationToken)> DoExecuteUIFunctionTask<Tin, Tout>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task<Tout>> taskBody, CancellationToken cancellationToken = default(CancellationToken))
            {
                return sequence.Select
                    (
                          inContext => (vm.ExecuteFunctionTask(taskBody, inContext, cancellationToken, false), inContext, cancellationToken)
                    );
            }

            public static IObservable<(Task<Tout> Task, Tin InputContext, CancellationToken CancellationToken)> DoExecuteUIFunctionTask<Tin, Tout>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task<Tout>> taskBody)
            {
                return DoExecuteUIFunctionTask(sequence, vm, (inContext, cancel) => taskBody(inContext));
            }

            public static IObservable<(Task Task, Tin InputContext, CancellationToken CancellationToken)> DoExecuteUIActionTask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task> taskBody, CancellationToken cancellationToken = default(CancellationToken))
            {
                return sequence.Select
                    (
                          inContext => (vm.ExecuteTask(taskBody, inContext, cancellationToken, false), inContext, cancellationToken)
                    );
            }

            public static IObservable<(Task Task, Tin InputContext, CancellationToken CancellationToken)> DoExecuteUIActionTask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task> taskBody)
            {
                return DoExecuteUIActionTask(sequence, vm, (inContext, cancel) => taskBody(inContext));
            }








            public static IObservable<(Task<Tout> Task, Tin InputContext, CancellationToken CancellationToken)> DoExecuteUIBusyFunctionTask<Tin, Tout>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task<Tout>> taskBody, CancellationToken cancellationToken = default(CancellationToken))
            {
                return sequence.Select
                    (
                          inContext => (vm.ExecuteFunctionTask(taskBody, inContext, cancellationToken, false), inContext, cancellationToken)
                    );
            }

            public static IObservable<(Task<Tout> Task, Tin InputContext, CancellationToken CancellationToken)> DoExecuteUIBusyFunctionTask<Tin, Tout>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task<Tout>> taskBody)
            {
                return DoExecuteUIBusyFunctionTask(sequence, vm, (inContext, cancel) => taskBody(inContext));
            }

            public static IObservable<(Task Task, Tin InputContext, CancellationToken CancellationToken)> DoExecuteUIBusyActionTask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task> taskBody, CancellationToken cancellationToken = default(CancellationToken))
            {
                return sequence.Select
                    (
                          inContext => (vm.ExecuteTask(taskBody, inContext, cancellationToken, false), inContext, cancellationToken)
                    );
            }

            public static IObservable<(Task Task, Tin InputContext, CancellationToken CancellationToken)> DoExecuteUIBusyActionTask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task> taskBody)
            {
                return DoExecuteUIBusyActionTask(sequence, vm, (inContext, cancel) => taskBody(inContext));
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
            public static IObservable<EventPattern<NotifyCollectionChangedEventArgs>> GetCollectionChangedEventObservable<T>(this ObservableCollection<T> source, BindableBase model)
            {
                var rval = Observable
                  .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>
                      (
                          ev => source.CollectionChanged += ev,
                          ev => source.CollectionChanged -= ev
                      ).Where(_ => model.IsNotificationActivated);
                return rval;
            }


            /// <summary>Gets the value changed event observable.</summary>
            /// <typeparam name="TValue">The type of the t service.</typeparam>
            /// <param name="source">The source.</param>
            /// <returns>
            /// IObservable&lt;EventTuple&lt;ValueContainer&lt;TValue&gt;, ValueChangedEventArgs&lt;TValue&gt;&gt;&gt;.
            /// </returns>
            public static IObservable<EventTuple<ValueContainer<TValue>, ValueChangedEventArgs<TValue>>> GetValueChangedEventObservable<TValue>(this ValueContainer<TValue> source)
            {

                var eventArgSeq = Observable.FromEventPattern<EventHandler<ValueChangedEventArgs<TValue>>, ValueChangedEventArgs<TValue>>(
                        eh => source.ValueChanged += eh,
                        eh => source.ValueChanged -= eh);
                return eventArgSeq.Select(
                            x => EventTuple.Create(source, x.EventArgs)
                        );
                ;
            }

            public static IObservable<EventTuple<IValueContainer, ValueChangedEventArgs>> GetValueChangedNonGenericEventObservable(this IValueContainer source)
            {

                var eventArgSeq = Observable.FromEventPattern<EventHandler<ValueChangedEventArgs>, ValueChangedEventArgs>(
                        eh => source.NonGenericValueChanged += eh,
                        eh => source.NonGenericValueChanged -= eh);
                return eventArgSeq.Select(
                            x => EventTuple.Create(source as IValueContainer, x.EventArgs as ValueChangedEventArgs)
                        );
                ;
            }
            public static IObservable<EventTuple<IValueContainer, ValueChangingEventArgs>> GetValueChangingNonGenericEventObservable(this IValueContainer source)
            {

                var eventArgSeq = Observable.FromEventPattern<EventHandler<ValueChangingEventArgs>, ValueChangingEventArgs>(
                        eh => source.NonGenericValueChanging += eh,
                        eh => source.NonGenericValueChanging -= eh);
                return eventArgSeq.Select(
                            x => EventTuple.Create(source as IValueContainer, x.EventArgs as ValueChangingEventArgs)
                        );
                ;
            }

            /// <summary>Gets the value changing event observable.</summary>
            /// <typeparam name="TValue">The type of the t service.</typeparam>
            /// <param name="source">The source.</param>
            /// <returns>
            /// IObservable&lt;EventTuple&lt;ValueContainer&lt;TValue&gt;, ValueChangedEventArgs&lt;TValue&gt;&gt;&gt;.
            /// </returns>
            public static IObservable<EventTuple<ValueContainer<TValue>, ValueChangingEventArgs<TValue>>> GetValueChangingdEventObservable<TValue>(this ValueContainer<TValue> source)
            {

                var eventArgSeq = Observable.FromEventPattern<EventHandler<ValueChangingEventArgs<TValue>>, ValueChangingEventArgs<TValue>>(
                        eh => source.ValueChanging += eh,
                        eh => source.ValueChanging -= eh);
                return eventArgSeq.Select(
                            x => EventTuple.Create(source, x.EventArgs)
                       );
                ;
            }





            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="TModel"></typeparam>
            /// <param name="source"></param>
            /// <param name="properties"></param>
            /// <returns></returns>
            public static IObservable<EventTuple<IValueContainer, ValueChangedEventArgs>> ListenValueChangedEvents<TModel>(this TModel source,
                   params Expression<Func<TModel, object>>[] properties
               ) where TModel : BindableBase<TModel>
            {

                return source.GetValueContainers(properties)
                    .ToObservable()
                    .SelectMany(x => x.GetValueChangedNonGenericEventObservable());
            }


            /// <summary>
            /// Also Listen the changed with these properties.
            /// </summary>
            /// <typeparam name="TModel">The type of the model.</typeparam>
            /// <param name="sequence">The event sequence.</param>
            /// <param name="secondSource">The second source.</param>
            /// <param name="properties">The properties.</param>
            /// <returns>Merged event Sequence</returns>
            public static IObservable<EventTuple<IValueContainer, ValueChangedEventArgs>> AlsoListenValueChangedWith<TModel>(
                this IObservable<EventTuple<IValueContainer, ValueChangedEventArgs>> sequence,
                TModel secondSource,
                params Expression<Func<TModel, object>>[] properties
                ) where TModel : BindableBase<TModel>
            {

                var another = ListenValueChangedEvents(secondSource);
                return Observable.Merge(sequence, another);

            }





            /// <summary>
            /// Listen the changing with these properties of model instance.
            /// </summary>
            /// <typeparam name="TModel"></typeparam>
            /// <param name="source"></param>
            /// <param name="properties"></param>
            /// <returns></returns>
            public static IObservable<EventTuple<IValueContainer, ValueChangingEventArgs>> ListenValueChangingEvents<TModel>(this TModel source,
                   params Expression<Func<TModel, object>>[] properties
               ) where TModel : BindableBase<TModel>
            {

                return source.GetValueContainers(properties)
                    .ToObservable()
                    .SelectMany(x => x.GetValueChangingNonGenericEventObservable());
            }


            /// <summary>
            /// Also Listen the changing with these properties of another model instance.
            /// </summary>
            /// <typeparam name="TModel">The type of the model.</typeparam>
            /// <param name="sequence">The event sequence.</param>
            /// <param name="secondSource">The second source.</param>
            /// <param name="properties">The properties.</param>
            /// <returns>Merged event Sequence</returns>
            public static IObservable<EventTuple<IValueContainer, ValueChangingEventArgs>> AlsoListenValueChangingWith<TModel>(
                this IObservable<EventTuple<IValueContainer, ValueChangingEventArgs>> sequence,
                TModel secondSource,
                params Expression<Func<TModel, object>>[] properties
                ) where TModel : BindableBase<TModel>
            {

                var another = ListenValueChangingEvents(secondSource);
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
            /// Bind Command to IsUIBusy property.
            /// </summary>
            /// <typeparam name="TCommand">A sub class of ReactiveCommand</typeparam>
            /// <typeparam name="TResource">The resource type of CommandModel</typeparam>
            /// <typeparam name="TViewModel">The View Model type command wanna bind to</typeparam>
            /// <param name="command">Command itself</param>
            /// <param name="model">The View Model  command wanna bind to</param>
            /// <param name="canExecuteWhenBusy">if can execute when ui busy , input true</param>
            /// <returns>command instance itself</returns>
            public static CommandModel<TCommand, TResource> ListenToIsUIBusy<TCommand, TResource, TViewModel>(this CommandModel<TCommand, TResource> command, ViewModelBase<TViewModel> model, bool canExecuteWhenBusy = false, IObservable<bool> and_also_listen_to_this_sequence = null)
                where TViewModel : ViewModelBase<TViewModel>
                where TCommand : ReactiveCommand
            {

                var eventSeq = model.GetValueContainer(x => x.IsUIBusy)
                        .GetValueChangedEventObservable()
                        .Select(e => canExecuteWhenBusy ? canExecuteWhenBusy : (!e.EventArgs.NewValue));


                if (and_also_listen_to_this_sequence != null)
                {
                    eventSeq = eventSeq.CombineLatest(and_also_listen_to_this_sequence, (a, b) => a && b);
                }


                //See Test  CommandListenToUIBusy_Test
                var mainSeq = Observable.Range(0, 1)
                   .Select(x => (x == 0) ? !command.LastCanExecuteValue : command.LastCanExecuteValue)
                   .Concat(eventSeq);

                command.CommandCore.ListenCanExecuteObservable(mainSeq)
                    .DisposeWith(model);

                return command;
            }


        }
    }
}

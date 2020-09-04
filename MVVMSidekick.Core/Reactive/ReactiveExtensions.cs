﻿
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
                          inContext => (vm.ExecuteFunctionTask(taskBody, inContext, cancellationToken), inContext, cancellationToken)
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
                          inContext => (vm.ExecuteTask(taskBody, inContext, cancellationToken), inContext, cancellationToken)
                    );
            }

            public static IObservable<(Task Task, Tin InputContext, CancellationToken CancellationToken)> DoExecuteUIBusyActionTask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task> taskBody)
            {
                return DoExecuteUIBusyActionTask(sequence, vm, (inContext, cancel) => taskBody(inContext));
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



            public static IObservable<(ValueContainer<TValue> ValueContainer, ValueChangedEventArgs<TValue> EventArgs)> GetValueChangedEventObservable<TValue>(this ValueContainer<TValue> source)
            {

                var eventArgSeq = Observable.FromEventPattern<EventHandler<ValueChangedEventArgs<TValue>>, ValueChangedEventArgs<TValue>>(
                        eh => source.ValueChanged += eh,
                        eh => source.ValueChanged -= eh);
                return eventArgSeq.Select(
                            x => (source, x.EventArgs)
                        );
                ;
            }

            public static IObservable<(BindableBase Model, IValueContainer ValueContainer, ValueChangedEventArgs EventArgs)> GetValueChangedNonGenericEventObservable(this IValueContainer source)
            {

                var eventArgSeq = Observable.FromEventPattern<EventHandler<ValueChangedEventArgs>, ValueChangedEventArgs>(
                        eh => source.NonGenericValueChanged += eh,
                        eh => source.NonGenericValueChanged -= eh);
                return eventArgSeq.Select(
                            x => (source.Model as BindableBase, source as IValueContainer, x.EventArgs as ValueChangedEventArgs)
                        );
                ;
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

            public static IObservable<(BindableBase Model, IValueContainer ValueContainer, ValueChangingEventArgs EventArgs)> GetValueChangingNonGenericEventObservable(this IValueContainer source)
            {

                var eventArgSeq = Observable.FromEventPattern<EventHandler<ValueChangingEventArgs>, ValueChangingEventArgs>(
                        eh => source.NonGenericValueChanging += eh,
                        eh => source.NonGenericValueChanging -= eh);
                return eventArgSeq.Select(
                            x => (source.Model as BindableBase, source as IValueContainer, x.EventArgs as ValueChangingEventArgs)
                        );
                ;
            }

            /// <summary>Gets the value changing event observable.</summary>
            /// <typeparam name="TValue">The type of the t service.</typeparam>
            /// <param name="source">The source.</param>
            /// <returns>
            /// IObservable&lt;EventTuple&lt;ValueContainer&lt;TValue&gt;, ValueChangedEventArgs&lt;TValue&gt;&gt;&gt;.
            /// </returns>
            public static IObservable<(BindableBase Model, ValueContainer<TValue> ValueContainer, ValueChangingEventArgs<TValue> EventArgs)> GetValueChangingdEventObservable<TValue>(this ValueContainer<TValue> source)
            {

                var eventArgSeq = Observable.FromEventPattern<EventHandler<ValueChangingEventArgs<TValue>>, ValueChangingEventArgs<TValue>>(
                        eh => source.ValueChanging += eh,
                        eh => source.ValueChanging -= eh);
                return eventArgSeq.Select(
                            x => (source.Model as BindableBase, source, x.EventArgs)
                       );
                ;
            }



            public static IObservable<(TModel Model, IValueContainer ValueContainer,  ValueChangedEventArgs EventArgs)> ListenValueChangedEvents<TModel>(this TModel source,
                   params Expression<Func<TModel, object>>[] properties
               ) where TModel : BindableBase<TModel>
            {

                var containers = source.GetValueContainers(properties);

                return containers
                    .ToObservable()
                    .SelectMany(x => x.GetValueChangedNonGenericEventObservable())
                    .Select(x=> (x.Model as TModel,x.ValueContainer,x.EventArgs));


            }


            /// <summary>
            /// Also Listen the changed with these properties.
            /// </summary>
            /// <typeparam name="TModel">The type of the model.</typeparam>
            /// <param name="sequence">The event sequence.</param>
            /// <param name="secondSource">The second source.</param>
            /// <param name="properties">The properties.</param>
            /// <returns>Merged event Sequence</returns>
            public static IObservable<(TModel Model, IValueContainer ValueContainer,  ValueChangedEventArgs EventArgs)> AlsoListenValueChangedWith<TModel>(
                this IObservable<(TModel Model, IValueContainer ValueContainer, ValueChangedEventArgs EventArgs)> sequence,
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
            public static IObservable<(TModel Model, IValueContainer ValueContainer, ValueChangingEventArgs EventArgs)> ListenValueChangingEvents<TModel>(this TModel source,
                   params Expression<Func<TModel, object>>[] properties
               ) where TModel : BindableBase<TModel>
            {

                return source.GetValueContainers(properties)
                    .ToObservable()
                    .SelectMany(x => x.GetValueChangingNonGenericEventObservable()).Select(x=>(x.Model as TModel,x.ValueContainer,x.EventArgs));
            }


            /// <summary>
            /// Also Listen the changing with these properties of another model instance.
            /// </summary>
            /// <typeparam name="TModel">The type of the model.</typeparam>
            /// <param name="sequence">The event sequence.</param>
            /// <param name="secondSource">The second source.</param>
            /// <param name="properties">The properties.</param>
            /// <returns>Merged event Sequence</returns>
            public static IObservable<(TModel Model, IValueContainer ValueContainer, ValueChangingEventArgs EventArgs)> AlsoListenValueChangingWith<TModel>(
                this IObservable<(TModel Model, IValueContainer ValueContainer, ValueChangingEventArgs EventArgs)> sequence,
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
            public static CommandModel<TCommand, TResource> WireExecutableToViewModelIsUIBusy<TCommand, TResource, TViewModel>(this CommandModel<TCommand, TResource> command, ViewModelBase<TViewModel> model, bool canExecuteWhenBusy = false, bool canExecuteWhenUIFree = true)
                where TViewModel : ViewModelBase<TViewModel>
                where TCommand : ReactiveCommand
            {

                var eventSeq = model.GetValueContainer(x => x.IsUIBusy)
                        .GetValueChangedEventObservable()
                        .Select(e =>
                                e.EventArgs.NewValue ? canExecuteWhenBusy : canExecuteWhenUIFree);

                var mainSeq = new[] { !command.CanExecuteValue, command.CanExecuteValue }
                .ToObservable().Concat(eventSeq);

                command.CommandCore.ListenCanExecuteObservable(mainSeq)
                    .DisposeWith(model);

                return command;
            }


        }
    }
}

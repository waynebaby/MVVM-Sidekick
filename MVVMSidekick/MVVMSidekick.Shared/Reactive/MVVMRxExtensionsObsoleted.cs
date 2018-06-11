
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
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using MVVMSidekick.ViewModels;
using System.Reactive.Linq;
using System.Reactive;
using System.Reactive.Threading.Tasks;
using MVVMSidekick.EventRouting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
#if NETFX_CORE


#elif WPF



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
        public static class MVVMRxExtensionsObsoleted
        {
            #region Obsoleted

            /// <summary>Gets the new value observable.</summary>
            /// <typeparam name="TValue">The type of the value.</typeparam>
            /// <param name="source">The source.</param>
            /// <returns>
            /// IObservable&lt;EventTuple&lt;ValueContainer&lt;TValue&gt;, TValue&gt;&gt;.
            /// </returns>
            [Obsolete("Use GetValueChangedEventObservable instead for book changed events")]
            public static IObservable<EventTuple<ValueContainer<TValue>, TValue>> GetNewValueObservable<TValue>(this ValueContainer<TValue> source)
            {

                return Observable.FromEventPattern<EventHandler<ValueChangedEventArgs<TValue>>, ValueChangedEventArgs<TValue>>(
                        eh => source.ValueChanged += eh,
                        eh => source.ValueChanged -= eh)
                        .Select(
                            x => EventTuple.Create(source, x.EventArgs.NewValue)

                        );

            }



            [Obsolete(@"Try use DoExecuteUIBusyActionTask() instead. If you are using with EventCommand, please delete DoNotifyDefaultEventRouter() in following lines.")]
            public static IObservable<(Task Task, Tin InputContext, CancellationToken CancellationToken)> DoExecuteUIBusyTask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task> taskBody, CancellationToken cancellationToken = default(CancellationToken))
            {
                return sequence.Select
                    (
                          inContext => (vm.ExecuteTask(taskBody, inContext, cancellationToken, false), inContext, cancellationToken)
                    );
            }
            [Obsolete(@"Try use DoExecuteUIBusyActionTask() instead. If you are using with EventCommand, please delete DoNotifyDefaultEventRouter() in following lines.")]
            public static IObservable<(Task Task, Tin InputContext, CancellationToken CancellationToken)> DoExecuteUIBusyTask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task> taskBody)
            {
                return DoExecuteUIBusyTask(sequence, vm, (inContext, cancel) => taskBody(inContext));
            }


            [Obsolete(@"Try use DoExecuteUIActionTask() instead. If you are using with EventCommand, please delete DoNotifyDefaultEventRouter() in following lines.")]
            public static IObservable<(Task Task, Tin InputContext, CancellationToken CancellationToken)> DoExecuteUITask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, CancellationToken, Task> taskBody, CancellationToken cancellationToken = default(CancellationToken))
            {
                return sequence.Select
                    (
                          inContext => (vm.ExecuteTask(taskBody, inContext, cancellationToken, false), inContext, cancellationToken)
                    );
            }
            [Obsolete(@"Try use DoExecuteUIActionTask() instead. If you are using with EventCommand, please delete DoNotifyDefaultEventRouter() in following lines.")]
            public static IObservable<(Task Task, Tin InputContext, CancellationToken CancellationToken)> DoExecuteUITask<Tin>(this IObservable<Tin> sequence, IViewModel vm, Func<Tin, Task> taskBody)
            {
                return DoExecuteUIBusyTask(sequence, vm, (inContext, cancel) => taskBody(inContext));
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
            [Obsolete("try use GetCollectionChangedEventObservable()")]
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

            /// <summary>Gets the value changed event observable.</summary>
            /// <typeparam name="TValue">The type of the t service.</typeparam>
            /// <param name="source">The source.</param>
            /// <returns>
            /// IObservable&lt;EventTuple&lt;ValueContainer&lt;TValue&gt;, ValueChangedEventArgs&lt;TValue&gt;&gt;&gt;.
            /// </returns>

            [Obsolete("Use GetValueChangedEventObservable instead for cache changed events")]
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
            /// Gets the named observable.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <returns>IObservable&lt;System.String&gt;.</returns>
            [Obsolete("Use GetValueChangedEventObservable instead for cache changed events")]
            public static IObservable<string> GetNamedObservable(this INotifyChanged source)
            {

                var eventArgSeq = Observable.FromEventPattern<EventHandler<ValueChangedEventArgs>, ValueChangedEventArgs>(
                        eh => source.NonGenericValueChanged += eh,
                        eh => source.NonGenericValueChanged -= eh);
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
            [Obsolete("")]
            public static IObservable<EventTuple<object, string>> ListenChanged<TModel>(this TModel source,
                    params Expression<Func<TModel, object>>[] properties
                ) where TModel : BindableBase<TModel>
            {

                return source.GetValueContainers(properties)
                    .ToObservable()
                    .SelectMany(x => x.GetValueChangedNonGenericEventObservable().Select(y =>
                        new EventTuple<object, string>()
                        {
                            Source = source,
                            EventArgs = y.EventArgs.PropertyName
                        }));
            }

            [Obsolete("")]
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
            #endregion

        }
    }
}

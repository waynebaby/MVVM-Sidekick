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
using MVVMSidekick.EventRouting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Collections;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using System.Collections.Concurrent;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Xaml.Controls.Primitives;
using Windows.System.Threading;

#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Services;

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
    namespace Utilities
    {
        public class Disposable : IDisposable
        {


            public Disposable(Action disposeAction)
            {
                _disposeAction = disposeAction;
            }

            Action _disposeAction;



            public void Dispose()
            {
                var a = Interlocked.Exchange(ref _disposeAction, null);
                if (a != null)
                {
                    a();
                }
            }
        }


        /// <summary>
        /// Unify Task(4.5) and TaskEx (SL5) method in this helper
        /// </summary>
        public static class TaskExHelper
        {



            public static async Task Yield()
            {
#if SILVERLIGHT_5||WINDOWS_PHONE_7
            await TaskEx.Yield();
#elif NET40
            await Task.Factory.StartNew(() => { });
#else
                await Task.Yield();
#endif

            }

            public static async Task<T> FromResult<T>(T result)
            {
#if SILVERLIGHT_5||WINDOWS_PHONE_7
                return await TaskEx.FromResult(result);
#elif NET40
            return await Task.Factory.StartNew(() => result);
#else
                return await Task.FromResult(result);
#endif

            }

            public static async Task Delay(int ms)
            {

#if SILVERLIGHT_5||WINDOWS_PHONE_7
            await TaskEx.Delay(ms);
        
#elif NET40
            var task = new Task(() => { });
            using (var tm = new System.Threading.Timer(o => task.Start()))
            {
                tm.Change(ms, -1);
                await task;
            }
#else

                await Task.Delay(ms);
#endif

            }

        }
        /// <summary>
        /// Unify Type(4.5 SL & WP) and TypeInfo (Windows Runtime) class in this helper
        /// </summary>
        public static class TypeInfoHelper
        {
#if NETFX_CORE
            public static TypeInfo GetTypeOrTypeInfo(this Type type)
            {
                return type.GetTypeInfo();

            }
#else
            public static Type GetTypeOrTypeInfo(this Type type)
            {
                return type;

            }
#endif

        }

        public static class ReflectionCache
        {
            static class ReflectInfoCache<T> where T : MemberInfo
            {
                static ConcurrentDictionary<Type, Dictionary<string, T>> cache
                    = new ConcurrentDictionary<Type, Dictionary<string, T>>();

                static public Dictionary<string, T> GetCache(Type type, Func<Type, T[]> dataGetter)
                {
                    return cache.GetOrAdd(type, s => dataGetter(s).ToDictionary(x => x.Name, x => x));
                }
            }


            public static Dictionary<string, MethodInfo> GetMethodsFromCache(this Type type)
            {
#if NETFX_CORE
                return ReflectInfoCache<MethodInfo>.GetCache(type, x => x.GetRuntimeMethods().ToArray());
#else
                return ReflectInfoCache<MethodInfo>.GetCache(type, x => x.GetMethods());
#endif
            }

            public static Dictionary<string, EventInfo> GetEventsFromCache(this Type type)
            {
#if NETFX_CORE
                return ReflectInfoCache<EventInfo>.GetCache(type, x => x.GetRuntimeEvents().ToArray());
#else
                return ReflectInfoCache<EventInfo>.GetCache(type, x => x.GetEvents());
#endif
            }



        }

        public delegate void EventHandlerInvoker(object sender, object eventArgs, string eventName, Type eventHandlerType);
        public static class EventHandlerHelper
        {
            private static Delegate CreateHandler(
                Expression<EventHandlerInvoker> bind,
                string eventName,
                Type delegateType,
                Type[] eventParametersTypes
            )
            {
                var pars =
                        eventParametersTypes
                            .Select(
                                et => System.Linq.Expressions.Expression.Parameter(et))
                        .ToArray();
                var en = System.Linq.Expressions.Expression.Constant (eventName, typeof(string));
                var eht = System.Linq.Expressions.Expression.Constant(delegateType, typeof(Type));
               

                var expInvoke = System.Linq.Expressions.Expression.Invoke(bind,pars[0],pars [1], en,eht);
                var lambda = System.Linq.Expressions.Expression.Lambda(delegateType, expInvoke,pars );
                var compiled = lambda.Compile();
                return compiled;
            }

            public static IDisposable BindEvent(this object sender, string eventName, EventHandlerInvoker executeAction)
            {



                var t = sender.GetType();

                while (t != null)
                {

                    var es = t.GetEventsFromCache();
                    EventInfo ei = es.MatchOrDefault(eventName);


                    if (ei != null)
                    {

                        var handlerType = ei.EventHandlerType;
                        var eventMethod = handlerType.GetMethodsFromCache().MatchOrDefault("Invoke");
                        if (eventMethod != null)
                        {
                            var pts = eventMethod.GetParameters().Select(p => p.ParameterType)
                                .ToArray();
                            var newHandler = CreateHandler(
                                               (o, e, en, ehtype) => executeAction(o, e, en, ehtype),
                                               eventName,
                                               handlerType,
                                               pts
                                               );

#if NETFX_CORE ||WINDOWS_PHONE_8
                            var etmodule = sender.GetType().GetTypeOrTypeInfo().Module;
                            try
                            {
                                return DoNetEventBind(sender, ei, newHandler);
                            }
                            catch (InvalidOperationException ex)
                            {
                                var newMI = WinRTEventBindMethodInfo.MakeGenericMethod(newHandler.GetType());

                                var rval = newMI.Invoke(null, new object[] { sender, ei, newHandler }) as IDisposable;


                                return rval;
                            }


#else

                             return DoNetEventBind(sender, ei, newHandler);
#endif


                        }

                        return null;
                    }

                    t = t.GetTypeOrTypeInfo().BaseType;
                }

                return null;
            }


#if NETFX_CORE||WINDOWS_PHONE_8
            static MethodInfo WinRTEventBindMethodInfo = typeof(EventHandlerHelper).GetTypeInfo().GetDeclaredMethod("WinRTEventBind");
            private static IDisposable WinRTEventBind<THandler>(object sender, EventInfo ei, object handler)
            {
                System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken tk = default(System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken);

                Action<System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken> remove
                    = et =>
                    {
                        ei.RemoveMethod.Invoke(sender, new object[] { et });
                    };

                System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal.AddEventHandler<THandler>(
                    ev =>
                    {
                        tk = (System.Runtime.InteropServices.WindowsRuntime.EventRegistrationToken)ei.AddMethod.Invoke(sender, new object[] { ev });
                        return tk;
                    },
                    remove,
                    (THandler)handler);

                return new Disposable(() =>
                    {
                        System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal.RemoveEventHandler<THandler>(
                           remove,
                        (THandler)handler);


                    }
                );

            }
#endif
            private static IDisposable DoNetEventBind(object sender, EventInfo ei, Delegate newHandler)
            {
                ei.AddEventHandler(sender, newHandler);
                return new Disposable(() => ei.RemoveEventHandler(sender, newHandler));
            }

        }

        public static class ColllectionHelper
        {


            public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
            {

                return new ObservableCollection<T>(items);
            }


            public static TValue MatchOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
            {
                TValue val = default(TValue);
                dic.TryGetValue(key, out val);
                return val;
            }
        }

#if WINDOWS_PHONE_7
    public class Lazy<T>
    {
        public Lazy(Func<T> factory)
        { 
            _factory =()=>
            {
                lock(this)
                {
                    if(_value.Equals(default(T)))
                    {
                        _value=_factory();
                 
                    
                    }
                    return _value;
                }
            
            };
        
        }

        T _value;
        Func<T> _factory;
        public T Value 
        { 
            get
            {
               return _value.Equals (default(T))?_factory():_value; 
            }
        
            set
            {
                _value=value;
            } 
        }
    }



#endif

        public class ExpressionHelper
        {
            public static string GetPropertyName<TSubClassType, TProperty>(Expression<Func<TSubClassType, TProperty>> expression)
            {
                MemberExpression body = expression.Body as MemberExpression;
                var propName = (body.Member is PropertyInfo) ? body.Member.Name : string.Empty;
                return propName;
            }



            public static string GetPropertyName<TSubClassType>(Expression<Func<TSubClassType, object>> expression)
            {
                MemberExpression body = expression.Body as MemberExpression;
                if (body != null)
                {
                    var propName = (body.Member is PropertyInfo) ? body.Member.Name : string.Empty;
                    return propName;
                }

                var exp2 = expression.Body as System.Linq.Expressions.UnaryExpression;
                if (exp2 != null)
                {
                    body = exp2.Operand as MemberExpression;
                    var propName = (body.Member is PropertyInfo) ? body.Member.Name : string.Empty;
                    return propName;
                }
                else
                {

                    throw new Exception();
                }

            }





        }

#if SILVERLIGHT_5||WINDOWS_PHONE_8||WINDOWS_PHONE_7
    public class ConcurrentDictionary<TK, TV> : Dictionary<TK, TV>
    {
        public TV GetOrAdd(TK key, Func<TK, TV> factory)
        {
            TV rval = default(TV);

            if (!base.TryGetValue(key, out rval))
            {
                lock (this)
                {
                    if (!base.TryGetValue(key, out rval))
                    {
                        rval = factory(key);
                        base.Add(key, rval);
                    }


                }
            }

            return rval;
        }
    }
#endif

        /// <summary> 
        /// Provides a task scheduler that ensures a maximum concurrency level while 
        /// running on top of the ThreadPool. 
        /// </summary> 
        public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
        {
            /// <summary>Whether the current thread is processing work items.</summary>
            [ThreadStatic]
            private static bool _currentThreadIsProcessingItems;
            /// <summary>The list of tasks to be executed.</summary> 
            private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks) 
            /// <summary>The maximum concurrency level allowed by this scheduler.</summary> 
            private readonly int _maxDegreeOfParallelism;
            /// <summary>Whether the scheduler is currently processing work items.</summary> 
            private int _delegatesQueuedOrRunning = 0; // protected by lock(_tasks) 

            /// <summary> 
            /// Initializes an instance of the LimitedConcurrencyLevelTaskScheduler class with the 
            /// specified degree of parallelism. 
            /// </summary> 
            /// <param name="maxDegreeOfParallelism">The maximum degree of parallelism provided by this scheduler.</param>
            public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
            {
                if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
                _maxDegreeOfParallelism = maxDegreeOfParallelism;
            }

            /// <summary>Queues a task to the scheduler.</summary> 
            /// <param name="task">The task to be queued.</param>
            protected sealed override void QueueTask(Task task)
            {
                // Add the task to the list of tasks to be processed.  If there aren't enough 
                // delegates currently queued or running to process tasks, schedule another. 
                lock (_tasks)
                {
                    _tasks.AddLast(task);
                    if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                    {
                        ++_delegatesQueuedOrRunning;
                        NotifyThreadPoolOfPendingWork();
                    }
                }
            }

            /// <summary> 
            /// Informs the ThreadPool that there's work to be executed for this scheduler. 
            /// </summary> 
            private void NotifyThreadPoolOfPendingWork()
            {

#if NETFX_CORE
                ThreadPool.RunAsync((_1) =>
#else

                ThreadPool.QueueUserWorkItem(_ =>
#endif


                    {
                        // Note that the current thread is now processing work items. 
                        // This is necessary to enable inlining of tasks into this thread.
                        _currentThreadIsProcessingItems = true;
                        try
                        {
                            // Process all available items in the queue. 
                            while (true)
                            {
                                Task item;
                                lock (_tasks)
                                {
                                    // When there are no more items to be processed, 
                                    // note that we're done processing, and get out. 
                                    if (_tasks.Count == 0)
                                    {
                                        --_delegatesQueuedOrRunning;
                                        break;
                                    }

                                    // Get the next item from the queue
                                    item = _tasks.First.Value;
                                    _tasks.RemoveFirst();
                                }

                                // Execute the task we pulled out of the queue 
                                base.TryExecuteTask(item);
                            }
                        }
                        // We're done processing items on the current thread 
                        finally { _currentThreadIsProcessingItems = false; }

#if NETFX_CORE
                    });
#else

                }, null);
#endif

            }

            /// <summary>Attempts to execute the specified task on the current thread.</summary> 
            /// <param name="task">The task to be executed.</param>
            /// <param name="taskWasPreviouslyQueued"></param>
            /// <returns>Whether the task could be executed on the current thread.</returns> 
            protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                // If this thread isn't already processing a task, we don't support inlining 
                if (!_currentThreadIsProcessingItems) return false;

                // If the task was previously queued, remove it from the queue 
                if (taskWasPreviouslyQueued) TryDequeue(task);

                // Try to run the task. 
                return base.TryExecuteTask(task);
            }

            /// <summary>Attempts to remove a previously scheduled task from the scheduler.</summary> 
            /// <param name="task">The task to be removed.</param>
            /// <returns>Whether the task could be found and removed.</returns> 
            protected sealed override bool TryDequeue(Task task)
            {
                lock (_tasks) return _tasks.Remove(task);
            }

            /// <summary>Gets the maximum concurrency level supported by this scheduler.</summary> 
            public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

            /// <summary>Gets an enumerable of the tasks currently scheduled on this scheduler.</summary> 
            /// <returns>An enumerable of the tasks currently scheduled.</returns> 
            protected sealed override IEnumerable<Task> GetScheduledTasks()
            {
                bool lockTaken = false;
                try
                {
                    Monitor.TryEnter(_tasks, ref lockTaken);
                    if (lockTaken) return _tasks.ToArray();
                    else throw new NotSupportedException();
                }
                finally
                {
                    if (lockTaken) Monitor.Exit(_tasks);
                }
            }
        }

    }

}


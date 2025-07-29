// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Utilities.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Security;
using System.Linq;
#if WINDOWS_UWP
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
using System.Reactive.Disposables;


#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Reactive.Disposables;

#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Reactive;
#endif



namespace MVVMSidekick
{

    namespace Utilities
    {
        /// <summary>
        /// <para>提供有限并发级别的任务调度器，确保在ThreadPool之上运行时保持最大并发级别</para>
        /// <para>Provides a task scheduler that ensures a maximum concurrency level while running on top of the ThreadPool</para>
        /// </summary>
        public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
        {
            /// <summary>
            /// <para>当前线程是否正在处理工作项</para>
            /// <para>Whether the current thread is processing work items</para>
            /// </summary>
            [ThreadStatic]
            private static bool _currentThreadIsProcessingItems;
            
            /// <summary>
            /// <para>要执行的任务列表</para>
            /// <para>The list of tasks to be executed</para>
            /// </summary>
            private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks) 
            
            /// <summary>
            /// <para>此调度器允许的最大并发级别</para>
            /// <para>The maximum concurrency level allowed by this scheduler</para>
            /// </summary>
            private readonly int _maxDegreeOfParallelism;
            
            /// <summary>
            /// <para>调度器当前是否正在处理工作项</para>
            /// <para>Whether the scheduler is currently processing work items</para>
            /// </summary>
            private int _delegatesQueuedOrRunning = 0; // protected by lock(_tasks) 

            /// <summary>
            /// <para>使用指定的并行度初始化LimitedConcurrencyLevelTaskScheduler类的实例</para>
            /// <para>Initializes an instance of the LimitedConcurrencyLevelTaskScheduler class with the specified degree of parallelism</para>
            /// </summary>
            /// <param name="maxDegreeOfParallelism">
            /// <para>此调度器提供的最大并行度</para>
            /// <para>The maximum degree of parallelism provided by this scheduler</para>
            /// </param>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// <para>当maxDegreeOfParallelism小于1时抛出</para>
            /// <para>Thrown when maxDegreeOfParallelism is less than 1</para>
            /// </exception>
            public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
            {
                if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
                _maxDegreeOfParallelism = maxDegreeOfParallelism;
            }

            /// <summary>
            /// <para>将任务排队到调度器</para>
            /// <para>Queues a task to the scheduler</para>
            /// </summary>
            /// <param name="task">
            /// <para>要排队的任务</para>
            /// <para>The task to be queued</para>
            /// </param>
            [SecurityCritical]
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
            /// 通知 ThreadPool 有待执行的工作
            /// Informs the ThreadPool that there's work to be executed for this scheduler.
            /// </summary>
            private async void NotifyThreadPoolOfPendingWork()
            {


                await Task.Yield();
                ThreadPool.QueueUserWorkItem(
                    _ =>
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
                        finally
                        {
                            _currentThreadIsProcessingItems = false;
                        }
                    },
                    null);


            }

            /// <summary>
            /// 尝试在当前线程上执行指定的任务
            /// Attempts to execute the specified task on the current thread.
            /// </summary>
            /// <param name="task">要执行的任务 / The task to be executed.</param>
            /// <param name="taskWasPreviouslyQueued">如果设置为 <c>true</c>，则任务之前已排队 / if set to <c>true</c> [task was previously queued].</param>
            /// <returns>是否可以在当前线程上执行任务 / Whether the task could be executed on the current thread.</returns>
            [SecurityCritical]
            protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                // If this thread isn't already processing a task, we don't support inlining 
                if (!_currentThreadIsProcessingItems) return false;

                // If the task was previously queued, remove it from the queue 
                if (taskWasPreviouslyQueued) TryDequeue(task);

                // Try to run the task. 
                return base.TryExecuteTask(task);
            }

            /// <summary>
            /// 尝试从调度器中移除先前调度的任务
            /// Attempts to remove a previously scheduled task from the scheduler.
            /// </summary>
            /// <param name="task">要移除的任务 / The task to be removed.</param>
            /// <returns>是否能找到并移除任务 / Whether the task could be found and removed.</returns>
            [SecurityCritical]
            protected sealed override bool TryDequeue(Task task)
            {
                lock (_tasks) return _tasks.Remove(task);
            }

            /// <summary>
            /// 获取此调度器支持的最大并发级别
            /// Gets the maximum concurrency level supported by this scheduler.
            /// </summary>
            /// <value>最大并发级别 / The maximum concurrency level.</value>
            public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

            /// <summary>
            /// 获取当前在此调度器上调度的任务的可枚举对象
            /// Gets an enumerable of the tasks currently scheduled on this scheduler.
            /// </summary>
            /// <returns>当前调度的任务的可枚举对象 / An enumerable of the tasks currently scheduled.</returns>
            /// <exception cref="System.NotSupportedException"></exception>
            [SecurityCritical]
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


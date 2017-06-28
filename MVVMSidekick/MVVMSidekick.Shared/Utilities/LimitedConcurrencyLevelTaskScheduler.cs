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
#if NETFX_CORE
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
        /// Provides a task scheduler that ensures a maximum concurrency level while
        /// running on top of the ThreadPool.
        /// </summary>
        public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
		{
			/// <summary>
			/// Whether the current thread is processing work items.
			/// </summary>
			[ThreadStatic]
			private static bool _currentThreadIsProcessingItems;
			/// <summary>
			/// The list of tasks to be executed.
			/// </summary>
			private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks) 
			/// <summary>
			/// The maximum concurrency level allowed by this scheduler.
			/// </summary>
			private readonly int _maxDegreeOfParallelism;
			/// <summary>
			/// Whether the scheduler is currently processing work items.
			/// </summary>
			private int _delegatesQueuedOrRunning = 0; // protected by lock(_tasks) 

			/// <summary>
			/// Initializes an instance of the LimitedConcurrencyLevelTaskScheduler class with the
			/// specified degree of parallelism.
			/// </summary>
			/// <param name="maxDegreeOfParallelism">The maximum degree of parallelism provided by this scheduler.</param>
			/// <exception cref="System.ArgumentOutOfRangeException">maxDegreeOfParallelism</exception>
			public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
			{
				if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
				_maxDegreeOfParallelism = maxDegreeOfParallelism;
			}

			/// <summary>
			/// Queues a task to the scheduler.
			/// </summary>
			/// <param name="task">The task to be queued.</param>
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
			/// Informs the ThreadPool that there's work to be executed for this scheduler.
			/// </summary>
			private async void NotifyThreadPoolOfPendingWork()
			{

#if NETFX_CORE
				await ThreadPool.RunAsync((_1) =>
#else
				await TaskExHelper.Yield();
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

			/// <summary>
			/// Attempts to execute the specified task on the current thread.
			/// </summary>
			/// <param name="task">The task to be executed.</param>
			/// <param name="taskWasPreviouslyQueued">if set to <c>true</c> [task was previously queued].</param>
			/// <returns>Whether the task could be executed on the current thread.</returns>
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
			/// Attempts to remove a previously scheduled task from the scheduler.
			/// </summary>
			/// <param name="task">The task to be removed.</param>
			/// <returns>Whether the task could be found and removed.</returns>
			[SecurityCritical]
			protected sealed override bool TryDequeue(Task task)
			{
				lock (_tasks) return _tasks.Remove(task);
			}

			/// <summary>
			/// Gets the maximum concurrency level supported by this scheduler.
			/// </summary>
			/// <value>The maximum concurrency level.</value>
			public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

			/// <summary>
			/// Gets an enumerable of the tasks currently scheduled on this scheduler.
			/// </summary>
			/// <returns>An enumerable of the tasks currently scheduled.</returns>
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


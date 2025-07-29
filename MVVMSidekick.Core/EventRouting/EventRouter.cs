// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="EventRouting.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Reactive.Subjects;
using MVVMSidekick.Common;
using System.Runtime.CompilerServices;
using System.Reflection;
using MVVMSidekick.Utilities;




namespace MVVMSidekick
{

    namespace EventRouting
    {
        /// <summary>
        /// 全局事件路由器，用于处理应用程序范围内的事件
        /// Global event router for handling application-wide events
        /// </summary>
        public class EventRouter : InstanceCountableBase
		{
			/// <summary>
			/// 初始化EventRouter类的新实例
			/// Initializes a new instance of the <see cref="EventRouter" /> class.
			/// </summary>
			public EventRouter()
			{
			}
			
			/// <summary>
			/// 初始化EventRouter类的静态成员
			/// Initializes static members of the <see cref="EventRouter" /> class.
			/// </summary>
			static EventRouter()
			{
				Instance = new EventRouter();
			}

			/// <summary>
			/// 获取或设置全局事件路由器实例
			/// Gets or sets the global event router instance
			/// </summary>
			/// <value>全局实例 / The global instance</value>
			public static EventRouter Instance { get; protected set; }

			/// <summary>
			/// 引发错误事件，使用指定的异常信息
			/// Raises an error event with the specified exception
			/// </summary>
			/// <typeparam name="TException">异常类型 / The exception type</typeparam>
			/// <param name="sender">发送者 / The sender</param>
			/// <param name="exception">异常实例 / The exception</param>
			/// <param name="callerMemberOrEventName">调用成员或事件名 / Caller member or event name</param>
			public static void RaiseErrorEvent<TException>(object sender, TException exception, [CallerMemberName] string callerMemberOrEventName = null) where TException : Exception
			{
				EventRouter.Instance.RaiseEvent(sender, exception, callerMemberOrEventName, true, true);
			}

			/// <summary>
			/// 触发事件（泛型版本）
			/// Triggers an event (generic version)
			/// </summary>
			/// <typeparam name="TEventArgs">事件参数类型 / The type of the event arguments</typeparam>
			/// <param name="sender">事件发送者 / The sender</param>
			/// <param name="eventArgs">事件数据 / The event arguments</param>
			/// <param name="callerMemberNameOrEventName">发送事件名 / Caller member name or event name</param>
			/// <param name="isFiringToAllBaseClassChannels">是否触发到所有基类通道 / Whether to fire to all base class channels</param>
			/// <param name="isFiringToAllImplementedInterfaceChannels">是否触发到所有实现接口通道 / Whether to fire to all implemented interface channels</param>
			public virtual void RaiseEvent<TEventArgs>(object sender, TEventArgs eventArgs, string callerMemberNameOrEventName = "", bool isFiringToAllBaseClassChannels = false, bool isFiringToAllImplementedInterfaceChannels = false)
			{
				RaiseEvent(sender, eventArgs, typeof(TEventArgs), callerMemberNameOrEventName, isFiringToAllBaseClassChannels, isFiringToAllImplementedInterfaceChannels);
			}

			/// <summary>
			/// 触发事件（非泛型版本）
			/// Triggers an event (non-generic version)
			/// </summary>
			/// <param name="sender">事件发送者 / The sender</param>
			/// <param name="args">事件数据 / The event arguments</param>
			/// <param name="eventArgsType">事件参数类型 / Type of the event arguments</param>
			/// <param name="callerMemberNameOrEventName">发送事件名 / Caller member name or event name</param>
			/// <param name="isFiringToAllBaseClassChannels">是否触发到所有基类通道 / Whether to fire to all base class channels</param>
			/// <param name="isFiringToAllImplementedInterfaceChannels">是否触发到所有实现接口通道 / Whether to fire to all implemented interface channels</param>
			public virtual void RaiseEvent(object sender, object args, Type eventArgsType, string callerMemberNameOrEventName = "", bool isFiringToAllBaseClassChannels = false, bool isFiringToAllImplementedInterfaceChannels = false)
			{
				var channel = GetEventChannel(eventArgsType);
				channel.RaiseEvent(sender, callerMemberNameOrEventName, args, isFiringToAllBaseClassChannels, isFiringToAllImplementedInterfaceChannels);
			}


			/// <summary>
			/// 获取指定类型的事件通道（泛型版本）
			/// Gets the event channel for the specified type (generic version)
			/// </summary>
			/// <typeparam name="TEventData">事件数据类型 / The type of the event data</typeparam>
			/// <returns>事件通道实例 / The event channel instance</returns>
			public virtual EventChannel<TEventData> GetEventChannel<TEventData>()
			{
				var channel = (EventChannel<TEventData>)GetEventChannel(typeof(TEventData));
				return channel;
			}

			/// <summary>
			/// 事件通道缓存字典，用于存储各种类型的事件通道
			/// Event channels cache dictionary for storing various types of event channels
			/// </summary>
			protected readonly System.Collections.Concurrent.ConcurrentDictionary<Type, IEventChannel> EventChannels
				= new System.Collections.Concurrent.ConcurrentDictionary<Type, IEventChannel>();
				
			/// <summary>
			/// 获取或创建指定类型的事件通道
			/// Gets or creates an event channel for the specified type
			/// </summary>
			/// <param name="argsType">事件数据类型 / The event data type</param>
			/// <returns>事件通道实例 / The event channel instance</returns>
			public IEventChannel GetEventChannel(Type argsType)
			{
				if (argsType == null)
				{
					return null;
				}
				var rval = EventChannels.GetOrAdd(
					argsType,
					t =>
					{
						try
						{
							var t2 = typeof(EventChannel<>).MakeGenericType(t);
							return Activator.CreateInstance(t2, this) as IEventChannel;
						}
						catch (Exception ex)
						{
							EventRouter.RaiseErrorEvent(this, ex);
						}
						return null;
					}
					);



				return rval;
			}


			/// <summary>
			/// 事件通道接口，定义事件通道的基本操作
			/// Event channel interface that defines basic operations for event channels
			/// </summary>
			public interface IEventChannel
			{
				/// <summary>
				/// 获取或设置基类类型通道列表
				/// Gets or sets the base class type channels list
				/// </summary>
				/// <value>基类类型通道实例 / The base class type channel instances</value>
				IList<IEventChannel> BaseClassTypeChannels { get; set; }

				/// <summary>
				/// 获取或设置实现接口类型通道列表
				/// Gets or sets the implemented interface type channels list
				/// </summary>
				/// <value>实现接口类型通道实例 / The implemented interface type channel instances</value>
				IList<IEventChannel> ImplementedInterfaceTypeInstances { get; set; }

				/// <summary>
				/// 触发事件
				/// Raises the event
				/// </summary>
				/// <param name="sender">发送者 / The sender</param>
				/// <param name="eventName">事件名称 / Name of the event</param>
				/// <param name="args">事件参数 / The arguments</param>
				/// <param name="isFiringToAllBaseClassChannels">是否触发到所有基类通道 / Whether to fire to all base class channels</param>
				/// <param name="isFiringToAllImplementedInterfaceChannels">是否触发到所有实现接口通道 / Whether to fire to all implemented interface channels</param>
				void RaiseEvent(object sender, string eventName, object args, bool isFiringToAllBaseClassChannels = false, bool isFiringToAllImplementedInterfaceChannels = false);
			}




			/// <summary>
			/// 事件通道类，处理特定类型的事件数据
			/// Event channel class that handles specific type of event data
			/// </summary>
			/// <typeparam name="TEventData">事件数据类型 / The type of the event data</typeparam>
			public class EventChannel<TEventData> : InstanceCountableBase, IEventChannel, IObservable<RouterEventData<TEventData>>, IDisposable
			{
				/// <summary>
				/// 初始化EventChannel类的新实例
				/// Initializes a new instance of the EventChannel class
				/// </summary>
				public EventChannel() : this(null)
				{
				}

				/// <summary>
				/// 初始化EventChannel类的新实例
				/// Initializes a new instance of the EventChannel class
				/// </summary>
				/// <param name="router">事件路由器实例 / The event router instance</param>
				public EventChannel(EventRouter router)
				{
					var current = this;
					var argsType = typeof(TEventData);
					var basetypes = new List<Type>()
					{
						//argsType
					};


					for (; ; )
					{


						argsType = argsType.GetTypeInfo().BaseType;
						if (argsType != null && argsType?.Name != "RuntimeClass")
						{
							basetypes.Add(argsType);
						}
						else
						{
							break;
						}


						if (router != null)
						{



							BaseClassTypeChannels = basetypes

								.Select(x => router.GetEventChannel(x))
								.Where(x => x != null)
								.ToList();


							ImplementedInterfaceTypeInstances = typeof(TEventData)
								.GetTypeInfo()
								.ImplementedInterfaces
								.Where(x => x.Name[0] == 'I')
								.Select(x =>
										router.GetEventChannel(x))
								.Where(x => x != null)
								.ToList();

						}

					}
				}

				/// <summary>
				/// 核心主题对象，用于管理事件的发布和订阅
				/// The core subject object for managing event publishing and subscription
				/// </summary>
				private Subject<RouterEventData<TEventData>> _core = new Subject<RouterEventData<TEventData>>();

				/// <summary>
				/// 获取或设置基类类型通道列表
				/// Gets or sets the base class type channels list
				/// </summary>
				public IList<IEventChannel> BaseClassTypeChannels
				{
					get;
					set;
				}

				/// <summary>
				/// 获取或设置实现接口类型通道列表
				/// Gets or sets the implemented interface type channels list
				/// </summary>
				public IList<IEventChannel> ImplementedInterfaceTypeInstances
				{
					get;
					set;
				}





				///// <summary>
				///// Raises the event.
				///// </summary>
				///// <param name="sender">The sender.</param>
				///// <param name="eventName">Name of the event.</param>
				///// <param name="args">The arguments.</param>
				//void IEventChannel.RaiseEvent(object sender, string eventName, object args)
				//{
				//	RaiseEvent(sender, eventName, (TEventData)args);
				//}

				///// <summary>
				///// 发起事件
				///// </summary>
				///// <param name="sender">发送者</param>
				///// <param name="eventName">事件名</param>
				///// <param name="args">参数</param>
				//public void RaiseEvent(object sender, string eventName, TEventData args)
				//{



				//}

				//public event EventHandler<DataEventArgs<RouterEventData<TEventArgs>>> Event;



				/// <summary>
				/// 订阅事件通道，返回用于取消订阅的对象
				/// Subscribes to the event channel and returns an object for unsubscribing
				/// </summary>
				/// <param name="observer">观察者对象 / The observer</param>
				/// <returns>用于取消订阅的对象 / The disposable object for unsubscribing</returns>
				public IDisposable Subscribe(IObserver<RouterEventData<TEventData>> observer)
				{
					return _core.Subscribe(observer);
				}

				/// <summary>
				/// 释放标志，用于防止重复释放
				/// The disposal flag to prevent multiple disposal
				/// </summary>
				int _Disposed = 0;
				
				/// <summary>
				/// 析构函数，确保资源得到释放
				/// Finalizer to ensure resources are released
				/// </summary>
				~EventChannel()
				{
					Dispose(false);
				}
				/// <summary>
				/// 释放当前实例使用的所有资源
				/// Disposes all resources used by the current instance
				/// </summary>
				public void Dispose()
				{
					Dispose(true);
					GC.SuppressFinalize(this);
				}

				/// <summary>
				/// 释放非托管资源，可选择释放托管资源
				/// Releases unmanaged and optionally managed resources
				/// </summary>
				/// <param name="disposing">如果为true则释放托管和非托管资源；如果为false则仅释放非托管资源 / true to release both managed and unmanaged resources; false to release only unmanaged resources</param>
				virtual protected void Dispose(bool disposing)
				{
					var v = Interlocked.Exchange(ref _Disposed, 1);
					if (v == 0)
					{
						var cbak = Interlocked.Exchange(ref _core, null);

						if (cbak != null)
						{
							try
							{
								cbak.Dispose();
							}
							catch (Exception)
							{


							}
						}

						if (disposing)
						{

						}
					}
				}

				/// <summary>
				/// 触发事件，并可选择向基类和接口通道传播
				/// Raises an event and optionally propagates to base class and interface channels
				/// </summary>
				/// <param name="sender">发送者 / The sender</param>
				/// <param name="eventName">事件名称 / The event name</param>
				/// <param name="args">事件参数 / The event arguments</param>
				/// <param name="isFiringToAllBaseClassChannels">是否触发到所有基类通道 / Whether to fire to all base class channels</param>
				/// <param name="isFiringToAllImplementedInterfaceChannels">是否触发到所有实现接口通道 / Whether to fire to all implemented interface channels</param>
				public void RaiseEvent(object sender, string eventName, object args, bool isFiringToAllBaseClassChannels = false, bool isFiringToAllImplementedInterfaceChannels = false)
				{
					var a = args;
					_core.OnNext(new RouterEventData<TEventData>(sender, eventName, (TEventData)args));

					if (isFiringToAllBaseClassChannels && BaseClassTypeChannels != null)
					{
						foreach (var item in BaseClassTypeChannels)
						{
							item.RaiseEvent(sender, eventName, args, false, false);
						}

					}

					if (isFiringToAllImplementedInterfaceChannels && ImplementedInterfaceTypeInstances != null)
					{

						foreach (var item in ImplementedInterfaceTypeInstances)
						{
							item.RaiseEvent(sender, eventName, args, false, false);
						}
					}

				}
			}


		}



	}


}

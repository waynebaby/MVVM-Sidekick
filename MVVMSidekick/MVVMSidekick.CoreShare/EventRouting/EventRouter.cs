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

#if NETFX_CORE
using System.Collections.Concurrent;

#elif WPF
using System.Collections.Concurrent;


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

    namespace EventRouting
    {
        /// <summary>
        /// 全局事件根
        /// </summary>
        public class EventRouter : InstanceCountableBase
		{

			/// <summary>
			/// Initializes a new instance of the <see cref="EventRouter" /> class.
			/// </summary>
			public EventRouter()
			{
			}
			/// <summary>
			/// Initializes static members of the <see cref="EventRouter" /> class.
			/// </summary>
			static EventRouter()
			{
				Instance = new EventRouter();
			}

			/// <summary>
			/// Gets or sets the instance.
			/// </summary>
			/// <value>The instance.</value>
			public static EventRouter Instance { get; protected set; }



			public static void RaiseErrorEvent<TException>(object sender, TException exception, [CallerMemberName] string callerMemberOrEventName = null) where TException : Exception
			{
				EventRouter.Instance.RaiseEvent(sender, exception, callerMemberOrEventName, true, true);
			}





			/// <summary>
			/// 触发事件
			/// </summary>
			/// <typeparam name="TEventArgs">The type of the t event arguments.</typeparam>
			/// <param name="sender">事件发送者</param>
			/// <param name="eventArgs">事件数据</param>
			/// <param name="callerMemberNameOrEventName">发送事件名</param>
			public virtual void RaiseEvent<TEventArgs>(object sender, TEventArgs eventArgs, string callerMemberNameOrEventName = "", bool isFiringToAllBaseClassChannels = false, bool isFiringToAllImplementedInterfaceChannels = false)
			//#if !NETFX_CORE
			//// where TEventArgs : EventArgs
			//#endif
			{
				RaiseEvent(sender, eventArgs, typeof(TEventArgs), callerMemberNameOrEventName, isFiringToAllBaseClassChannels, isFiringToAllImplementedInterfaceChannels);
			}

			/// <summary>
			/// 触发事件
			/// </summary>
			/// <param name="sender">事件发送者</param>
			/// <param name="args">事件数据</param>
			/// <param name="eventArgsType">Type of the event arguments.</param>
			/// <param name="callerMemberNameOrEventName">发送事件名</param>
			public virtual void RaiseEvent(object sender, object args, Type eventArgsType, string callerMemberNameOrEventName = "", bool isFiringToAllBaseClassChannels = false, bool isFiringToAllImplementedInterfaceChannels = false)
			{
				var channel = GetEventChannel(eventArgsType);
				channel.RaiseEvent(sender, callerMemberNameOrEventName, args, isFiringToAllBaseClassChannels, isFiringToAllImplementedInterfaceChannels);
			}


			/// <summary>
			/// 取得独立事件类
			/// </summary>
			/// <typeparam name="TEventData">The type of the t event arguments.</typeparam>
			/// <returns>事件独立类</returns>
			public virtual EventChannel<TEventData> GetEventChannel<TEventData>()
			{
				var channel = (EventChannel<TEventData>)GetEventChannel(typeof(TEventData));

				return channel;

			}




			/// <summary>
			/// 事件来源的代理对象实例
			/// </summary>

			protected readonly System.Collections.Concurrent.ConcurrentDictionary<Type, IEventChannel> EventChannels
				= new System.Collections.Concurrent.ConcurrentDictionary<Type, IEventChannel>();
			/// <summary>
			/// 创建事件代理对象
			/// </summary>
			/// <param name="argsType">事件数据类型</param>
			/// <returns>代理对象实例</returns>
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
			/// 事件对象接口
			/// </summary>
			public interface IEventChannel
			{
				/// <summary>
				/// Gets or sets the base arguments type instance.
				/// </summary>
				/// <value>The base classes type instances.</value>
				IList<IEventChannel> BaseClassTypeChannels { get; set; }


				/// <summary>
				/// Gets or sets the base arguments type instance.
				/// </summary>
				/// <value>The base classes type instances.</value>
				IList<IEventChannel> ImplementedInterfaceTypeInstances { get; set; }


				/// <summary>
				/// Raises the event.
				/// </summary>
				/// <param name="sender">The sender.</param>
				/// <param name="eventName">Name of the event.</param>
				/// <param name="args">The arguments.</param>
				void RaiseEvent(object sender, string eventName, object args, bool isFiringToAllBaseClassChannels = false, bool isFiringToAllImplementedInterfaceChannels = false);
			}




			/// <summary>
			/// 事件对象
			/// </summary>
			/// <typeparam name="TEventData">The type of the t event arguments.</typeparam>
			public class EventChannel<TEventData> : InstanceCountableBase, IEventChannel, IObservable<RouterEventData<TEventData>>, IDisposable
			{


				public EventChannel() : this(null)
				{
				}

				public EventChannel(EventRouter router)
				{
					var current = this;
					var argsType = typeof(TEventData);
					var basetypes = new List<Type>()
					{
						//argsType
					};

#if NETFX_CORE

					for (; ; )
					{


						argsType = argsType.GetTypeInfo().BaseType;
						if (argsType != typeof(object) && argsType != null)
						{
							if (argsType.Name != "RuntimeClass")
							{

								basetypes.Add(argsType);
							}
							else
							{
								break;

							}
																									  
						}
						else
						{
							break;
						}
					}
#else
					for (; ;)
					{


						argsType = argsType.BaseType;
						if (argsType != typeof(object) && argsType != null)
						{
							basetypes.Add(argsType);
						}
						else
						{
							break;
						}
					}
#endif
					if (typeof(TEventData) != typeof(Object))
					{
						basetypes.Add(typeof(object));

					}
					if (router != null)
					{



						BaseClassTypeChannels = basetypes

							.Select(x => router.GetEventChannel(x))
							.Where(x => x != null)
							.ToList();


#if NETFX_CORE
						ImplementedInterfaceTypeInstances = typeof(TEventData)
							.GetTypeOrTypeInfo()
							.ImplementedInterfaces
							.Where (x=>x.Name[0]=='I')
							.Select(x =>
									router.GetEventChannel(x))
							.Where(x => x != null)
							.ToList();
#else
						ImplementedInterfaceTypeInstances = typeof(TEventData)
							.GetInterfaces()
							.Select(x =>
									router.GetEventChannel(x))
							.Where(x => x != null)
							.ToList();
#endif
					}


				}

				/// <summary>
				/// The _core
				/// </summary>
				private Subject<RouterEventData<TEventData>> _core = new Subject<RouterEventData<TEventData>>();




				public IList<IEventChannel> BaseClassTypeChannels
				{
					get;
					set;
				}

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
				/// Subscribes the specified observer.
				/// </summary>
				/// <param name="observer">The observer.</param>
				/// <returns>IDisposable.</returns>
				public IDisposable Subscribe(IObserver<RouterEventData<TEventData>> observer)
				{
					return _core.Subscribe(observer);

				}


				/// <summary>
				/// The _ disposed
				/// </summary>
				int _Disposed = 0;
				/// <summary>
				/// Finalizes an instance of the <see cref="EventChannel{TEventArgs}" /> class.
				/// </summary>
				~EventChannel()
				{
					Dispose(false);
				}
				/// <summary>
				/// Disposes this instance.
				/// </summary>
				public void Dispose()
				{
					Dispose(true);
					GC.SuppressFinalize(this);
				}

				/// <summary>
				/// Releases unmanaged and - optionally - managed resources.
				/// </summary>
				/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
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



		//#if !NETFX_CORE

		//		/// <summary>
		//		/// Class DataEventArgs.
		//		/// </summary>
		//		/// <typeparam name="TData">The type of the t data.</typeparam>
		//		public class DataEventArgs<TData> : EventArgs
		//		{
		//			/// <summary>
		//			/// Initializes a new instance of the <see cref="DataEventArgs{TData}" /> class.
		//			/// </summary>
		//			/// <param name="data">The data.</param>
		//			public DataEventArgs(TData data)
		//			{

		//				Data = data;
		//			}

		//			/// <summary>
		//			/// Gets or sets the data.
		//			/// </summary>
		//			/// <value>The data.</value>
		//			public TData Data { get; protected set; }

		//		}
		//#endif
	}


}

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
using System.Reflection;
using System.Threading;
using MVVMSidekick.ViewModels;
using System.Reactive.Subjects;
using MVVMSidekick.Utilities;
using MVVMSidekick.Common;
using System.Runtime.CompilerServices;

#if NETFX_CORE
using System.Collections.Concurrent;

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

	namespace EventRouting
	{

		/// <summary>
		/// 全局事件根
		/// </summary>
		public class EventRouter : InstanceCounableBase
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

			protected readonly ConcurrentDictionary<Type, IEventChannel> EventChannels
				= new ConcurrentDictionary<Type, IEventChannel>();
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
			public class EventChannel<TEventData> : InstanceCounableBase, IEventChannel, IObservable<RouterEventData<TEventData>>, IDisposable
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
		/// <summary>
		/// 导航事件数据
		/// </summary>
		public class NavigateCommandEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="NavigateCommandEventArgs" /> class.
			/// </summary>
			public NavigateCommandEventArgs()
			{
				ParameterDictionary = new Dictionary<string, object>();
			}
			/// <summary>
			/// Initializes a new instance of the <see cref="NavigateCommandEventArgs" /> class.
			/// </summary>
			/// <param name="dic">The dic.</param>
			public NavigateCommandEventArgs(IDictionary<string, object> dic)
				: this()
			{
				foreach (var item in dic)
				{

					(ParameterDictionary as IDictionary<string, object>)[item.Key] = item.Value;
				}

			}
			/// <summary>
			/// Gets or sets the parameter dictionary.
			/// </summary>
			/// <value>The parameter dictionary.</value>
			public Dictionary<string, object> ParameterDictionary { get; set; }

			/// <summary>
			/// Gets or sets the type of the source view.
			/// </summary>
			/// <value>The type of the source view.</value>
			public Type SourceViewType { get; set; }

			/// <summary>
			/// Gets or sets the type of the target view.
			/// </summary>
			/// <value>The type of the target view.</value>
			public Type TargetViewType { get; set; }

			/// <summary>
			/// Gets or sets the view model.
			/// </summary>
			/// <value>The view model.</value>
			public IViewModel ViewModel { get; set; }

			/// <summary>
			/// Gets or sets the target frame.
			/// </summary>
			/// <value>The target frame.</value>
			public Object TargetFrame { get; set; }
		}

		///// <summary>
		///// 保存状态事件数据
		///// </summary>
		//public class SaveStateEventArgs : EventArgs
		//{
		//	/// <summary>
		//	/// Gets or sets the view key identifier.
		//	/// </summary>
		//	/// <value>The view key identifier.</value>
		//	public string ViewKeyId { get; set; }
		//	/// <summary>
		//	/// Gets or sets the state.
		//	/// </summary>
		//	/// <value>The state.</value>
		//	public Dictionary<string, object> State { get; set; }
		//}

		/// <summary>
		/// 事件路由的扩展方法集合
		/// </summary>
		public static class EventRouterHelper
		{
			/// <summary>
			/// 触发事件
			/// </summary>
			/// <typeparam name="TEventArgs">The type of the t event arguments.</typeparam>
			/// <param name="source">事件来源</param>
			/// <param name="eventArgs">事件数据</param>
			/// <param name="callerMemberName">事件名</param>
			public static void RaiseEvent<TEventArgs>(this BindableBase source, TEventArgs eventArgs, string callerMemberName = "")
#if !NETFX_CORE
			// where TEventArgs : EventArgs
#endif

			{
				EventRouter.Instance.RaiseEvent(source, eventArgs, callerMemberName);
			}

		}




		/// <summary>
		/// 事件信息
		/// </summary>
		/// <typeparam name="TEventArgs">事件数据类型</typeparam>
		public struct RouterEventData<TEventArgs>
#if !NETFX_CORE
		//: EventArgs
		// where TEventArgs : EventArgs
#endif

		{
			/// <summary>
			/// Initializes a new instance of the <see cref="RouterEventData{TEventArgs}" /> struct.
			/// </summary>
			/// <param name="sender">The sender.</param>
			/// <param name="eventName">Name of the event.</param>
			/// <param name="eventArgs">The instance containing the event data.</param>
			public RouterEventData(object sender, string eventName, TEventArgs eventArgs)
			{

				_Sender = sender;
				_EventName = eventName;
				_EventArgs = eventArgs;
			}

			/// <summary>
			/// The _ sender
			/// </summary>
			private Object _Sender;
			/// <summary>
			/// 事件发送者
			/// </summary>
			/// <value>The sender.</value>
			public Object Sender
			{
				get { return _Sender; }

			}

			/// <summary>
			/// The _ event name
			/// </summary>
			private string _EventName;

			/// <summary>
			/// 事件名
			/// </summary>
			/// <value>The name of the event.</value>
			public string EventName
			{
				get { return _EventName; }
			}

			/// <summary>
			/// The _ event arguments
			/// </summary>
			private TEventArgs _EventArgs;
			/// <summary>
			/// 事件数据
			/// </summary>
			/// <value>The event arguments.</value>
			public TEventArgs EventData
			{
				get { return _EventArgs; }
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

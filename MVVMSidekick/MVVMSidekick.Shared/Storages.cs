//// ***********************************************************************
//// Assembly         : MVVMSidekick_Wp8
//// Author           : waywa
//// Created          : 05-17-2014
////
//// Last Modified By : waywa
//// Last Modified On : 01-04-2015
//// ***********************************************************************
//// <copyright file="Storages.cs" company="">
////     Copyright ©  2012
//// </copyright>
//// <summary></summary>
//// ***********************************************************************
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.ComponentModel;
//using System.Linq.Expressions;
//using System.Runtime.Serialization;
//using System.Reflection;
//using System.Threading.Tasks;
//using System.Threading;
//using System.Windows.Input;
//using MVVMSidekick.ViewModels;
//using MVVMSidekick.Commands;
//using System.Runtime.CompilerServices;
//using MVVMSidekick.Reactive;
//using System.Reactive.Linq;
//using System.Reactive.Subjects;
//using System.Reactive;
//using MVVMSidekick.EventRouting;
//using MVVMSidekick.Utilities;
//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.IO;
//using System.Collections;

//#if NETFX_CORE
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Data;
//using Windows.UI.Xaml.Controls;
//using System.Collections.Concurrent;
//using Windows.UI.Xaml.Navigation;

//using Windows.UI.Xaml.Controls.Primitives;
//using Windows.Storage;

//#elif WPF
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Collections.Concurrent;
//using System.Windows.Navigation;

//using MVVMSidekick.Views;
//using System.Windows.Controls.Primitives;


//#elif SILVERLIGHT_5||SILVERLIGHT_4
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Navigation;
//using System.Windows.Controls.Primitives;
//using System.IO.IsolatedStorage;
//#elif WINDOWS_PHONE_8||WINDOWS_PHONE_7
//using System.Windows;
//using System.Windows.Controls;
//using Microsoft.Phone.Controls;
//using System.Windows.Data;
//using System.Windows.Navigation;
//using System.Windows.Controls.Primitives;
//using System.IO.IsolatedStorage;
//#endif


///// <summary>
///// The MVVMSidekick namespace.
///// </summary>
//namespace MVVMSidekick
//{

//	/// <summary>
//	/// The Storages namespace.
//	/// </summary>
//	namespace Storages
//	{
//		/// <summary>
//		/// <para>Simple storage interface, for persistence.</para>
//		/// <para>简单的持久化存储类型接口</para>
//		/// </summary>
//		/// <typeparam name="T"><para>The Type needs to be save/load</para>
//		/// <para>需要存取的类型</para></typeparam>
//		public interface IStorage<T>
//		{
//			/// <summary>
//			/// <para>Ignore current changes, load from storage</para>
//			/// <para>忽略当前值的变化，从持久化存储中读取</para>
//			/// </summary>
//			/// <returns>Async Task</returns>
//			System.Threading.Tasks.Task<T> RefreshAsync();
//			/// <summary>
//			/// <para>Save current changes to storage</para>
//			/// <para>把当前值的变化写入持久化存储中</para>
//			/// </summary>
//			/// <param name="value">The value.</param>
//			/// <returns>Async Task</returns>
//			System.Threading.Tasks.Task SaveAsync(T value);

//			/// <summary>
//			/// <para>Current value</para>
//			/// <para>当前值</para>
//			/// </summary>
//			/// <value>The value.</value>
//			T Value { get; }
//		}




//		/// <summary>
//		/// <para>Simple storage interface, for persistence.</para>
//		/// <para>简单的持久化存储类型接口</para>
//		/// </summary>
//		/// <typeparam name="TToken"><para>The Token/token Type needs to be save/load</para>
//		/// <para>需要存取的凭据类型</para></typeparam>
//		/// <typeparam name="TValue"><para>The Value Type needs to be save/load</para>
//		/// <para>需要存取的类型</para></typeparam>
//		public interface IStorageHub<TToken, TValue>
//		{

//			/// <summary>
//			/// Loads the asynchronous.
//			/// </summary>
//			/// <param name="token">The token.</param>
//			/// <param name="forceRefresh">if set to <c>true</c> [force refresh].</param>
//			/// <returns>System.Threading.Tasks.Task&lt;TValue&gt;.</returns>
//			System.Threading.Tasks.Task<TValue> LoadAsync(TToken token, bool forceRefresh);

//			/// <summary>
//			/// Saves the asynchronous.
//			/// </summary>
//			/// <param name="token">The token.</param>
//			/// <param name="value">The value.</param>
//			/// <returns>System.Threading.Tasks.Task.</returns>
//			System.Threading.Tasks.Task SaveAsync(TToken token, TValue value);

//		}

//		/// <summary>
//		/// Class StorageHub.
//		/// </summary>
//		/// <typeparam name="TToken">The type of the t token.</typeparam>
//		/// <typeparam name="TValue">The type of the t value.</typeparam>
//		public class StorageHub<TToken, TValue> : IStorageHub<TToken, TValue>
//		{
//			/// <summary>
//			/// Initializes a new instance of the <see cref="StorageHub{TToken, TValue}"/> class.
//			/// </summary>
//			/// <param name="storageFactory">The storage factory.</param>
//			/// <param name="storageTokensSelector">The storage tokens selector.</param>
//			public StorageHub(Func<TToken, IStorage<TValue>> storageFactory, Func<Task<TToken[]>> storageTokensSelector = null)
//			{
//				_storageFactory = storageFactory;
//				_storageTokensSelector = storageTokensSelector;
//			}


//			/// <summary>
//			/// Gets the or creat storage.
//			/// </summary>
//			/// <param name="token">The token.</param>
//			/// <returns>IStorage&lt;TValue&gt;.</returns>
//			IStorage<TValue> GetOrCreatStorage(TToken token)
//			{

//				return _dic.GetOrAdd(token, _storageFactory);
//			}


//			/// <summary>
//			/// The _storage factory
//			/// </summary>
//			Func<TToken, IStorage<TValue>> _storageFactory;
//			/// <summary>
//			/// The _storage tokens selector
//			/// </summary>
//			Func<Task<TToken[]>> _storageTokensSelector;

//			/// <summary>
//			/// Gets the exists tokens.
//			/// </summary>
//			/// <returns>Task&lt;TToken[]&gt;.</returns>
//			/// <exception cref="System.NotImplementedException">Current storageTokensSelector is not set in constructor. </exception>
//			public async Task<TToken[]> GetExistsTokens()
//			{
//				if (_storageTokensSelector != null)
//				{
//					return await _storageTokensSelector();
//				}
//				else
//				{
//					throw new NotImplementedException("Current storageTokensSelector is not set in constructor. ");

//				}

//			}


//			/// <summary>
//			/// The _dic
//			/// </summary>
//			ConcurrentDictionary<TToken, IStorage<TValue>> _dic = new ConcurrentDictionary<TToken, IStorage<TValue>>();

//			/// <summary>
//			/// load as an asynchronous operation.
//			/// </summary>
//			/// <param name="token">The token.</param>
//			/// <param name="forceRefresh">if set to <c>true</c> [force refresh].</param>
//			/// <returns>Task&lt;TValue&gt;.</returns>
//			public async Task<TValue> LoadAsync(TToken token, bool forceRefresh)
//			{
//				var storage = GetOrCreatStorage(token);
//				if (forceRefresh)
//				{
//					return await storage.RefreshAsync();
//				}
//				else
//				{
//					return storage.Value;
//				}

//			}

//			/// <summary>
//			/// save as an asynchronous operation.
//			/// </summary>
//			/// <param name="token">The token.</param>
//			/// <param name="value">The value.</param>
//			/// <returns>Task.</returns>
//			public async Task SaveAsync(TToken token, TValue value)
//			{
//				var storage = GetOrCreatStorage(token);


//				await storage.SaveAsync(value);

//			}

//			#region Json

//#if NETFX_CORE
//			public static StorageHub<TToken, TValue> CreateJsonDatacontractFileStorageHub(
//				Func<TToken, string> fileNameFactory,
//				StorageFolder folder = null,
//				Func<Task<TToken[]>> storageTokensSelector = null)
//			{



//				var hub = new JsonDataContractStreamStorageHub<TToken, TValue>(
//				   async (tp, tk) =>
//				   {
//					   folder = folder ?? Windows.Storage.ApplicationData.Current.LocalFolder;
//					   switch (tp)
//					   {
//						   case StreamOpenType.Read:
//							   {
//								   var file = await folder.CreateFileAsync(fileNameFactory(tk), CreationCollisionOption.OpenIfExists);

//								   return await file.OpenStreamForReadAsync();
//							   }

//						   case StreamOpenType.Write:
//							   {
//								   var file = await folder.CreateFileAsync(fileNameFactory(tk), CreationCollisionOption.ReplaceExisting);

//								   return await file.OpenStreamForWriteAsync();
//							   }

//						   default:
//							   return null;

//					   }

//				   },
//				storageTokensSelector
//				);
//				return hub;

//			}

//#elif WPF
//			/// <summary>
//			/// Create Jason storage
//			/// </summary>
//			/// <param name="fileNameFactory">how to get file name from token</param>
//			/// <param name="folder">folder this storage is using</param>
//			/// <param name="storageTokensSelector">how to </param>
//			/// <returns></returns>
//			public static StorageHub<TToken, TValue> CreateJsonDatacontractFileStorageHub(
//				Func<TToken, string> fileNameFactory = null,
//				string folder = null,
//				Func<Task<TToken[]>> storageTokensSelector = null)
//			{

//				fileNameFactory = fileNameFactory ??
//				new Func<TToken, string>(t => t.ToString());

//				var hub = new JsonDataContractStreamStorageHub<TToken, TValue>(
//				   async (tp, tk) =>
//				   {
//					   folder = folder ?? Environment.CurrentDirectory;
//					   var filepath = Path.Combine(folder, fileNameFactory(tk));


//					   switch (tp)
//					   {
//						   case StreamOpenType.Read:

//							   return await TaskExHelper.FromResult(new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read));

//						   case StreamOpenType.Write:
//							   return await TaskExHelper.FromResult(new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));


//						   default:
//							   return null;

//					   }

//				   },
//				   storageTokensSelector
//				);
//				return hub;

//			}
//#elif WINDOWS_PHONE_8|| WINDOWS_PHONE_7
//			/// <summary>
//			/// Creates the json datacontract isolated storage hub.
//			/// </summary>
//			/// <param name="fileNameFactory">The file name factory.</param>
//			/// <param name="folder">The folder.</param>
//			/// <param name="storageTokensSelector">The storage tokens selector.</param>
//			/// <returns>StorageHub&lt;TToken, TValue&gt;.</returns>
//			  public static StorageHub<TToken, TValue> CreateJsonDatacontractIsolatedStorageHub(
//				Func<TToken, string> fileNameFactory,
//				IsolatedStorageFile folder = null,
//				Func<Task<TToken[]>> storageTokensSelector = null)
//		{


 
//			var hub = new JsonDataContractStreamStorageHub<TToken, TValue>(
//			   async (tp, tk) =>
//			   {
               
//				   folder = folder ?? IsolatedStorageFile.GetUserStoreForApplication();

        
                  
//				   var filepath=fileNameFactory(tk);
//				   switch (tp)
//				   {
//					   case StreamOpenType.Read:

//						   return folder.OpenFile(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

//					   case StreamOpenType.Write:
//						   return folder.OpenFile(filepath, FileMode.Create, FileAccess.Write, FileShare.None);


//					   default:
//						   return null;

//				   }

//			   },
//				storageTokensSelector
//			);
//			return hub;

//		}

//#elif SILVERLIGHT_5
//			public static StorageHub<TToken, TValue> CreateJsonDatacontractFileStorageHub(
//				Func<TToken, string> fileNameFactory,
//				string folder = null,
//				Func<Task<TToken[]>> storageTokensSelector = null)
//			{



//				var hub = new JsonDataContractStreamStorageHub<TToken, TValue>(
//				   async (tp, tk) =>
//				   {
//					   folder = folder ?? Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
//					   var filepath = Path.Combine(folder, fileNameFactory(tk));

//					   switch (tp)
//					   {
//						   case StreamOpenType.Read:

//							   return new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

//						   case StreamOpenType.Write:
//							   return new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);


//						   default:
//							   return null;

//					   }

//				   },
//					storageTokensSelector 
//				);
//				return hub;

//			}
    
//		public static StorageHub<TToken, TValue> CreateJsonDatacontractIsolatedStorageHub(
//				Func<TToken, string> fileNameFactory,
//				IsolatedStorageFile folder = null,
//				Func<Task<TToken[]>> storageTokensSelector = null)
//		{


 
//			var hub = new JsonDataContractStreamStorageHub<TToken, TValue>(
//			   async (tp, tk) =>
//			   {
//				   await TaskEx.Yield();
//				   folder = folder ?? IsolatedStorageFile.GetUserStoreForApplication();

        
                  
//				   var filepath=fileNameFactory(tk);
//				   switch (tp)
//				   {
//					   case StreamOpenType.Read:

//						   return folder.OpenFile(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

//					   case StreamOpenType.Write:
//						   return folder.OpenFile(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);


//					   default:
//						   return null;

//				   }

//			   },
//				storageTokensSelector
//			);
//			return hub;

//		}

//#endif
//			#endregion


//			#region Xml

//#if NETFX_CORE
//			public static StorageHub<TToken, TValue> CreateXmlDatacontractFileStorageHub(
//				Func<TToken, string> fileNameFactory,
//				StorageFolder folder = null,
//				Func<Task<TToken[]>> storageTokensSelector = null)
//			{



//				var hub = new XmlDataContractStreamStorageHub<TToken, TValue>(
//				   async (tp, tk) =>
//				   {
//					   folder = folder ?? Windows.Storage.ApplicationData.Current.LocalFolder;
//					   switch (tp)
//					   {
//						   case StreamOpenType.Read:
//							   {
//								   var file = await folder.CreateFileAsync(fileNameFactory(tk), CreationCollisionOption.OpenIfExists);

//								   return await file.OpenStreamForReadAsync();
//							   }

//						   case StreamOpenType.Write:
//							   {
//								   var file = await folder.CreateFileAsync(fileNameFactory(tk), CreationCollisionOption.ReplaceExisting);

//								   return await file.OpenStreamForWriteAsync();
//							   }

//						   default:
//							   return null;

//					   }

//				   },
//				storageTokensSelector
//				);
//				return hub;

//			}

//#elif WPF
//			public static StorageHub<TToken, TValue> CreateXmlDatacontractFileStorageHub(
//				Func<TToken, string> fileNameFactory,
//				string folder = null,
//				Func<Task<TToken[]>> storageTokensSelector = null)
//			{



//				var hub = new XmlDataContractStreamStorageHub<TToken, TValue>(
//				   async (tp, tk) =>
//				   {
//					   folder = folder ?? Environment.CurrentDirectory;
//					   var filepath = Path.Combine(folder, fileNameFactory(tk));


//					   switch (tp)
//					   {
//						   case StreamOpenType.Read:

//							   return await TaskExHelper.FromResult(new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read));

//						   case StreamOpenType.Write:
//							   return await TaskExHelper.FromResult(new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));


//						   default:
//							   return null;

//					   }

//				   },
//				   storageTokensSelector
//				);
//				return hub;

//			}
//#elif WINDOWS_PHONE_8|| WINDOWS_PHONE_7
//			  /// <summary>
//			  /// Creates the XML datacontract isolated storage hub.
//			  /// </summary>
//			  /// <param name="fileNameFactory">The file name factory.</param>
//			  /// <param name="folder">The folder.</param>
//			  /// <param name="storageTokensSelector">The storage tokens selector.</param>
//			  /// <returns>StorageHub&lt;TToken, TValue&gt;.</returns>
//			  public static StorageHub<TToken, TValue> CreateXmlDatacontractIsolatedStorageHub(
//				Func<TToken, string> fileNameFactory,
//				IsolatedStorageFile folder = null,
//				Func<Task<TToken[]>> storageTokensSelector = null)
//		{


 
//			var hub = new XmlDataContractStreamStorageHub<TToken, TValue>(
//			   async (tp, tk) =>
//			   {
               
//				   folder = folder ?? IsolatedStorageFile.GetUserStoreForApplication();

        
                  
//				   var filepath=fileNameFactory(tk);
//				   switch (tp)
//				   {
//					   case StreamOpenType.Read:

//						   return folder.OpenFile(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

//					   case StreamOpenType.Write:

//						   return folder.OpenFile(filepath, FileMode.Create, FileAccess.Write, FileShare.None);


//					   default:
//						   return null;

//				   }

//			   },
//				storageTokensSelector
//			);
//			return hub;

//		}

//#elif SILVERLIGHT_5
//			public static StorageHub<TToken, TValue> CreateXmlDatacontractFileStorageHub(
//				Func<TToken, string> fileNameFactory,
//				string folder = null,
//				Func<Task<TToken[]>> storageTokensSelector = null)
//			{



//				var hub = new XmlDataContractStreamStorageHub<TToken, TValue>(
//				   async (tp, tk) =>
//				   {
//					   folder = folder ?? Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
//					   var filepath = Path.Combine(folder, fileNameFactory(tk));

//					   switch (tp)
//					   {
//						   case StreamOpenType.Read:

//							   return new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

//						   case StreamOpenType.Write:
//							   return new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);


//						   default:
//							   return null;

//					   }

//				   },
//					storageTokensSelector 
//				);
//				return hub;

//			}
    
//		public static StorageHub<TToken, TValue> CreateXmlDatacontractIsolatedStorageHub(
//				Func<TToken, string> fileNameFactory,
//				IsolatedStorageFile folder = null,
//				Func<Task<TToken[]>> storageTokensSelector = null)
//		{


 
//			var hub = new XmlDataContractStreamStorageHub<TToken, TValue>(
//			   async (tp, tk) =>
//			   {
//				   await TaskEx.Yield();
//				   folder = folder ?? IsolatedStorageFile.GetUserStoreForApplication();

        
                  
//				   var filepath=fileNameFactory(tk);
//				   switch (tp)
//				   {
//					   case StreamOpenType.Read:

//						   return folder.OpenFile(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

//					   case StreamOpenType.Write:
//						   return folder.OpenFile(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);


//					   default:
//						   return null;

//				   }

//			   },
//				storageTokensSelector
//			);
//			return hub;

//		}

//#endif

//			#endregion

//		}


//		/// <summary>
//		/// Enum StreamOpenType
//		/// </summary>
//		public enum StreamOpenType
//		{
//			/// <summary>
//			/// The read
//			/// </summary>
//			Read,
//			/// <summary>
//			/// The write
//			/// </summary>
//			Write
//		}

//		/// <summary>
//		/// Class JsonDataContractStreamStorageHub.
//		/// </summary>
//		/// <typeparam name="TToken">The type of the t token.</typeparam>
//		/// <typeparam name="TValue">The type of the t value.</typeparam>
//		public class JsonDataContractStreamStorageHub<TToken, TValue> : StorageHub<TToken, TValue>
//		{
//			/// <summary>
//			/// Initializes a new instance of the <see cref="JsonDataContractStreamStorageHub{TToken, TValue}"/> class.
//			/// </summary>
//			/// <param name="streamOpener">The stream opener.</param>
//			/// <param name="storageTokensSelector">The storage tokens selector.</param>
//			public JsonDataContractStreamStorageHub(Func<StreamOpenType, TToken, Task<Stream>> streamOpener, Func<Task<TToken[]>> storageTokensSelector = null)
//				: base
//					(tk => new JsonDataContractStreamStorage<TValue>(async tp => await streamOpener(tp, tk)), storageTokensSelector)
//			{


//			}
//		}


//		/// <summary>
//		/// Class JsonDataContractStreamStorage.
//		/// </summary>
//		/// <typeparam name="TValue">The type of the t value.</typeparam>
//		public class JsonDataContractStreamStorage<TValue> : IStorage<TValue>
//		{
//#if NET45
//			ConcurrentExclusiveSchedulerPair _sch = new ConcurrentExclusiveSchedulerPair();
//#else
//			/// <summary>
//			/// The _SCH
//			/// </summary>
//			TaskScheduler _sch = new LimitedConcurrencyLevelTaskScheduler(1);
//#endif
//			/// <summary>
//			/// Initializes a new instance of the <see cref="JsonDataContractStreamStorage{TValue}"/> class.
//			/// </summary>
//			/// <param name="streamOpener">The stream opener.</param>
//			/// <param name="knownTypes">The known types.</param>
//			public JsonDataContractStreamStorage(Func<StreamOpenType, Task<Stream>> streamOpener, params Type[] knownTypes)
//			{
//				_streamOpener = streamOpener;
//				_knownTypes = knownTypes;
//			}

//			/// <summary>
//			/// The _stream opener
//			/// </summary>
//			Func<StreamOpenType, Task<Stream>> _streamOpener;

//			/// <summary>
//			/// The _known types
//			/// </summary>
//			Type[] _knownTypes;

//			/// <summary>
//			/// Gets or sets the known types.
//			/// </summary>
//			/// <value>The known types.</value>
//			public Type[] KnownTypes
//			{
//				get { return _knownTypes; }
//				set { _knownTypes = value; }
//			}

//			/// <summary>
//			/// refresh as an asynchronous operation.
//			/// </summary>
//			/// <returns>Task&lt;TValue&gt;.</returns>
//			public async Task<TValue> RefreshAsync()
//			{
//				var kts = _knownTypes;
//				return await await
//					   Task.Factory.StartNew(
//						   new Func<Task<TValue>>(
//						   async () =>
//						   {
//							   var ms = new MemoryStream();
//							   var ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(TValue), kts);
//							   using (var strm = await _streamOpener(StreamOpenType.Read))
//							   {
//								   await strm.CopyToAsync(ms);

//							   }
//							   if (ms.Length == 0)
//							   {
//								   return default(TValue);
//							   }
//							   ms.Position = 0;

//							   var obj = (TValue)ser.ReadObject(ms);
//							   Value = obj;
//							   return obj;

//						   }),
//							CancellationToken.None,
//							TaskCreationOptions.None,
//#if NET45
// _sch.ConcurrentScheduler
//#else
// _sch
//#endif

//);



//			}

//			/// <summary>
//			/// save as an asynchronous operation.
//			/// </summary>
//			/// <param name="value">The value.</param>
//			/// <returns>Task.</returns>
//			public async Task SaveAsync(TValue value)
//			{
//				var kts = _knownTypes;
//				await await Task.Factory.StartNew(
//						async () =>
//						{
//							var ms = new MemoryStream();
//							var ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(TValue), kts);
//							Value = value;
//							ser.WriteObject(ms, value);
//							ms.Position = 0;
//							using (var strm = await _streamOpener(StreamOpenType.Write))
//							{
//								await ms.CopyToAsync(strm);
//								await strm.FlushAsync();

//							}
//						},
//						CancellationToken.None,
//						TaskCreationOptions.None,
//#if NET45
// _sch.ExclusiveScheduler
//#else
//					_sch
//#endif


//);

//			}


//			/// <summary>
//			/// Gets the value.
//			/// </summary>
//			/// <value>The value.</value>
//			public TValue Value
//			{
//				get;
//				private set;
//			}
//		}
//		/// <summary>
//		/// Class XmlDataContractStreamStorageHub.
//		/// </summary>
//		/// <typeparam name="TToken">The type of the t token.</typeparam>
//		/// <typeparam name="TValue">The type of the t value.</typeparam>
//		public class XmlDataContractStreamStorageHub<TToken, TValue> : StorageHub<TToken, TValue>
//		{
//			/// <summary>
//			/// Initializes a new instance of the <see cref="XmlDataContractStreamStorageHub{TToken, TValue}"/> class.
//			/// </summary>
//			/// <param name="streamOpener">The stream opener.</param>
//			/// <param name="storageTokensSelector">The storage tokens selector.</param>
//			public XmlDataContractStreamStorageHub(Func<StreamOpenType, TToken, Task<Stream>> streamOpener, Func<Task<TToken[]>> storageTokensSelector = null)
//				: base
//					(tk => new XmlDataContractStreamStorage<TValue>(async tp => await streamOpener(tp, tk)), storageTokensSelector)
//			{


//			}
//		}


//		/// <summary>
//		/// Class XmlDataContractStreamStorage.
//		/// </summary>
//		/// <typeparam name="TValue">The type of the t value.</typeparam>
//		public class XmlDataContractStreamStorage<TValue> : IStorage<TValue>
//		{
//#if NET45
//			ConcurrentExclusiveSchedulerPair _sch = new ConcurrentExclusiveSchedulerPair();
//#else
//			/// <summary>
//			/// The _SCH
//			/// </summary>
//			TaskScheduler _sch = new LimitedConcurrencyLevelTaskScheduler(1);
//#endif
//			/// <summary>
//			/// Initializes a new instance of the <see cref="XmlDataContractStreamStorage{TValue}"/> class.
//			/// </summary>
//			/// <param name="streamOpener">The stream opener.</param>
//			/// <param name="knownTypes">The known types.</param>
//			public XmlDataContractStreamStorage(Func<StreamOpenType, Task<Stream>> streamOpener, params Type[] knownTypes)
//			{
//				_streamOpener = streamOpener;
//				_knownTypes = knownTypes;
//			}

//			/// <summary>
//			/// The _stream opener
//			/// </summary>
//			Func<StreamOpenType, Task<Stream>> _streamOpener;

//			/// <summary>
//			/// The _known types
//			/// </summary>
//			Type[] _knownTypes;

//			/// <summary>
//			/// Gets or sets the known types.
//			/// </summary>
//			/// <value>The known types.</value>
//			public Type[] KnownTypes
//			{
//				get { return _knownTypes; }
//				set { _knownTypes = value; }
//			}

//			/// <summary>
//			/// refresh as an asynchronous operation.
//			/// </summary>
//			/// <returns>Task&lt;TValue&gt;.</returns>
//			public async Task<TValue> RefreshAsync()
//			{
//				var kts = _knownTypes;
//				return await await
//					   Task.Factory.StartNew(
//						   async () =>
//						   {
//							   var ms = new MemoryStream();
//							   var ser = new System.Runtime.Serialization.DataContractSerializer(typeof(TValue), kts);
//							   using (var strm = await _streamOpener(StreamOpenType.Read))
//							   {
//								   await strm.CopyToAsync(ms);

//							   }
//							   if (ms.Length == 0)
//							   {
//								   return default(TValue);
//							   }
//							   ms.Position = 0;

//							   var obj = (TValue)ser.ReadObject(ms);
//							   Value = obj;
//							   return obj;

//						   },
//							CancellationToken.None,
//							TaskCreationOptions.AttachedToParent,
//#if NET45
// _sch.ConcurrentScheduler
//#else
// _sch
//#endif

//);



//			}

//			/// <summary>
//			/// save as an asynchronous operation.
//			/// </summary>
//			/// <param name="value">The value.</param>
//			/// <returns>Task.</returns>
//			public async Task SaveAsync(TValue value)
//			{
//				var kts = _knownTypes;
//				await await Task.Factory.StartNew(
//						async () =>
//						{
//							var ms = new MemoryStream();
//							var ser = new System.Runtime.Serialization.DataContractSerializer(typeof(TValue), kts);
//							Value = value;
//							ser.WriteObject(ms, value);
//							ms.Position = 0;
//							using (var strm = await _streamOpener(StreamOpenType.Write))
//							{
//								await ms.CopyToAsync(strm);
//								await strm.FlushAsync();

//							}
//						},
//						CancellationToken.None,
//						TaskCreationOptions.None,
//#if NET45
// _sch.ExclusiveScheduler
//#else
//					_sch
//#endif


//);

//			}


//			/// <summary>
//			/// Gets the value.
//			/// </summary>
//			/// <value>The value.</value>
//			public TValue Value
//			{
//				get;
//				private set;
//			}
//		}

//	}




//}

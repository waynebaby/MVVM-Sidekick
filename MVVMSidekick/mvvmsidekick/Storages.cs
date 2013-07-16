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
using Windows.Storage;

#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;

#elif SILVERLIGHT_5||SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.IO.IsolatedStorage;
#elif WINDOWS_PHONE_8||WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.IO.IsolatedStorage;
#endif


namespace MVVMSidekick
{

    namespace Storages
    {
        /// <summary>
        /// <para>Simple storage interface, for persistence.</para>
        /// <para>简单的持久化存储类型接口</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The Type needs to be save/load</para>
        /// <para>需要存取的类型</para>
        /// </typeparam>
        public interface IStorage<T>
        {
            /// <summary>
            /// <para>Ignore current changes, load from storage</para>
            /// <para>忽略当前值的变化，从持久化存储中读取</para>
            /// </summary>
            /// <returns>Async Task</returns>
            System.Threading.Tasks.Task<T> RefreshAsync();
            /// <summary>
            /// <para>Save current changes to storage</para>
            /// <para>把当前值的变化写入持久化存储中</para>
            /// </summary>
            /// <returns>Async Task</returns>
            System.Threading.Tasks.Task SaveAsync(T value);

            /// <summary>
            /// <para>Current value</para>
            /// <para>当前值</para>
            /// </summary>
            T Value { get; }
        }




        /// <summary>
        /// <para>Simple storage interface, for persistence.</para>
        /// <para>简单的持久化存储类型接口</para>
        /// </summary>
        /// <typeparam name="TToken">
        /// <para>The Token/token Type needs to be save/load</para>
        /// <para>需要存取的凭据类型</para>
        /// </typeparam>
        /// <typeparam name="TValue">
        /// <para>The Value Type needs to be save/load</para>
        /// <para>需要存取的类型</para>
        /// </typeparam>
        public interface IStorageHub<TToken, TValue>
        {

            System.Threading.Tasks.Task<TValue> LoadAsync(TToken token, bool forceRefresh);

            System.Threading.Tasks.Task SaveAsync(TToken token, TValue value);

        }

        public class StorageHub<TToken, TValue> : IStorageHub<TToken, TValue>
        {
            public StorageHub(Func<TToken, IStorage<TValue>> storageFactory, Func<Task<TToken[]>> storageTokensSelector = null)
            {
                _storageFactory = storageFactory;
                _storageTokensSelector = storageTokensSelector;
            }


            IStorage<TValue> GetOrCreatStorage(TToken token)
            {

                return _dic.GetOrAdd(token, _storageFactory);
            }


            Func<TToken, IStorage<TValue>> _storageFactory;
            Func<Task<TToken[]>> _storageTokensSelector;

            public async Task<TToken[]> GetExistsTokens()
            {
                if (_storageTokensSelector != null)
                {
                    return await _storageTokensSelector();
                }
                else
                {
                    throw new NotImplementedException("Current storageTokensSelector is not set in constructor. ");

                }

            }


            ConcurrentDictionary<TToken, IStorage<TValue>> _dic = new ConcurrentDictionary<TToken, IStorage<TValue>>();

            public async Task<TValue> LoadAsync(TToken token, bool forceRefresh)
            {
                var storage = GetOrCreatStorage(token);
                if (forceRefresh)
                {
                    return await storage.RefreshAsync();
                }
                else
                {
                    return storage.Value;
                }

            }

            public async Task SaveAsync(TToken token, TValue value)
            {
                var storage = GetOrCreatStorage(token);


                await storage.SaveAsync(value);

            }


#if NETFX_CORE
            public static StorageHub<TToken, TValue> CreateJsonDatacontractFileStorageHub(
                Func<TToken, string> fileNameFactory,
                StorageFolder folder = null,
                Func<Task<TToken[]>> storageTokensSelector = null)
            {



                var hub = new JsonDataContractStreamStorageHub<TToken, TValue>(
                   async (tp, tk) =>
                   {
                       folder = folder ?? Windows.Storage.ApplicationData.Current.LocalFolder;
                       switch (tp)
                       {
                           case StreamOpenType.Read:
                               {
                                   var file = await folder.CreateFileAsync(fileNameFactory(tk), CreationCollisionOption.OpenIfExists);

                                   return await file.OpenStreamForReadAsync();
                               }

                           case StreamOpenType.Write:
                               {
                                   var file = await folder.CreateFileAsync(fileNameFactory(tk), CreationCollisionOption.ReplaceExisting);

                                   return await file.OpenStreamForWriteAsync();
                               }

                           default:
                               return null;

                       }

                   },
                storageTokensSelector
                );
                return hub;

            }

#elif WPF
            public static StorageHub<TToken, TValue> CreateJsonDatacontractFileStorageHub(
                Func<TToken, string> fileNameFactory,
                string folder = null,
                Func<Task<TToken[]>> storageTokensSelector = null)
            {



                var hub = new JsonDataContractStreamStorageHub<TToken, TValue>(
                   async (tp, tk) =>
                   {
                       folder = folder ?? Environment.CurrentDirectory;
                       var filepath = Path.Combine(folder, fileNameFactory(tk));


                       switch (tp)
                       {
                           case StreamOpenType.Read:

                               return await TaskEx.FromResult(new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read));

                           case StreamOpenType.Write:
                               return await TaskEx.FromResult(new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));


                           default:
                               return null;

                       }

                   },
                   storageTokensSelector
                );
                return hub;

            }
#elif WINDOWS_PHONE_8|| WINDOWS_PHONE_7
              public static StorageHub<TToken, TValue> CreateJsonDatacontractIsolatedStorageHub(
                Func<TToken, string> fileNameFactory,
                IsolatedStorageFile folder = null,
                Func<Task<TToken[]>> storageTokensSelector = null)
        {


 
            var hub = new JsonDataContractStreamStorageHub<TToken, TValue>(
               async (tp, tk) =>
               {
               
                   folder = folder ?? IsolatedStorageFile.GetUserStoreForApplication();

        
                  
                   var filepath=fileNameFactory(tk);
                   switch (tp)
                   {
                       case StreamOpenType.Read:

                           return folder.OpenFile(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

                       case StreamOpenType.Write:
                           return folder.OpenFile(filepath, FileMode.Create, FileAccess.Write, FileShare.None);


                       default:
                           return null;

                   }

               },
                storageTokensSelector
            );
            return hub;

        }

#elif SILVERLIGHT_5
            public static StorageHub<TToken, TValue> CreateJsonDatacontractFileStorageHub(
                Func<TToken, string> fileNameFactory,
                string folder = null,
                Func<Task<TToken[]>> storageTokensSelector = null)
            {



                var hub = new JsonDataContractStreamStorageHub<TToken, TValue>(
                   async (tp, tk) =>
                   {
                       folder = folder ?? Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                       var filepath = Path.Combine(folder, fileNameFactory(tk));

                       switch (tp)
                       {
                           case StreamOpenType.Read:

                               return new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

                           case StreamOpenType.Write:
                               return new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);


                           default:
                               return null;

                       }

                   },
                    storageTokensSelector 
                );
                return hub;

            }
    
        public static StorageHub<TToken, TValue> CreateJsonDatacontractIsolatedStorageHub(
                Func<TToken, string> fileNameFactory,
                IsolatedStorageFile folder = null,
                Func<Task<TToken[]>> storageTokensSelector = null)
        {


 
            var hub = new JsonDataContractStreamStorageHub<TToken, TValue>(
               async (tp, tk) =>
               {
                   await TaskEx.Yield();
                   folder = folder ?? IsolatedStorageFile.GetUserStoreForApplication();

        
                  
                   var filepath=fileNameFactory(tk);
                   switch (tp)
                   {
                       case StreamOpenType.Read:

                           return folder.OpenFile(filepath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

                       case StreamOpenType.Write:
                           return folder.OpenFile(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);


                       default:
                           return null;

                   }

               },
                storageTokensSelector
            );
            return hub;

        }

#endif
        }


        public enum StreamOpenType
        {
            Read,
            Write
        }

        public class JsonDataContractStreamStorageHub<TToken, TValue> : StorageHub<TToken, TValue>
        {
            public JsonDataContractStreamStorageHub(Func<StreamOpenType, TToken, Task<Stream>> streamOpener, Func<Task<TToken[]>> storageTokensSelector = null)
                : base
                    (tk => new JsonDataContractStreamStorage<TValue>(async tp => await streamOpener(tp, tk)), storageTokensSelector)
            {


            }
        }


        public class JsonDataContractStreamStorage<TValue> : IStorage<TValue>
        {
#if NET45
            ConcurrentExclusiveSchedulerPair _sch = new ConcurrentExclusiveSchedulerPair();
#else
            TaskScheduler _sch = new LimitedConcurrencyLevelTaskScheduler(1);
#endif
            public JsonDataContractStreamStorage(Func<StreamOpenType, Task<Stream>> streamOpener, params Type[] knownTypes)
            {
                _streamOpener = streamOpener;
                _knownTypes = knownTypes;
            }

            Func<StreamOpenType, Task<Stream>> _streamOpener;

            Type[] _knownTypes;

            public Type[] KnownTypes
            {
                get { return _knownTypes; }
                set { _knownTypes = value; }
            }

            public async Task<TValue> RefreshAsync()
            {
                var kts = _knownTypes;
                return await await
                       Task.Factory.StartNew(
                           async () =>
                           {
                               var ms = new MemoryStream();
                               var ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(TValue), kts);
                               using (var strm = await _streamOpener(StreamOpenType.Read))
                               {
                                   await strm.CopyToAsync(ms);

                               }

                               ms.Position = 0;

                               var obj = (TValue)ser.ReadObject(ms);
                               Value = obj;
                               return obj;

                           },
                            CancellationToken.None,
                            TaskCreationOptions.AttachedToParent,
#if NET45
 _sch.ConcurrentScheduler
#else
 _sch
#endif

);



            }

            public async Task SaveAsync(TValue value)
            {
                var kts = _knownTypes;
                await await Task.Factory.StartNew(
                        async () =>
                        {
                            var ms = new MemoryStream();
                            var ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(TValue), kts);
                            Value = value;
                            ser.WriteObject(ms, value);
                            ms.Position = 0;
                            using (var strm = await _streamOpener(StreamOpenType.Write))
                            {
                                await ms.CopyToAsync(strm);
                                await strm.FlushAsync();

                            }
                        },
                        CancellationToken.None,
                        TaskCreationOptions.None,
#if NET45
 _sch.ExclusiveScheduler
#else
                    _sch
#endif


);

            }

            TValue _Value;
            public TValue Value
            {
                get;
                private set;
            }
        }








    }




}

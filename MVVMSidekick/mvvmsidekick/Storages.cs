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
            System.Threading.Tasks.Task<T> Refresh();
            /// <summary>
            /// <para>Save current changes to storage</para>
            /// <para>把当前值的变化写入持久化存储中</para>
            /// </summary>
            /// <returns>Async Task</returns>
            System.Threading.Tasks.Task Save(T value);

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

            System.Threading.Tasks.Task<TValue> Load(TToken token, bool forceRefresh);

            System.Threading.Tasks.Task Save(TToken token, TValue value);

        }

        public abstract class StorageHubBase<TToken, TValue> : IStorageHub<TToken, TValue>
        {
            public StorageHubBase(Func<TToken, IStorage<TValue>> storageFactory)
            {
                _storageFactory = storageFactory;
            }


            IStorage<TValue> GetOrCreatStorage(TToken token)
            {

                return _dic.GetOrAdd(token, _storageFactory);
            }


            Func<TToken, IStorage<TValue>> _storageFactory;
            ConcurrentDictionary<TToken, IStorage<TValue>> _dic = new ConcurrentDictionary<TToken, IStorage<TValue>>();

            public async Task<TValue> Load(TToken token, bool forceRefresh)
            {
                var storage = GetOrCreatStorage(token);
                if (forceRefresh)
                {
                    return await storage.Refresh();
                }
                else
                {
                    return storage.Value;
                }

            }

            public async Task Save(TToken token, TValue value)
            {
                var storage = GetOrCreatStorage(token);


                await storage.Save(value);

            }
        }


        public class JsonDataContractStreamStorageHub<TToken, TValue> : StorageHubBase<TToken, TValue>
        {
            public JsonDataContractStreamStorageHub(Func<TToken, Stream> streamOpener)
                : base
                    (tk=> new JsonDataContractStreamStorage<TValue>( ()=>streamOpener(tk)))
            {


            }

        }


        public class JsonDataContractStreamStorage<TValue> : IStorage<TValue>
        {

            Subject<int> _a;

            public JsonDataContractStreamStorage(Func< Stream> streamOpener)
            {
    

            }



            public Task<TValue> Refresh()
            {
                throw new NotImplementedException();
            }

            public Task Save(TValue value)
            {
                throw new NotImplementedException();
            }

            public TValue Value
            {
                get { throw new NotImplementedException(); }
            }
        }


    }

}

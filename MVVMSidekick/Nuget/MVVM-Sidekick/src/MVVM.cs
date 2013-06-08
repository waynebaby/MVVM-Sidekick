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
using MVVMSidekick.EventRouter;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using MVVMSidekick.Common;
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


#if NET40

namespace System.ComponentModel
{
    // Summary:
    //     Defines members that data entity classes can implement to provide custom
    //     synchronous and asynchronous validation support.
    public interface INotifyDataErrorInfo
    {
        // Summary:
        //     Gets a value that indicates whether the entity has validation errors.
        //
        // Returns:
        //     true if the entity currently has validation errors; otherwise, false.
        bool HasErrors { get; }

        // Summary:
        //     Occurs when the validation errors have changed for a property or for the
        //     entire entity.
        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        // Summary:
        //     Gets the validation errors for a specified property or for the entire entity.
        //
        // Parameters:
        //   propertyName:
        //     The name of the property to retrieve validation errors for; or null or System.String.Empty,
        //     to retrieve entity-level errors.
        //
        // Returns:
        //     The validation errors for the property or entity.
        IEnumerable GetErrors(string propertyName);
    }

    // Summary:
    //     Provides data for the System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged
    //     event.
    public class DataErrorsChangedEventArgs : EventArgs
    {
        // Summary:
        //     Initializes a new instance of the System.ComponentModel.DataErrorsChangedEventArgs
        //     class.
        //
        // Parameters:
        //   propertyName:
        //     The name of the property that has an error. null or System.String.Empty if
        //     the error is object-level.
        public DataErrorsChangedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }

        // Summary:
        //     Gets the name of the property that has an error.
        //
        // Returns:
        //     The name of the property that has an error. null or System.String.Empty if
        //     the error is object-level.
        public virtual string PropertyName { get; private set; }
    }

}
#endif




/*
#if NETFX_CORE
// Summary:y

namespace System.ComponentModel
{
    /// <summary>
    /// Provides the functionality to offer custom error information that a user
    /// interface can bind to.
    /// </summary>
    public interface IDataErrorInfo
    {

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        ///   An error message indicating what is wrong with this object. The default is
        ///   an empty string ("").
        /// </returns>
        string Error { get; }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property whose error message to get.
        /// </param>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        string this[string propertyName] { get; }
    }
}
#endif
 */
#if SILVERLIGHT_5
namespace System.Runtime.CompilerServices
{
    // Summary:
    //     Allows you to obtain the method or property name of the caller to the method.
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed class CallerMemberNameAttribute : Attribute
    {
        // Summary:
        //     Initializes a new instance of the System.Runtime.CompilerServices.CallerMemberNameAttribute
        //     class.
        public CallerMemberNameAttribute()
        {
        }
    }
    // Summary:
    //     Allows you to obtain the full path of the source file that contains the caller.
    //     This is the file path at the time of compile.
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed class CallerFilePathAttribute : Attribute
    {
        // Summary:
        //     Initializes a new instance of the System.Runtime.CompilerServices.CallerFilePathAttribute
        //     class.
        public CallerFilePathAttribute() { }
    }
    // Summary:
    //     Allows you to obtain the line number in the source file at which the method
    //     is called.
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed class CallerLineNumberAttribute : Attribute
    {
        // Summary:
        //     Initializes a new instance of the System.Runtime.CompilerServices.CallerLineNumberAttribute
        //     class.
        public CallerLineNumberAttribute() { }
    }
}

#endif



namespace MVVMSidekick
{
    /// <summary>
    /// Unify Task(4.5) and TaskEx (SL5) method in this helper
    /// </summary>
    public static class TaskExHelper
    {

        public static async Task Yield()
        {
#if SILVERLIGHT_5
            await TaskEx.Yield();
#elif NET40
            await Task.Factory.StartNew(() => { });
#else
            await Task.Yield();
#endif

        }

        public static async Task<T> FromResult<T>(T result)
        {
#if SILVERLIGHT_5
            return await TaskEx.FromResult(result);
#elif NET40
            return await Task.Factory.StartNew(() => result);
#else
            return await Task.FromResult(result);
#endif

        }

        public static async Task Delay(int ms)
        {

#if SILVERLIGHT_5
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

#if WINDOWS_PHONE_7
    public class Lazy<T>
    {
        public Lazy(Func<T> factory)
        { }
        public T Value { get; set; }
    }



#endif


    namespace Services
    {
        public interface IServiceLocator : IDisposable
        {
            bool HasInstance<TService>(string name = "");
            bool IsAsync<TService>(string name = "");
            ServiceLocatorEntryStruct<TService> Register<TService>(TService instance);
            ServiceLocatorEntryStruct<TService> Register<TService>(string name, TService instance);
            ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, TService> factory, bool alwaysNew = true);
            ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, TService> factory, bool alwaysNew = true);
            TService Resolve<TService>(string name = null, object paremeter = null);

            ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, Task<TService>> asyncFactory, bool alwaysNew = true);
            ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, Task<TService>> asyncFactory, bool alwaysNew = true);
            Task<TService> ResolveAsync<TService>(string name = null, object paremeter = null);

        }

        public interface IServiceLocator<TService> : IDisposable
        {
            bool HasInstance(string name = "");
            bool IsAsync(string name = "");
            ServiceLocatorEntryStruct<TService> Register(TService instance);
            ServiceLocatorEntryStruct<TService> Register(string name, TService instance);
            ServiceLocatorEntryStruct<TService> Register(Func<object, TService> factory, bool alwaysNew = true);
            ServiceLocatorEntryStruct<TService> Register(string name, Func<object, TService> factory, bool alwaysNew = true);
            TService Resolve(string name = null, object paremeter = null);
            ServiceLocatorEntryStruct<TService> Register(Func<object, Task<TService>> asyncFactory, bool alwaysNew = true);
            ServiceLocatorEntryStruct<TService> Register(string name, Func<object, Task<TService>> asyncFactory, bool alwaysNew = true);
            Task<TService> ResolveAsync(string name = null, object paremeter = null);
        }

        public class ServiceLocatorEntryStruct<TService>
        {
            public ServiceLocatorEntryStruct(string name)
            {
                Name = name;
            }
            public string Name { get; set; }
            public CacheType CacheType { get; set; }
            public TService ServiceInstance { private get; set; }
            public Func<object, TService> ServiceFactory { private get; set; }
            public Func<object, Task<TService>> AsyncServiceFactory { private get; set; }
            public bool GetIsValueCreated()
            {

                return (ServiceInstance != null && (!ServiceInstance.Equals(default(TService))));

            }

            public TService GetService(object paremeter = null)
            {
                switch (CacheType)
                {
                    case CacheType.Instance:
                        return ServiceInstance;
                    case CacheType.Factory:
                        return ServiceFactory(paremeter);
                    case CacheType.LazyInstance:
                        var rval = ServiceInstance;
                        if (rval == null || rval.Equals(default(TService)))
                        {
                            lock (this)
                            {
                                if (ServiceInstance == null || ServiceInstance.Equals(default(TService)))
                                {
                                    return ServiceInstance = ServiceFactory(paremeter);
                                }
                            }
                        }
                        return rval;
                    case CacheType.AsyncFactory:            //  not really suguessed to acces async factory in sync method cos may lead to deadlock ,
                    case CacheType.AsyncLazyInstance:       // but still can do.
                        Task<TService> t = GetServiceAsync(paremeter);
                        return t.Result;
                    default:
                        throw new ArgumentException("No such value supported in enum " + typeof(CacheType).ToString());
                }
            }

            Task<TService> _NotFinishedServiceTask;

            public async Task<TService> GetServiceAsync(object paremeter = null)
            {
                switch (CacheType)
                {
                    case CacheType.AsyncFactory:
                        return await AsyncServiceFactory(paremeter);
                    case CacheType.AsyncLazyInstance:
                        if (GetIsValueCreated())
                        {
                            return ServiceInstance;
                        }
                        else
                        {
                            TService rval;
                            Task<TService> rawait;
                            lock (this)
                            {
                                if (GetIsValueCreated())
                                {
                                    return ServiceInstance;
                                }

                                rawait = _NotFinishedServiceTask;
                                if (rawait == null)
                                {
                                    rawait = _NotFinishedServiceTask = AsyncServiceFactory(paremeter);
                                }

                            }

                            rval = await rawait;
                            lock (this)
                            {
                                ServiceInstance = rval;
                                _NotFinishedServiceTask = null;
                            }

                            return rval;
                        }


                    default:
#if SILVERLIGHT_5||WINDOWS_PHONE_7
                        return GetService(paremeter);


#else
                        return GetService(paremeter);
#endif

                }

            }
        }

        public class TypeSpecifiedServiceLocatorBase<TSubClass, TService> : IServiceLocator<TService>
            where TSubClass : TypeSpecifiedServiceLocatorBase<TSubClass, TService>
        {
            public ServiceLocatorEntryStruct<TService> Register(TService instance)
            {

                return Register(null, instance);
            }

            public ServiceLocatorEntryStruct<TService> Register(string name, TService instance)
            {
                name = name ?? "";
                return dic[name] =
                        new ServiceLocatorEntryStruct<TService>(name)
                        {

                            CacheType = CacheType.Instance,
                            ServiceInstance = instance,
                        };
            }

            public ServiceLocatorEntryStruct<TService> Register(Func<object, TService> factory, bool alwaysNew = true)
            {
                return Register(null, factory, alwaysNew);
            }

            public ServiceLocatorEntryStruct<TService> Register(string name, Func<object, TService> factory, bool alwaysNew = true)
            {
                name = name ?? "";

                if (alwaysNew)
                {
                    return dic[name] = new ServiceLocatorEntryStruct<TService>(name)
                    {
                        CacheType = CacheType.Factory,
                        ServiceFactory = factory
                    };

                }
                else
                {

                    return dic[name] = new ServiceLocatorEntryStruct<TService>(name)
                    {
                        CacheType = CacheType.LazyInstance,
                        ServiceFactory = factory
                    };

                }


            }

            public TService Resolve(string name = null, object parameters = null)
            {
                name = name ?? "";
                var subdic = dic;
                ServiceLocatorEntryStruct<TService> entry = null;
                if (subdic.TryGetValue(name, out entry))
                {
                    return entry.GetService(parameters);
                }
                else
                    return default(TService);
            }

            public void Dispose()
            {
                dic.Clear();
            }


            static Dictionary<string, ServiceLocatorEntryStruct<TService>> dic
               = new Dictionary<string, ServiceLocatorEntryStruct<TService>>();




            public bool HasInstance(string name = "")
            {
                name = name ?? "";
                ServiceLocatorEntryStruct<TService> entry = null;
                if (dic.TryGetValue(name, out entry))
                {
                    return
                       entry.GetIsValueCreated();

                }
                else
                {
                    return false;
                }
            }


            public ServiceLocatorEntryStruct<TService> Register(Func<object, Task<TService>> asyncFactory, bool alwaysNew = true)
            {
                return Register(null, asyncFactory, alwaysNew);
            }

            public ServiceLocatorEntryStruct<TService> Register(string name, Func<object, Task<TService>> asyncFactory, bool alwaysNew = true)
            {
                name = name ?? "";

                if (alwaysNew)
                {
                    return dic[name] = new ServiceLocatorEntryStruct<TService>(name)
                    {
                        CacheType = CacheType.AsyncFactory,
                        AsyncServiceFactory = asyncFactory
                    };

                }
                else
                {
                    return dic[name] = new ServiceLocatorEntryStruct<TService>(name)
                    {
                        CacheType = CacheType.AsyncLazyInstance,
                        AsyncServiceFactory = asyncFactory
                    };

                }

            }

            public async Task<TService> ResolveAsync(string name = null, object paremeter = null)
            {
                name = name ?? "";
                var subdic = dic;
                ServiceLocatorEntryStruct<TService> entry = null;
                if (subdic.TryGetValue(name, out entry))
                {
                    return await entry.GetServiceAsync();
                }
                else
                    //#if SILVERLIGHT_5||WINDOWS_PHONE_7
                    //                    return await T.askEx.FromResult(default(TService));
                    //#else
                    //                    return await T.ask.FromResult(default(TService));
                    //#endif
                    return await TaskExHelper.FromResult(default(TService));

            }


            public bool IsAsync(string name = "")
            {
                name = name ?? "";
                ServiceLocatorEntryStruct<TService> entry = null;
                if (dic.TryGetValue(name, out entry))
                {
                    return
                       entry.GetIsValueCreated();

                }
                else
                {
                    throw new ArgumentException("No such key");
                }
            }
        }


        public class ServiceLocatorBase<TSubClass> : IServiceLocator
            where TSubClass : ServiceLocatorBase<TSubClass>
        {
            Dictionary<Type, Action> disposeActions = new Dictionary<Type, Action>();




            public ServiceLocatorEntryStruct<TService> Register<TService>(TService instance)
            {

                return Register<TService>(null, instance);
            }

            public ServiceLocatorEntryStruct<TService> Register<TService>(string name, TService instance)
            {
                name = name ?? "";


                if (!disposeActions.ContainsKey(typeof(TService)))
                    disposeActions[typeof(TService)] = () => ServiceTypedCache<TService>.dic.Clear();
                return ServiceTypedCache<TService>.dic[name] =
                     new ServiceLocatorEntryStruct<TService>(name)
                     {
                         ServiceInstance = instance,
                         CacheType = CacheType.Instance
                     };
            }

            public ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, TService> factory, bool alwaysNew = true)
            {
                return Register<TService>(null, factory, alwaysNew);
            }

            public ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, TService> factory, bool alwaysNew = true)
            {
                name = name ?? "";
                ServiceLocatorEntryStruct<TService> rval;
                if (alwaysNew)
                {
                    ServiceTypedCache<TService>.dic[name] =
                       rval =
                  new ServiceLocatorEntryStruct<TService>(name)
                  {
                      ServiceFactory = factory,
                      CacheType = CacheType.Factory
                  };
                }
                else
                {

                    ServiceTypedCache<TService>.dic[name] =
                        rval =
                       new ServiceLocatorEntryStruct<TService>(name)
                       {
                           ServiceFactory = factory,
                           CacheType = CacheType.LazyInstance
                       };
                }
                if (!disposeActions.ContainsKey(typeof(TService)))
                    disposeActions[typeof(TService)] = () => ServiceTypedCache<TService>.dic.Clear();

                return rval;
            }

            public TService Resolve<TService>(string name = null, object paremeters = null)
            {
                name = name ?? "";
                var subdic = ServiceTypedCache<TService>.dic;
                ServiceLocatorEntryStruct<TService> entry = null;
                if (subdic.TryGetValue(name, out entry))
                {
                    return entry.GetService();
                }
                else
                    return default(TService);
            }

            public void Dispose()
            {
                foreach (var act in disposeActions.Values)
                {
                    try
                    {
                        act();
                    }
                    catch (Exception)
                    {

                    }

                }
            }

            static class ServiceTypedCache<TService>
            {
                public static Dictionary<string, ServiceLocatorEntryStruct<TService>> dic
                    = new Dictionary<string, ServiceLocatorEntryStruct<TService>>();
            }



            public bool HasInstance<TService>(string name = "")
            {
                name = name ?? "";
                ServiceLocatorEntryStruct<TService> entry = null;
                if (ServiceTypedCache<TService>.dic.TryGetValue(name, out entry))
                {
                    return
                       entry.GetIsValueCreated();

                }
                else
                {
                    return false;
                }
            }





            public bool IsAsync<TService>(string name = "")
            {
                name = name ?? "";
                ServiceLocatorEntryStruct<TService> entry = null;
                if (ServiceTypedCache<TService>.dic.TryGetValue(name, out entry))
                {
                    return
                       entry.GetIsValueCreated();

                }
                else
                {
                    throw new ArgumentException("No such key");
                }
            }

            public ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, Task<TService>> asyncFactory, bool alwaysNew = true)
            {
                return Register(null, asyncFactory, alwaysNew);
            }

            public ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, Task<TService>> asyncFactory, bool alwaysNew = true)
            {
                name = name ?? "";
                ServiceLocatorEntryStruct<TService> rval;
                if (alwaysNew)
                {
                    ServiceTypedCache<TService>.dic[name] = rval = new ServiceLocatorEntryStruct<TService>(name)
                    {
                        CacheType = CacheType.AsyncFactory,
                        AsyncServiceFactory = asyncFactory
                    };

                }
                else
                {
                    ServiceTypedCache<TService>.dic[name] = rval = new ServiceLocatorEntryStruct<TService>(name)
                    {
                        CacheType = CacheType.AsyncLazyInstance,
                        AsyncServiceFactory = asyncFactory
                    };

                }
                if (!disposeActions.ContainsKey(typeof(TService)))
                    disposeActions[typeof(TService)] = () => ServiceTypedCache<TService>.dic.Clear();
                return rval;
            }


            public async Task<TService> ResolveAsync<TService>(string name = null, object paremeter = null)
            {
                name = name ?? "";
                var subdic = ServiceTypedCache<TService>.dic;
                ServiceLocatorEntryStruct<TService> entry = null;
                if (subdic.TryGetValue(name, out entry))
                {
                    return await entry.GetServiceAsync();
                }
                else
                    //#if SILVERLIGHT_5||WINDOWS_PHONE_7
                    //                    return await T.askEx.FromResult(default(TService));
                    //#else
                    //                    return await T.ask.FromResult(default(TService));
                    //#endif
                    return await TaskExHelper.FromResult(default(TService));
            }
        }

        public enum CacheType
        {
            Instance,
            Factory,
            LazyInstance,
            AsyncFactory,
            AsyncLazyInstance
        }

        //public class DictionaryServiceLocator : IServiceLocator
        //{
        //    Dictionary<Type, Dictionary<string, Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>>>
        //       dic = new Dictionary<Type, Dictionary<string, Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>>>();
        //    public static IServiceLocator Instance { get; set; }


        //    public ServiceLocatorEntryStruct<TService> Register<TService>(TService instance)
        //    {

        //        Register<TService>(null, instance);
        //    }

        //    public ServiceLocatorEntryStruct<TService> Register<TService>(string name, TService instance)
        //    {
        //        name = name ?? "";
        //        dic[typeof(TService)] = dic[typeof(TService)] ?? new Dictionary<string, Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>>();
        //        dic[typeof(TService)][name] =
        //             new Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>(
        //                 null,
        //                 instance,
        //                 null,
        //                 CacheType.Instance);



        //    }

        //    public ServiceLocatorEntryStruct<TService> Register<TService>(Func<object, TService> factory, bool alwaysNew = true)
        //    {
        //        Register<TService>(null, factory, alwaysNew);
        //    }

        //    public ServiceLocatorEntryStruct<TService> Register<TService>(string name, Func<object, TService> factory, bool alwaysNew = true)
        //    {
        //        name = name ?? "";
        //        dic[typeof(TService)] = dic[typeof(TService)] ?? new Dictionary<string, Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>>();

        //        if (alwaysNew)
        //        {
        //            dic[typeof(TService)][name] =
        //                new Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>(
        //                    null,
        //                    default(TService),
        //                    d => factory(d) as object,
        //                    CacheType.Factory);
        //        }
        //        else
        //        {

        //            dic[typeof(TService)][name] =
        //                new Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType>(
        //                    new Lazy<object>(() => factory(null)),
        //                    default(TService),
        //                    null,
        //                    CacheType.LazyInstance);
        //        }


        //    }

        //    public TService Resolve<TService>(string name = null, object parameters = null)
        //    {
        //        name = name ?? "";
        //        var subdic = dic[typeof(TService)];
        //        if (subdic != null)
        //        {

        //            Tuple<Lazy<Object>, Object, Func<object, Object>, CacheType> entry = null;
        //            if (subdic.TryGetValue(name, out entry))
        //            {
        //                switch (entry.Item4)
        //                {
        //                    case CacheType.Instance:
        //                        return (TService)entry.Item2;
        //                    case CacheType.Factory:
        //                        return (TService)entry.Item3(parameters);
        //                    case CacheType.LazyInstance:
        //                        return (TService)entry.Item1.Value;
        //                    default:
        //                        break;
        //                }
        //                return default(TService);
        //            }
        //            else
        //                return default(TService);
        //        }
        //        else
        //        {
        //            return default(TService);
        //        }
        //    }

        //    public void Dispose()
        //    {
        //        dic.Clear();
        //    }

        //    public bool HasInstance<TService>(string name = "")
        //    {
        //        name = name ?? "";
        //        Tuple<Lazy<Object>, object, Func<object, Object>, CacheType> entry = null;
        //        if (dic[typeof(TService)].TryGetValue(name, out entry))
        //        {
        //            return
        //                (entry.Item4 == CacheType.Instance
        //                ||
        //                (
        //                    entry.Item4 == CacheType.LazyInstance
        //                    &&
        //                    entry.Item1.IsValueCreated)
        //                );


        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}
        public sealed class ServiceLocator : ServiceLocatorBase<ServiceLocator>
        {
            static ServiceLocator()
            {
                Instance = new ServiceLocator();
            }

            private ServiceLocator()
            {

            }

            public static IServiceLocator Instance { get; set; }
        }

    }
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
            System.Threading.Tasks.Task Refresh();
            /// <summary>
            /// <para>Save current changes to storage</para>
            /// <para>把当前值的变化写入持久化存储中</para>
            /// </summary>
            /// <returns>Async Task</returns>
            System.Threading.Tasks.Task Save();

            /// <summary>
            /// <para>Current value</para>
            /// <para>当前值</para>
            /// </summary>
            T Value { get; set; }
        }
    }

    namespace ViewModels
    {
        using MVVMSidekick.Views;
        /// <summary>
        /// <para>A ViewModel by default, with basic implement of name-value container.</para>
        /// <para>缺省的 ViewModel。可以用作最简单的字典绑定</para>
        /// </summary>
        public class DefaultViewModel : ViewModelBase<DefaultViewModel>
        {

        }

        /// <summary>
        /// <para>Base type of bindable model.</para>
        /// <para>ViewModel 基类</para>
        /// </summary>
        [DataContract(IsReference = true)]
        public abstract class BindableBase
            : IDisposable, INotifyPropertyChanged, IBindable
        {

            protected event EventHandler<DataErrorsChangedEventArgs> _ErrorsChanged;
            protected internal void RaiseErrorsChanged(string propertName)
            {
                if (_ErrorsChanged != null)
                {
                    _ErrorsChanged(this, new DataErrorsChangedEventArgs(propertName));
                }
            }



            private bool _IsValidationActivated = false;
            /// <summary>
            /// <para>Gets ot sets if the validation is activatied. This is a flag only， internal logic is not depend on this.</para>
            /// <para>读取/设置 此模型是否激活验证。这只是一个标记，内部逻辑并没有参考这个值</para>
            /// </summary>
            public bool IsValidationActivated
            {
                get { return _IsValidationActivated; }
                set { _IsValidationActivated = value; }
            }

            private bool _IsNotificationActivated = true;
            /// <summary>
            /// <para>Gets ot sets if the property change notification is activatied. </para>
            /// <para>读取/设置 此模型是否激活变化通知</para>
            /// </summary>
            public bool IsNotificationActivated
            {
                get { return (!IsInDesignMode) ? _IsNotificationActivated : false; }
                set { _IsNotificationActivated = value; }
            }





            static bool? _IsInDesignMode;


            /// <summary>
            /// <para>Gets if the code is running in design time. </para>
            /// <para>读取目前是否在设计时状态。</para>
            /// </summary>
            public static bool IsInDesignMode
            {
                get
                {

                    return (
                        _IsInDesignMode
                        ??
                        (

                            _IsInDesignMode =
#if SILVERLIGHT_5||WINDOWS_PHONE_8||WINDOWS_PHONE_7
 DesignerProperties.IsInDesignTool
#elif NETFX_CORE
 Windows.ApplicationModel.DesignMode.DesignModeEnabled
#else
 (bool)System.ComponentModel.DependencyPropertyDescriptor
                                .FromProperty(
                                    DesignerProperties.IsInDesignModeProperty,
                                    typeof(System.Windows.FrameworkElement))
                                .Metadata
                                .DefaultValue
#endif
))
                        .Value;
                }

            }



            /// <summary>
            ///  <para>0 for not disposed, 1 for disposed</para>
            ///  <para>0 表示没有被Dispose 1 反之</para>
            /// </summary>
            private int disposedFlag = 0;

            #region  Index and property names/索引与字段名
            /// <summary>
            /// <para>Get all property names that were defined in subtype, or added objectly in runtime</para>
            /// <para>取得本VM实例已经定义的所有字段名。其中包括静态声明的和动态添加的。</para>
            /// </summary>
            /// <returns>String[]  Property names/字段名数组 </returns>
            public abstract string[] GetFieldNames();

            ///// <summary>
            ///// <para>Gets or sets  poperty values by property name index.</para>
            ///// <para>使用索引方式取得/设置字段值</para>
            ///// </summary>
            ///// <param name="name">Property name/字段名</param>
            ///// <returns>Property value/字段值</returns>
            public abstract object this[string name] { get; set; }


            #endregion

            #region Disposing Logic/Disposing相关逻辑
            /// <summary>
            ///  <para>Dispose action infomation struct</para>
            ///  <para>注册销毁方法时的相关信息</para>
            /// </summary>
            public struct DisposeInfo
            {
                /// <summary>
                ///  <para>Comment of this dispose.</para>
                ///  <para>对此次Dispose的附加说明</para>
                /// </summary>
                public string Comment { get; set; }
                /// <summary>
                ///  <para>Caller Member Name of this dispose registeration.</para>
                ///  <para>此次Dispose注册的来源</para>
                /// </summary>
                public string Caller { get; set; }
                /// <summary>
                ///  <para>Code file path of this dispose registeration.</para>
                ///  <para>注册此次Dispose注册的代码文件</para>
                /// </summary>
                public string File { get; set; }
                /// <summary>
                ///  <para>Code line number of this dispose registeration.</para>
                ///  <para>注册此次Dispose注册的代码行</para>
                /// </summary>
                public int Line { get; set; }


                /// <summary>
                ///  <para>Exception thrown in this dispose action execution .</para>
                ///  <para>执行此次Dispose动作产生的Exception</para>
                /// </summary>
                public Exception Exception { get; set; }
                /// <summary>
                ///  <para>Dispose action.</para>
                ///  <para>Dispose动作</para>
                /// </summary>

                public Action Action { get; set; }
            }

            /// <summary>
            /// <para>Logic actions need to be executed when the instance is disposing</para>
            /// <para>销毁对象时 需要执行的操作</para>
            /// </summary>
            private List<DisposeInfo> _disposeInfos;
            private static Func<BindableBase, List<DisposeInfo>> _locateDisposeInfos =
                m =>
                {
                    if (m._disposeInfos == null)
                    {
                        Interlocked.CompareExchange(ref m._disposeInfos, new List<DisposeInfo>(), null);

                    }
                    return m._disposeInfos;

                };

            /// <summary>
            /// <para>Register logic actions need to be executed when the instance is disposing</para>
            /// <para>注册一个销毁对象时需要执行的操作</para>
            /// </summary>
            /// <param name="newAction">Disposing action/销毁操作</param>
            public void AddDisposeAction(Action newAction, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber]int line = -1)
            {

                var di = new DisposeInfo
                {
                    Caller = caller,
                    Comment = comment,
                    File = file,
                    Line = line,
                    Action = newAction

                };
                _locateDisposeInfos(this).Add(di);

            }


            /// <summary>
            /// <para>Register an object that need to be disposed when the instance is disposing</para>
            /// <para>销毁对象时 需要一起销毁的对象</para>
            /// </summary>
            /// <param name="item">disposable object/需要一起销毁的对象</param>
            public void AddDisposable(IDisposable item, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1)
            {
                AddDisposeAction(() => item.Dispose(), comment, caller, file, line);
            }


            ~BindableBase()
            {
                Dispose();
            }
            /// <summary>
            /// <para>Do all the dispose </para>
            /// <para>销毁，尝试运行所有注册的销毁操作</para>
            /// </summary>
            public void Dispose()
            {
                if (Interlocked.Exchange(ref disposedFlag, 1) == 0)
                {
                    if (_disposeInfos != null)
                    {
                        var l = _disposeInfos.Select
                            (
                                info =>
                                {
                                    //Exception gotex = null;
                                    try
                                    {
                                        info.Action();
                                    }
                                    catch (Exception ex)
                                    {
                                        info.Exception = ex;

                                    }

                                    return info;
                                }

                            )
                            .Where(x => x.Exception != null)
                            .ToArray();
                        if (l.Length > 0)
                        {
                            OnDisposeExceptions(l);
                        }
                    }


                    GC.SuppressFinalize(this);
                }


            }

            /// <summary>
            /// <para>If dispose actions got exceptions, will handled here. </para>
            /// <para>处理Dispose 时产生的Exception</para>
            /// </summary>
            /// <param name="exceptions">
            /// <para>The exception and dispose infomation</para>
            /// <para>需要处理的异常信息</para>
            /// </param>
            protected virtual void OnDisposeExceptions(IList<DisposeInfo> exceptions)
            {

            }

            #endregion

            #region Propery Changed Logic/ Propery Changed事件相关逻辑


            internal void RaisePropertyChanged(Func<PropertyChangedEventArgs> lazyEAFactory, string propertyName)
            {


                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, lazyEAFactory());
                }


            }

            /// <summary>
            ///<para>Event that raised when properties were changed and Notification was activited</para>
            ///<para> VM属性任何绑定用值被修改后,在启用通知情况下触发此事件</para>
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;


            #endregion

            #region 验证与错误相关逻辑






            protected bool CheckError(Func<Boolean> test, string errorMessage)
            {

                var rval = test();
                if (rval)
                {
                    SetErrorAndTryNotify(errorMessage);
                }
                return rval;

            }


            ///// <summary>
            ///// 验证错误内容
            ///// </summary>
            //string IDataErrorInfo.Error
            //{
            //    get
            //    {
            //        return GetError();
            //    }


            //}
            /// <summary>
            /// <para>Gets the validate error of this model </para>
            /// <para>取得错误内容</para>
            /// </summary>
            /// <returns>Error string/错误内容字符串</returns>
            public abstract string Error { get; }
            /// <summary>
            /// <para>Sets the validate error of this model </para>
            /// <para>设置错误内容</para>
            /// </summary>
            /// <returns>Error string/错误内容字符串</returns>
            protected abstract void SetError(string value);

            /// <summary>
            /// <para>Sets the validate error of this model and notify </para>
            /// <para>设置错误内容并且尝试用事件通知</para>
            /// </summary>
            /// <returns>Error string/错误内容字符串</returns>
            protected abstract void SetErrorAndTryNotify(string value);



            /// <summary>
            /// <para>Gets validate error string of this field</para>
            /// <para>取得对于每个字段，验证失败所产生的错误信息</para>
            /// </summary>
            /// <param name="propertyName">Property Name of error /要检查错误的属性名</param>
            /// <returns>Rrror string /错误字符串</returns>
            protected abstract string GetColumnError(string propertyName);



            #endregion


            //   public abstract bool IsUIBusy { get; set; }








        }

        /// <summary>
        /// <para>Extension methods of models</para>
        /// <para>为Model增加的一些快捷方法</para>
        /// </summary>
        public static class BindableBaseExtensions
        {



            /// <summary>
            /// <para>Config Value Container with delegate</para>
            /// <para>使用连续的API设置ValueContainer的一些参数</para>            
            /// </summary>
            /// <typeparam name="TProperty">ValueContainer内容的类型</typeparam>
            /// <param name="target">ValueContainer的配置目标实例</param>
            /// <param name="action">配置内容</param>
            /// <returns>ValueContainer的配置目标实例</returns>
            public static ValueContainer<TProperty> Config<TProperty>(this ValueContainer<TProperty> target, Action<ValueContainer<TProperty>> action)
            {
                action(target);
                return target;
            }

            /// <summary>
            /// <para>Add Idisposeable to model's despose action list</para>
            /// <para>将IDisposable 对象注册到VM中的销毁对象列表。</para>
            /// </summary>
            /// <typeparam name="T">Type of Model /Model的类型</typeparam>
            /// <param name="item">IDisposable Inastance/IDisposable实例</param>
            /// <param name="vm">Model instance /Model 实例</param>
            /// <returns></returns>
            public static T DisposeWith<T>(this T item, IBindable vm, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1) where T : IDisposable
            {
                vm.AddDisposable(item, comment, caller, file, line);
                return item;
            }

            public static ValueContainer<T> Initialize<T>(this BindableBase model, string propertyName, ref Property<T> reference, ref Func<BindableBase, ValueContainer<T>> locator, Func<T> defaultValueFactory = null)
            {
                if (reference == null)
                    reference = new Property<T> { LocatorFunc = locator };
                if (reference.Container == null)
                {
                    reference.Container = new ValueContainer<T>(propertyName, model);
                    if (defaultValueFactory != null)
                    {
                        reference.Container.Value = defaultValueFactory();
                    }
                }
                return reference.Container;
            }

            public static ValueContainer<T> Initialize<T>(this BindableBase model, string propertyName, ref Property<T> reference, ref Func<BindableBase, ValueContainer<T>> locator, Func<BindableBase, T> defaultValueFactory = null)
            {
                return Initialize(model, propertyName, ref reference, ref locator, () => (defaultValueFactory != null) ? defaultValueFactory(model) : default(T));
            }
        }


        /// <summary>
        /// <para>A slot to place the value container field and value container locator.</para>
        /// <para>属性定义。一个属性定义包括一个创建/定位属性“值容器”的静态方法引用，和一个缓存该方法执行结果“值容器”的槽位</para>
        /// </summary>
        /// <typeparam name="TProperty">Type of the property value /属性的类型</typeparam>
        public class Property<TProperty>
        {
            public Property()
            {

            }

            /// <summary>
            /// <para>Locate or create the value container of this model intances</para>
            /// <para>通过定位方法定位本Model实例中的值容器</para>

            /// </summary>
            /// <param name="model">Model intances/model 实例</param>
            /// <returns>Value Container of this property/值容器</returns>
            public ValueContainer<TProperty> LocateValueContainer(BindableBase model)
            {

                return LocatorFunc(model);
            }


            /// <summary>
            /// <para>Gets sets the factory to locate/create value container of this model instance</para>
            /// <para>读取/设置定位值容器用的方法。</para>
            /// </summary>
            public Func<BindableBase, ValueContainer<TProperty>> LocatorFunc
            {
                internal get;
                set;
            }

            /// <summary>
            /// <para>Gets or sets Value Container, it can be recently create and cached here，by LocatorFunc </para>
            /// <para>读取/设置值容器,这事值容器LocatorFunc创建值容器并且缓存的位置 </para>
            /// </summary>
            public ValueContainer<TProperty> Container
            {
                get;
                set;
            }


        }

        /// <summary>
        /// <para>Value Container, holds the value of certain field, with notifition /and compare support</para>
        /// <para>值容器</para>
        /// </summary>
        /// <typeparam name="TProperty">Type of the property value /属性的类型</typeparam>
        public class ValueContainer<TProperty> : IErrorInfo, IValueCanSet<TProperty>, IValueCanGet<TProperty>, IValueContainer
        {


            #region Constructors /构造器
            /// <summary>
            /// <para>Create a new Value Container</para>
            /// <para>创建属性值容器</para>
            /// </summary>
            /// <param name="model">
            /// <para>The model that Value Container will be held with.</para>
            /// <para>所属的model实例</para>
            /// </param>
            /// <param name="info">Property name/属性名</param>
            /// <param name="initValue">The first value of this container/初始值</param>
            public ValueContainer(string info, BindableBase model, TProperty initValue = default (TProperty ))
                : this(info, model, (v1, v2) => v1.Equals(v2), initValue)
            {
            }





            /// <summary>
            /// <para>Create a new Value Container</para>
            /// <para>创建属性值容器</para>
            /// </summary>
            /// <param name="model">
            /// <para>The model that Value Container will be held with.</para>
            /// <para>所属的model实例</para>
            /// </param>
            /// <param name="info">Property name/属性名</param>
            /// <param name="equalityComparer">
            /// <para>Comparer of new/old value, for notifition.</para>
            /// <para>判断两个值是否相等的比较器,用于判断是否通知变更</para>
            /// </param>
            /// <param name="initValue">The first value of this container/初始值</param>
            public ValueContainer(string info, BindableBase model, Func<TProperty, TProperty, bool> equalityComparer, TProperty initValue = default (TProperty))
            {
                EqualityComparer = equalityComparer;
                PropertyName = info;
                PropertyType = typeof(TProperty);
                Model = model;
                Value = initValue;
                _Errors = new ObservableCollection<ErrorEntity>();
                _Errors.GetEventObservable(model)
                    .Subscribe
                    (
                        e =>
                        {
                            model.RaiseErrorsChanged(PropertyName);
                        }
                    )
                    .DisposeWith(model);

            }

            #endregion

            /// <summary>
            /// <para>Event that raised when value was changed</para>
            /// <para>值变更时触发的事件</para>
            /// </summary>
            public event EventHandler<ValueChangedEventArgs<TProperty>> ValueChanged;

            /// <summary>
            /// <para>Gets comparer instance of new/old value, for notifition.</para>
            /// <para>读取判断两个值是否相等的比较器,用于判断是否通知变更</para>
            /// </summary>
            public Func<TProperty, TProperty, bool> EqualityComparer { get; private set; }

            /// <summary>
            /// Property name /属性名
            /// </summary>
            public string PropertyName { get; private set; }

            TProperty _value;

            /// <summary>
            /// Value/值 
            /// </summary>
            public TProperty Value
            {
                get { return _value; }
                set { SetValueAndTryNotify(value); }
            }

            /// <summary>
            /// <para>Save the value and try raise the value changed event</para>
            /// <para>保存值并且尝试触发更改事件</para>
            /// </summary>
            /// <param name="value">New value/属性值</param>
            public ValueContainer<TProperty> SetValueAndTryNotify(TProperty value)
            {
                InternalPropertyChange(this.Model, value, ref _value, PropertyName);
                return this;
            }

            /// <summary>
            /// <para>Save the value and do not try raise the value changed event</para>
            /// <para>仅保存值 不尝试触发更改事件</para>
            /// </summary>
            /// <param name="value">New value/属性值</param>
            public ValueContainer<TProperty> SetValue(TProperty value)
            {
                _value = value;
                return this;
            }


            private void InternalPropertyChange(BindableBase objectInstance, TProperty newValue, ref TProperty currentValue, string message)
            {
                var changing = (this.EqualityComparer == null) ?
                    !this.EqualityComparer(newValue, currentValue) :
                    !Object.Equals(newValue, currentValue);


                if (changing)
                {
                    var oldvalue = currentValue;
                    currentValue = newValue;

                    ValueChangedEventArgs<TProperty> arg = null;

                    Func<PropertyChangedEventArgs> lzf =
                        () =>
                        {

                            arg = arg ?? new ValueChangedEventArgs<TProperty>(message, oldvalue, newValue);
                            return arg;
                        };


                    objectInstance.RaisePropertyChanged(lzf, message);
                    if (ValueChanged != null) ValueChanged(this, lzf() as ValueChangedEventArgs<TProperty>);

                }
            }


            /// <summary>
            /// <para>The model instance that Value Container was held.</para>
            /// <para>此值容器所在的Model</para>
            /// </summary>
            public BindableBase Model { get; internal set; }





            object IValueContainer.Value
            {
                get
                {
                    return Value;
                }
                set
                {
                    SetValueAndTryNotify((TProperty)value);
                }
            }


            /// <summary>
            /// Gets the type of property/读取值类型
            /// </summary>
            public Type PropertyType
            {
                get;
                private set;
            }


            ObservableCollection<ErrorEntity> _Errors;

            public ObservableCollection<ErrorEntity> Errors
            {
                get { return _Errors; }

            }



#if NETFX_CORE
            bool _IsCopyToAllowed = !typeof(ICommand).GetTypeInfo().IsAssignableFrom(typeof(TProperty).GetTypeInfo());
#else
            bool _IsCopyToAllowed = !typeof(ICommand).IsAssignableFrom(typeof(TProperty));
#endif
            /// <summary>
            /// <para>Can be copied by CopyTo method</para>
            /// <para>是否可以被 `Copyto` 复制到另外一个属性</para>
            /// </summary>
            public bool IsCopyToAllowed
            {
                get { return _IsCopyToAllowed; }
                set { _IsCopyToAllowed = value; }
            }
        }


        /// <summary>
        /// <para>Event args that fired when property changed, with old value and new value field.</para>
        /// <para>值变化事件参数</para>
        /// </summary>
        /// <typeparam name="TProperty">Type of propery/变化属性的类型</typeparam>
        public class ValueChangedEventArgs<TProperty> : PropertyChangedEventArgs
        {
            /// <summary>
            /// Constructor of ValueChangedEventArgs
            /// </summary>
            public ValueChangedEventArgs(string propertyName, TProperty oldValue, TProperty newValue)
                : base(propertyName)
            {
                NewValue = newValue;
                OldValue = oldValue;
            }

            /// <summary>
            /// New Value
            /// </summary>
            public TProperty NewValue { get; private set; }
            /// <summary>
            /// Old Value
            /// </summary>
            public TProperty OldValue { get; private set; }
        }


        /// <summary>
        /// <para>A Bindebale Tuple</para>
        /// <para>一个可绑定的Tuple实现</para>
        /// </summary>
        /// <typeparam name="TItem1">Type of first item/第一个元素的类型</typeparam>
        /// <typeparam name="TItem2">Type of second item/第二个元素的类型</typeparam>
        [DataContract(IsReference = true)]
        public class BindableTuple<TItem1, TItem2> : BindableBase<BindableTuple<TItem1, TItem2>>
        {
            public BindableTuple(TItem1 item1, TItem2 item2)
            {
                this.IsNotificationActivated = false;
                Item1 = item1;
                Item2 = item2;
                this.IsNotificationActivated = true;
            }
            /// <summary>
            /// 第一个元素
            /// </summary>

            public TItem1 Item1
            {
                get { return _Item1Locator(this).Value; }
                set { _Item1Locator(this).SetValueAndTryNotify(value); }
            }
            #region Property TItem1 Item1 Setup
            protected Property<TItem1> _Item1 = new Property<TItem1> { LocatorFunc = _Item1Locator };
            static Func<BindableBase, ValueContainer<TItem1>> _Item1Locator = RegisterContainerLocator<TItem1>("Item1", model => model.Initialize("Item1", ref model._Item1, ref _Item1Locator, _Item1DefaultValueFactory));
            static Func<BindableBase, TItem1> _Item1DefaultValueFactory = null;
            #endregion

            /// <summary>
            /// 第二个元素
            /// </summary>

            public TItem2 Item2
            {
                get { return _Item2Locator(this).Value; }
                set { _Item2Locator(this).SetValueAndTryNotify(value); }
            }
            #region Property TItem2 Item2 Setup
            protected Property<TItem2> _Item2 = new Property<TItem2> { LocatorFunc = _Item2Locator };
            static Func<BindableBase, ValueContainer<TItem2>> _Item2Locator = RegisterContainerLocator<TItem2>("Item2", model => model.Initialize("Item2", ref model._Item2, ref _Item2Locator, _Item2DefaultValueFactory));
            static Func<BindableBase, TItem2> _Item2DefaultValueFactory = null;
            #endregion


        }
        /// <summary>
        /// <para>Fast create Bindable Tuple </para>
        /// <para>帮助快速创建BindableTuple的帮助类</para>
        /// </summary>
        public static class BindableTuple
        {
            /// <summary>
            /// Create a Tuple
            /// </summary>

            public static BindableTuple<TItem1, TItem2> Create<TItem1, TItem2>(TItem1 item1, TItem2 item2)
            {
                return new BindableTuple<TItem1, TItem2>(item1, item2);
            }

        }


        /// <summary>
        /// <para>Model type with detail subtype type paremeter.</para>
        /// <para>具有子类详细类型定义的model </para>
        /// <example>
        /// public class Class1:BindableBase&lt;Class1&gt;  {}
        /// </example>
        /// </summary>
        /// <typeparam name="TSubClassType"> Sub Type / 子类类型</typeparam>
        [DataContract(IsReference = true)]
        public abstract class BindableBase<TSubClassType> : BindableBase, INotifyDataErrorInfo where TSubClassType : BindableBase<TSubClassType>
        {

            /// <summary>
            /// 清除值
            /// </summary>
            public void ResetPropertyValue<T>(Property<T> property)
            {
                if (property != null)
                {
                    var oldContainer = property.Container;
                    if (oldContainer != null)
                    {


                        property.Container = null;
                        property.LocatorFunc(oldContainer.Model);
                        oldContainer.SetValueAndTryNotify(property.Container.Value);
                        property.Container = oldContainer;
                    }
                }


            }




            /// <summary>
            /// <para>Cast a model instance to current model subtype</para>
            /// <para>将一个 model 引用特化为本子类型的引用</para>
            /// </summary>
            /// <param name="model"> some bindable model/某种可绑定model</param>
            /// <returns>Current sub type instance/本类型引用</returns>
            public static TSubClassType CastToCurrentType(BindableBase model)
            {
                return (TSubClassType)model;

            }
            /// <summary>
            /// <para>Type cache of container getter</para>
            /// <para>每个属性类型独占的一个专门的类型缓存。</para>
            /// </summary>
            /// <typeparam name="TProperty"></typeparam>
            protected static class TypeDic<TProperty>
            {
                public static Dictionary<string, Func<TSubClassType, ValueContainer<TProperty>>> _propertyContainerGetters = new Dictionary<string, Func<TSubClassType, ValueContainer<TProperty>>>();

            }

            /// <summary>
            /// 根据索引获取属性值
            /// </summary>
            /// <param name="colName">属性名</param>
            /// <returns>属性值</returns>
            public override object this[string colName]
            {
                get
                {
                    var lc = GetOrCreatePlainLocator(colName, this);
                    return lc((TSubClassType)this).Value;
                }
                set
                {

                    var lc = GetOrCreatePlainLocator(colName, this);
                    lc((TSubClassType)this).Value = value;
                }
            }

            private static Func<TSubClassType, IValueContainer> GetOrCreatePlainLocator(string colName, BindableBase viewModel)
            {
                Func<TSubClassType, IValueContainer> pf;
                if (!_plainPropertyContainerGetters.TryGetValue(colName, out pf))
                {
                    var p = new ValueContainer<object>(colName, viewModel);
                    pf = _ => p;
                    _plainPropertyContainerGetters[colName] = pf;
                }
                return pf;
            }




#if SILVERLIGHT_5||WINDOWS_PHONE_8||WINDOWS_PHONE_7
            protected static Dictionary<string, Func<TSubClassType, IValueContainer>>
             _plainPropertyContainerGetters =
             new Dictionary<string, Func<TSubClassType, IValueContainer>>(StringComparer.CurrentCultureIgnoreCase);
#else

            protected static Dictionary<string, Func<TSubClassType, IValueContainer>>
                _plainPropertyContainerGetters =
                new Dictionary<string, Func<TSubClassType, IValueContainer>>(StringComparer.CurrentCultureIgnoreCase);
#endif



            public override string Error
            {
                get { return _ErrorLocator(this).Value; }
            }

            protected override void SetError(string value)
            {
                _ErrorLocator(this).SetValue(value);
            }

            protected override void SetErrorAndTryNotify(string value)
            {
                _ErrorLocator(this).SetValueAndTryNotify(value);
            }


            #region Property string Error Setup

            protected Property<string> _Error =
              new Property<string> { LocatorFunc = _ErrorLocator };
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            static Func<BindableBase, ValueContainer<string>> _ErrorLocator =
                RegisterContainerLocator<string>(
                "Error",
                model =>
                {
                    model._Error =
                        model._Error
                        ??
                        new Property<string> { LocatorFunc = _ErrorLocator };
                    return model._Error.Container =
                        model._Error.Container
                        ??
                        new ValueContainer<string>("Error", model);
                });

            #endregion






            /// <summary>
            /// 注册一个属性容器的定位器。
            /// </summary>
            /// <typeparam name="TProperty">属性类型</typeparam>
            /// <param name="propertyName">属性名</param>
            /// <param name="getOrCreateLocatorMethod">属性定位/创建方法 也就是定位器</param>
            /// <returns>注册后的定位器</returns>
            protected static Func<BindableBase, ValueContainer<TProperty>> RegisterContainerLocator<TProperty>(string propertyName, Func<TSubClassType, ValueContainer<TProperty>> getOrCreateLocatorMethod)
            {


                TypeDic<TProperty>._propertyContainerGetters[propertyName] = getOrCreateLocatorMethod;
                _plainPropertyContainerGetters[propertyName] = (v) => getOrCreateLocatorMethod(v) as IValueContainer;
                return o => getOrCreateLocatorMethod((TSubClassType)o);
            }


            /// <summary>
            /// 根据属性名取得一个值容器
            /// </summary>
            /// <typeparam name="TProperty">属性类型</typeparam>
            /// <param name="propertyName">属性名</param>
            /// <returns>值容器</returns>
            public ValueContainer<TProperty> GetValueContainer<TProperty>(string propertyName)
            {
                Func<TSubClassType, ValueContainer<TProperty>> contianerGetterCreater;
                if (!TypeDic<TProperty>._propertyContainerGetters.TryGetValue(propertyName, out contianerGetterCreater))
                {
                    throw new Exception("Property Not Exists!");

                }

                return contianerGetterCreater((TSubClassType)(Object)this);

            }

            /// <summary>
            /// 根据表达式树取得一个值容器
            /// </summary>
            /// <typeparam name="TProperty">属性类型</typeparam>
            /// <param name="expression">表达式树</param>
            /// <returns>值容器</returns>
            public ValueContainer<TProperty> GetValueContainer<TProperty>(Expression<Func<TSubClassType, TProperty>> expression)
            {
                var propName = MVVMSidekick.Common.ExpressionHelper.GetPropertyName<TSubClassType, TProperty>(expression);
                return GetValueContainer<TProperty>(propName);

            }




            /// <summary>
            /// 根据属性名取得一个值容器
            /// </summary>
            /// <param name="propertyName">属性名</param>
            /// <returns>值容器</returns>
            public IValueContainer GetValueContainer(string propertyName)
            {
                Func<TSubClassType, IValueContainer> contianerGetterCreater;
                if (!_plainPropertyContainerGetters.TryGetValue(propertyName, out contianerGetterCreater))
                {
                    return null;

                }

                return contianerGetterCreater((TSubClassType)(Object)this);

            }




            /// <summary>
            /// 获取某一属性的验证错误信息
            /// </summary>
            /// <param name="propertyName">属性名</param>
            /// <returns>错误信息字符串</returns>
            protected override string GetColumnError(string propertyName)
            {
                if (_plainPropertyContainerGetters[propertyName]((TSubClassType)this).Errors.Count > 0)
                {


                    var error = string.Join(",", _plainPropertyContainerGetters[propertyName]((TSubClassType)this).Errors.Select(x => x.Message));
                    var propertyContainer = this.GetValueContainer(propertyName);
#if NETFX_CORE
                    if (propertyContainer != null && typeof(INotifyDataErrorInfo).GetTypeInfo().IsAssignableFrom(propertyContainer.PropertyType.GetTypeInfo()))
#else

                    if (propertyContainer != null && typeof(INotifyDataErrorInfo).IsAssignableFrom(propertyContainer.PropertyType))
#endif
                    {
                        INotifyDataErrorInfo di = this[propertyName] as INotifyDataErrorInfo;
                        if (di != null)
                        {
                            error = error + "\r\n-----Inner " + propertyName + " as INotifyDataErrorInfo -------\r\n\t" + di.HasErrors.ToString();
                        }
                    }

                    return error;
                }
                return null;
            }



            /// <summary>
            /// 获取所有属性名，包括静态声明和动态添加的
            /// </summary>
            /// <returns></returns>
            public override string[] GetFieldNames()
            {
                return _plainPropertyContainerGetters.Keys.ToArray();
            }


            /// <summary>
            /// 创建一个VM副本
            /// </summary>
            /// <returns>新引用</returns>
            public TSubClassType Clone()
            {
                var x = (TSubClassType)Activator.CreateInstance(typeof(TSubClassType));
                CopyTo(x);
                return x;
            }

            static void Copyref<T>(T source, ref T target)
            {


                if (source == null)
                {
                    target = source;
                    return;
                }

                var sourcetype = source.GetType().GetTypeOrTypeInfo();
                if (sourcetype.IsValueType || source is string)
                {
                    target = source;
                }
#if ! (SILVERLIGHT_5 || WINDOWS_PHONE_8|| WINDOWS_PHONE_7 || NETFX_CORE)

                else if (typeof(ICloneable).IsAssignableFrom(sourcetype))
                {
                    target = (T)((ICloneable)source).Clone();
                }
#endif
                else if (typeof(System.Collections.IList).GetTypeOrTypeInfo().IsAssignableFrom(sourcetype))
                {
                    var tarcol = target as System.Collections.IList;
                    var scol = source as System.Collections.IList;
                    if (tarcol == null)
                    {

                        var newcol = sourcetype.IsArray ?
                            Array.CreateInstance(sourcetype.GetElementType(), scol.Count) :
                            System.Activator.CreateInstance(source.GetType(), new object[0]) as System.Collections.IList;


                        tarcol = (System.Collections.IList)newcol;
                    }
                    else
                    {
                        tarcol.Clear();
                    }
                    if (tarcol != null)
                    {


                        foreach (var item in scol)
                        {
                            object newv = null;
                            Copyref(item, ref newv);
                            tarcol.Add(newv);
                        }
                        target = (T)tarcol;
                    }
                    else
                    {
                        target = default(T);
                    }
                }
            }

            public void CopyTo(TSubClassType target)
            {
                foreach (var item in GetFieldNames())
                {
                    var ctThis = GetValueContainer(item);
                    var ctTarget = target.GetValueContainer(item);
                    if (ctThis.IsCopyToAllowed)
                    {
                        object temp = null;
                        Copyref(this[item], ref temp);
                        target[item] = temp;
                    }


                }
            }


            event EventHandler<DataErrorsChangedEventArgs> INotifyDataErrorInfo.ErrorsChanged
            {
                add { _ErrorsChanged += value; }
                remove { _ErrorsChanged -= value; }
            }



            System.Collections.IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
            {
                if (this.GetFieldNames().Contains(propertyName))
                {
                    return this.GetValueContainer(propertyName).Errors;
                }
                else
                {
                    return null;
                }

            }


            bool INotifyDataErrorInfo.HasErrors
            {
                get
                {
                    //  return false;
                    RefreshErrors();
                    return !string.IsNullOrEmpty(this.Error);

                }
            }

            private void RefreshErrors()
            {
                var sb = new StringBuilder();
                var rt = GetAllErrors().Select(x =>
                {
                    return sb.Append(x.Message).Append(":").AppendLine(x.Exception.ToString());
                }
                    )
                    .ToArray();
                this.SetErrorAndTryNotify(sb.ToString());


            }

            public ErrorEntity[] GetAllErrors()
            {
                var errors = GetFieldNames()
                     .SelectMany(name => this.GetValueContainer(name).Errors)
                     .Where(x => x != null)
                     .Where(x => !(string.IsNullOrEmpty(x.Message) || x.Exception == null))
                     .ToArray();
                return errors;
            }

            //public override IDictionary<string,object >  Values
            //{
            //    get { return new BindableAccesser<TSubClassType>(this); }
            //}


        }

        public interface IBindable : INotifyPropertyChanged
        {
            void AddDisposable(IDisposable item, string comment = "", string member = "", string file = "", int line = -1);
            void AddDisposeAction(Action action, string comment = "", string member = "", string file = "", int line = -1);
            string Error { get; }
            void Dispose();
            //IDictionary<string,object >  Values { get; }
            string[] GetFieldNames();
            object this[string name] { get; set; }
        }


        //#if !NETFX_CORE

        //        public class StringToViewModelInstanceConverter : TypeConverter
        //        {
        //            public override bool CanConvertTo(ITypeDescriptorContext context, Type sourceType)
        //            {

        //                //if (sourceType == typeof(string))
        //                    return true;
        //                //return base.CanConvertFrom(context, sourceType);
        //            }
        //            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        //            {
        //                return true;
        //            }

        //            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        //            {

        //                var str = value.ToString();
        //                var t = Type.GetType(str);
        //                var v = Activator.CreateInstance(t);
        //                return v;
        //                ////  return base.ConvertFrom(context, culture, value);
        //            }
        //            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        //            {
        //                return value.ToString();
        //            }
        //        }

        //        [TypeConverter(typeof(StringToViewModelInstanceConverter))]
        //#endif
        public partial interface IViewModel : IBindable, INotifyPropertyChanged
        {
            Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue);
            Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue);
            Task OnBindedViewLoad(MVVMSidekick.Views.IView view);
            Task WaitForClose(Action closingCallback = null);
            bool IsUIBusy { get; set; }
            bool HaveReturnValue { get; }
            void Close();
            MVVMSidekick.Views.StageManager StageManager { get; set; }
        }

        public partial interface IViewModel<TResult> : IViewModel
        {
            Task<TResult> WaitForCloseWithResult(Action closingCallback = null);
            TResult Result { get; set; }
        }


        [DataContract(IsReference = true)]
        public struct NoResult
        {

        }

        public struct ShowAwaitableResult<TViewModel>
        {
            public TViewModel ViewModel { get; set; }
            public Task Closing { get; set; }

        }
        public partial class ViewModelBase<TViewModel, TResult> : ViewModelBase<TViewModel>, IViewModel<TResult>
            where TViewModel : ViewModelBase<TViewModel, TResult>, IViewModel<TResult>
        {

            public override bool HaveReturnValue { get { return true; } }

            public Task<TResult> WaitForCloseWithResult(Action closingCallback = null)
            {
                var t = new Task<TResult>(() => Result);

                this.AddDisposeAction(
                    () =>
                    {
                        if (closingCallback != null)
                        {
                            closingCallback();
                        }
                        t.Start();
                    }
                    );


                return t;
            }

            public TResult Result
            {
                get { return _ResultLocator(this).Value; }
                set { _ResultLocator(this).SetValueAndTryNotify(value); }
            }

            #region Property TResult Result Setup
            protected Property<TResult> _Result =
              new Property<TResult> { LocatorFunc = _ResultLocator };
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            static Func<BindableBase, ValueContainer<TResult>> _ResultLocator =
                RegisterContainerLocator<TResult>(
                    "Result",
                    model =>
                    {
                        model._Result =
                            model._Result
                            ??
                            new Property<TResult> { LocatorFunc = _ResultLocator };
                        return model._Result.Container =
                            model._Result.Container
                            ??
                            new ValueContainer<TResult>("Result", model);
                    });
            #endregion




        }


        /// <summary>
        /// 一个VM,带有若干界面特性
        /// </summary>
        /// <typeparam name="TViewModel">本身的类型</typeparam>

        public abstract partial class ViewModelBase<TViewModel> : BindableBase<TViewModel>, IViewModel where TViewModel : ViewModelBase<TViewModel>
        {

            protected virtual async Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
            {
                //#if SILVERLIGHT_5
                //                await T.askEx.Yield();
                //#else
                //                await T.ask.Yield();
                //#endif

                StageManager = new StageManager(this) { CurrentBindingView = view };
                StageManager.InitParent(() => view.Parent);
                StageManager.DisposeWith(this);
                await TaskExHelper.Yield();
            }

            protected virtual async Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue)
            {
                //#if SILVERLIGHT_5
                //                await T.askEx.Yield();
                //#else
                //                await T.ask.Yield();
                //#endif
                await TaskExHelper.Yield();
            }

            protected virtual async Task OnBindedViewLoad(IView view)
            {
                StageManager = new StageManager(this) { CurrentBindingView = view };
                StageManager.InitParent(() => view.Parent);
                StageManager.DisposeWith(this);
                await TaskExHelper.Yield();
            }

            async Task IViewModel.OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
            {
                if (IsInDesignMode)
                {
                    await TaskExHelper.Yield();
                }
                else
                    await OnBindedToView(view, oldValue);
            }
            async Task IViewModel.OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue)
            {
                if (IsInDesignMode)
                {
                    await TaskExHelper.Yield();
                }
                else
                    await OnUnbindedFromView(view, newValue);
            }

            async Task IViewModel.OnBindedViewLoad(MVVMSidekick.Views.IView view)
            {

                if (IsInDesignMode)
                {
                    await TaskExHelper.Yield();
                }
                else
                {

                    await OnBindedViewLoad(view);
                }
            }



            MVVMSidekick.Views.StageManager _StageManager;

            public MVVMSidekick.Views.StageManager StageManager
            {
                get { return _StageManager; }
                set { _StageManager = value; }
            }

            /// <summary>
            /// 是否有返回值
            /// </summary>
            public virtual bool HaveReturnValue { get { return false; } }
            /// <summary>
            /// 本UI是否处于忙状态
            /// </summary>
            public bool IsUIBusy
            {
                get { return _IsUIBusyLocator(this).Value; }
                set { _IsUIBusyLocator(this).SetValueAndTryNotify(value); }
            }

            #region Property bool IsUIBusy Setup
            protected Property<bool> _IsUIBusy =
              new Property<bool> { LocatorFunc = _IsUIBusyLocator };
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            static Func<BindableBase, ValueContainer<bool>> _IsUIBusyLocator =
                RegisterContainerLocator<bool>(
                    "IsUIBusy",
                    model =>
                    {
                        model._IsUIBusy =
                            model._IsUIBusy
                            ??
                            new Property<bool> { LocatorFunc = _IsUIBusyLocator };
                        return model._IsUIBusy.Container =
                            model._IsUIBusy.Container
                            ??
                            new ValueContainer<bool>("IsUIBusy", model);
                    });
            #endregion

            public Task WaitForClose(Action closingCallback = null)
            {
                var t = new Task(() => { });
                if (closingCallback != null)
                {
                    this.AddDisposeAction(
                        () =>
                        {
                            closingCallback();
                            t.Start();
                        }
                        );
                }

                return t;
            }
            public void Close()
            {
                if (StageManager != null)
                {

                    this.StageManager.CurrentBindingView.SelfClose();
                }
            }


        }




        public class ErrorEntity
        {
            public string Message { get; set; }
            public Exception Exception { get; set; }
            public IErrorInfo InnerErrorInfoSource { get; set; }
            public override string ToString()
            {

                return null;// string.Format("{0}，{1}，{2}", Message, Exception, InnerErrorInfoSource);
            }
        }
        public interface IErrorInfo
        {
            ObservableCollection<ErrorEntity> Errors { get; }
        }

        public interface IValueCanSet<in T>
        {
            T Value { set; }
        }

        public interface IValueCanGet<out T>
        {
            T Value { get; }
        }

        public interface IValueContainer : IErrorInfo
        {
            Type PropertyType { get; }
            Object Value { get; set; }
            bool IsCopyToAllowed { get; set; }
        }

        public interface ICommandModel<TCommand, TResource> : ICommand
        {
            TCommand CommandCore { get; }
            bool LastCanExecuteValue { get; set; }
            TResource Resource { get; set; }
        }

        public class StringResourceReactiveCommandModel : CommandModel<ReactiveCommand, string>
        {

        }

        /// <summary>
        /// 用于封装ICommand的ViewModel。一般包括一个Command实例和对应此实例的一组资源
        /// </summary>
        /// <typeparam name="TCommand">ICommand 详细类型</typeparam>
        /// <typeparam name="TResource">配合Command 的资源类型</typeparam>
        public class CommandModel<TCommand, TResource> : BindableBase<CommandModel<TCommand, TResource>>, ICommandModel<TCommand, TResource>
            where TCommand : ICommand
        {
            public override string ToString()
            {
                return Resource.ToString();
            }

            public CommandModel()
            { }
            /// <summary>
            /// 构造器
            /// </summary>
            /// <param name="commandCore">ICommand核心</param>
            /// <param name="resource">初始资源</param>
            public CommandModel(TCommand commandCore, TResource resource)
            {
                CommandCore = commandCore;
                commandCore.CanExecuteChanged += commandCore_CanExecuteChanged;
                Resource = resource;
            }

            void commandCore_CanExecuteChanged(object sender, EventArgs e)
            {
                if (CanExecuteChanged != null)
                {
                    this.CanExecuteChanged(this, e);
                }

            }


            /// <summary>
            /// ICommand核心
            /// </summary>
            public TCommand CommandCore
            {
                get;
                private set;

            }

            //public CommandModel<TCommand, TResource> ConfigCommandCore(Action<TCommand> commandConfigAction)
            //{
            //    commandConfigAction(CommandCore);
            //    return this;
            //}


            /// <summary>
            /// 上一次是否能够运行的值
            /// </summary>
            public bool LastCanExecuteValue
            {
                get { return _LastCanExecuteValueLocator(this).Value; }
                set { _LastCanExecuteValueLocator(this).SetValueAndTryNotify(value); }
            }


            #region Property bool LastCanExecuteValue Setup

            protected Property<bool> _LastCanExecuteValue =
              new Property<bool> { LocatorFunc = _LastCanExecuteValueLocator };
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            static Func<BindableBase, ValueContainer<bool>> _LastCanExecuteValueLocator =
                RegisterContainerLocator<bool>(
                "LastCanExecuteValue",
                model =>
                {
                    model._LastCanExecuteValue =
                        model._LastCanExecuteValue
                        ??
                        new Property<bool> { LocatorFunc = _LastCanExecuteValueLocator };
                    return model._LastCanExecuteValue.Container =
                        model._LastCanExecuteValue.Container
                        ??
                        new ValueContainer<bool>("LastCanExecuteValue", model);
                });

            #endregion



            /// <summary>
            /// 资源
            /// </summary>
            public TResource Resource
            {
                get { return _ResourceLocator(this).Value; }
                set { _ResourceLocator(this).SetValueAndTryNotify(value); }
            }


            #region Property TResource Resource Setup

            protected Property<TResource> _Resource =
              new Property<TResource> { LocatorFunc = _ResourceLocator };
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            static Func<BindableBase, ValueContainer<TResource>> _ResourceLocator =
                RegisterContainerLocator<TResource>(
                "Resource",
                model =>
                {
                    model._Resource =
                        model._Resource
                        ??
                        new Property<TResource> { LocatorFunc = _ResourceLocator };
                    return model._Resource.Container =
                        model._Resource.Container
                        ??
                        new ValueContainer<TResource>("Resource", model);
                });

            #endregion











            /// <summary>
            /// 判断是否可执行
            /// </summary>
            /// <param name="parameter">指定参数</param>
            /// <returns></returns>
            public bool CanExecute(object parameter)
            {
                var s = CommandCore.CanExecute(parameter);
                LastCanExecuteValue = s;
                return s;
            }

            public event EventHandler CanExecuteChanged;

            /// <summary>
            /// 执行
            /// </summary>
            /// <param name="parameter">指定参数</param>
            public void Execute(object parameter)
            {
                CommandCore.Execute(parameter);
            }
        }

        /// <summary>
        /// 可绑定的CommandVM 扩展方法集
        /// </summary>
        public static class CommandModelExtensions
        {
            /// <summary>
            /// 根据ICommand实例创建CommandModel
            /// </summary>
            /// <typeparam name="TCommand">ICommand实例的具体类型</typeparam>
            /// <typeparam name="TResource">附加资源类型</typeparam>
            /// <param name="command">ICommand实例</param>
            /// <param name="resource">资源实例</param>
            /// <returns>CommandModel实例</returns>
            public static CommandModel<TCommand, TResource> CreateCommandModel<TCommand, TResource>(this TCommand command, TResource resource)
                where TCommand : ICommand
            {
                return new CommandModel<TCommand, TResource>(command, resource);
            }

            /// <summary>
            /// 据ICommand实例创建不具备/弱类型资源的CommandModel
            /// </summary>
            /// <typeparam name="TCommand">ICommand实例的具体类型</typeparam>
            /// <param name="command">ICommand实例</param>
            /// <param name="resource">资源实例</param>
            /// <returns>CommandModel实例</returns>
            public static CommandModel<TCommand, object> CreateCommandModel<TCommand>(this TCommand command, object resource = null)
            where TCommand : ICommand
            {
                return new CommandModel<TCommand, object>(command, null);
            }

            /// <summary>
            /// 为CommandModel指定ViewModel
            /// </summary>
            /// <typeparam name="TCommand">ICommand实例的具体类型</typeparam>
            /// <typeparam name="TResource">附加资源类型</typeparam>
            /// <param name="cmdModel">CommandModel具体实例</param>
            /// <param name="viewModel">ViewModel具体实例</param>
            /// <returns></returns>
            public static CommandModel<TCommand, TResource> WithViewModel<TCommand, TResource>(this CommandModel<TCommand, TResource> cmdModel, BindableBase viewModel)
                where TCommand : ICommand
            {
                //cmdModel.
                var cmd2 = cmdModel.CommandCore as ICommandWithViewModel;
                if (cmd2 != null)
                {
                    cmd2.ViewModel = viewModel;
                }
                return cmdModel;
            }
        }





    }


    namespace EventRouter
    {

        /// <summary>
        /// 全局事件根
        /// </summary>
        public class EventRouter
        {
            protected EventRouter()
            {

            }
            static EventRouter()
            {
                Instance = new EventRouter();
            }

            public static EventRouter Instance { get; protected set; }




            /// <summary>
            /// 触发事件    
            /// </summary>
            /// <typeparam name="TEventArgs">事件数据类型</typeparam>
            /// <param name="sender">事件发送者</param>
            /// <param name="eventArgs">事件数据</param>
            /// <param name="callerMemberName">发送事件名</param>
            public virtual void RaiseEvent<TEventArgs>(object sender, TEventArgs eventArgs, string callerMemberName = "")
            {
                var eventObject = GetIEventObjectInstance(typeof(TEventArgs));
                eventObject.RaiseEvent(sender, callerMemberName, eventArgs);

                while (eventObject.BaseArgsTypeInstance != null)
                {
                    eventObject = eventObject.BaseArgsTypeInstance;
                }
            }


            /// <summary>
            /// 取得独立事件类
            /// </summary>
            /// <typeparam name="TEventArgs">事件数据类型</typeparam>
            /// <returns>事件独立类</returns>
            public virtual EventObject<TEventArgs> GetEventObject<TEventArgs>()
#if !NETFX_CORE
 where TEventArgs : EventArgs
#endif
            {
                var eventObject = (EventObject<TEventArgs>)GetIEventObjectInstance(typeof(TEventArgs));

                return eventObject;

            }

            /// <summary>
            /// 事件来源的代理对象实例
            /// </summary>
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
            static protected readonly ConcurrentDictionary<Type, IEventObject> EventObjects
     = new ConcurrentDictionary<Type, IEventObject>();
            /// <summary>
            /// 创建事件代理对象
            /// </summary>
            /// <param name="argsType">事件数据类型</param>
            /// <returns>代理对象实例</returns>
            static protected IEventObject GetIEventObjectInstance(Type argsType)
            {

                var rval = EventObjects.GetOrAdd(
                    argsType,
                    t =>
                        Activator.CreateInstance(typeof(EventObject<>).MakeGenericType(t)) as IEventObject
                    );

                if (rval.BaseArgsTypeInstance == null)
                {
#if NETFX_CORE
                    var baseT = argsType.GetTypeInfo().BaseType;
                    if (baseT != typeof(object) && baseT.Name != "RuntimeClass")
#else
                    var baseT = argsType.BaseType;
                    if (baseT != typeof(object))
#endif
                    {
                        rval.BaseArgsTypeInstance = GetIEventObjectInstance(baseT);
                    }

                }

                return rval;
            }


            /// <summary>
            /// 事件对象接口
            /// </summary>
            protected interface IEventObject
            {
                IEventObject BaseArgsTypeInstance { get; set; }
                void RaiseEvent(object sender, string eventName, object args);
            }




            /// <summary>
            ///事件对象
            /// </summary>
            /// <typeparam name="TEventArgs"></typeparam>
            public class EventObject<TEventArgs> : IEventObject
#if !NETFX_CORE
 where TEventArgs : EventArgs
#endif

            {
                public EventObject()
                {
                }


                IEventObject IEventObject.BaseArgsTypeInstance
                {
                    get;
                    set;
                }

                void IEventObject.RaiseEvent(object sender, string eventName, object args)
                {
                    RaiseEvent(sender, eventName, args);
                }

                public void RaiseEvent(object sender, string eventName, object args)
                {


                    var a = args;
                    if (a != null && Event != null)
                    {
                        Event(sender, new RouterEventData<TEventArgs>(sender, eventName, (TEventArgs)args));
                    }
                }

                public event EventHandler<RouterEventData<TEventArgs>> Event;

            }


        }
        /// <summary>
        /// 导航事件数据
        /// </summary>
        public class NavigateCommandEventArgs : EventArgs
        {
            public NavigateCommandEventArgs()
            {
                ParameterDictionary = new Dictionary<string, object>();
            }
            public NavigateCommandEventArgs(IDictionary<string, object> dic)
                : this()
            {
                foreach (var item in dic)
                {

                    (ParameterDictionary as IDictionary<string, object>)[item.Key] = item.Value;
                }

            }
            public Dictionary<string, object> ParameterDictionary { get; set; }

            public Type SourceViewType { get; set; }

            public Type TargetViewType { get; set; }

            public IViewModel ViewModel { get; set; }

            public Object TargetFrame { get; set; }
        }

        /// <summary>
        /// 保存状态事件数据
        /// </summary>
        public class SaveStateEventArgs : EventArgs
        {
            public string ViewKeyId { get; set; }
            public Dictionary<string, object> State { get; set; }
        }

        /// <summary>
        /// 事件路由的扩展方法集合
        /// </summary>
        public static class EventRouterHelper
        {
            /// <summary>
            /// 触发事件
            /// </summary>
            /// <typeparam name="TEventArgs">事件类型</typeparam>
            /// <param name="source">事件来源</param>
            /// <param name="eventArgs">事件数据</param>
            /// <param name="callerMemberName">事件名</param>
            public static void RaiseEvent<TEventArgs>(this BindableBase source, TEventArgs eventArgs, string callerMemberName = "")
            {
                EventRouter.Instance.RaiseEvent(source, eventArgs, callerMemberName);
            }

        }

        /// <summary>
        /// 事件信息
        /// </summary>
        /// <typeparam name="TEventArgs">事件数据类型</typeparam>
        public class RouterEventData<TEventArgs>
#if ! NETFX_CORE
 : EventArgs
                where TEventArgs : EventArgs
#endif

        {
            public RouterEventData(object sender, string eventName, TEventArgs eventArgs)
            {

                Sender = sender;
                EventName = eventName;
                EventArgs = eventArgs;
            }
            /// <summary>
            /// 事件发送者
            /// </summary>
            public Object Sender { get; private set; }
            /// <summary>
            /// 事件名
            /// </summary>
            public string EventName { get; private set; }
            /// <summary>
            /// 事件数据
            /// </summary>
            public TEventArgs EventArgs { get; private set; }
        }

    }


    namespace Commands
    {
        /// <summary>
        /// Command被运行触发的事件数据类型
        /// </summary>
        public class EventCommandEventArgs : EventArgs
        {
            public Object Parameter { get; set; }
            public Object ViewModel { get; set; }

            public static EventCommandEventArgs Create(Object parameter, Object viewModel)
            {

                return new EventCommandEventArgs { Parameter = parameter, ViewModel = viewModel };

            }
        }

        /// <summary>
        /// 事件Command的助手类
        /// </summary>
        public static class EventCommandHelper
        {
            /// <summary>
            /// 为一个事件Command制定一个VM
            /// </summary>
            /// <typeparam name="TCommand">事件Command具体类型</typeparam>
            /// <param name="cmd">事件Command实例</param>
            /// <param name="viewModel">VM实例</param>
            /// <returns>事件Command实例本身</returns>
            public static TCommand WithViewModel<TCommand>(this TCommand cmd, BindableBase viewModel)
                where TCommand : EventCommandBase
            {
                cmd.ViewModel = viewModel;
                return cmd;
            }

        }

        /// <summary>
        /// 带有VM的Command接口
        /// </summary>
        public interface ICommandWithViewModel : ICommand
        {
            BindableBase ViewModel { get; set; }
        }

        /// <summary>
        /// 事件Command,运行后马上触发一个事件，事件中带有Command实例和VM实例属性
        /// </summary>
        public abstract class EventCommandBase : ICommandWithViewModel
        {
            /// <summary>
            /// VM
            /// </summary>
            public BindableBase ViewModel { get; set; }

            /// <summary>
            /// 运行时触发的事件
            /// </summary>
            public event EventHandler<EventCommandEventArgs> CommandExecute;
            /// <summary>
            /// 执行时的逻辑
            /// </summary>
            /// <param name="args">执行时的事件数据</param>
            protected virtual void OnCommandExecute(EventCommandEventArgs args)
            {
                if (CommandExecute != null)
                {
                    CommandExecute(this, args);
                }
            }


            /// <summary>
            /// 该Command是否能执行
            /// </summary>
            /// <param name="parameter">判断参数</param>
            /// <returns>是否</returns>
            public abstract bool CanExecute(object parameter);

            /// <summary>
            /// 是否能执行的值产生变化的事件
            /// </summary>
            public event EventHandler CanExecuteChanged;

            /// <summary>
            /// 是否能执行变化时触发事件的逻辑
            /// </summary>
            protected void OnCanExecuteChanged()
            {
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }

            /// <summary>
            /// 执行Command
            /// </summary>
            /// <param name="parameter">参数条件</param>
            public virtual void Execute(object parameter)
            {
                if (CanExecute(parameter))
                {
                    OnCommandExecute(EventCommandEventArgs.Create(parameter, ViewModel));
                }
            }
        }


    }


    //    namespace ValueConverters
    //    {


    //        public class GenericValueConverter<TSource, TTarget, TParemeter> : IValueConverter
    //        {
    //            public GenericValueConverter()
    //            {

    //            }

    //            public GenericValueConverter(
    //                Func<TSource, TParemeter, string, TTarget> converter,

    //                Func<TTarget, TParemeter, string, TSource> convertBacker
    //                )
    //            {
    //                Converter = converter;
    //                ConvertBacker = convertBacker;
    //            }
    //            public object Convert(object value, Type targetType, object parameter, string language)
    //            {
    //                if (Converter == null)
    //                {
    //                    throw new NotImplementedException();
    //                }
    //                OnConvertCheckInputType(value, targetType);


    //                return Converter((TSource)value, (TParemeter)parameter, language);
    //            }

    //            public Func<TSource, TParemeter, string, TTarget> Converter { get; set; }

    //            public Func<TTarget, TParemeter, string, TSource> ConvertBacker { get; set; }




    //            public object ConvertBack(object value, Type targetType, object parameter, string language)
    //            {

    //                if (ConvertBacker == null)
    //                {
    //                    throw new NotImplementedException();
    //                }

    //                OnConvertBackCheckInputType(value, targetType);
    //                return ConvertBacker((TTarget)value, (TParemeter)parameter, language);
    //            }



    //            private static void OnConvertCheckInputType(object sourceValue, Type targetType)
    //            {
    //#if NETFX_CORE
    //                if (!targetType.GetTypeInfo().IsAssignableFrom(typeof(TTarget).GetTypeInfo()))
    //                {
    //                    throw new ArgumentOutOfRangeException(string.Format("Target type is not supported.  {0} and its base class type would be fine.", typeof(TTarget).FullName));
    //                }
    //#else
    //                if (!targetType.IsAssignableFrom(typeof(TTarget)))
    //                {
    //                    throw new ArgumentOutOfRangeException(string.Format("Target type is not supported.  {0} and its base class type would be fine.", typeof(TTarget).FullName));
    //                }
    //#endif
    //                if (!(sourceValue is TSource))
    //                {
    //                    throw new ArgumentOutOfRangeException(string.Format("Source type is expected source type. A {0} reference is expected.", typeof(TSource).FullName));
    //                }
    //            }

    //            private static void OnConvertBackCheckInputType(object backingValue, Type backType)
    //            {
    //                if (typeof(TSource) != backType)
    //                {
    //                    throw new ArgumentOutOfRangeException(string.Format("Target type is not supported.  {0} is expected.", typeof(TSource).FullName));
    //                }
    //                if (!(backingValue is TTarget))
    //                {
    //                    throw new ArgumentOutOfRangeException(string.Format("Source type is expected source type. A {0} reference is expected.", typeof(TTarget).FullName));
    //                }
    //            }


    //#if NETFX_CORE
    //#else
    //            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //            {
    //                return Convert(value, targetType, parameter, culture.EnglishName);
    //            }

    //            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //            {
    //                return Convert(value, targetType, parameter, culture.EnglishName);
    //            }
    //#endif
    //        }


    //        public enum ErrorInfoTextConverterOptions
    //        {
    //            ErrorOnly,
    //            ErrorWithFieldsErrors

    //        }



    //        public class ViewModelDataErrorInfoTextConverter : GenericValueConverter<IBindable, string, ErrorInfoTextConverterOptions>
    //        {
    //            //public ViewModelDataErrorInfoTextConverter()
    //            //{
    //            //    Converter = (val, options, lan) =>
    //            //        {
    //            //            var dataError = val as IDataErrorInfo;
    //            //            switch (options)
    //            //            {


    //            //                case ErrorInfoTextConverterOptions.ErrorWithFieldsErrors:
    //            //                    var sb = new StringBuilder();
    //            //                    sb.AppendLine(val.Error);
    //            //                    foreach (var fn in val.GetFieldNames().ToArray())
    //            //                    {
    //            //                        sb.Append("\t").Append(fn).Append(":\t").AppendLine(dataError[fn]);
    //            //                    }
    //            //                    return sb.ToString();

    //            //                case ErrorInfoTextConverterOptions.ErrorOnly:
    //            //                default:
    //            //                    return val.Error;
    //            //            }
    //            //        };



    //            //}

    //        }




    //    }


    namespace Reactive
    {

        public static class EventTuple
        {
            public static EventTuple<TSource, TEventArgs> Create<TSource, TEventArgs>(TSource source, TEventArgs eventArgs)
            {
                return new EventTuple<TSource, TEventArgs> { Source = source, EventArgs = eventArgs };
            }

        }
        public struct EventTuple<TSource, TEventArgs>
        {
            public TSource Source { get; set; }
            public TEventArgs EventArgs { get; set; }
        }

        public static class MVVMRxExtensions
        {

            /// <summary>
            /// <para>Create a instance of IObservable that fires when property changed event is raised.</para>
            /// <para>创建一个监视属性变化事件观察者IObservable实例。</para>
            /// </summary>
            /// <returns></returns>
            public static IObservable<EventPattern<PropertyChangedEventArgs>> CreatePropertyChangedObservable(this BindableBase bindable)
            {
                return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                        eh => bindable.PropertyChanged += eh,
                        eh => bindable.PropertyChanged -= eh
                    )
                    .Where(_ => bindable.IsNotificationActivated);
            }


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
            public static IObservable<EventTuple<ValueContainer<TValue>, TValue>> GetNewValueObservable<TValue>
                (
                    this ValueContainer<TValue> source

                )
            {

                return Observable.FromEventPattern<EventHandler<ValueChangedEventArgs<TValue>>, ValueChangedEventArgs<TValue>>(
                        eh => source.ValueChanged += eh,
                        eh => source.ValueChanged -= eh)
                        .Select(
                            x => EventTuple.Create(source, x.EventArgs.NewValue)

                        );

            }

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


            public static IObservable<object> GetNullObservable<TValue>(this ValueContainer<TValue> source)
            {

                var eventArgSeq = Observable.FromEventPattern<EventHandler<ValueChangedEventArgs<TValue>>, ValueChangedEventArgs<TValue>>(
                        eh => source.ValueChanged += eh,
                        eh => source.ValueChanged -= eh);
                return eventArgSeq.Select(
                            x => null as object
                        );
                ;
            }


            public static IObservable<RouterEventData<TEventArgs>>
                GetRouterEventObservable<TEventArgs>(this MVVMSidekick.EventRouter.EventRouter.EventObject<TEventArgs> source)
#if !NETFX_CORE
 where TEventArgs : EventArgs
#endif
            {

                var eventArgSeq = Observable.FromEventPattern<EventHandler<RouterEventData<TEventArgs>>, RouterEventData<TEventArgs>>(
                  eh => source.Event += eh,
                  eh => source.Event -= eh)
                  .Select(e =>
                      e.EventArgs);



                return eventArgSeq;

            }
            /// <summary>
            /// Bind Command to IsUIBusy property.
            /// </summary>
            /// <typeparam name="TCommand">A sub class of ReactiveCommand</typeparam>
            /// <typeparam name="TResource">The resource type of CommandModel</typeparam>
            /// <typeparam name="TViewModel">The View Model type command wanna bind to</typeparam>
            /// <param name="command">Command itself</param>
            /// <param name="model">The View Model  command wanna bind to</param>
            /// <returns>command instance itself</returns>
            public static CommandModel<TCommand, TResource> ListenToIsUIBusy<TCommand, TResource, TViewModel>(this CommandModel<TCommand, TResource> command, ViewModelBase<TViewModel> model, bool canExecuteWhenBusy = false)
                where TViewModel : ViewModelBase<TViewModel>
                where TCommand : ReactiveCommand
            {

                //See Test  CommandListenToUIBusy_Test
                model.GetValueContainer(x => x.IsUIBusy).GetNewValueObservable()
                  .Select(e => !(canExecuteWhenBusy ^ e.EventArgs))
                  .Subscribe(command.CommandCore.CanExecuteObserver)
                  .DisposeWith(model);

                return command;
            }

        }



        public class ReactiveCommand : EventCommandBase, ICommand, IObservable<EventPattern<EventCommandEventArgs>>
        {



            protected Lazy<IObservable<EventPattern<EventCommandEventArgs>>> _LazyObservableExecute;
            protected Lazy<IObserver<bool>> _LazyObserverCanExecute;
            protected bool _CurrentCanExecuteObserverValue;

            protected ReactiveCommand()
            {
                ConfigReactive();

            }

            public ReactiveCommand(bool canExecute = false)
                : this()
            {
                _CurrentCanExecuteObserverValue = canExecute;
            }


            virtual protected void ConfigReactive()
            {
                _LazyObservableExecute = new Lazy<IObservable<EventPattern<EventCommandEventArgs>>>
                (
                    () =>
                    {
                        var ob = Observable.FromEventPattern<EventHandler<EventCommandEventArgs>, EventCommandEventArgs>
                    (
                        eh =>
                        {
                            this.CommandExecute += eh;
                        },
                        eh =>
                        {
                            this.CommandExecute -= eh;
                        }
                    );

                        return ob;
                    }
                );

                _LazyObserverCanExecute = new Lazy<IObserver<bool>>
                (
                    () =>
                        Observer.Create<bool>(
                        canExe =>
                        {
                            var oldv = this._CurrentCanExecuteObserverValue;
                            _CurrentCanExecuteObserverValue = canExe;
                            if (oldv != canExe)
                            {
                                OnCanExecuteChanged();
                            }
                        }
                        )

                );
            }
            public IObserver<bool> CanExecuteObserver { get { return _LazyObserverCanExecute.Value; } }

            public override bool CanExecute(object parameter)
            {
                return _CurrentCanExecuteObserverValue;
            }






            public IDisposable Subscribe(IObserver<EventPattern<EventCommandEventArgs>> observer)
            {
                return _LazyObservableExecute
                      .Value
                      .Subscribe(observer);
            }
        }


    }

    namespace Views
    {


        public static class ViewHelper
        {
            internal static PropertyChangedCallback ViewModelChangedCallback
                = (o, e) =>
                    {
                        dynamic item = o;
                        ((o as IView).Content as FrameworkElement).DataContext = e.NewValue;
                        var nv = e.NewValue as IViewModel;
                        var ov = e.OldValue as IViewModel;
                        if (ov != null)
                        {
                            ov.OnUnbindedFromView(o as IView, nv);
                        }
                        if (nv != null)
                        {
                            nv.OnBindedToView(o as IView, ov);
                        }

                    };

            internal static FrameworkElement CheckContent(this IView control)
            {
                var c = (control.Content as FrameworkElement);
                if (c == null)
                {
                    control.Content = c = new Grid();
                }
                return c;
            }

            public static void SelfClose(this IView view)
            {
                view.ViewModel = null;
                if (view is UserControl || view is Page)
                {
                    var viewElement = view as FrameworkElement;
                    var parent = viewElement.Parent;
                    if (parent is Panel)
                    {
                        (parent as Panel).Children.Remove(viewElement);
                    }
                    else if (parent is Frame)
                    {
                        var f = (parent as Frame);
                        if (f.CanGoBack)
                        {
                            f.GoBack();
                        }
                        else
                        {
                            f.Content = null;
                        }
                    }
                    else if (parent is ContentControl)
                    {
                        (parent as ContentControl).Content = null;
                    }
                    else if (parent is Page)
                    {
                        (parent as Page).Content = null;
                    }
                    else if (parent is UserControl)
                    {
                        (parent as UserControl).Content = null;
                    }

                }
#if WPF
                else if (view is Window)
                {
                    (view as Window).Close();
                }
#endif
                else
                {
                    view.Dispose();
                }



            }

        }
#if WPF

#elif NETCORE_FX
#elif WINDOWS_PHONE_7||WINDOWS_PHONE_8||WINDOWS_PHONE_7
#endif
#if WPF
        public class MVVMWindow : Window, IView
        {

            public MVVMWindow()
                : this(null)
            {
                //ViewModel = new DefaultViewModel ();
            }

            public MVVMWindow(IViewModel viewModel)
            {

                Loaded += async (_1, _2) =>
                {
                    if (viewModel != null)
                    {
                        if (!object.ReferenceEquals(ViewModel, viewModel))
                        {
                            ViewModel = viewModel;
                        }
                    }
                    await ViewModel.OnBindedViewLoad(this);
                };
            }



            public IViewModel ViewModel
            {
                get
                {
                    var rval = GetValue(ViewModelProperty) as IViewModel;
                    var c = this.CheckContent();
                    if (rval == null)
                    {

                        rval = c.DataContext as IViewModel;
                        SetValue(ViewModelProperty, rval);

                    }
                    else
                    {

                        if (!Object.ReferenceEquals(c.DataContext, rval))
                        {
                            c.DataContext = rval;
                        }
                    }
                    return rval;
                }
                set
                {
                    SetValue(ViewModelProperty, value);
                    var c = this.CheckContent();
                    c.DataContext = value;

                }
            }



            // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register("ViewModel", typeof(IViewModel), typeof(MVVMWindow), new PropertyMetadata(null, ViewHelper.ViewModelChangedCallback));


            public ViewType ViewType
            {
                get { return ViewType.Window; }
            }
            public void Dispose()
            {
                this.SelfClose();
            }



        }




#endif

#if WINDOWS_PHONE_7||WINDOWS_PHONE_8
        public class MVVMPage : PhoneApplicationPage, IView
#else
        public class MVVMPage : Page, IView
#endif
        {

            public MVVMPage()
                : this(null)
            {

            }

            public MVVMPage(IViewModel viewModel)
            {
                Loaded += async (_1, _2) =>
                    {
                        if (viewModel != null)
                        {
                            if (!object.ReferenceEquals(ViewModel, viewModel))
                            {
                                ViewModel = viewModel;
                            }
                        }
                        await ViewModel.OnBindedViewLoad(this);
                    };
            }




#if WINDOWS_PHONE_7||WINDOWS_PHONE_8||SILVERLIGHT_5||NETFX_CORE
            object IView.Content
            {
                get { return Content; }
                set { Content = value as FrameworkElement; }

            }

            protected override void OnNavigatedTo(NavigationEventArgs e)
            {
                base.OnNavigatedTo(e);
                RoutedEventHandler loadEvent = null;

                loadEvent = (_1, _2) =>
                {
                    EventRouter.EventRouter.Instance.RaiseEvent(this, e);
                    this.Loaded -= loadEvent;
                };
                this.Loaded += loadEvent;
            }
#endif






            public IViewModel ViewModel
            {
                get
                {
                    var rval = GetValue(ViewModelProperty) as IViewModel;
                    var c = this.CheckContent();
                    if (rval == null)
                    {

                        rval = c.DataContext as IViewModel;
                        SetValue(ViewModelProperty, rval);

                    }
                    else
                    {

                        if (!Object.ReferenceEquals(c.DataContext, rval))
                        {
                            c.DataContext = rval;
                        }
                    }
                    return rval;
                }
                set
                {

                    SetValue(ViewModelProperty, value);
                    var c = this.CheckContent();
                    c.DataContext = value;

                }
            }

            // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register("ViewModel", typeof(IViewModel), typeof(MVVMPage), new PropertyMetadata(null, ViewHelper.ViewModelChangedCallback));




            public ViewType ViewType
            {
                get { return ViewType.Page; }
            }

            public void Dispose()
            {

                this.SelfClose();


            }
        }



        public class MVVMControl : UserControl, IView
        {

            public MVVMControl()
                : this(null)
            {


            }
            public MVVMControl(IViewModel viewModel)
            {
                Loaded += async (_1, _2) =>
                {
                    if (viewModel != null)
                    {
                        if (!object.ReferenceEquals(ViewModel, viewModel))
                        {
                            ViewModel = viewModel;
                        }
                    }
                    await ViewModel.OnBindedViewLoad(this);
                };
            }
#if WINDOWS_PHONE_7||WINDOWS_PHONE_8||SILVERLIGHT_5||NETFX_CORE
            object IView.Content
            {
                get { return Content; }
                set { Content = value as FrameworkElement; }

            }
#endif
            //public MVVMPage WarpSelfByNewPage()
            //{
            //    lock (this)
            //    {
            //        if (this.IsWarppedToPage)
            //        {
            //            throw new InvalidOperationException("This  WarpSelfByNewPage() or WarpSelfByPage()functions can only be called by one time");
            //        }
            //        MVVMPage page = new MVVMPage(ViewModel);
            //        page.DisposeWith(ViewModel);
            //        page.Content = this;
            //        this.IsWarppedToPage = true;
            //        return page;
            //    }
            //}

            //public void WarpSelfByPage(MVVMPage page)
            //{
            //    lock (this)
            //    {
            //        if (this.IsWarppedToPage)
            //        {
            //            throw new InvalidOperationException("This WarpSelfByNewPage() or WarpSelfByPage()functions can only be called by one time");
            //        }
            //        page.ViewModel = ViewModel;
            //        page.DisposeWith(ViewModel);
            //        page.Content = this;
            //        this.IsWarppedToPage = true;

            //    }
            //}

            //public Page ParentPage
            //{
            //    get { return Parent as Page; }

            //}
            //public bool IsWarppedToPage { get; protected set; }

            public IViewModel ViewModel
            {
                get
                {
                    var rval = GetValue(ViewModelProperty) as IViewModel;
                    var c = this.CheckContent();
                    if (rval == null)
                    {

                        rval = c.DataContext as IViewModel;
                        SetValue(ViewModelProperty, rval);

                    }
                    else
                    {

                        if (!Object.ReferenceEquals(c.DataContext, rval))
                        {
                            c.DataContext = rval;
                        }
                    }
                    return rval;
                }
                set
                {
                    SetValue(ViewModelProperty, value);
                    var c = this.CheckContent();
                    c.DataContext = value;

                }
            }

            // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register("ViewModel", typeof(IViewModel), typeof(MVVMControl), new PropertyMetadata(null, ViewHelper.ViewModelChangedCallback));


            public ViewType ViewType
            {
                get { return ViewType.Control; }
            }
            public void Dispose()
            {
                this.SelfClose();
            }
        }
        public enum ViewType
        {
            Page,
            Window,
            Control
        }

        public interface IView : IDisposable
        {
            IViewModel ViewModel { get; set; }

            ViewType ViewType { get; }

            Object Content { get; set; }

            DependencyObject Parent { get; }

        }


        public interface IView<TViewModel> : IView, IDisposable where TViewModel : IViewModel
        {
            TViewModel SpecificTypedViewModel { get; set; }
        }

        public struct ViewModelToViewMapper<TModel>
            where TModel : BindableBase
        {

#if WPF
            public ViewModelToViewMapper<TModel> MapToDefault<TView>(TView instance) where TView : class,IView
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(instance);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapTo<TView>(string viewName, TView instance) where TView : class,IView
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewName, instance);
                return this;
            }


            public ViewModelToViewMapper<TModel> MapToDefault<TView>(bool alwaysNew = true) where TView : class,IView
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => (TView)Activator.CreateInstance(typeof(TView), d as object), alwaysNew);
                return this;
            }
            public ViewModelToViewMapper<TModel> MapTo<TView>(string viewName, bool alwaysNew = true) where TView : class,IView
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewName, d => (TView)Activator.CreateInstance(typeof(TView), d as object), alwaysNew);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapToDefault<TView>(Func<TModel, TView> factory, bool alwaysNew = true) where TView : class,IView
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => factory(d as TModel), alwaysNew);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapTo<TView>(string viewName, Func<TModel, TView> factory, bool alwaysNew = true) where TView : class,IView
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewName, d => factory(d as TModel), alwaysNew);
                return this;
            }
#else
            public ViewModelToViewMapper<TModel> MapToDefaultControl<TControl>(TControl instance) where TControl : MVVMControl
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(instance);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapToControl<TControl>(string viewName, TControl instance) where TControl : MVVMControl
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewName, instance);
                return this;
            }


            public ViewModelToViewMapper<TModel> MapToDefaultControl<TControl>(bool alwaysNew = true) where TControl : MVVMControl
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => (TControl)Activator.CreateInstance(typeof(TControl), d as object), alwaysNew);
                return this;
            }
            public ViewModelToViewMapper<TModel> MapToControl<TControl>(string viewName, bool alwaysNew = true) where TControl : MVVMControl
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewName, d => (TControl)Activator.CreateInstance(typeof(TControl), d as object), alwaysNew);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapToDefaultControl<TControl>(Func<TModel, TControl> factory, bool alwaysNew = true) where TControl : MVVMControl
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(null, d => factory(d as TModel), alwaysNew);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapToControl<TControl>(string viewName, Func<TModel, TControl> factory, bool alwaysNew = true) where TControl : MVVMControl
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewName, d => factory(d as TModel), alwaysNew);
                return this;
            }
#endif

#if WINDOWS_PHONE_8||WINDOWS_PHONE_7||SILVERLIGHT_5
            private static Uri GuessViewUri<TPage>(Uri baseUri) where TPage : MVVMPage
            {

                baseUri = baseUri ?? new Uri("/", UriKind.Relative);


                if (baseUri.IsAbsoluteUri)
                {
                    var path = Path.Combine(baseUri.LocalPath, typeof(TPage).Name + ".xaml");
                    UriBuilder ub = new UriBuilder(baseUri);
                    ub.Path = path;
                    return ub.Uri;
                }
                else
                {
                    var path = Path.Combine(baseUri.OriginalString, typeof(TPage).Name + ".xaml");
                    var pageUri = new Uri(path, UriKind.Relative);
                    return pageUri;
                }
            }

            public ViewModelToViewMapper<TModel> MapToDefault<TPage>(Uri baseUri = null) where TPage : MVVMPage
            {

                var pageUri = GuessViewUri<TPage>(baseUri);
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(pageUri);
                return this;
            }




            public ViewModelToViewMapper<TModel> MapTo<TPage>(string viewName, Uri baseUri = null) where TPage : MVVMPage
            {
                var pageUri = GuessViewUri<TPage>(baseUri);
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewName, pageUri);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapToDefault(Uri pageUri)
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(pageUri);
                return this;
            }

            public ViewModelToViewMapper<TModel> MapTo(string viewName, Uri pageUri)
            {
                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewName, pageUri);
                return this;
            }
#endif
#if NETFX_CORE



            public ViewModelToViewMapper<TModel> MapToDefault<TPage>() where TPage : MVVMPage
            {


                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(typeof(TPage));
                return this;
            }


            public ViewModelToViewMapper<TModel> MapToDefault<TPage>(string viewName) where TPage : MVVMPage
            {

                ViewModelToViewMapperServiceLocator<TModel>.Instance.Register(viewName, typeof(TPage));
                return this;
            }



#endif


        }

        public static class ViewModelToViewMapperExtensions
        {

            public static ViewModelToViewMapper<TViewModel> GetViewMapper<TViewModel>(this MVVMSidekick.Services.ServiceLocatorEntryStruct<TViewModel> vmRegisterEntry)
                  where TViewModel : BindableBase
            {
                return new ViewModelToViewMapper<TViewModel>();
            }

        }
        public class ViewModelToViewMapperServiceLocator<TViewModel> : MVVMSidekick.Services.TypeSpecifiedServiceLocatorBase<ViewModelToViewMapperServiceLocator<TViewModel>, object>
        {
            static ViewModelToViewMapperServiceLocator()
            {
                Instance = new ViewModelToViewMapperServiceLocator<TViewModel>();
            }
            public static ViewModelToViewMapperServiceLocator<TViewModel> Instance { get; set; }


        }
        public class ViewModelLocator<TViewModel> : MVVMSidekick.Services.TypeSpecifiedServiceLocatorBase<ViewModelLocator<TViewModel>, TViewModel>
            where TViewModel : IViewModel
        {
            static ViewModelLocator()
            {
                Instance = new ViewModelLocator<TViewModel>();
            }
            public static ViewModelLocator<TViewModel> Instance { get; set; }

        }



        public class Stage : DependencyObject
        {
            public Stage(FrameworkElement target, string beaconKey, StageManager navigator)
            {
                Target = target;
                _navigator = navigator;
                BeaconKey = beaconKey;
                //SetupNavigateFrame();
            }

            StageManager _navigator;
            FrameworkElement _target;




            public Frame Frame
            {
                get { return (Frame)GetValue(FrameProperty); }
                private set { SetValue(FrameProperty, value); }
            }

            // Using a DependencyProperty as the backing store for Frame.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty FrameProperty =
                DependencyProperty.Register("Frame", typeof(Frame), typeof(Stage), new PropertyMetadata(null));



            public FrameworkElement Target
            {
                get { return _target; }
                private set
                {
                    _target = value;
                    Frame = _target as Frame;


                }
            }

            public bool IsGoBackSupported
            {
                get
                {
                    return Frame != null;
                }
            }


            public bool CanGoBack
            {
                get
                {
                    return IsGoBackSupported ? Frame.CanGoBack : false;
                }

            }




            public string BeaconKey
            {
                get { return (string)GetValue(BeaconKeyProperty); }
                private set { SetValue(BeaconKeyProperty, value); }
            }

            // Using a DependencyProperty as the backing store for BeaconKey.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty BeaconKeyProperty =
                DependencyProperty.Register("BeaconKey", typeof(string), typeof(Stage), new PropertyMetadata(""));


#if WPF
            private static IView InternalLocateViewIfNotSet<TTarget>(TTarget targetViewModel, string viewKey, IView view) where TTarget : class, IViewModel
            {
                if (targetViewModel != null && targetViewModel.StageManager != null)
                {
                    view = targetViewModel.StageManager.CurrentBindingView as IView;

                }
                view = view ?? ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewKey, targetViewModel) as IView;
                return view;
            }





            public async Task Show<TTarget>(TTarget targetViewModel = null, string viewKey = null)
                 where TTarget : class,IViewModel
            {
                IView view = null;
                view = InternalLocateViewIfNotSet<TTarget>(targetViewModel, viewKey, view);
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
                await targetViewModel.WaitForClose();
            }


            public async Task<TResult> Show<TTarget, TResult>(TTarget targetViewModel = null, string viewKey = null)
                where TTarget : class,IViewModel<TResult>
            {
                IView view = null;
                view = InternalLocateViewIfNotSet<TTarget>(targetViewModel, viewKey, view);
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
                return await targetViewModel.WaitForCloseWithResult();
            }



            public async Task<ShowAwaitableResult<TTarget>> ShowAndGetViewModel<TTarget>(TTarget targetViewModel = null, string viewKey = null)
                where TTarget : class,IViewModel
            {
                IView view = null;
                view = InternalLocateViewIfNotSet<TTarget>(targetViewModel, viewKey, view);

                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);


                return await TaskExHelper.FromResult(new ShowAwaitableResult<TTarget> { Closing = targetViewModel.WaitForClose(), ViewModel = targetViewModel });
            }
#endif
#if SILVERLIGHT_5||WINDOWS_PHONE_7||WINDOWS_PHONE_8

            public async Task Show<TTarget>(TTarget targetViewModel = null, string viewKey = null)
                 where TTarget : class,IViewModel
            {

                var item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewKey, targetViewModel);
                Uri uri;
                if ((uri = item as Uri) != null) //only sl like page Can be registered as uri
                {
                    Frame frame;
                    if ((frame = Target as Frame) != null)
                    {
                        var task = new Task(() => { });
                        var guid = Guid.NewGuid();
                        var newUriWithParameter = new Uri(uri.ToString() + "?CallBackGuid=" + guid.ToString(), UriKind.Relative);
                        using (EventRouter.EventRouter.Instance.GetEventObject<System.Windows.Navigation.NavigationEventArgs>()
                            .GetRouterEventObservable()
                            .Where(e =>
                                    e.EventArgs.Uri == newUriWithParameter)
                            .Subscribe(e =>
                            {
                                var page = e.Sender as MVVMPage;

                                if (targetViewModel != null) page.ViewModel = targetViewModel;
                                targetViewModel = (TTarget)page.ViewModel;
                                task.Start();
                            }
                            ))
                        {
                            frame.Navigate(newUriWithParameter);
                            await task;
                        }

                        await targetViewModel.WaitForClose();
                        return;
                    }

                }
                IView view = item as IView;
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
                await targetViewModel.WaitForClose();
            }

            public async Task<TResult> Show<TTarget, TResult>(TTarget targetViewModel = null, string viewKey = null)
                where TTarget : class,IViewModel<TResult>
            {

                var item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewKey, targetViewModel);
                Uri uri;
                if ((uri = item as Uri) != null) //only sl like page Can be registered as uri
                {
                    Frame frame;
                    if ((frame = Target as Frame) != null)
                    {
                        var task = new Task(() => { });
                        var guid = Guid.NewGuid();
                        var newUriWithParameter = new Uri(uri.ToString() + "?CallBackGuid=" + guid.ToString(), UriKind.Relative);
                        using (EventRouter.EventRouter.Instance.GetEventObject<System.Windows.Navigation.NavigationEventArgs>()
                            .GetRouterEventObservable()
                            .Where(e =>
                                    e.EventArgs.Uri == newUriWithParameter)
                            .Subscribe(e =>
                            {
                                var page = e.Sender as MVVMPage;
                                if (targetViewModel != null) page.ViewModel = targetViewModel;
                                targetViewModel = (TTarget)page.ViewModel;
                                task.Start();
                            }
                            ))
                        {
                            frame.Navigate(newUriWithParameter);
                            await task;
                        }

                        return await targetViewModel.WaitForCloseWithResult();
                    }

                }
                IView view = item as IView;
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
                return await targetViewModel.WaitForCloseWithResult();
            }



            public async Task<ShowAwaitableResult<TTarget>> ShowAndGetViewModel<TTarget>(TTarget targetViewModel = null, string viewKey = null)
                where TTarget : class,IViewModel
            {
                var item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewKey, targetViewModel);
                Uri uri;
                if ((uri = item as Uri) != null) //only sl like page Can be registered as uri
                {
                    Frame frame;
                    if ((frame = Target as Frame) != null)
                    {
                        var task = new Task(() => { });
                        var guid = Guid.NewGuid();
                        var newUriWithParameter = new Uri(uri.ToString() + "?CallBackGuid=" + guid.ToString(), UriKind.Relative);

                        using (EventRouter.EventRouter.Instance.GetEventObject<System.Windows.Navigation.NavigationEventArgs>()
                            .GetRouterEventObservable()
                            .Where(e =>
                                e.EventArgs.Uri.ToString().ToUpper() == newUriWithParameter.ToString().ToUpper())
                            .Subscribe(
                                e =>
                                {
                                    var page = e.Sender as MVVMPage;
                                    if (targetViewModel != null) page.ViewModel = targetViewModel;
                                    targetViewModel = (TTarget)page.ViewModel;
                                    task.Start();
                                }

                            ))
                        {
                            frame.Navigate(newUriWithParameter);
                            await task;
                        }

                        return new ShowAwaitableResult<TTarget> { Closing = targetViewModel.WaitForClose(), ViewModel = targetViewModel };

                    }

                }


                IView view = item as IView;
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);



                var tr = targetViewModel.WaitForClose();
                return new ShowAwaitableResult<TTarget> { Closing = targetViewModel.WaitForClose(), ViewModel = targetViewModel };
            }

#endif

#if NETFX_CORE


            public async Task Show<TTarget>(TTarget targetViewModel = null, string viewKey = null)
                 where TTarget : class,IViewModel
            {


                var item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewKey, targetViewModel);
                Type type;
                if ((type = item as Type) != null) //only MVVMPage Can be registered as Type
                {
                    Frame frame;

                    if ((frame = Target as Frame) != null)
                    {
                        Task task = new Task(() => { });
                        object paremeter = new object();


                        using (EventRouter.EventRouter.Instance.GetEventObject<NavigationEventArgs>()
                           .GetRouterEventObservable()
                           .Where(e =>
                                   object.ReferenceEquals(e.EventArgs.Parameter, paremeter))
                           .Subscribe(e =>
                           {
                               var page = e.Sender as MVVMPage;
                               if (targetViewModel != null) page.ViewModel = targetViewModel;
                               targetViewModel = (TTarget)page.ViewModel;
                               task.Start();
                           }))
                        {
                            frame.Navigate(type, paremeter);
                            await task;
                        }


                        await targetViewModel.WaitForClose();
                        return;
                    }

                }

                IView view = item as IView;
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
                await targetViewModel.WaitForClose();


            }

            public async Task<TResult> Show<TTarget, TResult>(TTarget targetViewModel = null, string viewKey = null)
                where TTarget : class,IViewModel<TResult>
            {
                var item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewKey, targetViewModel);
                Type type;
                if ((type = item as Type) != null) //only MVVMPage Can be registered as Type
                {
                    Frame frame;
                    if ((frame = Target as Frame) != null)
                    {
                        Task task = new Task(() => { });
                        object paremeter = new object();


                        using (EventRouter.EventRouter.Instance.GetEventObject<NavigationEventArgs>()
                           .GetRouterEventObservable()
                           .Where(e =>
                                   object.ReferenceEquals(e.EventArgs.Parameter, paremeter))
                           .Subscribe(e =>
                           {
                               var page = e.Sender as MVVMPage;
                               if (targetViewModel != null) page.ViewModel = targetViewModel;
                               targetViewModel = (TTarget)page.ViewModel;
                               task.Start();
                           }))
                        {
                            frame.Navigate(type, paremeter);
                            await task;
                        }




                        return await targetViewModel.WaitForCloseWithResult();
                    }

                }


                IView view = item as IView;
                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);
                return await targetViewModel.WaitForCloseWithResult();
            }



            public async Task<ShowAwaitableResult<TTarget>> ShowAndGetViewModel<TTarget>(TTarget targetViewModel = null, string viewKey = null)
    where TTarget : class,IViewModel
            {
                var item = ViewModelToViewMapperServiceLocator<TTarget>.Instance.Resolve(viewKey, targetViewModel);
                Type type;
                if ((type = item as Type) != null) //only MVVMPage Can be registered as Type
                {
                    Frame frame;
                    if ((frame = Target as Frame) != null)
                    {
                        Task task = new Task(() => { });
                        object paremeter = new object();


                        using (EventRouter.EventRouter.Instance.GetEventObject<NavigationEventArgs>()
                           .GetRouterEventObservable()
                           .Where(e =>
                                   object.ReferenceEquals(e.EventArgs.Parameter, paremeter))
                           .Subscribe(e =>
                           {
                               var page = e.Sender as MVVMPage;
                               if (targetViewModel != null) page.ViewModel = targetViewModel;
                               targetViewModel = (TTarget)page.ViewModel;
                               task.Start();
                           }))
                        {
                            frame.Navigate(type, paremeter);
                            await task;
                        }



                        return new ShowAwaitableResult<TTarget>
                        {
                            Closing = targetViewModel.WaitForClose(),
                            ViewModel = targetViewModel
                        };
                    }

                }


                IView view = item as IView;

                targetViewModel = targetViewModel ?? view.ViewModel as TTarget;
                InternalShowView(view, Target, _navigator.CurrentBindingView.ViewModel);


                var tr = targetViewModel.WaitForClose();
                return new ShowAwaitableResult<TTarget> { Closing = tr, ViewModel = targetViewModel };
            }
#endif



            private void InternalShowView(IView view, FrameworkElement target, IViewModel sourceVM)
            {

                if (view is UserControl || view is Page)
                {

                    if (target is ContentControl)
                    {
                        var targetCControl = target as ContentControl;
                        var oldcontent = targetCControl.Content as IDisposable;


                        targetCControl.Content = view;
                    }
                    else if (target is Panel)
                    {
                        var targetPanelControl = target as Panel;

                        targetPanelControl.Children.Add(view as UIElement);
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("This view {0} is not support show in {1} ", view.GetType(), target.GetType()));
                    }
                }
#if WPF
                else if (view is Window)
                {
                    var viewWindow = view as Window;
                    viewWindow.HorizontalAlignment = HorizontalAlignment.Center;
                    viewWindow.VerticalAlignment = VerticalAlignment.Center;
                    var targetWindow = target as Window;
                    if (targetWindow == null)
                    {
                        targetWindow = sourceVM.StageManager.CurrentBindingView as Window;

                    }

                    viewWindow.Owner = targetWindow;
                    viewWindow.Show();

                }
#endif
            }


        }

        /// <summary>
        /// The abstract  for frame/contentcontrol. VM can access this class to Show other vm and vm's mapped view.
        /// </summary>
        public class StageManager : DependencyObject, IDisposable
        {
            static StageManager()
            {
                NavigatorBeaconsKey = "NavigatorBeaconsKey";
            }
            public StageManager(IViewModel viewModel)
            {
                _ViewModel = viewModel;


            }
            IViewModel _ViewModel;

            /// <summary>
            /// This Key is a prefix for register keys. 
            /// The stage registeration store the String-Element-Mapping in view's Resource Dictionary(Resource property). 
            /// This can help not to overwrite the resources already defined.
            /// </summary>
            public static string NavigatorBeaconsKey;



            IView _CurrentBindingView;
            /// <summary>
            /// Get the currently binded view of this stagemanager. A stagemanager is for a certain view. If viewmodel is not binded to a view, the whole thing cannot work.
            /// </summary>
            public IView CurrentBindingView
            {
                get
                {

                    return _CurrentBindingView;
                }
                internal set
                {
                    _CurrentBindingView = value;
                }
            }







            public void InitParent(Func<DependencyObject> parentLocator)
            {
                _parentLocator = parentLocator;
                DefaultStage = this[""];
            }



            Func<DependencyObject> _parentLocator;


            #region Attached Property




            public static string GetBeacon(DependencyObject obj)
            {
                return (string)obj.GetValue(BeaconProperty);
            }

            public static void SetBeacon(DependencyObject obj, string value)
            {
                obj.SetValue(BeaconProperty, value);
            }

            // Using a DependencyProperty as the backing store for Beacon.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty BeaconProperty =
                DependencyProperty.RegisterAttached("Beacon", typeof(string), typeof(StageManager), new PropertyMetadata(null,
                       (o, p) =>
                       {
                           var name = (p.NewValue as string);
                           var target = o as FrameworkElement;

                           target.Loaded +=
                               (_1, _2)
                               =>
                               {
                                   StageManager.RegisterTargetBeacon(name, target);
                               };
                       }

                       ));





            #endregion


            internal FrameworkElement LocateTargetContainer(IView view, ref string targetContainerName, IViewModel sourceVM)
            {
                targetContainerName = targetContainerName ?? "";
                var viewele = view as FrameworkElement;
                FrameworkElement target = null;



                var dic = GetOrCreateBeacons(sourceVM.StageManager.CurrentBindingView as FrameworkElement);
                dic.TryGetValue(targetContainerName, out target);


                if (target == null)
                {
                    target = _parentLocator() as FrameworkElement;
                }

                if (target == null)
                {
                    var vieweleCt = viewele as ContentControl;
                    if (vieweleCt != null)
                    {
                        target = vieweleCt.Content as FrameworkElement;
                    }
                }
                return target;
            }



            public void Dispose()
            {
                _ViewModel = null;
            }




            private static Dictionary<string, FrameworkElement> GetOrCreateBeacons(FrameworkElement view)
            {
                Dictionary<string, FrameworkElement> dic;
#if NETFX_CORE
                if (!view.Resources.ContainsKey(NavigatorBeaconsKey))
#else
                if (!view.Resources.Contains(NavigatorBeaconsKey))
#endif
                {
                    dic = new Dictionary<string, FrameworkElement>();
                    view.Resources.Add(NavigatorBeaconsKey, dic);
                }
                else
                    dic = view.Resources[NavigatorBeaconsKey] as Dictionary<string, FrameworkElement>;

                return dic;
            }

            public static void RegisterTargetBeacon(string name, FrameworkElement target)
            {
                var view = LocateIView(target);
                var beacons = GetOrCreateBeacons(view);


                beacons[name] = target;


            }

            private static FrameworkElement LocateIView(FrameworkElement target)
            {

                var view = target;

                while (view != null)
                {
                    if (view is IView)
                    {
                        break;
                    }
                    view = view.Parent as FrameworkElement;

                }
                return view;
            }







            public Stage DefaultStage
            {
                get { return (Stage)GetValue(DefaultTargetProperty); }
                set { SetValue(DefaultTargetProperty, value); }
            }

            // Using a DependencyProperty as the backing store for DefaultTarget.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty DefaultTargetProperty =
                DependencyProperty.Register("DefaultTarget", typeof(Stage), typeof(StageManager), new PropertyMetadata(null));



            public Stage this[string beaconKey]
            {
                get
                {
                    var fr = LocateTargetContainer(CurrentBindingView, ref beaconKey, _ViewModel);
                    if (fr != null)
                    {
                        return new Stage(fr, beaconKey, this);
                    }
                    else
                        return null;
                }
            }
        }



    }

    namespace Collections
    {
        public class KeyedObserableCollection<K, V> : ObservableCollection<KeyValuePair<K, V>>
        {

            public KeyedObserableCollection(IDictionary<K, V> items)
            {
                if (items == null)
                {
                    throw new ArgumentException("items could not be null.");
                }
                _coreDictionary = items;
                foreach (var item in items)
                {
                    base.Add(item);
                }
            }



            IDictionary<K, V> _coreDictionary;
            int _coreVersion;
            int _shadowVersion;
            private void IncVer()
            {
                _coreVersion++;
                if (_coreVersion >= 1024 * 1024 * 1024)
                {
                    _coreVersion = 0;
                }
            }



            protected override void ClearItems()
            {
                base.ClearItems();
                _coreDictionary.Clear();
                IncVer();
            }


            protected override void InsertItem(int index, KeyValuePair<K, V> item)
            {
                _coreDictionary.Add(item.Key, item.Value);
                base.InsertItem(index, item);
                IncVer();
            }

            protected override void SetItem(int index, KeyValuePair<K, V> item)
            {

                _coreDictionary.Add(item.Key, item.Value);
                RemoveFromDic(index);

                base.SetItem(index, item);
                IncVer();
            }

            private void RemoveFromDic(int index)
            {
                var rem = base[index];
                if (rem.Key != null)
                {
                    _coreDictionary.Remove(rem.Key);
                }
                IncVer();
            }

            protected override void RemoveItem(int index)
            {
                RemoveFromDic(index);
                base.RemoveItem(index);
                IncVer();
            }


#if SILVERLIGHT_5||NET40
            Dictionary<K, V> _shadowDictionary;
            public IDictionary<K, V> Items
            {
                get
                {
                    if (_shadowDictionary == null || _shadowVersion != _coreVersion)
                    {
                        _shadowDictionary = new Dictionary<K, V>(_coreDictionary);
                        _shadowVersion = _coreVersion;
                    }
                    return _shadowDictionary;

                }
            }

#else
            ReadOnlyDictionary<K, V> _shadowDictionary;
            public IDictionary<K, V> Items
            {
                get
                {
                    if (_shadowDictionary == null || _shadowVersion != _coreVersion)
                    {
                        _shadowDictionary = new ReadOnlyDictionary<K, V>(_coreDictionary);
                        _shadowVersion = _coreVersion;
                    }
                    return _shadowDictionary;

                }
            }

#endif

        }


        public static class ObservableItemsAndSelectionGroup
        {


            public static IObservableItemsAndSelectionGroup<object, ICollection, IList> GetItemSelectionGroup(DependencyObject obj)
            {
                return (IObservableItemsAndSelectionGroup<object, ICollection, IList>)obj.GetValue(ItemSelectionGroupProperty);
            }

            public static void SetItemSelectionGroup(DependencyObject obj, IObservableItemsAndSelectionGroup<object, ICollection, IList> value)
            {
                obj.SetValue(ItemSelectionGroupProperty, value);
            }

            // Using a DependencyProperty as the backing store for ItemSelectionGroup.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty ItemSelectionGroupProperty =
                DependencyProperty.RegisterAttached("ItemSelectionGroup", typeof(IObservableItemsAndSelectionGroup<object, ICollection, IList>), typeof(ObservableItemsAndSelectionGroup), new PropertyMetadata(null,
                    (o, s) =>
                    {
                        var ls = o as ItemsControl;
                        if (ls == null)
                        {
                            return;
                        }
                        var vm = s.NewValue as IObservableItemsAndSelectionGroup<object, ICollection, IList>;
                        if (vm == null)
                        {
                            return;
                        }

                        vm.BindedTo = ls;
                        var itemsBinding = new Binding()
                        {
                            Source = s.NewValue,
                            Mode = BindingMode.OneWay,
                            Path = new PropertyPath(
                                ExpressionHelper.GetPropertyName<IObservableItemsAndSelectionGroup<object, ICollection, IList>>(
                                    x => x.Items))
                        };

                        BindingOperations.SetBinding(ls, ItemsControl.ItemsSourceProperty, itemsBinding);



                        if (!(ls is Selector))
                        {
                            return;
                        }



                        var selectedBinding = new Binding()
                        {
                            Source = s.NewValue,
                            Mode = BindingMode.TwoWay,
                            Path = new PropertyPath(
                                ExpressionHelper.GetPropertyName<IObservableItemsAndSelectionGroup<object, ICollection, IList>>(
                                    x => x.SelectedItem))
                        };

                        BindingOperations.SetBinding(ls, Selector.SelectedItemProperty, selectedBinding);


                        var selectedindexBinding = new Binding()
                        {
                            Source = s.NewValue,
                            Mode = BindingMode.TwoWay,
                            Path = new PropertyPath(
                                ExpressionHelper.GetPropertyName<IObservableItemsAndSelectionGroup<object, ICollection, IList>>(
                                    x => x.SelectedIndex))
                        };

                        BindingOperations.SetBinding(ls, Selector.SelectedIndexProperty, selectedindexBinding);



                        var selectedValuePathBinding = new Binding()
                        {
                            Source = s.NewValue,
                            Mode = BindingMode.TwoWay,
                            Path = new PropertyPath(
                                ExpressionHelper.GetPropertyName<IObservableItemsAndSelectionGroup<object, ICollection, IList>>(
                                    x => x.SelectedValuePath))
                        };

                        BindingOperations.SetBinding(ls, Selector.SelectedValuePathProperty, selectedValuePathBinding);

                        var selectedValueBinding = new Binding()
                        {
                            Source = s.NewValue,
                            Mode = BindingMode.TwoWay,
                            Path = new PropertyPath(
                                ExpressionHelper.GetPropertyName<IObservableItemsAndSelectionGroup<object, ICollection, IList>>(
                                    x => x.SelectedValue))
                        };

                        BindingOperations.SetBinding(ls, Selector.SelectedValueProperty, selectedValueBinding);
#if SILVERLIGHT_5 || WINDOWS_PHONE_8
                        if (!(ls is ListBox))
#else
                        if (!(ls is ListBox) && (!(ls is ListView)))
#endif

                        {
                            return;
                        }

                        var selectionModeBinding = new Binding()
                        {
                            Source = s.NewValue,
                            Mode = BindingMode.TwoWay,
                            Path = new PropertyPath(
                                ExpressionHelper.GetPropertyName<IObservableItemsAndSelectionGroup<object, ICollection, IList>>(
                                    x => x.SelectionMode))
                        };

                        BindingOperations.SetBinding(ls, ListBox.SelectionModeProperty, selectionModeBinding);


                    }));






        }

        public interface IObservableItemsAndSelectionGroup<out TValue, out TCollection, out TList>
            where TList : IList
            where TCollection : ICollection
        {
            FrameworkElement BindedTo { get; set; }
            string SelectedValuePath { get; set; }
            SelectionMode SelectionMode { get; set; }
            Object SelectedValue { get; set; }
            TCollection Items { get; }
            int SelectedIndex { get; }
            TValue SelectedItem
            {
                get;
            }
            TList SelectedItems
            {
                get;
            }
        }
        public class ObservableItemsAndSelectionGroup<T> : BindableBase<ObservableItemsAndSelectionGroup<T>>, IObservableItemsAndSelectionGroup<T, ObservableCollection<T>, IList>
        {

            public ObservableItemsAndSelectionGroup()
            {
                base.AddDisposeAction(() => BindedTo = null);

            }

            public FrameworkElement BindedTo { get; set; }

            public SelectionMode SelectionMode
            {
                get { return _SelectionModeLocator(this).Value; }
                set { _SelectionModeLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property SelectionMode SelectionMode Setup
            protected Property<SelectionMode> _SelectionMode = new Property<SelectionMode> { LocatorFunc = _SelectionModeLocator };
            static Func<BindableBase, ValueContainer<SelectionMode>> _SelectionModeLocator = RegisterContainerLocator<SelectionMode>("SelectionMode", model => model.Initialize("SelectionMode", ref model._SelectionMode, ref _SelectionModeLocator, _SelectionModeDefaultValueFactory));
            static Func<SelectionMode> _SelectionModeDefaultValueFactory = null;
            #endregion



            public string SelectedValuePath
            {
                get { return _SelectedValuePathLocator(this).Value; }
                set { _SelectedValuePathLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property string SelectedValuePath Setup
            protected Property<string> _SelectedValuePath = new Property<string> { LocatorFunc = _SelectedValuePathLocator };
            static Func<BindableBase, ValueContainer<string>> _SelectedValuePathLocator = RegisterContainerLocator<string>("SelectedValuePath", model => model.Initialize("SelectedValuePath", ref model._SelectedValuePath, ref _SelectedValuePathLocator, _SelectedValuePathDefaultValueFactory));
            static Func<string> _SelectedValuePathDefaultValueFactory = null;
            #endregion


            public object SelectedValue
            {
                get { return _SelectedValueLocator(this).Value; }
                set { _SelectedValueLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property object SelectedValue Setup
            protected Property<object> _SelectedValue = new Property<object> { LocatorFunc = _SelectedValueLocator };
            static Func<BindableBase, ValueContainer<object>> _SelectedValueLocator = RegisterContainerLocator<object>("SelectedValue", model => model.Initialize("SelectedValue", ref model._SelectedValue, ref _SelectedValueLocator, _SelectedValueDefaultValueFactory));
            static Func<object> _SelectedValueDefaultValueFactory = null;
            #endregion



            public ObservableCollection<T> Items
            {
                get { return _ItemsLocator(this).Value; }
                set { _ItemsLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property ObservableCollection<T>  Items Setup
            protected Property<ObservableCollection<T>> _Items = new Property<ObservableCollection<T>> { LocatorFunc = _ItemsLocator };
            static Func<BindableBase, ValueContainer<ObservableCollection<T>>> _ItemsLocator = RegisterContainerLocator<ObservableCollection<T>>("Items", model => model.Initialize("Items", ref model._Items, ref _ItemsLocator, _ItemsDefaultValueFactory));
            static Func<ObservableCollection<T>> _ItemsDefaultValueFactory = () => new ObservableCollection<T>();
            #endregion



            public int SelectedIndex
            {
                get { return _SelectedIndexLocator(this).Value; }
                set { _SelectedIndexLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property int SelectedIndex Setup
            protected Property<int> _SelectedIndex = new Property<int> { LocatorFunc = _SelectedIndexLocator };
            static Func<BindableBase, ValueContainer<int>> _SelectedIndexLocator = RegisterContainerLocator<int>("SelectedIndex", model => model.Initialize("SelectedIndex", ref model._SelectedIndex, ref _SelectedIndexLocator, _SelectedIndexDefaultValueFactory));
            static Func<int> _SelectedIndexDefaultValueFactory = null;
            #endregion



            public T SelectedItem
            {
                get { return _SelectedItemLocator(this).Value; }
                set { _SelectedItemLocator(this).SetValueAndTryNotify(value); }
            }
            #region Property T SelectedItem Setup
            protected Property<T> _SelectedItem = new Property<T> { LocatorFunc = _SelectedItemLocator };
            static Func<BindableBase, ValueContainer<T>> _SelectedItemLocator = RegisterContainerLocator<T>("SelectedItem", model => model.Initialize("SelectedItem", ref model._SelectedItem, ref _SelectedItemLocator, _SelectedItemDefaultValueFactory));
            static Func<T> _SelectedItemDefaultValueFactory = null;
            #endregion


            public IList SelectedItems
            {
                get
                {
                    if (BindedTo != null)
                    {
                        dynamic x = BindedTo;
                        return x.SelectedItems as IList;
                    }
                    else
                    {
                        return null;
                    }
                }
            }





        }
    }

    namespace Common
    {
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


    }

}


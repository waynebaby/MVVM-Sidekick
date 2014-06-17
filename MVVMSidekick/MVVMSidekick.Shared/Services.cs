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
using MVVMSidekick.Utilities;
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

namespace MVVMSidekick
{

    namespace Services
    {
        public interface IServiceLocator 
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

        public interface IServiceLocator<TService> 
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
                    return entry.GetService(paremeters);
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
}

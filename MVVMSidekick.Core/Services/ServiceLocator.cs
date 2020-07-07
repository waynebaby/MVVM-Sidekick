// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Services.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Threading.Tasks;
using MVVMSidekick.Utilities;
using MVVMSidekick.Views;
using System.Globalization;
using System.Linq;

namespace MVVMSidekick
{


    namespace Services
    {



        public delegate Task AsyncAction<T>(T input);

        public abstract class ServiceLocator : IServiceLocator
        {
            static public string DefaultServiceLocatorInstanceTypeName = "MVVMSidekick.DependencyInjection";
            static public string DefaultServiceLocatorInstanceAssemblyName = "MVVMSidekick.Services.UnityServiceLocator";



            static internal Lazy<ServiceLocator> _defaultInstance
                = new Lazy<ServiceLocator>(
                    () =>
                    {
                        var type = AppDomain.CurrentDomain.GetAssemblies()
                            .Where(x => x.GetName().Name.StartsWith(DefaultServiceLocatorInstanceAssemblyName ))
                            .Select(x =>x.GetType(DefaultServiceLocatorInstanceTypeName )).Single();
                        return Activator.CreateInstance(type) as ServiceLocator;

                    }, true);

            static IServiceLocator _instance;

            public static IServiceLocator Instance
            {
                get
                {
                    lock (_defaultInstance)
                    {
                        if (_instance == null)
                        {
                            _instance = _defaultInstance.Value;
                        }

                    }
                    return _instance;
                }
            }


            /// <summary>
            ///  SetInstance
            /// </summary>
            /// <typeparam name="TServiceLocator"></typeparam>
            /// <param name="instance"></param>
            /// <returns>IServiceLocator because we don't want the instance be configured after had been set to instance. </returns>
            public static IServiceLocator SetInstance<TServiceLocator>(TServiceLocator instance) where TServiceLocator : ServiceLocator
            {

                lock (_defaultInstance)
                {
                    _instance = instance;
                }
                return instance;
            }






            public abstract bool HasInstance<TService>(string name = null);
            public abstract IServiceLocator Register<TService>(TService instance);
            public abstract IServiceLocator Register<TService>(string name, TService instance);
            public abstract IServiceLocator Register<TFrom, TTo>(string name = null)
                where TTo : TFrom;
            public abstract IServiceLocator RegisterAsyncFactory<TService>(Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
            public abstract IServiceLocator RegisterAsyncFactory<TService>(string name, Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true);
            public abstract IServiceLocator RegisterFactory<TService>(string name, Func<object, IServiceLocator, TService> factory, bool isAlwaysCreatingNew = true);
            public abstract TService Resolve<TService>(string name = null);
            public abstract Task<TService> ResolveAsyncFactoryAsync<TService>(string name = null, object parameter = null);
            public abstract TService ResolveFactory<TService>(string name = null, object parameter = null);

            public TService TryResolve<TService>(Func<TService> backupPlan, string name = null)
            {
                return HasInstance<TService>(name) ? Resolve<TService>(name) : backupPlan();
            }
        }




    }
}

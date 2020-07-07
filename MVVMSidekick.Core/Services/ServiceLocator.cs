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
            static string DefaultServiceLocatorInstanceTypeName = "MVVMSidekick.Services.UnityServiceLocator, MVVMSidekick.DependencyInjection.Unity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            public static Func<ServiceLocator> DefaultInstanceFactory =
            () =>
                    {
                  
                        var type = Type.GetType(DefaultServiceLocatorInstanceTypeName);
                        return Activator.CreateInstance(type) as ServiceLocator;

                    };
            static internal Lazy<ServiceLocator> _defaultInstance
                    = new Lazy<ServiceLocator>(DefaultInstanceFactory
                       , true);

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

            public (TService Service, Exception Exception) TryResolve<TService>(Func<TService> backupPlan, string name = null)
            {
                TService rval = default;
                Exception exception = default;
                try
                {
                    rval = Resolve<TService>();

                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                return (Object.Equals(rval, default(TService)) ? backupPlan() : rval, exception);
            }
        }




    }
}

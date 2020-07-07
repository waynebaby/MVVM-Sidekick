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
using System.Collections.Generic;
using System.Threading.Tasks;
using MVVMSidekick.Utilities;
using Unity;
using MVVMSidekick.Views;
using System.Globalization;

namespace MVVMSidekick
{


    namespace Services
    {


        public class UnityServiceLocator : ServiceLocator
        {
            public UnityServiceLocator() : this(new UnityContainer())
            {
            }


            //public UnityServiceLocator(bool isForUnitTesting) : this(new UnityContainer())
            //{
            //}

            public UnityServiceLocator(IUnityContainer coreContainer)
            {
                UnityContainer = coreContainer;
            }


            public IUnityContainer UnityContainer { get; protected set; }

            public override bool HasInstance<TService>(string name = null)
            {
                return name == null ? UnityContainer.IsRegistered(typeof(TService)) : UnityContainer.IsRegistered(typeof(TService), name);
            }

            public override IServiceLocator Register<TService>(TService instance)
            {
                UnityContainer.RegisterInstance(instance);
                return this;
            }

            public override IServiceLocator Register<TService>(string name, TService instance)
            {
                UnityContainer.RegisterInstance(name, instance);
                return this;

            }

            public override IServiceLocator Register<TFrom, TTo>(string name = null)

            {
                if (name == null)
                {
                    UnityContainer.RegisterType<TFrom, TTo>();

                }
                else
                {
                    UnityContainer.RegisterType<TFrom, TTo>(name);
                }

                return this;
            }

            public override IServiceLocator RegisterAsyncFactory<TService>(Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true)
            {
                var fac = new ResolveFactory<Task<TService>>(asyncFactory, isAlwaysCreatingNew);
                UnityContainer.RegisterInstance(fac);
                return this;
            }

            public override IServiceLocator RegisterAsyncFactory<TService>(string name, Func<object, IServiceLocator, Task<TService>> asyncFactory, bool isAlwaysCreatingNew = true)
            {
                var fac = new ResolveFactory<Task<TService>>(asyncFactory, isAlwaysCreatingNew);
                UnityContainer.RegisterInstance(name, fac);
                return this;
            }

            public override IServiceLocator RegisterFactory<TService>(string name, Func<object, IServiceLocator, TService> factory, bool isAlwaysCreatingNew = true)
            {
                var fac = new ResolveFactory<TService>(factory, isAlwaysCreatingNew);
                UnityContainer.RegisterInstance(name, fac);
                return this;
            }

            public override TService Resolve<TService>(string name = null)
            {
                return name == null ? UnityContainer.Resolve<TService>() : UnityContainer.Resolve<TService>(name);
            }

            public override async Task<TService> ResolveAsyncFactoryAsync<TService>(string name = null, object parameter = null)
            {
                return await ResolveFactory<Task<TService>>(name);
            }

            public override TService ResolveFactory<TService>(string name = null, object parameter = null)
            {
                var fac = Resolve<ResolveFactory<TService>>(name);
                if (fac != null)
                {
                    return fac.GetInstance(parameter, this);
                }
                else
                {
                    return default(TService);
                }
            }


        }



    }
}

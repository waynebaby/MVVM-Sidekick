/// <summary>
/// <para>微软依赖注入服务提供者定位器，提供基于Microsoft.Extensions.DependencyInjection的服务定位功能</para>
/// <para>Microsoft dependency injection service provider locator, providing service location functionality based on Microsoft.Extensions.DependencyInjection</para>
/// </summary>

//using MVVMSidekick.Services;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.Extensions.DependencyInjection;
//using System.Threading.Tasks;
//using System.Collections.Concurrent;

//namespace MVVMSidekick.Core.Services
//{

//    /// <summary>
//    /// <para>微软依赖注入服务提供者定位器构建器</para>
//    /// <para>Microsoft dependency injection service provider locator builder</para>
//    /// </summary>
//    public class MSDIServiceProviderServiceLocatorBuilder : ServiceLocatorBuilder
//    {
//        private readonly IServiceCollection services;
//        private readonly ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>> factoryDataCore
//             = new ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>>();

//        /// <summary>
//        /// <para>初始化MSDIServiceProviderServiceLocatorBuilder类的新实例</para>
//        /// <para>Initializes a new instance of the MSDIServiceProviderServiceLocatorBuilder class</para>
//        /// </summary>
//        /// <param name="services">服务集合 / Service collection</param>
//        public MSDIServiceProviderServiceLocatorBuilder(IServiceCollection services)
//        {
//            this.services = services;
//            services.AddSingleton(factoryDataCore);
//        }


//        public override IServiceLocatorBuilder Register<TService>(string name, TService instance)
//        {
//            Func<IServiceProvider, object> factory = _ => instance;

//            factoryDataCore.AddOrUpdate((name, typeof(TService)), factory, (k, oldv) => factory);
//            return this;
//        }

//        public override IServiceLocatorBuilder Register<TAbstract, TConcrete>(string name = null)
//        {
//            name = name ?? "";
//            Func<IServiceProvider, object> factory = sp => sp.GetService<TConcrete>();
//            factoryDataCore.AddOrUpdate((name, typeof(TAbstract)), factory, (k, oldv) => factory);
//            return this;
//        }

//        public override IServiceLocatorBuilder RegisterFactory<TService>(string name, Func<(string Name, IServiceLocator ServiceProvider), TService> factory, bool isAlwaysCreatingNew = true)
//        {
//            if (isAlwaysCreatingNew)
//            {
//                Func<IServiceProvider, object> factoryValue = sp =>factory((name,sp.GetService<IServiceLocator>()) );

               
//                factoryDataCore.AddOrUpdate((name, typeof(TService)), 

//                , (k, oldv) => _ => instance);
//                return this;
//            }
//            else
//            {

//            }

//            return this;
//        }
//    }
//    public class MSDIServiceProviderServiceLocator : ServiceLocator
//    {
//        private readonly IServiceProvider serviceProvider;
//        private readonly ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>> factoryDataCore;

//        public MSDIServiceProviderServiceLocator(IServiceProvider serviceProvider, ConcurrentDictionary<(string Name, Type ServiceType), Func<IServiceProvider, object>> factoryDataCore)
//        {
//            this.serviceProvider = serviceProvider;
//            this.factoryDataCore = factoryDataCore;
//        }


//        public IServiceCollection Services { get; set; }




//        public override TService Resolve<TService>(string name = null)
//        {
//            throw new NotImplementedException();
//        }
//    }


//}

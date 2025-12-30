/// <summary>
/// <para>解析工厂，提供服务解析的工厂模式实现</para>
/// <para>Resolve factory, providing factory pattern implementation for service resolution</para>
/// </summary>

//// ***********************************************************************
//// Assembly         : MVVMSidekick_Wp8
//// Author           : waywa
//// Created          : 05-17-2014
////
//// Last Modified By : waywa
//// Last Modified On : 01-04-2015
//// ***********************************************************************
//// <copyright file="Services.cs" company="">
////     Copyright ©  2012
//// </copyright>
//// <summary></summary>
//// ***********************************************************************
//using System;

//namespace MVVMSidekick
//{

//    namespace Services
//    {
//        /// <summary>
//        /// <para>泛型解析工厂类，提供指定类型服务的解析功能</para>
//        /// <para>Generic resolve factory class, providing resolution functionality for services of specified type</para>
//        /// </summary>
//        /// <typeparam name="TService">服务类型 / Service type</typeparam>
//        public class ResolveFactory<TService>
//        {
//            /// <summary>
//            /// <para>初始化ResolveFactory类的新实例</para>
//            /// <para>Initializes a new instance of the ResolveFactory class</para>
//            /// </summary>
//            /// <param name="factory">工厂方法 / Factory method</param>
//            /// <param name="isAlwaysCreatingNew">是否总是创建新实例 / Whether always creating new instance</param>
//            public ResolveFactory(Func<object, IServiceLocator, TService> factory, bool isAlwaysCreatingNew)
//            {
//                IsAlwaysCreatingNew = isAlwaysCreatingNew;
//                _factory = factory;
//            }

//            public bool IsAlwaysCreatingNew { get; private set; }

//            private TService _instance;
//            private bool _executed;
//            private object _lock = new object();
//            private Func<object, IServiceLocator, TService> _factory;

//            public virtual TService GetInstance(object parameter, IServiceLocator locator)
//            {
//                if (IsAlwaysCreatingNew)
//                {
//                    return _factory(parameter, locator);
//                }
//                else
//                {
//                    lock (_lock)
//                    {
//                        if (_executed)
//                        {
//                            return _instance;
//                        }
//                        else
//                        {
//                            _instance = _factory(parameter, locator);
//                            _executed = true;
//                            return _instance;
//                        }
//                    }
//                }
//            }


//        }




//    }
//}

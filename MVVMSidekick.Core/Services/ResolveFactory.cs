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

namespace MVVMSidekick
{


    namespace Services
    {
        public class ResolveFactory<TService>
        {
            public ResolveFactory(Func<object, IServiceLocator, TService> factory, bool isAlwaysCreatingNew)
            {
                IsAlwaysCreatingNew = isAlwaysCreatingNew;
                _factory = factory;
            }

            public bool IsAlwaysCreatingNew { get; private set; }

            private TService _instance;
            private bool _executed;
            private object _lock = new object();
            private Func<object, IServiceLocator, TService> _factory;

            public virtual TService GetInstance(object parameter, IServiceLocator locator)
            {
                if (IsAlwaysCreatingNew)
                {
                    return _factory(parameter, locator);
                }
                else
                {
                    lock (_lock)
                    {
                        if (_executed)
                        {
                            return _instance;
                        }
                        else
                        {
                            _instance = _factory(parameter, locator);
                            _executed = true;
                            return _instance;
                        }
                    }
                }
            }


        }




    }
}

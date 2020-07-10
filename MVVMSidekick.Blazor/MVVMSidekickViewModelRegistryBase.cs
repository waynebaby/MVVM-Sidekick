using MVVMSidekick.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MVVMSidekick
{
    public abstract class MVVMSidekickViewModelRegistryBase
    {
        public static Action<MVVMSidekickOptions> AddConfigure(Action<MVVMSidekickOptions> configure)
        {
            actionList.Add(configure);
            return configure;
        }
        //public MVVMSidekickViewModelRegistryBase Configure(Action<MVVMSidekickOptions> configure)
        //{
        //    actionList.Add(configure);
        //    return this;
        //}

        private static ConcurrentBag<Action<MVVMSidekickOptions>> actionList = new ConcurrentBag<Action<MVVMSidekickOptions>>();

        public IEnumerable<Action<MVVMSidekickOptions>> Actions { get => actionList; }

        virtual public void ConfigureInstance(MVVMSidekickOptions opt)
        {
          
            foreach (var item in Actions)
            {
                item?.Invoke( opt);
            }
       
        }


    }
}
using MVVMSidekick;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public abstract class MVVMSidekickStartupBase
    {
        public static Action<MVVMSidekickOptions> AddConfigure(Action<MVVMSidekickOptions> configure)
        {
            ActionList.Add(configure);
            return configure;
        }


        public static ConcurrentBag<Action<MVVMSidekickOptions>> ActionList { get; } = new ConcurrentBag<Action<MVVMSidekickOptions>>();



        virtual public void ConfigureInstance(MVVMSidekickOptions opt)
        {

            foreach (var item in ActionList)
            {
                item?.Invoke(opt);
            }

        }
    }

}
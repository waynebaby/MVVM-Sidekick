using MVVMSidekick.Views;
using Samples.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace Samples.Startups
{
    public static partial class StartupFunctions
    {
        public static void ConfigAll()
        {
            ViewModelLocator<Calculator_Model>
                .Instance
                .Register(new Calculator_Model())
                .GetViewMapper()
                .MapToDefault<Calculator>();

#if !(NETFX_CORE||WINDOWS_PHONE_8)

            ViewModelLocator<Tree_Model>
                .Instance
                .Register(new Tree_Model())
                .GetViewMapper()
                .MapToDefault<Tree>();
#endif
        }


    }
}

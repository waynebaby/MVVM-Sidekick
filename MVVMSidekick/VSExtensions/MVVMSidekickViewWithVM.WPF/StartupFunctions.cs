using MVVMSidekick.Views;
using $rootnamespace$.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace $rootnamespace$.Startups
{
    public static partial class StartupFunctions
    {
        public static void Config$safeitemname$()
        {
            ViewModelLocator<$safeitemname$_Model>
                .Instance
                .Register(new $safeitemname$_Model())
                .GetViewMapper()
                .MapToDefault(model => new $safeitemname$(model));

        }
    }
}

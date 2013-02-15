using MVVMSidekick.Views;
using Samples.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace Samples
{
    public static class StartupFunctions
    {
        public static void ConfigCalculator()
        {
            ViewModelLocator<Calculator_Model>
                .Instance
                .Register(new Calculator_Model())
                .GetViewRegister()
                .Register<Calculator>(true);
        }
    }
}

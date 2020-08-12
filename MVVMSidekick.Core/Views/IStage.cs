﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVVMSidekick.ViewModels;
using System.Reactive.Linq;
using System.Windows;
using System.IO;
using MVVMSidekick.Services;



#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;


#elif WPF
using System.Windows.Controls;
using System.Windows.Media;

using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;
#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif




namespace MVVMSidekick.Views
{
    public interface IStage
    {
        string BeaconKey { get; }
        bool CanGoBack { get; }
        bool CanGoForward { get; }



        bool IsGoBackSupported { get; }
        bool IsGoForwardSupported { get; }
        Object Target { get; }

        Task<TTarget> Show<TTarget>(
            string viewMappingKey = null, 
            Action<(IServiceProvider serviceProvider, TTarget viewModel)> additionalViewModelConfig = null, 
            bool isWaitingForDispose = false,
            bool autoDisposeWhenViewUnload=true) where TTarget : class, IViewModel;



    }



}
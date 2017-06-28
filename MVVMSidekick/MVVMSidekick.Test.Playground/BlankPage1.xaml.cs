
using MVVMSidekick.Test.Playground.ViewModels;
using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MVVMSidekick.Test.Playground
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {



        public BlankPage1()
        {
            this.InitializeComponent();            
        }


        #region IView Disguise
        public BlankPage1_Model StrongTypeViewModel
        {
            get { return (BlankPage1_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register(nameof(StrongTypeViewModel), typeof(BlankPage1_Model), typeof(BlankPage1), new PropertyMetadata(null));



        PageViewDisguise ViewDisguise { get { return this.GetViewDisguise(); } }

        long strongTypeRegisterToken = 0;
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewDisguise.UnregisterPropertyChangedCallback(PageViewDisguise.ViewModelProperty, strongTypeRegisterToken);
            ViewDisguise.OnNavigatedFrom(e);
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewDisguise.OnNavigatedTo(e);
            strongTypeRegisterToken = ViewDisguise.RegisterPropertyChangedCallback(PageViewDisguise.ViewModelProperty, (_, __) =>
             {
                 this.StrongTypeViewModel = ViewDisguise.ViewModel as BlankPage1_Model;
             });
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            ViewDisguise.OnNavigatingFrom(e);
        }

        #endregion
    }
}

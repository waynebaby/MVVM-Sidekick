
using MVVMSidekickUWPDemo.ViewModels;
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

namespace MVVMSidekickUWPDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Counter : Page
    {



        public Counter()
        {
            this.InitializeComponent();
            ViewDisguise.RegisterPropertyChangedCallback(PageViewDisguise.ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = ViewDisguise.ViewModel as Counter_Model;
            });
            StrongTypeViewModel = ViewDisguise.ViewModel as Counter_Model;
        }


        public Counter_Model StrongTypeViewModel
        {
            get { return (Counter_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
        DependencyProperty.Register(nameof(StrongTypeViewModel), typeof(Counter_Model), typeof(Counter), new PropertyMetadata(null));


        #region IView Disguise
        PageViewDisguise ViewDisguise { get { return this.GetOrCreateViewDisguise(); } }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewDisguise.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewDisguise.OnNavigatedFrom(e);
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewDisguise.OnNavigatingFrom(e);
            base.OnNavigatingFrom(e);
        }


    }
}

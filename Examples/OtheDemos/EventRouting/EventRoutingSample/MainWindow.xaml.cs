using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using EventRoutingSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick;

namespace EventRoutingSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()

        {

            InitializeComponent();
            ViewDisguise.ViewModel = ServiceProviderLocator.RootServiceProvider.GetService<MainWindow_Model>();
        }


        #region IView Disguise
        WindowViewDisguise ViewDisguise { get { return this.GetOrCreateViewDisguise(); } }
        #endregion
    }


  
}

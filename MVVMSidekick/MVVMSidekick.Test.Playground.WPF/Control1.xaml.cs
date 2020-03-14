using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using MVVMSidekick.Test.Playground.WPF;
using MVVMSidekick.Test.Playground.WPF.ViewModels;
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

namespace MVVMSidekick.Test.Playground.WPF
{
    /// <summary>
    /// Interaction logic for Control1.xaml
    /// </summary>
    public partial class Control1 :UserControl
    {
        public Control1()
        {

            InitializeComponent();
        }

        ControlViewDisguise ViewDisguise { get { return this.GetOrCreateViewDisguise(); } }

    }
}


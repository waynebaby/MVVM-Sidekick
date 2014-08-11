using MVVMSidekick.Views;
using Samples.ViewModels;
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
using System.Windows.Shapes;

namespace Samples
{
    /// <summary>
    /// Interaction logic for Calculator.xaml
    /// </summary>
    public partial class Calculator : MVVMWindow
    {
        public Calculator()
            : base(null)
        {
            InitializeComponent();
        }

        public Calculator(Calculator_Model model)
            : base(model)
        {
            InitializeComponent();
        }
    }
}

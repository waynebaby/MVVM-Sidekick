using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MVVMSidekick.Views;
using Samples.ViewModels;

namespace Samples
{
    public partial class Calculator : MVVMPage
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using MVVMSidekick.Views;
using Samples.ViewModels;

namespace Samples
{
    public partial class Tree : MVVMPage
    {
        public Tree():base(null)
        {
            InitializeComponent();
        }

        public Tree(Tree_Model model)
            : base(model)
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.


    }
}

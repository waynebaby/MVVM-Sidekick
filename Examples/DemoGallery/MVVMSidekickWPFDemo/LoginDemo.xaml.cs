﻿using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using MVVMSidekickWPFDemo;
using MVVMSidekickWPFDemo.ViewModels;
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

namespace MVVMSidekickWPFDemo
{
    /// <summary>
    /// Interaction logic for LoginDemo.xaml
    /// </summary>
    public partial class LoginDemo : Page
    {
        public LoginDemo()
        {
            InitializeComponent();
        }

        public PageViewDisguise ViewDisguise { get { return this.GetOrCreateViewDisguise(); } }

    }

}


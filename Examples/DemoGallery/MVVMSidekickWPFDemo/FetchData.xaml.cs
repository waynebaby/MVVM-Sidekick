using System.Reactive;
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
    /// FetchData.xaml的交互逻辑，数据获取页面
    /// Interaction logic for FetchData.xaml, data fetching page
    /// </summary>
    public partial class FetchData : Page
    {
        /// <summary>
        /// 初始化FetchData页面的新实例
        /// Initializes a new instance of FetchData page
        /// </summary>
        public FetchData()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取页面视图伪装对象，用于MVVM绑定
        /// Gets the page view disguise object for MVVM binding
        /// </summary>
        public PageViewDisguise ViewDisguise { get { return this.GetOrCreateViewDisguise(); } }

    }

}


using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
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
using Microsoft.Extensions.DependencyInjection;


namespace MVVMSidekickWPFDemo
{
    /// <summary>
    /// MainWindow.xaml的交互逻辑，应用程序主窗口
    /// Interaction logic for MainWindow.xaml, main window of the application
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 初始化MainWindow的新实例，设置视图模型并初始化组件
        /// Initializes a new instance of MainWindow, sets view model and initializes components
        /// </summary>
        public MainWindow()
        {
            ViewDisguise.ViewModel = ServiceProviderLocator.RootServiceProvider.GetRequiredService<MainWindow_Model>();
            this.InitializeComponent();


        }

        #region IView Disguise
        /// <summary>
        /// 获取窗口视图伪装对象，用于MVVM绑定
        /// Gets the window view disguise object for MVVM binding
        /// </summary>
        public WindowViewDisguise ViewDisguise { get { return this.GetOrCreateViewDisguise(); } }
        #endregion

    }
}

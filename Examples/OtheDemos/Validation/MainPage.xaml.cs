using MVVMSidekick.Views;
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
using Validation.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Validation
{
    /// <summary>
    /// <para>可以单独使用或在Frame内导航到的空页面</para>
    /// <para>An empty page that can be used on its own or navigated to within a Frame</para>
    /// </summary>
    public sealed partial class MainPage : MVVMPage
    {
        public MainPage()
        {
            this.InitializeComponent();
			this.ViewModel = StrongTypeViewModel =  ViewModelLocator<MainPage_Model>.Instance.Resolve(); 
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as MainPage_Model;
            });
        }


    /// <summary>
    /// <para>获取或设置强类型视图模型</para>
    /// <para>Gets or sets the strong-typed view model</para>
    /// </summary>
    public MainPage_Model StrongTypeViewModel
        {
            get { return (MainPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        /// <summary>
        /// <para>强类型视图模型依赖属性</para>
        /// <para>Strong-typed view model dependency property</para>
        /// </summary>
        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(MainPage_Model), typeof(MainPage), new PropertyMetadata(null));



    }
}

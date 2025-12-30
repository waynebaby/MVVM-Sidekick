using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using EventRoutingSample;
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

namespace EventRoutingSample
{
	/// <summary>
	/// <para>DisopseTestForBehaviors.xaml 的交互逻辑</para>
	/// <para>Interaction logic for DisopseTestForBehaviors.xaml</para>
	/// </summary>
	public partial class DisopseTestForBehaviors : Window
	{
		/// <summary>
		/// <para>初始化 DisopseTestForBehaviors 类的新实例</para>
		/// <para>Initializes a new instance of the DisopseTestForBehaviors class</para>
		/// </summary>
		public DisopseTestForBehaviors()
		{
			InitializeComponent();
		}
		
		#region IView Disguise
		/// <summary>
		/// <para>获取视图伪装对象，用于MVVM模式下的视图管理</para>
		/// <para>Gets the view disguise object for view management in MVVM pattern</para>
		/// </summary>
		WindowViewDisguise ViewDisguise { get { return this.GetOrCreateViewDisguise(); } }
		#endregion
	}
}


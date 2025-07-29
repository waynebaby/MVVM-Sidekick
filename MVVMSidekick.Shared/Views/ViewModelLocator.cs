/// <summary>
/// <para>视图模型定位器文件 - 此文件中的代码当前被注释，可能是遗留或备用实现</para>
/// <para>View model locator file - The code in this file is currently commented out, possibly legacy or backup implementation</para>
/// </summary>
/// <remarks>
/// <para>这个文件包含了ViewModelLocator泛型类的实现，用于定位和管理特定类型的视图模型</para>
/// <para>This file contains the implementation of ViewModelLocator generic class for locating and managing specific types of view models</para>
/// <para>由于代码被完全注释，可能表示此功能已被其他实现替代或暂时禁用</para>
/// <para>Since the code is completely commented out, it may indicate this functionality has been replaced by other implementations or temporarily disabled</para>
/// </remarks>

//// ***********************************************************************
//// Assembly         : MVVMSidekick_Wp8
//// Author           : waywa
//// Created          : 05-17-2014
////
//// Last Modified By : waywa
//// Last Modified On : 01-04-2015
//// ***********************************************************************
//// <copyright file="Views.cs" company="">
////     Copyright ©  2012
//// </copyright>
//// <summary></summary>
//// ***********************************************************************
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using MVVMSidekick.ViewModels;
//using System.Reactive.Linq;
//using System.Windows;
//using System.IO;
//using MVVMSidekick.Services;



//#if WINDOWS_UWP
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Navigation;
//using Windows.UI.Xaml.Media;


//#elif WPF
//using System.Windows.Controls;
//using System.Windows.Media;

//using System.Collections.Concurrent;
//using System.Windows.Navigation;

//using MVVMSidekick.Views;
//using System.Windows.Controls.Primitives;
//using MVVMSidekick.Utilities;
//#elif SILVERLIGHT_5 || SILVERLIGHT_4
//						   using System.Windows.Media;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Navigation;
//using System.Windows.Controls.Primitives;
//#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
//using System.Windows.Media;
//using System.Windows.Controls;
//using Microsoft.Phone.Controls;
//using System.Windows.Data;
//using System.Windows.Navigation;
//using System.Windows.Controls.Primitives;
//#endif




//namespace MVVMSidekick
//{


//    namespace Views
//    {
//        /// <summary>
//        /// <para>视图模型定位器类，用于定位和管理特定类型的视图模型实例</para>
//        /// <para>View model locator class for locating and managing specific types of view model instances</para>
//        /// </summary>
//        /// <typeparam name="TViewModel">
//        /// <para>视图模型类型，必须实现IViewModel接口</para>
//        /// <para>The type of the view model that must implement IViewModel interface</para>
//        /// </typeparam>
//        /// <remarks>
//        /// <para>Locator of view model class</para>
//        /// </remarks>
//        public class ViewModelLocator<TViewModel> : MVVMSidekick.Services.TypeSpecifiedServiceLocatorBase<ViewModelLocator<TViewModel>, TViewModel>
//			where TViewModel : IViewModel
//		{
//			/// <summary>
//			/// <para>静态构造函数，初始化ViewModelLocator实例</para>
//			/// <para>Static constructor that initializes the ViewModelLocator instance</para>
//			/// </summary>
//			static ViewModelLocator()
//			{
//				Instance = new ViewModelLocator<TViewModel>();
//			}
//			
//			/// <summary>
//			/// <para>获取或设置ViewModelLocator的静态实例</para>
//			/// <para>Gets or sets the static instance of ViewModelLocator</para>
//			/// </summary>
//			/// <value>
//			/// <para>ViewModelLocator实例</para>
//			/// <para>The ViewModelLocator instance</para>
//			/// </value>
//			/// <remarks>
//			/// <para>Gets or sets the instance.</para>
//			/// </remarks>
//#pragma warning disable CA1000 // Do not declare static members on generic types
//			public static ViewModelLocator<TViewModel> Instance { get; set; }
//#pragma warning restore CA1000 // Do not declare static members on generic types

//		}



//	}
//}

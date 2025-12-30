/// <summary>
/// <para>视图模型到视图映射器服务定位器文件 - 此文件中的代码当前被注释，可能是遗留或备用实现</para>
/// <para>View model to view mapper service locator file - The code in this file is currently commented out, possibly legacy or backup implementation</para>
/// </summary>
/// <remarks>
/// <para>这个文件包含了ViewModelToViewMapperServiceLocator泛型类的实现，用于提供视图模型到视图映射的服务定位功能</para>
/// <para>This file contains the implementation of ViewModelToViewMapperServiceLocator generic class for providing service location functionality for view model to view mapping</para>
/// <para>支持多平台包括WPF、UWP、Silverlight、Windows Phone等</para>
/// <para>Supports multiple platforms including WPF, UWP, Silverlight, Windows Phone, etc.</para>
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



//#if WINDOWS_UWP


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
//        /// View model to view service locator
//        /// </summary>
//        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
//        public class ViewModelToViewMapperServiceLocator<TViewModel> :
//			MVVMSidekick.Services.TypeSpecifiedServiceLocatorBase<ViewModelToViewMapperServiceLocator<TViewModel>, object>,
//			IViewModelToViewMapperServiceLocator
//		{
//			/// <summary>
//			/// Constuctor
//			/// </summary>
//			static ViewModelToViewMapperServiceLocator()
//			{
//				//Instance = new ViewModelToViewMapperServiceLocator<TViewModel>();

//			}

//			static Lazy<IViewModelToViewMapperServiceLocator> _Instance = new Lazy<IViewModelToViewMapperServiceLocator>
//				(() => new ViewModelToViewMapperServiceLocator<TViewModel>(), true);

//			/// <summary>
//			/// Instance
//			/// </summary>

//#pragma warning disable CA1000 // Do not declare static members on generic types
//			public static IViewModelToViewMapperServiceLocator Instance
//#pragma warning restore CA1000 // Do not declare static members on generic types
//			{
//				get
//				{
//					return _Instance.Value;
//				}
//				set
//				{
//					_Instance = new Lazy<IViewModelToViewMapperServiceLocator>(() => value);
//				}
//			}


//		}



//	}
//}

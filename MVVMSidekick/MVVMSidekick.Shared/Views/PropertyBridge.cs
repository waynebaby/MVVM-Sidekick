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



//#if NETFX_CORE
//using Windows.UI.Xaml;


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
//        ///  A bridge binds two Dependency property
//        /// </summary>
//        public class PropertyBridge : FrameworkElement
//		{
//			/// <summary>
//			/// Initializes a new instance of the <see cref="PropertyBridge"/> class.
//			/// </summary>
//			public PropertyBridge()
//			{
//				base.Width = 0;
//				base.Height = 0;
//				base.Visibility = Visibility.Collapsed;

//			}



//			/// <summary>
//			/// Gets or sets the source.
//			/// </summary>
//			/// <value>
//			/// The source.
//			/// </value>
//			public object Source
//			{
//				private get { return (object)GetValue(SourceProperty); }
//				set { SetValue(SourceProperty, value); }
//			}

//			// Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...			
//			/// <summary>
//			/// The source property
//			/// </summary>
//			public static readonly DependencyProperty SourceProperty =
//				DependencyProperty.Register("Source", typeof(object), typeof(PropertyBridge), new PropertyMetadata(null,

//					(o, a) =>
//					{
//						var pb = o as PropertyBridge;
//						if (pb != null)
//						{
//							pb.Target = a.NewValue;
//						}
//					}
//					));



//			/// <summary>
//			/// Gets or sets the target.
//			/// </summary>
//			/// <value>
//			/// The target.
//			/// </value>
//			public object Target
//			{
//				get { return (object)GetValue(TargetProperty); }
//				set { SetValue(TargetProperty, value); }
//			}

//			// Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...


//			/// <summary>
//			/// The target property
//			/// </summary>
//			public static readonly DependencyProperty TargetProperty =
//				DependencyProperty.Register("Target", typeof(object), typeof(PropertyBridge), new PropertyMetadata(null));





//		}



//	}
//}

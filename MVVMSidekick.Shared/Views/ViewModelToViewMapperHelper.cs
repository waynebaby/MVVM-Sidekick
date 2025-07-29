/// <summary>
/// <para>视图模型到视图映射器帮助器文件，提供视图与视图模型映射的辅助功能</para>
/// <para>View model to view mapper helper file that provides auxiliary functions for view-to-view model mapping</para>
/// </summary>
/// <remarks>
/// <para>这个文件包含ViewModelToViewMapperHelper静态类，用于处理视图与视图模型之间的映射关系</para>
/// <para>This file contains the ViewModelToViewMapperHelper static class for handling mapping relationships between views and view models</para>
/// <para>支持多平台包括WPF、UWP、Silverlight、Windows Phone等</para>
/// <para>Supports multiple platforms including WPF, UWP, Silverlight, Windows Phone, etc.</para>
/// <para>部分代码被注释，可能表示某些功能已被重构或禁用</para>
/// <para>Some code is commented out, possibly indicating that certain features have been refactored or disabled</para>
/// </remarks>

#if !BLAZOR

using System;
using System.Collections.Generic;
using MVVMSidekick.ViewModels;



#if WINDOWS_UWP


#elif WPF
using System.Windows.Controls;
using System.Windows.Media;

using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;
#elif SILVERLIGHT_5 || SILVERLIGHT_4
						   using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif




//namespace MVVMSidekick
//{


//    namespace Views
//    {
//        /// <summary>
//        /// 		 class ViewModelToViewMapperHelper
//        /// </summary>
//        public static class ViewModelToViewMapperHelper
//        {

//            internal static Dictionary<Type, Func<IViewModel>> ViewToVMMapping = new Dictionary<Type, Func<IViewModel>>();

//            /// <summary>
//            /// Gets the default view model.
//            /// </summary>
//            /// <param name="view">The view.</param>
//            /// <returns></returns>
//            public static IViewModel GetDefaultViewModel(this IView view)
//            {
//                Func<IViewModel> func;
//                Type viewType = null;
//                switch (view)
//                {
//                    case PageViewDisguise pd:
//                        viewType = pd.AssocatedObject.GetType();
//                        break;
//                    case ControlViewDisguise pd:
//                        viewType = pd.AssocatedObject.GetType();
//                        break;
//#if WPF
//                    case WindowViewDisguise pd:
//                        viewType = pd.AssocatedObject.GetType();
//                        break;
//#endif
//                    default:
//                        viewType = view?.GetType();
//                        break;
//                }
//                if (ViewModelToViewMapperHelper.ViewToVMMapping.TryGetValue(viewType, out func))
//                {

//                    return func();
//                }
//                return null;
//            }

//            /// <summary>
//            /// Gets the view mapper.
//            /// </summary>
//            /// <typeparam name="TViewModel">The type of the view model.</typeparam>
//            /// <param name="vmRegisterEntry">The vm register entry.</param>
//            /// <returns></returns>
//            public static ViewModelToViewMapper<TViewModel> GetViewMapper<TViewModel>(this MVVMSidekick.Services.ServiceLocatorEntryStruct<TViewModel> vmRegisterEntry)
//                  where TViewModel : IViewModel
//            {
//                return new ViewModelToViewMapper<TViewModel>();
//            }


//        }



//    }
//}
#endif
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




namespace MVVMSidekick
{


    namespace Views
    {
        /// <summary>
        /// 		 class ViewModelToViewMapperHelper
        /// </summary>
        public static class ViewModelToViewMapperHelper
        {

            internal static Dictionary<Type, Func<IViewModel>> ViewToVMMapping = new Dictionary<Type, Func<IViewModel>>();

            /// <summary>
            /// Gets the default view model.
            /// </summary>
            /// <param name="view">The view.</param>
            /// <returns></returns>
            public static IViewModel GetDefaultViewModel(this IView view)
            {
                Func<IViewModel> func;
                Type viewType = null;
                switch (view)
                {
                    case PageViewDisguise pd:
                        viewType = pd.AssocatedObject.GetType();
                        break;
                    case ControlViewDisguise pd:
                        viewType = pd.AssocatedObject.GetType();
                        break;
#if WPF
                    case WindowViewDisguise pd:
                        viewType = pd.AssocatedObject.GetType();
                        break;
#endif
                    default:
                        viewType = view?.GetType();
                        break;
                }
                if (ViewModelToViewMapperHelper.ViewToVMMapping.TryGetValue(viewType, out func))
                {

                    return func();
                }
                return null;
            }

            /// <summary>
            /// Gets the view mapper.
            /// </summary>
            /// <typeparam name="TViewModel">The type of the view model.</typeparam>
            /// <param name="vmRegisterEntry">The vm register entry.</param>
            /// <returns></returns>
            public static ViewModelToViewMapper<TViewModel> GetViewMapper<TViewModel>(this MVVMSidekick.Services.ServiceLocatorEntryStruct<TViewModel> vmRegisterEntry)
                  where TViewModel : IViewModel
            {
                return new ViewModelToViewMapper<TViewModel>();
            }


        }



    }
}
#endif
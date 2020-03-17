// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Views.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;



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
        /// View model to view service locator
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        public class ViewModelToViewMapperServiceLocator<TViewModel> :
			MVVMSidekick.Services.TypeSpecifiedServiceLocatorBase<ViewModelToViewMapperServiceLocator<TViewModel>, object>,
			IViewModelToViewMapperServiceLocator
		{
			/// <summary>
			/// Constuctor
			/// </summary>
			static ViewModelToViewMapperServiceLocator()
			{
				//Instance = new ViewModelToViewMapperServiceLocator<TViewModel>();

			}

			static Lazy<IViewModelToViewMapperServiceLocator> _Instance = new Lazy<IViewModelToViewMapperServiceLocator>
				(() => new ViewModelToViewMapperServiceLocator<TViewModel>(), true);

			/// <summary>
			/// Instance
			/// </summary>

#pragma warning disable CA1000 // Do not declare static members on generic types
			public static IViewModelToViewMapperServiceLocator Instance
#pragma warning restore CA1000 // Do not declare static members on generic types
			{
				get
				{
					return _Instance.Value;
				}
				set
				{
					_Instance = new Lazy<IViewModelToViewMapperServiceLocator>(() => value);
				}
			}


		}



	}
}

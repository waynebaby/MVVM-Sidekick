// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="EventRouting.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using MVVMSidekick.ViewModels;





namespace MVVMSidekick
{

    namespace EventRouting
    {
        /// <summary>
        /// 导航事件数据
        /// </summary>
        public class NavigateCommandEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="NavigateCommandEventArgs" /> class.
			/// </summary>
			public NavigateCommandEventArgs()
			{
				ParameterDictionary = new Dictionary<string, object>();
			}
			/// <summary>
			/// Initializes a new instance of the <see cref="NavigateCommandEventArgs" /> class.
			/// </summary>
			/// <param name="dic">The dic.</param>
			public NavigateCommandEventArgs(IDictionary<string, object> dic)
				: this()
			{
				foreach (var item in dic)
				{

					(ParameterDictionary as IDictionary<string, object>)[item.Key] = item.Value;
				}

			}
			/// <summary>
			/// Gets or sets the parameter dictionary.
			/// </summary>
			/// <value>The parameter dictionary.</value>
			public Dictionary<string, object> ParameterDictionary { get; set; }

			/// <summary>
			/// Gets or sets the type of the source view.
			/// </summary>
			/// <value>The type of the source view.</value>
			public Type SourceViewType { get; set; }

			/// <summary>
			/// Gets or sets the type of the target view.
			/// </summary>
			/// <value>The type of the target view.</value>
			public Type TargetViewType { get; set; }

			/// <summary>
			/// Gets or sets the view model.
			/// </summary>
			/// <value>The view model.</value>
			public IViewModel ViewModel { get; set; }

			/// <summary>
			/// Gets or sets the target frame.
			/// </summary>
			/// <value>The target frame.</value>
			public Object TargetFrame { get; set; }
		}


	}


}

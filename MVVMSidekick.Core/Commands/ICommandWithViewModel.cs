// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Commands.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows.Input;
using MVVMSidekick.ViewModels;



namespace MVVMSidekick
{

    namespace Commands
    {
        /// <summary>
        /// 带有VM的Command接口
        /// </summary>
        public interface ICommandWithViewModel : ICommand
		{
			/// <summary>
			/// Gets or sets the view model.
			/// </summary>
			/// <value>The view model.</value>
			BindableBase ViewModel { get; set; }
		}


	}

}

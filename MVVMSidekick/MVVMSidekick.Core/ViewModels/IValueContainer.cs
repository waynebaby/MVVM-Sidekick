// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="ViewModels.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;



namespace MVVMSidekick
{

    namespace ViewModels
    {
        /// <summary>
        /// Interface IValueContainer
        /// </summary>
        public interface IValueContainer : IErrorInfo, INotifyChanged,INotifyChanging
        {
            string PropertyName { get; }

            /// <summary>
            /// Gets the type of the property.
            /// </summary>
            /// <value>The type of the property.</value>
            Type PropertyType { get; }
            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            Object Value { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether this instance is copy to allowed.
            /// </summary>
            /// <value><c>true</c> if this instance is copy to allowed; otherwise, <c>false</c>.</value>
            bool IsCopyToAllowed { get; set; }

            void AddErrorEntry(string message, Exception exception = null);


            Object Model { get; }

        }





    }

}

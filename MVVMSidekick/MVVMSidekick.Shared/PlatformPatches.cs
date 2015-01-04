// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="PlatformPatches.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#if NET40

namespace System.ComponentModel
{
    // Summary:
    //     Defines members that data entity classes can implement to provide custom
    //     synchronous and asynchronous validation support.
    public interface INotifyDataErrorInfo
    {
        // Summary:
        //     Gets a value that indicates whether the entity has validation errors.
        //
        // Returns:
        //     true if the entity currently has validation errors; otherwise, false.
        bool HasErrors { get; }

        // Summary:
        //     Occurs when the validation errors have changed for a property or for the
        //     entire entity.
        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        // Summary:
        //     Gets the validation errors for a specified property or for the entire entity.
        //
        // Parameters:
        //   propertyName:
        //     The name of the property to retrieve validation errors for; or null or System.String.Empty,
        //     to retrieve entity-level errors.
        //
        // Returns:
        //     The validation errors for the property or entity.
        IEnumerable GetErrors(string propertyName);
    }

    // Summary:
    //     Provides data for the System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged
    //     event.
    public class DataErrorsChangedEventArgs : EventArgs
    {
        // Summary:
        //     Initializes a new instance of the System.ComponentModel.DataErrorsChangedEventArgs
        //     class.
        //
        // Parameters:
        //   propertyName:
        //     The name of the property that has an error. null or System.String.Empty if
        //     the error is object-level.
        public DataErrorsChangedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }

        // Summary:
        //     Gets the name of the property that has an error.
        //
        // Returns:
        //     The name of the property that has an error. null or System.String.Empty if
        //     the error is object-level.
        public virtual string PropertyName { get; private set; }
    }

}
#endif

//#if SILVERLIGHT_5
//namespace System.Runtime.CompilerServices
//{
//    // Summary:
//    //     Allows you to obtain the method or property name of the caller to the method.
//    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
//    public sealed class CallerMemberNameAttribute : Attribute
//    {
//        // Summary:
//        //     Initializes a new instance of the System.Runtime.CompilerServices.CallerMemberNameAttribute
//        //     class.
//        public CallerMemberNameAttribute()
//        {
//        }
//    }
//    // Summary:
//    //     Allows you to obtain the full path of the source file that contains the caller.
//    //     This is the file path at the time of compile.
//    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
//    public sealed class CallerFilePathAttribute : Attribute
//    {
//        // Summary:
//        //     Initializes a new instance of the System.Runtime.CompilerServices.CallerFilePathAttribute
//        //     class.
//        public CallerFilePathAttribute() { }
//    }
//    // Summary:
//    //     Allows you to obtain the line number in the source file at which the method
//    //     is called.
//    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
//    public sealed class CallerLineNumberAttribute : Attribute
//    {
//        // Summary:
//        //     Initializes a new instance of the System.Runtime.CompilerServices.CallerLineNumberAttribute
//        //     class.
//        public CallerLineNumberAttribute() { }
//    }
//}

//#endif
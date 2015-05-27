

#if NET40

using System.Collections;

namespace System.ComponentModel
{

	/// <summary>
	/// Defines members that data entity classes can implement to provide custom
	/// synchronous and asynchronous validation support.
	/// </summary>
	public interface INotifyDataErrorInfo
	{
		// Summary:
		//     Gets a value that indicates whether the entity has validation errors.
		//
		// Returns:
		//     true if the entity currently has validation errors; otherwise, false.
		/// <summary>
		/// Gets a value that indicates whether the entity has validation errors.
		/// </summary>
		/// <value>
		/// true if the entity currently has validation errors; otherwise, false.
		/// </value>
		bool HasErrors { get; }

		// Summary:
		//     Occurs when the validation errors have changed for a property or for the
		//     entire entity.
		/// <summary>
		///  Occurs when the validation errors have changed for a property or for the
		///  entire entity.
		/// </summary>
		event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		/// <summary>
		/// The name of the property to retrieve validation errors for; or null or System.String.Empty,
		/// to retrieve entity-level errors.
		/// </summary>
		/// <param name="propertyName">The validation errors for the property or entity.</param>
		/// <returns></returns>

		IEnumerable GetErrors(string propertyName);
	}

	/// <summary>
	///   Provides data for the System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged
	///    event.
	/// </summary>
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
		/// <summary>
		/// Initializes a new instance of the <see cref="DataErrorsChangedEventArgs"/> class.
		/// </summary>
		/// <param name="propertyName"> The name of the property that has an error. null or System.String.Empty if  the error is object-level.</param>
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
		/// <summary>
		/// Gets the name of the property that has an error.
		/// </summary>
		/// <value>
		/// 	The name of the property that has an error. null or System.String.Empty if
		///     the error is object-level.
		/// </value>
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
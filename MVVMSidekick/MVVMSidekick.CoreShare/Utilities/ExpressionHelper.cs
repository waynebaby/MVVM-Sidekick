// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="Utilities.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq.Expressions;
using System.Reflection;
#if NETFX_CORE

#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Services;
using System.Reactive.Disposables;


#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Reactive.Disposables;

#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Reactive;
#endif



namespace MVVMSidekick
{

    namespace Utilities
    {
#if WINDOWS_PHONE_7
    public class Lazy<T>
    {
        public Lazy(Func<T> factory)
        { 
            _factory =()=>
            {
                lock(this)
                {
                    if(_value.Equals(default(T)))
                    {
                        _value=_factory();
                 
                    
                    }
                    return _value;
                }
            
            };
        
        }

        T _value;
        Func<T> _factory;
        public T Value 
        { 
            get
            {
               return _value.Equals (default(T))?_factory():_value; 
            }
        
            set
            {
                _value=value;
            } 
        }
    }



#endif

        /// <summary>
        /// Class ExpressionHelper.
        /// </summary>
        public class ExpressionHelper
		{
			/// <summary>
			/// Gets the name of the property.
			/// </summary>
			/// <typeparam name="TSubClassType">The type of the sub class type.</typeparam>
			/// <typeparam name="TProperty">The type of the t property.</typeparam>
			/// <param name="expression">The expression.</param>
			/// <returns>
			/// System.String.
			/// </returns>
			public static string GetPropertyName<TSubClassType, TProperty>(Expression<Func<TSubClassType, TProperty>> expression)
			{
				MemberExpression body = expression.Body as MemberExpression;
				var propName = (body.Member is PropertyInfo) ? body.Member.Name : string.Empty;
				return propName;
			}



			/// <summary>
			/// Gets the name of the property.
			/// </summary>
			/// <typeparam name="TSubClassType">The type of the sub class type.</typeparam>
			/// <param name="expression">The expression.</param>
			/// <returns>
			/// System.String.
			/// </returns>
			/// <exception cref="System.InvalidOperationException">The expression inputed should be like \x=&gt;x.PropertyName\ but currently is not: + expression.ToString()</exception>
			public static string GetPropertyName<TSubClassType>(Expression<Func<TSubClassType, object>> expression)
			{
				MemberExpression body = expression.Body as MemberExpression;
				if (body != null)
				{
					var propName = (body.Member is PropertyInfo) ? body.Member.Name : string.Empty;
					return propName;
				}

				var exp2 = expression.Body as System.Linq.Expressions.UnaryExpression;
				if (exp2 != null)
				{
					body = exp2.Operand as MemberExpression;
					var propName = (body.Member is PropertyInfo) ? body.Member.Name : string.Empty;
					return propName;
				}
				else
				{

					throw new InvalidOperationException("The expression inputed should be like \"x=>x.PropertyName\" but currently is not:" + expression.ToString());
				}

			}





		}

	}

}


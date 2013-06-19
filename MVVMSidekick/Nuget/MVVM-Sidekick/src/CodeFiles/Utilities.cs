using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Commands;
using System.Runtime.CompilerServices;
using MVVMSidekick.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using MVVMSidekick.EventRouting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Collections;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using System.Collections.Concurrent;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Xaml.Controls.Primitives;

#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;

#elif SILVERLIGHT_5||SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8||WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif


namespace MVVMSidekick
{
    namespace Utilities
    {

        /// <summary>
        /// Unify Task(4.5) and TaskEx (SL5) method in this helper
        /// </summary>
        public static class TaskExHelper
        {

            public static async Task Yield()
            {
#if SILVERLIGHT_5
            await TaskEx.Yield();
#elif NET40
            await Task.Factory.StartNew(() => { });
#else
                await Task.Yield();
#endif

            }

            public static async Task<T> FromResult<T>(T result)
            {
#if SILVERLIGHT_5
            return await TaskEx.FromResult(result);
#elif NET40
            return await Task.Factory.StartNew(() => result);
#else
                return await Task.FromResult(result);
#endif

            }

            public static async Task Delay(int ms)
            {

#if SILVERLIGHT_5
            await TaskEx.Delay(ms);
        
#elif NET40
            var task = new Task(() => { });
            using (var tm = new System.Threading.Timer(o => task.Start()))
            {
                tm.Change(ms, -1);
                await task;
            }
#else

                await Task.Delay(ms);
#endif

            }

        }
        /// <summary>
        /// Unify Type(4.5 SL & WP) and TypeInfo (Windows Runtime) class in this helper
        /// </summary>
        public static class TypeInfoHelper
        {
#if NETFX_CORE
        public static TypeInfo GetTypeOrTypeInfo(this Type type)
        {
            return type.GetTypeInfo();

        }
#else
            public static Type GetTypeOrTypeInfo(this Type type)
            {
                return type;

            }
#endif

        }


        public static class ColllectionHelper
        {


            public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
            {

                return new ObservableCollection<T>(items);
            }
        
        }

#if WINDOWS_PHONE_7
    public class Lazy<T>
    {
        public Lazy(Func<T> factory)
        { 
            _factory =()=>
            {
                lock(this)
                {
                    if(_value ==default(T))
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
               return _value ==default(T)?_factory()：_value; 
            }
        
            set
            {
                _value=value;
            } 
        }
    }



#endif

        public class ExpressionHelper
        {
            public static string GetPropertyName<TSubClassType, TProperty>(Expression<Func<TSubClassType, TProperty>> expression)
            {
                MemberExpression body = expression.Body as MemberExpression;
                var propName = (body.Member is PropertyInfo) ? body.Member.Name : string.Empty;
                return propName;
            }



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

                    throw new Exception();
                }

            }





        }


    }
}

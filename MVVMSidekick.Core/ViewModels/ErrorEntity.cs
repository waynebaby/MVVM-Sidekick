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
#if WINDOWS_UWP


#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;
using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;
using System.Windows.Threading;
#elif SILVERLIGHT_5 || SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
#elif WINDOWS_PHONE_8 || WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
#endif






namespace MVVMSidekick
{

    namespace ViewModels
    {
        /// <summary>
        /// 错误实体类，表示验证或业务逻辑错误信息
        /// Error entity class that represents validation or business logic error information
        /// </summary>
        public class ErrorEntity
        {
            /// <summary>
            /// 初始化ErrorEntity类的新实例
            /// Initializes a new instance of the ErrorEntity class
            /// </summary>
            public ErrorEntity()
            {
            }

            /// <summary>
            /// 获取或设置规则名称
            /// Gets or sets the rule name
            /// </summary>
            /// <value>规则名称 / The rule name</value>
            public string RuleName { get; set; }

            /// <summary>
            /// 获取或设置友好显示名称
            /// Gets or sets the friendly display name
            /// </summary>
            /// <value>友好名称 / The friendly name</value>
            public string FriendlyName { get; set; }

            /// <summary>
            /// 获取或设置属性名称
            /// Gets or sets the property name
            /// </summary>
            /// <value>属性名称 / The property name</value>
            public string PropertyName { get; set; }
            /// <summary>
            /// 获取或设置消息
            /// Gets or sets the message.
            /// </summary>
            /// <value>消息 / The message.</value>
            public string Message { get; set; }
            /// <summary>
            /// 获取或设置异常
            /// Gets or sets the exception.
            /// </summary>
            /// <value>异常 / The exception.</value>
            public Exception Exception { get; set; }
            /// <summary>
            /// 获取或设置内部错误信息源
            /// Gets or sets the inner error information source.
            /// </summary>
            /// <value>内部错误信息源 / The inner error information source.</value>
            public IErrorInfo InnerErrorInfoSource { get; set; }
            /// <summary>
            /// 返回表示此实例的 <see cref="System.String" />
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>表示此实例的 <see cref="System.String" /> / A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return $"{(string.IsNullOrEmpty(FriendlyName )? PropertyName:FriendlyName)}:{(Message ?? nameof(ErrorEntity))}";// string.Format("{0}，{1}，{2}", Message, Exception, InnerErrorInfoSource);
            }
        }





    }

}

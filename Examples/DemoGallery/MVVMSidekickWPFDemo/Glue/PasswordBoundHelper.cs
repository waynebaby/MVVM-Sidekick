using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MVVMSidekickWPFDemo.Glue
{
    /// <summary>
    /// 密码绑定助手类，提供密码框的绑定支持
    /// Password bound helper class providing binding support for password boxes
    /// </summary>
    public  class PasswordBoundHelper:DependencyObject
    {

        /// <summary>
        /// 获取密码绑定值
        /// Gets the password bound value
        /// </summary>
        /// <param name="obj">依赖对象 / Dependency object</param>
        /// <returns>密码绑定值 / Password bound value</returns>
        public static string GetPasswordBound(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordBoundProperty);
        }

        /// <summary>
        /// 设置密码绑定值
        /// Sets the password bound value
        /// </summary>
        /// <param name="obj">依赖对象 / Dependency object</param>
        /// <param name="value">要设置的值 / Value to set</param>
        public static void SetPasswordBound(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordBoundProperty, value);
        }

        /// <summary>
        /// 密码绑定依赖属性，支持动画、样式、绑定等功能
        /// Password bound dependency property that enables animation, styling, binding, etc.
        /// </summary>
        public static readonly DependencyProperty PasswordBoundProperty =
            DependencyProperty.RegisterAttached(nameof(PasswordBoundProperty).Substring(0, nameof(PasswordBoundProperty).Length - "Property".Length), typeof(string), typeof(PasswordBoundHelper), new PropertyMetadata(""));



    }
}

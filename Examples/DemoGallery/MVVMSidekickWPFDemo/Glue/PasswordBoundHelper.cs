using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MVVMSidekickWPFDemo.Glue
{
    public  class PasswordBoundHelper:DependencyObject
    {


        public static string GetPasswordBound(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordBoundProperty);
        }

        public static void SetPasswordBound(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordBoundProperty, value);
        }

        // Using a DependencyProperty as the backing store for PasswordBound.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordBoundProperty =
            DependencyProperty.RegisterAttached(nameof(PasswordBoundProperty).Substring(0, nameof(PasswordBoundProperty).Length - "Property".Length), typeof(string), typeof(PasswordBoundHelper), new PropertyMetadata(""));



    }
}

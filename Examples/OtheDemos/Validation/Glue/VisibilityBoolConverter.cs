using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Validation.Glue
{
    /// <summary>
    /// <para>布尔值到可见性的转换器</para>
    /// <para>Boolean to Visibility converter</para>
    /// </summary>
    public class VisibilityBoolConverter : IValueConverter
    {
        /// <summary>
        /// <para>将布尔值转换为可见性枚举</para>
        /// <para>Converts boolean value to Visibility enumeration</para>
        /// </summary>
        /// <param name="value">要转换的布尔值 / Boolean value to convert</param>
        /// <param name="targetType">目标类型 / Target type</param>
        /// <param name="parameter">转换参数 / Conversion parameter</param>
        /// <param name="language">语言标识符 / Language identifier</param>
        /// <returns>可见性枚举值 / Visibility enumeration value</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = (bool)value;
         
            return v ? Visibility.Visible:Visibility.Collapsed;

        }

        /// <summary>
        /// <para>将可见性枚举转换回布尔值</para>
        /// <para>Converts Visibility enumeration back to boolean value</para>
        /// </summary>
        /// <param name="value">要转换的可见性值 / Visibility value to convert</param>
        /// <param name="targetType">目标类型 / Target type</param>
        /// <param name="parameter">转换参数 / Conversion parameter</param>
        /// <param name="language">语言标识符 / Language identifier</param>
        /// <returns>布尔值 / Boolean value</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Object.Equals(value, Visibility.Visible);
        }
    }


}

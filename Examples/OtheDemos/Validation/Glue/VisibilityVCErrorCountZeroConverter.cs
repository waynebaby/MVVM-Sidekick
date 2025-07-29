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
    /// <para>错误计数非零到可见性的转换器</para>
    /// <para>Error count non-zero to Visibility converter</para>
    /// </summary>
    public class VisibilityVCErrorCountZeroConverter : IValueConverter
    {
        /// <summary>
        /// <para>将错误计数转换为可见性，非零时显示</para>
        /// <para>Converts error count to Visibility, visible when non-zero</para>
        /// </summary>
        /// <param name="value">错误计数值 / Error count value</param>
        /// <param name="targetType">目标类型 / Target type</param>
        /// <param name="parameter">转换参数 / Conversion parameter</param>
        /// <param name="language">语言标识符 / Language identifier</param>
        /// <returns>可见性枚举值 / Visibility enumeration value</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = (int)value != 0;

            return v ? Visibility.Visible : Visibility.Collapsed;

        }

        /// <summary>
        /// <para>反向转换未实现</para>
        /// <para>Reverse conversion not implemented</para>
        /// </summary>
        /// <param name="value">要转换的可见性值 / Visibility value to convert</param>
        /// <param name="targetType">目标类型 / Target type</param>
        /// <param name="parameter">转换参数 / Conversion parameter</param>
        /// <param name="language">语言标识符 / Language identifier</param>
        /// <returns>抛出未实现异常 / Throws NotImplementedException</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

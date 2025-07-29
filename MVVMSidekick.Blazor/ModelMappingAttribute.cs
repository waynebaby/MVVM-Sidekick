using System;
using System.Linq.Expressions;

namespace MVVMSidekick.Views
{
    /// <summary>
    /// 模型映射特性，用于属性的模型映射配置
    /// Model mapping attribute for property model mapping configuration
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ModelMappingAttribute : Attribute
    {
        /// <summary>
        /// 获取或设置映射到的属性名称
        /// Gets or sets the property name to map to
        /// </summary>
        public string MapToProperty { get; set; }
        
        /// <summary>
        /// 获取或设置是否忽略此属性
        /// Gets or sets whether to ignore this property
        /// </summary>
        public bool Ignore { get; set; }

    }
}

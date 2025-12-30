#if BLAZOR
using System;
using System.Linq.Expressions;

namespace MVVMSidekick.Views
{
    /// <summary>
    /// <para>模型映射特性，用于标记属性与模型的映射关系</para>
    /// <para>Model mapping attribute, used to mark the mapping relationship between properties and models</para>
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ModelMappingAttribute : Attribute
    {
        /// <summary>
        /// <para>获取或设置映射到的属性名称</para>
        /// <para>Gets or sets the property name to map to</para>
        /// </summary>
        public string MapToProperty { get; set; }
        
        /// <summary>
        /// <para>获取或设置是否忽略此属性</para>
        /// <para>Gets or sets whether to ignore this property</para>
        /// </summary>
        public bool Ignore { get; set; }
    }
}
#endif

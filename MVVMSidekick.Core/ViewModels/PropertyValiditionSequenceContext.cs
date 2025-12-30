using System;
using System.Linq.Expressions;

namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// <para>属性验证序列上下文结构体</para>
    /// <para>Structure for property validation sequence context</para>
    /// </summary>
    /// <typeparam name="TModel">
    /// <para>模型的类型</para>
    /// <para>The type of the model</para>
    /// </typeparam>
    /// <typeparam name="TValue">
    /// <para>属性值的类型</para>
    /// <para>The type of the property value</para>
    /// </typeparam>
    public struct PropertyValiditionSequenceContext<TModel,TValue>
    {
        /// <summary>
        /// <para>获取或设置模型上下文</para>
        /// <para>Gets or sets the model context</para>
        /// </summary>
        public ModelValiditionSequenceContext<TModel> ModelContext { get; set; }
        
        /// <summary>
        /// <para>获取或设置属性表达式</para>
        /// <para>Gets or sets the property expression</para>
        /// </summary>
        public Expression<Func<TModel, TValue>> PropertyExpression { get; set; }
        
        /// <summary>
        /// <para>获取或设置属性名称</para>
        /// <para>Gets or sets the property name</para>
        /// </summary>
        public string PropertyName { get; set; }
    }
}

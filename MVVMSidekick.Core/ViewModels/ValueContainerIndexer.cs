using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text;

namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// <para>值容器索引器，提供通过属性名访问值容器的功能</para>
    /// <para>Value container indexer that provides access to value containers by property name</para>
    /// </summary>
    public class ValueContainerIndexer// : DynamicObject
    {
        /// <summary>
        /// <para>初始化ValueContainerIndexer的新实例</para>
        /// <para>Initializes a new instance of the ValueContainerIndexer class</para>
        /// </summary>
        /// <param name="model">
        /// <para>绑定模型实例</para>
        /// <para>The bindable model instance</para>
        /// </param>
        public ValueContainerIndexer(IBindable model)
        {
            _model = model;
        }

        /// <summary>
        /// <para>绑定模型字段</para>
        /// <para>The bindable model field</para>
        /// </summary>
        IBindable _model;

        /// <summary>
        /// <para>通过属性名获取值容器</para>
        /// <para>Gets the value container by property name</para>
        /// </summary>
        /// <param name="propertyName">
        /// <para>属性名称</para>
        /// <para>The property name</para>
        /// </param>
        /// <returns>
        /// <para>对应属性的值容器</para>
        /// <para>The value container for the specified property</para>
        /// </returns>
        public IValueContainer this[string propertyName]
        {
            get
            {
                return _model.GetValueContainer(propertyName);
            }

        }
        

    }



}

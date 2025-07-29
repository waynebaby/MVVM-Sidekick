using MVVMSidekick.Common;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// <para>BindableBase 的扩展方法集合</para>
    /// <para>Extension methods collection for BindableBase</para>
    /// </summary>
    public static class BindableBaseExtensions
    {



        /// <summary>
        /// <para>配置值容器使用委托</para>
        /// <para>Configures Value Container with delegate</para>
        /// </summary>
        /// <typeparam name="TProperty">
        /// <para>值容器内容的类型</para>
        /// <para>Type of ValueContainer content</para>
        /// </typeparam>
        /// <param name="target">
        /// <para>值容器的配置目标实例</para>
        /// <para>Target ValueContainer instance to configure</para>
        /// </param>
        /// <param name="action">
        /// <para>配置操作</para>
        /// <para>Configuration action</para>
        /// </param>
        /// <returns>
        /// <para>配置后的值容器实例</para>
        /// <para>Configured ValueContainer instance</para>
        /// </returns>
        public static ValueContainer<TProperty> Config<TProperty>(this ValueContainer<TProperty> target, Action<ValueContainer<TProperty>> action)
        {
            action(target);
            return target;
        }

        /// <summary>
        /// <para>将 IDisposable 对象添加到释放组</para>
        /// <para>Adds IDisposable object to dispose group</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>IDisposable 对象的类型</para>
        /// <para>Type of IDisposable object</para>
        /// </typeparam>
        /// <param name="item">
        /// <para>IDisposable 实例</para>
        /// <para>IDisposable instance</para>
        /// </param>
        /// <param name="targetGroup">
        /// <para>目标释放组</para>
        /// <para>Target dispose group</para>
        /// </param>
        /// <param name="needCheckInFinalizer">
        /// <para>是否需要在终结器中检查</para>
        /// <para>Whether to check in finalizer</para>
        /// </param>
        /// <param name="comment">
        /// <para>注释信息</para>
        /// <para>Comment information</para>
        /// </param>
        /// <param name="caller">
        /// <para>调用者名称</para>
        /// <para>Caller name</para>
        /// </param>
        /// <param name="file">
        /// <para>文件路径</para>
        /// <para>File path</para>
        /// </param>
        /// <param name="line">
        /// <para>行号</para>
        /// <para>Line number</para>
        /// </param>
        /// <returns>
        /// <para>原始对象实例</para>
        /// <para>Original object instance</para>
        /// </returns>
        public static T DisposeWith<T>(this T item, IDisposeGroup targetGroup, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1) where T : IDisposable
        {

            targetGroup.AddDisposable(item, needCheckInFinalizer, comment, caller, file, line);
            return item;


        }

        /// <summary>
        /// 初始化值容器（使用默认值工厂）
        /// Initialize value container (using default value factory)
        /// </summary>
        /// <typeparam name="TValue">值类型 / The value type</typeparam>
        /// <param name="model">模型实例 / The model instance</param>
        /// <param name="propertyName">属性名称 / The property name</param>
        /// <param name="reference">属性引用 / The property reference</param>
        /// <param name="locator">定位器函数 / The locator function</param>
        /// <param name="defaultValueFactory">默认值工厂 / The default value factory</param>
        /// <returns>值容器实例 / The value container instance</returns>
        public static ValueContainer<TValue> Initialize<TValue>(this BindableBase model, string propertyName, ref Property<TValue> reference, ref Func<BindableBase, ValueContainer<TValue>> locator, Func<TValue> defaultValueFactory = null)
        {
            return model.Initialize(propertyName, ref reference, ref locator, defaultValueFactory == null ? default(TValue) : defaultValueFactory.Invoke());
        }

        /// <summary>
        /// 初始化值容器（使用默认值）
        /// Initialize value container (using default value)
        /// </summary>
        /// <typeparam name="TValue">值类型 / The value type</typeparam>
        /// <param name="model">模型实例 / The model instance</param>
        /// <param name="propertyName">属性名称 / The property name</param>
        /// <param name="reference">属性引用 / The property reference</param>
        /// <param name="locator">定位器函数 / The locator function</param>
        /// <param name="defaultValue">默认值 / The default value</param>
        /// <returns>值容器实例 / The value container instance</returns>
        public static ValueContainer<TValue> Initialize<TValue>(this BindableBase model, string propertyName, ref Property<TValue> reference, ref Func<BindableBase, ValueContainer<TValue>> locator, TValue defaultValue = default)
        {
            if (reference == null)
                reference = new Property<TValue>(locator);
            if (reference.Container == null)
            {
                reference.Container = new ValueContainer<TValue>(propertyName, model);
                reference.Container.SetValue(defaultValue);
            }
            return reference.Container;
        }

        /// <summary>
        /// 初始化值容器（使用模型相关的默认值工厂）
        /// Initialize value container (using model-related default value factory)
        /// </summary>
        /// <typeparam name="TModel">模型类型 / The model type</typeparam>
        /// <typeparam name="TValue">值类型 / The value type</typeparam>
        /// <param name="model">模型实例 / The model instance</param>
        /// <param name="propertyName">属性名称 / The property name</param>
        /// <param name="reference">属性引用 / The property reference</param>
        /// <param name="locator">定位器函数 / The locator function</param>
        /// <param name="defaultValueFactory">默认值工厂 / The default value factory</param>
        /// <returns>值容器实例 / The value container instance</returns>
        public static ValueContainer<TValue> Initialize<TModel, TValue>(this TModel model, string propertyName, ref Property<TValue> reference, ref Func<BindableBase, ValueContainer<TValue>> locator, Func<TModel, TValue> defaultValueFactory = null) where TModel : BindableBase
        {
            return model.Initialize(propertyName, ref reference, ref locator, defaultValueFactory == null ? default(TValue) : defaultValueFactory.Invoke(model));
        }
    }

}

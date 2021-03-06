﻿using MVVMSidekick.Common;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// <para>Extension methods of models</para>
    /// <para>为Model增加的一些快捷方法</para>
    /// </summary>
    public static class BindableBaseExtensions
    {



        /// <summary>
        /// <para>Config Value Container with delegate</para>
        /// <para>使用连续的API设置ValueContainer的一些参数</para>
        /// </summary>
        /// <typeparam name="TProperty">ValueContainer内容的类型</typeparam>
        /// <param name="target">ValueContainer的配置目标实例</param>
        /// <param name="action">配置内容</param>
        /// <returns>ValueContainer的配置目标实例</returns>
        public static ValueContainer<TProperty> Config<TProperty>(this ValueContainer<TProperty> target, Action<ValueContainer<TProperty>> action)
        {
            action(target);
            return target;
        }

        /// <summary>
        /// <para>Add Idisposeable to model's despose action list</para>
        /// <para>将IDisposable 对象注册到VM中的销毁对象列表。</para>
        /// </summary>
        /// <typeparam name="T">Type of Model /Model的类型</typeparam>
        /// <param name="item">IDisposable Inastance/IDisposable实例</param>
        /// <param name="targetGroup">The tg.</param>
        /// <param name="needCheckInFinalizer">if set to <c>true</c> [need check in finalizer].</param>
        /// <param name="comment">The comment.</param>
        /// <param name="caller">The caller.</param>
        /// <param name="file">The file.</param>
        /// <param name="line">The line.</param>
        /// <returns>T.</returns>
        public static T DisposeWith<T>(this T item, IDisposeGroup targetGroup, bool needCheckInFinalizer = false, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1) where T : IDisposable
        {

            targetGroup.AddDisposable(item, needCheckInFinalizer, comment, caller, file, line);
            return item;


        }

        /// <summary>
        /// Initializes the specified property name.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>ValueContainer&lt;T&gt;.</returns>
        public static ValueContainer<TValue> Initialize<TValue>(this BindableBase model, string propertyName, ref Property<TValue> reference, ref Func<BindableBase, ValueContainer<TValue>> locator, Func<TValue> defaultValueFactory = null)
        {
            return model.Initialize(propertyName, ref reference, ref locator, defaultValueFactory == null ? default(TValue) : defaultValueFactory.Invoke());
        }


        /// <summary>
        /// Initializes the specified property name.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>ValueContainer&lt;T&gt;.</returns>
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
        /// Initializes the specified property name.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>ValueContainer&lt;T&gt;.</returns>
        public static ValueContainer<TValue> Initialize<TModel, TValue>(this TModel model, string propertyName, ref Property<TValue> reference, ref Func<BindableBase, ValueContainer<TValue>> locator, Func<TModel, TValue> defaultValueFactory = null) where TModel : BindableBase
        {
            return model.Initialize(propertyName, ref reference, ref locator, defaultValueFactory == null ? default(TValue) : defaultValueFactory.Invoke(model));
        }
    }

}

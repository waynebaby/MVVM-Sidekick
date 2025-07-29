using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;

namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// <para>模型验证序列上下文结构体</para>
    /// <para>Structure for model validation sequence context</para>
    /// </summary>
    /// <typeparam name="TModel">
    /// <para>模型的类型</para>
    /// <para>The type of the model</para>
    /// </typeparam>
    public struct ModelValiditionSequenceContext<TModel>
    {
        /// <summary>
        /// <para>获取或设置模型实例</para>
        /// <para>Gets or sets the model instance</para>
        /// </summary>
        public TModel Model { get; set; }
        
        /// <summary>
        /// <para>获取或设置监听变化的序列</para>
        /// <para>Gets or sets the sequence for listening to changes</para>
        /// </summary>
        public IObservable<(TModel Model, IValueContainer ValueContainer, ValueChangedEventArgs EventArgs)> ListenChangedSequence { get; set; }
        
        /// <summary>
        /// <para>获取或设置被监听字段的延迟字典</para>
        /// <para>Gets or sets the lazy dictionary of fields being listened to</para>
        /// </summary>
        public Lazy<IDictionary<string, IValueContainer>> FieldsListenedTo { get; set; }
    }
}

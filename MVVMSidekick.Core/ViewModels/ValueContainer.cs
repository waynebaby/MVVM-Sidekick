// ***********************************************************************
// Assembly         : MVVMSidekick_Wp8
// Author           : waywa
// Created          : 05-17-2014
//
// Last Modified By : waywa
// Last Modified On : 01-04-2015
// ***********************************************************************
// <copyright file="ViewModels.cs" company="">
//     Copyright ©  2012
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using MVVMSidekick.Reactive;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;

namespace MVVMSidekick
{
    namespace ViewModels
    {
        /// <summary>
        /// 值容器，保存特定字段的值，具有通知和比较支持
        /// Value Container, holds the value of certain field, with notification and compare support
        /// </summary>
        /// <typeparam name="TProperty">属性值的类型 / Type of the property value</typeparam>
        public class ValueContainer<TProperty> : IErrorInfo, IValueCanSet<TProperty>, IValueCanGet<TProperty>, IValueContainer, INotifyChanged<TProperty>, INotifyChanging<TProperty>//, INotifyPropertyChanged ,INotifyPropertyChanging
        {
            #region Constructors /构造器
            /// <summary>
            /// 创建新的值容器
            /// Create a new Value Container
            /// </summary>
            /// <param name="info">属性名 / Property name</param>
            /// <param name="model">值容器将被持有的模型实例 / The model that Value Container will be held with</param>
            /// <param name="initValue">此容器的初始值 / The first value of this container</param>
            public ValueContainer(string info, BindableBase model, TProperty initValue = default(TProperty))
                : this(info, model, (v1, v2) =>
                    {
                        if (v1 == null)
                        {
                            if (v2 == null)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (v2 == null)
                        {
                            return false;
                        }
                        else
                        {
                            return v1.Equals(v2);
                        }
                    }, initValue)
            {
            }

            /// <summary>
            /// 创建新的值容器
            /// Create a new Value Container
            /// </summary>
            /// <param name="info">属性名 / Property name</param>
            /// <param name="model">值容器将被持有的模型实例 / The model that Value Container will be held with</param>
            /// <param name="equalityComparer">新值/旧值的比较器，用于通知 / Comparer of new/old value, for notification</param>
            /// <param name="initValue">此容器的初始值 / The first value of this container</param>
            public ValueContainer(string info, BindableBase model, Func<TProperty, TProperty, bool> equalityComparer, TProperty initValue = default(TProperty))
            {
                EqualityComparer = equalityComparer;
                PropertyName = info;
                PropertyType = typeof(TProperty);
                Model = model;
                SetValue(initValue);
                _Errors = new Dictionary<string, ErrorEntity>();
            }
            #endregion



            /// <summary>
            /// 获取新值/旧值的比较器实例，用于通知
            /// Gets comparer instance of new/old value, for notification
            /// </summary>
            /// <value>相等性比较器 / The equality comparer</value>
            public Func<TProperty, TProperty, bool> EqualityComparer { get; private set; }

            /// <summary>
            /// 属性名
            /// Property name
            /// </summary>
            /// <value>属性名称 / The name of the property</value>
            public string PropertyName { get; private set; }

            /// <summary>
            /// 内部值字段
            /// The internal value field
            /// </summary>
            TProperty _value;

            /// <summary>
            /// 获取或设置值
            /// Gets or sets the value
            /// </summary>
            /// <value>值 / The value</value>
            public TProperty Value
            {
                get { return _value; }
                set
                {
                    SetValueAndTryNotify(value);
                }
            }

            /// <summary>
            /// 保存值并尝试引发值变化事件。警告：这将启动一个异步UI线程任务，不会导致UI忙碌
            /// Save the value and try raise the value changed event. Warning, it will start an Async UI thread task, which will not cause UI busy
            /// </summary>
            /// <param name="value">新值 / New value</param>
            /// <returns>值容器实例 / ValueContainer instance</returns>
            public ValueContainer<TProperty> SetValueAndTryNotify(TProperty value)
            {
                WireINPCValue(value);
                WireINCCValue(value);
                InternalPropertyChange(this.Model, value, PropertyName);
                return this;
            }

            /// <summary>
            /// 仅保存值，不尝试引发值变化事件
            /// Save the value and do not try raise the value changed event
            /// </summary>
            /// <param name="value">新值 / New value</param>
            /// <returns>值容器实例 / ValueContainer instance</returns>
            public ValueContainer<TProperty> SetValue(TProperty value)
            {
                WireINPCValue(value);
                WireINCCValue(value);
                _value = value;
                return this;
            }

            /// <summary>
            /// 内部属性更改处理
            /// Internal property change processing
            /// </summary>
            /// <param name="modelInstance">模型实例 / The model instance</param>
            /// <param name="newValue">新值 / The new value</param>
            /// <param name="message">消息 / The message</param>
            private async void InternalPropertyChange(BindableBase modelInstance, TProperty newValue, string message)
            {
                //通过确保它们不相等来找出是否会有变化
                //find out there will be a changing by making sure they are not equal
                var changing = (this.EqualityComparer != null) ?
                    !this.EqualityComparer(newValue, _value) :
                    !Object.Equals(newValue, _value);

                if (!changing)
                {
                    return;
                }
                
                //触发变化事件，询问是否有人反对变化
                //fire changing event ask if anyone against changing
                var changingArg = new ValueChangingEventArgs<TProperty>(message, _value, newValue);

                modelInstance.RaisePropertyChanging(changingArg);
                var oldvalue = _value;
                _value = newValue;

                if (changingArg.Cancellation.IsCancellationRequested)
                {
                    _value = oldvalue;
                    return;
                }

                if (ValueChanging != null)
                {
                    ValueChanging.Invoke(this, changingArg);
                    await Task.Yield();
                    if (changingArg.Cancellation.IsCancellationRequested)
                    {
                        _value = oldvalue;
                        return;
                    }
                }

                if (NonGenericValueChanging != null)
                {
                    NonGenericValueChanging.Invoke(this, changingArg);
                    await Task.Yield();

                    if (changingArg.Cancellation.IsCancellationRequested)
                    {
                        _value = oldvalue;
                        return;
                    }
                }

                ValueChangedEventArgs<TProperty> changedArg = new ValueChangedEventArgs<TProperty>(message, oldvalue, newValue);

                modelInstance.RaisePropertyChanged(changedArg);
                ValueChanged?.Invoke(this, changedArg);
                NonGenericValueChanged?.Invoke(this, changedArg);
            }

            /// <summary>
            /// 绑定INotifyPropertyChanged值的变化监听
            /// Wire INotifyPropertyChanged value change listening
            /// </summary>
            /// <param name="newValue">新值 / New value</param>
            private void WireINPCValue(TProperty newValue)
            {
                var inpcNewValue = newValue as INotifyPropertyChanged;
                var inpcOldValue = _value as INotifyPropertyChanged;

                if (inpcNewValue != null)
                {
                    inpcNewValue.PropertyChanged += InpcValue_PropertyChanged;
                }
                if (inpcOldValue != null)
                {
                    inpcOldValue.PropertyChanged -= InpcValue_PropertyChanged;
                }


                var inpcingNewValue = newValue as INotifyPropertyChanging;
                var inpcingOldValue = _value as INotifyPropertyChanging;

                if (inpcingNewValue != null)
                {
                    inpcingNewValue.PropertyChanging += InpcingNewValue_PropertyChanging;
                }
                if (inpcingOldValue != null)
                {
                    inpcingOldValue.PropertyChanging -= InpcingNewValue_PropertyChanging;
                }
            }

            /// <summary>
            /// 处理内部值的属性正在变化事件
            /// Handles the property changing event of internal value
            /// </summary>
            /// <param name="sender">事件发送者 / Event sender</param>
            /// <param name="e">属性变化事件参数 / Property changing event args</param>
            private void InpcingNewValue_PropertyChanging(object sender, PropertyChangingEventArgs e)
            {
                Model.RaisePropertyChanging(e, sender);
            }

            /// <summary>
            /// 处理内部值的属性已变化事件
            /// Handles the property changed event of internal value
            /// </summary>
            /// <param name="sender">事件发送者 / Event sender</param>
            /// <param name="e">属性变化事件参数 / Property changed event args</param>
            private void InpcValue_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                Model.RaisePropertyChanged(e, sender);
                Model.RaisePropertyChanged(new ValueChangedEventArgs<TProperty>(this.PropertyName, _value, _value));
            }

            /// <summary>
            /// 绑定INotifyCollectionChanged值的变化监听
            /// Wire INotifyCollectionChanged value change listening
            /// </summary>
            /// <param name="newValue">新值 / New value</param>
            private void WireINCCValue(TProperty newValue)
            {
                var inccNewValue = newValue as INotifyCollectionChanged;
                var inccOldValue = _value as INotifyCollectionChanged;

                if (inccNewValue != null)
                {
                    inccNewValue.CollectionChanged += InccValue_CollectionChanged;
                }
                if (inccOldValue != null)
                {
                    inccOldValue.CollectionChanged -= InccValue_CollectionChanged;
                }
            }

            /// <summary>
            /// 处理内部值的集合变化事件
            /// Handles the collection changed event of internal value
            /// </summary>
            /// <param name="sender">事件发送者 / Event sender</param>
            /// <param name="e">集合变化事件参数 / Collection changed event args</param>
            private void InccValue_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                Model.RaisePropertyChanged(new PropertyChangedEventArgs(this.PropertyName), this);
            }

            /// <summary>
            /// 添加错误条目
            /// Add error entry
            /// </summary>
            /// <param name="ruleName">规则名称 / Rule name</param>
            /// <param name="message">错误消息 / Error message</param>
            /// <param name="exception">异常（可选）/ Exception (optional)</param>
            public void AddErrorEntry(string ruleName, string message, Exception exception = null)
            {
                _Errors[ruleName] = new ViewModels.ErrorEntity
                {
                    FriendlyName = PropertyName,
                    RuleName = ruleName,
                    Exception = exception,
                    InnerErrorInfoSource = this,
                    PropertyName = PropertyName,
                    Message = message
                };
                Model.RaiseErrorsChanged(PropertyName);
            }
            
            /// <summary>
            /// 移除错误条目
            /// Remove error entry
            /// </summary>
            /// <param name="ruleName">规则名称 / Rule name</param>
            public void RemoveErrorEntry(string ruleName)
            {
                _Errors.Remove(ruleName);
                Model.RaiseErrorsChanged(PropertyName);
            }
            
            /// <summary>
            /// 清除所有错误条目
            /// Clear all error entries
            /// </summary>
            public void ClearErrorEntries()
            {
                _Errors.Clear();
                Model.RaiseErrorsChanged(PropertyName);
            }

            /// <summary>
            /// 值容器所在的模型实例
            /// The model instance that Value Container was held
            /// </summary>
            /// <value>模型实例 / The model</value>
            public BindableBase Model { get; internal set; }





            /// <summary>
            /// IValueContainer接口的Value属性实现
            /// Implementation of Value property from IValueContainer interface
            /// </summary>
            /// <value>属性值对象 / The property value as object</value>
            object IValueContainer.Value
            {
                get
                {
                    return Value;
                }
                set
                {
                    SetValueAndTryNotify((TProperty)value);
                }
            }

            /// <summary>
            /// 获取属性的类型信息
            /// Gets the type of property
            /// </summary>
            /// <value>属性类型 / The type of the property</value>
            public Type PropertyType
            {
                get;
                private set;
            }

            /// <summary>
            /// 错误条目存储字典
            /// The storage dictionary for error entries
            /// </summary>
            IDictionary<string, ErrorEntity> _Errors;


            /// <summary>
            /// 获取只读的错误条目字典
            /// Gets the read-only dictionary of error entries
            /// </summary>
            /// <value>只读错误字典 / The read-only errors dictionary</value>
            public IReadOnlyDictionary<string, ErrorEntity> Errors
            {
                get { return new ReadOnlyDictionary<string, ErrorEntity>(_Errors); }
            }

            /// <summary>
            /// 非泛型值变化事件
            /// Non-generic value changed event
            /// </summary>
            public event EventHandler<ValueChangedEventArgs> NonGenericValueChanged;
            
            /// <summary>
            /// 非泛型值正在变化事件
            /// Non-generic value changing event
            /// </summary>
            public event EventHandler<ValueChangingEventArgs> NonGenericValueChanging;
            
            /// <summary>
            /// 泛型值变化事件
            /// Generic value changed event
            /// </summary>
            public event EventHandler<ValueChangedEventArgs<TProperty>> ValueChanged;
            
            /// <summary>
            /// 泛型值正在变化事件
            /// Generic value changing event
            /// </summary>
            public event EventHandler<ValueChangingEventArgs<TProperty>> ValueChanging;


            /// <summary>
            /// 是否允许复制标志（默认情况下ICommand类型不允许复制）
            /// Flag indicating whether copying is allowed (ICommand types are not allowed by default)
            /// </summary>
            bool _IsCopyToAllowed = !typeof(ICommand).GetTypeInfo().IsAssignableFrom(typeof(TProperty).GetTypeInfo());

            /// <summary>
            /// 获取或设置是否可以通过CopyTo方法复制到另一个属性
            /// Gets or sets whether this instance can be copied by CopyTo method
            /// </summary>
            /// <value>如果允许复制则为true，否则为false / true if this instance is copy to allowed; otherwise, false</value>
            public bool IsCopyToAllowed
            {
                get { return _IsCopyToAllowed; }
                set { _IsCopyToAllowed = value; }
            }

            /// <summary>
            /// IValueContainer接口的Model属性实现
            /// Implementation of Model property from IValueContainer interface
            /// </summary>
            object IValueContainer.Model => this.Model;


        }





    }

}

﻿// ***********************************************************************
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

namespace MVVMSidekick
{

    namespace ViewModels
    {
        /// <summary>
        /// <para>Value Container, holds the value of certain field, with notifition /and compare support</para>
        /// <para>值容器</para>
        /// </summary>
        /// <typeparam name="TProperty">Type of the property value /属性的类型</typeparam>
        public class ValueContainer<TProperty> : IErrorInfo, IValueCanSet<TProperty>, IValueCanGet<TProperty>, IValueContainer, INotifyChanged<TProperty>, INotifyChanging<TProperty>//, INotifyPropertyChanged ,INotifyPropertyChanging
        {


            #region Constructors /构造器
            /// <summary>
            /// <para>Create a new Value Container</para>
            /// <para>创建属性值容器</para>
            /// </summary>
            /// <param name="info">Property name/属性名</param>
            /// <param name="model"><para>The model that Value Container will be held with.</para>
            /// <para>所属的model实例</para></param>
            /// <param name="initValue">The first value of this container/初始值</param>
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
            /// <para>Create a new Value Container</para>
            /// <para>创建属性值容器</para>
            /// </summary>
            /// <param name="info">Property name/属性名</param>
            /// <param name="model"><para>The model that Value Container will be held with.</para>
            /// <para>所属的model实例</para></param>
            /// <param name="equalityComparer"><para>Comparer of new/old value, for notifition.</para>
            /// <para>判断两个值是否相等的比较器,用于判断是否通知变更</para></param>
            /// <param name="initValue">The first value of this container/初始值</param>
            public ValueContainer(string info, BindableBase model, Func<TProperty, TProperty, bool> equalityComparer, TProperty initValue = default(TProperty))
            {
                EqualityComparer = equalityComparer;
                PropertyName = info;
                PropertyType = typeof(TProperty);
                Model = model;
                SetValue(initValue);
                _Errors = new ObservableCollection<ErrorEntity>();
                _Errors.GetCollectionChangedEventObservable(model)
                    .Subscribe
                    (
                        e =>
                        {
                            model.RaiseErrorsChanged(PropertyName);
                        }
                    )
                    .DisposeWith(model);

            }

            #endregion



            /// <summary>
            /// <para>Gets comparer instance of new/old value, for notifition.</para>
            /// <para>读取判断两个值是否相等的比较器,用于判断是否通知变更</para>
            /// </summary>
            /// <value>The equality comparer.</value>
            public Func<TProperty, TProperty, bool> EqualityComparer { get; private set; }

            /// <summary>
            /// Property name /属性名
            /// </summary>
            /// <value>The name of the property.</value>
            public string PropertyName { get; private set; }

            /// <summary>
            /// The _value
            /// </summary>
            TProperty _value;

            /// <summary>
            /// Value/值
            /// </summary>
            /// <value>The value.</value>
            public TProperty Value
            {
                get { return _value; }
                set
                {
                    SetValueAndTryNotify(value);
                }
            }

            /// <summary>
            /// <para>Save the value and try raise the value changed event. Warning, it will start an Async UI thread task, which will not cause UI busy</para>
            /// <para>保存值并且尝试触发更改事件</para>
            /// </summary>
            /// <param name="value">New value/属性值</param>
            /// <returns>ValueContainer&lt;TProperty&gt;.</returns>
            public ValueContainer<TProperty> SetValueAndTryNotify(TProperty value)
            {
                WireINPCValue(value);
                WireINCCValue(value);
                InternalPropertyChange(this.Model, value, PropertyName);
                return this;
            }



            /// <summary>
            /// <para>Save the value and do not try raise the value changed event</para>
            /// <para>仅保存值 不尝试触发更改事件</para>
            /// </summary>
            /// <param name="value">New value/属性值</param>
            /// <returns>ValueContainer&lt;TProperty&gt;.</returns>
            public ValueContainer<TProperty> SetValue(TProperty value)
            {
                WireINPCValue(value);
                WireINCCValue(value);

                _value = value;
                return this;
            }


            /// <summary>
            /// Internals the property change.
            /// </summary>
            /// <param name="modelInstance">The model instance.</param>
            /// <param name="newValue">The new value.</param>

            /// <param name="message">The message.</param>
            private async void InternalPropertyChange(BindableBase modelInstance, TProperty newValue, string message)
            {


                //find out there will be a changing by makesure they are not equal
                var changing = (this.EqualityComparer != null) ?
                    !this.EqualityComparer(newValue, _value) :
                    !Object.Equals(newValue, _value);

                if (!changing)
                {
                    return;
                }
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

            private void InpcingNewValue_PropertyChanging(object sender, PropertyChangingEventArgs e)
            {
                Model.RaisePropertyChanging(e, sender);

            }

            private void InpcValue_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                Model.RaisePropertyChanged(e, sender);
                Model.RaisePropertyChanged(new ValueChangedEventArgs<TProperty>(this.PropertyName, _value, _value));
            }

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

            private void InccValue_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                Model.RaisePropertyChanged(new PropertyChangedEventArgs(this.PropertyName), this);
            }







            public void AddErrorEntry(string message, Exception exception = null)
            {

                Errors.Add(new ViewModels.ErrorEntity
                {
                    Exception = exception,
                    InnerErrorInfoSource = this,
                    PropertyName = PropertyName,
                    Message = message
                });

            }

            void IValueContainer.AddErrorEntry(string message, Exception exception)
            {
                throw new NotImplementedException();
            }


            /// <summary>
            /// <para>The model instance that Value Container was held.</para>
            /// <para>此值容器所在的Model</para>
            /// </summary>
            /// <value>The model.</value>
            public BindableBase Model { get; internal set; }





            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
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
            /// Gets the type of property/读取值类型
            /// </summary>
            /// <value>The type of the property.</value>
            public Type PropertyType
            {
                get;
                private set;
            }


            /// <summary>
            /// The _ errors
            /// </summary>
            ObservableCollection<ErrorEntity> _Errors;

            /// <summary>
            /// Gets the errors.
            /// </summary>
            /// <value>The errors.</value>
            public ObservableCollection<ErrorEntity> Errors
            {
                get { return _Errors; }

            }


            public event EventHandler<ValueChangedEventArgs> NonGenericValueChanged;
            public event EventHandler<ValueChangingEventArgs> NonGenericValueChanging;
            public event EventHandler<ValueChangedEventArgs<TProperty>> ValueChanged;
            public event EventHandler<ValueChangingEventArgs<TProperty>> ValueChanging;


            bool _IsCopyToAllowed = !typeof(ICommand).GetTypeInfo().IsAssignableFrom(typeof(TProperty).GetTypeInfo());

            /// <summary>
            /// <para>Can be copied by CopyTo method</para>
            /// <para>是否可以被 `Copyto` 复制到另外一个属性</para>
            /// </summary>
            /// <value><c>true</c> if this instance is copy to allowed; otherwise, <c>false</c>.</value>
            public bool IsCopyToAllowed
            {
                get { return _IsCopyToAllowed; }
                set { _IsCopyToAllowed = value; }
            }



            object IValueContainer.Model => this.Model;


        }





    }

}

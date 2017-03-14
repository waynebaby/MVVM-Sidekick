using MVVMSidekick.Reactive;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows.Input;

namespace MVVMSidekick.ViewModels
{


    /// <summary>
    /// <para>Value Container, holds the value of certain field, with notifition /and compare support</para>
    /// <para>值容器</para>
    /// </summary>
    /// <typeparam name="TProperty">Type of the property value /属性的类型</typeparam>
    public class ValueContainer<TProperty> : IErrorInfo, IValueCanSet<TProperty>, IValueCanGet<TProperty>, IValueContainer, INotifyChanges<TProperty>, INotifyPropertyChanged
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
            Value = initValue;
            _Errors = new ObservableCollection<ErrorEntity>();
            _Errors.GetEventObservable(model)
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
        /// <para>Event that raised when value was changed</para>
        /// <para>值变更时触发的事件</para>
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<TProperty>> ValueChanged;

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
        /// <para>Save the value and try raise the value changed event</para>
        /// <para>保存值并且尝试触发更改事件</para>
        /// </summary>
        /// <param name="value">New value/属性值</param>
        /// <returns>ValueContainer&lt;TProperty&gt;.</returns>
        public ValueContainer<TProperty> SetValueAndTryNotify(TProperty value)
        {
            InternalPropertyChange(this.Model, value, ref _value, PropertyName);
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
            _value = value;
            return this;
        }


        /// <summary>
        /// Internals the property change.
        /// </summary>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="currentValue">The current value.</param>
        /// <param name="message">The message.</param>
        private void InternalPropertyChange(BindableBase objectInstance, TProperty newValue, ref TProperty currentValue, string message)
        {
            var changing = (this.EqualityComparer != null) ?
                !this.EqualityComparer(newValue, currentValue) :
                !Object.Equals(newValue, currentValue);


            if (changing)
            {
                var oldvalue = currentValue;
                currentValue = newValue;

                ValueChangedEventArgs<TProperty> arg = null;

                Func<PropertyChangedEventArgs> lzf =
                    () =>
                    {
                        arg = arg ?? new ValueChangedEventArgs<TProperty>(message, oldvalue, newValue);
                        return arg;
                    };


                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(message));
                objectInstance.RaisePropertyChanged(lzf);
                ValueChanged?.Invoke(this, lzf() as ValueChangedEventArgs<TProperty>);
                ValueChangedWithNameOnly?.Invoke(this, new PropertyChangedEventArgs(message));
                ValueChangedWithNothing?.Invoke(this, EventArgs.Empty);

            }
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



#if NETFX_CORE
        bool _IsCopyToAllowed = !typeof(ICommand).GetTypeInfo().IsAssignableFrom(typeof(TProperty).GetTypeInfo());
#else
        /// <summary>
        /// The _ is copy to allowed
        /// </summary>
        bool _IsCopyToAllowed = !typeof(ICommand).IsAssignableFrom(typeof(TProperty));
#endif
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



        /// <summary>
        /// Occurs when [value changed with name only].
        /// </summary>
        public event PropertyChangedEventHandler ValueChangedWithNameOnly;

        /// <summary>
        /// Occurs when [value changed with nothing].
        /// </summary>
        public event EventHandler ValueChangedWithNothing;
        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// Interface IValueCanSet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValueCanSet<in T>
    {
        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <value>The value.</value>
        T Value { set; }
    }

    /// <summary>
    /// Interface IValueCanGet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValueCanGet<out T>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        T Value { get; }
    }



    /// <summary>
    /// Interface INotifyChanges
    /// </summary>
    public interface INotifyChanges
    {
        /// <summary>
        /// Occurs when [value changed with name only].
        /// </summary>
        event PropertyChangedEventHandler ValueChangedWithNameOnly;
        /// <summary>
        /// Occurs when [value changed with nothing].
        /// </summary>
        event EventHandler ValueChangedWithNothing;

    }
    /// <summary>
    /// Interface INotifyChanges
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INotifyChanges<T> : INotifyChanges
    {
        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        event EventHandler<ValueChangedEventArgs<T>> ValueChanged;

    }
    /// <summary>
    /// Interface IValueContainer
    /// </summary>
    public interface IValueContainer : IErrorInfo, INotifyChanges
    {
        string PropertyName { get; }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>The type of the property.</value>
        Type PropertyType { get; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        Object Value { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is copy to allowed.
        /// </summary>
        /// <value><c>true</c> if this instance is copy to allowed; otherwise, <c>false</c>.</value>
        bool IsCopyToAllowed { get; set; }

        void AddErrorEntry(string message, Exception exception = null);

    }

    /// <summary>
    /// Interface ICommandModel
    /// </summary>
    /// <typeparam name="TCommand">The type of the t command.</typeparam>
    /// <typeparam name="TResource">The type of the t resource.</typeparam>
    public interface ICommandModel<TCommand, TResource> : ICommand
    {
        /// <summary>
        /// Gets the command core.
        /// </summary>
        /// <value>The command core.</value>
        TCommand CommandCore { get; }
        /// <summary>
        /// Gets or sets a value indicating whether [last can execute value].
        /// </summary>
        /// <value><c>true</c> if [last can execute value]; otherwise, <c>false</c>.</value>
        bool LastCanExecuteValue { get; set; }
        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        /// <value>The resource.</value>
        TResource State { get; set; }
    }


    /// <summary>
    /// Interface IErrorInfo
    /// </summary>
    public interface IErrorInfo
    {
        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>The errors.</value>
        ObservableCollection<ErrorEntity> Errors { get; }
    }



}

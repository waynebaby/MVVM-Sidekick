using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Commands;
using System.Runtime.CompilerServices;
using MVVMSidekick.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using MVVMSidekick.EventRouting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Collections;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using System.Collections.Concurrent;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Xaml.Controls.Primitives;

#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Concurrent;
using System.Windows.Navigation;

using MVVMSidekick.Views;
using System.Windows.Controls.Primitives;
using MVVMSidekick.Utilities;

#elif SILVERLIGHT_5||SILVERLIGHT_4
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#elif WINDOWS_PHONE_8||WINDOWS_PHONE_7
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
#endif





namespace MVVMSidekick
{
    namespace ViewModels
    {
        using MVVMSidekick.Views;
        using MVVMSidekick.Utilities;
        /// <summary>
        /// <para>A ViewModel by default, with basic implement of name-value container.</para>
        /// <para>缺省的 ViewModel。可以用作最简单的字典绑定</para>
        /// </summary>
        public class DefaultViewModel : ViewModelBase<DefaultViewModel>
        {

        }

        /// <summary>
        /// <para>Base type of bindable model.</para>
        /// <para>ViewModel 基类</para>
        /// </summary>
        [DataContract]
        public abstract class BindableBase
            : IDisposable, INotifyPropertyChanged, IBindable
        {

            protected event EventHandler<DataErrorsChangedEventArgs> _ErrorsChanged;
            protected internal void RaiseErrorsChanged(string propertName)
            {
                if (_ErrorsChanged != null)
                {
                    _ErrorsChanged(this, new DataErrorsChangedEventArgs(propertName));
                }
            }



            private bool _IsValidationActivated = false;
            /// <summary>
            /// <para>Gets ot sets if the validation is activatied. This is a flag only， internal logic is not depend on this.</para>
            /// <para>读取/设置 此模型是否激活验证。这只是一个标记，内部逻辑并没有参考这个值</para>
            /// </summary>
            public bool IsValidationActivated
            {
                get { return _IsValidationActivated; }
                set { _IsValidationActivated = value; }
            }

            private bool _IsNotificationActivated = true;
            /// <summary>
            /// <para>Gets ot sets if the property change notification is activatied. </para>
            /// <para>读取/设置 此模型是否激活变化通知</para>
            /// </summary>
            public bool IsNotificationActivated
            {
                get { return (!IsInDesignMode) ? _IsNotificationActivated : false; }
                set { _IsNotificationActivated = value; }
            }





            static bool? _IsInDesignMode;


            /// <summary>
            /// <para>Gets if the code is running in design time. </para>
            /// <para>读取目前是否在设计时状态。</para>
            /// </summary>
            public static bool IsInDesignMode
            {
                get
                {

                    return (
                        _IsInDesignMode
                        ??
                        (

                            _IsInDesignMode =
#if SILVERLIGHT_5||WINDOWS_PHONE_8||WINDOWS_PHONE_7
 DesignerProperties.IsInDesignTool
#elif NETFX_CORE
 Windows.ApplicationModel.DesignMode.DesignModeEnabled
#else
 (bool)System.ComponentModel.DependencyPropertyDescriptor
                                .FromProperty(
                                    DesignerProperties.IsInDesignModeProperty,
                                    typeof(System.Windows.FrameworkElement))
                                .Metadata
                                .DefaultValue
#endif
))
                        .Value;
                }

            }



            /// <summary>
            ///  <para>0 for not disposed, 1 for disposed</para>
            ///  <para>0 表示没有被Dispose 1 反之</para>
            /// </summary>
            private int disposedFlag = 0;

            #region  Index and property names/索引与字段名
            /// <summary>
            /// <para>Get all property names that were defined in subtype, or added objectly in runtime</para>
            /// <para>取得本VM实例已经定义的所有字段名。其中包括静态声明的和动态添加的。</para>
            /// </summary>
            /// <returns>String[]  Property names/字段名数组 </returns>
            public abstract string[] GetFieldNames();

            ///// <summary>
            ///// <para>Gets or sets  poperty values by property name index.</para>
            ///// <para>使用索引方式取得/设置字段值</para>
            ///// </summary>
            ///// <param name="name">Property name/字段名</param>
            ///// <returns>Property value/字段值</returns>
            public abstract object this[string name] { get; set; }


            #endregion

            #region Disposing Logic/Disposing相关逻辑
            /// <summary>
            ///  <para>Dispose action infomation struct</para>
            ///  <para>注册销毁方法时的相关信息</para>
            /// </summary>
            public struct DisposeInfo
            {
                /// <summary>
                ///  <para>Comment of this dispose.</para>
                ///  <para>对此次Dispose的附加说明</para>
                /// </summary>
                public string Comment { get; set; }
                /// <summary>
                ///  <para>Caller Member Name of this dispose registeration.</para>
                ///  <para>此次Dispose注册的来源</para>
                /// </summary>
                public string Caller { get; set; }
                /// <summary>
                ///  <para>Code file path of this dispose registeration.</para>
                ///  <para>注册此次Dispose注册的代码文件</para>
                /// </summary>
                public string File { get; set; }
                /// <summary>
                ///  <para>Code line number of this dispose registeration.</para>
                ///  <para>注册此次Dispose注册的代码行</para>
                /// </summary>
                public int Line { get; set; }


                /// <summary>
                ///  <para>Exception thrown in this dispose action execution .</para>
                ///  <para>执行此次Dispose动作产生的Exception</para>
                /// </summary>
                public Exception Exception { get; set; }
                /// <summary>
                ///  <para>Dispose action.</para>
                ///  <para>Dispose动作</para>
                /// </summary>

                public Action Action { get; set; }
            }

            /// <summary>
            /// <para>Logic actions need to be executed when the instance is disposing</para>
            /// <para>销毁对象时 需要执行的操作</para>
            /// </summary>
            private List<DisposeInfo> _disposeInfos;
            private static Func<BindableBase, List<DisposeInfo>> _locateDisposeInfos =
                m =>
                {
                    if (m._disposeInfos == null)
                    {
                        Interlocked.CompareExchange(ref m._disposeInfos, new List<DisposeInfo>(), null);

                    }
                    return m._disposeInfos;

                };

            /// <summary>
            /// <para>Register logic actions need to be executed when the instance is disposing</para>
            /// <para>注册一个销毁对象时需要执行的操作</para>
            /// </summary>
            /// <param name="newAction">Disposing action/销毁操作</param>
            public void AddDisposeAction(Action newAction, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber]int line = -1)
            {

                var di = new DisposeInfo
                {
                    Caller = caller,
                    Comment = comment,
                    File = file,
                    Line = line,
                    Action = newAction

                };
                _locateDisposeInfos(this).Add(di);

            }


            /// <summary>
            /// <para>Register an object that need to be disposed when the instance is disposing</para>
            /// <para>销毁对象时 需要一起销毁的对象</para>
            /// </summary>
            /// <param name="item">disposable object/需要一起销毁的对象</param>
            public void AddDisposable(IDisposable item, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1)
            {
                AddDisposeAction(() => item.Dispose(), comment, caller, file, line);
            }


            ~BindableBase()
            {
                Dispose();
            }
            /// <summary>
            /// <para>Do all the dispose </para>
            /// <para>销毁，尝试运行所有注册的销毁操作</para>
            /// </summary>
            public void Dispose()
            {
                if (Interlocked.Exchange(ref disposedFlag, 1) == 0)
                {
                    if (_disposeInfos != null)
                    {
                        var l = _disposeInfos.ToList()
                            .Select
                            (
                                info =>
                                {
                                    //Exception gotex = null;
                                    try
                                    {
                                        info.Action();
                                    }
                                    catch (Exception ex)
                                    {
                                        info.Exception = ex;

                                    }

                                    return info;
                                }

                            )
                            .Where(x => x.Exception != null)
                            .ToArray();
                        if (l.Length > 0)
                        {
                            OnDisposeExceptions(l);
                        }
                    }

                    _disposeInfos = null;
                    GC.SuppressFinalize(this);
                }


            }

            /// <summary>
            /// <para>If dispose actions got exceptions, will handled here. </para>
            /// <para>处理Dispose 时产生的Exception</para>
            /// </summary>
            /// <param name="exceptions">
            /// <para>The exception and dispose infomation</para>
            /// <para>需要处理的异常信息</para>
            /// </param>
            protected virtual void OnDisposeExceptions(IList<DisposeInfo> exceptions)
            {

            }

            #endregion

            #region Propery Changed Logic/ Propery Changed事件相关逻辑


            internal void RaisePropertyChanged(Func<PropertyChangedEventArgs> lazyEAFactory, string propertyName)
            {


                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, lazyEAFactory());
                }


            }

            /// <summary>
            ///<para>Event that raised when properties were changed and Notification was activited</para>
            ///<para> VM属性任何绑定用值被修改后,在启用通知情况下触发此事件</para>
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;


            #endregion

            #region 验证与错误相关逻辑






            protected bool CheckError(Func<Boolean> test, string errorMessage)
            {

                var rval = test();
                if (rval)
                {
                    SetErrorAndTryNotify(errorMessage);
                }
                return rval;

            }


            ///// <summary>
            ///// 验证错误内容
            ///// </summary>
            //string IDataErrorInfo.Error
            //{
            //    get
            //    {
            //        return GetError();
            //    }


            //}
            /// <summary>
            /// <para>Gets the validate error of this model </para>
            /// <para>取得错误内容</para>
            /// </summary>
            /// <returns>Error string/错误内容字符串</returns>
            public abstract string Error { get; }
            /// <summary>
            /// <para>Sets the validate error of this model </para>
            /// <para>设置错误内容</para>
            /// </summary>
            /// <returns>Error string/错误内容字符串</returns>
            protected abstract void SetError(string value);

            /// <summary>
            /// <para>Sets the validate error of this model and notify </para>
            /// <para>设置错误内容并且尝试用事件通知</para>
            /// </summary>
            /// <returns>Error string/错误内容字符串</returns>
            protected abstract void SetErrorAndTryNotify(string value);



            /// <summary>
            /// <para>Gets validate error string of this field</para>
            /// <para>取得对于每个字段，验证失败所产生的错误信息</para>
            /// </summary>
            /// <param name="propertyName">Property Name of error /要检查错误的属性名</param>
            /// <returns>Rrror string /错误字符串</returns>
            protected abstract string GetColumnError(string propertyName);



            #endregion


            //   public abstract bool IsUIBusy { get; set; }








        }

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
            /// <param name="vm">Model instance /Model 实例</param>
            /// <returns></returns>
            public static T DisposeWith<T>(this T item, IBindable vm, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = -1) where T : IDisposable
            {
                vm.AddDisposable(item, comment, caller, file, line);
                return item;
            }

            public static ValueContainer<T> Initialize<T>(this BindableBase model, string propertyName, ref Property<T> reference, ref Func<BindableBase, ValueContainer<T>> locator, Func<T> defaultValueFactory = null)
            {
                if (reference == null)
                    reference = new Property<T> { LocatorFunc = locator };
                if (reference.Container == null)
                {
                    reference.Container = new ValueContainer<T>(propertyName, model);
                    if (defaultValueFactory != null)
                    {
                        reference.Container.Value = defaultValueFactory();
                    }
                }
                return reference.Container;
            }

            public static ValueContainer<T> Initialize<T>(this BindableBase model, string propertyName, ref Property<T> reference, ref Func<BindableBase, ValueContainer<T>> locator, Func<BindableBase, T> defaultValueFactory = null)
            {
                return Initialize(model, propertyName, ref reference, ref locator, () => (defaultValueFactory != null) ? defaultValueFactory(model) : default(T));
            }
        }


        /// <summary>
        /// <para>A slot to place the value container field and value container locator.</para>
        /// <para>属性定义。一个属性定义包括一个创建/定位属性“值容器”的静态方法引用，和一个缓存该方法执行结果“值容器”的槽位</para>
        /// </summary>
        /// <typeparam name="TProperty">Type of the property value /属性的类型</typeparam>
        public class Property<TProperty>
        {
            public Property()
            {

            }

            /// <summary>
            /// <para>Locate or create the value container of this model intances</para>
            /// <para>通过定位方法定位本Model实例中的值容器</para>

            /// </summary>
            /// <param name="model">Model intances/model 实例</param>
            /// <returns>Value Container of this property/值容器</returns>
            public ValueContainer<TProperty> LocateValueContainer(BindableBase model)
            {

                return LocatorFunc(model);
            }


            /// <summary>
            /// <para>Gets sets the factory to locate/create value container of this model instance</para>
            /// <para>读取/设置定位值容器用的方法。</para>
            /// </summary>
            public Func<BindableBase, ValueContainer<TProperty>> LocatorFunc
            {
                internal get;
                set;
            }

            /// <summary>
            /// <para>Gets or sets Value Container, it can be recently create and cached here，by LocatorFunc </para>
            /// <para>读取/设置值容器,这事值容器LocatorFunc创建值容器并且缓存的位置 </para>
            /// </summary>
            public ValueContainer<TProperty> Container
            {
                get;
                set;
            }


        }

        /// <summary>
        /// <para>Value Container, holds the value of certain field, with notifition /and compare support</para>
        /// <para>值容器</para>
        /// </summary>
        /// <typeparam name="TProperty">Type of the property value /属性的类型</typeparam>
        public class ValueContainer<TProperty> : IErrorInfo, IValueCanSet<TProperty>, IValueCanGet<TProperty>, IValueContainer
        {


            #region Constructors /构造器
            /// <summary>
            /// <para>Create a new Value Container</para>
            /// <para>创建属性值容器</para>
            /// </summary>
            /// <param name="model">
            /// <para>The model that Value Container will be held with.</para>
            /// <para>所属的model实例</para>
            /// </param>
            /// <param name="info">Property name/属性名</param>
            /// <param name="initValue">The first value of this container/初始值</param>
            public ValueContainer(string info, BindableBase model, TProperty initValue = default (TProperty ))
                : this(info, model, (v1, v2) => v1.Equals(v2), initValue)
            {
            }





            /// <summary>
            /// <para>Create a new Value Container</para>
            /// <para>创建属性值容器</para>
            /// </summary>
            /// <param name="model">
            /// <para>The model that Value Container will be held with.</para>
            /// <para>所属的model实例</para>
            /// </param>
            /// <param name="info">Property name/属性名</param>
            /// <param name="equalityComparer">
            /// <para>Comparer of new/old value, for notifition.</para>
            /// <para>判断两个值是否相等的比较器,用于判断是否通知变更</para>
            /// </param>
            /// <param name="initValue">The first value of this container/初始值</param>
            public ValueContainer(string info, BindableBase model, Func<TProperty, TProperty, bool> equalityComparer, TProperty initValue = default (TProperty))
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
            public Func<TProperty, TProperty, bool> EqualityComparer { get; private set; }

            /// <summary>
            /// Property name /属性名
            /// </summary>
            public string PropertyName { get; private set; }

            TProperty _value;

            /// <summary>
            /// Value/值 
            /// </summary>
            public TProperty Value
            {
                get { return _value; }
                set { SetValueAndTryNotify(value); }
            }

            /// <summary>
            /// <para>Save the value and try raise the value changed event</para>
            /// <para>保存值并且尝试触发更改事件</para>
            /// </summary>
            /// <param name="value">New value/属性值</param>
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
            public ValueContainer<TProperty> SetValue(TProperty value)
            {
                _value = value;
                return this;
            }


            private void InternalPropertyChange(BindableBase objectInstance, TProperty newValue, ref TProperty currentValue, string message)
            {
                var changing = (this.EqualityComparer == null) ?
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


                    objectInstance.RaisePropertyChanged(lzf, message);
                    if (ValueChanged != null) ValueChanged(this, lzf() as ValueChangedEventArgs<TProperty>);

                }
            }


            /// <summary>
            /// <para>The model instance that Value Container was held.</para>
            /// <para>此值容器所在的Model</para>
            /// </summary>
            public BindableBase Model { get; internal set; }





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
            public Type PropertyType
            {
                get;
                private set;
            }


            ObservableCollection<ErrorEntity> _Errors;

            public ObservableCollection<ErrorEntity> Errors
            {
                get { return _Errors; }

            }



#if NETFX_CORE
            bool _IsCopyToAllowed = !typeof(ICommand).GetTypeInfo().IsAssignableFrom(typeof(TProperty).GetTypeInfo());
#else
            bool _IsCopyToAllowed = !typeof(ICommand).IsAssignableFrom(typeof(TProperty));
#endif
            /// <summary>
            /// <para>Can be copied by CopyTo method</para>
            /// <para>是否可以被 `Copyto` 复制到另外一个属性</para>
            /// </summary>
            public bool IsCopyToAllowed
            {
                get { return _IsCopyToAllowed; }
                set { _IsCopyToAllowed = value; }
            }
        }


        /// <summary>
        /// <para>Event args that fired when property changed, with old value and new value field.</para>
        /// <para>值变化事件参数</para>
        /// </summary>
        /// <typeparam name="TProperty">Type of propery/变化属性的类型</typeparam>
        public class ValueChangedEventArgs<TProperty> : PropertyChangedEventArgs
        {
            /// <summary>
            /// Constructor of ValueChangedEventArgs
            /// </summary>
            public ValueChangedEventArgs(string propertyName, TProperty oldValue, TProperty newValue)
                : base(propertyName)
            {
                NewValue = newValue;
                OldValue = oldValue;
            }

            /// <summary>
            /// New Value
            /// </summary>
            public TProperty NewValue { get; private set; }
            /// <summary>
            /// Old Value
            /// </summary>
            public TProperty OldValue { get; private set; }
        }


        /// <summary>
        /// <para>A Bindebale Tuple</para>
        /// <para>一个可绑定的Tuple实现</para>
        /// </summary>
        /// <typeparam name="TItem1">Type of first item/第一个元素的类型</typeparam>
        /// <typeparam name="TItem2">Type of second item/第二个元素的类型</typeparam>
        [DataContract]
        public class BindableTuple<TItem1, TItem2> : BindableBase<BindableTuple<TItem1, TItem2>>
        {
            public BindableTuple(TItem1 item1, TItem2 item2)
            {
                this.IsNotificationActivated = false;
                Item1 = item1;
                Item2 = item2;
                this.IsNotificationActivated = true;
            }
            /// <summary>
            /// 第一个元素
            /// </summary>

            public TItem1 Item1
            {
                get { return _Item1Locator(this).Value; }
                set { _Item1Locator(this).SetValueAndTryNotify(value); }
            }
            #region Property TItem1 Item1 Setup
            protected Property<TItem1> _Item1 = new Property<TItem1> { LocatorFunc = _Item1Locator };
            static Func<BindableBase, ValueContainer<TItem1>> _Item1Locator = RegisterContainerLocator<TItem1>("Item1", model => model.Initialize("Item1", ref model._Item1, ref _Item1Locator, _Item1DefaultValueFactory));
            static Func<BindableBase, TItem1> _Item1DefaultValueFactory = null;
            #endregion

            /// <summary>
            /// 第二个元素
            /// </summary>

            public TItem2 Item2
            {
                get { return _Item2Locator(this).Value; }
                set { _Item2Locator(this).SetValueAndTryNotify(value); }
            }
            #region Property TItem2 Item2 Setup
            protected Property<TItem2> _Item2 = new Property<TItem2> { LocatorFunc = _Item2Locator };
            static Func<BindableBase, ValueContainer<TItem2>> _Item2Locator = RegisterContainerLocator<TItem2>("Item2", model => model.Initialize("Item2", ref model._Item2, ref _Item2Locator, _Item2DefaultValueFactory));
            static Func<BindableBase, TItem2> _Item2DefaultValueFactory = null;
            #endregion


        }
        /// <summary>
        /// <para>Fast create Bindable Tuple </para>
        /// <para>帮助快速创建BindableTuple的帮助类</para>
        /// </summary>
        public static class BindableTuple
        {
            /// <summary>
            /// Create a Tuple
            /// </summary>

            public static BindableTuple<TItem1, TItem2> Create<TItem1, TItem2>(TItem1 item1, TItem2 item2)
            {
                return new BindableTuple<TItem1, TItem2>(item1, item2);
            }

        }


        /// <summary>
        /// <para>Model type with detail subtype type paremeter.</para>
        /// <para>具有子类详细类型定义的model </para>
        /// <example>
        /// public class Class1:BindableBase&lt;Class1&gt;  {}
        /// </example>
        /// </summary>
        /// <typeparam name="TSubClassType"> Sub Type / 子类类型</typeparam>
        [DataContract]
        public abstract class BindableBase<TSubClassType> : BindableBase, INotifyDataErrorInfo where TSubClassType : BindableBase<TSubClassType>
        {

            /// <summary>
            /// 清除值
            /// </summary>
            public void ResetPropertyValue<T>(Property<T> property)
            {
                if (property != null)
                {
                    var oldContainer = property.Container;
                    if (oldContainer != null)
                    {


                        property.Container = null;
                        property.LocatorFunc(oldContainer.Model);
                        oldContainer.SetValueAndTryNotify(property.Container.Value);
                        property.Container = oldContainer;
                    }
                }


            }




            /// <summary>
            /// <para>Cast a model instance to current model subtype</para>
            /// <para>将一个 model 引用特化为本子类型的引用</para>
            /// </summary>
            /// <param name="model"> some bindable model/某种可绑定model</param>
            /// <returns>Current sub type instance/本类型引用</returns>
            public static TSubClassType CastToCurrentType(BindableBase model)
            {
                return (TSubClassType)model;

            }
            /// <summary>
            /// <para>Type cache of container getter</para>
            /// <para>每个属性类型独占的一个专门的类型缓存。</para>
            /// </summary>
            /// <typeparam name="TProperty"></typeparam>
            protected static class TypeDic<TProperty>
            {
                public static Dictionary<string, Func<TSubClassType, ValueContainer<TProperty>>> _propertyContainerGetters = new Dictionary<string, Func<TSubClassType, ValueContainer<TProperty>>>();

            }

            /// <summary>
            /// 根据索引获取属性值
            /// </summary>
            /// <param name="colName">属性名</param>
            /// <returns>属性值</returns>
            public override object this[string colName]
            {
                get
                {
                    var lc = GetOrCreatePlainLocator(colName, this);
                    return lc((TSubClassType)this).Value;
                }
                set
                {

                    var lc = GetOrCreatePlainLocator(colName, this);
                    lc((TSubClassType)this).Value = value;
                }
            }

            private static Func<TSubClassType, IValueContainer> GetOrCreatePlainLocator(string colName, BindableBase viewModel)
            {
                Func<TSubClassType, IValueContainer> pf;
                if (!_plainPropertyContainerGetters.TryGetValue(colName, out pf))
                {
                    var p = new ValueContainer<object>(colName, viewModel);
                    pf = _ => p;
                    _plainPropertyContainerGetters[colName] = pf;
                }
                return pf;
            }




#if SILVERLIGHT_5||WINDOWS_PHONE_8||WINDOWS_PHONE_7
            protected static Dictionary<string, Func<TSubClassType, IValueContainer>>
             _plainPropertyContainerGetters =
             new Dictionary<string, Func<TSubClassType, IValueContainer>>(StringComparer.CurrentCultureIgnoreCase);
#else

            protected static Dictionary<string, Func<TSubClassType, IValueContainer>>
                _plainPropertyContainerGetters =
                new Dictionary<string, Func<TSubClassType, IValueContainer>>(StringComparer.CurrentCultureIgnoreCase);
#endif



            public override string Error
            {
                get { return _ErrorLocator(this).Value; }
            }

            protected override void SetError(string value)
            {
                _ErrorLocator(this).SetValue(value);
            }

            protected override void SetErrorAndTryNotify(string value)
            {
                _ErrorLocator(this).SetValueAndTryNotify(value);
            }


            #region Property string Error Setup

            protected Property<string> _Error =
              new Property<string> { LocatorFunc = _ErrorLocator };
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            static Func<BindableBase, ValueContainer<string>> _ErrorLocator =
                RegisterContainerLocator<string>(
                "Error",
                model =>
                {
                    model._Error =
                        model._Error
                        ??
                        new Property<string> { LocatorFunc = _ErrorLocator };
                    return model._Error.Container =
                        model._Error.Container
                        ??
                        new ValueContainer<string>("Error", model);
                });

            #endregion






            /// <summary>
            /// 注册一个属性容器的定位器。
            /// </summary>
            /// <typeparam name="TProperty">属性类型</typeparam>
            /// <param name="propertyName">属性名</param>
            /// <param name="getOrCreateLocatorMethod">属性定位/创建方法 也就是定位器</param>
            /// <returns>注册后的定位器</returns>
            protected static Func<BindableBase, ValueContainer<TProperty>> RegisterContainerLocator<TProperty>(string propertyName, Func<TSubClassType, ValueContainer<TProperty>> getOrCreateLocatorMethod)
            {


                TypeDic<TProperty>._propertyContainerGetters[propertyName] = getOrCreateLocatorMethod;
                _plainPropertyContainerGetters[propertyName] = (v) => getOrCreateLocatorMethod(v) as IValueContainer;
                return o => getOrCreateLocatorMethod((TSubClassType)o);
            }


            /// <summary>
            /// 根据属性名取得一个值容器
            /// </summary>
            /// <typeparam name="TProperty">属性类型</typeparam>
            /// <param name="propertyName">属性名</param>
            /// <returns>值容器</returns>
            public ValueContainer<TProperty> GetValueContainer<TProperty>(string propertyName)
            {
                Func<TSubClassType, ValueContainer<TProperty>> contianerGetterCreater;
                if (!TypeDic<TProperty>._propertyContainerGetters.TryGetValue(propertyName, out contianerGetterCreater))
                {
                    throw new Exception("Property Not Exists!");

                }

                return contianerGetterCreater((TSubClassType)(Object)this);

            }

            /// <summary>
            /// 根据表达式树取得一个值容器
            /// </summary>
            /// <typeparam name="TProperty">属性类型</typeparam>
            /// <param name="expression">表达式树</param>
            /// <returns>值容器</returns>
            public ValueContainer<TProperty> GetValueContainer<TProperty>(Expression<Func<TSubClassType, TProperty>> expression)
            {
                var propName = MVVMSidekick.Utilities.ExpressionHelper.GetPropertyName<TSubClassType, TProperty>(expression);
                return GetValueContainer<TProperty>(propName);

            }




            /// <summary>
            /// 根据属性名取得一个值容器
            /// </summary>
            /// <param name="propertyName">属性名</param>
            /// <returns>值容器</returns>
            public IValueContainer GetValueContainer(string propertyName)
            {
                Func<TSubClassType, IValueContainer> contianerGetterCreater;
                if (!_plainPropertyContainerGetters.TryGetValue(propertyName, out contianerGetterCreater))
                {
                    return null;

                }

                return contianerGetterCreater((TSubClassType)(Object)this);

            }




            /// <summary>
            /// 获取某一属性的验证错误信息
            /// </summary>
            /// <param name="propertyName">属性名</param>
            /// <returns>错误信息字符串</returns>
            protected override string GetColumnError(string propertyName)
            {
                if (_plainPropertyContainerGetters[propertyName]((TSubClassType)this).Errors.Count > 0)
                {


                    var error = string.Join(",", _plainPropertyContainerGetters[propertyName]((TSubClassType)this).Errors.Select(x => x.Message));
                    var propertyContainer = this.GetValueContainer(propertyName);
#if NETFX_CORE
                    if (propertyContainer != null && typeof(INotifyDataErrorInfo).GetTypeInfo().IsAssignableFrom(propertyContainer.PropertyType.GetTypeInfo()))
#else

                    if (propertyContainer != null && typeof(INotifyDataErrorInfo).IsAssignableFrom(propertyContainer.PropertyType))
#endif
                    {
                        INotifyDataErrorInfo di = this[propertyName] as INotifyDataErrorInfo;
                        if (di != null)
                        {
                            error = error + "\r\n-----Inner " + propertyName + " as INotifyDataErrorInfo -------\r\n\t" + di.HasErrors.ToString();
                        }
                    }

                    return error;
                }
                return null;
            }



            /// <summary>
            /// 获取所有属性名，包括静态声明和动态添加的
            /// </summary>
            /// <returns></returns>
            public override string[] GetFieldNames()
            {
                return _plainPropertyContainerGetters.Keys.ToArray();
            }


            /// <summary>
            /// 创建一个VM副本
            /// </summary>
            /// <returns>新引用</returns>
            public TSubClassType Clone()
            {
                var x = (TSubClassType)Activator.CreateInstance(typeof(TSubClassType));
                CopyTo(x);
                return x;
            }

            static void Copyref<T>(T source, ref T target)
            {


                if (source == null)
                {
                    target = source;
                    return;
                }

                var sourcetype = source.GetType().GetTypeOrTypeInfo();
                if (sourcetype.IsValueType || source is string)
                {
                    target = source;
                }
#if ! (SILVERLIGHT_5 || WINDOWS_PHONE_8|| WINDOWS_PHONE_7 || NETFX_CORE)

                else if (typeof(ICloneable).IsAssignableFrom(sourcetype))
                {
                    target = (T)((ICloneable)source).Clone();
                }
#endif
                else if (typeof(System.Collections.IList).GetTypeOrTypeInfo().IsAssignableFrom(sourcetype))
                {
                    var tarcol = target as System.Collections.IList;
                    var scol = source as System.Collections.IList;
                    if (tarcol == null)
                    {

                        var newcol = sourcetype.IsArray ?
                            Array.CreateInstance(sourcetype.GetElementType(), scol.Count) :
                            System.Activator.CreateInstance(source.GetType(), new object[0]) as System.Collections.IList;


                        tarcol = (System.Collections.IList)newcol;
                    }
                    else
                    {
                        tarcol.Clear();
                    }
                    if (tarcol != null)
                    {


                        foreach (var item in scol)
                        {
                            object newv = null;
                            Copyref(item, ref newv);
                            tarcol.Add(newv);
                        }
                        target = (T)tarcol;
                    }
                    else
                    {
                        target = default(T);
                    }
                }
            }

            public void CopyTo(TSubClassType target)
            {
                foreach (var item in GetFieldNames())
                {
                    var ctThis = GetValueContainer(item);
                    var ctTarget = target.GetValueContainer(item);
                    if (ctThis.IsCopyToAllowed)
                    {
                        object temp = null;
                        Copyref(this[item], ref temp);
                        target[item] = temp;
                    }


                }
            }


            event EventHandler<DataErrorsChangedEventArgs> INotifyDataErrorInfo.ErrorsChanged
            {
                add { _ErrorsChanged += value; }
                remove { _ErrorsChanged -= value; }
            }



            System.Collections.IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
            {
                if (this.GetFieldNames().Contains(propertyName))
                {
                    return this.GetValueContainer(propertyName).Errors;
                }
                else
                {
                    return null;
                }

            }


            bool INotifyDataErrorInfo.HasErrors
            {
                get
                {
                    //  return false;
                    RefreshErrors();
                    return !string.IsNullOrEmpty(this.Error);

                }
            }

            private void RefreshErrors()
            {
                var sb = new StringBuilder();
                var rt = GetAllErrors().Select(x =>
                {
                    return sb.Append(x.Message).Append(":").AppendLine(x.Exception.ToString());
                }
                    )
                    .ToArray();
                this.SetErrorAndTryNotify(sb.ToString());


            }

            public ErrorEntity[] GetAllErrors()
            {
                var errors = GetFieldNames()
                     .SelectMany(name => this.GetValueContainer(name).Errors)
                     .Where(x => x != null)
                     .Where(x => !(string.IsNullOrEmpty(x.Message) || x.Exception == null))
                     .ToArray();
                return errors;
            }

            //public override IDictionary<string,object >  Values
            //{
            //    get { return new BindableAccesser<TSubClassType>(this); }
            //}


        }

        public interface IBindable : INotifyPropertyChanged
        {
            void AddDisposable(IDisposable item, string comment = "", string member = "", string file = "", int line = -1);
            void AddDisposeAction(Action action, string comment = "", string member = "", string file = "", int line = -1);
            string Error { get; }
            void Dispose();
            //IDictionary<string,object >  Values { get; }
            string[] GetFieldNames();
            object this[string name] { get; set; }
        }


        //#if !NETFX_CORE

        //        public class StringToViewModelInstanceConverter : TypeConverter
        //        {
        //            public override bool CanConvertTo(ITypeDescriptorContext context, Type sourceType)
        //            {

        //                //if (sourceType == typeof(string))
        //                    return true;
        //                //return base.CanConvertFrom(context, sourceType);
        //            }
        //            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        //            {
        //                return true;
        //            }

        //            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        //            {

        //                var str = value.ToString();
        //                var t = Type.GetType(str);
        //                var v = Activator.CreateInstance(t);
        //                return v;
        //                ////  return base.ConvertFrom(context, culture, value);
        //            }
        //            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        //            {
        //                return value.ToString();
        //            }
        //        }

        //        [TypeConverter(typeof(StringToViewModelInstanceConverter))]
        //#endif
        public partial interface IViewModel : IBindable, INotifyPropertyChanged
        {
            Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue);
            Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue);
            Task OnBindedViewLoad(MVVMSidekick.Views.IView view);
            Task WaitForClose(Action closingCallback = null);
            bool IsUIBusy { get; set; }
            bool HaveReturnValue { get; }
            void Close();
            MVVMSidekick.Views.StageManager StageManager { get; set; }
#if NETFX_CORE
            void LoadState(Object navigationParameter, Dictionary<String, Object> pageState);
            void SaveState(Dictionary<String, Object> pageState);
#endif
        }

        public partial interface IViewModel<TResult> : IViewModel
        {
            Task<TResult> WaitForCloseWithResult(Action closingCallback = null);
            TResult Result { get; set; }
        }


        [DataContract]
        public struct NoResult
        {

        }

        public struct ShowAwaitableResult<TViewModel>
        {
            public TViewModel ViewModel { get; set; }
            public Task Closing { get; set; }

        }
        public partial class ViewModelBase<TViewModel, TResult> : ViewModelBase<TViewModel>, IViewModel<TResult>
            where TViewModel : ViewModelBase<TViewModel, TResult>, IViewModel<TResult>
        {

            public override bool HaveReturnValue { get { return true; } }

            public Task<TResult> WaitForCloseWithResult(Action closingCallback = null)
            {
                var t = new Task<TResult>(() => Result);

                this.AddDisposeAction(
                    () =>
                    {
                        if (closingCallback != null)
                        {
                            closingCallback();
                        }
                        t.Start();
                    }
                    );


                return t;
            }

            public TResult Result
            {
                get { return _ResultLocator(this).Value; }
                set { _ResultLocator(this).SetValueAndTryNotify(value); }
            }

            #region Property TResult Result Setup
            protected Property<TResult> _Result =
              new Property<TResult> { LocatorFunc = _ResultLocator };
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            static Func<BindableBase, ValueContainer<TResult>> _ResultLocator =
                RegisterContainerLocator<TResult>(
                    "Result",
                    model =>
                    {
                        model._Result =
                            model._Result
                            ??
                            new Property<TResult> { LocatorFunc = _ResultLocator };
                        return model._Result.Container =
                            model._Result.Container
                            ??
                            new ValueContainer<TResult>("Result", model);
                    });
            #endregion




        }


        /// <summary>
        /// 一个VM,带有若干界面特性
        /// </summary>
        /// <typeparam name="TViewModel">本身的类型</typeparam>

        public abstract partial class ViewModelBase<TViewModel> : BindableBase<TViewModel>, IViewModel where TViewModel : ViewModelBase<TViewModel>
        {

            protected virtual async Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
            {
                //#if SILVERLIGHT_5
                //                await T.askEx.Yield();
                //#else
                //                await T.ask.Yield();
                //#endif

                StageManager = new StageManager(this) { CurrentBindingView = view };
                StageManager.InitParent(() => view.Parent);
                StageManager.DisposeWith(this);
                await TaskExHelper.Yield();
            }
            
            /// <summary>
            ///  Dispose By Default, override id you don't want.
            /// </summary>
            /// <param name="view"></param>
            /// <param name="newValue"></param>
            /// <returns></returns>
            protected virtual async Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue)
            {
                Dispose();
                await TaskExHelper.Yield();
            }

            protected virtual async Task OnBindedViewLoad(IView view)
            {
                StageManager = new StageManager(this) { CurrentBindingView = view };
                StageManager.InitParent(() => view.Parent);
                StageManager.DisposeWith(this);
                await TaskExHelper.Yield();
            }

            async Task IViewModel.OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
            {
                if (IsInDesignMode)
                {
                    await TaskExHelper.Yield();
                }
                else
                    await OnBindedToView(view, oldValue);
            }
            async Task IViewModel.OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue)
            {
                if (IsInDesignMode)
                {
                    await TaskExHelper.Yield();
                }
                else
                    await OnUnbindedFromView(view, newValue);
            }

            async Task IViewModel.OnBindedViewLoad(MVVMSidekick.Views.IView view)
            {

                if (IsInDesignMode)
                {
                    await TaskExHelper.Yield();
                }
                else
                {

                    await OnBindedViewLoad(view);
                }
            }

#if NETFX_CORE
            /// <summary>
            /// Populates the page with content passed during navigation.  Any saved state is also
            /// provided when recreating a page from a prior session.
            /// </summary>
            /// <param name="navigationParameter">The parameter value passed to
            /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
            /// </param>
            /// <param name="pageState">A dictionary of state preserved by this page during an earlier
            /// session.  This will be null the first time a page is visited.</param>
            public virtual void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
            {

            }

            /// <summary>
            /// Preserves state associated with this page in case the application is suspended or the
            /// page is discarded from the navigation cache.  Values must conform to the serialization
            /// requirements of <see cref="SuspensionManager.SessionState"/>.
            /// </summary>
            /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
            public virtual void SaveState(Dictionary<String, Object> pageState)
            {

            }
#endif

            MVVMSidekick.Views.StageManager _StageManager;

            public MVVMSidekick.Views.StageManager StageManager
            {
                get { return _StageManager; }
                set { _StageManager = value; }
            }

            /// <summary>
            /// 是否有返回值
            /// </summary>
            public virtual bool HaveReturnValue { get { return false; } }
            /// <summary>
            /// 本UI是否处于忙状态
            /// </summary>
            public bool IsUIBusy
            {
                get { return _IsUIBusyLocator(this).Value; }
                set { _IsUIBusyLocator(this).SetValueAndTryNotify(value); }
            }

            #region Property bool IsUIBusy Setup
            protected Property<bool> _IsUIBusy =
              new Property<bool> { LocatorFunc = _IsUIBusyLocator };
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            static Func<BindableBase, ValueContainer<bool>> _IsUIBusyLocator =
                RegisterContainerLocator<bool>(
                    "IsUIBusy",
                    model =>
                    {
                        model._IsUIBusy =
                            model._IsUIBusy
                            ??
                            new Property<bool> { LocatorFunc = _IsUIBusyLocator };
                        return model._IsUIBusy.Container =
                            model._IsUIBusy.Container
                            ??
                            new ValueContainer<bool>("IsUIBusy", model);
                    });
            #endregion

            public Task WaitForClose(Action closingCallback = null)
            {
                var t = new Task(() => { });

                this.AddDisposeAction(
                    () =>
                    {
                        if (closingCallback != null)
                        {
                            closingCallback();
                        }
                        t.Start();
                    }
                    );


                return t;
            }
            public void Close()
            {
                if (StageManager != null)
                {

                    this.StageManager.CurrentBindingView.SelfClose();
                }
            }


        }




        public class ErrorEntity
        {
            public string Message { get; set; }
            public Exception Exception { get; set; }
            public IErrorInfo InnerErrorInfoSource { get; set; }
            public override string ToString()
            {

                return null;// string.Format("{0}，{1}，{2}", Message, Exception, InnerErrorInfoSource);
            }
        }
        public interface IErrorInfo
        {
            ObservableCollection<ErrorEntity> Errors { get; }
        }

        public interface IValueCanSet<in T>
        {
            T Value { set; }
        }

        public interface IValueCanGet<out T>
        {
            T Value { get; }
        }

        public interface IValueContainer : IErrorInfo
        {
            Type PropertyType { get; }
            Object Value { get; set; }
            bool IsCopyToAllowed { get; set; }
        }

        public interface ICommandModel<TCommand, TResource> : ICommand
        {
            TCommand CommandCore { get; }
            bool LastCanExecuteValue { get; set; }
            TResource Resource { get; set; }
        }

        public class StringResourceReactiveCommandModel : CommandModel<ReactiveCommand, string>
        {

        }

        /// <summary>
        /// 用于封装ICommand的ViewModel。一般包括一个Command实例和对应此实例的一组资源
        /// </summary>
        /// <typeparam name="TCommand">ICommand 详细类型</typeparam>
        /// <typeparam name="TResource">配合Command 的资源类型</typeparam>
        public class CommandModel<TCommand, TResource> : BindableBase<CommandModel<TCommand, TResource>>, ICommandModel<TCommand, TResource>, ICommandWithViewModel
            where TCommand : ICommand
        {
            public override string ToString()
            {
                return Resource.ToString();
            }

            public CommandModel()
            { }
            /// <summary>
            /// 构造器
            /// </summary>
            /// <param name="commandCore">ICommand核心</param>
            /// <param name="resource">初始资源</param>
            public CommandModel(TCommand commandCore, TResource resource)
            {
                CommandCore = commandCore;
                commandCore.CanExecuteChanged += commandCore_CanExecuteChanged;
                Resource = resource;
            }

            void commandCore_CanExecuteChanged(object sender, EventArgs e)
            {
                if (CanExecuteChanged != null)
                {
                    this.CanExecuteChanged(this, e);
                }

            }


            /// <summary>
            /// ICommand核心
            /// </summary>
            public TCommand CommandCore
            {
                get;
                private set;

            }

            //public CommandModel<TCommand, TResource> ConfigCommandCore(Action<TCommand> commandConfigAction)
            //{
            //    commandConfigAction(CommandCore);
            //    return this;
            //}


            /// <summary>
            /// 上一次是否能够运行的值
            /// </summary>
            public bool LastCanExecuteValue
            {
                get { return _LastCanExecuteValueLocator(this).Value; }
                set { _LastCanExecuteValueLocator(this).SetValueAndTryNotify(value); }
            }


            #region Property bool LastCanExecuteValue Setup

            protected Property<bool> _LastCanExecuteValue =
              new Property<bool> { LocatorFunc = _LastCanExecuteValueLocator };
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            static Func<BindableBase, ValueContainer<bool>> _LastCanExecuteValueLocator =
                RegisterContainerLocator<bool>(
                "LastCanExecuteValue",
                model =>
                {
                    model._LastCanExecuteValue =
                        model._LastCanExecuteValue
                        ??
                        new Property<bool> { LocatorFunc = _LastCanExecuteValueLocator };
                    return model._LastCanExecuteValue.Container =
                        model._LastCanExecuteValue.Container
                        ??
                        new ValueContainer<bool>("LastCanExecuteValue", model);
                });

            #endregion



            /// <summary>
            /// 资源
            /// </summary>
            public TResource Resource
            {
                get { return _ResourceLocator(this).Value; }
                set { _ResourceLocator(this).SetValueAndTryNotify(value); }
            }


            #region Property TResource Resource Setup

            protected Property<TResource> _Resource =
              new Property<TResource> { LocatorFunc = _ResourceLocator };
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            static Func<BindableBase, ValueContainer<TResource>> _ResourceLocator =
                RegisterContainerLocator<TResource>(
                "Resource",
                model =>
                {
                    model._Resource =
                        model._Resource
                        ??
                        new Property<TResource> { LocatorFunc = _ResourceLocator };
                    return model._Resource.Container =
                        model._Resource.Container
                        ??
                        new ValueContainer<TResource>("Resource", model);
                });

            #endregion











            /// <summary>
            /// 判断是否可执行
            /// </summary>
            /// <param name="parameter">指定参数</param>
            /// <returns></returns>
            public bool CanExecute(object parameter)
            {
                var s = CommandCore.CanExecute(parameter);
                LastCanExecuteValue = s;
                return s;
            }

            public event EventHandler CanExecuteChanged;

            /// <summary>
            /// 执行
            /// </summary>
            /// <param name="parameter">指定参数</param>
            public void Execute(object parameter)
            {
                var ec = CommandCore as EventCommandBase;
                if (ec != null)
                {
                    ec.OnCommandExecute(parameter as EventCommandEventArgs);
                }
                else
                    CommandCore.Execute(parameter);
            }

            public BindableBase ViewModel
            {
                get
                {
                    var c = CommandCore as ICommandWithViewModel;
                    if (c != null)
                    {
                        return c.ViewModel;
                    }
                    return null;
                }
                set
                {
                    var c = CommandCore as ICommandWithViewModel;
                    if (c != null)
                    {
                        c.ViewModel = value;
                    }

                }
            }
        }

        /// <summary>
        /// 可绑定的CommandVM 扩展方法集
        /// </summary>
        public static class CommandModelExtensions
        {
            /// <summary>
            /// 根据ICommand实例创建CommandModel
            /// </summary>
            /// <typeparam name="TCommand">ICommand实例的具体类型</typeparam>
            /// <typeparam name="TResource">附加资源类型</typeparam>
            /// <param name="command">ICommand实例</param>
            /// <param name="resource">资源实例</param>
            /// <returns>CommandModel实例</returns>
            public static CommandModel<TCommand, TResource> CreateCommandModel<TCommand, TResource>(this TCommand command, TResource resource)
                where TCommand : ICommand
            {
                return new CommandModel<TCommand, TResource>(command, resource);
            }

            /// <summary>
            /// 据ICommand实例创建不具备/弱类型资源的CommandModel
            /// </summary>
            /// <typeparam name="TCommand">ICommand实例的具体类型</typeparam>
            /// <param name="command">ICommand实例</param>
            /// <param name="resource">资源实例</param>
            /// <returns>CommandModel实例</returns>
            public static CommandModel<TCommand, object> CreateCommandModel<TCommand>(this TCommand command, object resource = null)
            where TCommand : ICommand
            {
                return new CommandModel<TCommand, object>(command, null);
            }

            /// <summary>
            /// 为CommandModel指定ViewModel
            /// </summary>
            /// <typeparam name="TCommand">ICommand实例的具体类型</typeparam>
            /// <typeparam name="TResource">附加资源类型</typeparam>
            /// <param name="cmdModel">CommandModel具体实例</param>
            /// <param name="viewModel">ViewModel具体实例</param>
            /// <returns></returns>
            public static CommandModel<TCommand, TResource> WithViewModel<TCommand, TResource>(this CommandModel<TCommand, TResource> cmdModel, BindableBase viewModel)
                where TCommand : ICommand
            {
                //cmdModel.
                var cmd2 = cmdModel.CommandCore as ICommandWithViewModel;
                if (cmd2 != null)
                {
                    cmd2.ViewModel = viewModel;
                }
                return cmdModel;
            }
        }





    }
}

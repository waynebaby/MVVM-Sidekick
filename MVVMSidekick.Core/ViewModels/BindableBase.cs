using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.EventRouting;
using MVVMSidekick.Services;
using MVVMSidekick.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace MVVMSidekick.ViewModels
{
    /// <summary>
    /// <para>具有详细子类型类型参数的模型类型</para>
    /// <para>Model type with detail subtype type parameter.</para>
    /// <para>具有子类详细类型定义的模型。</para>
    /// <example>
    /// public class Class1:BindableBase&lt;Class1&gt;  {}
    /// </example>
    /// </summary>
    /// <typeparam name="TSubClassType">子类类型 / Sub Type</typeparam>
    [DataContract]
    public abstract class BindableBase<TSubClassType> : BindableBase, INotifyDataErrorInfo where TSubClassType : BindableBase<TSubClassType>
    {


        /// <summary>
        /// 获取所有错误
        /// Gets all errors.
        /// </summary>
        /// <returns>错误实体数组 / ErrorEntity[].</returns>
        public override IEnumerable<ErrorEntity> GetAllErrors()
        {
            var errors = GetFieldNames()
                 .SelectMany(name => this.GetValueContainer(name).Errors)
                 .Where(x => x.Value != null)
                 .Select(x => x.Value)
                 .ToArray();
            return errors;
        }
        /// <summary>
        /// 属性容器获取器字典，用于缓存属性访问器
        /// Property container getters dictionary for caching property accessors
        /// </summary>
        protected static Dictionary<string, Func<TSubClassType, IValueContainer>>
            _plainPropertyContainerGetters =
              new Dictionary<string, Func<TSubClassType, IValueContainer>>(StringComparer.CurrentCultureIgnoreCase);

        /// <summary>
        /// 静态构造函数
        /// Static constructor
        /// </summary>
        static BindableBase()
        {

        }

        /// <summary>
        /// 初始化BindableBase类的新实例，设置错误状态监听
        /// Initializes a new instance of the BindableBase class with error state listening
        /// </summary>
        public BindableBase()
        {
            Observable.FromEventPattern<DataErrorsChangedEventArgs>(
                eh => this._ErrorsChanged += eh,
                eh => this._ErrorsChanged -= eh)
                .Subscribe(_ =>
                {
                    HasErrors = GetAllErrors().Any();
                })
                .DisposeWith(this);

            //_BindableInstanceIdLocator(this).SetValueAndTryNotify( string.Format("{0}:{1}", this.GetType().Name, base._instanceIdOfThisType));
        }

        /// <summary>
        /// 获取可绑定实例标识符
        /// Gets the bindable instance identifier
        /// </summary>
        /// <value>可绑定实例标识符 / The bindable instance identifier</value>
        public override string BindableInstanceId
        {
            get { return _instanceIdOfThisType.ToString(); }

        }

        /// <summary>
        /// 重置属性值为默认值
        /// Resets property value to default
        /// </summary>
        /// <typeparam name="T">属性类型 / Property type</typeparam>
        /// <param name="property">要重置的属性 / The property to reset</param>
        public void ResetPropertyValue<T>(Property<T> property)
        {
            if (property != null)
            {

                if (property.Container != null)
                {

                    var newContainer = property.LocateValueContainer(property.Container.Model);
                    property.Container.SetValueAndTryNotify(newContainer.Value);

                }
            }


        }








        /// <summary>
        /// <para>Cast a model instance to current model subtype</para>
        /// <para>将一个 model 引用特化为本子类型的引用</para>
        /// </summary>
        /// <param name="model">some bindable model/某种可绑定model</param>
        /// <returns>Current sub type instance/本类型引用</returns>
        public static TSubClassType CastToCurrentType(BindableBase model)
        {
            return (TSubClassType)model;

        }
        ///// <summary>
        ///// <para>Type cache of container getter</para>
        ///// <para>每个属性类型独占的一个专门的类型缓存。</para>
        ///// </summary>
        ///// <typeparam name="TProperty"></typeparam>
        //protected static class TypeDic<TProperty>
        //{
        //	public static Dictionary<string, Func<TSubClassType, ValueContainer<TProperty>>> _propertyContainerGetters = new Dictionary<string, Func<TSubClassType, ValueContainer<TProperty>>>();

        //}

        /// <summary>
        /// 根据列名获取或设置属性值的索引器
        /// Indexer for getting or setting property values by column name
        /// </summary>
        /// <param name="colName">列名/属性名 / Column/property name</param>
        /// <returns>属性值 / Property value</returns>
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

        /// <summary>
        /// 获取或创建普通定位器
        /// Gets or creates a plain locator
        /// </summary>
        /// <param name="colName">列名 / Column name</param>
        /// <param name="viewModel">视图模型 / View model</param>
        /// <returns>值容器定位器函数 / Value container locator function</returns>
        private static Func<TSubClassType, IValueContainer> GetOrCreatePlainLocator(string colName, BindableBase viewModel)
        {
            Func<TSubClassType, IValueContainer> pf;
            if (!_plainPropertyContainerGetters.TryGetValue(colName, out pf))
            {
                var p = new ValueContainer<object>(colName, viewModel);

                Func<TSubClassType, ValueContainer<object>> tpf = _ => p;
                pf = tpf;
                _plainPropertyContainerGetters[colName] = pf;
                //TypeDic<object>._propertyContainerGetters[colName] = tpf;
            }
            return pf;
        }

        /// <summary>
        /// 获取错误消息
        /// Gets the error message
        /// </summary>
        /// <value>错误消息 / The error message</value>
        public override string ErrorMessage
        {
            get { return _ErrorMessageLocator(this).Value; }
        }

        /// <summary>
        /// 设置错误消息
        /// Sets the error message
        /// </summary>
        /// <param name="value">错误消息值 / Error message value</param>
        protected override void SetErrorMessage(string value)
        {
            _ErrorMessageLocator(this).SetValue(value);
        }
        /// <summary>
        /// 
        /// </summary>
        public void GenrateErrorMessage()
        {
            var sb = new StringBuilder();
            OnGenrateErrorsMessage(GetAllErrors(), sb);
            SetErrorMessageAndTryNotify(sb.ToString());
        }

        /// <summary>
        /// Sets the error and try notify.
        /// </summary>
        /// <param name="value">The value.</param>
        protected override void SetErrorMessageAndTryNotify(string value)
        {
            _ErrorMessageLocator(this).SetValueAndTryNotify(value);
        }




        #region Property string ErrorMessage Setup        
        protected Property<string> _ErrorMessage = new Property<string>(_ErrorMessageLocator);
        static Func<BindableBase, ValueContainer<string>> _ErrorMessageLocator = RegisterContainerLocator<string>(nameof(ErrorMessage), model => model.Initialize(nameof(ErrorMessage), ref model._ErrorMessage, ref _ErrorMessageLocator, _ErrorMessageDefaultValueFactory));
        static Func<string> _ErrorMessageDefaultValueFactory = () => default(string);
        #endregion






        /// <summary>
        /// 注册一个属性容器的定位器。
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="getOrCreateLocatorMethod">属性定位/创建方法 也就是定位器</param>
        /// <returns>
        /// 注册后的定位器
        /// </returns>
        protected static Func<BindableBase, ValueContainer<TProperty>> RegisterContainerLocator<TProperty>(string propertyName, Func<TSubClassType, ValueContainer<TProperty>> getOrCreateLocatorMethod)
        {
            _plainPropertyContainerGetters[propertyName] = getOrCreateLocatorMethod;
            return o => getOrCreateLocatorMethod((TSubClassType)o);
        }
        protected static Func<BindableBase, ValueContainer<TProperty>> RegisterContainerLocator<TProperty>(string propertyName, Func<TSubClassType, string, ValueContainer<TProperty>> getOrCreateLocatorMethod)
        {
            _plainPropertyContainerGetters[propertyName] = o => getOrCreateLocatorMethod((TSubClassType)o, propertyName);
            return o => getOrCreateLocatorMethod((TSubClassType)o, propertyName);
        }

        /// <summary>
        /// 根据属性名获取指定类型的值容器
        /// Gets a value container of specified type by property name
        /// </summary>
        /// <typeparam name="TProperty">属性类型 / Property type</typeparam>
        /// <param name="propertyName">属性名 / Property name</param>
        /// <returns>值容器 / Value container</returns>
        /// <exception cref="System.Exception">
        /// Property Not Exists!
        /// or
        /// Property ' + propertyName + ' is found but it does not match the property type ' + typeof(TProperty).Name + '!
        /// </exception>
        public ValueContainer<TProperty> GetValueContainer<TProperty>(string propertyName)
        {
            Func<TSubClassType, ValueContainer<TProperty>> containerGetterCreater;
            Func<TSubClassType, IValueContainer> contPlanGetter;
            if (!_plainPropertyContainerGetters.TryGetValue(propertyName, out contPlanGetter))
            {
                throw new Exception("Property Not Exists!");

            }

            containerGetterCreater = contPlanGetter as Func<TSubClassType, ValueContainer<TProperty>>;
            if (containerGetterCreater == null)
            {
                throw new Exception("Property '" + propertyName + "' is found but it does not match the property type '" + typeof(TProperty).Name + "'!");
            }

            return containerGetterCreater((TSubClassType)(Object)this);

        }

        /// <summary>
        /// 根据表达式树获取值容器
        /// Gets a value container by expression tree
        /// </summary>
        /// <typeparam name="TProperty">属性类型 / Property type</typeparam>
        /// <param name="expression">表达式树 / Expression tree</param>
        /// <returns>值容器 / Value container</returns>
        public ValueContainer<TProperty> GetValueContainer<TProperty>(Expression<Func<TSubClassType, TProperty>> expression)
        {
            var propName = MVVMSidekick.Utilities.ExpressionHelper.GetPropertyName<TSubClassType, TProperty>(expression);
            return GetValueContainer<TProperty>(propName);

        }




        /// <summary>
        /// 根据属性名获取值容器
        /// Gets a value container by property name
        /// </summary>
        /// <param name="propertyName">属性名 / Property name</param>
        /// <returns>值容器 / Value container</returns>
        /// <exception cref="System.NotImplementedException">当前属性未实现 / Current property is not implemented</exception>
        public override IValueContainer GetValueContainer(string propertyName)
        {
            Func<TSubClassType, IValueContainer> contianerGetterCreater;
            if (!_plainPropertyContainerGetters.TryGetValue(propertyName, out contianerGetterCreater))
            {
                this[propertyName] = null;
                if (!_plainPropertyContainerGetters.TryGetValue(propertyName, out contianerGetterCreater))
                {
                    throw new NotImplementedException(string.Format("Current property \"{0}\" is not implemented", propertyName));
                }
            }
            return contianerGetterCreater((TSubClassType)(Object)this);

        }


        /// <summary>
        /// 根据属性名获取多个值容器
        /// Gets multiple value containers by property names
        /// </summary>
        /// <param name="propertyNames">属性名数组 / Property names array</param>
        /// <returns>值容器数组 / Value containers array</returns>
        public IValueContainer[] GetValueContainers(params string[] propertyNames)
        {
            return propertyNames.Select(pn => GetValueContainer(pn)).ToArray();

        }


        /// <summary>
        /// 根据表达式树获取多个值容器
        /// Gets multiple value containers by expression trees
        /// </summary>
        /// <param name="expressions">表达式树数组 / Expression trees array</param>
        /// <returns>值容器数组 / Value containers array</returns>
        public IValueContainer[] GetValueContainers(params Expression<Func<TSubClassType, object>>[] expressions)
        {

            var names = expressions
                .Select(expression => ExpressionHelper.GetPropertyName(expression))
                .ToArray();

            var rval= GetValueContainers(names);
            return rval;
        }

        /// <summary>
        /// 获取所有属性名，包括静态声明和动态添加的
        /// Gets all property names including statically declared and dynamically added ones
        /// </summary>
        /// <returns>属性名数组 / Property names array</returns>
        public override string[] GetFieldNames()
        {
            return _plainPropertyContainerGetters.Keys.ToArray();
        }

        /// <summary>
        /// 创建当前视图模型的副本
        /// Creates a copy of the current view model
        /// </summary>
        /// <returns>新的视图模型实例 / New view model instance</returns>
        public TSubClassType Clone()
        {
            var x = (TSubClassType)Activator.CreateInstance(typeof(TSubClassType));
            CopyTo(x);
            return x;
        }

        /// <summary>
        /// 将当前视图模型的数据复制到目标视图模型
        /// Copies current view model data to target view model
        /// </summary>
        /// <param name="target">目标视图模型 / Target view model</param>
        public void CopyTo(TSubClassType target)
        {
            foreach (var item in GetFieldNames())
            {
                var ctThis = GetValueContainer(item);
                var ctTarget = target.GetValueContainer(item);



            }
        }


        /// <summary>
        /// 错误状态发生变化时触发的事件
        /// Event triggered when error state changes
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { _ErrorsChanged += value; }
            remove { _ErrorsChanged -= value; }
        }

        /// <summary>
        /// 获取指定属性的错误信息
        /// Gets error information for the specified property
        /// </summary>
        /// <param name="propertyName">属性名 / Property name</param>
        /// <returns>错误信息集合 / Error information collection</returns>
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (this.GetFieldNames().Contains(propertyName))
            {
                return this.GetValueContainer(propertyName).Errors.Values;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取或设置一个值，指示此实例是否有错误
        /// Gets or sets a value indicating whether this instance has errors
        /// </summary>
        /// <value>如果此实例有错误则为 true，否则为 false / true if this instance has errors; otherwise, false</value>
        public bool HasErrors { get => _HasErrorsLocator(this).Value; set => _HasErrorsLocator(this).SetValueAndTryNotify(value); }
        #region Property bool HasErrors Setup        
        protected Property<bool> _HasErrors = new Property<bool>(_HasErrorsLocator);
        static Func<BindableBase, ValueContainer<bool>> _HasErrorsLocator = RegisterContainerLocator(nameof(HasErrors), m => m.Initialize(nameof(HasErrors), ref m._HasErrors, ref _HasErrorsLocator, () => default(bool)));
        #endregion

        /// <summary>
        /// 生成错误消息的虚方法，可由子类重写以自定义错误消息格式
        /// Virtual method for generating error messages, can be overridden by subclasses to customize error message format
        /// </summary>
        /// <param name="errors">错误实体集合 / Error entities collection</param>
        /// <param name="errorMessageBuilder">错误消息构建器 / Error message builder</param>
        protected virtual void OnGenrateErrorsMessage(IEnumerable<ErrorEntity> errors, StringBuilder errorMessageBuilder)
        {
            var sb = errorMessageBuilder;
            var rt = GetAllErrors().Select(x =>
            {
                return sb.Append(x.PropertyName).Append("\t").Append(x.Message).Append("\t").AppendLine(x.Exception == null ? " " : x.Exception.ToString());
            })
                .ToArray();
            this.SetErrorMessageAndTryNotify(sb.ToString());


        }

        //public override IDictionary<string,object >  Values
        //{
        //    get { return new BindableAccesser<TSubClassType>(this); }
        //}
        
        /// <summary>
        /// 获取或设置分配给此模型的本地事件路由器（延迟加载）
        /// Gets or sets the local event router assigned to this model (lazy loaded)
        /// </summary>
        /// <value>本地事件路由器 / The local event router</value>
        public override EventRouter LocalEventRouter
        {
            get =>
                _LocalEventRouterLocator(this).Value;
            set =>
                _LocalEventRouterLocator(this).SetValueAndTryNotify(value);
        }
        #region Property EventRouter LocalEventRouter Setup        
        protected Property<EventRouter> _LocalEventRouter = new Property<EventRouter>(_LocalEventRouterLocator);
        static Func<BindableBase, ValueContainer<EventRouter>> _LocalEventRouterLocator = RegisterContainerLocator(nameof(LocalEventRouter), m => m.Initialize(nameof(LocalEventRouter), ref m._LocalEventRouter, ref _LocalEventRouterLocator, () =>
            new EventRouter()));
        #endregion


    }





    /// <summary>
    /// <para>Base type of bindable model.</para>
    /// <para>可绑定模型的基类。</para>
    /// </summary>
    [DataContract]
    public abstract class BindableBase
        : DisposeGroupBase, INotifyPropertyChanged, IBindable, INotifyPropertyChanging
    {

        /// <summary>
        /// 获取所有错误信息的抽象方法
        /// Abstract method to get all error information
        /// </summary>
        /// <returns>错误实体集合 / Error entities collection</returns>
        public abstract IEnumerable<ErrorEntity> GetAllErrors();
        
        /// <summary>
        /// 释放非托管资源和（可选的）托管资源
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">如果为 true，则释放托管和非托管资源；如果为 false，则仅释放非托管资源 / true to release both managed and unmanaged resources; false to release only unmanaged resources</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// 初始化BindableBase类的新实例
        /// Initializes a new instance of the BindableBase class
        /// </summary>
        public BindableBase()
        {
            ValueContainers = new ValueContainerIndexer(this);
        }
        
        /// <summary>
        /// 获取此模型的值容器索引器
        /// Gets the value container indexer for this model
        /// </summary>
        public ValueContainerIndexer ValueContainers { get; private set; }

        /// <summary>
        /// 错误状态变化事件的内部事件字段
        /// Internal event field for errors changed event
        /// </summary>
        protected event EventHandler<DataErrorsChangedEventArgs> _ErrorsChanged;
        
        /// <summary>
        /// 引发错误状态变化事件
        /// Raises the errors changed event
        /// </summary>
        /// <param name="propertName">属性名 / Property name</param>
        protected internal void RaiseErrorsChanged(string propertName)
        {
            if (_ErrorsChanged != null)
            {
                _ErrorsChanged(this, new DataErrorsChangedEventArgs(propertName));
            }
        }

        /// <summary>
        /// 获取可绑定实例标识符的抽象属性
        /// Abstract property to get the bindable instance identifier
        /// </summary>
        /// <value>可绑定实例标识符 / The bindable instance identifier</value>
        abstract public String BindableInstanceId { get; }

        /// <summary>
        /// 验证是否激活的私有字段
        /// Private field for validation activation state
        /// </summary>
        private bool _IsValidationActivated = false;
        
        /// <summary>
        /// 获取或设置此模型是否激活验证。这只是一个标记，内部逻辑并不依赖此值
        /// Gets or sets if validation is activated for this model. This is a flag only, internal logic does not depend on this
        /// </summary>
        /// <value>如果激活验证则为 true，否则为 false / true if validation is activated; otherwise, false</value>
        public bool IsValidationActivated
        {
            get { return _IsValidationActivated; }
            set { _IsValidationActivated = value; }
        }

        /// <summary>
        /// 通知是否激活的私有字段
        /// Private field for notification activation state
        /// </summary>
        private bool _IsNotificationActivated = true;
        
        /// <summary>
        /// 获取或设置此模型是否激活属性变化通知
        /// Gets or sets if property change notification is activated for this model
        /// </summary>
        /// <value>如果激活通知则为 true，否则为 false / true if notification is activated; otherwise, false</value>
        public bool IsNotificationActivated
        {
            get { return (!IsInDesignMode) ? _IsNotificationActivated : false; }
            set { _IsNotificationActivated = value; }
        }

        #region  Index and property names/索引与字段名
        /// <summary>
        /// 获取在子类型中定义的或在运行时动态添加的所有属性名
        /// Get all property names that were defined in subtype, or added dynamically in runtime
        /// </summary>
        /// <returns>属性名数组 / Property names array</returns>
        public abstract string[] GetFieldNames();

        /// <summary>
        /// 根据名称获取或设置属性值的索引器
        /// Indexer for getting or setting property values by name
        /// </summary>
        /// <param name="name">属性名 / Property name</param>
        /// <returns>属性值 / Property value</returns>
        public abstract object this[string name] { get; set; }

        #endregion

        #region Propery Changed Logic/ Propery Changed事件相关逻辑

        /// <summary>
        /// 引发属性已更改事件
        /// Raises the property changed event
        /// </summary>
        /// <param name="e">属性更改事件参数 / Property changed event args</param>
        /// <param name="anotherObjectSurce">可选的事件源对象 / Optional event source object</param>
        protected internal void RaisePropertyChanged(PropertyChangedEventArgs e, object anotherObjectSurce = null)
        {
            this.PropertyChanged?.Invoke(anotherObjectSurce ?? this, e);

        }

        /// <summary>
        /// 当属性被更改且通知被激活时引发的事件
        /// Event that raised when properties were changed and notification was activated
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 引发属性正在更改事件
        /// Raises the property changing event
        /// </summary>
        /// <param name="e">属性正在更改事件参数 / Property changing event args</param>
        /// <param name="anotherObjectSurce">可选的事件源对象 / Optional event source object</param>
        protected internal void RaisePropertyChanging(PropertyChangingEventArgs e, object anotherObjectSurce = null)
        {
            this.PropertyChanging?.Invoke(anotherObjectSurce ?? this, e);
        }

        /// <summary>
        /// 当属性即将更改且通知被激活时引发的事件
        /// Event that raised when properties are about to change and notification was activated
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region 验证与错误相关逻辑

        /// <summary>
        /// 获取此模型的验证错误信息
        /// Gets the validation error information of this model
        /// </summary>
        /// <value>错误信息 / Error information</value>
        public abstract string ErrorMessage { get; }
        
        /// <summary>
        /// 设置此模型的验证错误信息
        /// Sets the validation error information of this model
        /// </summary>
        /// <param name="value">错误信息值 / Error information value</param>
        protected abstract void SetErrorMessage(string value);

        /// <summary>
        /// 设置此模型的验证错误信息并尝试通过事件通知
        /// Sets the validation error information of this model and tries to notify through events
        /// </summary>
        /// <param name="value">错误信息值 / Error information value</param>
        protected abstract void SetErrorMessageAndTryNotify(string value);

        /// <summary>
        /// 根据属性名获取值容器的抽象方法
        /// Abstract method to get value container by property name
        /// </summary>
        /// <param name="propertyName">属性名 / Property name</param>
        /// <returns>值容器 / Value container</returns>
        public abstract IValueContainer GetValueContainer(string propertyName);

        #endregion

        /// <summary>
        /// 仅在此模型对象中生效的事件路由器
        /// The event router that effects only in this model object
        /// </summary>
        public abstract EventRouter LocalEventRouter { get; set; }

        /// <summary>
        /// 全局生效的事件路由器引用
        /// The event router reference that effects globally
        /// </summary>
        /// <value>全局事件路由器 / The global event router</value>
        public EventRouter GlobalEventRouter
        {
            get { return EventRouter.Instance; }
        }

        /// <summary>
        /// 获取一个值，指示此实例是否处于设计模式
        /// Gets a value indicating whether this instance is in design mode
        /// </summary>
        public bool IsInDesignMode => (ServiceProviderLocator.RootServiceProvider?.GetService<ITellDesignTimeService>() ?? new InDesignTime())?.IsInDesignMode ?? false;
    }
}

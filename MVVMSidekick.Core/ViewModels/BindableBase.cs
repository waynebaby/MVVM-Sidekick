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
    /// <para>Model type with detail subtype type parameter.</para>
    /// <para>具有子类详细类型定义的model </para>
    /// <example>
    /// public class Class1:BindableBase&lt;Class1&gt;  {}
    /// </example>
    /// </summary>
    /// <typeparam name="TSubClassType">Sub Type / 子类类型</typeparam>
    [DataContract]
    public abstract class BindableBase<TSubClassType> : BindableBase, INotifyDataErrorInfo where TSubClassType : BindableBase<TSubClassType>
    {


        /// <summary>
        /// Gets all errors.
        /// </summary>
        /// <returns>ErrorEntity[].</returns>
        public override IEnumerable<ErrorEntity> GetAllErrors()
        {
            var errors = GetFieldNames()
                 .SelectMany(name => this.GetValueContainer(name).Errors)
                 .Where(x => x.Value != null)
                 .Select(x => x.Value)
                 .ToArray();
            return errors;
        }
        protected static Dictionary<string, Func<TSubClassType, IValueContainer>>
            _plainPropertyContainerGetters =
              new Dictionary<string, Func<TSubClassType, IValueContainer>>(StringComparer.CurrentCultureIgnoreCase);

        static BindableBase()
        {

        }




        /// <summary>
        /// Initializes a new instance of the <see cref="BindableBase{TSubClassType}"/> class.
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
        /// Gets the bindable instance identifier.
        /// </summary>
        /// <value>The bindable instance identifier.</value>

        public override string BindableInstanceId
        {
            get { return _instanceIdOfThisType.ToString(); }

        }






        /// <summary>
        /// 清除值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">The property.</param>
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
        /// 根据索引获取属性值
        /// </summary>
        /// <param name="colName">Name of the col.</param>
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

        /// <summary>
        /// Gets the or create plain locator.
        /// </summary>
        /// <param name="colName">Name of the col.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns>Func&lt;TSubClassType, IValueContainer&gt;.</returns>
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
        /// Gets the error.
        /// </summary>
        /// <value>The error.</value>
        public override string ErrorMessage
        {
            get { return _ErrorMessageLocator(this).Value; }
        }

        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="value">The value.</param>
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
        /// 根据属性名取得一个值容器
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyName">属性名</param>
        /// <returns>值容器</returns>
        /// <exception cref="System.Exception">
        /// Property Not Exists!
        /// or
        /// Property ' + propertyName + ' is found but it does not match the property type ' + type of(TProperty).Name + '!
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
        /// 根据表达式树取得一个值容器
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">表达式树</param>
        /// <returns>
        /// 值容器
        /// </returns>
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
        /// <exception cref="System.NotImplementedException"></exception>
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
        /// 根据属性名取得多个值容器
        /// </summary>
        /// <param name="propertyNames">The property names.</param>
        /// <returns>值容器</returns>
        public IValueContainer[] GetValueContainers(params string[] propertyNames)
        {
            return propertyNames.Select(pn => GetValueContainer(pn)).ToArray();

        }


        /// <summary>
        /// 根据表达式树取得多个值容器
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <returns>值容器</returns>
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
        /// </summary>
        /// <returns>System.String[].</returns>
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


        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="target">The target.</param>
        public void CopyTo(TSubClassType target)
        {
            foreach (var item in GetFieldNames())
            {
                var ctThis = GetValueContainer(item);
                var ctTarget = target.GetValueContainer(item);



            }
        }


        /// <summary>
        /// Occurs when [errors changed].
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { _ErrorsChanged += value; }
            remove { _ErrorsChanged -= value; }
        }



        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Collections.IEnumerable.</returns>
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
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
        //public bool HasErrors
        //{
        //    get
        //    {
        //        //  return false;
        //        RefreshErrors();
        //        return !string.IsNullOrEmpty(this.ErrorMessage);

        //    }
        //}




        public bool HasErrors { get => _HasErrorsLocator(this).Value; set => _HasErrorsLocator(this).SetValueAndTryNotify(value); }
        #region Property bool HasErrors Setup        
        protected Property<bool> _HasErrors = new Property<bool>(_HasErrorsLocator);
        static Func<BindableBase, ValueContainer<bool>> _HasErrorsLocator = RegisterContainerLocator(nameof(HasErrors), m => m.Initialize(nameof(HasErrors), ref m._HasErrors, ref _HasErrorsLocator, () => default(bool)));
        #endregion

        /// <summary>
        /// Refreshes the errors.
        /// </summary>
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
        /// 给这个模型分配的消息路由引用（延迟加载）
        /// </summary>
        /// <value>The event router.</value>



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
    /// <para>ViewModel 基类</para>
    /// </summary>
    [DataContract]
    public abstract class BindableBase
        : DisposeGroupBase, INotifyPropertyChanged, IBindable, INotifyPropertyChanging
    {

        public abstract IEnumerable<ErrorEntity> GetAllErrors();
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        public BindableBase()
        {
            ValueContainers = new ValueContainerIndexer(this);
        }
        public ValueContainerIndexer ValueContainers { get; private set; }

        /// <summary>
        /// Occurs when [_ errors changed].
        /// </summary>
        protected event EventHandler<DataErrorsChangedEventArgs> _ErrorsChanged;
        /// <summary>
        /// Raises the errors changed.
        /// </summary>
        /// <param name="propertName">Name of the propert.</param>
        protected internal void RaiseErrorsChanged(string propertName)
        {
            if (_ErrorsChanged != null)
            {
                _ErrorsChanged(this, new DataErrorsChangedEventArgs(propertName));
            }
        }

        /// <summary>
        /// Gets the bindable instance identifier.
        /// </summary>
        /// <value>The bindable instance identifier.</value>
        abstract public String BindableInstanceId { get; }





        /// <summary>
        /// The _ is validation activated
        /// </summary>
        private bool _IsValidationActivated = false;
        /// <summary>
        /// <para>Gets ot sets if the validation is activatied. This is a flag only， internal logic is not depend on this.</para>
        /// <para>读取/设置 此模型是否激活验证。这只是一个标记，内部逻辑并没有参考这个值</para>
        /// </summary>
        /// <value><c>true</c> if this instance is validation activated; otherwise, <c>false</c>.</value>
        public bool IsValidationActivated
        {
            get { return _IsValidationActivated; }
            set { _IsValidationActivated = value; }
        }

        /// <summary>
        /// The _ is notification activated
        /// </summary>
        private bool _IsNotificationActivated = true;
        /// <summary>
        /// <para>Gets ot sets if the property change notification is activatied. </para>
        /// <para>读取/设置 此模型是否激活变化通知</para>
        /// </summary>
        /// <value><c>true</c> if this instance is notification activated; otherwise, <c>false</c>.</value>
        public bool IsNotificationActivated
        {
            get { return (!IsInDesignMode) ? _IsNotificationActivated : false; }
            set { _IsNotificationActivated = value; }
        }






        ///// <summary>
        /////  <para>0 for not disposed, 1 for disposed</para>
        /////  <para>0 表示没有被Dispose 1 反之</para>
        ///// </summary>
        //private int disposedFlag = 0;

        #region  Index and property names/索引与字段名
        /// <summary>
        /// <para>Get all property names that were defined in subtype, or added objectly in runtime</para>
        /// <para>取得本VM实例已经定义的所有字段名。其中包括静态声明的和动态添加的。</para>
        /// </summary>
        /// <returns>String[]  Property names/字段名数组</returns>
        public abstract string[] GetFieldNames();

        ///// <summary>
        ///// <para>Gets or sets  poperty values by property name index.</para>
        ///// <para>使用索引方式取得/设置字段值</para>
        ///// </summary>
        ///// <param name="name">Property name/字段名</param>
        ///// <returns>Property value/字段值</returns>
        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.Object.</returns>
        public abstract object this[string name] { get; set; }


        #endregion



        #region Propery Changed Logic/ Propery Changed事件相关逻辑


        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="lazyEAFactory">The lazy ea factory.</param>
        protected internal void RaisePropertyChanged(PropertyChangedEventArgs e, object anotherObjectSurce = null)
        {
            this.PropertyChanged?.Invoke(anotherObjectSurce ?? this, e);

        }

        /// <summary>
        /// <para>Event that raised when properties were changed and Notification was activited</para>
        /// <para> VM属性任何绑定用值被修改后,在启用通知情况下触发此事件</para>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="lazyEAFactory">The lazy ea factory.</param>
        protected internal void RaisePropertyChanging(PropertyChangingEventArgs e, object anotherObjectSurce = null)
        {
            this.PropertyChanging?.Invoke(anotherObjectSurce ?? this, e);
        }

        /// <summary>
        /// <para>Event that raised when properties were changed and Notification was activited</para>
        /// <para> VM属性任何绑定用值被修改后,在启用通知情况下触发此事件</para>
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;


        #endregion

        #region 验证与错误相关逻辑






        ///// <summary>
        ///// Checks the error.
        ///// </summary>
        ///// <param name="test">The test.</param>
        ///// <param name="errorMessage">The error message.</param>
        ///// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        //protected bool CheckError(Func<Boolean> test, string errorMessage)
        //{

        //    var rval = test();
        //    if (rval)
        //    {
        //        SetErrorAndTryNotify(errorMessage);
        //    }
        //    return rval;

        //}


        ///// <summary>
        ///// 验证错误内容
        ///// </summary>
        //string IDataErrorInfo.ErrorMessage
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
        /// <value>The error.</value>
        public abstract string ErrorMessage { get; }
        /// <summary>
        /// <para>Sets the validate error of this model </para>
        /// <para>设置错误内容</para>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ErrorMessage string/错误内容字符串</returns>
        protected abstract void SetErrorMessage(string value);

        /// <summary>
        /// <para>Sets the validate error of this model and notify </para>
        /// <para>设置错误内容并且尝试用事件通知</para>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>ErrorMessage string/错误内容字符串</returns>
        protected abstract void SetErrorMessageAndTryNotify(string value);



        public abstract IValueContainer GetValueContainer(string propertyName);




        #endregion


        //   public abstract bool IsUIBusy { get; set; }






        /// <summary>
        /// The Event Router that effects only in this Model Object/本Model生效的EventRouter
        /// </summary>
        public abstract EventRouter LocalEventRouter { get; set; }


        /// <summary>
        /// The Event Router that effects Globally /全局生效的Event Router引用
        /// </summary>
        /// <value>The global event router.</value>

        public EventRouter GlobalEventRouter
        {
            get { return EventRouter.Instance; }
        }


        public bool IsInDesignMode => (ServiceProviderLocator.RootServiceProvider?.GetService<ITellDesignTimeService>() ?? new InDesignTime())?.IsInDesignMode ?? false;
    }
}

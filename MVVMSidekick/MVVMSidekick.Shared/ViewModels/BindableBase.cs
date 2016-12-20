using MVVMSidekick.EventRouting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace MVVMSidekick.ViewModels
{

    /// <summary>
    /// <para>Base type of bindable model.</para>
    /// <para>ViewModel 基类</para>
    /// </summary>
    [DataContract]
    public abstract class BindableBase
        : DisposeGroupBase, INotifyPropertyChanged, IBindable
    {
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

        /// <summary>
        /// Gets a value indicating whether this instance is in design mode.
        /// </summary>
        /// <value><c>true</c> if this instance is in design mode; otherwise, <c>false</c>.</value>
        public static bool IsInDesignMode { get { return Utilities.Runtime.IsInDesignMode; } }






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
        protected internal void RaisePropertyChanged(Func<PropertyChangedEventArgs> lazyEAFactory)
        {


            if (this.PropertyChanged != null)
            {
                var ea = lazyEAFactory();
                this.PropertyChanged(this, ea);
            }


        }

        /// <summary>
        /// <para>Event that raised when properties were changed and Notification was activited</para>
        /// <para> VM属性任何绑定用值被修改后,在启用通知情况下触发此事件</para>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


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

    }
}

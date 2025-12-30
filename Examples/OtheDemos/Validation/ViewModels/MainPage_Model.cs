using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Validation.ViewModels
{

    [DataContract]
    /// <summary>
    /// <para>主页面的视图模型，演示验证功能</para>
    /// <para>View model for the main page, demonstrates validation functionality</para>
    /// </summary>
    public class MainPage_Model : ViewModelBase<MainPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

        /// <summary>
        /// <para>初始化 MainPage_Model 类的新实例</para>
        /// <para>Initializes a new instance of the MainPage_Model class</para>
        /// </summary>
        public MainPage_Model()
        {
            if (IsInDesignMode)
            {
                Title = "Title is a little different in Design mode";
            }
            var n = new SomeBindable() {  N1=1000,N2=5555};

            Number1 = n.N1;
            Number2 = n.N2;

        }

        //propvm tab tab string tab Title
        /// <summary>
        /// <para>获取或设置页面标题</para>
        /// <para>Gets or sets the page title</para>
        /// </summary>
        public String Title
        {
            get { return _TitleLocator(this).Value; }
            set { _TitleLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String Title Setup
        protected Property<String> _Title = new Property<String> { LocatorFunc = _TitleLocator };
        static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator<String>("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
        static Func<String> _TitleDefaultValueFactory = () => "Title is Here";
        #endregion



        #region Life Time Event Handling

        ///// <summary>
        ///// <para>当此视图模型实例被设置到视图的ViewModel属性时，将由视图调用</para>
        ///// <para>This will be invoked by view when this viewmodel instance is set to view's ViewModel property</para>
        ///// </summary>
        ///// <param name="view">设置目标 / Set target</param>
        ///// <param name="oldValue">设置前的值 / Value before set</param>
        ///// <returns>任务等待器 / Task awaiter</returns>
        //protected override Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
        //{
        //    return base.OnBindedToView(view, oldValue);
        //}

        ///// <summary>
        ///// <para>当ViewModel属性中的此视图模型实例被覆盖时，将由视图调用</para>
        ///// <para>This will be invoked by view when this instance of viewmodel in ViewModel property is overwritten</para>
        ///// </summary>
        ///// <param name="view">覆盖目标视图 / Overwrite target view</param>
        ///// <param name="newValue">新的替换值 / The value replacing</param>
        ///// <returns>任务等待器 / Task awaiter</returns>
        //protected override Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue)
        //{
        //    return base.OnUnbindedFromView(view, newValue);
        //}

        ///// <summary>
        ///// <para>当视图触发Load事件且此视图模型实例已在视图的ViewModel属性中时，由视图调用</para>
        ///// <para>This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property</para>
        ///// </summary>
        ///// <param name="view">触发Load事件的视图 / View that firing Load event</param>
        ///// <returns>任务等待器 / Task awaiter</returns>
        //protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewLoad(view);
        //}

        ///// <summary>
        ///// <para>当视图触发Unload事件且此视图模型实例仍在视图的ViewModel属性中时，由视图调用</para>
        ///// <para>This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's ViewModel property</para>
        ///// </summary>
        ///// <param name="view">触发Unload事件的视图 / View that firing Unload event</param>
        ///// <returns>任务等待器 / Task awaiter</returns>
        //protected override Task OnBindedViewUnload(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewUnload(view);
        //}

        ///// <summary>
        ///// <para>如果dispose操作发生异常，将在此处理</para>
        ///// <para>If dispose actions got exceptions, will handled here</para>
        ///// </summary>
        ///// <param name="exceptions">异常和dispose信息 / The exception and dispose infomation</param>
        //protected override async void OnDisposeExceptions(IList<DisposeInfo> exceptions)
        //{
        //    base.OnDisposeExceptions(exceptions);
        //    await TaskExHelper.Yield();
        //}

        #endregion

        /// <summary>
        /// <para>这里是一个计算数字加法的Validation例子</para>
        /// <para>Here is a Validation example for calculating number addition</para>
        /// </summary>

        /// <summary>
        /// <para>第一个数字</para>
        /// <para>First number</para>
        /// </summary>
        public Decimal Number1
        {
            get { return _Number1Locator(this).Value; }
            set { _Number1Locator(this).SetValueAndTryNotify(value); }
        }
        #region Property Decimal Number1 Setup        
        protected Property<Decimal> _Number1 = new Property<Decimal> { LocatorFunc = _Number1Locator };
        static Func<BindableBase, ValueContainer<Decimal>> _Number1Locator = RegisterContainerLocator<Decimal>(nameof(Number1), model => model.Initialize(nameof(Number1), ref model._Number1, ref _Number1Locator, _Number1DefaultValueFactory));
        static Func<Decimal> _Number1DefaultValueFactory = () => default(Decimal);
        #endregion

        /// <summary>
        /// <para>第二个数字</para>
        /// <para>Second number</para>
        /// </summary>
        public decimal Number2
        {
            get { return _Number2Locator(this).Value; }
            set { _Number2Locator(this).SetValueAndTryNotify(value); }
        }
        #region Property decimal Number2 Setup        
        protected Property<decimal> _Number2 = new Property<decimal> { LocatorFunc = _Number2Locator };
        static Func<BindableBase, ValueContainer<decimal>> _Number2Locator = RegisterContainerLocator<decimal>(nameof(Number2), model => model.Initialize(nameof(Number2), ref model._Number2, ref _Number2Locator, _Number2DefaultValueFactory));
        static Func<decimal> _Number2DefaultValueFactory = () => default(decimal);
        #endregion

        /// <summary>
        /// <para>数字加法计算结果</para>
        /// <para>Number addition calculation result</para>
        /// </summary>
        public string NumberResult
        {
            get { return _NumberResultLocator(this).Value; }
            set { _NumberResultLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string NumberResult Setup        
        protected Property<string> _NumberResult = new Property<string> { LocatorFunc = _NumberResultLocator };
        static Func<BindableBase, ValueContainer<string>> _NumberResultLocator = RegisterContainerLocator<string>(nameof(NumberResult), model => model.Initialize(nameof(NumberResult), ref model._NumberResult, ref _NumberResultLocator, _NumberResultDefaultValueFactory));
        static Func<string> _NumberResultDefaultValueFactory = () => default(string);
        #endregion

        /// <summary>
        /// <para>视图绑定加载事件处理，设置验证规则</para>
        /// <para>View binding load event handler, setting validation rules</para>
        /// </summary>
        /// <param name="view">绑定的视图 / Bound view</param>
        /// <returns>任务等待器 / Task awaiter</returns>
        protected override async Task OnBindedViewLoad(IView view)
        {
            /// <summary>
            /// <para>检查规则：验证数字结果输入值</para>
            /// <para>Check rules: validate number result input value</para>
            /// </summary>
            this.ListenValueChangedEvents(x => x.NumberResult)
                .Select(_ => this.Number1 + this.Number2)
                .Subscribe(targetValue =>
                {
                    decimal tv;
                    var nc = this.GetValueContainer(x => x.NumberResult);
                    nc.Errors.Clear();
                    if (!decimal.TryParse(this.NumberResult, out tv))
                    {
                        nc.AddErrorEntry("target is invalid");
                    }
                    if (targetValue > tv)
                    {
                        nc.AddErrorEntry("target is bigger");
                    }
                    if (targetValue < tv)
                    {
                        nc.AddErrorEntry("target is smaller");
                    }

                    /// <summary>
                    /// <para>如果有验证错误，生成错误消息</para>
                    /// <para>If there are validation errors, generate error messages</para>
                    /// </summary>
                    if (HasErrors)
                    {
                        GenrateErrorMessage();
                    }
                })
                .DisposeWhenUnload(this);

            await base.OnBindedViewLoad(view);
        }

        /// <summary>
        /// <para>生成错误消息时调用，用于自定义错误消息格式</para>
        /// <para>Called when generating error messages, used to customize error message format</para>
        /// </summary>
        /// <param name="errors">错误实体集合 / Error entity collection</param>
        /// <param name="errorMessageBuilder">错误消息构建器 / Error message builder</param>
        protected override void OnGenrateErrorsMessage(IEnumerable<ErrorEntity> errors, StringBuilder errorMessageBuilder)
        {
            errorMessageBuilder.AppendLine("Hi,");
            base.OnGenrateErrorsMessage(errors, errorMessageBuilder);
            errorMessageBuilder.AppendLine().AppendLine("Bye");
        }
    }

    /// <summary>
    /// <para>某个可绑定对象示例类</para>
    /// <para>Some bindable object example class</para>
    /// </summary>
    public class SomeBindable : BindableBase<SomeBindable>
    {
        /// <summary>
        /// <para>初始化SomeBindable类的新实例</para>
        /// <para>Initializes a new instance of the SomeBindable class</para>
        /// </summary>
        public SomeBindable()
        {
            // Use propery to init value here:
            if (IsInDesignMode)
            {
                //Add design time test data init here. These will not execute in runtime.
            }
        }

        /// <summary>
        /// <para>数字N1属性</para>
        /// <para>Number N1 property</para>
        /// </summary>
        public decimal N1 { get => _N1Locator(this).Value; set => 
                _N1Locator(this).SetValueAndTryNotify(value); }
        #region Property decimal N1 Setup        
        protected Property<decimal> _N1 = new Property<decimal>(_N1Locator);
        static Func<BindableBase, ValueContainer<decimal>> _N1Locator = RegisterContainerLocator(nameof(N1), m => m.Initialize(nameof(N1), ref m._N1, ref _N1Locator, () => default(decimal)));
        #endregion

        /// <summary>
        /// <para>数字N2属性</para>
        /// <para>Number N2 property</para>
        /// </summary>
        public decimal N2 { get => _N2Locator(this).Value; set => _N2Locator(this).SetValueAndTryNotify(value); }
        #region Property decimal N2 Setup        
        protected Property<decimal> _N2 = new Property<decimal>(_N2Locator);
        static Func<BindableBase, ValueContainer<decimal>> _N2Locator = RegisterContainerLocator(nameof(N2), m => m.Initialize(nameof(N2), ref m._N2, ref _N2Locator, () => default(decimal)));
        #endregion

        /// <summary>
        /// <para>使用propvm + tab + tab来创建新的可绑定属性</para>
        /// <para>Use propvm + tab + tab to create a new property of bindable here</para>
        /// </summary>
    }
}
}


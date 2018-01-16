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
    public class MainPage_Model : ViewModelBase<MainPage_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

        public MainPage_Model()
        {
            if (IsInDesignMode)
            {
                Title = "Title is a little different in Design mode";
            }

        }

        //propvm tab tab string tab Title
        public String Title
        {
            get { return _TitleLocator(this).Value; }
            set { _TitleLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String Title Setup
        protected Property<String> _Title = new Property<String>( _TitleLocator };
        static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator<String>("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
        static Func<String> _TitleDefaultValueFactory = () => "Title is Here";
        #endregion



        #region Life Time Event Handling

        ///// <summary>
        ///// This will be invoked by view when this viewmodel instance is set to view's ViewModel property. 
        ///// </summary>
        ///// <param name="view">Set target</param>
        ///// <param name="oldValue">Value before set.</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
        //{
        //    return base.OnBindedToView(view, oldValue);
        //}

        ///// <summary>
        ///// This will be invoked by view when this instance of viewmodel in ViewModel property is overwritten.
        ///// </summary>
        ///// <param name="view">Overwrite target view.</param>
        ///// <param name="newValue">The value replacing </param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue)
        //{
        //    return base.OnUnbindedFromView(view, newValue);
        //}

        ///// <summary>
        ///// This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Load event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewLoad(view);
        //}

        ///// <summary>
        ///// This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's  ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Unload event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewUnload(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewUnload(view);
        //}

        ///// <summary>
        ///// <para>If dispose actions got exceptions, will handled here. </para>
        ///// </summary>
        ///// <param name="exceptions">
        ///// <para>The exception and dispose infomation</para>
        ///// </param>
        //protected override async void OnDisposeExceptions(IList<DisposeInfo> exceptions)
        //{
        //    base.OnDisposeExceptions(exceptions);
        //    await TaskExHelper.Yield();
        //}

        #endregion


        //这里是一个计算数字加法的Validation 例子



        public Decimal Number1
        {
            get { return _Number1Locator(this).Value; }
            set { _Number1Locator(this).SetValueAndTryNotify(value); }
        }
        #region Property Decimal Number1 Setup        
        protected Property<Decimal> _Number1 = new Property<Decimal>( _Number1Locator };
        static Func<BindableBase, ValueContainer<Decimal>> _Number1Locator = RegisterContainerLocator<Decimal>(nameof(Number1), model => model.Initialize(nameof(Number1), ref model._Number1, ref _Number1Locator, _Number1DefaultValueFactory));
        static Func<Decimal> _Number1DefaultValueFactory = () => default(Decimal);
        #endregion



        public decimal Number2
        {
            get { return _Number2Locator(this).Value; }
            set { _Number2Locator(this).SetValueAndTryNotify(value); }
        }
        #region Property decimal Number2 Setup        
        protected Property<decimal> _Number2 = new Property<decimal>( _Number2Locator };
        static Func<BindableBase, ValueContainer<decimal>> _Number2Locator = RegisterContainerLocator<decimal>(nameof(Number2), model => model.Initialize(nameof(Number2), ref model._Number2, ref _Number2Locator, _Number2DefaultValueFactory));
        static Func<decimal> _Number2DefaultValueFactory = () => default(decimal);
        #endregion





        public string NumberResult
        {
            get { return _NumberResultLocator(this).Value; }
            set { _NumberResultLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string NumberResult Setup        
        protected Property<string> _NumberResult = new Property<string>( _NumberResultLocator };
        static Func<BindableBase, ValueContainer<string>> _NumberResultLocator = RegisterContainerLocator<string>(nameof(NumberResult), model => model.Initialize(nameof(NumberResult), ref model._NumberResult, ref _NumberResultLocator, _NumberResultDefaultValueFactory));
        static Func<string> _NumberResultDefaultValueFactory = () => default(string);
        #endregion


        protected override async Task OnBindedViewLoad(IView view)
        {
            //检查规则
            this.ListenChanged(x => x.NumberResult)
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


                    if (HasErrors)
                    {
                        GenrateErrorMessage();
                    }


                })
                .DisposeWhenUnload(this);

            await base.OnBindedViewLoad(view);


        }

        protected override void OnGenrateErrorsMessage(IEnumerable<ErrorEntity> errors, StringBuilder errorMessageBuilder)
        {
            errorMessageBuilder.AppendLine("Hi,");
            base.OnGenrateErrorsMessage(errors, errorMessageBuilder);
            errorMessageBuilder.AppendLine().AppendLine("Bye");

        }
    }

}


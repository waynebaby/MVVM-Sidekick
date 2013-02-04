using MVVMSidekick.Reactive;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.ViewModels
{
    public class Calculator_Model : ViewModelBase<Calculator_Model>
    {

        public Calculator_Model()
        {
            if (IsInDesignMode)
            {
                CurrentInput = "CurrentInput.TEXT";
                LeftNumber = "LeftNumber.TEXT";
                RightNumber = "RightNumber.TEXT";
                Sign = "+";
                State = CaculatorState.Finish;
            }
            
            

        }




        /// <summary>
        /// 当前输入的内容，或者结果
        /// </summary>
        public String CurrentInput
        {
            get { return _CurrentInputLocator(this).Value; }
            set { _CurrentInputLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String CurrentInput Setup
        protected Property<String> _CurrentInput = new Property<String> { LocatorFunc = _CurrentInputLocator };
        static Func<BindableBase, ValueContainer<String>> _CurrentInputLocator = RegisterContainerLocator<String>("CurrentInput", model => model.Initialize("CurrentInput", ref model._CurrentInput, ref _CurrentInputLocator, _CurrentInputDefaultValueFactory));
        static Func<String> _CurrentInputDefaultValueFactory = ()=>"0.";
        #endregion

        /// <summary>
        /// 算式左边的数字
        /// </summary>
        public string LeftNumber
        {
            get { return _LeftNumberLocator(this).Value; }
            set { _LeftNumberLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string LeftNumber Setup
        protected Property<string> _LeftNumber = new Property<string> { LocatorFunc = _LeftNumberLocator };
        static Func<BindableBase, ValueContainer<string>> _LeftNumberLocator = RegisterContainerLocator<string>("LeftNumber", model => model.Initialize("LeftNumber", ref model._LeftNumber, ref _LeftNumberLocator, _LeftNumberDefaultValueFactory));
        static Func<string> _LeftNumberDefaultValueFactory = null;
        #endregion

        /// <summary>
        /// 算式的符号
        /// </summary>
        public String Sign
        {
            get { return _SignLocator(this).Value; }
            set { _SignLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String Sign Setup
        protected Property<String> _Sign = new Property<String> { LocatorFunc = _SignLocator };
        static Func<BindableBase, ValueContainer<String>> _SignLocator = RegisterContainerLocator<String>("Sign", model => model.Initialize("Sign", ref model._Sign, ref _SignLocator, _SignDefaultValueFactory));
        static Func<String> _SignDefaultValueFactory = null;
        #endregion

        /// <summary>
        /// 算式右边的数字
        /// </summary>
        public string RightNumber
        {
            get { return _RightNumberLocator(this).Value; }
            set { _RightNumberLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string RightNumber Setup
        protected Property<string> _RightNumber = new Property<string> { LocatorFunc = _RightNumberLocator };
        static Func<BindableBase, ValueContainer<string>> _RightNumberLocator = RegisterContainerLocator<string>("RightNumber", model => model.Initialize("RightNumber", ref model._RightNumber, ref _RightNumberLocator, _RightNumberDefaultValueFactory));
        static Func<string> _RightNumberDefaultValueFactory = null;
        #endregion



        /// <summary>
        /// 任何数字与小数点的按钮
        /// </summary>
        public CommandModel<ReactiveCommand, String> CommandInputNumber
        {
            get { return _CommandInputNumberLocator(this).Value; }
            set { _CommandInputNumberLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandInputNumber Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandInputNumber = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandInputNumberLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandInputNumberLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandInputNumber", model => model.Initialize("CommandInputNumber", ref model._CommandInputNumber, ref _CommandInputNumberLocator, _CommandInputNumberDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandInputNumberDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //cmd.Subscribe (_=>{ } ).RegisterDisposeToViewModel(model); //Config it if needed
                return cmd.CreateCommandModel("InputNumber");
            };
        #endregion


        /// <summary>
        /// 符号输入
        /// </summary>
        public CommandModel<ReactiveCommand, String> CommandInputSign
        {
            get { return _CommandInputSignLocator(this).Value; }
            set { _CommandInputSignLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandInputSign Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandInputSign = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandInputSignLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandInputSignLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandInputSign", model => model.Initialize("CommandInputSign", ref model._CommandInputSign, ref _CommandInputSignLocator, _CommandInputSignDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandInputSignDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //cmd.Subscribe (_=>{ } ).RegisterDisposeToViewModel(model); //Config it if needed
                return cmd.CreateCommandModel("InputSign");
            };
        #endregion

        /// <summary>
        /// 计算，即等号按钮
        /// </summary>
        public CommandModel<ReactiveCommand, String> CommandCaculate
        {
            get { return _CommandCaculateLocator(this).Value; }
            set { _CommandCaculateLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandCaculate Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandCaculate = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandCaculateLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandCaculateLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandCaculate", model => model.Initialize("CommandCaculate", ref model._CommandCaculate, ref _CommandCaculateLocator, _CommandCaculateDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandCaculateDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //cmd.Subscribe (_=>{ } ).RegisterDisposeToViewModel(model); //Config it if needed
                return cmd.CreateCommandModel("=");
            };
        #endregion


        /// <summary>
        /// 清除按钮
        /// </summary>
        public CommandModel<ReactiveCommand, String> CommandClear
        {
            get { return _CommandClearLocator(this).Value; }
            set { _CommandClearLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandClear Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandClear = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandClearLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandClearLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandClear", model => model.Initialize("CommandClear", ref model._CommandClear, ref _CommandClearLocator, _CommandClearDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandClearDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //cmd.Subscribe (_=>{ } ).RegisterDisposeToViewModel(model); //Config it if needed
                return cmd.CreateCommandModel("CE");
            };
        #endregion


        /// <summary>
        /// 当前状态
        /// </summary>
        public CaculatorState State
        {
            get { return _StateLocator(this).Value; }
            set { _StateLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CaculatorStatus State Setup
        protected Property<CaculatorState> _State = new Property<CaculatorState> { LocatorFunc = _StateLocator };
        static Func<BindableBase, ValueContainer<CaculatorState>> _StateLocator = RegisterContainerLocator<CaculatorState>("State", model => model.Initialize("State", ref model._State, ref _StateLocator, _StateDefaultValueFactory));
        static Func<CaculatorState> _StateDefaultValueFactory = () => CaculatorState.InputingLeft;
        #endregion



    }

    /// <summary>
    /// 计算器的状态
    /// </summary>
    public enum CaculatorState
    {
        /// <summary>
        /// 输入左数字
        /// </summary>
        InputingLeft,
        /// <summary>
        /// 输入计算符号
        /// </summary>
        InputingSign,
        /// <summary>
        /// 输入右数字
        /// </summary>
        InputingRight,
        /// <summary>
        /// 计算完毕
        /// </summary>
        Finish
    }
}

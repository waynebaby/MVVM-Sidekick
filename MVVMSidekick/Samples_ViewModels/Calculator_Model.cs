using MVVMSidekick.Reactive;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Threading;


namespace Samples.ViewModels
{
    public class Calculator_Model : ViewModelBase<Calculator_Model, String>
    {

        protected override Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
        {


            return base.OnBindedToView(view, oldValue);


        }


        protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        {
            Init();
            return base.OnBindedViewLoad(view);
        }
        private void Init()
        {

            ResetStatus();
            if (IsInDesignMode)
            {
                CurrentInput = "CurrentInput.TEXT55555555555555555555555555555555";
                LeftNumber = "LeftNumber.TEXT88888888888888888888";
                RightNumber = "RightNumber.TEXTTEXT55555555555555555555555555555555";
                Sign = "+";

            }


            this.GetValueContainer(x => x.CurrentInput)
                .GetNewValueObservable()
                .Subscribe
                (
                    nvp =>
                    {
                        switch (this.State)
                        {
                            case CalculatorState.InputingLeft:
                                this.LeftNumber = decimal.Parse(nvp.EventArgs).ToString();
                                break;

                            case CalculatorState.InputingRight:
                                this.RightNumber = decimal.Parse(nvp.EventArgs).ToString();
                                break;

                            default:
                                break;
                        }

                    }
                ).DisposeWith(this);

            this.CommandInputNumber
                .CommandCore
                .Where(e => e.EventArgs.Parameter.ToString() != ".")
                .Subscribe(
                    e =>
                    {
                        if (State == CalculatorState.Finished)
                        {
                            ResetStatus();
                        }
                        else if (State == CalculatorState.InputedSign)
                        {
                            State = CalculatorState.InputingRight;
                            ResetPropertyValue(_IsPointed);
                            ResetPropertyValue(_CurrentInput);

                        }
                        string output = null;
                        if (IsPointed)
                        {
                            output = CurrentInput + e.EventArgs.Parameter.ToString();

                        }
                        else
                        {
                            output = CurrentInput.TrimEnd('.') + e.EventArgs.Parameter.ToString() + '.';
                        }

                        if (!output.StartsWith("0."))
                        {
                            CurrentInput = output.TrimStart('0');
                        }
                    }
                )
                .DisposeWith(this);


            this.CommandInputNumber
                .CommandCore
                .Where(e => e.EventArgs.Parameter.ToString() == ".")
                .Subscribe(e => IsPointed = true)
                .DisposeWith(this);


            this.CommandBackspace
                .CommandCore
                .Where(e => IsPointed && CurrentInput.EndsWith("."))
                .Subscribe(_ => IsPointed = false)
                .DisposeWith(this);

            this.CommandBackspace
                .CommandCore
                .Where(e => !(IsPointed && CurrentInput.EndsWith(".")))
                .Subscribe(
                    e =>
                    {
                        var tempv = CurrentInput;
                        if (tempv.Trim('-').Length > 2)
                        {
                            CurrentInput = tempv.Substring(tempv.Length - 1);
                        }
                        else
                        {
                            ResetPropertyValue(_CurrentInput);
                        }

                    }
                );

            this.CommandInputSign
                .CommandCore
                .Subscribe(
                    e =>
                    {
                        this.Sign = e.EventArgs.Parameter.ToString();
                        if (State == CalculatorState.Finished)
                        {
                            LeftNumber = CurrentInput;
                            RightNumber = "0.";

                        }
                        State = CalculatorState.InputedSign;

                    }
                )
                .DisposeWith(this);

            this.CommandClear
                .CommandCore
                .Subscribe(
                    e =>
                    {
                        ResetStatus();
                    }
                )
                .DisposeWith(this);
            this.CommandCalculate.CommandCore
                .Where(
                    e => State == CalculatorState.InputingRight || State == CalculatorState.Finished
                )
                .Subscribe
                (
                    e =>
                    {
                        if (State == CalculatorState.Finished)
                        {
                            LeftNumber = Result;
                        }
                        State = CalculatorState.Calculating;
                        var left = decimal.Parse(LeftNumber);
                        var right = decimal.Parse(RightNumber);
                        decimal result = 0;
                        switch (Sign)
                        {
                            case "+":

                                result = left + right;
                                break;
                            case "-":

                                result = left - right;
                                break;
                            case "*":

                                result = left * right;
                                break;
                            case "/":
                                result = left / right;
                                break;
                            default:
                                break;

                        }
                        Result = result.ToString();

                        IsPointed = Result.Contains(".");
                        CurrentInput = IsPointed ? Result : Result + ".";
                        State = CalculatorState.Finished;
                    }

                )
                .DisposeWith(this);

            this.CommandChangeSignOfCurrentInput
                .CommandCore
                .Subscribe(
                    e =>
                    {
                        var tmpv = (-decimal.Parse(CurrentInput)).ToString();

                        if (State == CalculatorState.Finished)
                        {
                            ResetStatus();
                        }
                        CurrentInput = tmpv.Contains(".") ? tmpv : tmpv + '.';
                    }
                )
                .DisposeWith(this);
        }
        public Calculator_Model()
        {

        }

        private void ResetStatus()
        {
            ResetPropertyValue(_State);
            ResetPropertyValue(_CurrentInput);
            ResetPropertyValue(_RightNumber);
            ResetPropertyValue(_Sign);
            ResetPropertyValue(_IsPointed);
            ResetPropertyValue(_Result);
        }




        public bool IsPointed
        {
            get { return _IsPointedLocator(this).Value; }
            set { _IsPointedLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsPointed Setup
        protected Property<bool> _IsPointed = new Property<bool> { LocatorFunc = _IsPointedLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsPointedLocator = RegisterContainerLocator<bool>("IsPointed", model => model.Initialize("IsPointed", ref model._IsPointed, ref _IsPointedLocator, _IsPointedDefaultValueFactory));
        static Func<bool> _IsPointedDefaultValueFactory = () => false;
        #endregion


        public String[] ButtonTexts
        {
            get { return _ButtonTextsLocator(this).Value; }
            set { _ButtonTextsLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String[] ButtonTexts Setup
        protected Property<String[]> _ButtonTexts = new Property<String[]> { LocatorFunc = _ButtonTextsLocator };
        static Func<BindableBase, ValueContainer<String[]>> _ButtonTextsLocator = RegisterContainerLocator<String[]>("ButtonTexts", model => model.Initialize("ButtonTexts", ref model._ButtonTexts, ref _ButtonTextsLocator, _ButtonTextsDefaultValueFactory));
        static Func<String[]> _ButtonTextsDefaultValueFactory =
            () => new string[]
            {
               "0",
               "1",
               "2",
               "3",
               "4",
               "5",
               "6",
               "7",
               "8",
               "9",
               "+",
               "-",
               "*",
               "/",
               "=",
               "C",
               ".",
               "Back",
               "+/-"
            };
        #endregion





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
        static Func<String> _CurrentInputDefaultValueFactory = () => "0.";
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
        static Func<string> _LeftNumberDefaultValueFactory = () => "?";
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
        static Func<string> _RightNumberDefaultValueFactory = () => "0";
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

        public CommandModel<ReactiveCommand, String> CommandCalculate
        {
            get { return _CommandCalculateLocator(this).Value; }
            set { _CommandCalculateLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandCalculate Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandCalculate = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandCalculateLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandCalculateLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandCalculate", model => model.Initialize("CommandCalculate", ref model._CommandCalculate, ref _CommandCalculateLocator, _CommandCalculateDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandCalculateDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //cmd.Subscribe (_=>{ } ).RegisterDisposeToViewModel(model); //Config it if needed
                return cmd.CreateCommandModel("Calculate");
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
                var text = "CE";
                var commandId = "CommandClear";
                var vm = CastToCurrentType(model); 
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                cmd
                    .DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            //Todo: Add command logic here. or:
                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                        }
                    )
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);
                return cmd.CreateCommandModel(text);
            };
        #endregion


        /// <summary>
        /// 当前状态
        /// </summary>
        public CalculatorState State
        {
            get { return _StateLocator(this).Value; }
            set { _StateLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CaculatorStatus State Setup
        protected Property<CalculatorState> _State = new Property<CalculatorState> { LocatorFunc = _StateLocator };
        static Func<BindableBase, ValueContainer<CalculatorState>> _StateLocator = RegisterContainerLocator<CalculatorState>("State", model => model.Initialize("State", ref model._State, ref _StateLocator, _StateDefaultValueFactory));
        static Func<CalculatorState> _StateDefaultValueFactory = () => CalculatorState.InputingLeft;
        #endregion


        public CommandModel<ReactiveCommand, String> CommandBackspace
        {
            get { return _CommandBackspaceLocator(this).Value; }
            set { _CommandBackspaceLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandBackspace Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandBackspace = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandBackspaceLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandBackspaceLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandBackspace", model => model.Initialize("CommandBackspace", ref model._CommandBackspace, ref _CommandBackspaceLocator, _CommandBackspaceDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandBackspaceDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //cmd.Subscribe (_=>{ } ).DisposeWith(model); //Config it if needed
                return cmd.CreateCommandModel("Backspace");
            };
        #endregion



        public CommandModel<ReactiveCommand, String> CommandChangeSignOfCurrentInput
        {
            get { return _CommandChangeSignOfCurrentInputLocator(this).Value; }
            set { _CommandChangeSignOfCurrentInputLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandChangeSignOfCurrentInput Setup
        protected Property<CommandModel<ReactiveCommand, String>> _CommandChangeSignOfCurrentInput = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandChangeSignOfCurrentInputLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandChangeSignOfCurrentInputLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandChangeSignOfCurrentInput", model => model.Initialize("CommandChangeSignOfCurrentInput", ref model._CommandChangeSignOfCurrentInput, ref _CommandChangeSignOfCurrentInputLocator, _CommandChangeSignOfCurrentInputDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandChangeSignOfCurrentInputDefaultValueFactory =
            model =>
            {
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core
                //cmd.Subscribe (_=>{ } ).DisposeWith(model); //Config it if needed
                return cmd.CreateCommandModel("ChangeSignOfCurrentInput");
            };
        #endregion

    }

    /// <summary>
    /// 计算器的状态
    /// </summary>
    public enum CalculatorState
    {
        /// <summary>
        /// 输入左数字
        /// </summary>
        InputingLeft,

        /// <summary>
        /// 输入过符号，下次输入数字会清空输入栏进入新值
        /// </summary>
        InputedSign,

        /// <summary>
        /// 输入右数字
        /// </summary>
        InputingRight,

        /// <summary>
        /// 计算中
        /// </summary>
        Calculating,
        /// <summary>
        /// 计算完毕 显示左右和结果
        /// </summary>
        Finished
    }


    //public enum CalculatorKey
    //{
    //    Key_0,
    //    Key_1,
    //    Key_2,
    //    Key_3,
    //    Key_4,
    //    Key_5,
    //    Key_6,
    //    Key_7,
    //    Key_8,
    //    Key_9,
    //    Key_Add,
    //    Key_Min,
    //    Key_Mtp,
    //    Key_Div,
    //    Key_Cal,
    //    Key_Esc
    //}
    //0,
    //1,
    //2,
    //3,
    //4,
    //5,
    //6,
    //7,
    //8,
    //9,
    //+,
    //-,
    //*,
    ///,
    //=,
    //C




    //[DataContract(IsReference=true) ] //if you want
    public class SomeBindable : BindableBase<SomeBindable>
    {
        public SomeBindable()
        {
            // Use propery to init value here:
            if (IsInDesignMode)
            {
                //Add design time demo data init here. These will not execute in runtime.
            }


        }

        //Use propvm + tab +tab  to create a new property of bindable here:


    }





}

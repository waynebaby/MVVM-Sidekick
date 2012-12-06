using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MVVMSidekick.Reactive;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Diagnostics;
using MVVMSidekick.Commands;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Input;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MVVMSidekick.Controls
{
    /// <summary>
    /// 一个通过扩展属性附加到界面上 且与Textbox绑定的数字编辑器
    /// </summary>
    public sealed partial class CalcNumberPad : UserControl
    {
        public CalcNumberPad()
        {
            this.InitializeComponent();
            //ViewModel.GetValueContainer<Visibility>("Visibility")
            //    .GetValueChangeObservable()
            //    .Subscribe(x =>
            //        Visibility = x.EventArgs);
            //  ViewModel.InputFinished += ViewModel_InputFinished;
        }



        public CalcNumberPad_Model ViewModel
        {
            get
            {
                return _InputPanel.DataContext as CalcNumberPad_Model;
            }
            set
            {
                _InputPanel.DataContext = value;
            }
        }
        public ICommand ShowInputCommand
        {
            get { return ViewModel.ShowInputCommand; }

        }






        public static CalcNumberPad GetCalcNumberPad(DependencyObject obj)
        {
            return (CalcNumberPad)obj.GetValue(CalcNumberPadProperty);
        }

        public static void SetCalcNumberPad(DependencyObject obj, CalcNumberPad value)
        {
            obj.SetValue(CalcNumberPadProperty, value);
        }

        // Using a DependencyProperty as the backing store for CalcNumberPad.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CalcNumberPadProperty =
            DependencyProperty.RegisterAttached("CalcNumberPad", typeof(CalcNumberPad), typeof(CalcNumberPad), new PropertyMetadata(null,
                 (o, e) =>
                 {
                     var itm = o as Panel;
                     if (itm != null)
                     {
                         var np = e.NewValue as CalcNumberPad;
                         itm.Children.Add(np);
                     }
                 }));








        public static double GetMinValue(DependencyObject obj)
        {
            return (double)obj.GetValue(MinValueProperty);
        }

        public static void SetMinValue(DependencyObject obj, double value)
        {
            obj.SetValue(MinValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for MinValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.RegisterAttached("MinValue", typeof(double), typeof(CalcNumberPad), new PropertyMetadata(double.MinValue));




        public static double GetMaxValue(DependencyObject obj)
        {
            return (double)obj.GetValue(MaxValueProperty); ;
        }

        public static void SetMaxValue(DependencyObject obj, double value)
        {
            obj.SetValue(MaxValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.RegisterAttached("MaxValue", typeof(double), typeof(CalcNumberPad), new PropertyMetadata(double.MaxValue));







        public static string GetFinalResult(DependencyObject obj)
        {
            return (string)obj.GetValue(FinalResultProperty);
        }

        public static void SetFinalResult(DependencyObject obj, string value)
        {
            obj.SetValue(FinalResultProperty, value);
        }

        // Using a DependencyProperty as the backing store for FinalResult.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FinalResultProperty =
            DependencyProperty.RegisterAttached("FinalResult", typeof(string), typeof(CalcNumberPad), new PropertyMetadata(""));




        public static bool GetHasLimitation(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasLimitationProperty);
        }

        public static void SetHasLimitation(DependencyObject obj, bool value)
        {
            obj.SetValue(HasLimitationProperty, value);
        }

        // Using a DependencyProperty as the backing store for HasLimitation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasLimitationProperty =
            DependencyProperty.RegisterAttached("HasLimitation", typeof(bool), typeof(CalcNumberPad), new PropertyMetadata(false));



        public static string GetEventName(DependencyObject obj)
        {
            return (string)obj.GetValue(EventNameProperty);
        }

        public static void SetEventName(DependencyObject obj, string value)
        {
            obj.SetValue(EventNameProperty, value);
        }

        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.RegisterAttached("EventName", typeof(string), typeof(CalcNumberPad), new PropertyMetadata(null,
                   (o, e) =>
                   {
                       var tb = o as FrameworkElement;


                       var cache = tb.GetType().GetOrCreateEventCache();
                       EventInfo ei;
                       if (cache.TryGetValue(e.NewValue.ToString(), out  ei))
                       {

                           tb.AddEventHandlerByType(ei.EventHandlerType, ei, (a, b) => CalcNumberPad_Model.ShowCommand(tb));


                           //if (ei.EventHandlerType == typeof(RoutedEventHandler))
                           //{
                           //    tb.AddEventHandler<RoutedEventHandler, RoutedEventArgs>(ei, (a, b) => 
                           //        CalcNumberPad_Model.ShowCommand(tb));
                           //}
                           //else if (ei.EventHandlerType == typeof(TappedEventHandler))
                           //{
                           //    tb.AddEventHandler<TappedEventHandler, TappedEventArgs>(ei, (a, b) => 
                           //        CalcNumberPad_Model.ShowCommand(tb));
                           //}
                       }



                   }

                ));





    }



    public class CalcNumberPad_Model : ViewModelBase<CalcNumberPad_Model>
    {
        static readonly string DefaultValue = "0";
        public CalcNumberPad_Model()
        {
            ConfigProperties();
            ConfigCommands();
        }
        internal Action ClosePad = () => { };
        internal Action<string> FillValue = s => { };
        //bool IsActiveActualInputCharsCollectionChangedObservable = true;
        void ConfigProperties()
        {
            //每次有内容修改
            var obsCol = this.ActualInputChars.GetCollectionChangedObservable (this);//.Where(_ => IsActiveActualInputCharsCollectionChangedObservable);

            obsCol
                .Do //进行验证并且把结果输出到 ShowString属性
                (
                     e =>
                     {
                         try
                         {


                             //IsActiveActualInputCharsCollectionChangedObservable = false;
                             //TODO:验证逻辑
                             var str = new string(ActualInputChars.ToArray());
                             double v;
                             if (!double.TryParse(str, out v))
                             {
                                 if (this.ActualInputChars.Count > 0)
                                 {
                                     this.ActualInputChars.RemoveAt(this.ActualInputChars.Count - 1);
                                 }
                                 else
                                 {
                                     str = DefaultValue;
                                 }
                             }
                             if (HasLimitation)
                             {
                                 if (MinValue > MaxValue)
                                 {
                                     throw new InvalidOperationException("Max must bigger or equal than min");
                                 }
                                 if (v > MaxValue)
                                 {
                                     v = MaxValue;
                                 }
                                 if (v < MinValue)
                                 {
                                     v = MinValue;
                                 }
                             }


                             str = v.ToString("#.##");
                             str = str.TrimStart('0');
                             if (string.IsNullOrEmpty(str))
                             {
                                 str = DefaultValue;
                             }
                             if (str.StartsWith("."))
                             {
                                 str = "0" + str;
                             }
                             //验证后显示

                             GetValueContainer(x => x.ShowString).SetValueAndTryNotify(str);

                         }
                         catch (Exception)
                         {

                             throw;
                         }
                         finally
                         {
                             //IsActiveActualInputCharsCollectionChangedObservable = true;
                         }
                     }
                )
                .Subscribe()
                .DisposeWith(this);


            this.GetValueContainer(x => x.HasLimitation).GetValueChangedObservableWithoutArgs()
                .Merge(this.GetValueContainer(x => x.MaxValue).GetValueChangedObservableWithoutArgs())
                .Merge(this.GetValueContainer(x => x.MinValue).GetValueChangedObservableWithoutArgs())
                .Subscribe(
                    _ =>
                    {
                        if (!this.HasLimitation)
                        {
                            this.MaxValueShowString = "";
                        }
                        else
                        {
                            this.MaxValueShowString = this.MaxValue.ToString("/#.##");
                        }
                    }

                );

        }

        void ConfigCommands()
        {

            #region 所有输入按钮

            var btnClick = this.ButtonPushCommand.CommandCore
                .Select(e => e.EventArgs.Parameter as string)
                .Where(s => s != null);

            btnClick.Where(x => x.Length == 1)
                .Subscribe
                (
                    input =>
                    {
                        ActualInputChars.Add(input[0]);
                    }

                ).DisposeWith(this);

            btnClick.Where(x => x.Length != 1)
                .Subscribe
                (
                    input =>
                    {
                        switch (input)
                        {

                            case "Back":
                                if (ActualInputChars.Count > 0)
                                {
                                    ActualInputChars.RemoveAt(ActualInputChars.Count - 1);
                                }
                                break;
                            case "Cancel":

                                this.ClosePad();
                                break;

                            case "Clear":
                                this.ActualInputChars.Clear();
                                foreach (var c in DefaultValue)
                                {
                                    ActualInputChars.Add(c);
                                }
                                break;
                            case "Enter":


                                FillValue(ShowString);
                                this.ClosePad();
                                break;
                            default:
                                break;
                        }
                    }

                ).DisposeWith(this);
            #endregion

            #region 绑定到 Textbox上的命令
            this.ShowInputCommand.CommandCore
                .Subscribe(
                  e =>
                  {

                      var fel = e.EventArgs.Parameter as FrameworkElement;

                      ShowCommand(fel as TextBox);

                  }
                ).DisposeWith(this);
            #endregion
        }

        public static void ShowCommand(FrameworkElement eventSource)
        {
            CalcNumberPad calc = null;
            DependencyObject elem = eventSource;
            //定位上层最近的一个CalcNumberPad定义的对象
            while (elem != null)
            {
                calc = elem.GetValue(CalcNumberPad.CalcNumberPadProperty) as CalcNumberPad;
                if (calc != null)
                {
                    break;

                }

                elem = VisualTreeHelper.GetParent(elem);

            }

            //找到的话 显示该对象
            if (calc != null)
            {
                var vm = calc.ViewModel;
                if (vm == null)
                {
                    calc.ViewModel = vm = new CalcNumberPad_Model();
                }

                vm.ClosePad = () =>
                   calc.Visibility = Visibility.Collapsed;
                vm.FillValue = s => CalcNumberPad.SetFinalResult(eventSource, s);
                vm.AddDisposeAction(() =>
                    {
                        vm.ClosePad = null;
                        vm.FillValue = null;
                    });

                vm.MaxValue = CalcNumberPad.GetMaxValue(eventSource);
                vm.MinValue = CalcNumberPad.GetMinValue(eventSource);
                vm.HasLimitation = CalcNumberPad.GetHasLimitation(eventSource);
                string val = CalcNumberPad.GetFinalResult(eventSource);


                calc.ViewModel.ActualInputChars.Clear();
                foreach (var item in val)
                {
                    calc.ViewModel.ActualInputChars.Add(item);
                }

                calc.Visibility = Visibility.Visible;
            }
        }

        // bool _raiseCollectionChangedEvent = true;

        public String ShowString
        {
            get { return _ShowStringLocator(this).Value; }
            set
            {
                //   _raiseCollectionChangedEvent = false;
                _ShowStringLocator(this).Value = value;
                FillActualInput(value);

                //  _raiseCollectionChangedEvent = true;
            }
        }

        private void FillActualInput(string value)
        {
            try
            {
                //IsActiveActualInputCharsCollectionChangedObservable = false;

                ActualInputChars.Clear();
                foreach (var c in value)
                {
                    ActualInputChars.Add(c);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }//IsActiveActualInputCharsCollectionChangedObservable = true; }
        }



        //public string FinalResult
        //{
        //    get { return _FinalResultLocator(this).Value; }
        //    set { _FinalResultLocator(this).SetValueAndTryNotify(value); }
        //}
        //#region Property string  FinalResult Setup
        //protected Property<string> _FinalResult = new Property<string> { LocatorFunc = _FinalResultLocator };
        //static Func<BindableBase, ValueContainer<string>> _FinalResultLocator = RegisterContainerLocator<string>("FinalResult", model => model.Initialize("FinalResult", ref model._FinalResult, ref _FinalResultLocator, _FinalResultDefaultValueFactory));
        //static Func<string> _FinalResultDefaultValueFactory = () => string.Empty;
        //#endregion



        #region Property String ShowString Setup
        protected Property<String> _ShowString =
          new Property<String> { LocatorFunc = _ShowStringLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<String>> _ShowStringLocator =
            RegisterContainerLocator<String>(
                "ShowString",
                model =>
                {
                    model._ShowString =
                        model._ShowString
                        ??
                        new Property<String> { LocatorFunc = _ShowStringLocator };
                    return model._ShowString.Container =
                        model._ShowString.Container
                        ??
                        new ValueContainer<String>("ShowString", model, DefaultValue);
                });
        #endregion






        public double MaxValue
        {
            get { return _MaxValueLocator(this).Value; }
            set { _MaxValueLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double MaxValue Setup
        protected Property<double> _MaxValue = new Property<double> { LocatorFunc = _MaxValueLocator };
        static Func<BindableBase, ValueContainer<double>> _MaxValueLocator = RegisterContainerLocator<double>("MaxValue", model => model.Initialize("MaxValue", ref model._MaxValue, ref _MaxValueLocator, _MaxValueDefaultValueFactory));
        static Func<double> _MaxValueDefaultValueFactory = () => 10000000;
        #endregion


        public string MaxValueShowString
        {
            get { return _MaxValueShowStringLocator(this).Value; }
            set { _MaxValueShowStringLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string MaxValueShowString Setup
        protected Property<string> _MaxValueShowString = new Property<string> { LocatorFunc = _MaxValueShowStringLocator };
        static Func<BindableBase, ValueContainer<string>> _MaxValueShowStringLocator = RegisterContainerLocator<string>("MaxValueShowString", model => model.Initialize("MaxValueShowString", ref model._MaxValueShowString, ref _MaxValueShowStringLocator, _MaxValueShowStringDefaultValueFactory));
        static Func<string> _MaxValueShowStringDefaultValueFactory = null;
        #endregion




        public double MinValue
        {
            get { return _MinValueLocator(this).Value; }
            set { _MinValueLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property double  MinValue Setup
        protected Property<double> _MinValue = new Property<double> { LocatorFunc = _MinValueLocator };
        static Func<BindableBase, ValueContainer<double>> _MinValueLocator = RegisterContainerLocator<double>("MinValue", model => model.Initialize("MinValue", ref model._MinValue, ref _MinValueLocator, _MinValueDefaultValueFactory));
        static Func<double> _MinValueDefaultValueFactory = () => 0;
        #endregion


        public bool HasLimitation
        {
            get { return _HasLimitationLocator(this).Value; }
            set { _HasLimitationLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool HasLimitation Setup
        protected Property<bool> _HasLimitation = new Property<bool> { LocatorFunc = _HasLimitationLocator };
        static Func<BindableBase, ValueContainer<bool>> _HasLimitationLocator = RegisterContainerLocator<bool>("HasLimitation", model => model.Initialize("HasLimitation", ref model._HasLimitation, ref _HasLimitationLocator, _HasLimitationDefaultValueFactory));
        static Func<bool> _HasLimitationDefaultValueFactory = null;
        #endregion



        public ObservableCollection<Char> ActualInputChars
        {
            get { return _ActualInputCharsLocator(this).Value; }

        }

        #region Property  ObservableCollection<Char> ActualInputChars Setup
        protected Property<ObservableCollection<Char>> _ActualInputChars =
          new Property<ObservableCollection<Char>> { LocatorFunc = _ActualInputCharsLocator };
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        static Func<BindableBase, ValueContainer<ObservableCollection<Char>>> _ActualInputCharsLocator =
            RegisterContainerLocator<ObservableCollection<Char>>(
                "ActualInputChars",
                model =>
                {
                    model._ActualInputChars =
                        model._ActualInputChars
                        ??
                        new Property<ObservableCollection<Char>> { LocatorFunc = _ActualInputCharsLocator };
                    return model._ActualInputChars.Container =
                        model._ActualInputChars.Container
                        ??
                        new ValueContainer<ObservableCollection<Char>>("ActualInputChars", model, new ObservableCollection<char>(DefaultValue.ToCharArray()));
                });
        #endregion







        /// <summary>
        /// 任意的Button Push，按钮的CommandName用 Parameter传递
        /// </summary>
        public ICommandModel<ReactiveCommand, String> ButtonPushCommand
        {
            get { return _ButtonPushCommand.WithViewModel(this); }
            protected set { _ButtonPushCommand = (CommandModel<ReactiveCommand, String>)value; }
        }

        #region ButtonPushCommand Configuration
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        CommandModel<ReactiveCommand, String> _ButtonPushCommand
            = new ReactiveCommand(canExecute: true).CreateCommandModel(default(String));
        #endregion





        public ICommandModel<ReactiveCommand, String> ShowInputCommand
        {
            get { return _ShowInputCommand.WithViewModel(this); }
            protected set { _ShowInputCommand = (CommandModel<ReactiveCommand, String>)value; }
        }

        #region ShowInputCommand Configuration
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        CommandModel<ReactiveCommand, String> _ShowInputCommand
            = new ReactiveCommand(canExecute: true).CreateCommandModel(default(String));
        #endregion



    }
}

using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using MVVMSidekick.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MVVMSidekickUWPDemo.ViewModels
{

    [DataContract]
    public class FetchData_Model : ViewModel<FetchData_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性

        public FetchData_Model()
        {
            if (IsInDesignMode)
            {
                Forecasts = new ObservableCollection<WeatherForecast>()
                {
                    new WeatherForecast
                    {
                        Date                =  DateTime.Parse(          "2018-05-08"),
                        TemperatureC        =            -13,
                        Summary             =    "Freezing"
                    },
                    new WeatherForecast  {
                        Date                =   DateTime.Parse( "2018-05-09"),
                        TemperatureC        =     -16,
                        Summary             =         "Balmy"
                    },
                        new WeatherForecast   {
                        Date                =   DateTime.Parse(   "2018-05-10"),
                        TemperatureC        =       -2,
                        Summary             =      "Chilly"
                    }
                };
            }

        }
        public FetchData_Model(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        protected IServiceProvider ServiceProvider { get; }


        public string Title { get => _TitleLocator(this).Value; set => _TitleLocator(this).SetValueAndTryNotify(value); }
        #region Property string Title Setup        
        protected Property<string> _Title = new Property<string>(_TitleLocator);
        static Func<BindableBase, ValueContainer<string>> _TitleLocator = RegisterContainerLocator(nameof(Title), m => m.Initialize(nameof(Title), ref m._Title, ref _TitleLocator, () => nameof(FetchData_Model)));
        #endregion


        public ObservableCollection<WeatherForecast> Forecasts { get => _ForecastsLocator(this).Value; set => _ForecastsLocator(this).SetValueAndTryNotify(value); }
        #region Property ObservableCollection<WeatherForecast>  Forecasts Setup        
        protected Property<ObservableCollection<WeatherForecast>> _Forecasts = new Property<ObservableCollection<WeatherForecast>>(_ForecastsLocator);
        static Func<BindableBase, ValueContainer<ObservableCollection<WeatherForecast>>> _ForecastsLocator = RegisterContainerLocator(nameof(Forecasts), m => m.Initialize(nameof(Forecasts), ref m._Forecasts, ref _ForecastsLocator, () => default(ObservableCollection<WeatherForecast>)));
        #endregion

        protected override async Task OnBindedViewLoad(IView view)
        {

            Forecasts = new ObservableCollection<WeatherForecast>()
            {
                new WeatherForecast ()
                {
                    Date                = DateTime.Parse( "2018-05-06"),
                    TemperatureC        = 1,
                    Summary             = "Freezing"
                },
                  new WeatherForecast  {
                    Date                = DateTime.Parse(  "2018-05-07"),
                    TemperatureC        =    14,
                    Summary             =     "Bracing"
                  },
                  new WeatherForecast  {
                    Date                =  DateTime.Parse(          "2018-05-08"),
                    TemperatureC        =            -13,
                    Summary             =    "Freezing"
                  },
                  new WeatherForecast  {
                    Date                =   DateTime.Parse( "2018-05-09"),
                    TemperatureC        =     -16,
                    Summary             =         "Balmy"
                  },
                 new WeatherForecast   {
                    Date                =   DateTime.Parse(   "2018-05-10"),
                    TemperatureC        =       -2,
                    Summary             =      "Chilly"
                  } };


            await base.OnBindedViewLoad(view);
        }


        #region Life Time Event Handling

        #region OnBindedToView
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
        #endregion

        #region OnUnbindedFromView
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
        #endregion

        #region OnBindedViewLoad

        ///// <summary>
        ///// This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Load event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewLoad(view);
        //}
        #endregion

        #region OnBindedViewUnload

        ///// <summary>
        ///// This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's  ViewModel property
        ///// </summary>
        ///// <param name="view">View that firing Unload event</param>
        ///// <returns>Task awaiter</returns>
        //protected override Task OnBindedViewUnload(MVVMSidekick.Views.IView view)
        //{
        //    return base.OnBindedViewUnload(view);
        //}
        #endregion

        #region OnDisposeExceptions

        ///// <summary>
        ///// <para>If dispose actions got exceptions, will handled here. </para>
        ///// </summary>
        ///// <param name="exceptions">
        ///// <para>The exception and dispose information</para>
        ///// </param>
        //protected override async void OnDisposeExceptions(IList<DisposeEntry> exceptions)
        //{
        //    base.OnDisposeExceptions(exceptions);
        //    await Task.Yield();
        //}
        #endregion

        #endregion



    }


    public class WeatherForecast : BindableBase<WeatherForecast>
    {
        public WeatherForecast()
        {
            this.ListenValueChangedEvents(_ => _.TemperatureC)
                 .Subscribe(_ => TemperatureF = 32 + (int)(TemperatureC / 0.5556))
                 .DisposeWith(this);
        }
        public DateTime Date { get => _DateLocator(this).Value; set => _DateLocator(this).SetValueAndTryNotify(value); }
        #region Property DateTime Date Setup        
        protected Property<DateTime> _Date = new Property<DateTime>(_DateLocator);
        static Func<BindableBase, ValueContainer<DateTime>> _DateLocator = RegisterContainerLocator(nameof(Date), m => m.Initialize(nameof(Date), ref m._Date, ref _DateLocator, () => default(DateTime)));
        #endregion


        public int TemperatureC { get => _TemperatureCLocator(this).Value; set => _TemperatureCLocator(this).SetValueAndTryNotify(value); }
        #region Property int TemperatureC Setup        
        protected Property<int> _TemperatureC = new Property<int>(_TemperatureCLocator);
        static Func<BindableBase, ValueContainer<int>> _TemperatureCLocator = RegisterContainerLocator(nameof(TemperatureC), m => m.Initialize(nameof(TemperatureC), ref m._TemperatureC, ref _TemperatureCLocator, () => default(int)));
        #endregion



        public string Summary { get => _SummaryLocator(this).Value; set => _SummaryLocator(this).SetValueAndTryNotify(value); }
        #region Property string Summary Setup        
        protected Property<string> _Summary = new Property<string>(_SummaryLocator);
        static Func<BindableBase, ValueContainer<string>> _SummaryLocator = RegisterContainerLocator(nameof(Summary), m => m.Initialize(nameof(Summary), ref m._Summary, ref _SummaryLocator, () => default(string)));
        #endregion



        public int TemperatureF { get => _TemperatureFLocator(this).Value; private set { _TemperatureFLocator(this).Value = value; } }
        #region Property int TemperatureF Setup        
        protected Property<int> _TemperatureF = new Property<int>(_TemperatureFLocator);
        static Func<BindableBase, ValueContainer<int>> _TemperatureFLocator = RegisterContainerLocator(nameof(TemperatureF), m => m.Initialize(nameof(TemperatureF), ref m._TemperatureF, ref _TemperatureFLocator, () => default(int)));
        #endregion

    }
}


using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick;
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
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace MVVMSidekickBlazorDemo.Pages.ViewModels
{


    public class FetchData_ViewModel : ViewModel<FetchData_ViewModel, FetchData>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。

        public FetchData_ViewModel(IServiceProvider serviceProvider,HttpClient http) : base(serviceProvider)
        {
            this.http = http;
        }
        private readonly HttpClient http;

        public string Title { get => _TitleLocator(this).Value; set => _TitleLocator(this).SetValueAndTryNotify(value); }
        #region Property string Title Setup        
        protected Property<string> _Title = new Property<string>(_TitleLocator);
        static Func<BindableBase, ValueContainer<string>> _TitleLocator = RegisterContainerLocator(nameof(Title), m => m.Initialize(nameof(Title), ref m._Title, ref _TitleLocator, () => default(string)));
        #endregion



        public ObservableCollection<WeatherForecast> Forecasts { get => _ForecastsLocator(this).Value; set => _ForecastsLocator(this).SetValueAndTryNotify(value); }
        #region Property ObservableCollection<WeatherForecast>  Forecasts Setup        
        protected Property<ObservableCollection<WeatherForecast> > _Forecasts = new Property<ObservableCollection<WeatherForecast> >(_ForecastsLocator);
        static Func<BindableBase, ValueContainer<ObservableCollection<WeatherForecast> >> _ForecastsLocator = RegisterContainerLocator(nameof(Forecasts), m => m.Initialize(nameof(Forecasts), ref m._Forecasts, ref _ForecastsLocator, () => default(ObservableCollection<WeatherForecast> )));
  
        #endregion

        public  async override Task OnInitializedAsync()
        {
            Forecasts =  new ObservableCollection<WeatherForecast> ( await http.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json"));
        }
    }

    public class WeatherForecast :BindableBase<WeatherForecast>
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



        public int TemperatureF { get => _TemperatureFLocator(this).Value;  private set { _TemperatureFLocator(this).Value =value; } }
        #region Property int TemperatureF Setup        
        protected Property<int> _TemperatureF = new Property<int>(_TemperatureFLocator);
        static Func<BindableBase, ValueContainer<int>> _TemperatureFLocator = RegisterContainerLocator(nameof(TemperatureF), m => m.Initialize(nameof(TemperatureF), ref m._TemperatureF, ref _TemperatureFLocator, () => default(int)));
        #endregion

    }
    #region ViewModelRegistry
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {

        internal static Action<MVVMSidekickOptions> FetchDataConfigEntry = AddConfigure(opt => opt.RegisterViewModel<FetchData_ViewModel>());
    }
    #endregion 
}


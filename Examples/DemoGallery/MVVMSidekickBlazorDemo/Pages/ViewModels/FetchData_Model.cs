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

    /// <summary>
    /// 获取数据页面的视图模型
    /// View model for the FetchData page
    /// </summary>
    public class FetchData_Model : ViewModel<FetchData_Model, FetchData>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。

        /// <summary>
        /// 初始化获取数据视图模型的新实例
        /// Initializes a new instance of the FetchData view model
        /// </summary>
        /// <param name="serviceProvider">服务提供者 / Service provider</param>
        /// <param name="http">HTTP客户端 / HTTP client</param>
        public FetchData_Model(IServiceProvider serviceProvider,HttpClient http) : base(serviceProvider)
        {
            this.http = http;
        }
        
        /// <summary>
        /// HTTP客户端实例
        /// HTTP client instance
        /// </summary>
        private readonly HttpClient http;

        /// <summary>
        /// 获取或设置页面标题
        /// Gets or sets the page title
        /// </summary>
        public string Title { get => _TitleLocator(this).Value; set => _TitleLocator(this).SetValueAndTryNotify(value); }
        #region Property string Title Setup        
        protected Property<string> _Title = new Property<string>(_TitleLocator);
        static Func<BindableBase, ValueContainer<string>> _TitleLocator = RegisterContainerLocator(nameof(Title), m => m.Initialize(nameof(Title), ref m._Title, ref _TitleLocator, () => default(string)));
        #endregion

        /// <summary>
        /// 获取或设置天气预报集合
        /// Gets or sets the weather forecast collection
        /// </summary>
        public ObservableCollection<WeatherForecast> Forecasts { get => _ForecastsLocator(this).Value; set => _ForecastsLocator(this).SetValueAndTryNotify(value); }
        #region Property ObservableCollection<WeatherForecast>  Forecasts Setup        
        protected Property<ObservableCollection<WeatherForecast> > _Forecasts = new Property<ObservableCollection<WeatherForecast> >(_ForecastsLocator);
        static Func<BindableBase, ValueContainer<ObservableCollection<WeatherForecast> >> _ForecastsLocator = RegisterContainerLocator(nameof(Forecasts), m => m.Initialize(nameof(Forecasts), ref m._Forecasts, ref _ForecastsLocator, () => default(ObservableCollection<WeatherForecast> )));
  
        #endregion

        /// <summary>
        /// 异步初始化方法，加载天气预报数据
        /// Asynchronous initialization method that loads weather forecast data
        /// </summary>
        /// <returns>异步任务 / Asynchronous task</returns>
        public  async override Task OnInitializedAsync()
        {
            Forecasts =  new ObservableCollection<WeatherForecast> ( await http.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json"));
        }
    }

    /// <summary>
    /// 天气预报数据模型
    /// Weather forecast data model
    /// </summary>
    public class WeatherForecast :BindableBase<WeatherForecast>
    {
        /// <summary>
        /// 初始化天气预报的新实例，设置温度转换逻辑
        /// Initializes a new instance of weather forecast with temperature conversion logic
        /// </summary>
        public WeatherForecast()
        {
            this.ListenValueChangedEvents(_ => _.TemperatureC)
                 .Subscribe(_ => TemperatureF = 32 + (int)(TemperatureC / 0.5556))
                 .DisposeWith(this);
        }
        
        /// <summary>
        /// 获取或设置日期
        /// Gets or sets the date
        /// </summary>
        public DateTime Date { get => _DateLocator(this).Value; set => _DateLocator(this).SetValueAndTryNotify(value); }
        #region Property DateTime Date Setup        
        protected Property<DateTime> _Date = new Property<DateTime>(_DateLocator);
        static Func<BindableBase, ValueContainer<DateTime>> _DateLocator = RegisterContainerLocator(nameof(Date), m => m.Initialize(nameof(Date), ref m._Date, ref _DateLocator, () => default(DateTime)));
        #endregion

        /// <summary>
        /// 获取或设置摄氏温度
        /// Gets or sets the temperature in Celsius
        /// </summary>
        public int TemperatureC { get => _TemperatureCLocator(this).Value; set => _TemperatureCLocator(this).SetValueAndTryNotify(value); }
        #region Property int TemperatureC Setup        
        protected Property<int> _TemperatureC = new Property<int>(_TemperatureCLocator);
        static Func<BindableBase, ValueContainer<int>> _TemperatureCLocator = RegisterContainerLocator(nameof(TemperatureC), m => m.Initialize(nameof(TemperatureC), ref m._TemperatureC, ref _TemperatureCLocator, () => default(int)));
        #endregion

        /// <summary>
        /// 获取或设置天气摘要
        /// Gets or sets the weather summary
        /// </summary>
        public string Summary { get => _SummaryLocator(this).Value; set => _SummaryLocator(this).SetValueAndTryNotify(value); }
        #region Property string Summary Setup        
        protected Property<string> _Summary = new Property<string>(_SummaryLocator);
        static Func<BindableBase, ValueContainer<string>> _SummaryLocator = RegisterContainerLocator(nameof(Summary), m => m.Initialize(nameof(Summary), ref m._Summary, ref _SummaryLocator, () => default(string)));
        #endregion

        /// <summary>
        /// 获取华氏温度（自动从摄氏温度计算）
        /// Gets the temperature in Fahrenheit (automatically calculated from Celsius)
        /// </summary>
        public int TemperatureF { get => _TemperatureFLocator(this).Value;  private set { _TemperatureFLocator(this).Value =value; } }
        #region Property int TemperatureF Setup        
        protected Property<int> _TemperatureF = new Property<int>(_TemperatureFLocator);
        static Func<BindableBase, ValueContainer<int>> _TemperatureFLocator = RegisterContainerLocator(nameof(TemperatureF), m => m.Initialize(nameof(TemperatureF), ref m._TemperatureF, ref _TemperatureFLocator, () => default(int)));
        #endregion

    }
    #region ViewModelRegistry
    /// <summary>
    /// 视图模型注册表的内部分部类
    /// Internal partial class for view model registry
    /// </summary>
    internal partial class ViewModelRegistry : MVVMSidekickStartupBase
    {
        /// <summary>
        /// 获取数据配置条目的静态操作
        /// Static action for FetchData configuration entry
        /// </summary>
        internal static Action<MVVMSidekickOptions> FetchDataConfigEntry = AddConfigure(opt => opt.RegisterViewModel<FetchData_Model>());
    }
    #endregion 
}


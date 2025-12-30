using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVVMSidekickBlazorDemo.Pages.ViewModels;

namespace MVVMSidekickBlazorDemo
{
    /// <summary>
    /// MVVM Sidekick Blazor 演示程序的主类
    /// Main class for the MVVM Sidekick Blazor demo program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 应用程序的主入口点
        /// The main entry point for the application
        /// </summary>
        /// <param name="args">命令行参数 / Command line arguments</param>
        /// <returns>异步任务 / Asynchronous task</returns>
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddMVVMSidekick(new ViewModelRegistry());
         
            await builder.Build().PushToMVVMSidekickRoot().RunAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Services;
using UWPDemo;
using UWPDemo.ViewModels;

using System.Threading;


	namespace MVVMSidekick.Startups
	{
		internal partial class ViewModelRegistry : MVVMSidekickStartupBase
		{
			internal  Action<MVVMSidekickOptions> MainPageConfigEntry =
				AddConfigure(opt => opt.RegisterViewAndModelMapping<MainPage, MainPage_Model>());
		}
	}


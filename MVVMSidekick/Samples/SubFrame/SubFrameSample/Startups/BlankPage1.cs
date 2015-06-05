using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using SubFrameSample;
using SubFrameSample.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace MVVMSidekick.Startups
{
	internal static partial class StartupFunctions
	{
		static Action BlankPage1Config =
		   CreateAndAddToAllConfig(ConfigBlankPage1);

		public static void ConfigBlankPage1()
		{
			ViewModelLocator<BlankPage1_Model>
				.Instance
				.Register(context =>
					new BlankPage1_Model())
				.GetViewMapper()
				.MapToDefault<BlankPage1>();

		}
	}
}

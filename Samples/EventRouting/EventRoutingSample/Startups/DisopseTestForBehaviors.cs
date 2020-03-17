using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using EventRoutingSample;
using EventRoutingSample.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace MVVMSidekick.Startups
{
	internal static partial class StartupFunctions
	{
		static Action DisopseTestForBehaviorsConfig =
			CreateAndAddToAllConfig(ConfigDisopseTestForBehaviors);

		public static void ConfigDisopseTestForBehaviors()
		{
			ViewModelLocator<DisopseTestForBehaviors_Model>
				.Instance
				.Register(context =>
					new DisopseTestForBehaviors_Model())
				.GetViewMapper()
				.MapToDefault<DisopseTestForBehaviors>();

		}
	}
}

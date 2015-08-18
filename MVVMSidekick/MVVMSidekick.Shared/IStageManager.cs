using System;
using System.Windows;
#if NETFX_CORE
using Windows.UI.Xaml;
#endif

namespace MVVMSidekick.Views
{
	public interface IStageManager
	{
		Stage this[string beaconKey] { get; }

		IView CurrentBindingView { get; }
		Stage DefaultStage { get; set; }

		void InitParent(Func<DependencyObject> parentLocator);
	}
}
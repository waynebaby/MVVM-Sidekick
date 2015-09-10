using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace TableGame_Sidekick.ViewModels
{

	//[DataContract]
	public class MainPage_Model : ViewModelBase<MainPage_Model>
	{
		// If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
		// 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

		public MainPage_Model()
		{
			if (IsInDesignMode)
			{
				Title = "Title is a little different in Design mode";
			}

		}


		//propvm tab tab string tab Title
		public String Title
		{
			get { return _TitleLocator(this).Value; }
			set { _TitleLocator(this).SetValueAndTryNotify(value); }
		}
		#region Property String Title Setup
		protected Property<String> _Title = new Property<String> { LocatorFunc = _TitleLocator };
		static Func<BindableBase, ValueContainer<String>> _TitleLocator = RegisterContainerLocator<String>("Title", model => model.Initialize("Title", ref model._Title, ref _TitleLocator, _TitleDefaultValueFactory));
		static Func<String> _TitleDefaultValueFactory = () => "Title is Here";
		#endregion


		#region Life Time Event Handling

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

		///// <summary>
		///// This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property
		///// </summary>
		///// <param name="view">View that firing Load event</param>
		///// <returns>Task awaiter</returns>
		//protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
		//{
		//    return base.OnBindedViewLoad(view);
		//}

		///// <summary>
		///// This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's  ViewModel property
		///// </summary>
		///// <param name="view">View that firing Unload event</param>
		///// <returns>Task awaiter</returns>
		//protected override Task OnBindedViewUnload(MVVMSidekick.Views.IView view)
		//{
		//    return base.OnBindedViewUnload(view);
		//}

		///// <summary>
		///// <para>If dispose actions got exceptions, will handled here. </para>
		///// </summary>
		///// <param name="exceptions">
		///// <para>The exception and dispose infomation</para>
		///// </param>
		//protected override async void OnDisposeExceptions(IList<DisposeInfo> exceptions)
		//{
		//    base.OnDisposeExceptions(exceptions);
		//    await TaskExHelper.Yield();
		//}

		#endregion

	}

}


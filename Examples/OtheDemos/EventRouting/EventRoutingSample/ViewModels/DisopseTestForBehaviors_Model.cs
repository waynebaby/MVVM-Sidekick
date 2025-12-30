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
using MVVMSidekick;
using Microsoft.Extensions.DependencyInjection;

namespace EventRoutingSample.ViewModels
{
	/// <summary>
	/// <para>DisopseTestForBehaviors视图的视图模型，用于测试行为释放功能</para>
	/// <para>View model for DisopseTestForBehaviors view, used for testing behavior disposal functionality</para>
	/// </summary>
	public class DisopseTestForBehaviors_Model : ViewModel<DisopseTestForBehaviors_Model>
	{
		// If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
		// 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

		/// <summary>
		/// <para>初始化 DisopseTestForBehaviors_Model 类的新实例</para>
		/// <para>Initializes a new instance of the DisopseTestForBehaviors_Model class</para>
		/// </summary>
		public DisopseTestForBehaviors_Model()
		{
			if (IsInDesignMode)
			{

			}
		}

		//propvm tab tab string tab Title

		#region Life Time Event Handling

		///// <summary>
		///// <para>当此视图模型实例被设置到视图的ViewModel属性时，将由视图调用</para>
		///// <para>This will be invoked by view when this viewmodel instance is set to view's ViewModel property.</para>
		///// </summary>
		///// <param name="view">设置目标 / Set target</param>
		///// <param name="oldValue">设置前的值 / Value before set</param>
		///// <returns>任务等待器 / Task awaiter</returns>
		//protected override Task OnBindedToView(MVVMSidekick.Views.IView view, IViewModel oldValue)
		//{
		//    return base.OnBindedToView(view, oldValue);
		//}

		///// <summary>
		///// <para>当ViewModel属性中的此视图模型实例被覆盖时，将由视图调用</para>
		///// <para>This will be invoked by view when this instance of viewmodel in ViewModel property is overwritten.</para>
		///// </summary>
		///// <param name="view">覆盖目标视图 / Overwrite target view</param>
		///// <param name="newValue">替换的值 / The value replacing</param>
		///// <returns>任务等待器 / Task awaiter</returns>
		//protected override Task OnUnbindedFromView(MVVMSidekick.Views.IView view, IViewModel newValue)
		//{
		//    return base.OnUnbindedFromView(view, newValue);
		//}

		///// <summary>
		///// <para>当视图触发Load事件且此视图模型实例已在视图的ViewModel属性中时，将由视图调用</para>
		///// <para>This will be invoked by view when the view fires Load event and this viewmodel instance is already in view's ViewModel property</para>
		///// </summary>
		///// <param name="view">触发Load事件的视图 / View that firing Load event</param>
		///// <returns>任务等待器 / Task awaiter</returns>
		//protected override Task OnBindedViewLoad(MVVMSidekick.Views.IView view)
		//{
		//    return base.OnBindedViewLoad(view);
		//}

		///// <summary>
		///// <para>当视图触发Unload事件且此视图模型实例仍在视图的ViewModel属性中时，将由视图调用</para>
		///// <para>This will be invoked by view when the view fires Unload event and this viewmodel instance is still in view's ViewModel property</para>
		///// </summary>
		///// <param name="view">触发Unload事件的视图 / View that firing Unload event</param>
		///// <returns>任务等待器 / Task awaiter</returns>
		//protected override Task OnBindedViewUnload(MVVMSidekick.Views.IView view)
		//{
		//    return base.OnBindedViewUnload(view);
		//}

		///// <summary>
		///// <para>如果释放操作发生异常，将在此处理</para>
		///// <para>If dispose actions got exceptions, will handled here.</para>
		///// </summary>
		///// <param name="exceptions">
		///// 异常和释放信息
		///// The exception and dispose information
		///// </param>
		//protected override async void OnDisposeExceptions(IList<DisposeEntry> exceptions)
		//{
		//    base.OnDisposeExceptions(exceptions);
		//    await Task.Yield();
		//}

		#endregion
	}
}


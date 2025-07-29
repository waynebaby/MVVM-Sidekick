using System;
using System.Reactive;
using MVVMSidekick.Commands;
using System.Windows.Input;
using System.Threading.Tasks;

namespace MVVMSidekick.Reactive
{
	/// <summary>
	/// 响应式命令接口，扩展了标准命令接口以支持响应式编程
	/// Reactive command interface that extends standard command interface to support reactive programming
	/// </summary>
	public interface IReactiveCommand : ICommand, ICommandWithViewModel	,IObservable<EventPattern<EventCommandEventArgs>> 
	{
		/// <summary>
		/// 获取可执行状态的可观察序列
		/// Gets the observable sequence of executable state
		/// </summary>
		IObservable<bool> CanExecuteObserveable { get; }
		
		/// <summary>
		/// 监听可执行状态的可观察序列
		/// Listens to the observable sequence of executable state
		/// </summary>
		/// <param name="canExecuteSeq">可执行状态序列 / Executable state sequence</param>
		/// <returns>可释放的订阅 / Disposable subscription</returns>
		IDisposable ListenCanExecuteObservable(IObservable<bool> canExecuteSeq);
		
		/// <summary>
		/// 覆盖可执行判断函数
		/// Overwrites the can execute function
		/// </summary>
		/// <param name="canExecuteFunc">可执行判断函数 / Can execute function</param>
		/// <returns>响应式命令 / Reactive command</returns>
		IReactiveCommand OverwriteCanExecute(Func<object, bool> canExecuteFunc);
		
		/// <summary>
		/// 异步执行命令
		/// Executes command asynchronously
		/// </summary>
		/// <param name="parameter">命令参数 / Command parameter</param>
		/// <returns>表示异步操作的任务 / Task representing the asynchronous operation</returns>
		Task ExecuteAsync(object parameter);
		
        /// <summary>
        /// 获取或设置命令标识符
        /// Gets or sets the command identifier
        /// </summary>
        string CommandId { get; set; }
	}


}
using System;
using System.Reactive;
using MVVMSidekick.Commands;
using System.Windows.Input;
using System.Threading.Tasks;

namespace MVVMSidekick.Reactive
{
	public interface IReactiveCommand : ICommand, ICommandWithViewModel	,IObservable<EventPattern<EventCommandEventArgs>> 
	{
		IObservable<bool> CanExecuteObserveable { get; }
		IDisposable ListenCanExecuteObservable(IObservable<bool> canExecuteSeq);
		IReactiveCommand OverwriteCanExecute(Func<object, bool> canExecuteFunc);
		Task ExecuteAsync(object parameter);
        string CommandId { get; set; }
	}


}
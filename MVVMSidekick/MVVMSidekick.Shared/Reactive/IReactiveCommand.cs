using System;
using System.Reactive;
using MVVMSidekick.Commands;
using System.Windows.Input;
using System.Threading.Tasks;

namespace MVVMSidekick.Reactive
{
	public interface IReactiveCommand : ICommand, ICommandWithViewModel	,IObservable<EventPattern<EventCommandEventArgs>> 
	{
		bool CanExecute(object parameter);
		IObservable<bool> CanExecuteObserveable { get; }
		IDisposable ListenCanExecuteObservable(IObservable<bool> canExecuteSeq);
		IReactiveCommand ConfigureSyncCanExecute(Func<object, bool> canExecuteFunc);
		//IDisposable Subscribe(IObserver<EventPattern<EventCommandEventArgs>> observer);
		Task ExecuteAsync(object parameter);
	}


}
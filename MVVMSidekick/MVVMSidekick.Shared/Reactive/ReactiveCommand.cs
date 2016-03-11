using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using MVVMSidekick.Commands;
using MVVMSidekick.ViewModels;
using System.Reactive.Subjects;
using System.Reactive.Linq;


namespace MVVMSidekick.Reactive
{
	public class ReactiveCommand : IReactiveCommand
	{
		BehaviorSubject<bool> _canExecuteSource;
		Subject<EventPattern<EventCommandEventArgs>> _executeSource;
		Func<object, bool> _canExecuteFunc;

		public ReactiveCommand(bool canExecute = false)
		{
			_canExecuteSource = new BehaviorSubject<bool>(canExecute);
			_canExecuteSource
				.Zip(_canExecuteSource.Skip(1),
					(x, y) =>
						x != y)
				.Where(x =>
					x && CanExecuteChanged != null)
				.Subscribe(_ =>
					CanExecuteChanged(this, EventArgs.Empty));


			_executeSource = new Subject<EventPattern<EventCommandEventArgs>>();
		}

		public BindableBase ViewModel
		{
			get; set;
		}

		public IObservable<bool> CanExecuteObserveable
		{
			get
			{
				return _canExecuteSource;
			}
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			if (_canExecuteFunc != null)
			{
				var value = _canExecuteFunc(parameter);
				_canExecuteSource.OnNext(value);
				return value;
			}
			else
			{
				return _canExecuteSource.Value;
			}
		}

		public IReactiveCommand ConfigureSyncCanExecute(Func<object, bool> canExecuteFunc)
		{
			_canExecuteFunc = canExecuteFunc;
			return this;
		}

		public void Execute(object parameter)
		{
			if (CanExecute(parameter))
			{
				var evp = EventCommandEventArgs.Create(parameter, ViewModel, null);
				_executeSource.OnNext(new EventPattern<EventCommandEventArgs>(this, evp));
			}
		}

		public async Task ExecuteAsync(object parameter)
		{
			if (CanExecute(parameter))
			{
				var evp = EventCommandEventArgs.Create(parameter, ViewModel);
				_executeSource.OnNext(new EventPattern<EventCommandEventArgs>(this, evp));
				await evp.Completion.Task;
			}
		}

		public IDisposable ListenCanExecuteObservable(IObservable<bool> canExecuteSeq)
		{
			return canExecuteSeq.Subscribe(_canExecuteSource);
		}

		public IDisposable Subscribe(IObserver<EventPattern<EventCommandEventArgs>> observer)
		{
			return _executeSource.Subscribe(observer);
		}
	}
}

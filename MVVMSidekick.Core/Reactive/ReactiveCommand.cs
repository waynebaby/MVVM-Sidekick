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
    /// <summary> 
    /// Reactive Command
    /// </summary>
    public class ReactiveCommand : IReactiveCommand, IDisposable
    {
        BehaviorSubject<bool> _canExecuteSource;
        Subject<EventPattern<EventCommandEventArgs>> _executeSource;
        Func<object, bool> _canExecuteFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveCommand"/> class.
        /// </summary>
        /// <param name="canExecute">if set to <c>true</c> [can execute].</param>
        public ReactiveCommand(bool canExecute = false,string commandId="NO_ID")
        {
            CommandId = commandId;
            _canExecuteSource = new BehaviorSubject<bool>(canExecute);
            _canExecuteSource
                .Zip(_canExecuteSource.Skip(1),
                    (x, y) =>
                        x != y)
                .Subscribe(_ =>
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty));


            _executeSource = new Subject<EventPattern<EventCommandEventArgs>>();
        }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public BindableBase ViewModel
        {
            get; set;
        }

        /// <summary>
        /// Gets the can execute observeable.
        /// </summary>
        /// <value>
        /// The can execute observeable.
        /// </value>
        public IObservable<bool> CanExecuteObserveable
        {
            get
            {
                return _canExecuteSource;
            }
        }

        public string CommandId { get; set; }

        /// <summary>
        /// Occurs when [can execute changed].
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Determines whether this instance can execute the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Overwrites the can execute logic.
        /// </summary>
        /// <param name="canExecuteFunc">The can execute function.</param>
        /// <returns></returns>
        public IReactiveCommand OverwriteCanExecute(Func<object, bool> canExecuteFunc)
        {
            _canExecuteFunc = canExecuteFunc;
            return this;
        }

        /// <summary>
        /// Executes the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                var evp = EventCommandEventArgs.Create(parameter, ViewModel, null);
                _executeSource.OnNext(new EventPattern<EventCommandEventArgs>(this, evp));
            }
        }

        /// <summary>
        /// Executes asynchronously.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        public async Task ExecuteAsync(object parameter)
        {
            if (CanExecute(parameter))
            {
                var evp = EventCommandEventArgs.Create(parameter, ViewModel);
                _executeSource.OnNext(new EventPattern<EventCommandEventArgs>(this, evp));
                await evp.Completion.Task;
            }
        }

        /// <summary>
        /// Listens the can execute observable.
        /// </summary>
        /// <param name="canExecuteSeq">The can execute seq.</param>
        /// <returns></returns>
        public IDisposable ListenCanExecuteObservable(IObservable<bool> canExecuteSeq)
        {
            return canExecuteSeq.Subscribe(_canExecuteSource);
        }

        /// <summary>
        /// Subscribes the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<EventPattern<EventCommandEventArgs>> observer)
        {
            return _executeSource.Subscribe(observer);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _executeSource.Dispose();
                }


                disposedValue = true;

            }
        }



        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }

}

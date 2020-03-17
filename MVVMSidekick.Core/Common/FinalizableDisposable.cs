using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace MVVMSidekick.Common
{
    /// <summary>
    /// Finalizable Disposable
    /// </summary>
    /// <typeparam name="TInnerDisposable">The type of the inner disposable.</typeparam>
    public sealed class FinalizableDisposable<TInnerDisposable> : IDisposable where TInnerDisposable : class, IDisposable
	{

        /// <summary>
        /// Initializes a new instance of the <see cref="FinalizableDisposable{TInnerDisposable}"/> class.
        /// </summary>
        /// <param name="innerDisposable">The inner disposable.</param>
        public FinalizableDisposable(TInnerDisposable innerDisposable)
		{
			_innerDisposable = innerDisposable;
		}

		TInnerDisposable _innerDisposable;

        /// <summary>
        /// Gets the inner disposable.
        /// </summary>
        /// <value>
        /// The inner disposable.
        /// </value>
        public TInnerDisposable InnerDisposable
		{
			get
			{
				return _innerDisposable;
			}
		}

        /// <summary>
        /// Finalizes an instance of the <see cref="FinalizableDisposable{TInnerDisposable}"/> class.
        /// </summary>
        ~FinalizableDisposable()
		{
			Dispose();
		}



        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
		{
			var d = Interlocked.Exchange<TInnerDisposable>(ref _innerDisposable, (TInnerDisposable)null);
			d?.Dispose();

		}


	}
}

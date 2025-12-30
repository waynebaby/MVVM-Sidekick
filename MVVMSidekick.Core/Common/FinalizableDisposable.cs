using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace MVVMSidekick.Common
{
    /// <summary>
    /// 可终结的Disposable包装器
    /// Finalizable Disposable
    /// </summary>
    /// <typeparam name="TInnerDisposable">内部可销毁对象的类型 / The type of the inner disposable.</typeparam>
    public sealed class FinalizableDisposable<TInnerDisposable> : IDisposable where TInnerDisposable : class, IDisposable
	{

        /// <summary>
        /// 初始化FinalizableDisposable类的新实例
        /// Initializes a new instance of the <see cref="FinalizableDisposable{TInnerDisposable}"/> class.
        /// </summary>
        /// <param name="innerDisposable">内部可销毁对象 / The inner disposable.</param>
        public FinalizableDisposable(TInnerDisposable innerDisposable)
		{
			_innerDisposable = innerDisposable;
		}

		TInnerDisposable _innerDisposable;

        /// <summary>
        /// 获取内部可销毁对象
        /// Gets the inner disposable.
        /// </summary>
        /// <value>
        /// 内部可销毁对象 / The inner disposable.
        /// </value>
        public TInnerDisposable InnerDisposable
		{
			get
			{
				return _innerDisposable;
			}
		}

        /// <summary>
        /// 析构函数，确保资源被释放
        /// Finalizes an instance of the <see cref="FinalizableDisposable{TInnerDisposable}"/> class.
        /// </summary>
        ~FinalizableDisposable()
		{
			Dispose();
		}

        /// <summary>
        /// 释放非托管资源和可选的托管资源
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
		{
			var d = Interlocked.Exchange<TInnerDisposable>(ref _innerDisposable, (TInnerDisposable)null);
			d?.Dispose();

		}


	}
}

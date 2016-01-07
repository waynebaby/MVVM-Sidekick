using MVVMSidekick.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace MVVMSidekick.Common
{

	public static class CommonHelper
	{
		public static FinalizableDisposable<TInnerDisposable> MakeFinalizableDisposable<TInnerDisposable>(this TInnerDisposable innerDisposable) where TInnerDisposable : class, IDisposable
		{
			return new FinalizableDisposable<TInnerDisposable>(innerDisposable);
		}

	}

	[DataContract]
	public class InstanceCounableBase
	{
		static ConcurrentDictionary<Type, InstanceCounter> _instanceCounters = new ConcurrentDictionary<Type, InstanceCounter>();

		protected int _instanceIdOfThisType;
		protected InstanceCounableBase()
		{
			var ct = _instanceCounters.GetOrAdd(this.GetType(), t => new InstanceCounter());
			_instanceIdOfThisType = ct.Click();
		}

		public override string ToString()
		{
			return string.Format("Id {0} of {1} ({2})", _instanceIdOfThisType, base.GetType().Name, base.ToString());
		}
	}


	public class InstanceCounter
	{
		int count;

		public int Click()
		{
			return Interlocked.Increment(ref count);
		}

	}

	public class FinalizableDisposable<TInnerDisposable> : IDisposable where TInnerDisposable : class, IDisposable
	{

		public FinalizableDisposable(TInnerDisposable innerDisposable)
		{
			_innerDisposable = innerDisposable;
		}

		TInnerDisposable _innerDisposable;

		public TInnerDisposable InnerDisposable
		{
			get
			{
				return _innerDisposable;
			}
		}

		~FinalizableDisposable()
		{
			Dispose();
		}


		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			var d = Interlocked.Exchange<TInnerDisposable>(ref _innerDisposable, (TInnerDisposable)null);
			d?.Dispose();

		}


	}
}

using System;

namespace MVVMSidekick.Common
{
    public static class CommonHelper
	{
		public static FinalizableDisposable<TInnerDisposable> MakeFinalizableDisposable<TInnerDisposable>(this TInnerDisposable innerDisposable) where TInnerDisposable : class, IDisposable
		{
			return new FinalizableDisposable<TInnerDisposable>(innerDisposable);
		}

	}
}

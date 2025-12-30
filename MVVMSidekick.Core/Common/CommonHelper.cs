using System;

namespace MVVMSidekick.Common
{
    /// <summary>
    /// 通用助手类，提供常用的扩展方法和实用功能
    /// Common helper class that provides commonly used extension methods and utility functions
    /// </summary>
    public static class CommonHelper
	{
        /// <summary>
        /// 将IDisposable对象包装为可终结的Disposable对象
        /// Wraps an IDisposable object as a finalizable disposable object
        /// </summary>
        /// <typeparam name="TInnerDisposable">内部Disposable对象的类型 / Type of the inner disposable object</typeparam>
        /// <param name="innerDisposable">要包装的内部Disposable对象 / The inner disposable object to wrap</param>
        /// <returns>包装后的可终结Disposable对象 / The wrapped finalizable disposable object</returns>
		public static FinalizableDisposable<TInnerDisposable> MakeFinalizableDisposable<TInnerDisposable>(this TInnerDisposable innerDisposable) where TInnerDisposable : class, IDisposable
		{
			return new FinalizableDisposable<TInnerDisposable>(innerDisposable);
		}
	}
}

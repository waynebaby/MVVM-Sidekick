using System.Threading;

namespace MVVMSidekick.Common
{
    /// <summary>
    /// 实例计数器类，提供线程安全的实例计数功能
    /// Instance counter class that provides thread-safe instance counting functionality
    /// </summary>
    public class InstanceCounter
	{
        /// <summary>
        /// 当前计数值
        /// Current count value
        /// </summary>
		int count;

        /// <summary>
        /// 增加计数并返回新的计数值（线程安全）
        /// Increments the count and returns the new count value (thread-safe)
        /// </summary>
        /// <returns>递增后的计数值 / The incremented count value</returns>
		public int Click()
		{
			return Interlocked.Increment(ref count);
		}
	}
}

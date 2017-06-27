using System.Threading;

namespace MVVMSidekick.Common
{
    public class InstanceCounter
	{
		int count;

		public int Click()
		{
			return Interlocked.Increment(ref count);
		}

	}
}

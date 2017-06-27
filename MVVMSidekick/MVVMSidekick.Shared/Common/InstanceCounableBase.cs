using System;
using System.Collections.Concurrent;
using System.Runtime.Serialization;

namespace MVVMSidekick.Common
{
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
}

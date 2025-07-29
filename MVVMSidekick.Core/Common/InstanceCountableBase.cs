using System;
using System.Collections.Concurrent;
using System.Runtime.Serialization;

namespace MVVMSidekick.Common
{
    /// <summary>
    /// 实例可计数基类，为继承的类提供实例计数功能
    /// Instance countable base class that provides instance counting functionality for inheriting classes
    /// </summary>
    [DataContract]
	public class InstanceCountableBase
	{
        /// <summary>
        /// 静态实例计数器字典，按类型存储计数器
        /// Static instance counter dictionary that stores counters by type
        /// </summary>
		static ConcurrentDictionary<Type, InstanceCounter> _instanceCounters = new ConcurrentDictionary<Type, InstanceCounter>();

        /// <summary>
        /// 当前实例在其类型中的唯一标识符
        /// Unique identifier of the current instance within its type
        /// </summary>
		protected int _instanceIdOfThisType;
		
        /// <summary>
        /// 初始化InstanceCountableBase类的新实例，并为当前类型分配唯一ID
        /// Initializes a new instance of the InstanceCountableBase class and assigns a unique ID for the current type
        /// </summary>
		protected InstanceCountableBase()
		{
			var ct = _instanceCounters.GetOrAdd(this.GetType(), t => new InstanceCounter());
			_instanceIdOfThisType = ct.Click();
		}

        /// <summary>
        /// 返回包含实例ID和类型信息的字符串表示形式
        /// Returns a string representation that includes instance ID and type information
        /// </summary>
        /// <returns>包含实例ID的字符串 / String containing instance ID</returns>
		public override string ToString()
		{
			return string.Format("Id {0} of {1} ({2})", _instanceIdOfThisType, base.GetType().Name, base.ToString());
		}
	}
}

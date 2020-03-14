using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMSidekick.Common
{

    /// <summary>
    /// <para>Dispose action infomation struct</para>
    /// <para>注册销毁方法时的相关信息</para>
    /// </summary>
    public struct DisposeEntry
    {
        /// <summary>
        /// <para>Code Context in this dispose action execution register .</para>
        /// <para>执行代码上下文</para>
        /// </summary>
        /// <value>The calling code context.</value>
        public CallingCodeContext CallingCodeContext { get; set; }

        /// <summary>
        /// 是否为托管资源，需要在析构器强制检查
        /// </summary>
        /// <value><c>true</c> if this instance is need check on finalizer; otherwise, <c>false</c>.</value>
        public bool IsNeedCheckOnFinalizer { get; set; }

        /// <summary>
        /// <para>Exception thrown in this dispose action execution .</para>
        /// <para>执行此次Dispose动作产生的Exception</para>
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }
        /// <summary>
        /// <para>Dispose action.</para>
        /// <para>Dispose动作</para>
        /// </summary>
        /// <value>The action.</value>

        public Action Action { get; set; }
    }

}

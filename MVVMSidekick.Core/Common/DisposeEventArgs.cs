using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMSidekick.Common
{
    /// <summary>
    /// 销毁事件参数类
    /// Class DisposeEventArgs.
    /// </summary>
    public class DisposeEventArgs : EventArgs
    {
        /// <summary>
        /// 创建DisposeEventArgs实例
        /// Creates the specified information.
        /// </summary>
        /// <param name="info">销毁信息 / The information.</param>
        /// <returns>DisposeEventArgs实例 / DisposeEventArgs.</returns>
        public static DisposeEventArgs Create(DisposeEntry info)
        {
            return new DisposeEventArgs(info);
        }
        
        /// <summary>
        /// 初始化DisposeEventArgs类的新实例
        /// Initializes a new instance of the <see cref="DisposeEventArgs"/> class.
        /// </summary>
        /// <param name="info">销毁信息 / The information.</param>
        public DisposeEventArgs(DisposeEntry info)
        {
            DisposeEntry = info;
        }
        
        /// <summary>
        /// 获取销毁条目信息
        /// Gets the dispose entry.
        /// </summary>
        /// <value>销毁条目 / The dispose entry.</value>
        public DisposeEntry DisposeEntry { get; private set; }
    }
}

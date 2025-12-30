using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMSidekick.Common
{

    /// <summary>
    /// 销毁组接口，用于管理一组需要同时销毁的对象
    /// Interface IDisposeGroup
    /// </summary>
    public interface IDisposeGroup : IDisposable
    {
        /// <summary>
        /// 增加一个一起Dispose的对象
        /// Adds a disposable object that will be disposed together
        /// </summary>
        /// <param name="item">要添加的可销毁对象 / The item.</param>
        /// <param name="needCheckInFinalizer">是否需要在终结器中检查 / if set to <c>true</c> [need check in finalizer].</param>
        /// <param name="comment">注释 / The comment.</param>
        /// <param name="member">调用成员名 / The member.</param>
        /// <param name="file">文件路径 / The file.</param>
        /// <param name="line">行号 / The line.</param>
        void AddDisposable(IDisposable item, bool needCheckInFinalizer = false, string comment = "", string member = "", string file = "", int line = -1);

        /// <summary>
        /// 增加一个Dispose的时候需要做的操作
        /// Adds an action to be executed during disposal
        /// </summary>
        /// <param name="action">要执行的操作 / The action.</param>
        /// <param name="needCheckInFinalizer">是否需要在终结器中检查 / if set to <c>true</c> [need check in finalizer].</param>
        /// <param name="comment">注释 / The comment.</param>
        /// <param name="member">调用成员名 / The member.</param>
        /// <param name="file">文件路径 / The file.</param>
        /// <param name="line">行号 / The line.</param>
        void AddDisposeAction(Action action, bool needCheckInFinalizer = false, string comment = "", string member = "", string file = "", int line = -1);

        /// <summary>
        /// 获取销毁信息列表
        /// Gets the dispose information list.
        /// </summary>
        /// <value>销毁信息列表 / The dispose information list.</value>
        IList<DisposeEntry> DisposeInfoList { get; }

        /// <summary>
        /// 当销毁条目正在销毁时发生
        /// Occurs when [disposing entry].
        /// </summary>
        event EventHandler<DisposeEventArgs> DisposeEntryDisposing;
        
        /// <summary>
        /// 当销毁条目已销毁时发生
        /// Occurs when [disposed entry].
        /// </summary>
        event EventHandler<DisposeEventArgs> DisposeEntryDisposed;


    }


}

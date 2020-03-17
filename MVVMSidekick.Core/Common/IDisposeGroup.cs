using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMSidekick.Common
{

    /// <summary>
    /// Interface IDisposeGroup
    /// </summary>
    public interface IDisposeGroup : IDisposable
    {
        /// <summary>
        /// 增加一个一起Dispose的对象
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="needCheckInFinalizer">if set to <c>true</c> [need check in finalizer].</param>
        /// <param name="comment">The comment.</param>
        /// <param name="member">The member.</param>
        /// <param name="file">The file.</param>
        /// <param name="line">The line.</param>
        void AddDisposable(IDisposable item, bool needCheckInFinalizer = false, string comment = "", string member = "", string file = "", int line = -1);

        /// <summary>
        /// 增加一个Dispose的时候需要做的操作
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="needCheckInFinalizer">if set to <c>true</c> [need check in finalizer].</param>
        /// <param name="comment">The comment.</param>
        /// <param name="member">The member.</param>
        /// <param name="file">The file.</param>
        /// <param name="line">The line.</param>
        void AddDisposeAction(Action action, bool needCheckInFinalizer = false, string comment = "", string member = "", string file = "", int line = -1);


        /// <summary>
        /// Gets the dispose information list.
        /// </summary>
        /// <value>The dispose information list.</value>
        IList<DisposeEntry> DisposeInfoList { get; }

        /// <summary>
        /// Occurs when [disposing entry].
        /// </summary>
        event EventHandler<DisposeEventArgs> DisposingEntry;
        /// <summary>
        /// Occurs when [disposed entry].
        /// </summary>
        event EventHandler<DisposeEventArgs> DisposedEntry;


    }


}

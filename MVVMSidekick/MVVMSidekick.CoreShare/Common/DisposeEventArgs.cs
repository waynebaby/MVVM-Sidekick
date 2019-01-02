using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMSidekick.Common
{
    /// <summary>
    /// Class DisposeEventArgs.
    /// </summary>
    public class DisposeEventArgs : EventArgs
    {
        /// <summary>
        /// Creates the specified information.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>DisposeEventArgs.</returns>
        public static DisposeEventArgs Create(DisposeEntry info)
        {
            return new DisposeEventArgs(info);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DisposeEventArgs"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        public DisposeEventArgs(DisposeEntry info)
        {
            DisposeEntry = info;
        }
        /// <summary>
        /// Gets the dispose entry.
        /// </summary>
        /// <value>The dispose entry.</value>
        public DisposeEntry DisposeEntry { get; private set; }
    }
}

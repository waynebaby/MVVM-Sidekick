using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MVVMSidekick.Common
{

    /// <summary>
    /// 代码调用上下文
    /// Calling code-context
    /// </summary>
    public struct CallingCodeContext
    {
        /// <summary>
        /// 创建一个当前调用上下文数据
        /// </summary>
        /// <param name="autoFillProperties">if set to <c>true</c> [automatic fill properties].</param>
        /// <param name="comment">注释</param>
        /// <param name="caller">调用者</param>
        /// <param name="file">文件</param>
        /// <param name="line">行数</param>
        public CallingCodeContext(bool autoFillProperties, string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber]int line = -1)
            : this()
        {
            if (autoFillProperties)
            {
                Caller = caller;
                Comment = comment;
                File = file;
                Line = line;
            }

        }



        /// <summary>
        /// 创建一个当前调用上下文数据
        /// </summary>
        /// <param name="comment">注释</param>
        /// <param name="caller">调用者</param>
        /// <param name="file">文件</param>
        /// <param name="line">行数</param>
        /// <returns>数据</returns>
        public static CallingCodeContext Create(string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber]int line = -1)
        {
            return new CallingCodeContext
                (true, comment, caller, file, line);
        }

        /// <summary>
        /// <para>Comment of this Calling .</para>
        /// <para>对此次Calling 的附加说明</para>
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; private set; }
        /// <summary>
        /// <para>Caller Member Name of this Calling  registeration.</para>
        /// <para>此次Calling 注册的来源</para>
        /// </summary>
        /// <value>The caller.</value>
        public string Caller { get; private set; }
        /// <summary>
        /// <para>Code file path of this Calling  registeration.</para>
        /// <para>注册此次Calling 注册的代码文件</para>
        /// </summary>
        /// <value>The file.</value>
        public string File { get; private set; }
        /// <summary>
        /// <para>Code line number of this Calling  registeration.</para>
        /// <para>注册此次Calling 注册的代码行</para>
        /// </summary>
        /// <value>The line.</value>
        public int Line { get; private set; }

    }


}

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
        /// Creates a current calling context data
        /// </summary>
        /// <param name="autoFillProperties">是否自动填充属性 / If set to true, automatically fill properties</param>
        /// <param name="comment">注释 / Comment</param>
        /// <param name="caller">调用者 / Caller member name</param>
        /// <param name="file">文件 / File path</param>
        /// <param name="line">行数 / Line number</param>
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
        /// Creates a current calling context data
        /// </summary>
        /// <param name="comment">注释 / Comment</param>
        /// <param name="caller">调用者 / Caller member name</param>
        /// <param name="file">文件 / File path</param>
        /// <param name="line">行数 / Line number</param>
        /// <returns>数据 / The context data</returns>
        public static CallingCodeContext Create(string comment = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber]int line = -1)
        {
            return new CallingCodeContext
                (true, comment, caller, file, line);
        }

        /// <summary>
        /// <para>Comment of this Calling.</para>
        /// <para>对此次调用的附加说明。</para>
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; private set; }
        /// <summary>
        /// <para>Caller Member Name of this Calling registration.</para>
        /// <para>此次调用注册的来源。</para>
        /// </summary>
        /// <value>The caller.</value>
        public string Caller { get; private set; }
        /// <summary>
        /// <para>Code file path of this Calling registration.</para>
        /// <para>注册此次调用的代码文件。</para>
        /// </summary>
        /// <value>The file.</value>
        public string File { get; private set; }
        /// <summary>
        /// <para>Code line number of this Calling registration.</para>
        /// <para>注册此次调用的代码行。</para>
        /// </summary>
        /// <value>The line.</value>
        public int Line { get; private set; }

    }


}

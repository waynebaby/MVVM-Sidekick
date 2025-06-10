using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode
{
        /// <summary>
        /// <para>Interface for a command line command.</para>
        /// <para>命令行指令接口。</para>
        /// </summary>
        public interface ICommandLineCommand
        {
                /// <summary>
                /// <para>Gets the command keyword.</para>
                /// <para>获取命令关键字。</para>
                /// </summary>
                string CommandKeyword { get; }

                /// <summary>
                /// <para>Execute the command.</para>
                /// <para>执行命令。</para>
                /// </summary>
                /// <param name="args">Arguments/参数</param>
                void Execute(string[] args);

                /// <summary>
                /// <para>Show help information.</para>
                /// <para>显示帮助信息。</para>
                /// </summary>
                void Help();

                /// <summary>
                /// <para>Gets the parent command.</para>
                /// <para>获取父命令。</para>
                /// </summary>
                ICommandLineCommand Parent { get; }

                /// <summary>
                /// <para>Gets the sub commands.</para>
                /// <para>获取子命令集合。</para>
                /// </summary>
                IDictionary<string, ICommandLineCommand> Children { get; }
        }

        /// <summary>
        /// <para>Default implementation of <see cref="ICommandLineCommand"/>.</para>
        /// <para><see cref="ICommandLineCommand"/> 的默认实现。</para>
        /// </summary>
        public class CommandLineCommand : ICommandLineCommand
        {


                /// <summary>
                /// <para>Initializes a new instance of <see cref="CommandLineCommand"/>.</para>
                /// <para>初始化 <see cref="CommandLineCommand"/> 类的新实例。</para>
                /// </summary>
                /// <param name="commandKeyword">Command keyword/命令关键字</param>
                /// <param name="parent">Parent command/父命令</param>
                public CommandLineCommand(string commandKeyword, ICommandLineCommand parent)
                {
                        CommandKeyword = commandKeyword;
                        Parent = parent;
                }

                /// <summary>
                /// <para>Gets the child commands.</para>
                /// <para>获取子命令。</para>
                /// </summary>
                public virtual IDictionary<string, ICommandLineCommand> Children { get; private set; }
                        = new SortedDictionary<string, ICommandLineCommand>(
                                Comparer<string>
                                        .Create((x, y) => string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase)));


                /// <summary>
                /// <para>Gets the command keyword.</para>
                /// <para>获取命令关键字。</para>
                /// </summary>
                public virtual string CommandKeyword { get; }
                /// <summary>
                /// <para>Gets the parent command.</para>
                /// <para>获取父命令。</para>
                /// </summary>
                public virtual ICommandLineCommand Parent { get; }


                /// <summary>
                /// <para>Action executed when <see cref="Execute"/> is called.</para>
                /// <para>当调用 <see cref="Execute"/> 时执行的委托。</para>
                /// </summary>
                public Action<string[]> OnExecute { get; set; } = args => { };
                /// <summary>
                /// <para>Execute the command.</para>
                /// <para>执行命令。</para>
                /// </summary>
                /// <param name="args">Arguments/参数</param>
                public void Execute(string[] args)
                {
                        if (OnExecute != null)
                        {
                                OnExecute(args);
                        }
                }

                /// <summary>
                /// <para>Action executed when <see cref="Help"/> is called.</para>
                /// <para>当调用 <see cref="Help"/> 时执行的委托。</para>
                /// </summary>
                public Action OnHelp { get; set; } = () => { };
                /// <summary>
                /// <para>Show help information.</para>
                /// <para>显示帮助信息。</para>
                /// </summary>
                public void Help()
                {
                        if (OnHelp != null)
                        {
                                OnHelp();
                        }
                }
        }
}

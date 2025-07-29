using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode
{
	/// <summary>
	/// 定义命令行命令的接口
	/// Defines the interface for command line commands
	/// </summary>
	public interface ICommandLineCommand
	{
		/// <summary>
		/// 获取命令关键字
		/// Gets the command keyword
		/// </summary>
		string CommandKeyword { get; }

		/// <summary>
		/// 执行命令
		/// Executes the command
		/// </summary>
		/// <param name="args">命令参数 / Command arguments</param>
		void Execute(string[] args);

		/// <summary>
		/// 显示帮助信息
		/// Shows help information
		/// </summary>
		void Help();

		/// <summary>
		/// 获取父命令
		/// Gets the parent command
		/// </summary>
		ICommandLineCommand Parent { get; }

		/// <summary>
		/// 获取子命令集合
		/// Gets the collection of child commands
		/// </summary>
		IDictionary<string, ICommandLineCommand> Children { get; }
	}

	/// <summary>
	/// 命令行命令的默认实现
	/// Default implementation of command line command
	/// </summary>
	public class CommandLineCommand : ICommandLineCommand
	{

		/// <summary>
		/// 初始化命令行命令的新实例
		/// Initializes a new instance of the command line command
		/// </summary>
		/// <param name="commandKeyword">命令关键字 / Command keyword</param>
		/// <param name="parent">父命令 / Parent command</param>
		public CommandLineCommand(string commandKeyword, ICommandLineCommand parent)
		{
			CommandKeyword = commandKeyword;
			Parent = parent;
		}

		/// <summary>
		/// 获取子命令集合，按名称排序
		/// Gets the collection of child commands, sorted by name
		/// </summary>
		public virtual IDictionary<string, ICommandLineCommand> Children { get; private set; }
			= new SortedDictionary<string, ICommandLineCommand>(
				Comparer<string>
					.Create((x, y) => string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase)));


		/// <summary>
		/// 获取命令关键字
		/// Gets the command keyword
		/// </summary>
		public virtual string CommandKeyword { get; }
		
		/// <summary>
		/// 获取父命令
		/// Gets the parent command
		/// </summary>
		public virtual ICommandLineCommand Parent { get; }


		/// <summary>
		/// 获取或设置命令执行的委托
		/// Gets or sets the delegate for command execution
		/// </summary>
		public Action<string[]> OnExecute { get; set; } = args => { };
		
		/// <summary>
		/// 执行命令
		/// Executes the command
		/// </summary>
		/// <param name="args">命令参数 / Command arguments</param>
		public void Execute(string[] args)
		{
			if (OnExecute != null)
			{
				OnExecute(args);
			}
		}

		/// <summary>
		/// 获取或设置帮助显示的委托
		/// Gets or sets the delegate for help display
		/// </summary>
		public Action OnHelp { get; set; } = () => { };
		
		/// <summary>
		/// 显示帮助信息
		/// Shows help information
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

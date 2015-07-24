using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode
{
	public interface ICommandLineCommand
	{
		string CommandKeyword { get; }

		void Execute(string[] args);

		void Help();

		ICommandLineCommand Parent { get; }

		IDictionary<string, ICommandLineCommand> Children { get; }
	}

	public class CommandLineCommand : ICommandLineCommand
	{


		public CommandLineCommand(string commandKeyword, ICommandLineCommand parent)
		{
			CommandKeyword = commandKeyword;
			Parent = parent;
		}

		public virtual IDictionary<string, ICommandLineCommand> Children { get; private set; }
			= new SortedDictionary<string, ICommandLineCommand>(
				Comparer<string>
					.Create((x, y) => string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase)));


		public virtual string CommandKeyword { get; }
		public virtual ICommandLineCommand Parent { get; }


		public Action<string[]> OnExecute { get; set; } = args => { };
		public void Execute(string[] args)
		{
			if (OnExecute != null)
			{
				OnExecute(args);
			}
		}

		public Action OnHelp { get; set; } = () => { };
		public void Help()
		{
			if (OnHelp != null)
			{
				OnHelp();
			}
		}
	}
}

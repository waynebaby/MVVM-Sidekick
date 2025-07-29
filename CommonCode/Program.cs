using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode
{
	/// <summary>
	/// 程序入口点类
	/// Program entry point class
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// 应用程序的主入口点
		/// The main entry point for the application
		/// </summary>
		/// <param name="args">命令行参数 / Command line arguments</param>
		public static void Main(string[] args)
		{
			var cmd = Commands.GetCommand (args[0]);

			try
			{

				cmd.Execute(args);

			}
			catch (Exception ex)
			{

				Console.WriteLine(ex);

                if (args.LastOrDefault() =="pause")
                {
                    Console.ReadLine();
                }
			}


																					
			
		}	 
	}


	
}

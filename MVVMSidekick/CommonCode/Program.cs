using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode
{
	public static class Program
	{
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
			}


																					
			
		}	 
	}


	
}

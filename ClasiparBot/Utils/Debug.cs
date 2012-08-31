// 
// SmsServer.Debug.cs by Carlos Ruiz Diaz
// User: Carlos Ruiz Diaz at 07/29/2010 19:48, Asunción - Paraguay
// 
// carlos.ruizdiaz@gmail.com
// 
// 

using System;

namespace ClasiparBot.Utils
{
	public class Debug
	{
		static object _locker	= new object();			
		
		public static void PrintLine(params string[] parameters)
		{
			lock(_locker)
			{
				Console.Write("[{0}] ", DateTime.Now);
				foreach(string parameter in parameters)
					Console.Write("{0} ", parameter);
				
				Console.WriteLine();
			}
		}
		
		public static void Notify(params string[] parameters)
		{
			//ConsoleColor prevColor		= Console.ForegroundColor;
			
			//Console.ForegroundColor		= ConsoleColor.Green;
			parameters[0]					= "[_NOTICE_] " + parameters[0];
			PrintLine(parameters);
			//Console.ForegroundColor		= prevColor;
			
		}
		
		public static void PrintError(params string[] parameters)
		{
			//ConsoleColor prevColor		= Console.ForegroundColor;
			
			//Console.ForegroundColor		= ConsoleColor.Red;
			parameters[0]					= "[_ERROR_] " + parameters[0];
			PrintLine(parameters);
			//Console.ForegroundColor		= prevColor;
		}
	}
}

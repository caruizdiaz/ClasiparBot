// 
// ${ProjectName}.Main.cs by Carlos Ruiz Diaz
// User: Carlos Ruiz Diaz at 08/28/2010 16:38, Asunción - Paraguay
// 
// carlos.ruizdiaz@gmail.com
// 
// 
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.Specialized;
using System.Collections.Generic;

using HtmlAgilityPack;

using ClasiparBot.Bot;
using ClasiparBot.Utils;

namespace ClasiparBot
{
	class MainClass
	{
		static List<BotParameters> ParseConfigurationFile(string configurationFile)
		{
			using(StreamReader reader = new StreamReader(configurationFile))
			{
				List<BotParameters> parameters	= new List<BotParameters>();
				
				do
				{
					string line				= reader.ReadLine();
					string[] parameter		= line.Split(';');
					
					if (parameter.Length != 4)
					{
						Debug.PrintError("MainClass.ParseConfigurationFile(): Invalid line '", line, "'");	
						continue;
					}
					
					parameters.Add(new BotParameters(parameter[0], parameter[1], parameter[2], parameter[3]));
				}
				while(!reader.EndOfStream);
				
				return parameters;
			}
		}
		
		
		public static void Main (string[] args)
		{	
			if (args.Length != 1)
			{
				Debug.PrintError("MainClass.Main(): Invalid command line parameter(s)");
				return;
			}
			
			if (!File.Exists(args[0]))
			{
				Debug.PrintError("MainClass.Main(): Configuration file is missing");
				return;
			}					
			
			List<BotParameters> botParams	= ParseConfigurationFile(args[0]);						
			
			foreach(BotParameters botParameter in botParams)			
				Debug.Notify("MainClass.Main(): Error free? ", new ClasiBot(botParameter).StartBot().ToString().ToUpper());	
						
			return;			
		}				
	}
}


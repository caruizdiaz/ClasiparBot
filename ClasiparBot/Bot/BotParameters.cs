// 
// ClasiparBot.LoginParameters.cs by Carlos Ruiz Diaz
// User: Carlos Ruiz Diaz at 09/01/2010 09:49, Asunción - Paraguay
// 
// carlos.ruizdiaz@gmail.com
// 
// 

using System;
using System.Net;

namespace ClasiparBot.Bot
{
	public class BotParameters
	{	
		Uri _managementUri;
		string _userEmail;
		Uri _announcementUri;		
		AnnouncementOperation _annOperation	= AnnouncementOperation.Refresh;
		
		public BotParameters() {	}
		
		public BotParameters(string managementUri, string userEmail, string announcementUri, string operation)
		{
			_managementUri		= new Uri(managementUri);			
			_announcementUri	= new Uri(announcementUri);
			_userEmail			= userEmail;
			
			AsignAnnouncementOperation(operation);
		}
		
		public void AsignAnnouncementOperation(string operation)
		{
			switch(operation.Trim().ToLower())
			{
			default:
				throw new ArgumentException("Unrecognized announcement operation");
				break;
			case "refresh":
				_annOperation	= AnnouncementOperation.Refresh;
				break;
			case "delete":
				_annOperation	= AnnouncementOperation.Delete;
				break;
			case "sold":
				_annOperation	= AnnouncementOperation.MarkAsSold;
				break;
			}
		}
		
		public Uri ManagementLink
		{
			get { return _managementUri; }
			set { _managementUri = value; }
		}
		
		public string UserEmail
		{
			get { return _userEmail; }
			set { _userEmail = value; }
		}
		
		public Uri AnnouncementLink
		{
			get { return _announcementUri; }
			set { _announcementUri = value; }
		}
		
		public AnnouncementOperation Operation
		{
			get { return _annOperation; }
			set { _annOperation = value; }
		}
	}
}
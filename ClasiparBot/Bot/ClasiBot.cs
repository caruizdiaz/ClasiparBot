// 
// ClasiparBot.ClasiBot.cs by Carlos Ruiz Diaz
// User: Carlos Ruiz Diaz at 09/01/2010 09:42, Asunción - Paraguay
// 
// carlos.ruizdiaz@gmail.com
// 
// 

using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

using HtmlAgilityPack;

using ClasiparBot.Utils;

namespace ClasiparBot.Bot
{
	public class ClasiBot
	{
		BotParameters _parameter;			
			
		public ClasiBot(BotParameters parameter)
		{
			_parameter		= parameter;
		}
		
		public bool StartBot()
		{
			try
			{
				ServicePointManager.Expect100Continue	= false;
			
				HttpWebRequest request					= (HttpWebRequest) WebRequest.Create(_parameter.ManagementLink);
				request.UserAgent						= "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.1.9) Gecko/20100317 SUSE/3.5.9-0.1.1 Firefox/3.5.9\r\n";
				
				request.CookieContainer					= new CookieContainer();
				
				HttpWebResponse response				= (HttpWebResponse) request.GetResponse();
				StreamReader reader						= new StreamReader(response.GetResponseStream());
							
				HtmlDocument htmlDoc					= new HtmlDocument();
				htmlDoc.LoadHtml(reader.ReadToEnd());
				
				string hashID							= ParseHashID(_parameter.ManagementLink.ToString()).Trim();
				string authToken						= htmlDoc.DocumentNode.SelectSingleNode("//input[@name='authenticity_token']").Attributes["value"].Value;
				
				Debug.Notify("ClasiBot.StartBot(): HashID is", hashID);
				Debug.Notify("ClasiBot.StartBot(): Authenticity token is", authToken);			
				
				string output							= AccessDashboard(request.CookieContainer, hashID, authToken);
				output									= PerformAnnouncementOperation(request.CookieContainer);
				Debug.Notify("ClasiBot.StartBot(): Operation result is '", ParseResult(output), "'");			
				
				return true;
			}
			catch(Exception ex)
			{
				Debug.PrintError("ClasiBot.StartBot(): ", ex.Message);
				return false;
			}	
		}
		
		string ParseHashID(string uri)
		{
			Match match		= Regex.Match(uri, @".+hash=(?<hash>\w+)$");
			
			if (!match.Success)
				throw new Exception("Error parsing hashID in '" + uri + "'");
			
			return match.Groups["hash"].Value;
		}	
		
				
		// <div class="msj_check">Su anuncio ha sido editado</div>
		// <div class="msj_error">El anuncio ya tiene fecha de hoy, sólo puede 'ACTUALIZAR' un anuncio una vez al día</div>
		
		public string ParseResult(string html)
		{
			Match match		= Regex.Match(html, @"<div class=""msj_check"">(?<message>.+)</div>");
			
			if (match.Success)
				return match.Groups["message"].Value;
			
			match		= Regex.Match(html, @"<div class=""msj_error"">(?<message>.+)</div>");
			
			if (!match.Success)
				throw new Exception("Unable to match result in output html");
			
			return match.Groups["message"].Value;
		}
		
		string BuildURIByOperation(Uri mainUri, AnnouncementOperation operation, Uri announcementUri)
		{
			string operationUri			= "http://" + mainUri.Host + "/ad/";
			
			switch(operation)		
			{
			case AnnouncementOperation.Refresh:
				operationUri	+= "refresh_ad";
				break;
			case AnnouncementOperation.Delete:
				operationUri	+= "delete_ad";
				break;
			case AnnouncementOperation.MarkAsSold:
				operationUri	+= "mark_as_sol";
				break;
			}
			
			operationUri			 += announcementUri.AbsolutePath.Replace(".html", "");
			
			return operationUri;
			
		}
		
		string PerformAnnouncementOperation(CookieContainer cookieContainer)
		{
			string operationUri			= BuildURIByOperation(_parameter.ManagementLink, _parameter.Operation ,_parameter.AnnouncementLink);
				
			Debug.Notify("ClasiBot.PerformAnnouncementOperation(): Operation URI is '", operationUri, "'");
			
			HttpWebRequest request		= (HttpWebRequest) WebRequest.Create(operationUri);
			
			request.CookieContainer		= cookieContainer;						
			request.UserAgent			= "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.1.9) Gecko/20100317 SUSE/3.5.9-0.1.1 Firefox/3.5.9\r\n";
			request.Referer				= "http://clasipar.paraguay.com/users/dashboard";
			
			return new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
		}

		
		string AccessDashboard(CookieContainer cookieContainer, string hashID, string authenticityToken)
		{
			HttpWebRequest request		= (HttpWebRequest) WebRequest.Create("http://clasipar.paraguay.com/users/hash_login");
			
			string postQuery 			= string.Format("authenticity_token={0}&email={1}&hash={2}", 
			                 				           	HttpUtility.UrlEncode(authenticityToken), 
			                            				HttpUtility.UrlEncode(_parameter.UserEmail), 
			                            				HttpUtility.UrlEncode(hashID));			
			
			byte[] postQueryBytes		= Encoding.UTF8.GetBytes(postQuery);
						
			request.Method				= "POST";			
			
			request.Accept				= "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
			request.UserAgent			= "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.1.9) Gecko/20100317 SUSE/3.5.9-0.1.1 Firefox/3.5.9\r\n";
			request.ContentType 		= "application/x-www-form-urlencoded";		
			request.CookieContainer		= cookieContainer;			
			request.Headers.Add("Accept-Language", "en-us,en;q=0.5");
			request.KeepAlive 			= true;
			request.Timeout 			= 6000;					
			request.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");					
			request.Referer				= _parameter.ManagementLink.ToString();			
			request.ContentLength 		= postQueryBytes.Length;
			
			using(Stream writer	 = request.GetRequestStream())			
				writer.Write(postQueryBytes, 0, postQueryBytes.Length);			
		
			using(StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
				return reader.ReadToEnd();
			
		}				
	}
}

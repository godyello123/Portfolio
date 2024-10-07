using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;

namespace SCommon
{
	public class SKakaoVerifyCert
	{
		private static string s_Host = "";
		private static string s_AppID = "";
		private static string s_AppSecret = "";
		private static string s_Authorization = "";

		public static void Init(string host, string appID, string appSecret, string authorization)
		{
			s_Host = host;
			s_AppID = appID;
			s_AppSecret = appSecret;
			s_Authorization = authorization;
		}

		public static bool Verify(string id, string accessString, ref string countryCode, ref string languageCode, ref string errorString)
		{
			try
			{
				WebRequest webRequest = WebRequest.Create(string.Format("{0}/zat/validate", s_Host));
				webRequest.Method = "Post";
				webRequest.ContentType = "application/json;charset=UTF-8";
				webRequest.Headers.Add(string.Format("appId: {0}", s_AppID));
				webRequest.Headers.Add(string.Format("appSecret: {0}", s_AppSecret));
				webRequest.Headers.Add(string.Format("playerId: {0}", id));
				webRequest.Headers.Add(string.Format("Authorization: {0}", s_Authorization));
				byte[] payload = Encoding.UTF8.GetBytes(string.Format("{{\"zat\": \"{0}\"}}", accessString));
				webRequest.ContentLength = payload.Length;
				using(Stream requestStream = webRequest.GetRequestStream())
				{
					requestStream.Write(payload, 0, payload.Length);
				}
				using(HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
				{
					if(webResponse.StatusCode != HttpStatusCode.OK)
					{
						errorString = string.Format("Kakao cert failed : {0}", webResponse.StatusCode);
						return false;
					}
					using(StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
					{
						var response = SJson.JsonToObject<Dictionary<string, object>>(reader.ReadToEnd());
						countryCode = (string)response["country"];
						languageCode = (string)response["lang"];
						return true;
					}
				}
			}
			catch(Exception e)
			{
				errorString = string.Format("Kakao cert failed : {0}", e.ToString());
				return false;
			}
		}
	}
}

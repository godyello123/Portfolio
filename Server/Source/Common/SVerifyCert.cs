using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace SCommon
{
	public class SAOSVerifyCert
	{
		private static string s_AuthCodeURL = "https://www.googleapis.com/oauth2/v4/token";
		private static string s_AppID = "";
		private static string s_AccessTokenURL = "";
		private static string s_ClientID = "";
		private static string s_ClientSecret = "";
		private static string s_RedirectURI = "";

		public static void Init(string appID, string clientID, string clientSecret)
		{
			s_AppID = appID;
			s_AccessTokenURL = string.Format("https://www.googleapis.com/games/v1/applications/{0}/verify/", s_AppID);
			s_ClientID = clientID;
			s_ClientSecret = clientSecret;
		}

		public static bool VerifyAuthCode(string id, string accessString, ref string certToken)
		{
			try
			{
				WebRequest webRequest = WebRequest.Create(s_AuthCodeURL);
				webRequest.Method = "Post";
				webRequest.ContentType = "application/x-www-form-urlencoded";
				string param = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type={4}",
					accessString, s_ClientID, s_ClientSecret, s_RedirectURI, "authorization_code");
				byte[] payload = Encoding.UTF8.GetBytes(param);
				webRequest.ContentLength = payload.Length;
				using(Stream requestStream = webRequest.GetRequestStream())
				{
					requestStream.Write(payload, 0, payload.Length);
				}
				using(WebResponse webResponse = webRequest.GetResponse())
				{
					using(StreamReader Reader = new StreamReader(webResponse.GetResponseStream()))
					{
						string response = Reader.ReadToEnd();
						var responseObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
						if(responseObj.ContainsKey("error")) return false;
						certToken = responseObj["access_token"];
					}
				}
			}
			catch
			{
				return false;
			}

			return VerifyAccessToken(id, certToken);
		}

		public static bool VerifyAccessToken(string id, string accessString)
		{
			try
			{
				WebRequest webRequest = WebRequest.Create(s_AccessTokenURL);
				webRequest.Method = "Get";
				webRequest.Headers.Add(string.Format("Authorization:OAuth {0}", accessString));
				using(WebResponse webResponse = webRequest.GetResponse())
				{
					using(StreamReader Reader = new StreamReader(webResponse.GetResponseStream()))
					{
						string response = Reader.ReadToEnd();
						var responseObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
						if(responseObj.ContainsKey("error")) return false;
						if(responseObj["player_id"] != id) return false;
					}
				}
			}
			catch
			{
				return false;
			}

			return true;
		}
	}

	public class SIOSVerifyCert
	{
		public static bool Verify(string id, string accessString)
		{
			try
			{
				var accessStringObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(accessString);
				string publicKeyUrl = accessStringObj["publicKeyUrl"];
				string signature = accessStringObj["signature"];
				string salt = accessStringObj["salt"];
				ulong timestamp = ulong.Parse(accessStringObj["timestamp"]);
				string bundleID = accessStringObj["bundleID"];

				var cert = GetCertificate(publicKeyUrl);
				if(cert.Verify())
				{
					var csp = (RSACryptoServiceProvider)cert.PublicKey.Key;
					if(csp != null)
					{
						var sha = new SHA256Managed();
						var sig = ConcatSignature(id, bundleID, timestamp, salt);
						var hash = sha.ComputeHash(sig);
						if(!csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), Convert.FromBase64String(signature))) return false;
					}
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		private static X509Certificate2 GetCertificate(string url)
		{
			var client = new WebClient();
			var rawData = client.DownloadData(url);
			return new X509Certificate2(rawData);
		}

		private static byte[] ConcatSignature(string playerID, string bundleID, ulong timestamp, string salt)
		{
			var data = new List<byte>();
			data.AddRange(Encoding.UTF8.GetBytes(playerID));
			data.AddRange(Encoding.UTF8.GetBytes(bundleID));
			data.AddRange(ToBigEndian(timestamp));
			data.AddRange(Convert.FromBase64String(salt));
			return data.ToArray();
		}

		private static byte[] ToBigEndian(ulong value)
		{
			var buffer = new byte[8];
			for(int i = 0; i < 8; i++)
			{
				buffer[7 - i] = unchecked((byte)(value & 0xff));
				value = value >> 8;
			}
			return buffer;
		}
	}
}

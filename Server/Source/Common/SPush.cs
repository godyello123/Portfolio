using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using Newtonsoft.Json;

namespace SCommon
{
	public class SPush
	{
		private class SAndroidPostData
		{
			public List<string> registration_ids;
			//public Dictionary<string, string> data = new Dictionary<string, string>();
			public Dictionary<string, string> notification = new Dictionary<string, string>();
			public int time_to_live = 600;
		}
		public static string SendAndroid(string serverAPIKey, List<string> regIDList, string title, string message)
		{
			try
			{
				SAndroidPostData postData = new SAndroidPostData();
				postData.registration_ids = regIDList;
				//postData.data.Add("title", title);
				//postData.data.Add("body", message);
				postData.notification.Add("title", title);
				postData.notification.Add("body", message);
				postData.notification.Add("sound", "default");

				string ret = JsonConvert.SerializeObject(postData);

				byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postData));
				HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
				Request.Method = "POST";
				Request.KeepAlive = false;
				Request.ContentType = "application/json";
				Request.Headers.Add(string.Format("Authorization: key={0}", serverAPIKey));
				Request.ContentLength = byteArray.Length;
				using(Stream requestStream = Request.GetRequestStream())
				{
					requestStream.Write(byteArray, 0, byteArray.Length);
				}

				using(WebResponse Response = Request.GetResponse())
				{
					using(StreamReader Reader = new StreamReader(Response.GetResponseStream()))
					{
						string responseLine = Reader.ReadToEnd();
						return responseLine;
					}
				}	
			}
			catch(Exception e)
			{
				return e.ToString();
			}
		}
		private class SAndroidTopicPostData
		{
			public string to;
			//public Dictionary<string, string> data = new Dictionary<string, string>();
			public Dictionary<string, string> notification = new Dictionary<string, string>();
			public int time_to_live = 600;
		}
		public static string SendAndroidTopic(string serverAPIKey, string topic, string title, string message)
		{
			try
			{
				SAndroidTopicPostData postData = new SAndroidTopicPostData();
				postData.to = string.Format("/topics/{0}", topic);
				//postData.data.Add("title", title);
				//postData.data.Add("body", message);
				postData.notification.Add("title", title);
				postData.notification.Add("body", message);
				postData.notification.Add("sound", "default");

				string ret = JsonConvert.SerializeObject(postData);

				byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postData));
				HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
				Request.Method = "POST";
				Request.KeepAlive = false;
				Request.ContentType = "application/json";
				Request.Headers.Add(string.Format("Authorization: key={0}", serverAPIKey));
				Request.ContentLength = byteArray.Length;
				using(Stream requestStream = Request.GetRequestStream())
				{
					requestStream.Write(byteArray, 0, byteArray.Length);
				}

				using(WebResponse Response = Request.GetResponse())
				{
					using(StreamReader Reader = new StreamReader(Response.GetResponseStream()))
					{
						string responseLine = Reader.ReadToEnd();
						return responseLine;
					}
				}
			}
			catch(Exception e)
			{
				return e.ToString();
			}
		}

		private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}
		private static byte[] HexStringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
		}
		private static string EncodeMessage(string Message)
		{
			StringBuilder encodedString = new StringBuilder();
			foreach(char c in Message)
			{
				if((int)c >= 32 && (int)c <= 127) encodedString.Append(c);
				else encodedString.Append("\\u" + string.Format("{0:x4}", Convert.ToUInt32(c)));
			}
			return encodedString.ToString();
		}
		public static string SendApple(string certificatePath, string password, List<string> deviceIDList, string message, bool sandbox)
		{
			try
			{
				int port = 2195;
				string hostname = sandbox ? "gateway.sandbox.push.apple.com" : "gateway.push.apple.com";
				X509Certificate2 clientCertificate = new X509Certificate2(certificatePath, password);
				X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

				using(TcpClient client = new TcpClient(hostname, port))
				{
					using(SslStream sslStream = new SslStream(client.GetStream(), false,
						new RemoteCertificateValidationCallback(ValidateServerCertificate), null))
					{
						sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Tls, true);

						foreach(var deviceID in deviceIDList)
						{
							MemoryStream memoryStream = new MemoryStream();
							using(BinaryWriter writer = new BinaryWriter(memoryStream))
							{
								writer.Write((byte)0);
								writer.Write((byte)0);
								writer.Write((byte)32);
								writer.Write(HexStringToByteArray(deviceID.ToUpper()));
								string payload = "{\"aps\":{\"alert\":\"" + EncodeMessage(message) + "\",\"badge\":1,\"sound\":\"default\"}}";
								writer.Write((byte)0);
								writer.Write((byte)payload.Length);
								writer.Write(Encoding.UTF8.GetBytes(payload));
								writer.Flush();
								byte[] array = memoryStream.ToArray();
								sslStream.Write(array);
							}
						}

						sslStream.Flush();
					}
				}
			}
			catch(Exception e)
			{
				return e.ToString();
			}

			return "Success";
		}
	}
}

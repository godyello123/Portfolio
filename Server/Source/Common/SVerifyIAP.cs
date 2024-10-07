//using System;
//using System.IO;
//using System.Text;
//using System.Net;
//using System.Collections.Generic;
//using System.Security.Cryptography;
//using Org.BouncyCastle.Security;
//using Org.BouncyCastle.Crypto.Parameters;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace SCommon
//{
//	public class SAOSVerifyIAP
//	{
//		private RSAParameters m_RSAKeyInfo;

//		public SAOSVerifyIAP(string publicKey)
//		{
//			RsaKeyParameters rsaParameters = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
//			byte[] modulus = rsaParameters.Modulus.ToByteArray();
//			byte[] rsaExponent = rsaParameters.Exponent.ToByteArray();

//			int pos = 0;
//			for(int i = 0; i < modulus.Length; i++)
//			{
//				if(modulus[i] == 0) pos++;
//				else break;
//			}
//			byte[] rsaModulus = new byte[modulus.Length - pos];
//			Array.Copy(modulus, pos, rsaModulus, 0, modulus.Length - pos);

//			m_RSAKeyInfo = new RSAParameters();
//			m_RSAKeyInfo.Modulus = rsaModulus;
//			m_RSAKeyInfo.Exponent = rsaExponent;
//		}

//		public bool Verify(string message, string signature)
//		{
//			using(RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
//			{
//				rsa.ImportParameters(m_RSAKeyInfo);
//				return rsa.VerifyData(Encoding.ASCII.GetBytes(message), "SHA1", Convert.FromBase64String(signature));
//			}
//		}
//	}

//	public class SIOSVerifyIAP
//	{
//		public bool Verify(string transactionID, string receiptData, ref string productID, ref long purchaseTime)
//		{
//			const string URL_SERVICE = "https://buy.itunes.apple.com/verifyReceipt";
//			const string URL_SANDBOX = "https://sandbox.itunes.apple.com/verifyReceipt";

//			if(Verify(URL_SERVICE, transactionID, receiptData, ref productID, ref purchaseTime)) return true;
//			if(Verify(URL_SANDBOX, transactionID, receiptData, ref productID, ref purchaseTime)) return true;

//			return false;
//		}

//		private bool Verify(string url, string transactionID, string receiptData, ref string productID, ref long purchaseTime)
//		{
//			string response = PostRequest(url, ConvertReceiptToPost(receiptData));
//			if(string.IsNullOrEmpty(response)) return false;

//			var responseObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
//			if(!responseObject.ContainsKey("receipt")) return false;

//			var receiptObject = (JObject)responseObject["receipt"];
//			var inAppObject = (JArray)receiptObject["in_app"];
//			foreach(var data in inAppObject)
//			{
//				var dataObject = (JObject)data;
//				if((string)dataObject["transaction_id"] == transactionID)
//				{
//					productID = (string)dataObject["product_id"];
//					purchaseTime = long.Parse((string)dataObject["purchase_date_ms"]);
//					break;
//				}
//			}

//			return true;
//		}

//		private byte[] ConvertAppStoreTokenToBytes(string token)
//		{
//			token = token.Replace(" ", "");
//			int i = 0;
//			int b = 0;
//			List<byte> buff = new List<byte>();
//			while(i < token.Length)
//			{
//				buff.Add(Convert.ToByte(token.Substring(i, 2), 16));
//				i += 2;
//				b++;
//			}

//			return buff.ToArray();
//		}
//		private string ConvertReceiptToPost(string receipt)
//		{
//			return string.Format(@"{{""receipt-data"":""{0}""}}", receipt);
//		}
//		private string PostRequest(string url, string postData)
//		{
//			try
//			{
//				byte[] buff = Encoding.UTF8.GetBytes(postData);

//				WebRequest request = WebRequest.Create(url);
//				request.Method = "POST";
//				request.ContentType = "text/plain";
//				request.ContentLength = buff.Length;

//				using(Stream requestStream = request.GetRequestStream())
//				{
//					requestStream.Write(buff, 0, buff.Length);
//				}

//				using(WebResponse response = request.GetResponse())
//				{
//					using(StreamReader streamReader = new StreamReader(response.GetResponseStream()))
//					{
//						return streamReader.ReadToEnd();
//					}
//				}
//			}
//			catch
//			{
//				return null;
//			}
//		}
//	}
//}

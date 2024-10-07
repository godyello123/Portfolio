using System;
using System.Xml;
using SCommon;

namespace Global
{
	public class CAccountConfig : SSingleton<CAccountConfig>
	{
		public string APNSFile { get; set; }
		public string APNSPassword { get; set; }

		public string GooglePublicKey = "";
		public string GoogleServerAPIKey = "";
		public string GoogleAppID = "";
		public string GoogleClientID = "";
		public string GoogleClientSecret = "";

		public string AzureAccountName { get; set; }
		public string AzureAccountKey { get; set; }
		public string AzureTenantID { get; set; }
		public string AzureTenantDomain { get; set; }
		public string AzureClientID { get; set; }
		public string AzureClientSecret { get; set; }
		public string AzureSubscriptionID { get; set; }
		public string AzureResourceGroupName { get; set; }
		public string AzureProfileName { get; set; }
		public string AzureEndpointName { get; set; }

		public string KakaoHost { get; set; }
		public string KakaoAppID { get; set; }
		public string KakaoAppSecret { get; set; }
		public string KakaoAuthorization { get; set; }

		public bool Load(string path)
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(path);

				XmlElement root = doc.DocumentElement;

				XmlElement apple = (XmlElement)root.GetElementsByTagName("Apple")[0];
				APNSFile = apple.GetAttribute("apns_file");
				APNSPassword = apple.GetAttribute("apns_password");

				XmlElement google = (XmlElement)root.GetElementsByTagName("Google")[0];
				GooglePublicKey = google.GetAttribute("public_key");
				GoogleServerAPIKey = google.GetAttribute("server_api_key");
				GoogleAppID = google.GetAttribute("app_id");
				GoogleClientID = google.GetAttribute("client_id");
				GoogleClientSecret = google.GetAttribute("client_secret");

				XmlElement azure = (XmlElement)root.GetElementsByTagName("Azure")[0];
				AzureAccountName = azure.GetAttribute("account_name");
				AzureAccountKey = azure.GetAttribute("account_key");
				AzureTenantID = azure.GetAttribute("tenant_id");
				AzureTenantDomain = azure.GetAttribute("tenant_domain");
				AzureClientID = azure.GetAttribute("client_id");
				AzureClientSecret = azure.GetAttribute("client_secret");
				AzureSubscriptionID = azure.GetAttribute("subscription_id");
				AzureResourceGroupName = azure.GetAttribute("resource_group_name");
				AzureProfileName = azure.GetAttribute("profile_name");
				AzureEndpointName = azure.GetAttribute("endpoint_name");

				XmlElement kakao = (XmlElement)root.GetElementsByTagName("Kakao")[0];
				KakaoHost = kakao.GetAttribute("host");
				KakaoAppID = kakao.GetAttribute("app_id");
				KakaoAppSecret = kakao.GetAttribute("app_secret");
				KakaoAuthorization = kakao.GetAttribute("authorization");

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}

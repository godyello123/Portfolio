using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Cdn;
using Microsoft.Azure.Management.Cdn.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

namespace SCommon
{
	public class SAzureCDN
	{
		public static CdnManagementClient GetCDN(string tenantID,
			string tenantDomain, string clientID, string clientSecret, string subscriptionID)
		{
			string authority = string.Format("https://login.microsoftonline.com/{0}/{1}", tenantID, tenantDomain);
			AuthenticationContext authContext = new AuthenticationContext(authority);
			ClientCredential credential = new ClientCredential(clientID, clientSecret);
			AuthenticationResult authResult = authContext.AcquireTokenAsync("https://management.core.windows.net/", credential).Result;
			return new CdnManagementClient(new TokenCredentials(authResult.AccessToken)) { SubscriptionId = subscriptionID };
		}

		public static Task PurgeAsync(string tenantID, string tenantDomain, string clientID, string clientSecret,
			string subscriptionID, string resourceGroupName, string profileName, string endPointName, List<string> pathList)
		{
			var cdn = GetCDN(tenantID, tenantDomain, clientID, clientSecret, subscriptionID);
			return cdn.Endpoints.PurgeContentAsync(resourceGroupName, profileName, endPointName, pathList);
		}
	}
}

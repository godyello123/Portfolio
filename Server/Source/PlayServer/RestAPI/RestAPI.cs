using System;
using System.Linq;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using Google.Apis.Auth.OAuth2;
using Google.Apis.AndroidPublisher.v3;
using PlayServer;
using SDB;


namespace Global.RestAPI
{
    public class IRestQuery
    {
        protected Packet_Result.Result m_Result = Packet_Result.Result.Success;
        public void Excute()
        {
            try
            {
                Run();
            }
            catch (Exception e)
            {
                m_Result = Packet_Result.Result.SystemError;
                CommonLogger.Error($"Error : {e.ToString()}");
            }
        }
        virtual public void Run() { }
        virtual public void Complete() { }

        protected bool IsValid() { return m_Result == Packet_Result.Result.Success; }
    }

    public class CRestIAP : IRestQuery
    {
        enum EIOSStatusCode
        {
            Valid = 0,
            Test_ShouldRetrySandbox = 21007,
            Max,
        }

        //input
        private long m_Session = -1;
        private long m_AccountID = -1;
        private string m_StrReceipt = "";
        private LogBson m_Log;

        //
        private CDefine.eStoreType m_StoreType = CDefine.eStoreType.Max;
        private string m_Payload = "";
        private string m_TransactionID = "";
        private string m_PackageName;
        private string m_PurchaseToken;
        private string m_OrderID;
        private string m_ProductID;
        private _IAPReceipt m_IAPReceipt = new _IAPReceipt();

        public CRestIAP(long session, long accountID, string receipt, LogBson log)
        {
            m_Session = session;
            m_AccountID = accountID;
            m_StrReceipt = receipt;
            m_Log = log;
        }

        private Packet_Result.Result Verify()
        {
            var json = JObject.Parse(m_StrReceipt);

            var strStoreType = json.Value<string>("Store");
            if (string.IsNullOrEmpty(strStoreType))
            {
                CLogger.Instance.System($"Verify() 1");
                return Packet_Result.Result.SystemError;
            }

            m_Payload = json.Value<string>("Payload");
            if (string.IsNullOrEmpty(m_Payload))
            {
                CLogger.Instance.System($"Verify() 2");
                return Packet_Result.Result.SystemError;
            }

            m_TransactionID = json.Value<string>("TransactionID");
            if (string.IsNullOrEmpty(m_TransactionID))
            {
                CLogger.Instance.System($"Verify() 3");
                return Packet_Result.Result.SystemError;
            }
            
            if (Enum.TryParse(strStoreType, out m_StoreType) == false)
            {
                CLogger.Instance.System($"Verify() 4");
                m_StoreType = CDefine.eStoreType.Max;
                return Packet_Result.Result.SystemError;
            }

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result GooglePlayInit()
        {
            if (CConfig.Instance.GetGooglePlayClaim() == null)
            {
                CLogger.Instance.Error($"GooglePlayInit() Error");
                return Packet_Result.Result.SystemError;
            }

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result GooglePlayParsePayload()
        {
            var jsonPayload = JObject.Parse(m_Payload);
            if (jsonPayload == null)
            {
                CLogger.Instance.Error($"GooglePlayParsePayload() 1");
                return Packet_Result.Result.PacketError;
            }

            string strJsonInPayload = jsonPayload.Value<string>("json");
            if (string.IsNullOrEmpty(strJsonInPayload))
            {
                CLogger.Instance.Error($"GooglePlayParsePayload() 2");
                return Packet_Result.Result.PacketError;
            }

            var jsonInPayload = JObject.Parse(strJsonInPayload);
            if (jsonInPayload == null)
            {
                CLogger.Instance.Error($"GooglePlayParsePayload() 3");
                return Packet_Result.Result.PacketError;
            }
            
            m_PackageName = jsonInPayload.Value<string>("packageName");
            m_PurchaseToken = jsonInPayload.Value<string>("purchaseToken");
            m_OrderID = jsonInPayload.Value<string>("orderId");
            m_ProductID = jsonInPayload.Value<string>("productId");

            if (string.IsNullOrEmpty(m_PackageName) ||
                string.IsNullOrEmpty(m_PurchaseToken) ||
                string.IsNullOrEmpty(m_OrderID) ||
                string.IsNullOrEmpty(m_ProductID))
            {
                CLogger.Instance.Error($"GooglePlayParsePayload() 4");
                return Packet_Result.Result.PacketError;
            }

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result GooglePlayAccessToken()
        {
            var googlePlayClaim = CConfig.Instance.GetGooglePlayClaim();
            if (googlePlayClaim == null)
            {
                CLogger.Instance.Error($"GooglePlayAccessToken() 1");
                return Packet_Result.Result.SystemError;
            }

            //var credential = GoogleCredential.FromFile(CConfig.Instance.GoogleIAPFilePath);
            //var certificate = new X509Certificate2(CConfig.Instance.GoogleIAPFilePath, "notasecret");
            var serviceAccountCredential = new ServiceAccountCredential
            (
                new ServiceAccountCredential.Initializer(googlePlayClaim.issuer)
                {
                    Scopes = new[] { AndroidPublisherService.Scope.Androidpublisher },

                }//.FromCertificate(certificate)
                .FromPrivateKey(googlePlayClaim.privatekey)
            );

            AndroidPublisherService aosService = new AndroidPublisherService
            (
                new Google.Apis.Services.BaseClientService.Initializer
                {
                    HttpClientInitializer = serviceAccountCredential,
                    ApplicationName = m_PackageName
                }
            );

            var request = aosService.Purchases.Products.Get(m_PackageName, m_ProductID, m_PurchaseToken);
            var purchaseState = request.Execute();

            //if (purchaseState.AcknowledgementState != 0)
            //    return Packet_Result.Result.IAPFailedAcknowledgementState;

            if (purchaseState.PurchaseState == 1 || purchaseState.PurchaseState == 2)
            {
                CLogger.Instance.Error($"GooglePlayAccessToken() 1");
                return Packet_Result.Result.PacketError;
            }

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result GooglePlayVerify()
        {
            return Packet_Result.Result.Success;
        }

        private void CreateIAPReceipt()
        {
            m_IAPReceipt.m_StoreType = m_StoreType;
            m_IAPReceipt.m_OrderID = m_OrderID;
            m_IAPReceipt.m_ProductID = m_ProductID;
        }

        public override void Run()
        {
            m_Result = Verify();
            if (!IsValid())
            {
                CLogger.Instance.Error($"IAP Verify failed [pc_id : {m_AccountID}] [Error code : {m_Result}]");
                return;
            }

            switch (m_StoreType)
            {
                case CDefine.eStoreType.GooglePlay:
                    m_Result = GooglePlayInit();
                    if (!IsValid())
                    {
                        m_Log.LogStr = "verify";
                        return;
                    }

                    m_Result = GooglePlayParsePayload();
                    if (!IsValid()) return;

                    m_Result = GooglePlayAccessToken();
                    if (!IsValid()) return;

                    m_Result = GooglePlayVerify();
                    if (!IsValid()) return;

                    CreateIAPReceipt();
                    break;

                case CDefine.eStoreType.AppleAppStore:
                    m_Result = IOSVerify();
                    if (!IsValid()) return;

                    CreateIAPReceipt();
                    break;

                default:
                    m_Result = Packet_Result.Result.SystemError;
                    break;
            }   
        }

        private Packet_Result.Result IOSVerify()
        {
            const string productionUri = @"https://buy.itunes.apple.com/verifyReceipt";
            const string sandboxUri = @"https://sandbox.itunes.apple.com/verifyReceipt";

            var body = new JObject(new JProperty("receipt-data", m_Payload)).ToString();

            JObject jsonRes;
            if (!IOSVerify(productionUri, body, out jsonRes))
            {
                CLogger.Instance.Error($"IOSVerify() 1");
                return Packet_Result.Result.PacketError;
            }

            if (jsonRes == null)
            {
                CLogger.Instance.Error($"IOSVerify() 1");
                return Packet_Result.Result.PacketError;
            }

            int status = jsonRes.Value<int>("status");
            if (status == (int)EIOSStatusCode.Test_ShouldRetrySandbox)
            {
                IOSVerify(sandboxUri, body, out jsonRes);

                if (jsonRes == null)
                {
                    CLogger.Instance.Error($"IOSVerify() 1");
                    return Packet_Result.Result.PacketError;
                }

                status = jsonRes.Value<int>("status");
            }

            if (status != (int)EIOSStatusCode.Valid)
            {
                CLogger.Instance.Error($"IOSVerify() 1");
                return Packet_Result.Result.PacketError;
            }

            var jsonReceipt = jsonRes.Value<JObject>("receipt");
            if (jsonReceipt == null)
            {
                CLogger.Instance.Error($"IOSVerify() 1");
                return Packet_Result.Result.PacketError;

            }

            var jsonArrInApp = jsonReceipt.Value<JArray>("in_app");
            if (jsonArrInApp == null)
            {
                CLogger.Instance.Error($"IOSVerify() 1");
                return Packet_Result.Result.PacketError;

            }

            foreach (var it in jsonArrInApp)
            {
                var tranID = it.Value<string>("transaction_id");
                if (m_TransactionID == tranID)
                {
                    m_OrderID = m_TransactionID;
                    m_ProductID = it.Value<string>("product_id");
                    break;
                }
            }

            return Packet_Result.Result.Success;
        }

        private bool IOSVerify(string uri, string body, out JObject rOut)
        {
            using (var restClient = new RestClient(uri))
            {
                var restRequest = new RestRequest(uri, RestSharp.Method.Post);
                restRequest.Method = RestSharp.Method.Post;
                restRequest.RequestFormat = DataFormat.Json;
                restRequest.AddHeader("Content-Type", "application/json");
                restRequest.AddBody(body, "application/json");

                var task = restClient.PostAsync(restRequest);
                task.Wait();

                var response = task.Result;

                rOut = JObject.Parse(response.Content);
            }

            if (rOut == null)
            {
                CLogger.Instance.Error($"IOSVerify(string uri, string body, out JObject rOut) error1");
                return false;
            }

            return true;
        }

        public override void Complete()
        {           
            var user = CUserManager.Instance.FindbyUID(m_AccountID);
            if (user == null) return;

            if (IsValid())
            {
                string error_str = string.Empty;
                if (false == user.ShopAgent.AfterRestIAPTry(m_IAPReceipt,ref error_str))
                {
                    //todo : log
                    var log = LogHelper.MakeLogBson(eLogType.iap_fail, user.UserData, SCommon.SJson.ObjectToJson(m_IAPReceipt), null, 1);
                    CGameLog.Instance.Insert(log);

                    //CNetManager.Instance.P2L_ReportLogging(log);
                }
            }
            else
            {
                CNetManager.Instance.P2C_ResultIAPTry(user.SessionKey, new _ShopData(), "", m_Result);

                //todo : log
                m_Log.Type = (ushort)eLogType.iap_fail;
                m_Log.LogStr = "system";
                CGameLog.Instance.Insert(m_Log);
                //CNetManager.Instance.P2L_ReportLogging(m_Log);
            }
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using Google.Apis.Auth.OAuth2;
using Google.Apis.AndroidPublisher.v3;
using FirebaseAdmin.Auth;
using GateServer;

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
                CLogger.Instance.Error(e.ToString());
            }
        }
        virtual public void Run() { }
        virtual public void Complete() { }

        protected bool IsValid() { return m_Result == Packet_Result.Result.Success; }
    }

    public class CRestAuth : IRestQuery
    {
        //input
        private long m_Session = -1;
        private int m_ServetKey = -1;

        //output
        private string m_DeviceID = "";
        private string m_Token;

        public CRestAuth(long session, int serverkey, string token)
        {
            m_Session = session;
            m_ServetKey = serverkey;
            m_Token = token;
        }

        public override void Run()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(m_Token);

            if (jwtSecurityToken == null)
            {
                m_Result = Packet_Result.Result.SystemError;
                return;
            }

            var aud = jwtSecurityToken.Claims.First(clime => clime.Type == "aud");
            if (aud.Value != CConfig.Instance.FireBaseAudience)
            {
                m_Result = Packet_Result.Result.SystemError;
                return;
            }

            var userID = jwtSecurityToken.Claims.First(clime => clime.Type == "user_id");

            m_DeviceID = userID.Value;

            if (string.IsNullOrEmpty(m_DeviceID))
            {
                m_Result = Packet_Result.Result.SystemError;
            }
        }

        public override void Complete()
        {
            if (!CNetManager.Instance.IsAliveSession(m_Session))
                return;

            if (IsValid())
                CDBManager.Instance.QueryAccountAuth(m_Session, m_ServetKey, m_DeviceID, CDefine.AuthType.Firebase);
            else
                CNetManager.Instance.G2C_ResultAuth(m_ServetKey, (ushort)m_Result, -1, "", (ushort)0);
        }
    }


    //public class CRestAuth : IRestQuery
    //{
    //    //input
    //    private long m_Session = -1;
    //    private int m_ServetKey = -1;

    //    //output
    //    private string m_DeviceID = "";
    //    private string m_Token;

    //    public CRestAuth(long session, string token)
    //    {
    //        m_Session = session;
    //        m_Token = token;
    //    }

    //    public override void Run()
    //    {
    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var jwtSecurityToken = tokenHandler.ReadJwtToken(m_Token);

    //        if (jwtSecurityToken == null)
    //        {
    //            m_Result = Packet_Result.Result.SystemError;
    //            return;
    //        }

    //        var aud = jwtSecurityToken.Claims.First(clime => clime.Type == "aud");
    //        if (aud.Value != CConfig.Instance.FireBaseAudience)
    //        {
    //            m_Result = Packet_Result.Result.SystemError;
    //            return;
    //        }

    //        var userID = jwtSecurityToken.Claims.First(clime => clime.Type == "user_id");

    //        m_DeviceID = userID.Value;

    //        if (string.IsNullOrEmpty(m_DeviceID))
    //        {
    //            m_Result = Packet_Result.Result.SystemError;
    //        }
    //    }

    //    public override void Complete()
    //    {
    //        if (!CNetManager.Instance.IsAliveSession(m_Session))
    //            return;

    //        if (IsValid())
    //            CDBManager.Instance.QueryAccountAuth(m_Session, m_ServetKey, m_DeviceID);
    //        else
    //            CNetManager.Instance.G2C_ResultAuth(m_ServetKey, (ushort)m_Result, -1, "", -1);
    //    }
    //}

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
        //private LogBase m_Log;

        //
        //private eStoreType m_StoreType = eStoreType.Max;
        private string m_Payload = "";
        private string m_TransactionID = "";
        private string m_PackageName;
        private string m_PurchaseToken;
        private string m_OrderID;
        private string m_ProductID;
        //private IAPReceipt m_IAPReceipt = new IAPReceipt();

        //public CRestIAP(long session, long accountID, string receipt, LogBase log)
        //{
        //    m_Session = session;
        //    m_AccountID = accountID;
        //    m_StrReceipt = receipt;
        //    m_Log = log;
        //}

        private Packet_Result.Result Verify()
        {
            var json = JObject.Parse(m_StrReceipt);

            var strStoreType = json.Value<string>("Store");
            if (string.IsNullOrEmpty(strStoreType))
                return Packet_Result.Result.SystemError;

            m_Payload = json.Value<string>("Payload");
            if (string.IsNullOrEmpty(m_Payload))
                return Packet_Result.Result.SystemError;

            m_TransactionID = json.Value<string>("TransactionID");
            if (string.IsNullOrEmpty(m_TransactionID))
                return Packet_Result.Result.SystemError;

            //if (Enum.TryParse(strStoreType, out m_StoreType) == false)
            //{
            //    m_StoreType = eStoreType.Max;
            //    return Packet_Result.Result.SystemError;
            //}

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result GooglePlayInit()
        {
            //if (CConfig.Instance.GetGooglePlayClaim() == null)
            //    return Packet_Result.Result.SystemError;

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result GooglePlayParsePayload()
        {
            var jsonPayload = JObject.Parse(m_Payload);
            if (jsonPayload == null)
                return Packet_Result.Result.PacketError;

            string strJsonInPayload = jsonPayload.Value<string>("json");
            if (string.IsNullOrEmpty(strJsonInPayload))
                return Packet_Result.Result.PacketError;

            var jsonInPayload = JObject.Parse(strJsonInPayload);
            if (jsonInPayload == null)
                return Packet_Result.Result.PacketError;

            m_PackageName = jsonInPayload.Value<string>("packageName");
            m_PurchaseToken = jsonInPayload.Value<string>("purchaseToken");
            m_OrderID = jsonInPayload.Value<string>("orderId");
            m_ProductID = jsonInPayload.Value<string>("productId");

            if (string.IsNullOrEmpty(m_PackageName) ||
                string.IsNullOrEmpty(m_PurchaseToken) ||
                string.IsNullOrEmpty(m_OrderID) ||
                string.IsNullOrEmpty(m_ProductID))
            {
                return Packet_Result.Result.PacketError;
            }

            return Packet_Result.Result.Success;
        }

        //private Packet_Result.Result GooglePlayAccessToken()
        //{
        //    var googlePlayClaim = CConfig.Instance.GetGooglePlayClaim();
        //    if (googlePlayClaim == null)
        //        return Packet_Result.Result.SystemError;

        //    //var credential = GoogleCredential.FromFile(CConfig.Instance.GoogleIAPFilePath);
        //    //var certificate = new X509Certificate2(CConfig.Instance.GoogleIAPFilePath, "notasecret");
        //    var serviceAccountCredential = new ServiceAccountCredential
        //    (
        //        new ServiceAccountCredential.Initializer(googlePlayClaim.issuer)
        //        {
        //            Scopes = new[] { AndroidPublisherService.Scope.Androidpublisher },

        //        }//.FromCertificate(certificate)
        //        .FromPrivateKey(googlePlayClaim.privatekey)
        //    );

        //    AndroidPublisherService aosService = new AndroidPublisherService
        //    (
        //        new Google.Apis.Services.BaseClientService.Initializer
        //        {
        //            HttpClientInitializer = serviceAccountCredential,
        //            ApplicationName = m_PackageName
        //        }
        //    );

        //    var request = aosService.Purchases.Products.Get(m_PackageName, m_ProductID, m_PurchaseToken);
        //    var purchaseState = request.Execute();

        //    //if (purchaseState.AcknowledgementState != 0)
        //    //    return Packet_Result.Result.IAPFailedAcknowledgementState;

        //    if (purchaseState.PurchaseState == 1 || purchaseState.PurchaseState == 2)
        //        return Packet_Result.Result.PacketError;

        //    return Packet_Result.Result.Success;
        //}

        //private Packet_Result.Result GooglePlayVerify()
        //{
        //    return Packet_Result.Result.Success;
        //}

        //private void CreateIAPReceipt()
        //{
        //    //m_IAPReceipt.m_StoreType = m_StoreType;
        //    //m_IAPReceipt.m_OrderID = m_OrderID;
        //    //m_IAPReceipt.m_ProductID = m_ProductID;
        //}

        //public override void Run()
        //{
        //    //m_Result = Verify();
        //    //if (!IsValid())
        //    //{
        //    //    CLogger.Instance.Error($"IAP Verify failed [pc_id : {m_AccountID}] [Error code : {m_Result}]");
        //    //    return;
        //    //}

        //    //switch (m_StoreType)
        //    //{
        //    //    case eStoreType.GooglePlay:
        //    //        m_Result = GooglePlayInit();
        //    //        if (!IsValid())
        //    //        {
        //    //            m_Log.SetTargetObj("verify", -1, "");
        //    //            return;
        //    //        }

        //    //        m_Result = GooglePlayParsePayload();
        //    //        if (!IsValid()) return;

        //    //        m_Result = GooglePlayAccessToken();
        //    //        if (!IsValid()) return;

        //    //        m_Result = GooglePlayVerify();
        //    //        if (!IsValid()) return;

        //    //        CreateIAPReceipt();
        //    //        break;

        //    //    case eStoreType.AppleAppStore:
        //    //        m_Result = IOSVerify();
        //    //        if (!IsValid()) return;

        //    //        CreateIAPReceipt();
        //    //        break;

        //    //    default:
        //    //        m_Result = Packet_Result.Result.SystemError;
        //    //        break;
        //    //}   
        //}

        //private Packet_Result.Result IOSVerify()
        //{
        //    const string productionUri = @"https://buy.itunes.apple.com/verifyReceipt";
        //    const string sandboxUri = @"https://sandbox.itunes.apple.com/verifyReceipt";

        //    var body = new JObject(new JProperty("receipt-data", m_Payload)).ToString();

        //    JObject jsonRes;
        //    if (!IOSVerify(productionUri, body, out jsonRes))
        //        return Packet_Result.Result.PacketError;

        //    if (jsonRes == null)
        //        return Packet_Result.Result.PacketError;

        //    int status = jsonRes.Value<int>("status");
        //    if (status == (int)EIOSStatusCode.Test_ShouldRetrySandbox)
        //    {
        //        IOSVerify(sandboxUri, body, out jsonRes);

        //        if (jsonRes == null)
        //            return Packet_Result.Result.PacketError;

        //        status = jsonRes.Value<int>("status");
        //    }

        //    if (status != (int)EIOSStatusCode.Valid)
        //        return Packet_Result.Result.PacketError;

        //    var jsonReceipt = jsonRes.Value<JObject>("receipt");
        //    if (jsonReceipt == null)
        //        return Packet_Result.Result.PacketError;

        //    var jsonArrInApp = jsonReceipt.Value<JArray>("in_app");
        //    if (jsonArrInApp == null)
        //        return Packet_Result.Result.PacketError;

        //    foreach (var it in jsonArrInApp)
        //    {
        //        var tranID = it.Value<string>("transaction_id");
        //        if (m_TransactionID == tranID)
        //        {
        //            m_OrderID = m_TransactionID;
        //            m_ProductID = it.Value<string>("product_id");
        //            break;
        //        }
        //    }

        //    return Packet_Result.Result.Success;
        //}

        //private bool IOSVerify(string uri, string body, out JObject rOut)
        //{
        //    using (var restClient = new RestClient(uri))
        //    {
        //        var restRequest = new RestRequest(uri, RestSharp.Method.Post);
        //        restRequest.Method = RestSharp.Method.Post;
        //        restRequest.RequestFormat = DataFormat.Json;
        //        restRequest.AddHeader("Content-Type", "application/json");
        //        restRequest.AddBody(body, "application/json");

        //        var task = restClient.PostAsync(restRequest);
        //        task.Wait();

        //        var response = task.Result;

        //        rOut = JObject.Parse(response.Content);
        //    }

        //    if (rOut == null)
        //        return false;

        //    return true;
        //}

        public override void Complete()
        {
            //var user = CUserManager.Instance.FindPlayer(m_AccountID);
            //if (user == null) return;

            //if (IsValid())
            //{
            //    string error_str = string.Empty;
            //    if (false == user.ShopAgent.AfterRestIAPTry(m_IAPReceipt,ref error_str))
            //    {
            //        var log = LogHelper.PrepareLog(eLog.iap_fail, user, null, 1);
            //        log.SetTargetObj($"game", -1, $"failed [{m_Result.ToString()}, {error_str}]");
            //        log.SetSubStr(SCommon.SJson.ObjectToJson(m_IAPReceipt));
            //        CNetManager.Instance.P2L_ReportLogging(log);
            //    }
            //}
            //else
            //{
            //    CNetManager.Instance.P2C_ResultIAPTry(user.SessionKey, m_Result, new _ShopData(), "");

            //    m_Log.log_type = (int)eLog.iap_fail;
            //    m_Log.SetTargetObj($"system", -1, $"failed [{m_Result.ToString()}]");
            //    CNetManager.Instance.P2L_ReportLogging(m_Log);
            //}
        }
    }

    //public class CCheatGachaSimulation : IRestQuery
    //{
    //    //귀차낭
    //    //input
    //    private long m_Session = -1;
    //    private eGachaType m_GachaType = eGachaType.Max;
    //    private int m_Lv = 0;
    //    private int m_TestCase = 0;
    //    private CRewardInfo m_RewardInfo = new CRewardInfo();
    //    private _PostData m_TestResult = new _PostData();
    //    public CCheatGachaSimulation(long session, eGachaType type, int lv, int testCase)
    //    {
    //        m_Session = session;
    //        m_GachaType = type;
    //        m_Lv = lv;
    //        m_TestCase = testCase;

    //        m_TestResult.m_UID = CDefine.GeneraterGUID();
    //        m_TestResult.m_Title = "gacha simulation";
    //        m_TestResult.m_Msg = string.Format($"test case : {m_TestCase}");
    //        m_TestResult.m_PostType = ePostType.Post_User;
    //        m_TestResult.m_EndDate = DateTime.UtcNow.AddDays(100);
    //    }


    //    public override void Run()
    //    {
    //        var gachaRecord = CGachaLevelUpTable.Instance.Find(m_GachaType, m_Lv);
    //        if (gachaRecord == null)
    //            return;

    //        var dropRecord = CDropTable.Instance.Find(gachaRecord.m_DropID);
    //        if (dropRecord == null)
    //            return;

    //        CDropTable.Instance.GetGacha(dropRecord.m_TableID, m_TestCase, ref m_RewardInfo);

    //        foreach (var dropInfo in m_RewardInfo.GetMerge())
    //        {
    //            m_TestResult.m_ItemList.Add(new _ItemData(-1, dropInfo.m_DropKey, (int)dropInfo.m_DropCount));
    //        }
    //    }

    //    public override void Complete()
    //    {
    //        if (!CNetManager.Instance.IsAliveSession(m_Session))
    //            return;

    //        CNetManager.Instance.P2C_ReportPostDataList(m_Session, new List<_PostData> { m_TestResult });

    //        CNetManager.Instance.P2C_ResultCheat(m_Session, Packet_Result.Result.Success);
    //    }
    //}



}

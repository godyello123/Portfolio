using System;
using System.Xml;
using System.Collections.Generic;
using SCommon;
using SNetwork;
using Global;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace PlayServer
{
    public class CConfig : SSingleton<CConfig>
	{
        public string ConfigPath { get { return @".\Config\PlayerServerConfig.xml"; } }
        public string GoogleIAPFilePath { get { return @".\Config\knightidle-da3ca-e70cc28af473.json"; } }

        private GooglePlayClaim m_GooglePlayClaim;
        public JObject GoogleIAP { get; set; }
        
        public int ServerID { get; set; }
        public CDefine.eServiceType ServiceType { get; set; }
        public bool IsAcceptEditor { get; set; }
        public string IP { get; set; }
        public string DN { get; set; }
        public ushort PORT { get; set; }
        public int LogLevel { get; set; }
        public int TimeOut { get; set; }
        public int DBThreadCount { get; set; }
        public int RestThreadCount { get; set; }
        public bool IsMessageServer { get; set; }

        public ConnectionInfo MessageServer = new ConnectionInfo();
        public ConnectionInfo GateServer = new ConnectionInfo();

        public CDBServerInfo SystemDB = new CDBServerInfo();
        public CDBServerInfo GameDB = new CDBServerInfo();

        public bool LogEnable = false;
        public string MongoDBConnection { get; set; }
        public string MongoDBName { get; set; }

        public CConfig()
		{
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("Config/PlayServerConfig.xml");

                XmlElement root = doc.DocumentElement;

                XmlElement genaral = (XmlElement)root.GetElementsByTagName("Genaral")[0];

                string strServiceType = genaral.GetAttribute("service_type");
                Enum.TryParse<CDefine.eServiceType>(strServiceType, out var type);
                ServiceType = type;

                IsAcceptEditor = false;
                if (genaral.HasAttribute("editor_mode"))
                    IsAcceptEditor = Convert.ToBoolean(genaral.GetAttribute("editor_mode"));

                IP = genaral.GetAttribute("public_ip");
                if (string.IsNullOrEmpty(IP)) IP = SNetSystem.GetLocalIP();
                DN = genaral.GetAttribute("public_dn");
                if (string.IsNullOrEmpty(DN)) DN = IP;
                string temp;
                if (genaral.HasAttribute("public_port"))
                {
                    temp = genaral.GetAttribute("public_port");
                    if (temp != "") PORT = Convert.ToUInt16(temp);
                }

                LogLevel = Convert.ToByte(genaral.GetAttribute("loglevel"));
                if (genaral.HasAttribute("timeout")) TimeOut = Convert.ToInt32(genaral.GetAttribute("timeout")) * 1000;
                if (genaral.HasAttribute("dbthread_count")) DBThreadCount = Convert.ToInt32(genaral.GetAttribute("dbthread_count"));
                if (genaral.HasAttribute("rest_thread_cnt")) RestThreadCount = Convert.ToInt32(genaral.GetAttribute("rest_thread_cnt"));

                DBThreadCount = DBThreadCount <= 0 ? (Environment.ProcessorCount) : DBThreadCount;

                IsMessageServer = false;
                if (genaral.HasAttribute("is_message_Server"))
                    IsMessageServer = Convert.ToBoolean(genaral.GetAttribute("is_message_Server"));

                var gateServer = (XmlElement)root.GetElementsByTagName("gate_server")[0];
                GateServer.m_IP = gateServer.GetAttribute("ip");
                if (string.IsNullOrEmpty(GateServer.m_IP)) GateServer.m_IP = SNetSystem.GetLocalIP();
                GateServer.m_DN = gateServer.GetAttribute("dn");
                if (string.IsNullOrEmpty(GateServer.m_DN)) GateServer.m_DN = GateServer.m_IP;
                GateServer.m_Port = Convert.ToUInt16(gateServer.GetAttribute("port"));

                var messageServer = (XmlElement)root.GetElementsByTagName("message_server")[0];
                MessageServer.m_IP = messageServer.GetAttribute("ip");
                if (string.IsNullOrEmpty(MessageServer.m_IP)) MessageServer.m_IP = SNetSystem.GetLocalIP();
                MessageServer.m_DN = messageServer.GetAttribute("dn");
                if (string.IsNullOrEmpty(MessageServer.m_DN)) MessageServer.m_DN = MessageServer.m_IP;
                MessageServer.m_Port = Convert.ToUInt16(messageServer.GetAttribute("port"));

                var systemDBinfo = (XmlElement)root.GetElementsByTagName("SystemDB")[0];
                SystemDB.m_Host = systemDBinfo.GetAttribute("host");
                SystemDB.m_ID = systemDBinfo.GetAttribute("id");
                SystemDB.m_PW = systemDBinfo.GetAttribute("pw");
                SystemDB.m_Name = systemDBinfo.GetAttribute("name");

                var gameDBInfo = (XmlElement)root.GetElementsByTagName("GameDB")[0];
                GameDB.m_Host = gameDBInfo.GetAttribute("host");
                GameDB.m_ID = gameDBInfo.GetAttribute("id");
                GameDB.m_PW = gameDBInfo.GetAttribute("pw");
                GameDB.m_Name = gameDBInfo.GetAttribute("name");

                var gameLog = (XmlElement)root.GetElementsByTagName("GameLog")[0];
                MongoDBConnection = gameLog.GetAttribute("ConnectionSring");
                MongoDBName = gameLog.GetAttribute("DBName");

                if (gameLog.HasAttribute("enable"))
                    LogEnable = Convert.ToBoolean(gameLog.GetAttribute("enable"));

                //Firebase
                //XmlElement firebase = (XmlElement)root.GetElementsByTagName("Firebase")[0];
                //FirebaseIssuer = firebase.GetAttribute("issuer");
                //FirebaseAudience = firebase.GetAttribute("audience");

                //db server
                //doc.Load("Config/DBServerConfig.xml");

                //root = doc.DocumentElement;

                //XmlElement sysdb = (XmlElement)root.GetElementsByTagName("SystemDB")[0];
                //SystemDB.m_Name = sysdb.GetAttribute("db_name");
                //SystemDB.m_ID = sysdb.GetAttribute("db_user");
                //SystemDB.m_PW = sysdb.GetAttribute("db_password");
                //SystemDB.m_Host = sysdb.GetAttribute("ip");

                //XmlElement gamedb = (XmlElement)root.GetElementsByTagName("GameDB")[0];
                //GameDB.m_Name = gamedb.GetAttribute("db_name");
                //GameDB.m_ID = gamedb.GetAttribute("db_user");
                //GameDB.m_PW = gamedb.GetAttribute("db_password");
                //GameDB.m_Host = gamedb.GetAttribute("ip");

                //log server
                //doc.Load("Config/LogServerConfig.xml");
                //root = doc.DocumentElement;

                //XmlElement Log = (XmlElement)root.GetElementsByTagName("Log")[0];
                //m_LogServerIP = Log.GetAttribute("public_ip");
                //if (string.IsNullOrEmpty(m_LogServerIP))
                //    m_LogServerIP = SNetSystem.GetLocalIP();

                //m_LogServerPort = Convert.ToUInt16(Log.GetAttribute("port"));
                //m_LogEnable = Convert.ToBoolean(Log.GetAttribute("Log_Enable"));
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString(), "Load Config Error");
            }
        }


        public GooglePlayClaim GetGooglePlayClaim()
        {
            if (m_GooglePlayClaim == null)
            {
                LoadGoogleIAP();
            }

            return m_GooglePlayClaim;
        }
		public void LoadGoogleIAP()
        {
            string filePath = GoogleIAPFilePath;

            var stream = new StreamReader(filePath);
            var jsonString = stream.ReadToEnd();

            JObject jsonGoogleIAP = JsonConvert.DeserializeObject<JObject>(jsonString);

            GooglePlayClaim claim = new GooglePlayClaim();

            claim.kid = jsonGoogleIAP.Value<string>("private_key_id");
            claim.issuer = jsonGoogleIAP.Value<string>("client_email");
            claim.privatekey = jsonGoogleIAP.Value<string>("private_key");
            claim.audience = jsonGoogleIAP.Value<string>("token_uri");

            string certUrl = jsonGoogleIAP.Value<string>("client_x509_cert_url");

            if (string.IsNullOrEmpty(claim.kid) ||
                string.IsNullOrEmpty(claim.issuer) ||
                string.IsNullOrEmpty(claim.privatekey) ||
                string.IsNullOrEmpty(claim.audience) ||
                string.IsNullOrEmpty(certUrl))
            {
                return;
            }

            var restClient = new RestSharp.RestClient(certUrl);
            var request = new RestSharp.RestRequest(certUrl, RestSharp.Method.Get);
            
            var task = restClient.ExecuteAsync(request);
            task.Wait();

            var response = task.Result;
            if (!response.IsSuccessful)
                return;

            var jsonRes = JObject.Parse(response.Content);
            claim.publickey = jsonRes.Value<string>(claim.kid);

            if (string.IsNullOrEmpty(claim.publickey))
                return;

            m_GooglePlayClaim = claim;
        }
	}
}

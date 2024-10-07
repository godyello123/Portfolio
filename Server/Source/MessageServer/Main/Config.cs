using System;
using System.Xml;
using System.Collections.Generic;
using SCommon;
using SNetwork;
using Global;
using System.IO;

namespace MessageServer
{

    public class CConfig : SSingleton<CConfig>
    {
        private string ConfigPath { get { return "Config/MessageServerConfig.xml"; } }

        public CDefine.eServiceType m_ServiceType { get; set; }
        public string IP { get; set; }
        public string DN { get; set; }
        public ushort Port { get; set; }
        public ushort LogLevel { get; set; }
        public int Timeout { get; set; }
        public int DBThreadCount { get; set; }
        public int RestThreadCount { get; set; }

        public CDBServerInfo SystemDB = new CDBServerInfo();
        public CDBServerInfo GameDB = new CDBServerInfo();

        public string FireBaseIssuer { get; set; }
        public string FireBaseAudience { get; set; }


        public CConfig()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ConfigPath);

                XmlElement root = doc.DocumentElement;

                //server
                XmlElement genaral = (XmlElement)root.GetElementsByTagName("Genaral")[0];
                IP = genaral.GetAttribute("public_ip");
                if (string.IsNullOrEmpty(IP)) IP = SNetSystem.GetLocalIP();
                DN = genaral.GetAttribute("public_dn");
                if (string.IsNullOrEmpty(DN)) DN = IP;
                Port = Convert.ToUInt16(genaral.GetAttribute("public_port"));
                LogLevel = Convert.ToByte(genaral.GetAttribute("loglevel"));
                if (genaral.HasAttribute("timeout")) Timeout = Convert.ToInt32(genaral.GetAttribute("timeout")) * 1000;
                if (genaral.HasAttribute("dbthread_count")) DBThreadCount = Convert.ToInt32(genaral.GetAttribute("dbthread_count"));
                DBThreadCount = DBThreadCount <= 0 ? (Environment.ProcessorCount) : DBThreadCount;
                if (genaral.HasAttribute("rest_thread_cnt")) RestThreadCount = Convert.ToInt32(genaral.GetAttribute("rest_thread_cnt"));

                //db
                var dbinfo = (XmlElement)root.GetElementsByTagName("SystemDB")[0];
                SystemDB.m_Host = dbinfo.GetAttribute("host");
                SystemDB.m_ID = dbinfo.GetAttribute("id");
                SystemDB.m_PW = dbinfo.GetAttribute("pw");
                SystemDB.m_Name = dbinfo.GetAttribute("name");

                dbinfo = (XmlElement)root.GetElementsByTagName("GameDB")[0];
                GameDB.m_Host = dbinfo.GetAttribute("host");
                GameDB.m_ID = dbinfo.GetAttribute("id");
                GameDB.m_PW = dbinfo.GetAttribute("pw");
                GameDB.m_Name = dbinfo.GetAttribute("name");



                //firebase
                var firebase = (XmlElement)root.GetElementsByTagName("Firebase")[0];
                FireBaseIssuer = firebase.GetAttribute("issuer");
                FireBaseAudience = firebase.GetAttribute("audience");
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString(), "Load Config Error");
            }
        }
    }
}

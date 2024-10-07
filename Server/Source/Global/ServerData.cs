using System;
using System.Collections.Generic;
using System.Xml;
using SNetwork;

namespace Global
{

    public class CServerInfo : INetSerialize
    {
        public string m_Type;
        public long m_ServerSessionKey;
        public CDefine.eServiceType m_ServiceType;
        public int m_ClientCount;
        public string m_IP;
        public string m_DN;
        public ushort m_Port;
        public int m_MainFPS;
        public int m_DBFPS;
        public int m_DBInputCount;
        public bool m_Open;

        public void Read(SNetReader reader)
        {
            if (!reader.Read(ref m_Type)) return;
            if (!reader.Read(ref m_ServerSessionKey)) return;
            if (!reader.Read(ref m_IP)) return;
            if (!reader.Read(ref m_DN)) return;
            if (!reader.Read(ref m_Port)) return;
            if (!reader.ReadEnum(ref m_ServiceType)) return;
            if (!reader.Read(ref m_ClientCount)) return;
            if (!reader.Read(ref m_MainFPS)) return;
            if (!reader.Read(ref m_DBFPS)) return;
            if (!reader.Read(ref m_DBInputCount)) return;
            if (!reader.Read(ref m_Open)) return;
        }
        public void Write(SNetWriter writer)
        {
            if (!writer.Write(m_Type)) return;
            if (!writer.Write(m_ServerSessionKey)) return;
            if (!writer.Write(m_IP)) return;
            if (!writer.Write(m_DN)) return;
            if (!writer.Write(m_Port)) return;
            if (!writer.WriteEnum(m_ServiceType)) return;
            if (!writer.Write(m_ClientCount)) return;
            if (!writer.Write(m_MainFPS)) return;
            if (!writer.Write(m_DBFPS)) return;
            if (!writer.Write(m_DBInputCount)) return;
            if (!writer.Write(m_Open)) return;
        }
    }

    public class ConnectionInfo: INetSerialize
    {
        public string m_IP;
        public string m_DN;
        public ushort m_Port;

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_IP);
            reader.Read(ref m_DN);
            reader.Read(ref m_Port);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_IP);
            writer.Write(m_DN);
            writer.Write(m_Port);
        }
    }

    public class CDBServerInfo : INetSerialize
	{
		public string m_Host;
		public string m_Name;
		public string m_ID;
		public string m_PW;

		public void Read(SNetReader reader)
		{
			if(!reader.Read(ref m_Host)) return;
			if(!reader.Read(ref m_Name)) return;
			if(!reader.Read(ref m_ID)) return;
			if(!reader.Read(ref m_PW)) return;
		}
		public void Write(SNetWriter writer)
		{
			if(!writer.Write(m_Host)) return;
			if(!writer.Write(m_Name)) return;
			if(!writer.Write(m_ID)) return;
			if(!writer.Write(m_PW)) return;
		}
	}

	public class CDBServerGroup : INetSerialize
	{
		public CDBServerInfo m_SystemDB = new CDBServerInfo();
		public CDBServerInfo m_GameDB = new CDBServerInfo();
		public CDBServerInfo m_LogDB = new CDBServerInfo();
		public CDBServerInfo m_RankingDB = new CDBServerInfo();

		public void LoadDB(string filePath)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(filePath);

			XmlElement root = doc.DocumentElement;

			XmlElement element;
			XmlNodeList nodeList;

			element = (XmlElement)root.GetElementsByTagName("SystemDB")[0];
			m_SystemDB.m_Name = element.GetAttribute("db_name");
			m_SystemDB.m_ID = element.GetAttribute("db_user");
			m_SystemDB.m_PW = element.GetAttribute("db_password");
			nodeList = element.GetElementsByTagName("Uri");
			foreach(var node in nodeList)
			{
				XmlElement uriElement = (XmlElement)node;
			}

			element = (XmlElement)root.GetElementsByTagName("GameDB")[0];
			m_GameDB.m_Name = element.GetAttribute("db_name");
			m_GameDB.m_ID = element.GetAttribute("db_user");
			m_GameDB.m_PW = element.GetAttribute("db_password");
			nodeList = element.GetElementsByTagName("Uri");
			foreach(var node in nodeList)
			{
				XmlElement uriElement = (XmlElement)node;
			}

			element = (XmlElement)root.GetElementsByTagName("LogDB")[0];
			m_LogDB.m_Name = element.GetAttribute("db_name");
			m_LogDB.m_ID = element.GetAttribute("db_user");
			m_LogDB.m_PW = element.GetAttribute("db_password");
			nodeList = element.GetElementsByTagName("Uri");
			foreach(var node in nodeList)
			{
				XmlElement uriElement = (XmlElement)node;
			}

			element = (XmlElement)root.GetElementsByTagName("RankingDB")[0];
			m_RankingDB.m_Host = element.GetAttribute("db_host");
			nodeList = element.GetElementsByTagName("Uri");
			foreach(var node in nodeList)
			{
				XmlElement uriElement = (XmlElement)node;
			}
		}

		public void Read(SNetReader reader)
		{
			if(!reader.Read(ref m_SystemDB)) return;
			if(!reader.Read(ref m_GameDB)) return;
			if(!reader.Read(ref m_LogDB)) return;
			if(!reader.Read(ref m_RankingDB)) return;
		}
		public void Write(SNetWriter writer)
		{
			if(!writer.Write(m_SystemDB)) return;
			if(!writer.Write(m_GameDB)) return;
			if(!writer.Write(m_LogDB)) return;
			if(!writer.Write(m_RankingDB)) return;
		}
	}

	public class CGMData
	{
		public string ID;
		public string Password;
		public int Grade;
	}

	public class CGMList
	{
		//Key : "GMList"
		public static string TYPE() { return "GMList"; }
		public string Type = TYPE();
		public bool CheckType() { return Type == TYPE(); }

		public List<CGMData> GMList = new List<CGMData>();
	}
    
    public class CFireBaseInfo
    {
        public string m_Issuer;
        public string m_Audiance;
    }

    public class CWhiteUser : INetSerialize
    {
        public string m_DeviceID = string.Empty;
        public DateTime m_CreateTime = DateTime.MinValue;

        public void Read(SNetReader r)
        {
            if (!r.Read(ref m_DeviceID)) return;
            if (!r.Read(ref m_CreateTime)) return;
        }

        public void Write(SNetWriter w)
        {
            if (!w.Write(m_DeviceID)) return;
            if (!w.Write(m_CreateTime)) return;
        }
    }
}

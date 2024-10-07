using System;
using System.Xml;
using System.Collections.Generic;
using SCommon;
using SNetwork;
using Global;

namespace LogServer
{
	public class MySqlServerInfo
    {
		public string m_IP = string.Empty;
		public string m_Port = string.Empty;
		public string m_Schema = string.Empty;
		public string m_ID = string.Empty;
		public string m_Password = string.Empty;
    }

	public class CConfig : SSingleton<CConfig>
	{
		//public int AWS_UploadTimeSec { get; set; }
		//public int AWS_FileRowCapacity { get; set; }
		//public string AWS_BucketName { get; set; }
		//public string AWS_BucketRegion { get; set; }
		//public string AWS_AthenaPar1 { get; set; }

        public string MongoDB_ConnectString { get; set; }
        public string MongoDB_Name { get; set; }

		private string m_PublicIP;
		public string PublicIP { get { return m_PublicIP; } set { m_PublicIP = value; } }
		private string m_PublicDN;
		public string PublicDN { get { return m_PublicDN; } set { m_PublicDN = value; } }
		private ushort m_Port = 8200;
		public ushort Port { get { return m_Port; } set { m_Port = value; } }
		private ushort m_PublicPort = 0;
		public ushort PublicPort { get { return m_PublicPort; } set { m_PublicPort = value; } }
		
		public bool MatchOff { get; set; }
		public bool Passive { get; set; }
		private byte m_LogLevel = 1;
		public byte LogLevel { get { return m_LogLevel; } set { m_LogLevel = value; } }
		private int m_Timeout = 120000;
		public int Timeout { get { return m_Timeout; } set { m_Timeout = value; } }


		public CConfig()
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load("Config/LogServerConfig.xml");

				XmlElement root = doc.DocumentElement;

				XmlElement genaral = (XmlElement)root.GetElementsByTagName("Log")[0];

				m_PublicIP = genaral.GetAttribute("public_ip");
				if (string.IsNullOrEmpty(m_PublicIP)) m_PublicIP = SNetSystem.GetLocalIP();
				m_PublicDN = genaral.GetAttribute("public_dn");
				if (string.IsNullOrEmpty(m_PublicDN)) m_PublicDN = m_PublicIP;
				string temp;
				if (genaral.HasAttribute("public_port"))
				{
					temp = genaral.GetAttribute("public_port");
					if (temp != "") m_PublicPort = Convert.ToUInt16(temp);
				}
				
				m_LogLevel = Convert.ToByte(genaral.GetAttribute("loglevel"));
				if (genaral.HasAttribute("timeout")) m_Timeout = Convert.ToInt32(genaral.GetAttribute("timeout")) * 1000;

                //aws option
                //AWS_UploadTimeSec = 10 * 60;
                //AWS_FileRowCapacity = 1000000;
                //var awsOption = (XmlElement)root.GetElementsByTagName("AWS_Option")[0];
                //if (awsOption != null)
                //{
                //	if (int.TryParse(awsOption.GetAttribute("upload_time_sec"), out int uploadTime))
                //        AWS_UploadTimeSec = uploadTime;
                //    if (int.TryParse(awsOption.GetAttribute("file_row_capacity"), out int fileRowCap))
                //        AWS_FileRowCapacity = fileRowCap;
                //
                //	AWS_BucketName = awsOption.GetAttribute("bucket_name");
                //	AWS_BucketRegion = awsOption.GetAttribute("bucket_region");
                //	AWS_AthenaPar1 = awsOption.GetAttribute("athena_par1");
                //}

                //mongoDB
                var mongoDBOption = (XmlElement)root.GetElementsByTagName("Mongo_Option")[0];
                if (mongoDBOption != null)
                {
                    MongoDB_ConnectString = mongoDBOption.GetAttribute("connectString");
                    MongoDB_Name = mongoDBOption.GetAttribute("db_name");
                }
            }
			catch (Exception e)
			{
				System.Windows.Forms.MessageBox.Show(e.ToString(), "Load Config Error");
			}
		}
	}
}

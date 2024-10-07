using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using SCommon;
using Global;

namespace PlayServer
{
	public class CCertOutput
	{
		private long m_SessionKey;
		private bool m_Result;
		public bool Result { get { return m_Result; } }
		private string m_CertToken;
		public string CertToken { get { return m_CertToken; } }

		public CCertOutput(long sessionKey, bool result, string certToken)
		{
			m_SessionKey = sessionKey;
			m_Result = result;
			m_CertToken = certToken;
		}

		public void Complete()
		{
		}
	}

	public class CCertManager : SSingleton<CCertManager>
	{
		private Queue<CCertOutput> m_OutputQueue = new Queue<CCertOutput>();

		public void Update()
		{
			while(true)
			{
				CCertOutput output = null;
				lock(m_OutputQueue) { if(m_OutputQueue.Count > 0) output = m_OutputQueue.Dequeue(); }
				if(output == null) break;
				output.Complete();
			}
		}

		public void InsertOutput(CCertOutput output) { lock(m_OutputQueue) { m_OutputQueue.Enqueue(output); } }

		public static bool Check(string id, string certType, string certString, ref string certToken)
		{
			bool result = true;
			return result;
		}

		public static void Cert(long sessionKey, string id, string certType, string certString)
		{

		}
	}
}

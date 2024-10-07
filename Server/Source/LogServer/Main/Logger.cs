using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCommon;
using Global;

public class CommonLogger
{
    public static void Error(string msg)
    {
        LogServer.CLogger.Instance.Error(msg);
    }

    public static void Debug(string msg)
    {
        LogServer.CLogger.Instance.Error(msg);
    }

    public static void Info(string msg)
    {
        LogServer.CLogger.Instance.Info(msg);
    }

    public static void Warning(string msg)
    {
        LogServer.CLogger.Instance.Warning(msg);
    }

    public static void System(string msg)
    {
        LogServer.CLogger.Instance.System(msg);
    }
}

namespace LogServer
{
	class CLogger : SSingleton<CLogger>
	{
		private static long MAX_COUNT = 10000;
		private string m_LogName;
		private SLogger m_Logger = new SLogger();

		private Form1 m_Form;
		public Form1 Form { set { m_Form = value; } }
		public bool ShowLog { get; set; }

		public CLogger()
		{
			m_Logger.LogLevel = (SLogger.Level)CConfig.Instance.LogLevel;
			ShowLog = false;
		}
		public bool Start(string logName, bool append)
		{
			m_LogName = logName;
			string logFileName = string.Format("{0}_{1}.log", logName, DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss_fff"));
			return m_Logger.Start(logFileName, append);
		}
		public bool Debug(string msg)
		{
			CheckCount();
			//if (ShowLog && m_Logger.LogLevel <= SLogger.Level.Debug && m_FormMain != null) m_FormMain.WriteLog(msg);
			return m_Logger.Debug(msg);
		}
		public bool Info(string msg)
		{
			CheckCount();
			//if (ShowLog && m_Logger.LogLevel <= SLogger.Level.Info && m_FormMain != null) m_FormMain.WriteLog(msg);
			return m_Logger.Info(msg);
		}
		public bool Warning(string msg)
		{
			CheckCount();
			//if (ShowLog && m_Logger.LogLevel <= SLogger.Level.Warning && m_FormMain != null) m_FormMain.WriteLog(msg);
			return m_Logger.Warning(msg);
		}
		public bool Error(string msg)
		{
			string detail = string.Format("{0}{1}", msg, SLogger.CallStack(2));

			CheckCount();
			//if (ShowLog && m_Logger.LogLevel <= SLogger.Level.Error && m_FormMain != null) m_FormMain.WriteLog(msg);
			return m_Logger.Error(detail);
		}
		public bool System(string msg)
		{
			CheckCount();
			//if (m_Logger.LogLevel <= SLogger.Level.System && m_FormMain != null) m_FormMain.WriteLog(msg);
			return m_Logger.System(msg);
		}
		public bool User(string msg)
		{
			CheckCount();
			//if (ShowLog && m_Logger.LogLevel <= SLogger.Level.User && m_FormMain != null) m_FormMain.WriteUserLog(msg);
			return true;
		}
		public void CheckCount()
		{
			if (m_Logger.LogCount < MAX_COUNT) return;
			m_Logger.Stop();
			Start(m_LogName, true);
		}
		public bool IsUser(string name)
		{
			//if (true == string.IsNullOrEmpty(m_FormMain.SearchUserID))
			//	return false;

			//if (true == name.Equals(m_FormMain.SearchUserID))
			//	return true;

			return false;
		}

		public string PrevFuncName()
		{
			var st = new System.Diagnostics.StackFrame(2, true);
			string callstack = st.GetMethod().ToString();

			return callstack;
		}

		//public string AbliLogMessage(string _str)
		//{
		//	if (_str.Contains("Gold"))
		//		return eAbliLog.GoldLevel.ToString();
		//	else if (_str.Contains("Equip"))
		//		return eAbliLog.Equip.ToString();
		//	else if (_str.Contains("LevelPoint"))
		//		return eAbliLog.LevelPoint.ToString();
		//	else if (_str.Contains("Init"))
		//		return eAbliLog.Init.ToString();
		//	else
		//		return eAbliLog.None.ToString();
		//}
	}
}

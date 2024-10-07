﻿using System;
using System.Threading.Tasks;
using SCommon;
using Global;

public class CommonLogger
{
    public static void Error(string msg)
    {
        MessageServer.CLogger.Instance.Error(msg);
    }

    public static void Debug(string msg)
    {
        MessageServer.CLogger.Instance.Error(msg);
    }

    public static void Info(string msg)
    {
        MessageServer.CLogger.Instance.Info(msg);
    }

    public static void Warning(string msg)
    {
        MessageServer.CLogger.Instance.Warning(msg);
    }

    public static void System(string msg)
    {
        MessageServer.CLogger.Instance.System(msg);
    }
}

namespace MessageServer
{
    class CLogger : SSingleton<CLogger>
	{
		private static long MAX_COUNT = 30000;
		private string m_LogName;
		private SLogger m_Logger = new SLogger();

		private FormMain m_FormMain;
		public FormMain FormMain { set { m_FormMain = value; } }
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
			if(ShowLog && m_Logger.LogLevel <= SLogger.Level.Debug && m_FormMain != null)
            {
                m_FormMain.WriteLog(msg);
            }
				
			return m_Logger.Debug(msg);
		}
		public bool Info(string msg)
		{
			CheckCount();
			if(ShowLog && m_Logger.LogLevel <= SLogger.Level.Info && m_FormMain != null)
            {
               m_FormMain.WriteLog(msg);
            }
                
			return m_Logger.Info(msg);
		}
		public bool Warning(string msg)
		{
			CheckCount();
			if(ShowLog && m_Logger.LogLevel <= SLogger.Level.Warning && m_FormMain != null)
            {
                m_FormMain.WriteLog(msg);
            }

			return m_Logger.Warning(msg);
		}
		public bool Error(string msg)
		{
			string detail = string.Format("{0}{1}", msg, SLogger.CallStack(2));

			CheckCount();
			if(m_Logger.LogLevel <= SLogger.Level.Error && m_FormMain != null)
            {
               m_FormMain.WriteLog(msg);
            }
                
			return m_Logger.Error(detail);
		}
		public bool System(string msg)
		{
			CheckCount();
			if(m_Logger.LogLevel <= SLogger.Level.System && m_FormMain != null)
            {
               m_FormMain.WriteLog(msg);
            }
                
			return m_Logger.System(msg);
		}
        public bool User(string msg)
        {
            CheckCount();
            if (ShowLog && m_Logger.LogLevel <= SLogger.Level.User && m_FormMain != null)
            {
                //m_FormMain.WriteUserLog(msg);
            }
                
            return true;
        }
        public void CheckCount()
		{
			if(m_Logger.LogCount < MAX_COUNT) return;
			m_Logger.Stop();
			Start(m_LogName, true);
		}
	}
}

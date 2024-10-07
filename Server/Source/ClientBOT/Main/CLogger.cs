using System;
using System.Threading.Tasks;
using SCommon;
using Global;

public class CommonLogger
{
    public static void Error(string msg)
    {
        ClientBOT.CLogger.Instance.Error(msg);
    }

    public static void Debug(string msg)
    {
        ClientBOT.CLogger.Instance.Error(msg);
    }

    public static void Info(string msg)
    {
        ClientBOT.CLogger.Instance.Info(msg);
    }

    public static void Warning(string msg)
    {
        ClientBOT.CLogger.Instance.Warning(msg);
    }

    public static void System(string msg)
    {
        ClientBOT.CLogger.Instance.System(msg);
    }
}

namespace ClientBOT
{
    class CLogger : SSingleton<CLogger>
    {
        private static long MAX_COUNT = 30000;
        private string m_LogName;
        private SLogger m_Logger = new SLogger();
        public bool ShowLog { get; set; }

        public CLogger()
        {
            m_Logger.LogLevel = (SLogger.Level)1;
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
            return m_Logger.Debug(msg);
        }
        public bool Info(string msg)
        {
            CheckCount();
            return m_Logger.Info(msg);
        }
        public bool Warning(string msg)
        {
            CheckCount();
            return m_Logger.Warning(msg);
        }
        public bool Error(string msg)
        {
            string detail = string.Format("{0}{1}", msg, SLogger.CallStack(2));

            CheckCount();
            return m_Logger.Error(detail);
        }
        public bool System(string msg)
        {
            CheckCount();
            return m_Logger.System(msg);
        }
        public bool User(string msg)
        {
            CheckCount();
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
            //    return false;

            //if (true == name.Equals(m_FormMain.SearchUserID))
            //    return true;

            return false;
        }

        public string PrevFuncName()
        {
            var st = new System.Diagnostics.StackFrame(2, true);
            string callstack = st.GetMethod().ToString();

            return callstack;
        }
    }
}

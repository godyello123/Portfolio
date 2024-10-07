using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace SCommon
{
	public class SLogger
	{
		public enum State { Stopped, Running, Paused }
		public enum Level { Debug, Info, Warning, Error, System, User }

		private string m_LogFileName;
		private bool m_Append = false;
		private State m_State = State.Stopped;
		private Level m_Level = Level.Warning;
		private StreamWriter m_LogFile;

		public string LogFileName { get { return m_LogFileName; } }
		public State LogState { get { return m_State; } }
		public Level LogLevel { get { return m_Level; } set { m_Level = value; } }
		public long LogCount { get; set; }

		public static string CallStack(int skipFrames = 0)
		{
			string callStack = "";

			StackTrace stackTrace = new StackTrace(skipFrames, true);
			var stackFrames = stackTrace.GetFrames();
			foreach(var data in stackFrames)
			{
				if(string.IsNullOrEmpty(data.GetFileName())) continue;
				callStack += string.Format("\n  Function : {0}.{1}, File : {2}, Line : {3}",
					data.GetMethod().ReflectedType.Name, data.GetMethod().Name, data.GetFileName(), data.GetFileLineNumber());
			}

			return callStack;
		}

        public static string DebugCallFuncName(int frame = 0)
        {
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(frame); // 호출한 메서드
            string retStr = $"{stackFrame.GetFileName()} : {stackFrame.GetFileLineNumber()}";
            return retStr;
        }

		public bool Start(string logFileName, bool append)
		{
			lock (this)
			{
				if(LogState != State.Stopped) return false;
				if(string.IsNullOrEmpty(logFileName)) return false;

				m_LogFileName = logFileName;
				m_Append = append;

				string logDir = Path.GetDirectoryName(logFileName);
				if(logDir.Length > 0) Directory.CreateDirectory(logDir);

				if(!m_Append)
				{
					try
					{
						File.Delete(m_LogFileName);
					}
					catch(Exception e)
					{
						MessageBox.Show(e.ToString(), "Error");
						return false;
					}
				}

				if(!File.Exists(m_LogFileName))
				{
					try
					{
						m_LogFile = File.CreateText(m_LogFileName);
					}
					catch(Exception e)
					{
						m_LogFile = null;
						MessageBox.Show(e.ToString(), "Error");
						return false;
					}
				}
				else
				{
					try
					{
						m_LogFile = File.AppendText(m_LogFileName);
					}
					catch(Exception e)
					{
						m_LogFile = null;
						MessageBox.Show(e.ToString(), "Error");
						return false;
					}
				}
				m_LogFile.AutoFlush = true;

				m_State = SLogger.State.Running;
				LogCount = 0;

				return true;
			}
		}
		public bool Pause()
		{
			lock (this)
			{
				if(LogState != State.Running) return false;
				m_State = SLogger.State.Paused;
				return true;
			}
		}
		public bool Resume()
		{
			lock (this)
			{
				if(LogState != State.Paused) return false;
				m_State = SLogger.State.Running;
				return true;
			}
		}
		public bool Stop()
		{
			lock (this)
			{
				if(LogState != State.Running) return false;

				try
				{
					m_LogFile.Close();
					m_LogFile = null;
				}
				catch(Exception e)
				{
					MessageBox.Show(e.ToString(), "Error");
					return false;
				}
				m_State = SLogger.State.Stopped;

				return true;
			}
		}

		private bool WriteLogMsg(Level level, string msg)
		{
			lock (this)
			{
				if(LogState == State.Stopped) return false;
				if(LogState == State.Paused ) return true;

				string logMsg = string.Format("{0} [{1}] {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), level.ToString(), msg);
				m_LogFile.WriteLine(logMsg);
				LogCount++;

				return true;
			}
		}
		public bool Debug(string msg) { return WriteLogMsg(Level.Debug, msg); }
		public bool Info(string msg) { return WriteLogMsg(Level.Info, msg); }
		public bool Warning(string msg) { return WriteLogMsg(Level.Warning, msg); }
		public bool Error(string msg) { return WriteLogMsg(Level.Error, msg); }
		public bool System(string msg) { return WriteLogMsg(Level.System, msg); }
	}
}

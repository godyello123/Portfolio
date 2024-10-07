using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SCommon
{
	public class SCrashManager
	{
		public static string m_MailTitle;
		public static string m_MailSender;
		public static string m_MailPassword;
		public static string m_MailSMTP;    
		public static int m_MailSMTPPort;
		public static List<string> m_MailReceiver;
        public static string m_ProcessName;
        public static List<string> m_TableCrashStrList = new List<string>();

        public static void Init()
		{
			Application.ThreadException += new ThreadExceptionEventHandler(ThreadExeptionHandler);
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);
            m_ProcessName = Process.GetCurrentProcess().ProcessName;
            m_TableCrashStrList.Clear();
        }

		public static void SetMail(string mailTitle, string mailSender, string mailPassword, string mailSMTP, int mailSMTPPort, List<string> mailReceiver)
		{
			m_MailTitle = mailTitle;
			m_MailSender = mailSender;
			m_MailPassword = mailPassword;
			m_MailSMTP = mailSMTP;
			m_MailSMTPPort = mailSMTPPort;
			m_MailReceiver = mailReceiver;
		}

        public static void AddTableErrorString(string error_string)
        {
            m_TableCrashStrList.Add(error_string);
        }

        public static void TableLoadFailCheck()
        {
            if(m_TableCrashStrList.Count > 0)
            {
                System.Text.StringBuilder string_bulider = new System.Text.StringBuilder();
                foreach(var error in m_TableCrashStrList)
                {
                    string_bulider.Append($"Load Fail Table : [{error}]");
                    string_bulider.Append("\n");
                }

                MessageBox.Show(string_bulider.ToString());
                Environment.Exit(0);
            }
        }

		private static void ThreadExeptionHandler(object sender, ThreadExceptionEventArgs args)
		{
			try
			{
				ExceptionLog(args.Exception);
			}
			catch(Exception e)
			{
				MessageBox.Show("ThreadExeptionHandler Fatal Error : " + e.Message);
			}
			finally
            {
                //여기 넣어야하는지 내일 테스트
                //MinidumpWriter.MakeDumb();
            }

			Environment.Exit(0);
		}

		private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			try
			{
                ExceptionLog((Exception)args.ExceptionObject);
			}
			catch(Exception e)
			{
				MessageBox.Show("UnhandledExceptionHandler Fatal Error : " + e.Message);
			}
			finally
            {
                //MinidumpWriter.MakeDumb();
            }

			Environment.Exit(0);
		}

		private static void ExceptionLog(Exception e)
		{
            MinidumpWriter.MakeDumb();

            string ErrorMsg = e.Message + "\r\n\r\nStack Trace:\r\n" + e.StackTrace;
			Directory.CreateDirectory(string.Format(@"Crash\{0}",m_ProcessName));
			string FileName = String.Format("Crash/{0}/Crash_{1}.txt",m_ProcessName,DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"));

			if(!File.Exists(FileName))
			{
				using(StreamWriter Writer = File.CreateText(FileName))
				{
					Writer.WriteLine(ErrorMsg);
				}
			}
			else
			{
				using(StreamWriter Writer = File.AppendText(FileName))
				{
					Writer.WriteLine(ErrorMsg);
				}
			}

			if(!string.IsNullOrEmpty(m_MailTitle))
			{
				//SSendMail sendMail = new SSendMail(m_MailSender, m_MailPassword, m_MailSMTP, m_MailSMTPPort);
				//sendMail.SendMail(m_MailReceiver, m_MailTitle, ErrorMsg);
			}
        }
	}

    public class MinidumpWriter
    {
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr processHandle,
             [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool OpenProcessToken(IntPtr ProcessHandle,
            UInt32 DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool LookupPrivilegeValue(string lpSystemName, string lpName,
            out LUID lpLuid);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
           [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges,
           ref TOKEN_PRIVILEGES NewState,
           UInt32 Zero,
           IntPtr Null1,
           IntPtr Null2);

        [DllImport("DbgHelp.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        private static extern Boolean MiniDumpWriteDump(
                                    IntPtr hProcess,
                                    Int32 processId,
                                    IntPtr fileHandle,
                                    MiniDumpType dumpType,
                                    IntPtr excepInfo,
                                    IntPtr userInfo,
                                    IntPtr extInfo);

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            public UInt32 LowPart;
            public Int32 HighPart;
        }

        public enum MiniDumpType
        {
            Normal = 0x00000000,
            WithDataSegs = 0x00000001,
            WithFullMemory = 0x00000002,
            WithHandleData = 0x00000004,
            FilterMemory = 0x00000008,
            ScanMemory = 0x00000010,
            WithUnloadedModules = 0x00000020,
            WithIndirectlyReferencedMemory = 0x00000040,
            FilterModulePaths = 0x00000080,
            WithProcessThreadData = 0x00000100,
            WithPrivateReadWriteMemory = 0x00000200,
            WithoutOptionalData = 0x00000400,
            WithFullMemoryInfo = 0x00000800,
            WithThreadInfo = 0x00001000,
            WithCodeSegs = 0x00002000,
            WithoutAuxiliaryState = 0x00004000,
            WithFullAuxiliaryState = 0x00008000
        }

        private static uint STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        private static uint STANDARD_RIGHTS_READ = 0x00020000;
        private static uint TOKEN_ASSIGN_PRIMARY = 0x0001;
        private static uint TOKEN_DUPLICATE = 0x0002;
        private static uint TOKEN_IMPERSONATE = 0x0004;
        private static uint TOKEN_QUERY = 0x0008;
        private static uint TOKEN_QUERY_SOURCE = 0x0010;
        private static uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
        private static uint TOKEN_ADJUST_GROUPS = 0x0040;
        private static uint TOKEN_ADJUST_DEFAULT = 0x0080;
        private static uint TOKEN_ADJUST_SESSIONID = 0x0100;
        private static uint TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY);
        private static uint TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY |
            TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE |
            TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT |
            TOKEN_ADJUST_SESSIONID);

        public const string SE_DEBUG_NAME = "SeDebugPrivilege";

        public const UInt32 SE_PRIVILEGE_ENABLED_BY_DEFAULT = 0x00000001;
        public const UInt32 SE_PRIVILEGE_ENABLED = 0x00000002;
        public const UInt32 SE_PRIVILEGE_REMOVED = 0x00000004;
        public const UInt32 SE_PRIVILEGE_USED_FOR_ACCESS = 0x80000000;

        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            public UInt32 PrivilegeCount;
            public LUID Luid;
            public UInt32 Attributes;
        }

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        public static bool MakeDumb()
        {
            int processid = Process.GetCurrentProcess().Id;
            
            string RelativePath = "";

            SetDumpPrivileges(ref RelativePath);

            DirectoryInfo DirectoryPath = new DirectoryInfo(RelativePath);

            if (!DirectoryPath.Exists)
                DirectoryPath.Create();

            string FileName = string.Format(@"\Dump_{0}.dmp", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_dd_ss_fff"));

            string FullPath = RelativePath + FileName;
            Process targetProcess = Process.GetProcessById(processid);

            using (FileStream stream = new FileStream(FullPath, FileMode.Create))
            {
                Boolean res = MiniDumpWriteDump(
                       targetProcess.Handle,
                                       processid,
                                       stream.SafeFileHandle.DangerousGetHandle(),
                                       MiniDumpType.WithFullMemory,
                                       IntPtr.Zero,
                                       IntPtr.Zero,
                                       IntPtr.Zero);

                if (!res)
                {
                    string TextFileName = string.Format(@"\DumpError_{0}.text", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_dd_ss_fff"));

                    if (!File.Exists(FileName))
                    {
                        using (StreamWriter Writer = File.CreateText(FileName))
                        {
                            Writer.WriteLine(Marshal.GetLastWin32Error());
                        }
                    }
                }
            }

            CloseHandle(targetProcess.Handle);

            return true;
        }

        private static void SetDumpPrivileges(ref string Path)
        {
            IntPtr hToken;
            LUID luidSEDebugNameValue;
            TOKEN_PRIVILEGES tkpPrivileges;

            Path = Directory.GetCurrentDirectory() + string.Format(@"\Dump\{0}\",SCrashManager.m_ProcessName);
            DirectoryInfo DirectoryPath = new DirectoryInfo(Path);

            if (!DirectoryPath.Exists)
                DirectoryPath.Create();

            if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out hToken))
            {
                string FileName = string.Format(@"\DumpError_{0}.text", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_dd_ss_fff"));

                if (!File.Exists(FileName))
                {
                    using (StreamWriter Writer = File.CreateText(FileName))
                    {
                        Writer.WriteLine("OpenProcessToken() failed, error = {0} . SeDebugPrivilege is not available", Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    using (StreamWriter writer = File.AppendText(FileName))
                    {
                        writer.WriteLine("OpenProcessToken() failed, error = {0} . SeDebugPrivilege is not available", Marshal.GetLastWin32Error());
                    }
                }

                return;
            }

            if (!LookupPrivilegeValue(null, SE_DEBUG_NAME, out luidSEDebugNameValue))
            {
                string FileName = string.Format(@"\DumpError_{0}.text", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_dd_ss_fff"));

                if (!File.Exists(FileName))
                {
                    using (StreamWriter Writer = File.CreateText(FileName))
                    {
                        Writer.WriteLine("LookupPrivilegeValue() failed, error = {0} .SeDebugPrivilege is not available", Marshal.GetLastWin32Error()); ;
                    }
                }
                else
                {
                    using (StreamWriter writer = File.AppendText(FileName))
                    {
                        writer.WriteLine("LookupPrivilegeValue() failed, error = {0} .SeDebugPrivilege is not available", Marshal.GetLastWin32Error()); ;
                    }
                }

                CloseHandle(hToken);
                return;
            }

            tkpPrivileges.PrivilegeCount = 1;
            tkpPrivileges.Luid = luidSEDebugNameValue;
            tkpPrivileges.Attributes = SE_PRIVILEGE_ENABLED;

            if (!AdjustTokenPrivileges(hToken, false, ref tkpPrivileges, 0, IntPtr.Zero, IntPtr.Zero))
            {
                string FileName = string.Format(@"\DumpError_{0}.text", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_dd_ss_fff"));

                if (!File.Exists(FileName))
                {
                    using (StreamWriter Writer = File.CreateText(FileName))
                    {
                        Writer.WriteLine("AdjustTokenPrivileges() failed, error = {0} .SeDebugPrivilege is not available", Marshal.GetLastWin32Error()); ;
                    }
                }
                else
                {
                    using (StreamWriter writer = File.AppendText(FileName))
                    {
                        writer.WriteLine("AdjustTokenPrivileges() failed, error = {0} .SeDebugPrivilege is not available", Marshal.GetLastWin32Error()); ;
                    }
                }

                return;
            }

            CloseHandle(hToken);
        }
    }
}

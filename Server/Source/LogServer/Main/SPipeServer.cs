using System;
using System.IO;
using System.IO.Pipes;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Security.Principal;
using SCommon;

namespace LogServer
{
    public partial class SPipeServer : SSingleton<SPipeServer>
    {
        private Thread m_Thread;
        private volatile bool m_bRun = false;
        private NamedPipeServerStream m_Pipe = null;
        private ConcurrentQueue<string> m_WaitCmd = new ConcurrentQueue<string>();

        public delegate void PipeHandler();

        private Dictionary<string, PipeHandler> m_Handler = new Dictionary<string, PipeHandler>();

        public bool AddHandler(string cmd, PipeHandler handler)
        {
            if (m_Handler.ContainsKey(cmd))
                return false;

            m_Handler.Add(cmd, handler);
            return true;
        }

        public void Init(string pipeName)
        {
            SetupHandler();

            m_Pipe = CreatePipe(pipeName);
            if (m_Pipe == null)
                return;

            m_Thread = new Thread(new ThreadStart(ThreadFunc));
            m_Thread.Start();
        }

        public void Stop()
        {
            m_bRun = false;

            if (m_Thread != null)
                m_Thread.Join();

            if (m_Pipe != null)
                m_Pipe.Close();

            m_Pipe = null;
            m_Thread = null;
        }

        private NamedPipeServerStream CreatePipe(string pipeName)
        {
            PipeSecurity security = new PipeSecurity();

            var sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);

            PipeAccessRule rule1 = new PipeAccessRule(sid,
                PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);

            security.AddAccessRule(rule1);

            return new NamedPipeServerStream(pipeName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 0, 0, security);
        }
        private void ThreadFunc()
        {
            m_bRun = true;
            m_Pipe.WaitForConnectionAsync();
            while (m_bRun)
            {
                if (m_Pipe.IsConnected)
                {
                    string cmd = ReadString(m_Pipe);
                    if (!string.IsNullOrEmpty(cmd))
                        m_WaitCmd.Enqueue(cmd);
                    m_Pipe.Disconnect();
                    m_Pipe.WaitForConnectionAsync();
                }

                Thread.Sleep(1000);
            }
        }

        public void Update()
        {
            if (m_WaitCmd.TryDequeue(out string cmd))
            {
                if (m_Handler.TryGetValue(cmd, out PipeHandler foundHandler))
                    foundHandler();
            }
        }

        private void WriteString(PipeStream pipe, string text)
        {
            try
            {
                byte[] buf = Encoding.UTF8.GetBytes(text);

                WriteInt32(pipe, buf.Length);
                pipe.Write(buf, 0, buf.Length);
            }
            catch (Exception e)
            {
                CommonLogger.Error($"Error : {e.ToString()}");
            }
        }

        private string ReadString(PipeStream pipe)
        {
            try
            {
                int sentBytes = ReadInt32(pipe);
                byte[] buf = new byte[sentBytes];

                pipe.Read(buf, 0, buf.Length);
                return Encoding.UTF8.GetString(buf, 0, buf.Length);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        private void WriteInt32(PipeStream pipe, int value)
        {
            try
            {
                byte[] buf = BitConverter.GetBytes(value);
                pipe.Write(buf, 0, buf.Length);
            }
            catch (Exception e)
            {
                CommonLogger.Error($"Error : {e.ToString()}");
            }
        }

        private int ReadInt32(PipeStream pipe)
        {
            try
            {
                byte[] intBytes = new byte[4];
                pipe.Read(intBytes, 0, 4);
                return BitConverter.ToInt32(intBytes, 0);
            }
            catch (Exception e)
            {
                CommonLogger.Error($"Error : {e.ToString()}");
                return 0;
            }
        }
    }
}

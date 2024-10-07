using System;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using SCommon;
#if SERVER_ONLY
#endif

namespace SNetwork
{
	public delegate void UpdateDelegate();
	public delegate void CloseSessionDelegate(long sessionKey, CloseType closeType);
	public delegate void AcceptSessionDelegate(long sessionKey);
	public delegate void ConnectSessionDelegate(long sessionKey, bool result);
	public delegate void RecvSessionDelegate(long sessionKey, byte[] packet);
	public delegate void ExceptionSessionDelegate(long sessionKey, Exception e);

	public class SNetSystem
	{
		private static int s_Timeout = 999999999;
		public static int Timeout { get { return s_Timeout; } set { s_Timeout = value; } }
		public static bool TimeoutBothSide { get; set; }
		public static bool DisableCrypt { get; set; }
		private static int s_SendThreadSleep = 1;
		public static int SendThreadSleep { get { return s_SendThreadSleep; } set { s_SendThreadSleep = value; } }

		private bool m_Run;
		private ushort m_Port;
		private Thread m_MainThread;
		private Thread m_SendThread;
		private TcpListener m_Listener;
		private SNetEventQueue m_EventQueue;
		private SNetSessionTable m_SessionTable;
		private DateTime m_CheckTime = DateTime.UtcNow;

		private event UpdateDelegate m_UpdateDelegate;
		private event CloseSessionDelegate m_CloseSessionDelegate;
		private event AcceptSessionDelegate m_AcceptSessionDelegate;
		private event ConnectSessionDelegate m_ConnectSessionDelegate;
		private event RecvSessionDelegate m_RecvSessionDelegate;
		private event ExceptionSessionDelegate m_ExceptionSessionDelegate;

		public void RegisterUpdateDelegate(UpdateDelegate callback) { m_UpdateDelegate += callback; }
		public void RegisterCloseSessionDelegate(CloseSessionDelegate callback) { m_CloseSessionDelegate += callback; }
		public void RegisterAcceptSessionDelegate(AcceptSessionDelegate callback) { m_AcceptSessionDelegate += callback; }
		public void RegisterConnectSessionDelegate(ConnectSessionDelegate callback) { m_ConnectSessionDelegate += callback; }
		public void RegisterRecvSessionDelegate(RecvSessionDelegate callback) { m_RecvSessionDelegate += callback; }
		public void RegisterExceptionSessionDelegate(ExceptionSessionDelegate callback) { m_ExceptionSessionDelegate += callback; }

		public SNetSystem()
		{
			m_EventQueue = new SNetEventQueue();
			m_SessionTable = new SNetSessionTable(m_EventQueue);
		}

		public SNetSession FindSesssion(long sessionKey)
        {
			return m_SessionTable.Find(sessionKey);
        }

		public void Start(ushort port = 0)
		{
			Stop();

			m_Run = true;
			m_Port = port;

			if(m_Port != 0)
			{
				m_MainThread = new Thread(new ThreadStart(MainThreadFunc));
				m_MainThread.Start();

				m_SendThread = new Thread(new ThreadStart(SendThreadFunc));
				m_SendThread.Start();

				m_Listener = new TcpListener(System.Net.IPAddress.IPv6Any, m_Port);
				m_Listener.Server.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
				m_Listener.Start();
				m_Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), m_Listener);
			}
		}

		public void Stop()
		{
			if(!m_Run) return;

			m_Run = false;
			m_Port = 0;

			if(m_Listener != null)
			{
				m_Listener.Stop();
				m_Listener = null;
			}

			if(m_MainThread != null)
			{
				m_MainThread.Join();
				m_MainThread = null;
			}

			if(m_SendThread != null)
			{
				m_SendThread.Join();
				m_SendThread = null;
			}
		}

		private void Check()
		{
			TimeSpan elapsed = DateTime.UtcNow - m_CheckTime;
			if(elapsed.TotalMilliseconds >= 1000)
			{
				m_CheckTime = DateTime.UtcNow;
				m_SessionTable.CheckSession();
			}
		}

		public void Update()
		{
			if(m_SendThread == null)
			{
				Check();
				m_SessionTable.FlushSession();
			}

			while(true)
			{
				INetEvent netEvent = m_EventQueue.Pop();
				if(netEvent == null) break;

				if(netEvent.GetEventType() == EventType.Close)
				{
					SNetEventClose netEventClose = (SNetEventClose)netEvent;
					m_SessionTable.Erase(netEventClose.SessionKey);
					if(m_CloseSessionDelegate != null) m_CloseSessionDelegate(netEventClose.SessionKey, netEventClose.Type);
				}
				else if(netEvent.GetEventType() == EventType.Accept)
				{
					SNetEventAccept netEventAccept = (SNetEventAccept)netEvent;
					long sessionKey = m_SessionTable.NewSessionKey();
					m_SessionTable.Insert(netEventAccept.Session, sessionKey, true);
					if(m_AcceptSessionDelegate != null) m_AcceptSessionDelegate(sessionKey);
				}
				else if(netEvent.GetEventType() == EventType.Connect)
				{
					SNetEventConnect netEventConnect = (SNetEventConnect)netEvent;
					if(netEventConnect.Result) m_SessionTable.Insert(netEventConnect.Session, netEventConnect.SessionKey, false);
					if(m_ConnectSessionDelegate != null) m_ConnectSessionDelegate(netEventConnect.SessionKey, netEventConnect.Result);
				}
				else if(netEvent.GetEventType() == EventType.Recv)
				{
					SNetEventRecv netEventRecv = (SNetEventRecv)netEvent;
					if(m_RecvSessionDelegate != null) m_RecvSessionDelegate(netEventRecv.SessionKey, netEventRecv.Packet.AsSpan().ToArray());

					SCommon.SMemoryPool.Instance.Return(netEventRecv.Packet);
				}
				else if(netEvent.GetEventType() == EventType.Exception)
				{
					SNetEventException netEventException = (SNetEventException)netEvent;
					if(m_ExceptionSessionDelegate != null) m_ExceptionSessionDelegate(netEventException.SessionKey, netEventException.Except);
				}
			}
		}

		private void MainThreadFunc()
		{
			while(m_Run)
			{
                Update();
                if (m_UpdateDelegate != null) m_UpdateDelegate();
				Thread.Sleep(1);
            }
        }

		private void SendThreadFunc()
		{
			while(m_Run)
			{
				Check();
				m_SessionTable.FlushSession();
				Thread.Sleep(s_SendThreadSleep);
            }
        }

        private void CloseConnection(TcpClient client)
        {
            try
            {
                if (client == null)
                    return;

                // Gracefully close the connection
                client.GetStream().Close();
                client.Close();
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur while closing the connection
                m_EventQueue.Push(new SNetEventException(-1, ex));
            }
        }
		private void AcceptCallback(IAsyncResult ar)
		{
            TcpClient client = null;

            try
			{
				TcpListener listener = (TcpListener)ar.AsyncState;
                client = listener.EndAcceptTcpClient(ar);
                m_EventQueue.Push(new SNetEventAccept(client));
                //listener.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), listener);
            }
            catch (IOException ex)
            {
                // handle the error
                if (ex.InnerException != null &&
                    ex.InnerException is SocketException &&
                    ((SocketException)ex.InnerException).ErrorCode == 10054)
                {
                    // This is the "An existing connection was forcibly closed by the remote host" error
                    // Handle the error by logging and/or closing the connection gracefully.
                    m_EventQueue.Push(new SNetEventException(-1, ex));
                    CloseConnection(client);
                }
                else
                {
                    // Handle other IO errors
                    m_EventQueue.Push(new SNetEventException(-1, ex));
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                m_EventQueue.Push(new SNetEventException(-1, ex));
            }
            finally
            {
                
                // Start listening for the next connection
                TcpListener listener = (TcpListener)ar.AsyncState;
                if (listener.Server.IsBound)
                    listener.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), listener);
            }
		}

		public void CloseAllAcceptedSession()
		{
			m_SessionTable.CloseAllAcceptedSession();
		}

		public void CloseSession(long sessionKey)
		{
			m_SessionTable.CloseSession(sessionKey);
		}

		public void SendSession(long sessionKey, byte[] message, ushort length)
		{
			m_SessionTable.SendSession(sessionKey, message, length);
		}

        public void SendSession(long sessionKey, SNetWriter writer)
        {
            m_SessionTable.SendSession(sessionKey, writer);
        }

        public string GetSessionRemoteIP(long sessionKey)
		{
			return m_SessionTable.GetSessionRemoteIP(sessionKey);
		}

		class SNetConnector : IDisposable
		{
			private bool m_Disposed;

			public long SessionKey { get; set; }
			public TcpClient Session { get; set; }

			public SNetConnector()
			{
				Session = new TcpClient(AddressFamily.InterNetworkV6);
				Session.Client.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
			}
			~SNetConnector()
			{
				Dispose(false);
			}
			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}
			protected virtual void Dispose(bool disposing)
			{
				if(m_Disposed) return;
				if(disposing)
				{
					if(Session != null)
					{
						Session.Close();
						Session = null;
					}
				}
				m_Disposed = true;
			}
		}

		public long Connect(string host, ushort port, int timeout = 0)
		{
			SNetConnector netConnector = new SNetConnector();
			netConnector.SessionKey = m_SessionTable.NewSessionKey();
			lock(netConnector)
			{
				try
				{
					IAsyncResult result = netConnector.Session.BeginConnect(host, port, new AsyncCallback(ConnectCallback), netConnector);
					if(timeout > 0 && !result.AsyncWaitHandle.WaitOne(timeout, true))
					{
						m_EventQueue.Push(new SNetEventConnect(false, netConnector.Session, netConnector.SessionKey));
						netConnector.Dispose();
					}
				}
				catch(Exception e)
				{
					m_EventQueue.Push(new SNetEventException(netConnector.SessionKey, e));
					m_EventQueue.Push(new SNetEventConnect(false, netConnector.Session, netConnector.SessionKey));
					netConnector.Dispose();
				}
			}

			return netConnector.SessionKey;
		}

		private void ConnectCallback(IAsyncResult ar)
		{
			SNetConnector netConnector = (SNetConnector)ar.AsyncState;
			lock(netConnector)
			{
				try
				{
					if(netConnector.Session != null)
					{
						netConnector.Session.EndConnect(ar);
						m_EventQueue.Push(new SNetEventConnect(true, netConnector.Session, netConnector.SessionKey));
					}
				}
				catch(Exception e)
				{
					m_EventQueue.Push(new SNetEventException(netConnector.SessionKey, e));
					m_EventQueue.Push(new SNetEventConnect(false, netConnector.Session, netConnector.SessionKey));
					netConnector.Dispose();
				}
			}
		}

		public T ReadSerializable<T>(long sessionKey, byte[] packet) where T : class, new()
		{
			try
			{
				using(MemoryStream stream = new MemoryStream(packet))
				{
					BinaryFormatter formatter = new BinaryFormatter();
					return (T)formatter.Deserialize(stream);
				}
			}
			catch(Exception e)
			{
				m_EventQueue.Push(new SNetEventException(sessionKey, e));
				CloseSession(sessionKey);
				return null;
			}
		}

		//public bool WriteSerializable<T>(long sessionKey, T writeMsg)
		//{
		//	try
		//	{
		//		using(MemoryStream stream = new MemoryStream())
		//		{
		//			BinaryFormatter formatter = new BinaryFormatter();
		//			formatter.Serialize(stream, writeMsg);
		//			byte[] binary = stream.ToArray();
		//			SendSession(sessionKey, binary, (ushort)binary.Length);
		//		}
		//	}
		//	catch(Exception e)
		//	{
		//		m_EventQueue.Push(new SNetEventException(sessionKey, e));
		//		CloseSession(sessionKey);
		//		return false;
		//	}
		//	return true;
		//}

		public bool Read(long sessionKey, INetPacket readMsg, byte[] packet)
		{
			SNetReader reader = new SNetReader(packet);
			try
			{
				if(!DisableCrypt) reader.Crypt(readMsg.GetCrypt());
				ushort protocol = 0;
				reader.Read(ref protocol);
				readMsg.Read(reader);
				if(!reader.GetResult()) throw new Exception(string.Format("SNetSystem.Read Error : {0}", readMsg.GetProtocol()));
			}
			catch(Exception e)
			{
				m_EventQueue.Push(new SNetEventException(sessionKey, e));
				CloseSession(sessionKey);
				return false;
			}
			return true;
		}

		public bool Write(long sessionKey, INetPacket writeMsg, int buffSize = 1024)
		{
			SNetWriter writer = new SNetWriter(buffSize);
			try
			{
				writer.Write(writeMsg.GetProtocol());
				writeMsg.Write(writer);
				if(!writer.GetResult()) throw new Exception(string.Format("SNetSystem.Write Error : {0}", writeMsg.GetProtocol()));
				if(!DisableCrypt) writer.Crypt(writeMsg.GetCrypt());
				if(!writer.WriteLength()) throw new Exception(string.Format("SNetSystem.WriteLength Error : {0}", writeMsg.GetProtocol()));

				SendSession(sessionKey, writer);
				writer.Return();
			}
			catch(Exception e)
			{
				writer.Return();
				m_EventQueue.Push(new SNetEventException(sessionKey, e));
				CloseSession(sessionKey);
				return false;
			}
			return true;
		}

		public bool IsAliveSession(long sessionKey)
        {
			return m_SessionTable.IsAliveSession(sessionKey);
        }

		public static string GetLocalIP()
		{
			string ipHost = "";
			IPAddress[] ipList = Dns.GetHostAddresses(Dns.GetHostName());
			foreach(IPAddress ip in ipList)
			{
				if(ip.AddressFamily == AddressFamily.InterNetwork) return ipHost = ip.ToString();
			}
			return ipHost;
		}
	}
}

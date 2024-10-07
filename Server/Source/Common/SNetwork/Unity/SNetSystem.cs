using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace SNetwork
{
	public delegate void CloseSessionDelegate(long sessionKey, CloseType closeType);
	public delegate void AcceptSessionDelegate(long sessionKey);
	public delegate void ConnectSessionDelegate(long sessionKey, bool result);
	public delegate void RecvSessionDelegate(long sessionKey, byte[] packet);
	public delegate void ExceptionSessionDelegate(long sessionKey, Exception e);

	public class SNetSystem
	{
		private static int s_Timeout = 30000;
		public static int Timeout { get { return s_Timeout; } set { s_Timeout = value; } }
		public static bool TimeoutBothSide { get; set; }
		public static bool DisableCrypt { get; set; }

		private bool m_Run;
		private ushort m_Port;
		private TcpListener m_Listener;
		private SNetEventQueue m_EventQueue;
		private SNetSessionTable m_SessionTable;
		private DateTime m_CheckTime = DateTime.UtcNow;

		private event CloseSessionDelegate m_CloseSessionDelegate;
		private event AcceptSessionDelegate m_AcceptSessionDelegate;
		private event ConnectSessionDelegate m_ConnectSessionDelegate;
		private event RecvSessionDelegate m_RecvSessionDelegate;
		private event ExceptionSessionDelegate m_ExceptionSessionDelegate;

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

		public void Start(ushort port = 0)
		{
			Stop();

			m_Run = true;
			m_Port = port;

			if(m_Port != 0)
			{
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
			Check();
			m_SessionTable.FlushSession();

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
					if(m_RecvSessionDelegate != null) m_RecvSessionDelegate(netEventRecv.SessionKey, netEventRecv.Packet);
				}
				else if(netEvent.GetEventType() == EventType.Exception)
				{
					SNetEventException netEventException = (SNetEventException)netEvent;
					if(m_ExceptionSessionDelegate != null) m_ExceptionSessionDelegate(netEventException.SessionKey, netEventException.Except);
				}
			}
		}

		private void AcceptCallback(IAsyncResult ar)
		{
			try
			{
				TcpListener listener = (TcpListener)ar.AsyncState;
				m_EventQueue.Push(new SNetEventAccept(listener.EndAcceptTcpClient(ar)));
				listener.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), listener);
			}
			catch(Exception e)
			{
				m_EventQueue.Push(new SNetEventException(-1, e));
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

		public string GetSessionRemoteIP(long sessionKey)
		{
			return m_SessionTable.GetSessionRemoteIP(sessionKey);
		}

		class SNetConnector
		{
			public long m_SessionKey;
			public TcpClient m_Session;
			public List<IPAddress> m_IPAddress;
			public ushort m_Port;
			public int m_Timeout;
		}

		private void Connect(SNetConnector netConnector)
		{
			bool isFail = false;

			lock (netConnector)
			{
				try
				{
					if(netConnector.m_IPAddress.Count <= 0)
					{
						m_EventQueue.Push(new SNetEventConnect(false, netConnector.m_Session, netConnector.m_SessionKey));
						return;
					}

					IPAddress ipAddr = netConnector.m_IPAddress[0];
					netConnector.m_IPAddress.RemoveAt(0);
					netConnector.m_Session = new TcpClient(ipAddr.AddressFamily);
					IAsyncResult result = netConnector.m_Session.BeginConnect(ipAddr, netConnector.m_Port, new AsyncCallback(ConnectCallback), netConnector);
					if(netConnector.m_Timeout > 0 && !result.AsyncWaitHandle.WaitOne(netConnector.m_Timeout, true))
					{
						netConnector.m_Session.Close();
						netConnector.m_Session = null;
						isFail = true;
					}
				}
				catch(Exception e)
				{
					m_EventQueue.Push(new SNetEventException(netConnector.m_SessionKey, e));
					isFail = true;
				}
			}

			if(isFail) Connect(netConnector);
		}

		public long Connect(string host, ushort port, int timeout = 0)
		{
			SNetConnector netConnector = new SNetConnector();
			netConnector.m_SessionKey = m_SessionTable.NewSessionKey();
			netConnector.m_IPAddress = new List<IPAddress>();
			netConnector.m_IPAddress.InsertRange(0, Dns.GetHostAddresses(host));
			netConnector.m_Port = port;
			netConnector.m_Timeout = timeout;

			Connect(netConnector);

			return netConnector.m_SessionKey;
		}

		private void ConnectCallback(IAsyncResult ar)
		{
			bool isFail = false;

			SNetConnector netConnector = (SNetConnector)ar.AsyncState;
			lock (netConnector)
			{
				try
				{
					if(netConnector.m_Session != null)
					{
						netConnector.m_Session.EndConnect(ar);
						m_EventQueue.Push(new SNetEventConnect(true, netConnector.m_Session, netConnector.m_SessionKey));
					}
				}
				catch(Exception e)
				{
					m_EventQueue.Push(new SNetEventException(netConnector.m_SessionKey, e));
					isFail = true;
				}
			}

			if(isFail) Connect(netConnector);
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

		public bool WriteSerializable<T>(long sessionKey, T writeMsg)
		{
			try
			{
				using(MemoryStream stream = new MemoryStream())
				{
					BinaryFormatter formatter = new BinaryFormatter();
					formatter.Serialize(stream, writeMsg);
					byte[] binary = stream.ToArray();
					SendSession(sessionKey, binary, (ushort)binary.Length);
				}
			}
			catch(Exception e)
			{
				m_EventQueue.Push(new SNetEventException(sessionKey, e));
				CloseSession(sessionKey);
				return false;
			}
			return true;
		}

		public bool Read(long sessionKey, INetPacket readMsg, byte[] packet)
		{
			SNetReader reader = new SNetReader(packet);
			try
			{
				if(!DisableCrypt) reader.Crypt(readMsg.GetCrypt());
				ushort protocol = 0;
				reader.Read(ref protocol);
				readMsg.Read(reader);
				if(!reader.GetResult()) throw new Exception("SNetSystem.Read Error");
			}
			catch(Exception e)
			{
				m_EventQueue.Push(new SNetEventException(sessionKey, e));
				CloseSession(sessionKey);
				return false;
			}
			return true;
		}

		public bool Write(long sessionKey, INetPacket writeMsg, int buffSize = 128)
		{
			SNetWriter writer = new SNetWriter(buffSize);
			try
			{
				writer.Write(writeMsg.GetProtocol());
				writeMsg.Write(writer);
				if(!writer.GetResult()) throw new Exception("SNetSystem.Write Error");
				if(!DisableCrypt) writer.Crypt(writeMsg.GetCrypt());
				SendSession(sessionKey, writer.GetBuffer(), writer.GetBufferLength());
			}
			catch(Exception e)
			{
				m_EventQueue.Push(new SNetEventException(sessionKey, e));
				CloseSession(sessionKey);
				return false;
			}
			return true;
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

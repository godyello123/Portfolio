using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SNetwork
{
	public class SNetSession
	{
		public static bool s_SocketExceptionLog;

		public const int PACKET_HEADER = 2;
		public const int MAX_PACKET = short.MaxValue + 1;
		public const int DEFAULT_SEND_BUFFER = 8192;

		private TcpClient m_Session;
		private long m_SessionKey;
		private SNetEventQueue m_EventQueue;
		private bool m_Accepted;
		public bool Accepted { get { return m_Accepted; } }
		public EndPoint RemoteEndPoint { get; set; }

		private int m_Read;
		private byte[] m_RecvHeader = new byte[PACKET_HEADER];
		private byte[] m_RecvMessage;
		private DateTime m_RecvTime = DateTime.UtcNow;
		private Object m_RecvTimeLock = new Object();
		public DateTime RecvTime { get { lock(this) { return m_RecvTime; } } set { lock(this) { m_RecvTime = value; } } }

		private int m_Send;
		private SCircularQueue m_SendQueue = new SCircularQueue(DEFAULT_SEND_BUFFER);
		private Object m_SendLock = new Object();

		public SNetSession(TcpClient session, long sessionKey, SNetEventQueue eventQueue, bool accepted)
		{
			m_Session = session;
			m_SessionKey = sessionKey;
			m_EventQueue = eventQueue;
			m_Accepted = accepted;
			RemoteEndPoint = m_Session.Client.RemoteEndPoint;
			RecvHeader();
		}

		public void Close(CloseType closeType = CloseType.Normal)
		{
			lock(this)
			{
				if(m_Session == null) return;

				try
				{
					if(!m_Session.Connected) return;

					try
					{
						m_SendQueue.Pop(m_Send);
						m_Send = 0;

						while(true)
						{
							byte[] buff;
							int offset, length;
							m_SendQueue.GetBuffer(out buff, out offset, out length);
							if(buff == null) break;
							m_Session.GetStream().BeginWrite(buff, offset, length, new AsyncCallback(OnSend), null);
							m_SendQueue.Pop(length);
						}
					}
					finally
					{
						m_Session.GetStream().Close();
						m_Session.Close();
					}
				}
				catch(Exception e)
				{
					if(s_SocketExceptionLog) OnException(e);
				}
				finally
				{
					m_Session = null;
					m_EventQueue.Push(new SNetEventClose(m_SessionKey, closeType));
				}
			}
		}
		private void OnException(Exception e)
		{
			m_EventQueue.Push(new SNetEventException(m_SessionKey, e));
		}

		private void RecvHeader()
		{
			try
			{
				lock(this)
				{
					if(m_Session == null) return;

					m_Session.GetStream().BeginRead(m_RecvHeader, m_Read,
						m_RecvHeader.Length - m_Read, new AsyncCallback(OnRecvHeader), null);
				}
			}
			catch(Exception e)
			{
				if(s_SocketExceptionLog) OnException(e);
				Close();
			}
		}
		private void RecvMessage()
		{
			try
			{
				lock(this)
				{
					if(m_Session == null) return;

					m_Session.GetStream().BeginRead(m_RecvMessage, m_Read,
						m_RecvMessage.Length - m_Read, new AsyncCallback(OnRecvMessage), null);
				}
			}
			catch(Exception e)
			{
				if(s_SocketExceptionLog) OnException(e);
				Close();
			}
		}
		private void OnRecvHeader(IAsyncResult ar)
		{
			try
			{
				lock(this)
				{
					if(m_Session == null) return;

					int length = m_Session.GetStream().EndRead(ar);
					if(length == 0)
					{
						Close();
						return;
					}

					m_Read += length;
					if(m_Read < PACKET_HEADER)
					{
						RecvHeader();
						return;
					}

					ushort messageLength = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(m_RecvHeader, 0));
					if(messageLength < 0 || messageLength > MAX_PACKET)
					{
						m_EventQueue.Push(new SNetEventException(m_SessionKey, new Exception(string.Format("MessageLength Error : Length[{0}]", messageLength))));
						Close();
						return;
					}
					m_RecvMessage = new byte[messageLength];

					m_Read = 0;
					RecvMessage();
				}
			}
			catch(Exception e)
			{
				if(s_SocketExceptionLog) OnException(e);
				Close();
			}
		}
		private void OnRecvMessage(IAsyncResult ar)
		{
			try
			{
				lock(this)
				{
					if(m_Session == null) return;

					int length = m_Session.GetStream().EndRead(ar);
					if(length == 0)
					{
						Close();
						return;
					}

					m_Read += length;
					if(m_Read < m_RecvMessage.Length)
					{
						RecvMessage();
						return;
					}

					m_EventQueue.Push(new SNetEventRecv(m_SessionKey, m_RecvMessage));
					m_RecvMessage = null;
					RecvTime = DateTime.UtcNow;

					m_Read = 0;
					RecvHeader();
				}
			}
			catch(Exception e)
			{
				if(s_SocketExceptionLog) OnException(e);
				Close();
			}
		}

		public void Flush()
		{
			try
			{
				lock(this)
				{
					if(m_Session == null) return;

					if(m_Send <= 0)
					{
						byte[] buff;
						int offset, length;
						m_SendQueue.GetBuffer(out buff, out offset, out length);
						if(buff != null)
						{
							m_Send = length;
							m_Session.GetStream().BeginWrite(buff, offset, length, new AsyncCallback(OnSend), null);
						}
					}
				}
			}
			catch(Exception e)
			{
				if(s_SocketExceptionLog) OnException(e);
				Close();
			}
		}
		public void Send(byte[] message, ushort length)
		{
			try
			{
				lock(this)
				{
					byte[] header = BitConverter.GetBytes((ushort)IPAddress.HostToNetworkOrder((short)length));
					m_SendQueue.Push(header, 0, PACKET_HEADER);
					m_SendQueue.Push(message, 0, length);
				}
			}
			catch(Exception e)
			{
				if(s_SocketExceptionLog) OnException(e);
				Close();
			}
		}
		private void OnSend(IAsyncResult ar)
		{
			try
			{
				lock(this)
				{
					if(m_Session == null) return;

					m_Session.GetStream().EndWrite(ar);
					m_SendQueue.Pop(m_Send);
					m_Send = 0;
				}
			}
			catch(Exception e)
			{
				if(s_SocketExceptionLog) OnException(e);
				Close();
			}
		}
	}

	public class SNetSessionTable
	{
		private long m_SessionKey;
		private Dictionary<long, SNetSession> m_SessionTable = new Dictionary<long, SNetSession>();
		private SNetEventQueue m_EventQueue;

		public SNetSessionTable(SNetEventQueue eventQueue)
		{
			m_EventQueue = eventQueue;
		}

		public long NewSessionKey()
		{
			lock(this) { return ++m_SessionKey; }
		}

		private SNetSession Find(long sessionKey)
		{
			lock(m_SessionTable) { return m_SessionTable.ContainsKey(sessionKey) ? m_SessionTable[sessionKey] : null; }
		}

		public void Insert(TcpClient session, long sessionKey, bool accepted)
		{
			lock(m_SessionTable) { m_SessionTable.Add(sessionKey, new SNetSession(session, sessionKey, m_EventQueue, accepted)); }
		}

		public void Erase(long sessionKey)
		{
			lock(m_SessionTable) { m_SessionTable.Remove(sessionKey); }
		}

		public void CheckSession()
		{
			lock(m_SessionTable)
			{
				foreach(var pair in m_SessionTable)
				{
					SNetSession netSession = pair.Value;
					if(!SNetSystem.TimeoutBothSide && !netSession.Accepted) continue;
					TimeSpan elapsed = DateTime.UtcNow - netSession.RecvTime;
					if(elapsed.TotalMilliseconds > SNetSystem.Timeout) netSession.Close(CloseType.Timeout);
				}
			}
		}

		public void FlushSession()
		{
			lock(m_SessionTable)
			{
				foreach(var pair in m_SessionTable) pair.Value.Flush();
			}
		}

		public void CloseAllAcceptedSession()
		{
			lock(m_SessionTable)
			{
				foreach(var pair in m_SessionTable)
				{
					SNetSession netSession = pair.Value;
					if(netSession.Accepted) netSession.Close();
				}
			}
		}

		public void CloseSession(long sessionKey)
		{
			SNetSession netSession = Find(sessionKey);
			if(netSession != null) netSession.Close();
		}

		public void SendSession(long sessionKey, byte[] message, ushort length)
		{
			SNetSession netSession = Find(sessionKey);
			if(netSession != null) netSession.Send(message, length);
		}

		public string GetSessionRemoteIP(long sessionKey)
		{
			SNetSession netSession = Find(sessionKey);
			return (netSession != null) ? ((IPEndPoint)netSession.RemoteEndPoint).Address.ToString() : "";
		}
	}
}

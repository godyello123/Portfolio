using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SNetwork
{
	public delegate void CloseDelegate(SNetClient client);
	public delegate void ConnectDelegate(SNetClient client, bool result);
	public delegate void RecvDelegate(SNetClient client, byte[] packet);
	public delegate void ExceptionDelegate(SNetClient client, Exception e);

	public class SNetClient : IDisposable
	{
		private const int PACKET_HEADER = 2;
		private const int MAX_PACKET = 4096;
		private const int MAX_RECV_BUFFER = MAX_PACKET * 4;
		private const int MAX_SEND_BUFFER = MAX_PACKET * 4;

		private enum EVENT_TYPE { CLOSE, CONNECT, RECV, EXCEPTION }

		private class SNetEvent
		{
			public SNetClient m_Client;
			public EVENT_TYPE m_EventType;

			public SNetEvent(SNetClient client, EVENT_TYPE eventType)
			{
				m_Client = client;
				m_EventType = eventType;
			}
		}
		private class SNetEventConnect : SNetEvent
		{
			public bool m_Result;

			public SNetEventConnect(SNetClient client, bool result) : base(client, EVENT_TYPE.CONNECT)
			{
				m_Result = result;
			}
		}
		private class SNetEventRecv : SNetEvent
		{
			public byte[] m_Packet;

			public SNetEventRecv(SNetClient client, byte[] packet) : base(client, EVENT_TYPE.RECV)
			{
				m_Packet = packet;
			}
		}
		private class SNetEventException : SNetEvent
		{
			public Exception m_Exception;

			public SNetEventException(SNetClient client, Exception e) : base(client, EVENT_TYPE.EXCEPTION)
			{
				m_Exception = e;
			}
		}

		private bool m_Disposed;
		private Socket m_Socket;
		private bool m_Connecting;

		private class SNetRecvBuffer
		{
			public int m_Head;
			public int m_Tail;
			public int m_Left;
			public byte[] m_Buffer = new byte[MAX_RECV_BUFFER + MAX_PACKET];

			public void Reset() { m_Head = m_Tail = m_Left = 0; }
		}
		private SNetRecvBuffer m_RecvBuffer = new SNetRecvBuffer();

		private class SNetSendBuffer
		{
			public int m_Head;
			public int m_Tail;
			public int m_Left;
			public bool m_Sending;
			public byte[] m_Buffer = new byte[MAX_SEND_BUFFER + MAX_PACKET];

			public void Reset() { m_Head = m_Tail = m_Left = 0; m_Sending = false; }
		}
		private SNetSendBuffer m_SendBuffer = new SNetSendBuffer();

		private Queue<SNetEvent> m_EventQueue = new Queue<SNetEvent>();

		private event CloseDelegate m_CloseDelegate;
		private event ConnectDelegate m_ConnectDelegate;
		private event RecvDelegate m_RecvDelegate;
		private event ExceptionDelegate m_ExceptionDelegate;

		~SNetClient()
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
			if(disposing) Close();
			m_Disposed = true;
		}

		public bool IsConnecting() { return m_Connecting; }
		public bool IsConnected() { return m_Socket != null && m_Socket.Connected; }
		public void RegisterCloseDelegate(CloseDelegate closeDelegate) { m_CloseDelegate += closeDelegate; }
		public void RegisterConnectDelegate(ConnectDelegate connectDelegate) { m_ConnectDelegate += connectDelegate; }
		public void RegisterRecvDelegate(RecvDelegate recvDelegate) { m_RecvDelegate += recvDelegate; }
		public void RegisterExceptionDelegate(ExceptionDelegate exceptionDelegate) { m_ExceptionDelegate += exceptionDelegate; }

		public void Update()
		{
			while(true)
			{
				SNetEvent netEvent;
				lock(m_EventQueue)
				{
					if(m_EventQueue.Count <= 0) return;
					netEvent = m_EventQueue.Dequeue();
				}

				if(netEvent.m_EventType == EVENT_TYPE.CLOSE)
				{
					if(m_CloseDelegate != null) m_CloseDelegate(this);
				}
				else if(netEvent.m_EventType == EVENT_TYPE.CONNECT)
				{
					SNetEventConnect netEventConnect = (SNetEventConnect)netEvent;
					if(m_ConnectDelegate != null) m_ConnectDelegate(this, netEventConnect.m_Result);
				}
				else if(netEvent.m_EventType == EVENT_TYPE.RECV)
				{
					SNetEventRecv netEventRecv = (SNetEventRecv)netEvent;
					if(m_RecvDelegate != null) m_RecvDelegate(this, netEventRecv.m_Packet);
				}
				else if(netEvent.m_EventType == EVENT_TYPE.EXCEPTION)
				{
					SNetEventException netEventException = (SNetEventException)netEvent;
					if(m_ExceptionDelegate != null) m_ExceptionDelegate(this, netEventException.m_Exception);
				}
			}
		}

		public void Close()
		{
			try
			{
				if(!IsConnected()) return;

				m_Socket.Shutdown(SocketShutdown.Both);
				m_Socket.Close();
			}
			catch(Exception e)
			{
				OnException(e, false);
			}
			finally
			{
				if(m_Socket != null)
				{
					m_Socket.Dispose();
					m_Socket = null;
					m_Connecting = false;
					m_RecvBuffer.Reset();
					m_SendBuffer.Reset();
					lock(m_EventQueue) { m_EventQueue.Enqueue(new SNetEvent(this, EVENT_TYPE.CLOSE)); }
				}
			}
		}

		public bool Connect(string host, int port, bool async)
		{
			try
			{
				if(IsConnected() || IsConnecting()) return false;

				m_RecvBuffer.Reset();
				m_SendBuffer.Reset();
				m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				m_Socket.NoDelay = true;

				if(!async)
				{
					m_Socket.Connect(host, port);
					Recv();
				}
				else
				{
					m_Connecting = true;
					m_Socket.BeginConnect(host, port, new AsyncCallback(OnConnect), null);
				}

				return true;
			}
			catch(Exception e)
			{
				if(async)
				{
					OnException(e);
					lock(m_EventQueue) { m_EventQueue.Enqueue(new SNetEventConnect(this, false)); }
				}

				return false;
			}
		}
		private void OnConnect(IAsyncResult ar)
		{
			try
			{
				m_Connecting = false;
				m_Socket.EndConnect(ar);
				lock(m_EventQueue) { m_EventQueue.Enqueue(new SNetEventConnect(this, true)); }
				Recv();
			}
			catch(Exception e)
			{
				OnException(e);
				lock(m_EventQueue) { m_EventQueue.Enqueue(new SNetEventConnect(this, false)); }
			}
		}

		private void Recv()
		{
			try
			{
				if(!IsConnected()) return;

				m_Socket.BeginReceive(m_RecvBuffer.m_Buffer, m_RecvBuffer.m_Tail, MAX_PACKET, SocketFlags.None, new AsyncCallback(OnRecv), null);
			}
			catch(Exception e)
			{
				OnException(e);
			}
		}
		private void OnRecv(IAsyncResult ar)
		{
			try
			{
				if(!IsConnected()) return;

				int length = m_Socket.EndReceive(ar);
				if(length == 0)
				{
					Close();
					return;
				}

				m_RecvBuffer.m_Left += length;
				if(m_RecvBuffer.m_Left > MAX_RECV_BUFFER)
				{
					Close();
					return;
				}

				m_RecvBuffer.m_Tail += length;
				int delta = m_RecvBuffer.m_Tail - MAX_RECV_BUFFER;
				if(delta >= 0)
				{
					m_RecvBuffer.m_Tail = delta;
					Buffer.BlockCopy(m_RecvBuffer.m_Buffer, MAX_RECV_BUFFER, m_RecvBuffer.m_Buffer, 0, delta);
				}

				if(!CheckRecv())
				{
					Close();
					return;
				}

				Recv();
			}
			catch(System.Exception e)
			{
				OnException(e);
			}
		}
		private bool CheckRecv()
		{
			while(true)
			{
				if(m_RecvBuffer.m_Left < PACKET_HEADER) break;

				int delta = m_RecvBuffer.m_Head + PACKET_HEADER - MAX_RECV_BUFFER;
				if(delta > 0) Buffer.BlockCopy(m_RecvBuffer.m_Buffer, 0, m_RecvBuffer.m_Buffer, MAX_RECV_BUFFER, delta);

				ushort length = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(m_RecvBuffer.m_Buffer, m_RecvBuffer.m_Head));
				if(PACKET_HEADER + length > MAX_PACKET) return false;
				if(PACKET_HEADER + length > m_RecvBuffer.m_Left) break;

				m_RecvBuffer.m_Head += PACKET_HEADER;
				if(delta > 0) m_RecvBuffer.m_Head = delta;

				delta = m_RecvBuffer.m_Head + length - MAX_RECV_BUFFER;
				if(delta > 0) Buffer.BlockCopy(m_RecvBuffer.m_Buffer, 0, m_RecvBuffer.m_Buffer, MAX_RECV_BUFFER, delta);

				byte[] packetBody = new byte[length];
				Buffer.BlockCopy(m_RecvBuffer.m_Buffer, m_RecvBuffer.m_Head, packetBody, 0, length);
				lock(m_EventQueue) { m_EventQueue.Enqueue(new SNetEventRecv(this, packetBody)); }

				m_RecvBuffer.m_Head += length;
				if(delta > 0) m_RecvBuffer.m_Head = delta;

				m_RecvBuffer.m_Left -= PACKET_HEADER + length;
			}

			return true;
		}

		public bool Send(byte[] buffer, ushort length)
		{
			try
			{
				lock(m_SendBuffer)
				{
					if(!IsConnected()) return false;

					if(PACKET_HEADER + length > MAX_PACKET)
					{
						Close();
						return false;
					}
					if(m_SendBuffer.m_Left + PACKET_HEADER + length > MAX_SEND_BUFFER)
					{
						Close();
						return false;
					}

					ushort packetLength = (ushort)IPAddress.HostToNetworkOrder((short)length);
					Buffer.BlockCopy(BitConverter.GetBytes(packetLength), 0, m_SendBuffer.m_Buffer, m_SendBuffer.m_Tail, PACKET_HEADER);
					m_SendBuffer.m_Tail += PACKET_HEADER;

					if(length > 0)
					{
						Buffer.BlockCopy(buffer, 0, m_SendBuffer.m_Buffer, m_SendBuffer.m_Tail, length);
						m_SendBuffer.m_Tail += length;
					}

					int delta = m_SendBuffer.m_Tail - MAX_SEND_BUFFER;
					if(delta >= 0)
					{
						m_SendBuffer.m_Tail = delta;
						Buffer.BlockCopy(m_SendBuffer.m_Buffer, MAX_SEND_BUFFER, m_SendBuffer.m_Buffer, 0, delta);
					}

					m_SendBuffer.m_Left += PACKET_HEADER + length;

					if(m_SendBuffer.m_Sending) return true;

					m_SendBuffer.m_Sending = true;
					int sendSize = MAX_SEND_BUFFER - m_SendBuffer.m_Head < m_SendBuffer.m_Left ? MAX_SEND_BUFFER - m_SendBuffer.m_Head : m_SendBuffer.m_Left;
					m_Socket.BeginSend(m_SendBuffer.m_Buffer, m_SendBuffer.m_Head, sendSize, SocketFlags.None, new AsyncCallback(OnSend), null);

					return true;
				}
			}
			catch(System.Exception e)
			{
				OnException(e);
				return false;
			}
		}
		private void OnSend(IAsyncResult ar)
		{
			try
			{
				lock(m_SendBuffer)
				{
					if(!IsConnected()) return;

					int length = m_Socket.EndSend(ar);

					m_SendBuffer.m_Left -= length;

					m_SendBuffer.m_Head += length;
					if(m_SendBuffer.m_Head >= MAX_SEND_BUFFER) m_SendBuffer.m_Head = 0;

					if(m_SendBuffer.m_Left > 0)
					{
						int sendSize = MAX_SEND_BUFFER - m_SendBuffer.m_Head < m_SendBuffer.m_Left ? MAX_SEND_BUFFER - m_SendBuffer.m_Head : m_SendBuffer.m_Left;
						m_Socket.BeginSend(m_SendBuffer.m_Buffer, m_SendBuffer.m_Head, sendSize, SocketFlags.None, new AsyncCallback(OnSend), null);
						return;
					}

					m_SendBuffer.m_Sending = false;
				}
			}
			catch(System.Exception e)
			{
				OnException(e);
			}
		}

		private void OnException(Exception e, bool bClose = true)
		{
			lock(m_EventQueue) { m_EventQueue.Enqueue(new SNetEventException(this, e)); }
			if(bClose) Close();
		}

		public bool Read(INetSerialize readMsg, byte[] packet)
		{
			SNetReader reader = new SNetReader(packet);
			//TODO : 예외처리
			ushort usProtocol = 0;
			reader.Read(ref usProtocol);
			readMsg.Read(reader);
			return true;
		}
		public bool Write(INetSerialize writeMsg, int buffSize = 256)
		{
			byte[] packet = new byte[buffSize];
			SNetWriter writer = new SNetWriter(packet);
			//TODO : 예외처리
			writer.Write(writeMsg.GetProtocol());
			writeMsg.Write(writer);
			return Send(packet, writer.GetBufferLength());
		}
	}

	static public class SNetUtil
	{
		static public object ByteToStructure(byte[] buff, Type type)
		{
			IntPtr pBuff = Marshal.AllocHGlobal(buff.Length); //배열의 크기만큼 비관리 메모리 영역에 메모리를 할당
			Marshal.Copy(buff, 0, pBuff, buff.Length); //배열에 저장된 데이터를 위에서 할당한 메모리 영역에 복사
			object obj = Marshal.PtrToStructure(pBuff, type); //복사된 데이터를 구조체 객체로 변환
			Marshal.FreeHGlobal(pBuff); //비관리 메모리 영역에 할당했던 메모리를 해제
			return obj; //구조체 리턴
		}
		static public byte[] StructureToByte(object obj)
		{
			int buffSize = Marshal.SizeOf(obj); //구조체에 할당된 메모리의 크기
			IntPtr pBuff = Marshal.AllocHGlobal(buffSize); //비관리 메모리 영역에 구조체 크기만큼의 메모리를 할당
			Marshal.StructureToPtr(obj, pBuff, false); //할당된 구조체 객체의 주소
			byte[] buff = new byte[buffSize]; //구조체가 복사될 배열
			Marshal.Copy(pBuff, buff, 0, buffSize); //구조체 객체를 배열에 복사
			Marshal.FreeHGlobal(pBuff); //비관리 메모리 영역에 할당했던 메모리를 해제함
			return buff; //배열을 리턴
		}
	}

	public interface INetSerialize
	{
		ushort GetProtocol();
		void Read(SNetReader reader);
		void Write(SNetWriter writer);
	}

	public class SNetReader
	{
		private byte[] m_Buff;
		private ushort m_Position;

		public SNetReader(byte[] buff) { m_Buff = buff; }
		public ushort GetBufferLength() { return m_Position; }

		public void Read(ref bool value)
		{
			value = BitConverter.ToBoolean(m_Buff, m_Position);
			m_Position += 1;
		}
		public void Read(ref sbyte value)
		{
			value = (sbyte)m_Buff[m_Position];
			m_Position += 1;
		}
		public void Read(ref byte value)
		{
			value = m_Buff[m_Position];
			m_Position += 1;
		}
		public void Read(ref short value)
		{
			value = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(m_Buff, m_Position));
			m_Position += 2;
		}
		public void Read(ref ushort value)
		{
			value = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(m_Buff, m_Position));
			m_Position += 2;
		}
		public void Read(ref int value)
		{
			value = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(m_Buff, m_Position));
			m_Position += 4;
		}
		public void Read(ref uint value)
		{
			value = (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(m_Buff, m_Position));
			m_Position += 4;
		}
		public void Read(ref long value)
		{
			value = IPAddress.NetworkToHostOrder(BitConverter.ToInt64(m_Buff, m_Position));
			m_Position += 8;
		}
		public void Read(ref ulong value)
		{
			value = (ulong)IPAddress.NetworkToHostOrder(BitConverter.ToInt64(m_Buff, m_Position));
			m_Position += 8;
		}
		public void Read(ref float value)
		{
			int temp = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(m_Buff, m_Position));
			byte[] block = new byte[4];
			Buffer.BlockCopy(BitConverter.GetBytes(temp), 0, block, 0, 4);
			value = BitConverter.ToSingle(block, 0);
			m_Position += 4;
		}
		public void Read(ref double value)
		{
			long temp = IPAddress.NetworkToHostOrder(BitConverter.ToInt64(m_Buff, m_Position));
			byte[] block = new byte[8];
			Buffer.BlockCopy(BitConverter.GetBytes(temp), 0, block, 0, 8);
			value = BitConverter.ToDouble(block, 0);
			m_Position += 8;
		}
		public void Read(ref string value)
		{
			ushort usLength = 0;
			Read(ref usLength);
			value = Encoding.Unicode.GetString(m_Buff, m_Position, usLength);
			m_Position += usLength;
		}
		public void Read(ref byte[] value)
		{
			ushort usLength = 0;
			Read(ref usLength);
			if(usLength > 0)
			{
				value = new byte[usLength];
				Buffer.BlockCopy(m_Buff, m_Position, value, 0, usLength);
				m_Position += usLength;
			}
		}
		public void Read(INetSerialize value)
		{
			value.Read(this);
		}
	}

	public class SNetWriter
	{
		byte[] m_Buff;
		ushort m_Position;

		public SNetWriter(byte[] buff) { m_Buff = buff; }
		public ushort GetBufferLength() { return m_Position; }

		public void Write(bool value)
		{
			Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_Buff, m_Position, 1);
			m_Position += 1;
		}
		public void Write(sbyte value)
		{
			m_Buff[m_Position] = (byte)value;
			m_Position += 1;
		}
		public void Write(byte value)
		{
			m_Buff[m_Position] = value;
			m_Position += 1;
		}
		public void Write(short value)
		{
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)), 0, m_Buff, m_Position, 2);
			m_Position += 2;
		}
		public void Write(ushort value)
		{
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)value)), 0, m_Buff, m_Position, 2);
			m_Position += 2;
		}
		public void Write(int value)
		{
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)), 0, m_Buff, m_Position, 4);
			m_Position += 4;
		}
		public void Write(uint value)
		{
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)value)), 0, m_Buff, m_Position, 4);
			m_Position += 4;
		}
		public void Write(long value)
		{
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)), 0, m_Buff, m_Position, 8);
			m_Position += 8;
		}
		public void Write(ulong value)
		{
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)value)), 0, m_Buff, m_Position, 8);
			m_Position += 8;
		}
		public void Write(float value)
		{
			byte[] block = new byte[4];
			Buffer.BlockCopy(BitConverter.GetBytes(value), 0, block, 0, 4);
			int temp = BitConverter.ToInt32(block, 0);
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(temp)), 0, m_Buff, m_Position, 4);
			m_Position += 4;
		}
		public void Write(double value)
		{
			byte[] block = new byte[8];
			Buffer.BlockCopy(BitConverter.GetBytes(value), 0, block, 0, 8);
			long temp = BitConverter.ToInt64(block, 0);
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(temp)), 0, m_Buff, m_Position, 8);
			m_Position += 8;
		}
		public void Write(string value)
		{
			ushort usLength = (ushort)(value.Length * 2);
			Write(usLength);
			Buffer.BlockCopy(Encoding.Unicode.GetBytes(value), 0, m_Buff, m_Position, usLength);
			m_Position += usLength;
		}
		public void Write(byte[] value)
		{
			ushort usLength = (ushort)(value == null ? 0 : value.Length);
			Write(usLength);
			if(usLength > 0) Buffer.BlockCopy(value, 0, m_Buff, m_Position, usLength);
			m_Position += usLength;
		}
		public void Write(INetSerialize value)
		{
			value.Write(this);
		}
	}
}

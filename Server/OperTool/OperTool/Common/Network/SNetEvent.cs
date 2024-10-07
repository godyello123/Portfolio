using System;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SNetwork
{
	public enum EventType { Close, Accept, Connect, Recv, Exception }
	public enum CloseType { Normal, Timeout }

	public interface INetEvent
	{
		EventType GetEventType();
	}

	public class SNetEventClose : INetEvent
	{
		public long SessionKey { get; set; }
		public CloseType Type { get; set; }

		public SNetEventClose(long sessionKey, CloseType closeType)
		{
			SessionKey = sessionKey;
			Type = closeType;
		}
		public EventType GetEventType() { return EventType.Close; }
	}

	public class SNetEventAccept : INetEvent
	{
		public TcpClient Session { get; set; }

		public SNetEventAccept(TcpClient session)
		{
			Session = session;
		}
		public EventType GetEventType() { return EventType.Accept; }
	}

	public class SNetEventConnect : INetEvent
	{
		public bool Result { get; set; }
		public TcpClient Session { get; set; }
		public long SessionKey { get; set; }

		public SNetEventConnect(bool result, TcpClient session, long sessionKey)
		{
			Result = result;
			Session = session;
			SessionKey = sessionKey;
		}
		public EventType GetEventType() { return EventType.Connect; }
	}

	public class SNetEventRecv : INetEvent
	{
		public long SessionKey { get; set; }

		public ArraySegment<byte> Packet { get; set; }
		//public byte[] Packet { get; set; }

		public SNetEventRecv(long sessionKey, ArraySegment<byte> packet)
		{
			SessionKey = sessionKey;
			Packet = packet;
		}
		public EventType GetEventType() { return EventType.Recv; }
	}

	public class SNetEventException : INetEvent
	{
		public long SessionKey { get; set; }
		public Exception Except { get; set; }

		public SNetEventException(long sessionKey, Exception e)
		{
			SessionKey = sessionKey;
			Except = e;
		}
		public EventType GetEventType() { return EventType.Exception; }
	}

	public class SNetEventQueue
	{
		private Queue<INetEvent> m_EventQueue = new Queue<INetEvent>();

		public void Push(INetEvent netEvent)
		{
			lock(m_EventQueue) { m_EventQueue.Enqueue(netEvent); }
		}

		public INetEvent Pop()
		{
			lock(m_EventQueue)
			{
				if(m_EventQueue.Count <= 0) return null;
				return m_EventQueue.Dequeue();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace SNetwork
{
    public class SCircularQueue
	{
		class SBuffer
		{
			private int m_Head;
			private int m_Tail;
			private int m_Load;
			private ArraySegment<byte> m_Buffer;
			private bool m_bReturned = false;

			public SBuffer(int size) 
			{
				m_Buffer = SCommon.SMemoryPool.Instance.Rent(size);
			}

			public void Return()
            {
				if (!m_bReturned)
				{
					m_bReturned = true;
					SCommon.SMemoryPool.Instance.Return(m_Buffer);
				}
            }
			public int GetLoad() { return m_Load; }
			public int GetLeft() { return m_Buffer.Count - m_Load; }
            public void Push(byte[] buff, int offset, int length)
			{
                if (GetLeft() < length) throw new Exception("SCircularQueue.Push length Error");

				int size = Math.Min(length, m_Buffer.Count - m_Tail);
				Buffer.BlockCopy(buff, offset, m_Buffer.Array, m_Buffer.Offset + m_Tail, size);
				m_Load += size;
				m_Tail += size;
				if(m_Tail >= m_Buffer.Count) m_Tail -= m_Buffer.Count;
				if(size < length) Push(buff, offset + size, length - size);
			}
			public void Pop(int length)
			{
				if(m_Load < length) throw new Exception("SCircularQueue.Pop Error");
				m_Load -= length;
				m_Head += length;
				if(m_Head >= m_Buffer.Count) m_Head -= m_Buffer.Count;
			}
			public void GetBuffer(out byte[] buff, out int offset, out int length)
			{
				buff = null;
				offset = length = 0;
				if(m_Load <= 0) return;
				buff = m_Buffer.AsSpan().ToArray();
				offset = m_Head;
				length = Math.Min(m_Load, m_Buffer.Count - m_Head);
			}
		}

		private int m_Size;
		List<SBuffer> m_BufferList = new List<SBuffer>();

		public SCircularQueue(int size)
		{
			m_Size = size;
			m_BufferList.Add(new SBuffer(m_Size));
		}

		public void Return()
        {
			foreach (var data in m_BufferList)
				data.Return();
        }
		public int GetLoad()
		{
			lock(this)
			{
				int load = 0;
				foreach(SBuffer buffer in m_BufferList) load += buffer.GetLoad();
				return load;
			}
		}
		public void Push(byte[] buff, int offset, int length)
		{
			lock(this)
			{
				int left = m_BufferList[m_BufferList.Count - 1].GetLeft();
				if(left <= 0)
				{
					m_BufferList.Add(new SBuffer(m_Size));
					left = m_BufferList[m_BufferList.Count - 1].GetLeft();
				}

                int size = Math.Min(length, left);
				m_BufferList[m_BufferList.Count - 1].Push(buff, offset, size);
				if(size < length) Push(buff, offset + size, length - size);
			}
		}
		public void Pop(int length)
		{
			lock (this)
			{
				m_BufferList[0].Pop(length);
				if (m_BufferList.Count > 1 && m_BufferList[0].GetLoad() == 0)
				{
					m_BufferList[0].Return();
					m_BufferList.RemoveAt(0);
				};
			}
		}
		public void GetBuffer(out byte[] buff, out int offset, out int length)
		{
			lock(this) 
			{ 
				m_BufferList[0].GetBuffer(out buff, out offset, out length); 
			}
		}
    }
}

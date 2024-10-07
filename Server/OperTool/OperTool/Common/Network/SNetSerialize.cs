using System;
using System.Text;
using System.Collections.Generic;
using System.Net;

namespace SNetwork
{
	public enum CryptType { None, XOR, AES };

	public interface INetSerialize
	{
		void Read(SNetReader reader);
		void Write(SNetWriter writer);
	}

	public interface INetPacket : INetSerialize
	{
		CryptType GetCrypt();
		ushort GetProtocol();
	}

	//public class ProtobufPacket
	//{
	//	public Google.Protobuf.IMessage Msg { get; set; }
	//}

	public class SNetReader
	{
		private byte[] m_Buff;
		private ushort m_Position;
		private bool m_Result = true;

		public SNetReader(byte[] buff) { m_Buff = buff; }
		public bool GetResult() { return m_Result; }
		public ushort GetBufferLength() { return m_Position; }

		public void Crypt(CryptType crypt)
		{
			const int PROTOCOL = sizeof(ushort);
			if(m_Buff.Length <= PROTOCOL || crypt == CryptType.None)
			{
				return;
			}
			else if(crypt == CryptType.XOR)
			{
				SXOR.Crypt(m_Buff, PROTOCOL, m_Buff.Length - PROTOCOL);
			}
			else
			{
				byte[] decrypted = SAES.Instance.Decrypt(m_Buff, PROTOCOL, m_Buff.Length - PROTOCOL);
				Buffer.BlockCopy(decrypted, 0, m_Buff, PROTOCOL, decrypted.Length);
			}
		}
		private bool Check(int length)
		{
			m_Result = m_Result && m_Position + length <= m_Buff.Length;
			return m_Result;
		}
		public bool Read(ref bool value)
		{
			if(!Check(1)) return false;
			value = BitConverter.ToBoolean(m_Buff, m_Position);
			m_Position += 1;
			return true;
		}
		public bool Read(ref sbyte value)
		{
			if(!Check(1)) return false;
			value = (sbyte)m_Buff[m_Position];
			m_Position += 1;
			return true;
		}
		public bool Read(ref byte value)
		{
			if(!Check(1)) return false;
			value = m_Buff[m_Position];
			m_Position += 1;
			return true;
		}
		public bool Read(ref short value)
		{
			if(!Check(2)) return false;
			value = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(m_Buff, m_Position));
			m_Position += 2;
			return true;
		}
		public bool Read(ref ushort value)
		{
			if(!Check(2)) return false;
			value = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(m_Buff, m_Position));
			m_Position += 2;
			return true;
		}
		public bool Read(ref int value)
		{
			if(!Check(4)) return false;
			value = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(m_Buff, m_Position));
			m_Position += 4;
			return true;
		}
		public bool Read(ref uint value)
		{
			if(!Check(4)) return false;
			value = (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(m_Buff, m_Position));
			m_Position += 4;
			return true;
		}
		public bool Read(ref long value)
		{
			if(!Check(8)) return false;
			value = IPAddress.NetworkToHostOrder(BitConverter.ToInt64(m_Buff, m_Position));
			m_Position += 8;
			return true;
		}
		public bool Read(ref ulong value)
		{
			if(!Check(8)) return false;
			value = (ulong)IPAddress.NetworkToHostOrder(BitConverter.ToInt64(m_Buff, m_Position));
			m_Position += 8;
			return true;
		}
		public bool Read(ref float value)
		{
			if(!Check(4)) return false;
			int temp = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(m_Buff, m_Position));
			byte[] block = new byte[4];
			Buffer.BlockCopy(BitConverter.GetBytes(temp), 0, block, 0, 4);
			value = BitConverter.ToSingle(block, 0);
			m_Position += 4;
			return true;
		}
		public bool Read(ref double value)
		{
			if(!Check(8)) return false;
			long temp = IPAddress.NetworkToHostOrder(BitConverter.ToInt64(m_Buff, m_Position));
			byte[] block = new byte[8];
			Buffer.BlockCopy(BitConverter.GetBytes(temp), 0, block, 0, 8);
			value = BitConverter.ToDouble(block, 0);
			m_Position += 8;
			return true;
		}

        public bool Read(ref System.Numerics.BigInteger value)
        {
            string str = string.Empty;
            if (!Read(ref str))
                return false;

            return System.Numerics.BigInteger.TryParse(str, out value);
        }

        public bool Read(ref string value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length <= 0)
			{
				value = "";
			}
			else
			{
				if(!Check(length)) return false;
				value = Encoding.Unicode.GetString(m_Buff, m_Position, length);
				m_Position += length;
			}
			return true;
		}
		public bool Read(ref byte[] value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length <= 0)
			{
				value = new byte[0];
			}
			else
			{
				if(!Check(length)) return false;
				value = new byte[length];
				Buffer.BlockCopy(m_Buff, m_Position, value, 0, length);
				m_Position += length;
			}
			return true;
		}
		public bool Read(ref DateTime value)
		{
			if(!Check(8)) return false;
			long temp = IPAddress.NetworkToHostOrder(BitConverter.ToInt64(m_Buff, m_Position));
			byte[] block = new byte[8];
			Buffer.BlockCopy(BitConverter.GetBytes(temp), 0, block, 0, 8);
			double second = BitConverter.ToDouble(block, 0);

			if(second == double.MinValue) 
				value = DateTime.MinValue;
			else if(second == double.MaxValue) 
				value = DateTime.MaxValue;
			else 
				value = DateTime.MinValue.AddSeconds(second);

			m_Position += 8;
			return true;
		}

		public bool ReadEnum<T>(ref T rOut) where T : struct, IConvertible
		{
			int iVal = 0;
			if (!Read(ref iVal))
				return false;

			rOut = (T)(object)iVal;

			return true;
        }

		public bool Read<T>(ref T value) where T : INetSerialize
		{
			value.Read(this);
			return GetResult();
		}

		public bool Read(ref List<bool> value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length > 0)
			{
				value = new List<bool>();
				for(int i = 0; i < length; i++)
				{
					bool e = false;
					if(!Read(ref e)) return false;
					value.Add(e);
				}
			}
			return true;
		}
		public bool Read(ref List<sbyte> value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length > 0)
			{
				value = new List<sbyte>();
				for(int i = 0; i < length; i++)
				{
					sbyte e = 0;
					if(!Read(ref e)) return false;
					value.Add(e);
				}
			}
			return true;
		}
		public bool Read(ref List<byte> value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length > 0)
			{
				value = new List<byte>();
				for(int i = 0; i < length; i++)
				{
					byte e = 0;
					if(!Read(ref e)) return false;
					value.Add(e);
				}
			}
			return true;
		}
		public bool Read(ref List<short> value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length > 0)
			{
				value = new List<short>();
				for(int i = 0; i < length; i++)
				{
					short e = 0;
					if(!Read(ref e)) return false;
					value.Add(e);
				}
			}
			return true;
		}
		public bool Read(ref List<ushort> value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length > 0)
			{
				value = new List<ushort>();
				for(int i = 0; i < length; i++)
				{
					ushort e = 0;
					if(!Read(ref e)) return false;
					value.Add(e);
				}
			}
			return true;
		}
		public bool Read(ref List<int> value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length > 0)
			{
				value = new List<int>();
				for(int i = 0; i < length; i++)
				{
					int e = 0;
					if(!Read(ref e)) return false;
					value.Add(e);
				}
			}
			return true;
		}
		public bool Read(ref List<uint> value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length > 0)
			{
				value = new List<uint>();
				for(int i = 0; i < length; i++)
				{
					uint e = 0;
					if(!Read(ref e)) return false;
					value.Add(e);
				}
			}
			return true;
		}
		public bool Read(ref List<float> value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length > 0)
			{
				value = new List<float>();
				for(int i = 0; i < length; i++)
				{
					float e = 0;
					if(!Read(ref e)) return false;
					value.Add(e);
				}
			}
			return true;
		}
		public bool Read(ref List<double> value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length > 0)
			{
				value = new List<double>();
				for(int i = 0; i < length; i++)
				{
					double e = 0;
					if(!Read(ref e)) return false;
					value.Add(e);
				}
			}
			return true;
		}
		public bool Read(ref List<string> value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length > 0)
			{
				value = new List<string>();
				for(int i = 0; i < length; i++)
				{
					string e = "";
					if(!Read(ref e)) return false;
					value.Add(e);
				}
			}
			return true;
		}
		public bool Read(ref List<byte[]> value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length > 0)
			{
				value = new List<byte[]>();
				for(int i = 0; i < length; i++)
				{
					byte[] e = null;
					if(!Read(ref e)) return false;
					value.Add(e);
				}
			}
			return true;
		}
		public bool Read(ref List<DateTime> value)
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length > 0)
			{
				value = new List<DateTime>();
				for(int i = 0; i < length; i++)
				{
					DateTime e = DateTime.MinValue;
					if(!Read(ref e)) return false;
					value.Add(e);
				}
			}
			return true;
		}
		public bool Read<T>(ref List<T> value) where T : INetSerialize, new()
		{
			ushort length = 0;
			if(!Read(ref length)) return false;
			if(length > 0)
			{
				value = new List<T>();
				for(int i = 0; i < length; i++)
				{
					T e = new T();
					if(!Read(ref e)) return false;
					value.Add(e);
				}
			}
			return true;
		}
	}

	public class SNetWriter
	{
		private ArraySegment<byte> m_Buff;
		//private byte[] m_Buff;
		private ushort m_Position = 2;
		private bool m_Result = true;
		private bool m_bReturned = false;

		public SNetWriter(int buffSize) 
		{
			m_Buff = SCommon.SMemoryPool.Instance.Rent(buffSize);
			//m_Buff = new byte[buffSize]; 
		}

		public void Return()
        {
            if (!m_bReturned)
            {
                m_bReturned = true;
                SCommon.SMemoryPool.Instance.Return(m_Buff);
            }
        }
        public bool GetResult() { return m_Result; }
		//public byte[] GetBuffer() { return m_Buff; }
		public ArraySegment<byte> GetBuffer() { return m_Buff; }
		public ushort GetBufferLength() { return m_Position; }

		public void Crypt(CryptType crypt)
		{
			const int PROTOCOL = sizeof(ushort);
			if(m_Position <= PROTOCOL || crypt == CryptType.None)
			{
				return;
			}
			else if(crypt == CryptType.XOR)
			{
				SXOR.Crypt(m_Buff.Array, m_Buff.Offset + PROTOCOL, m_Position - PROTOCOL);
				//SXOR.Crypt(m_Buff, PROTOCOL, m_Position - PROTOCOL);
			}
			else
			{
				//byte[] encrypted = SAES.Instance.Encrypt(m_Buff, PROTOCOL, m_Position - PROTOCOL);
				byte[] encrypted = SAES.Instance.Encrypt(m_Buff.Array, m_Buff.Offset + PROTOCOL, m_Position - PROTOCOL);
				if (m_Buff.Count < encrypted.Length + PROTOCOL)
				{
					var newBuff = SCommon.SMemoryPool.Instance.Rent(encrypted.Length + PROTOCOL);
					//byte[] buff = new byte[encrypted.Length + PROTOCOL];

					Buffer.BlockCopy(m_Buff.Array, m_Buff.Offset, newBuff.Array, newBuff.Offset, PROTOCOL);
					SCommon.SMemoryPool.Instance.Return(m_Buff);
					m_Buff = newBuff;
				}
				Buffer.BlockCopy(encrypted, 0, m_Buff.Array, m_Buff.Offset + PROTOCOL, encrypted.Length);
				m_Position = (ushort)(encrypted.Length + PROTOCOL);
			}
		}
		private bool Check(int length)
		{
			m_Result = m_Result && m_Position + length <= m_Buff.Count;
			return m_Result;
		}
		private bool Resize()
		{
			if(m_Buff.Count * 2 > SNetSession.MAX_PACKET) return false;
			m_Result = true;

			var newBuff = SCommon.SMemoryPool.Instance.Rent(m_Buff.Count * 2);

			//byte[] buff = new byte[m_Buff.Length * 2];
			//Buffer.BlockCopy(m_Buff, 0, buff, 0, m_Buff.Length);
			Buffer.BlockCopy(m_Buff.Array, m_Buff.Offset, newBuff.Array, newBuff.Offset, m_Buff.Count);
			SCommon.SMemoryPool.Instance.Return(m_Buff);

			m_Buff = newBuff;
			//m_Buff = buff;
			return true;
		}

        public bool WriteLength()
        {
            ushort length = (ushort)(m_Position - 2);
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)length)), 0, m_Buff.Array, m_Buff.Offset, 2);
            return true;
        }
        public bool Write(bool value)
		{
			while(!Check(1)) if(!Resize()) return false;
			Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_Buff.Array, m_Buff.Offset + m_Position, 1);
			m_Position += 1;
			return true;
		}
        
        public bool Write(sbyte value)
		{
			while(!Check(1)) if(!Resize()) return false;
			m_Buff.Array[m_Buff.Offset + m_Position] = (byte)value;
			//m_Buff[m_Position] = (byte)value;
			m_Position += 1;
			return true;
		}
		public bool Write(byte value)
		{
			while(!Check(1)) if(!Resize()) return false;
			m_Buff.Array[m_Buff.Offset + m_Position] = (byte)value;
			//m_Buff[m_Position] = value;
			m_Position += 1;
			return true;
		}
		public bool Write(short value)
		{
			while(!Check(2)) if(!Resize()) return false;
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)), 0, m_Buff.Array, m_Buff.Offset + m_Position, 2);
			m_Position += 2;
			return true;
		}
		public bool Write(ushort value)
		{
			while(!Check(2)) if(!Resize()) return false;
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)value)), 0, m_Buff.Array, m_Buff.Offset + m_Position, 2);
			m_Position += 2;
			return true;
		}
		public bool Write(int value)
		{
			while(!Check(4)) if(!Resize()) return false;
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)), 0, m_Buff.Array, m_Buff.Offset + m_Position, 4);
			m_Position += 4;
			return true;
		}
		public bool Write(uint value)
		{
			while(!Check(4)) if(!Resize()) return false;
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)value)), 0, m_Buff.Array, m_Buff.Offset + m_Position, 4);
			m_Position += 4;
			return true;
		}
		public bool Write(long value)
		{
			while(!Check(8)) if(!Resize()) return false;
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)), 0, m_Buff.Array, m_Buff.Offset + m_Position, 8);
			m_Position += 8;
			return true;
		}
		public bool Write(ulong value)
		{
			while(!Check(8)) if(!Resize()) return false;
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)value)), 0, m_Buff.Array, m_Buff.Offset + m_Position, 8);
			m_Position += 8;
			return true;
		}
		public bool Write(float value)
		{
			while(!Check(4)) if(!Resize()) return false;
			byte[] block = new byte[4];
			Buffer.BlockCopy(BitConverter.GetBytes(value), 0, block, 0, 4);
			int temp = BitConverter.ToInt32(block, 0);
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(temp)), 0, m_Buff.Array, m_Buff.Offset + m_Position, 4);
			m_Position += 4;
			return true;
		}
		public bool Write(double value)
		{
			while(!Check(8)) if(!Resize()) return false;
			byte[] block = new byte[8];
			Buffer.BlockCopy(BitConverter.GetBytes(value), 0, block, 0, 8);
			long temp = BitConverter.ToInt64(block, 0);
			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(temp)), 0, m_Buff.Array, m_Buff.Offset + m_Position, 8);
			m_Position += 8;
			return true;
		}

   //     public bool Write(System.Numerics.BigInteger value)
   //     {
			////return Write(value.ToString());
   //     }

        public bool Write(string value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Length * 2);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) Buffer.BlockCopy(Encoding.Unicode.GetBytes(value), 0, m_Buff.Array, m_Buff.Offset + m_Position, length);
			m_Position += length;
			return true;
		}
		public bool Write(byte[] value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Length);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) Buffer.BlockCopy(value, 0, m_Buff.Array, m_Buff.Offset + m_Position, length);
			m_Position += length;
			return true;
		}
		public bool Write(DateTime value)
		{
			while(!Check(8)) 
				if(!Resize()) 
					return false;

			byte[] block = new byte[8];

			double second = 0;

			if(value == DateTime.MinValue) 
				second = double.MinValue;
			else if(value == DateTime.MaxValue) 
				second = double.MaxValue;
			else 
				second = (value - DateTime.MinValue).TotalSeconds;

			Buffer.BlockCopy(BitConverter.GetBytes(second), 0, block, 0, 8);

			long temp = BitConverter.ToInt64(block, 0);

			Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(temp)), 0, m_Buff.Array, m_Buff.Offset + m_Position, 8);

			m_Position += 8;
			return true;
		}
		public bool Write(INetSerialize value)
		{
			value.Write(this);
			return GetResult();
		}

		public bool Write(List<bool> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<sbyte> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<byte> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<short> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<ushort> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<int> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<uint> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<long> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<ulong> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<float> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<double> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<string> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<byte[]> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<DateTime> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}
		public bool Write(List<INetSerialize> value)
		{
			ushort length = (ushort)(value == null ? 0 : value.Count);
			Write(length);
			while(!Check(length)) if(!Resize()) return false;
			if(length > 0) foreach(var e in value) if(!Write(e)) return false;
			return true;
		}

        public bool Write<T>(List<T> value) where T : INetSerialize, new()
        {
            ushort length = (ushort)(value == null ? 0 : value.Count);
            Write(length);
            while (!Check(length)) if (!Resize()) return false;
            if (length > 0) foreach (var e in value) if (!Write(e)) return false;
            return true;
        }
        public bool WriteEnum<T>(T value) where T : struct, IConvertible
        {
			return Write((int)(object)value);
        }

    }
}

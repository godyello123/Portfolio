using System;
using System.IO;
using System.Collections.Generic;

namespace SCommon
{
	public class ExcelBin
	{
		private class Field
		{
			public int m_Index;
			public string m_Name;
			public bool m_IsString;

			public Field(int Index, string Name, bool IsString)
			{
				m_Index = Index;
				m_Name = Name;
				m_IsString = IsString;
			}
		}

		private string m_FilePath;
		public string FilePath { get { return m_FilePath; } }
		private int m_RecordCount;
		public int RecordCount { get { return m_RecordCount; } }
		private int m_FieldCount;
		public int FieldCount { get { return m_FieldCount; } }
		private Dictionary<string, Field> m_FieldTable = new Dictionary<string, Field>();
		private int[] m_OffsetBuffer;
		private byte[] m_DataBuffer;

		private void OnError(string ErrorMsg)
		{
			System.Windows.Forms.MessageBox.Show(ErrorMsg);
		}

		private int GetDataOffset(int RecordIndex, string FieldName)
		{
			if(RecordIndex >= m_RecordCount) return int.MaxValue;
			FieldName = FieldName.ToLower();
			if(!m_FieldTable.ContainsKey(FieldName)) return int.MaxValue;
			return m_OffsetBuffer[RecordIndex * m_FieldCount + m_FieldTable[FieldName].m_Index];
		}

		public bool Load(string FilePath)
		{
			m_FilePath = FilePath;

			FileStream Stream = null;
			try
			{
				Stream = File.OpenRead(FilePath);
			}
			catch
			{
				OnError("File open failed - " + FilePath);
				return false;
			}

			try
			{
				BinaryReader Reader = new BinaryReader(Stream);
				m_FieldCount = Reader.ReadUInt16();

				for(int i = 0; i < m_FieldCount; i++)
				{
					sbyte Type = Reader.ReadSByte();
					sbyte Len = Reader.ReadSByte();
					if(Len == 0) continue;

					byte[] Temp = new byte[Len];
					Reader.Read(Temp, 0, Temp.Length);
					string Name = System.Text.Encoding.ASCII.GetString(Temp).ToLower();

					m_FieldTable.Add(Name, new Field(i, Name, (Type == 0) ? false : true));
				}

				m_RecordCount = Reader.ReadUInt16();

				m_OffsetBuffer = new int[FieldCount * RecordCount];
				for(int i = 0; i < m_OffsetBuffer.Length; i++) m_OffsetBuffer[i] = Reader.ReadInt32();

				uint DataSize = Reader.ReadUInt32();
				m_DataBuffer = new byte[DataSize];
				Reader.Read(m_DataBuffer, 0, m_DataBuffer.Length);
			}
			catch
			{
				OnError("File load failed - " + FilePath);
				Stream.Close();
				return false;
			}

			Stream.Close();
			return true;
		}

		public bool GetBool(int RecordIndex, string FieldName)
		{
			try
			{
				int Offset = GetDataOffset(RecordIndex, FieldName);
				float Value = BitConverter.ToSingle(m_DataBuffer, Offset);
				return Value != 0.0f;
			}
			catch
			{
				OnError("GetBool failed - " + FilePath + ", FieldName(" + FieldName + "), RecordIndex(" + RecordIndex + ")");
				return false;
			}
		}
		public sbyte GetSByte(int RecordIndex, string FieldName)
		{
			try
			{
				int Offset = GetDataOffset(RecordIndex, FieldName);
				float Value = BitConverter.ToSingle(m_DataBuffer, Offset);
				return (sbyte)Value;
			}
			catch
			{
				OnError("GetSByte failed - " + FilePath + ", FieldName(" + FieldName + "), RecordIndex(" + RecordIndex + ")");
				return 0;
			}
		}
		public byte GetByte(int RecordIndex, string FieldName)
		{
			try
			{
				int Offset = GetDataOffset(RecordIndex, FieldName);
				float Value = BitConverter.ToSingle(m_DataBuffer, Offset);
				return (byte)Value;
			}
			catch
			{
				OnError("GetByte failed - " + FilePath + ", FieldName(" + FieldName + "), RecordIndex(" + RecordIndex + ")");
				return 0;
			}
		}
		public short GetShort(int RecordIndex, string FieldName)
		{
			try
			{
				int Offset = GetDataOffset(RecordIndex, FieldName);
				float Value = BitConverter.ToSingle(m_DataBuffer, Offset);
				return (short)Value;
			}
			catch
			{
				OnError("GetShort failed - " + FilePath + ", FieldName(" + FieldName + "), RecordIndex(" + RecordIndex + ")");
				return 0;
			}
		}
		public ushort GetUShort(int RecordIndex, string FieldName)
		{
			try
			{
				int Offset = GetDataOffset(RecordIndex, FieldName);
				float Value = BitConverter.ToSingle(m_DataBuffer, Offset);
				return (ushort)Value;
			}
			catch
			{
				OnError("GetUShort failed - " + FilePath + ", FieldName(" + FieldName + "), RecordIndex(" + RecordIndex + ")");
				return 0;
			}
		}
		public int GetInt(int RecordIndex, string FieldName)
		{
			try
			{
				int Offset = GetDataOffset(RecordIndex, FieldName);
				float Value = BitConverter.ToSingle(m_DataBuffer, Offset);
				return (int)Value;
			}
			catch
			{
				OnError("GetInt failed - " + FilePath + ", FieldName(" + FieldName + "), RecordIndex(" + RecordIndex + ")");
				return 0;
			}
		}
		public uint GetUInt(int RecordIndex, string FieldName)
		{
			try
			{
				int Offset = GetDataOffset(RecordIndex, FieldName);
				float Value = BitConverter.ToSingle(m_DataBuffer, Offset);
				return (uint)Value;
			}
			catch
			{
				OnError("GetUInt failed - " + FilePath + ", FieldName(" + FieldName + "), RecordIndex(" + RecordIndex + ")");
				return 0;
			}
		}
		public long GetLong(int RecordIndex, string FieldName)
		{
			try
			{
				int Offset = GetDataOffset(RecordIndex, FieldName);
				float Value = BitConverter.ToSingle(m_DataBuffer, Offset);
				return (long)Value;
			}
			catch
			{
				OnError("GetLong failed - " + FilePath + ", FieldName(" + FieldName + "), RecordIndex(" + RecordIndex + ")");
				return 0;
			}
		}
		public ulong GetULong(int RecordIndex, string FieldName)
		{
			try
			{
				int Offset = GetDataOffset(RecordIndex, FieldName);
				float Value = BitConverter.ToSingle(m_DataBuffer, Offset);
				return (ulong)Value;
			}
			catch
			{
				OnError("GetULong failed - " + FilePath + ", FieldName(" + FieldName + "), RecordIndex(" + RecordIndex + ")");
				return 0;
			}
		}
		public float GetFloat(int RecordIndex, string FieldName)
		{
			try
			{
				int Offset = GetDataOffset(RecordIndex, FieldName);
				float Value = BitConverter.ToSingle(m_DataBuffer, Offset);
				return Value;
			}
			catch
			{
				OnError("GetFloat failed - " + FilePath + ", FieldName(" + FieldName + "), RecordIndex(" + RecordIndex + ")");
				return 0;
			}
		}
		public double GetDouble(int RecordIndex, string FieldName)
		{
			try
			{
				int Offset = GetDataOffset(RecordIndex, FieldName);
				float Value = BitConverter.ToSingle(m_DataBuffer, Offset);
				return (double)Value;
			}
			catch
			{
				OnError("GetDouble failed - " + FilePath + ", FieldName(" + FieldName + "), RecordIndex(" + RecordIndex + ")");
				return 0;
			}
		}
		public string GetString(int RecordIndex, string FieldName)
		{
			try
			{
				int Offset = GetDataOffset(RecordIndex, FieldName);
				int Len = 0;
				while(true)
				{
					if(m_DataBuffer[Offset + Len] == 0) break;
					Len++;
				}
				return System.Text.Encoding.ASCII.GetString(m_DataBuffer, Offset, Len);
			}
			catch
			{
				OnError("GetString failed - " + FilePath + ", FieldName(" + FieldName + "), RecordIndex(" + RecordIndex + ")");
				return "";
			}
		}
	}
}

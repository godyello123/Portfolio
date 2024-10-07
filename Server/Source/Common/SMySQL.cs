using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace SDB
{
	class SMySQL : IDisposable
	{
		private bool m_Disposed;
		private string m_ConnectionString;
		public MySqlConnection Connection { get; set; }

		~SMySQL()
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
			if(disposing && Connection != null)
			{
				Connection.Dispose();
				Connection = null;
			}
			m_Disposed = true;
		}
		public void Close()
		{
			if(Connection != null)
			{
				Connection.Close();
				Connection = null;
			}
		}
		public bool Open(string ConnectionString)
		{
			try
			{
				Close();
				m_ConnectionString = ConnectionString;
				Connection = new MySqlConnection(m_ConnectionString);
				Connection.Open();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}

			return true;
		}
		public bool Open(string Host, string DBName, string ID, string Password)
		{
			return Open(String.Format("Server={0};Database={1};Uid={2};Pwd={3}", Host, DBName, ID, Password));
		}
		public bool Reopen()
		{
			return Open(m_ConnectionString);
		}
		public bool IsOpen()
		{
			return Connection != null && Connection.State != ConnectionState.Closed;
		}
		public MySqlTransaction BeginTrans()
		{
			if(Connection == null) return null;

			try
			{
				return Connection.BeginTransaction();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}
		public void CommitTrans(MySqlTransaction Trans)
		{
			try
			{
				Trans.Commit();
				Trans.Dispose();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		public void RollbackTrans(MySqlTransaction Trans)
		{
			try
			{
				Trans.Rollback();
				Trans.Dispose();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}

	public class SMySQLCommand : IDisposable
	{
		private bool m_Disposed;
		private MySqlCommand m_Command = new MySqlCommand();
		private bool m_HasReturnValue;
		private MySqlDataReader m_DataReader;

		public SMySQLCommand(MySqlConnection Connection, bool HasReturnValue = true)
		{
			m_Command.Connection = Connection;
			m_HasReturnValue = HasReturnValue;
			if(m_HasReturnValue)
			{
				MySqlParameter Param = new MySqlParameter("", SqlDbType.Int);
				Param.Direction = ParameterDirection.ReturnValue;
				m_Command.Parameters.Add(Param);
			}
		}
		~SMySQLCommand()
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
				m_Command.Dispose();
				m_Command = null;
				if(m_DataReader != null)
				{
					m_DataReader.Dispose();
					m_DataReader = null;
				}
			}
			m_Disposed = true;
		}
		public void Close()
		{
			m_Command.Dispose();
			m_Command = null;
			if(m_DataReader != null)
			{
				m_DataReader.Close();
				m_DataReader = null;
			}
		}
		public void AddParam(string Name, object Value)
		{
			try
			{
				m_Command.Parameters.AddWithValue(Name, Value);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		public void AddOutputParam(string Name, SqlDbType Type)
		{
			try
			{
				MySqlParameter Param = new MySqlParameter(Name, Type);
				Param.Direction = ParameterDirection.Output;
				m_Command.Parameters.Add(Param);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		public object GetParam(string Name)
		{
			try
			{
				return m_Command.Parameters[Name].Value;
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}
		public object GetReturn()
		{
			try
			{
				return m_HasReturnValue ? m_Command.Parameters[0].Value : null;
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}
		public bool Excute(string Query, CommandType Type = CommandType.StoredProcedure)
		{
			try
			{
				m_Command.CommandType = Type;
				m_Command.CommandText = Query;
				m_Command.ExecuteNonQuery();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}

			return true;
		}
		public bool ExcuteReader(string Query, CommandType Type = CommandType.StoredProcedure)
		{
			try
			{
				m_Command.CommandType = Type;
				m_Command.CommandText = Query;
				m_DataReader = m_Command.ExecuteReader();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}

			return true;
		}
		public bool Read()
		{
			try
			{
				return m_DataReader.Read();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}
		}
		public int GetFieldCount()
		{
			try
			{
				return m_DataReader.FieldCount;
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return 0;
			}
		}
		public object GetField(int Index)
		{
			try
			{
				return m_DataReader[Index];
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}
		public object GetField(string Name)
		{
			try
			{
				return m_DataReader[Name];
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}
	}
}

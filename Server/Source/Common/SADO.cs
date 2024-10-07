using System;
using System.Data;
using System.Data.SqlClient;

namespace SDB
{
	public class SADO : IDisposable
	{
		private bool m_Disposed;
		private string m_ConnectionString;
		public SqlConnection Connection { get; set; }

		~SADO()
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
				Connection = new SqlConnection(m_ConnectionString);
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
			return Open(String.Format("server={0};database={1};user id={2};password={3}", Host, DBName, ID, Password));
		}
		public bool Reopen()
		{
			return Open(m_ConnectionString);
		}
		public bool IsOpen()
		{
			return Connection != null && Connection.State != ConnectionState.Closed;
		}
		public SqlTransaction BeginTrans()
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
		public void CommitTrans(SqlTransaction Trans)
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
		public void RollbackTrans(SqlTransaction Trans)
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

	public class SADOCommand : IDisposable
	{
		private bool m_Disposed;
		private SqlCommand m_Command = new SqlCommand();
		private bool m_HasReturnValue = false;
		private SqlDataReader m_DataReader;

		public SADOCommand(SqlConnection Connection, bool HasReturnValue = true)
		{
			m_Command.Connection = Connection;
			if(m_HasReturnValue)
			{
				SqlParameter Param = new SqlParameter("", SqlDbType.Int);
				Param.Direction = ParameterDirection.ReturnValue;
				m_Command.Parameters.Add(Param);
			}
		}
		~SADOCommand()
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
				SqlParameter Param = new SqlParameter(Name, Type);
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

		public bool NextResult()
        {
			try
            {
				return m_DataReader.NextResult();
			}
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void Test()
        {
			int a = m_DataReader.Depth;
			//try
   //         {
   //             return m_DataReader.NextResult();
   //         }
   //         catch (Exception e)
   //         {
   //             Console.WriteLine(e.Message);
   //             return false;
   //         }
        }
    }
}

using System;
using System.Collections.Generic;
using SCommon;

namespace Global
{
	public abstract class CLog
	{
		public string Type;
		public string UserID;
		public DateTime Date = DateTime.UtcNow;

		public CLog() { }
		public CLog(string userID)
		{
			UserID = userID;
		}
	}

	public class CLogCreate : CLog
	{
		public CLogCreate()
		{
			Type = eLogType.logcreate.ToString();
		}
		public CLogCreate(string userID)
			: base(userID)
		{
			Type = eLogType.logcreate.ToString();
		}

		public static string GetInsertQuery(List<CLogCreate> logList)
		{
			bool isFirst = true;
			string query = string.Format("insert into {0} (UserID, Date) values ", eLogType.logcreate);
			foreach(var data in logList)
			{
				if(!isFirst) query += ", ";
				query += string.Format("('{0}', {1})", data.UserID, data.Date.ToString("yyyyMMddHHmmss"));
				isFirst = false;
			}
			return query;
		}
		public static string GetSelectQuery(string userID, DateTime begin, DateTime end)
		{
			return string.Format("select Date from {0} where UserID = '{1}' and Date between {2} and {3}",
				eLogType.logcreate, userID, begin.ToString("yyyyMMddHHmmss"), end.ToString("yyyyMMddHHmmss"));
		}
	}

	public class CLogConnect : CLog
	{
		public bool Login;

		public CLogConnect()
		{
			Type = eLogType.logconnect.ToString();
		}
		public CLogConnect(string userID, bool login)
			: base(userID)
		{
			Type = eLogType.logconnect.ToString();
			Login = login;
		}

		public static string GetInsertQuery(List<CLogConnect> logList)
		{
			bool isFirst = true;
			string query = string.Format("insert into {0} (UserID, Date, Login) values ", eLogType.logconnect);
			foreach(var data in logList)
			{
				if(!isFirst) query += ", ";
				query += string.Format("('{0}', {1}, {2})", data.UserID, data.Date.ToString("yyyyMMddHHmmss"), data.Login ? 1 : 0);
				isFirst = false;
			}
			return query;
		}
		public static string GetSelectQuery(string userID, DateTime begin, DateTime end)
		{
			return string.Format("select Date, Login from {0} where UserID = '{1}' and Date between {2} and {3}",
				eLogType.logconnect, userID, begin.ToString("yyyyMMddHHmmss"), end.ToString("yyyyMMddHHmmss"));
		}
	}
}

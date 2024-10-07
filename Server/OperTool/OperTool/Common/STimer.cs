using System;

namespace SCommon
{
	public class STimer
	{
		private double m_Term;
		private DateTime m_Time;
		
		public DateTime CurTime { get { return m_Time; } }

		public STimer() { }
		public STimer(double term, bool immediately = true)
		{
			SetTimer(term, immediately);
		}

		public void SetTimer(double term, bool immediately = true)
		{
			m_Term = term;
			m_Time = immediately ? DateTime.MinValue : DateTime.UtcNow;

		}

		public bool Check()
		{
			TimeSpan elapsed = DateTime.UtcNow - m_Time;
			if(elapsed.TotalMilliseconds <= m_Term) return false;
			m_Time = DateTime.UtcNow;
			
			return true;
		}
	}

	public class SFPS
	{
		private int m_FPS;
		private int m_Count;
		private DateTime m_LastTime;
		public int FPS { get { return m_FPS; } set { m_FPS = value; } }

		public SFPS()
		{
			m_FPS = 0;
			m_Count = 0;
			m_LastTime = DateTime.UtcNow;
		}
		public bool Update()
		{
			TimeSpan elapsed = DateTime.UtcNow - m_LastTime;
			if(elapsed.TotalMilliseconds <= 1000)
			{
				m_Count++;
				return false;
			}

			m_FPS = m_Count;
			m_Count = 0;
			m_LastTime = DateTime.UtcNow;

			return true;
		}
	}
}

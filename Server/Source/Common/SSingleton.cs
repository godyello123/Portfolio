using System;

namespace SCommon
{
	public abstract class SSingleton<T> where T : class, new()
	{
		private static volatile T s_Instance;
		private static Object s_Lock = new Object();

		public static T Instance
		{
			get
			{
				if(s_Instance == null)
				{
					lock (s_Lock)
					{
						if(s_Instance == null) s_Instance = new T();
					}
				}
				return s_Instance;
			}
		}
	}
}

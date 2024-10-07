using System;
using System.Text;
using System.Runtime.InteropServices;

namespace SCommon
{
	public class SINI
	{
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string appName, string keyName, string defaultValue, StringBuilder returnedString, int size, string fileName);
		[DllImport("kernel32.dll")]
		private static extern long WritePrivateProfileString(string appName, string keyName, string value, string filePath);

		public static string ReadINI(string section, string key, string filePath, string defaultValue = "")
		{
			StringBuilder builder = new StringBuilder(1024);
			GetPrivateProfileString(section, key, defaultValue, builder, builder.Capacity, filePath);
			return builder.ToString();
		}

		public static void WriteINI(string section, string key, string value, string filePath)
		{
			WritePrivateProfileString(section, key, value, filePath);
		}
	}
}

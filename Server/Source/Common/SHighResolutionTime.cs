using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SCommon
{
	public static class SHighResolutionTime
	{
		[System.Runtime.InteropServices.DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out long perfcount);

		[System.Runtime.InteropServices.DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long freq);

		private static long m_StartCounter;
		private static long m_Frequency;

		static SHighResolutionTime()
		{
			QueryPerformanceFrequency(out m_Frequency);
		}

		public static void Start()
		{
			QueryPerformanceCounter(out m_StartCounter);
		}

		public static double GetTime()
		{
			long endCounter;
			QueryPerformanceCounter(out endCounter);
			long elapsed = endCounter - m_StartCounter;
			return (double)elapsed / m_Frequency;
		}
	}
}

using System;

namespace SCommon
{
    public class SRandom
    {
        private static SRandom m_Instance = new SRandom();
        public static SRandom Instance { get { return m_Instance; } }

        private Random m_Rand = new Random();

        public int Next() { lock (this) { return m_Rand.Next(); } }
        public int Next(int maxValue) { lock (this) { return m_Rand.Next(maxValue); } }
        public int Next(int inclusiveMinValue, int inclusiveMaxValue) { lock (this) { return m_Rand.Next(inclusiveMinValue, inclusiveMaxValue + 1); } }

		public double NextDouble() { lock(this) { return m_Rand.NextDouble(); } }
	}
}

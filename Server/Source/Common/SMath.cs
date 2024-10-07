using System;
using System.Windows.Media.Media3D;

namespace SCommon
{
	public class SMath
	{
		public static float DegreeToRadian(float degree)
		{
			return (float)Math.PI * degree / 180.0f;
		}

		public static float RadianToDegree(float radian)
		{
			return radian * (180.0f / (float)Math.PI);
		}

		public static int DegreeZeroTo360(int degree)
		{
			return ((degree % 360) + 360) % 360;
		}
	}
}

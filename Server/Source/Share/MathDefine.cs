using System;
using System.Collections.Generic;
using SNetwork;
using Global;

#if SERVER_ONLY
using System.Windows.Media.Media3D;
#elif TEST_ONLY
#elif MAIN_SERVER_ONLY
using System.Windows.Media.Media3D;
#elif OPERATION
using System.Windows.Media.Media3D;
#else
using UnityEngine;
#endif

namespace Global
{
	public struct VECTOR3 : INetSerialize
	{
		public static readonly VECTOR3 Zero = new VECTOR3(0, 0, 0);
		public static readonly VECTOR3 Up = new VECTOR3(0, 1, 0);

		public float x, y, z;

		public VECTOR3(float x, float y, float z)
		{
			this.x = x; this.y = y; this.z = z;
		}
#if SERVER_ONLY
		public VECTOR3(Vector3D v3)
		{
			x = (float)v3.X; y = (float)v3.Y; z = (float)v3.Z;
		}
		public Vector3D v { get { return new Vector3D(x, y, z); } }
#elif TEST_ONLY
#elif MAIN_SERVER_ONLY
        public VECTOR3(Vector3D v3)
        {
            x = (float)v3.X; y = (float)v3.Y; z = (float)v3.Z;
        }
        public Vector3D v { get { return new Vector3D(x, y, z); } }

#else
        public VECTOR3(Vector3 v3)
		{
			x = v3.x; y = v3.y; z = v3.z;
		}
		public Vector3 v { get { return new Vector3(x, y, z); } }
#endif

        public float Length
		{
			get
			{
				return (float)Math.Sqrt(x * x + y * y + z * z);
			}
		}

		public float LengthSquared
		{
			get
			{
				return x * x + y * y + z * z;
			}
		}

		public static float AngleBetween(VECTOR3 vec1, VECTOR3 vec2)
		{
#if SERVER_ONLY
			return (float)Vector3D.AngleBetween(vec1.v, vec2.v);
#elif MAIN_SERVER_ONLY
            return (float)Vector3D.AngleBetween(vec1.v, vec2.v);
#elif TEST_ONLY
            return 0;
#else

            return Vector3.Angle(vec1.v, vec2.v);
#endif
        }

        public static float DotProduct(VECTOR3 vec1, VECTOR3 vec2)
		{
#if SERVER_ONLY
			return (float)Vector3D.DotProduct(vec1.v, vec2.v);
#elif MAIN_SERVER_ONLY
			return (float)Vector3D.DotProduct(vec1.v, vec2.v);
#elif TEST_ONLY
            return 0;
#else
			return Vector3.Dot(vec1.v, vec2.v);
#endif
        }

        public static VECTOR3 CrossProduct(VECTOR3 vec1, VECTOR3 vec2)
		{
#if SERVER_ONLY
			return new VECTOR3(Vector3D.CrossProduct(vec1.v, vec2.v));
#elif MAIN_SERVER_ONLY
			return new VECTOR3(Vector3D.CrossProduct(vec1.v, vec2.v));
#elif TEST_ONLY
            return new VECTOR3();
#else
			return new VECTOR3(Vector3.Cross(vec1.v, vec2.v));
#endif
        }

        public void Normalize()
		{
			float length = Length;
			if(length > 0)
			{
				x /= length;
				y /= length;
				z /= length;
			}
		}

		public static VECTOR3 operator +(VECTOR3 vec1, VECTOR3 vec2)
		{
			return new VECTOR3(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
		}

		public static VECTOR3 operator -(VECTOR3 vec1, VECTOR3 vec2)
		{
			return new VECTOR3(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z);
		}

		public static VECTOR3 operator *(float s, VECTOR3 vec)
		{
			return new VECTOR3(s * vec.x, s * vec.y, s * vec.z);
		}

		public static VECTOR3 operator *(VECTOR3 vec, float s)
		{
			return new VECTOR3(vec.x * s, vec.y * s, vec.z * s);
		}

		public static VECTOR3 operator /(VECTOR3 vec, float s)
		{
			return new VECTOR3(vec.x / s, vec.y / s, vec.z / s);
		}

		public void Read(SNetReader reader)
		{
			if(!reader.Read(ref x)) return;
			if(!reader.Read(ref y)) return;
			if(!reader.Read(ref z)) return;
		}
		public void Write(SNetWriter writer)
		{
			if(!writer.Write(x)) return;
			if(!writer.Write(y)) return;
			if(!writer.Write(z)) return;
		}
	}

	public class CLine
	{
		public VECTOR3 Srt { get; set; }
		public VECTOR3 End { get; set; }
	}

	public class CRectangle
	{
		public float Left { get; set; }
		public float Top { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }

		public CRectangle(float left, float top, float width, float height)
		{
			Left = left;
			Top = top;
			Width = width;
			Height = height;
		}
	}

	public static class CBresenham
	{
		private static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

		public delegate bool PlotFunction(int x, int y);

		public static void Line(int x0, int y0, int x1, int y1, PlotFunction plot)
		{
			bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
			if(steep) { Swap<int>(ref x0, ref y0); Swap<int>(ref x1, ref y1); }
			if(x0 > x1) { Swap<int>(ref x0, ref x1); Swap<int>(ref y0, ref y1); }
			int dX = (x1 - x0), dY = Math.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

			for(int x = x0; x <= x1; x++)
			{
				if(!(steep ? plot(y, x) : plot(x, y))) return;
				err = err - dY;
				if(err < 0) { y += ystep; err += dX; }
			}
		}
	}
}

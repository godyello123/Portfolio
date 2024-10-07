using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace PathFinder
{
	public struct PathFinderNode
	{
		public int F;
		public int G;
		public int H;  // f = gone + heuristic
		public int X;
		public int Y;
		public int PX; // Parent
		public int PY;

		public int Dir
		{
			get
			{
				if(X == PX && Y > PY) return 0;
				else if(X > PX && Y > PY) return 1;
				else if(X > PX && Y == PY) return 2;
				else if(X > PX && Y < PY) return 3;
				else if(X == PX && Y < PY) return 4;
				else if(X < PX && Y < PY) return 5;
				else if(X < PX && Y == PY) return 6;
				else if(X < PX && Y > PY) return 7;
				return -1;
			}
		}
	}

	public enum HeuristicFormula
	{
		Manhattan = 1,
		MaxDXDY = 2,
		DiagonalShortCut = 3,
		Euclidean = 4,
		EuclideanNoSQR = 5,
		Custom1 = 6
	}

	public class PathFinderFast
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct PathFinderNodeFast
		{
			public int F; // f = gone + heuristic
			public int G;
			public ushort PX; // Parent
			public ushort PY;
			public byte Status;
		}

		// Heap variables are initializated to default, but I like to do it anyway
		private byte[,] m_Grid = null;
		private PriorityQueueB<int> m_Open = null;
		private List<PathFinderNode> m_Close = new List<PathFinderNode>();
		private bool m_Stop = false;
		private bool m_Stopped = true;
		private int m_Horiz = 0;
		private HeuristicFormula m_Formula = HeuristicFormula.Euclidean;
		private bool m_Diagonals = true;
		private bool m_HeavyDiagonals = true;
		private int m_HEstimate = 2;
		private bool m_PunishChangeDirection = false;
		private bool m_ReopenCloseNodes = true;
		private bool m_TieBreaker = false;
		private int m_SearchLimit = 2000;

		private PathFinderNodeFast[] m_CalcGrid = null;
		private byte m_OpenNodeValue = 1;
		private byte m_CloseNodeValue = 2;

		//Promoted local variables to member variables to avoid recreation between calls
		private int m_H = 0;
		private int m_Location = 0;
		private int m_NewLocation = 0;
		private ushort m_LocationX = 0;
		private ushort m_LocationY = 0;
		private ushort m_NewLocationX = 0;
		private ushort m_NewLocationY = 0;
		private int m_CloseNodeCounter = 0;
		private ushort m_GridX = 0;
		private ushort m_GridY = 0;
		private ushort m_GridXMinus1 = 0;
		private ushort m_GridYLog2 = 0;
		private bool m_Found = false;
		private sbyte[,] m_Direction = new sbyte[8, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
		private int m_EndLocation = 0;
		private int m_NewG = 0;

		public PathFinderFast(byte[,] grid)
		{
			if(grid == null)
				throw new Exception("Grid cannot be null");

			m_Grid = grid;
			m_GridX = (ushort)(m_Grid.GetUpperBound(0) + 1);
			m_GridY = (ushort)(m_Grid.GetUpperBound(1) + 1);
			m_GridXMinus1 = (ushort)(m_GridX - 1);
			m_GridYLog2 = (ushort)Math.Log(m_GridY, 2);

			// This should be done at the constructor, for now we leave it here.
			if(Math.Log(m_GridX, 2) != (int)Math.Log(m_GridX, 2) ||
				Math.Log(m_GridY, 2) != (int)Math.Log(m_GridY, 2))
				throw new Exception("Invalid Grid, size in X and Y must be power of 2");

			if(m_CalcGrid == null || m_CalcGrid.Length != (m_GridX * m_GridY))
				m_CalcGrid = new PathFinderNodeFast[m_GridX * m_GridY];

			m_Open = new PriorityQueueB<int>(new ComparePFNodeMatrix(m_CalcGrid));
		}

		public bool Stopped
		{
			get { return m_Stopped; }
		}

		public HeuristicFormula Formula
		{
			get { return m_Formula; }
			set { m_Formula = value; }
		}

		public bool Diagonals
		{
			get { return m_Diagonals; }
			set
			{
				m_Diagonals = value;
				if(m_Diagonals)
					m_Direction = new sbyte[8, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
				else
					m_Direction = new sbyte[4, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
			}
		}

		public bool HeavyDiagonals
		{
			get { return m_HeavyDiagonals; }
			set { m_HeavyDiagonals = value; }
		}

		public int HeuristicEstimate
		{
			get { return m_HEstimate; }
			set { m_HEstimate = value; }
		}

		public bool PunishChangeDirection
		{
			get { return m_PunishChangeDirection; }
			set { m_PunishChangeDirection = value; }
		}

		public bool ReopenCloseNodes
		{
			get { return m_ReopenCloseNodes; }
			set { m_ReopenCloseNodes = value; }
		}

		public bool TieBreaker
		{
			get { return m_TieBreaker; }
			set { m_TieBreaker = value; }
		}

		public int SearchLimit
		{
			get { return m_SearchLimit; }
			set { m_SearchLimit = value; }
		}

		public void FindPathStop()
		{
			m_Stop = true;
		}

		private static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

		public bool CheckStraight(Point start, Point end, int min)
		{
			int x0 = start.X;
			int y0 = start.Y;
			int x1 = end.X;
			int y1 = end.Y;

			bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
			if(steep) { Swap<int>(ref x0, ref y0); Swap<int>(ref x1, ref y1); }
			if(x0 > x1) { Swap<int>(ref x0, ref x1); Swap<int>(ref y0, ref y1); }
			int dX = (x1 - x0), dY = Math.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

			for(int x = x0; x <= x1; x++)
			{
				if(steep)
				{
					if(m_Grid[y, x] >= min)
						return false;
				}
				else
				{
					if(m_Grid[x, y] >= min)
						return false;
				}
				err = err - dY;
				if(err < 0) { y += ystep; err += dX; }
			}

			return true;
		}

		public List<PathFinderNode> FindPath(Point start, Point end)
		{
			lock(this)
			{
				m_Found = false;
				m_Stop = false;
				m_Stopped = false;
				m_CloseNodeCounter = 0;
				m_OpenNodeValue += 2;
				m_CloseNodeValue += 2;
				m_Open.Clear();
				m_Close.Clear();

				m_Location = (start.Y << m_GridYLog2) + start.X;
				m_EndLocation = (end.Y << m_GridYLog2) + end.X;
				m_CalcGrid[m_Location].G = 0;
				m_CalcGrid[m_Location].F = m_HEstimate;
				m_CalcGrid[m_Location].PX = (ushort)start.X;
				m_CalcGrid[m_Location].PY = (ushort)start.Y;
				m_CalcGrid[m_Location].Status = m_OpenNodeValue;

				m_Open.Push(m_Location);
				while(m_Open.Count > 0 && !m_Stop)
				{
					m_Location = m_Open.Pop();

					//Is it in closed list? means this node was already processed
					if(m_CalcGrid[m_Location].Status == m_CloseNodeValue)
						continue;

					m_LocationX = (ushort)(m_Location & m_GridXMinus1);
					m_LocationY = (ushort)(m_Location >> m_GridYLog2);

					if(m_Location == m_EndLocation)
					{
						m_CalcGrid[m_Location].Status = m_CloseNodeValue;
						m_Found = true;
						break;
					}

					if(m_CloseNodeCounter > m_SearchLimit)
					{
						m_Stopped = true;
						return null;
					}

					if(m_PunishChangeDirection)
						m_Horiz = (m_LocationX - m_CalcGrid[m_Location].PX);

					//Lets calculate each successors
					for(int i = 0; i < (m_Diagonals ? 8 : 4); i++)
					{
						m_NewLocationX = (ushort)(m_LocationX + m_Direction[i, 0]);
						m_NewLocationY = (ushort)(m_LocationY + m_Direction[i, 1]);
						m_NewLocation = (m_NewLocationY << m_GridYLog2) + m_NewLocationX;

						if(m_NewLocationX >= m_GridX || m_NewLocationY >= m_GridY)
							continue;

						if(m_CalcGrid[m_NewLocation].Status == m_CloseNodeValue && !m_ReopenCloseNodes)
							continue;

						// Unbreakeable?
						if(m_Grid[m_NewLocationX, m_NewLocationY] == 255)
							continue;

						if(m_HeavyDiagonals && i > 3)
							m_NewG = m_CalcGrid[m_Location].G + (int)((m_Grid[m_NewLocationX, m_NewLocationY] + 1) * 2.41);
						else
							m_NewG = m_CalcGrid[m_Location].G + (m_Grid[m_NewLocationX, m_NewLocationY] + 1);

						if(m_PunishChangeDirection)
						{
							if((m_NewLocationX - m_LocationX) != 0)
							{
								if(m_Horiz == 0)
									m_NewG += Math.Abs(m_NewLocationX - end.X) + Math.Abs(m_NewLocationY - end.Y);
							}
							if((m_NewLocationY - m_LocationY) != 0)
							{
								if(m_Horiz != 0)
									m_NewG += Math.Abs(m_NewLocationX - end.X) + Math.Abs(m_NewLocationY - end.Y);
							}
						}

						//Is it open or closed?
						if(m_CalcGrid[m_NewLocation].Status == m_OpenNodeValue || m_CalcGrid[m_NewLocation].Status == m_CloseNodeValue)
						{
							// The current node has less code than the previous? then skip this node
							if(m_CalcGrid[m_NewLocation].G <= m_NewG)
								continue;
						}

						m_CalcGrid[m_NewLocation].PX = m_LocationX;
						m_CalcGrid[m_NewLocation].PY = m_LocationY;
						m_CalcGrid[m_NewLocation].G = m_NewG;

						switch(m_Formula)
						{
						default:
						case HeuristicFormula.Manhattan:
							m_H = m_HEstimate * (Math.Abs(m_NewLocationX - end.X) + Math.Abs(m_NewLocationY - end.Y));
							break;
						case HeuristicFormula.MaxDXDY:
							m_H = m_HEstimate * (Math.Max(Math.Abs(m_NewLocationX - end.X), Math.Abs(m_NewLocationY - end.Y)));
							break;
						case HeuristicFormula.DiagonalShortCut:
							int h_diagonal = Math.Min(Math.Abs(m_NewLocationX - end.X), Math.Abs(m_NewLocationY - end.Y));
							int h_straight = (Math.Abs(m_NewLocationX - end.X) + Math.Abs(m_NewLocationY - end.Y));
							m_H = (m_HEstimate * 2) * h_diagonal + m_HEstimate * (h_straight - 2 * h_diagonal);
							break;
						case HeuristicFormula.Euclidean:
							m_H = (int)(m_HEstimate * Math.Sqrt(Math.Pow((m_NewLocationX - end.X), 2) + Math.Pow((m_NewLocationY - end.Y), 2)));
							break;
						case HeuristicFormula.EuclideanNoSQR:
							m_H = (int)(m_HEstimate * (Math.Pow((m_NewLocationX - end.X), 2) + Math.Pow((m_NewLocationY - end.Y), 2)));
							break;
						case HeuristicFormula.Custom1:
							Point dxy = new Point(Math.Abs(end.X - m_NewLocationX), Math.Abs(end.Y - m_NewLocationY));
							int Orthogonal = Math.Abs(dxy.X - dxy.Y);
							int Diagonal = Math.Abs(((dxy.X + dxy.Y) - Orthogonal) / 2);
							m_H = m_HEstimate * (Diagonal + Orthogonal + dxy.X + dxy.Y);
							break;
						}
						if(m_TieBreaker)
						{
							int dx1 = m_LocationX - end.X;
							int dy1 = m_LocationY - end.Y;
							int dx2 = start.X - end.X;
							int dy2 = start.Y - end.Y;
							int cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
							m_H = (int)(m_H + cross * 0.001);
						}
						m_CalcGrid[m_NewLocation].F = m_NewG + m_H;

						m_Open.Push(m_NewLocation);
						m_CalcGrid[m_NewLocation].Status = m_OpenNodeValue;
					}

					m_CloseNodeCounter++;
					m_CalcGrid[m_Location].Status = m_CloseNodeValue;
				}

				if(m_Found)
				{
					var path = new List<PathFinderNode>();
					int posX = end.X;
					int posY = end.Y;

					PathFinderNodeFast fNodeTmp = m_CalcGrid[(end.Y << m_GridYLog2) + end.X];
					PathFinderNode fNode;
					fNode.F = fNodeTmp.F;
					fNode.G = fNodeTmp.G;
					fNode.H = 0;
					fNode.PX = fNodeTmp.PX;
					fNode.PY = fNodeTmp.PY;
					fNode.X = end.X;
					fNode.Y = end.Y;

					while(fNode.X != fNode.PX || fNode.Y != fNode.PY)
					{
						path.Add(fNode);
						posX = fNode.PX;
						posY = fNode.PY;
						fNodeTmp = m_CalcGrid[(posY << m_GridYLog2) + posX];
						fNode.F = fNodeTmp.F;
						fNode.G = fNodeTmp.G;
						fNode.H = 0;
						fNode.PX = fNodeTmp.PX;
						fNode.PY = fNodeTmp.PY;
						fNode.X = posX;
						fNode.Y = posY;
					}

					path.Add(fNode);
					m_Stopped = true;
					return path;
				}
				m_Stopped = true;
				return null;
			}
		}

		public List<Point> FindPathExt(Point start, Point end, int min)
		{
			var pathExt = new List<Point>();
			if(CheckStraight(start, end, min))
			{
				pathExt.Add(start);
				pathExt.Add(end);
				return pathExt;
			}

			var path = FindPath(start, end);
			if(path == null) return null;

			int lastDir = -1;
			var pathTemp = new List<Point>();
			for(int i = 0; i < path.Count; i++)
			{
				int dir = path[i].Dir;
				if(lastDir < 0 || dir != lastDir || dir < 0)
				{
					lastDir = dir;
					pathTemp.Insert(0, new Point(path[i].X, path[i].Y));
					continue;
				}
			}

			for(int i = 0; i < pathTemp.Count;)
			{
				Point pos = pathTemp[i];
				pathExt.Add(pos);
				if(i + 2 >= pathTemp.Count)
				{
					i++;
					continue;
				}
				for(int j = i + 2; j < pathTemp.Count; j++)
				{
					if(!CheckStraight(pos, pathTemp[j], min))
					{
						i = j - 1;
						break;
					}
					i = j;
				}
			}

			return pathExt;
		}

		internal class ComparePFNodeMatrix : IComparer<int>
		{
			PathFinderNodeFast[] mMatrix;

			public ComparePFNodeMatrix(PathFinderNodeFast[] matrix)
			{
				mMatrix = matrix;
			}

			public int Compare(int a, int b)
			{
				if(mMatrix[a].F > mMatrix[b].F)
					return 1;
				else if(mMatrix[a].F < mMatrix[b].F)
					return -1;
				return 0;
			}
		}
	}
}

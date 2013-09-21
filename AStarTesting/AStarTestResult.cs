using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarTesting
{
	public struct AStarTestResult
	{
		// Test parameters
		public bool		KeypressAdvance;
		public int		RandomSeed;
		public string	NavmeshGridType;
		public int		NavmeshColumns;
		public int		NavmeshRows;
		public int		XStart;
		public int		YStart;
		public int		XGoal;
		public int		YGoal;
		public float	SimplexAmplitude;
		public float	SimplexScale;
		public float	SimplexXOffset;
		public float	SimplexYOffset;
		public string	Heuristic;
		public float	CoeffCostFromStart;
		public float	CoeffCostToGoal;

		// Result data
		public long		MaxOpenSetCount;
		public long		NodesConsideredCount;
		public long		PathLength;
		public string	PathString;
		public long		MSBacktrace;
		public long		TicksBacktrace;
		public long		MSBody;
		public long		TicksBody;
		public long		MSClosedSet;
		public long		TicksClosedSet;
		public long		MSFindMin;
		public long		TicksFindMin;
		public long		MSNodes;
		public long		TicksNodes;
		public long		MSOpenSet;
		public long		TicksOpenSet;
		public long		MSSetup;
		public long		TicksSetup;
		public long		MSTotal;
		public long		TicksTotal;
		
		public override string ToString()
		{
			return String.Join( ",", 
				KeypressAdvance, RandomSeed, NavmeshGridType, NavmeshColumns, NavmeshRows, 
				"[ " + XStart + " : " + YStart + " ]", "[ " + XGoal + " : " + YGoal + " ]",
				SimplexAmplitude, SimplexScale, "[ " + SimplexXOffset + " : " + SimplexYOffset + " ]", 
				Heuristic, CoeffCostFromStart, CoeffCostToGoal,
				MSTotal, MSSetup, MSBody, MSFindMin, MSBacktrace, MSClosedSet, MSOpenSet, MSNodes, 
				TicksTotal, TicksSetup, TicksBody, TicksFindMin, TicksBacktrace, TicksClosedSet, TicksOpenSet, TicksNodes, 
				NodesConsideredCount, MaxOpenSetCount, PathLength, PathString 
			);
		}
	}
}

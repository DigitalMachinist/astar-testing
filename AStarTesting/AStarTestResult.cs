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
		public int		RandomSeed;
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
		public long		MSExternal;
		public long		TicksExternal;
		public long		MSTotal;
		public long		TicksTotal;
		public long		MSSetup;
		public long		TicksSetup;
		public long		MSBody;
		public long		TicksBody;
		public long		MSFindMin;
		public long		TicksFindMin;
		public long		MSBacktrace;
		public long		TicksBacktrace;
		public long		NodesConsideredCount;
		public long		MaxOpenSetCount;
		public long		PathLength;
		public string	PathString;

		public override string ToString()
		{
			return String.Join( ",", 
				RandomSeed, NavmeshColumns, NavmeshRows, 
				"[ " + XStart + " : " + YStart + " ]", "[ " + XGoal + " : " + YGoal + " ]",
				SimplexAmplitude, SimplexScale, "[ " + SimplexXOffset + " : " + SimplexYOffset + " ]", 
				Heuristic, CoeffCostFromStart, CoeffCostToGoal,
				MSExternal, MSTotal, MSSetup, MSBody, MSFindMin, MSBacktrace, 
				TicksExternal, TicksTotal, TicksSetup, TicksBody, TicksFindMin, TicksBacktrace, 
				NodesConsideredCount, MaxOpenSetCount, PathLength, PathString 
			);
		}
	}
}

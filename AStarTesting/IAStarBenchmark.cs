using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarTesting
{
	public interface IAStarBenchmark
	{
		///////////////////////////////////////////////////////////////////////////////////////////
		#region Property Declarations

		/// <summary>
		/// The stopwatch to measure the time for the entire Solve() operation from start to finish.
		/// </summary>
		Stopwatch SWTotal { get; set; }

		/// <summary>
		/// The stopwatch to measure the time cost of any operations before the bosy of the Solve() operation.
		/// </summary>
		Stopwatch SWSetup { get; set; }

		/// <summary>
		/// The stopwatch to measure the primary loop performed by the Solve() operation to find a path.
		/// </summary>
		Stopwatch SWBody { get; set; }

		/// <summary>
		/// The stopwatch to measure the time cost of any operation done to determine the lowest-cost node in the open set.
		/// </summary>
		Stopwatch SWFindMin { get; set; }

		/// <summary>
		/// The stopwatch to measure the time cost of performing the backtrace to produce the linear path from start node to goal node.
		/// </summary>
		Stopwatch SWBacktrace { get; set; }

		/// <summary>
		/// The total number of nodes considered by the last Solve() operation.
		/// </summary>
		long NodesConsideredCount { get; set; }

		/// <summary>
		/// The maximum size that the open set reached during the last Solve() operation.
		/// </summary>
		long MaxOpenSetCount { get; set; }

		/// <summary>
		/// The length of the path from the start node to the goal node computer by thenlast Solve() operation.
		/// </summary>
		long PathLength { get; set; }

		#endregion


		///////////////////////////////////////////////////////////////////////////////////////////
		#region Method Declarations

		/// <summary>
		/// Solves for the path from the start node to the goal node.
		/// </summary>
		void Solve();

		#endregion
	}
}

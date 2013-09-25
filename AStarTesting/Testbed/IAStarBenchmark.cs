using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarTesting.Testbed
{
	public interface IAStarBenchmark
	{
		/// <summary>
		/// The maximum size that the closed set reached during the last Solve() operation.
		/// </summary>
		long MaxClosedSetCount { get; set; }

		/// <summary>
		/// The maximum size that the open set reached during the last Solve() operation.
		/// </summary>
		long MaxOpenSetCount { get; set; }

		/// <summary>
		/// The total number of nodes considered by the last Solve() operation.
		/// </summary>
		long NodesConsideredCount { get; set; }

		/// <summary>
		/// The length of the path from the start node to the goal node computer by the last Solve() operation.
		/// </summary>
		long PathLength { get; set; }

		/// <summary>
		/// The string-formatted path chosen by the agent from the StartNode to the GoalNode.
		/// </summary>
		string PathString { get; }

		/// <summary>
		/// The stopwatch to measure the time cost of performing the backtrace to produce the linear path from start node to goal node.
		/// </summary>
		Stopwatch SWBacktrace { get; set; }

		/// <summary>
		/// The stopwatch to measure the primary loop performed by the Solve() operation to find a path.
		/// </summary>
		Stopwatch SWBody { get; set; }

		/// <summary>
		/// The stopwatch to measure the time cost of all operations using the closed set.
		/// </summary>
		Stopwatch SWClosedSet { get; set; }

		/// <summary>
		/// The stopwatch to measure the time cost of any operation done to determine the lowest-cost node in the open set.
		/// </summary>
		Stopwatch SWFindMin { get; set; }

		/// <summary>
		/// The stopwatch to measure the time cost of changing node values.
		/// </summary>
		Stopwatch SWNodes { get; set; }

		/// <summary>
		/// The stopwatch to measure the time cost of all operations using the open set.
		/// </summary>
		Stopwatch SWOpenSet { get; set; }

		/// <summary>
		/// The stopwatch to measure the time for the entire Solve() operation from start to finish.
		/// </summary>
		Stopwatch SWTotal { get; set; }

		/// <summary>
		/// The stopwatch to measure the time cost of any operations before the bosy of the Solve() operation.
		/// </summary>
		Stopwatch SWSetup { get; set; }
	}
}

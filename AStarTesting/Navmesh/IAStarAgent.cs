using System;
using AStarTesting.Heuristics;

namespace AStarTesting.Navmesh
{
	public interface IAStarAgent
	{
		float CoeffCostFromStart { get; }
		float CoeffCostToGoal { get; }
		AStarNode GoalNode { get; }
		IAStarHeuristic Heuristic { get; }
		AStarNode StartNode { get; }

		/// <summary>
		/// Solves for the path from the start node to the goal node.
		/// </summary>
		void Solve();
	}
}

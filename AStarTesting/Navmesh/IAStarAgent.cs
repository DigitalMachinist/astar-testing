using System;
using AStarTesting.Heuristics;

namespace AStarTesting.Navmesh
{
	public interface IAStarAgent
	{
		float CoeffCostFromStart { get; set; }
		float CoeffCostToGoal { get; set; }
		AStarNode GoalNode { get; set; }
		IAStarHeuristic Heuristic { get; set; }
		AStarNode StartNode { get; set; }

		/// <summary>
		/// Solves for the path from the start node to the goal node.
		/// </summary>
		void Solve();
	}
}

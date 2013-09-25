using System;
using System.Collections.Generic;
using AStarTesting.Navmesh;

namespace AStarTesting.Heuristics
{
	public interface IAStarHeuristic
	{
		///////////////////////////////////////////////////////////////////////////////////////////
		#region Method Declarations

		/// <summary>
		/// Returns the estimated heuristic cost to reach the goal node from the current node (define the nodes in your implementation).
		/// </summary>
		/// <returns>An integer representing the heuristic estimate traversal cost.</returns>
		float GetEstimatedCost( AStarNode currentNode, AStarNode goalNode );

		#endregion
	}
}

using System;
using System.Collections.Generic;
using AStarTesting.Navmesh;

namespace AStarTesting.Heuristics
{
	public class DijkstraAStarHeuristic : IAStarHeuristic
	{
		///////////////////////////////////////////////////////////////////////////////////////////
		#region Interface Methods
		
		public float GetEstimatedCost( AStarNode currentNode, AStarNode goalNode )
		{
			return 0;
		}

		#endregion
	}
}

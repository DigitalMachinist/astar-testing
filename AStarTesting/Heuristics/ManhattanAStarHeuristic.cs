using System;
using System.Collections.Generic;
using AStarTesting.Navmesh;

namespace AStarTesting.Heuristics
{
	public class ManhattanAStarHeuristic : IAStarHeuristic
	{
		///////////////////////////////////////////////////////////////////////////////////////////
		#region Interface Methods
		
		public float GetEstimatedCost( AStarNode currentNode, AStarNode goalNode )
		{
			return Math.Abs( goalNode.Column - currentNode.Column ) + Math.Abs( goalNode.Row - currentNode.Row );
		}

		#endregion
	}
}

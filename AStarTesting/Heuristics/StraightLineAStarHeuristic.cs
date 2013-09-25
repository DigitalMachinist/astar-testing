using System;
using System.Collections.Generic;
using AStarTesting.Navmesh;

namespace AStarTesting.Heuristics
{
	public class StraightLineAStarHeuristic : IAStarHeuristic
	{
		///////////////////////////////////////////////////////////////////////////////////////////
		#region Interface Methods
		
		public float GetEstimatedCost( AStarNode currentNode, AStarNode goalNode )
		{
			float diffColumns = Math.Abs( goalNode.Column - currentNode.Column );
			float diffRows = Math.Abs( goalNode.Row - currentNode.Row );
			return (float)Math.Sqrt( diffColumns * diffColumns + diffRows * diffRows );
		}

		#endregion
	}
}

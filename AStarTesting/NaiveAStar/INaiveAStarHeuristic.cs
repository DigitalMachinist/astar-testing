using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarTesting.NaiveAStar
{
	public interface INaiveAStarHeuristic
	{
		///////////////////////////////////////////////////////////////////////////////////////////
		#region Method Declarations

		/// <summary>
		/// Returns the estimated heuristic cost to reach the goal node from the current node (define the nodes in your implementation).
		/// </summary>
		/// <returns>An integer representing the heuristic estimate traversal cost.</returns>
		float GetEstimatedCost( NaiveAStarNode currentNode, NaiveAStarNode goalNode );

		#endregion
	}
}

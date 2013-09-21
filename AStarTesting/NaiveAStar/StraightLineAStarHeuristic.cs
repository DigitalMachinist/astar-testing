using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarTesting.NaiveAStar
{
	public class StraightLineAStarHeuristic : INaiveAStarHeuristic
	{
		///////////////////////////////////////////////////////////////////////////////////////////
		#region Interface Methods
		
		public float GetEstimatedCost( NaiveAStarNode currentNode, NaiveAStarNode goalNode )
		{
			return (float)Math.Sqrt( Math.Abs( goalNode.Column - currentNode.Column ) + Math.Abs( goalNode.Row - currentNode.Row ) );
		}

		#endregion
	}
}

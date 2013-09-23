using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarTesting.NaiveAStar
{
	public class DijakstraAStarHeuristic : INaiveAStarHeuristic
	{
		///////////////////////////////////////////////////////////////////////////////////////////
		#region Interface Methods
		
		public float GetEstimatedCost( NaiveAStarNode currentNode, NaiveAStarNode goalNode )
		{
			return 0;
		}

		#endregion
	}
}

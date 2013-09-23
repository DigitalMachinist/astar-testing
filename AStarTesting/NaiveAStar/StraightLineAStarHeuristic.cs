﻿using System;
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
			float diffColumns = Math.Abs( goalNode.Column - currentNode.Column );
			float diffRows = Math.Abs( goalNode.Row - currentNode.Row );
			return (float)Math.Sqrt( diffColumns * diffColumns + diffRows * diffRows );
		}

		#endregion
	}
}

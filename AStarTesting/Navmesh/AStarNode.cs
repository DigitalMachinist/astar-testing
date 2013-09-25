using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarTesting.Navmesh
{
	public class AStarNode
	{
		///////////////////////////////////////////////////////////////////////////////////////////
		#region Properties

		/// <summary>
		/// 
		/// </summary>
		IAStarAgent mAgent;
		public IAStarAgent Agent 
		{
			get { return mAgent; }
			set { mAgent = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		bool mBelongsToClosedSet;
		public bool BelongsToClosedSet 
		{
			get { return mBelongsToClosedSet; }
			set { mBelongsToClosedSet = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		bool mBelongsToOpenSet;
		public bool BelongsToOpenSet 
		{
			get { return mBelongsToOpenSet; }
			set { mBelongsToOpenSet = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		int mColumn;
		public int Column
		{
			get { return mColumn; }
			set { mColumn = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		float mCostTotal;
		public float CostTotal 
		{
			get { return mCostTotal; }
			set { mCostTotal = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		float mCostFromStart;
		public float CostFromStart 
		{
			get { return mCostFromStart; }
			set { mCostFromStart = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		float mCostToGoal;
		public float CostToGoal 
		{
			get { return mCostToGoal; }
			set { mCostToGoal = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		float mMoveCost;
		public float MoveCost
		{
			get { return mMoveCost; }
			set { mMoveCost = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		List<AStarNode> mNeighbors;
		public List<AStarNode> Neighbors 
		{
			get { return mNeighbors; }
			private set { mNeighbors = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		AStarNode mParent;
		public AStarNode Parent 
		{
			get { return mParent; }
			set { mParent = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		int mRow;
		public int Row
		{
			get { return mRow; }
			set { mRow = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		bool mTraversable;
		public bool Traversable 
		{
			get { return mTraversable; }
			set { mTraversable = value; }
		}

		#endregion


		///////////////////////////////////////////////////////////////////////////////////////////
		#region Methods

		public void AddNeighbor( AStarNode node )
		{
			Neighbors.Add( node );
		}

		public void RemoveNeighbor( AStarNode node )
		{
			Neighbors.Remove( node );
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder( "Node >> Column: " + Column + ", Row: " + Row + ", Traversable: " + Traversable + ", Neighbors: " );
		
			foreach ( AStarNode neighbor in Neighbors )
				result.Append( "[ " + neighbor.Column + " : " + neighbor.Row + " ],  " );
		
			return result.ToString();
		}

		#endregion


		///////////////////////////////////////////////////////////////////////////////////////////
		#region ctor

		public AStarNode( int column, int row, bool traversable = true, float moveCost = 1 )
		{
			Column = column;
			Row = row;
			Traversable = traversable;
			MoveCost = moveCost;
			Neighbors = new List<AStarNode>();
		}

		#endregion
	}
}

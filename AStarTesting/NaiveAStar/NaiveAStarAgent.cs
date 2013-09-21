using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarTesting.NaiveAStar
{
	public class NaiveAStarAgent : IAStarBenchmark
	{
		///////////////////////////////////////////////////////////////////////////////////////////
		#region Properties

		/// <summary>
		/// 
		/// </summary>
		List<NaiveAStarNode> mClosedSet;
		public List<NaiveAStarNode> ClosedSet
		{
			get { return mClosedSet; }
			private set { mClosedSet = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		float mCoeffCostFromStart;
		public float CoeffCostFromStart
		{
			get { return mCoeffCostFromStart; }
			set { mCoeffCostFromStart = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		float mCoeffCostToGoal;
		public float CoeffCostToGoal
		{
			get { return mCoeffCostToGoal; }
			set { mCoeffCostToGoal = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		NaiveAStarNode mGoalNode;
		public NaiveAStarNode GoalNode
		{
			get { return mGoalNode; }
			set { mGoalNode = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		INaiveAStarHeuristic mHeuristic;
		public INaiveAStarHeuristic Heuristic
		{
			get { return mHeuristic; }
			set { mHeuristic = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		long mMaxOpenSetCount;
		public long MaxOpenSetCount
		{
			get { return mMaxOpenSetCount; }
			set { mMaxOpenSetCount = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		long mNodesConsideredCount;
		public long NodesConsideredCount
		{
			get { return mNodesConsideredCount; }
			set { mNodesConsideredCount = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		List<NaiveAStarNode> mOpenSet;
		public List<NaiveAStarNode> OpenSet
		{
			get { return mOpenSet; }
			private set { mOpenSet = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		List<NaiveAStarNode> mPath;
		public List<NaiveAStarNode> Path
		{
			get { return mPath; }
			private set { mPath = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		long mPathLength;
		public long PathLength
		{
			get { return mPathLength; }
			set { mPathLength = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public string PathString
		{
			get
			{
				StringBuilder pathStr = new StringBuilder();
				foreach ( NaiveAStarNode node in Path ) pathStr.Append( "[ " + node.Column + " : " + node.Row + " ], " );
				return pathStr.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		NaiveAStarNode mStartNode;
		public NaiveAStarNode StartNode
		{
			get { return mStartNode; }
			set { mStartNode = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		Stopwatch mSWBacktrace;
		public Stopwatch SWBacktrace
		{
			get { return mSWBacktrace; }
			set { mSWBacktrace = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		Stopwatch mSWBody;
		public Stopwatch SWBody
		{
			get { return mSWBody; }
			set { mSWBody = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		Stopwatch mSWFindMin;
		public Stopwatch SWFindMin
		{
			get { return mSWFindMin; }
			set { mSWFindMin = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		Stopwatch mSWSetup;
		public Stopwatch SWSetup
		{
			get { return mSWSetup; }
			set { mSWSetup = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		Stopwatch mSWTotal;
		public Stopwatch SWTotal
		{
			get { return mSWTotal; }
			set { mSWTotal = value; }
		}

		#endregion


		///////////////////////////////////////////////////////////////////////////////////////////
		#region Methods

		/// <summary>
		/// Returns the traversal cost of moving from the start node to the specified node.
		/// </summary>
		/// <param name="node">The node to compute the traversal cost to from the start node.</param>
		/// <param name="parent">(Default null) The node to use as the parent. If null, this node is used.</param>
		/// <returns>An integer traversal cost from the start node to the specified node, or 0 if parent == null.</returns>
		public float GetCostFromStart( NaiveAStarNode node, NaiveAStarNode parent = null )
		{
			if ( parent == null ) parent = node.Parent;
			if ( parent == null ) return 0f;
			return parent.CostFromStart + CoeffCostFromStart * node.MoveCost;
		}

		/// <summary>
		/// Returns the estimated traversal cost of moving from the specified node to the goal node.
		/// </summary>
		/// <param name="node">The node to compute the heuristic estimate traversal cost from to the goal node.</param>
		/// <returns>An integer heuristic estimate traversal cost from the specified node to the goal node.</returns>
		public float GetCostToGoal( NaiveAStarNode node )
		{
			if ( Heuristic == null ) return 0f;
			return CoeffCostToGoal * Heuristic.GetEstimatedCost( node, mGoalNode );
		}

		/// <summary>
		/// Zeroes all benchmark counters and stopwatches.
		/// </summary>
		void ResetBenchmark()
		{
			SWTotal.Reset();
			SWSetup.Reset();
			SWBody.Reset();
			SWFindMin.Reset();
			SWBacktrace.Reset();
			NodesConsideredCount = 0;
			MaxOpenSetCount = 0;
			PathLength = 0;
		}

		public void Solve()
		{
			// Zero the benchmark variables
			ResetBenchmark();

			SWTotal.Start();
			SWSetup.Start();
			
			// Clear the node lists from last time
			ClosedSet.Clear();
			OpenSet.Clear();
			Path.Clear();

			// Set the current node being considered
			NaiveAStarNode currentNode = StartNode;
			currentNode.Parent = null;
			currentNode.CostFromStart = 0;
			currentNode.CostToGoal = 0;
			currentNode.CostTotal = 0;
			
			// Closed list of nodes to not be reconsidered
			mClosedSet.Add( currentNode );
		
			// Open list of nodes to consider and the node's parent (for backtracing)
			foreach ( NaiveAStarNode node in currentNode.Neighbors )
			{
				if ( node.Traversable )
				{
					mOpenSet.Add( node );
					node.Parent = currentNode;
					node.CostFromStart = GetCostFromStart( node );
					node.CostToGoal = GetCostToGoal( node );
					node.CostTotal = node.CostFromStart + node.CostToGoal;
				}

				NodesConsideredCount++;
			}

			SWSetup.Stop();
			SWBody.Start();

#if ( ASTAR_DEBUG && ASTAR_KEY_STEPPING )
			Console.WriteLine();
			Console.WriteLine( "Press any key to advance to the next iteration..." );
#endif

			// Continue looping until the goal is reached
			while ( currentNode != GoalNode && OpenSet.Count > 0 )
			{
#if ( ASTAR_DEBUG && ASTAR_KEY_STEPPING )
				// Run one iteration for each keypress
				Console.ReadKey();
				Console.WriteLine( currentNode );
#endif
				SWFindMin.Start();

				// Find the lowest cost node in the open list
				currentNode = OpenSet.Aggregate( 
					( curmin, x ) => 
					( ( curmin == null || ( x.CostTotal < curmin.CostTotal ) ) ? x : curmin ) 
				);

				SWFindMin.Stop();

				OpenSet.Remove( currentNode );
				ClosedSet.Add( currentNode );

				foreach ( NaiveAStarNode node in currentNode.Neighbors )
				{
					if ( node.Traversable && !ClosedSet.Contains( node ) )
					{
						float costFromThisNode = GetCostFromStart( node, currentNode );
						if ( !OpenSet.Contains( node ) )
						{
							OpenSet.Add( node );
							node.Parent = currentNode;

							NodesConsideredCount++;
						}
						else
						{
							// If this node would have been reached more easily by this route
							if ( node.CostFromStart > costFromThisNode )
							{
								node.Parent = currentNode;
							}
						}
						node.CostFromStart = costFromThisNode;
						node.CostTotal = node.CostFromStart + node.CostToGoal;
					}
				}

				MaxOpenSetCount = Math.Max( MaxOpenSetCount, OpenSet.Count );
			}

			SWBody.Stop();
			SWBacktrace.Start();

			for ( ; currentNode != null; currentNode = currentNode.Parent )
			{
				Path.Add( currentNode );
			}
			Path.Reverse();

			PathLength = mPath.Count;
			SWBacktrace.Stop();
			SWTotal.Stop();
		}

		#endregion


		///////////////////////////////////////////////////////////////////////////////////////////
		#region ctor

		public NaiveAStarAgent( INaiveAStarHeuristic heuristic, float coeffCostFromStart = 1f, float coeffCostToGoal = 1f )
		{
			SWTotal = new Stopwatch();
			SWSetup = new Stopwatch();
			SWBody = new Stopwatch();
			SWFindMin = new Stopwatch();
			SWBacktrace = new Stopwatch();

			Heuristic = heuristic;
			CoeffCostFromStart = coeffCostFromStart;
			CoeffCostToGoal = coeffCostToGoal;

			ClosedSet = new List<NaiveAStarNode>();
			OpenSet = new List<NaiveAStarNode>();
			Path = new List<NaiveAStarNode>();;
		}

		#endregion
	}
}

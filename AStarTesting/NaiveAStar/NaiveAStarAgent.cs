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
		bool mKeypressAdvance;
		public bool KeypressAdvance
		{
			get { return mKeypressAdvance; }
			set { mKeypressAdvance = value; }
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
		Stopwatch mSWClosedSet;
		public Stopwatch SWClosedSet
		{
			get { return mSWClosedSet; }
			set { mSWClosedSet = value; }
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
		Stopwatch mSWNodes;
		public Stopwatch SWNodes
		{
			get { return mSWNodes; }
			set { mSWNodes = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		Stopwatch mSWOpenSet;
		public Stopwatch SWOpenSet
		{
			get { return mSWOpenSet; }
			set { mSWOpenSet = value; }
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
			MaxOpenSetCount = 0;
			NodesConsideredCount = 0;
			PathLength = 0;
			SWBacktrace.Reset();
			SWBody.Reset();
			SWClosedSet.Reset();
			SWFindMin.Reset();
			SWNodes.Reset();
			SWOpenSet.Reset();
			SWSetup.Reset();
			SWTotal.Reset();
		}

		/// <summary>
		/// Solves for the first identified A* path from the StartNode to the GoalNode.
		/// </summary>
		public void Solve()
		{
			// Zero the benchmark variables
			ResetBenchmark();

			SWTotal.Start();
			SWSetup.Start();
			SWClosedSet.Start();

			// Clear the closed set
			ClosedSet.Clear();

			SWClosedSet.Stop();
			SWOpenSet.Start();

			// Clear the open set
			OpenSet.Clear();

			SWOpenSet.Stop();

			// Clear the path
			Path.Clear();

			SWNodes.Start();

			// Set the current node being considered
			NaiveAStarNode currentNode = StartNode;
			currentNode.Parent = null;
			currentNode.CostFromStart = 0;
			currentNode.CostToGoal = 0;
			currentNode.CostTotal = 0;

			SWNodes.Stop();
			SWClosedSet.Start();

			// Closed list of nodes to not be reconsidered
			mClosedSet.Add( currentNode );

			SWClosedSet.Stop();
		
			// Open list of nodes to consider and the node's parent (for backtracing)
			foreach ( NaiveAStarNode node in currentNode.Neighbors )
			{
				if ( node.Traversable )
				{
					SWOpenSet.Start();

					// Add the node to the open set
					mOpenSet.Add( node );

					SWOpenSet.Stop();
					SWNodes.Start();

					// Calculate its cost and set its parent to the current node
					node.Parent = currentNode;
					node.CostFromStart = GetCostFromStart( node );
					node.CostToGoal = GetCostToGoal( node );
					node.CostTotal = node.CostFromStart + node.CostToGoal;

					SWNodes.Stop();
				}

				NodesConsideredCount++;
			}

			SWSetup.Stop();
			SWBody.Start();

			if ( KeypressAdvance )
			{
				Console.WriteLine();
				Console.WriteLine( "Press any key to advance to the next iteration..." );
			}

			// Continue looping until the goal is reached
			while ( currentNode != GoalNode && OpenSet.Count > 0 )
			{
				if ( KeypressAdvance )
				{
					Console.ReadKey();
					Console.WriteLine( currentNode );
				}

				SWFindMin.Start();

				// Find the lowest cost node in the open list
				currentNode = OpenSet.Aggregate( 
					( curmin, x ) => 
					( ( curmin == null || ( x.CostTotal < curmin.CostTotal ) ) ? x : curmin ) 
				);

				SWFindMin.Stop();
				SWOpenSet.Start();

				// Remove this node on the open set
				OpenSet.Remove( currentNode );

				SWOpenSet.Stop();
				SWClosedSet.Start();

				// Then add it to the closed set
				ClosedSet.Add( currentNode );

				SWClosedSet.Stop();

				foreach ( NaiveAStarNode node in currentNode.Neighbors )
				{
					SWClosedSet.Start();

					// Check if the current node belongs to the closed set
					bool isInClosedSet = ClosedSet.Contains( node );

					SWClosedSet.Stop();

					// If this node is traversable and is NOT in the closed set
					if ( node.Traversable && !isInClosedSet )
					{
						SWOpenSet.Start();

						// Check if the current node is in the open set
						bool isInOpenSet = OpenSet.Contains( node );

						SWOpenSet.Stop();
						SWNodes.Start();

						// Compute the CoeffCostFromStart of moving to this node assuming the
						// current node were its parent
						float costFromThisNode = GetCostFromStart( node, currentNode );

						SWNodes.Stop();

						// If this node is NOT in the open set
						if ( !isInOpenSet )
						{
							SWOpenSet.Start();

							// Add the node to the open set
							OpenSet.Add( node );

							SWOpenSet.Stop();
							SWNodes.Start();

							// Calculate its cost and set its parent to the current node
							node.Parent = currentNode;
							node.CostFromStart = costFromThisNode;
							node.CostToGoal = GetCostToGoal( node );
							node.CostTotal = node.CostFromStart + node.CostToGoal;

							SWNodes.Stop();
							NodesConsideredCount++;
						}
						else
						{
							// If this node would have been reached more easily by this route
							if ( node.CostFromStart > costFromThisNode )
							{
								SWNodes.Start();

								// Reset the parent to the current node and recalculate the node cost
								node.Parent = currentNode;
								node.CostFromStart = costFromThisNode;
								// Don't need to update cost to goal -- that remains the same
								node.CostTotal = node.CostFromStart + node.CostToGoal;

								SWNodes.Stop();
							}
						}
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

			SWBacktrace.Stop();
			SWTotal.Stop();
			PathLength = mPath.Count;
		}

		public override string ToString()
		{
			return "Agent >> Heuristic: " + Heuristic.GetType().Name + 
				", Start: [ " + StartNode.Column + " : " + StartNode.Row + 
				" ], Goal: [ " + GoalNode.Column + " : " + GoalNode.Row + " ]";
		}

		#endregion


		///////////////////////////////////////////////////////////////////////////////////////////
		#region ctor

		public NaiveAStarAgent( INaiveAStarHeuristic heuristic, float coeffCostFromStart = 1f, float coeffCostToGoal = 1f, bool keypressAdvance = false )
		{
			SWBacktrace = new Stopwatch();
			SWBody = new Stopwatch();
			SWClosedSet = new Stopwatch();
			SWFindMin = new Stopwatch();
			SWNodes = new Stopwatch();
			SWOpenSet = new Stopwatch();
			SWSetup = new Stopwatch();
			SWTotal = new Stopwatch();
			
			KeypressAdvance = keypressAdvance;
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

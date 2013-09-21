using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplexNoise;

namespace AStarTesting.NaiveAStar
{
	public enum GridType
	{
		SquareGrid,		// A regular square grid where only directly adjacent squares are linked
		SquareDiagonal,	// A regular square grid where squares are linked to adjacent and diagonal squares
		HexGrid			// A regular hexagonal grid
	}
	
	public class NaiveAStarNavmesh
	{
		///////////////////////////////////////////////////////////////////////////////////////////
		#region Properties

		/// <summary>
		/// 
		/// </summary>
		Queue<NaiveAStarAgent> mAgents;
		public Queue<NaiveAStarAgent> Agents
		{
			get { return mAgents; }
			private set { mAgents = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		int mColumns;
		public int Columns
		{
			get { return mColumns; }
			private set { mColumns = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		GridType mGridType;
		public GridType GridType
		{
			get { return mGridType; }
			private set { mGridType = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		NaiveAStarNode[,] mNavmesh;
		public NaiveAStarNode[,] Navmesh
		{
			get { return mNavmesh; }
			private set { mNavmesh = value; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		int mRows;
		public int Rows
		{
			get { return mRows; }
			private set { mRows = value; }
		}

		#endregion


		///////////////////////////////////////////////////////////////////////////////////////////
		#region Methods

		/// <summary>
		/// Adds the specified agent to the agents queue if it isn't already in there.
		/// </summary>
		/// <param name="agentToAdd">The agent to add to the queue.</param>
		/// <returns>True if the specified agent was added. False if the specified user is in the queue already.</returns>
		public bool AddAgent( NaiveAStarAgent agentToAdd )
		{
			if ( Agents.Contains( agentToAdd ) )
				return false;
			
			Agents.Enqueue( agentToAdd );
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		public void GenerateResistanceNoise( float amplitude, float scale, float xOffset, float yOffset )
		{
			// Build the mesh nodes
			for ( int i = 0; i < Columns; i++ )
			{
				for ( int j = 0; j < Rows; j++ )
				{
					float xPerlin = xOffset + scale * i;
					float yPerlin = yOffset + scale * j;
					float perlinSample = 0.5f * amplitude * ( Noise.Generate( xPerlin, yPerlin ) + 1f );

					Navmesh[ i, j ].MoveCost = perlinSample;
				}
			}
		}

		/// <summary>
		/// Perform any startup operation necessary to prepare the nacmesh for traversal by agents.
		/// </summary>
		public void Init()
		{
			// Build the mesh nodes
			for ( int i = 0; i < Columns; i++ )
				for ( int j = 0; j < Rows; j++ )
					Navmesh[ i, j ] = new NaiveAStarNode( i, j, true, 1f );

			// Link the navmesh for the selected grid type
			switch ( GridType )
			{
				case GridType.SquareGrid:		LinkAdjacentSquareGrid();	break;
				case GridType.SquareDiagonal:	LinkDiagonalSquareGrid();	break;
				case GridType.HexGrid:			LinkHexGrid();				break;
			}
		}

		protected void LinkAdjacentSquareGrid()
		{
			// Link the mesh nodes into a square grid without diagonal links
			for ( int i = 0; i < Columns; i++ )
			{
				for ( int j = 0; j < Rows; j++ )
				{
					if ( i < Columns - 1 )
					{
						Navmesh[ i, j ].AddNeighbor( Navmesh[ i + 1, j ] );
						Navmesh[ i + 1, j ].AddNeighbor( Navmesh[ i, j ] );
					}

					if ( j < Rows - 1 )
					{
						Navmesh[ i, j ].AddNeighbor( Navmesh[ i, j + 1 ] );
						Navmesh[ i, j + 1 ].AddNeighbor( Navmesh[ i, j ] );
					}
				}
			}
		}

		protected void LinkDiagonalSquareGrid()
		{
			// Link the mesh nodes into a square grid WITH diagonal links
			for ( int i = 0; i < Columns; i++ )
			{
				for ( int j = 0; j < Rows; j++ )
				{
					if ( i < Columns - 1 )
					{
						Navmesh[ i, j ].AddNeighbor( Navmesh[ i + 1, j ] );
						Navmesh[ i + 1, j ].AddNeighbor( Navmesh[ i, j ] );
					}

					if ( j < Rows - 1 )
					{
						Navmesh[ i, j ].AddNeighbor( Navmesh[ i, j + 1 ] );
						Navmesh[ i, j + 1 ].AddNeighbor( Navmesh[ i, j ] );
					}

					if ( i < Columns - 1 && j < Rows - 1 )
					{
						Navmesh[ i, j ].AddNeighbor( Navmesh[ i + 1, j + 1 ] );
						Navmesh[ i + 1, j + 1 ].AddNeighbor( Navmesh[ i, j ] );
					}
				}
			}
		}

		protected void LinkHexGrid()
		{
			// Link the mesh nodes into a hex grid
			for ( int i = 0; i < Columns; i++ )
			{
				for ( int j = 0; j < Rows; j++ )
				{
					if ( j % 2 == 0 )
					{
						// Hex 0
						if ( i < Columns - 1 )
						{
							Navmesh[ i, j ].AddNeighbor( Navmesh[ i + 1, j ] );
							Navmesh[ i + 1, j ].AddNeighbor( Navmesh[ i, j ] );
						}

						// Hex 1
						if ( j < Rows - 1 )
						{
							Navmesh[ i, j ].AddNeighbor( Navmesh[ i, j + 1 ] );
							Navmesh[ i, j + 1 ].AddNeighbor( Navmesh[ i, j ] );
						}

						// Hex 2
						if ( i > 0 && j < Rows - 1 )
						{
							Navmesh[ i, j ].AddNeighbor( Navmesh[ i - 1, j + 1 ] );
							Navmesh[ i - 1, j + 1 ].AddNeighbor( Navmesh[ i, j ] );
						}
					}
					else
					{
						// Hex 0
						if ( i < Columns - 1 )
						{
							Navmesh[ i, j ].AddNeighbor( Navmesh[ i + 1, j ] );
							Navmesh[ i + 1, j ].AddNeighbor( Navmesh[ i, j ] );
						}

						// Hex 1
						if ( i < Columns - 1 && j < Rows - 1 )
						{
							Navmesh[ i, j ].AddNeighbor( Navmesh[ i + 1, j + 1 ] );
							Navmesh[ i + 1, j + 1 ].AddNeighbor( Navmesh[ i, j ] );
						}

						// Hex 2
						if ( j < Rows - 1 )
						{
							Navmesh[ i, j ].AddNeighbor( Navmesh[ i, j + 1 ] );
							Navmesh[ i, j + 1 ].AddNeighbor( Navmesh[ i, j ] );
						}
					}
				}
			}
		}

		/// <summary>
		/// Removes the specified agent from the agent queue.
		/// </summary>
		/// <param name="agentToRemove">The agent to remove from the queue.</param>
		/// <returns>True if the specified agent was removed. False if the specified user wan't found in the queue.</returns>
		public bool RemoveAgent( NaiveAStarAgent agentToRemove )
		{
			if ( !Agents.Contains( agentToRemove ) )
				return false;

			NaiveAStarAgent firstAgent = Agents.Dequeue();
			for ( NaiveAStarAgent nextAgent = Agents.Dequeue();   nextAgent != firstAgent;   nextAgent = Agents.Dequeue() )
			{
				if ( agentToRemove != nextAgent )
					Agents.Enqueue( nextAgent );
			}

			return true;
		}

		/// <summary>
		/// Solve the A* path for the next agent in the queue.
		/// </summary>
		/// <returns>The agent that a path was solved for.</returns>
		public NaiveAStarAgent SolveNext()
		{
			NaiveAStarAgent nextAgent = Agents.Dequeue();
			nextAgent.Solve();
			Agents.Enqueue( nextAgent );

			return nextAgent;
		}

		#endregion


		///////////////////////////////////////////////////////////////////////////////////////////
		#region ctor

		public NaiveAStarNavmesh( GridType gridType, int columns, int rows )
		{
			GridType = gridType;
			Columns = columns;
			Rows = rows;
			Agents = new Queue<NaiveAStarAgent>();
			Navmesh = new NaiveAStarNode[ mColumns, mRows ];

			Init();
		}

		#endregion
	}
}

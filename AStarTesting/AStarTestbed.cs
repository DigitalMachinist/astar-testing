using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AStarTesting.NaiveAStar;

namespace AStarTesting
{
	public class AStarTestbed
	{
		///////////////////////////////////////////////////////////////////////////////////////////
		#region Member Variables

		Dictionary<string, INaiveAStarHeuristic> heuristicsMap;
		bool		keypressAdvance;
		int			randomSeed;
		GridType	gridType;
		int			columns,				rows;
		int			agents,					iterations;
		float		minSimplexAmplitude,	maxSimplexAmplitude;
		float		minSimplexScale,		maxSimplexScale;
		float		minSimplexXOffset,		maxSimplexXOffset;
		float		minSimplexYOffset,		maxSimplexYOffset;
		float		minCoeffCostFromStart,	maxCoeffCostFromStart;
		float		minCoeffCostToGoal,		maxCoeffCostToGoal;

		#endregion

		public AStarTestbed()
		{
			///////////////////////////////////////////////////////////////////////////////////////
			#region Setup heuristics map

			heuristicsMap = new Dictionary<string,INaiveAStarHeuristic>();
			heuristicsMap.Add( "Dijakstra", new DijakstraAStarHeuristic() );
			heuristicsMap.Add( "Manhattan", new ManhattanAStarHeuristic() );
			heuristicsMap.Add( "StraightLine", new StraightLineAStarHeuristic() );

			#endregion
			

			///////////////////////////////////////////////////////////////////////////////////////
			#region Read configuration file

			XDocument xdoc = XDocument.Load( "config.xml" );

			// Read in the grid type
			string gridTypeString = xdoc.Descendants( "gridType" ).First().Value;
			switch ( gridTypeString )
			{
				case "SquareGrid":		gridType = GridType.SquareGrid;				break;
				case "SquareDiagonal":	gridType = GridType.SquareDiagonal;			break;
				case "HexGrid":			gridType = GridType.HexGrid;				break;
				default:				throw new ArgumentOutOfRangeException();
			}

			keypressAdvance			= bool.Parse( xdoc.Descendants( "keypressAdvance" ).First().Value );
			randomSeed				= int.Parse( xdoc.Descendants( "randomSeed" ).First().Value );
			columns					= int.Parse( xdoc.Descendants( "columns" ).First().Value );
			rows					= int.Parse( xdoc.Descendants( "rows" ).First().Value );
			agents					= int.Parse( xdoc.Descendants( "agents" ).First().Value );
			iterations				= int.Parse( xdoc.Descendants( "iterations" ).First().Value );
			minSimplexAmplitude		= float.Parse( xdoc.Descendants( "minSimplexAmplitude" ).First().Value );
			maxSimplexAmplitude		= float.Parse( xdoc.Descendants( "maxSimplexAmplitude" ).First().Value );
			minSimplexScale			= float.Parse( xdoc.Descendants( "minSimplexScale" ).First().Value );
			maxSimplexScale			= float.Parse( xdoc.Descendants( "maxSimplexScale" ).First().Value );
			minSimplexXOffset		= float.Parse( xdoc.Descendants( "minSimplexXOffset" ).First().Value );
			maxSimplexXOffset		= float.Parse( xdoc.Descendants( "maxSimplexXOffset" ).First().Value );
			minSimplexYOffset		= float.Parse( xdoc.Descendants( "minSimplexYOffset" ).First().Value );
			maxSimplexYOffset		= float.Parse( xdoc.Descendants( "maxSimplexYOffset" ).First().Value );
			minCoeffCostFromStart	= float.Parse( xdoc.Descendants( "minCoeffCostFromStart" ).First().Value );
			maxCoeffCostFromStart	= float.Parse( xdoc.Descendants( "maxCoeffCostFromStart" ).First().Value );
			minCoeffCostToGoal		= float.Parse( xdoc.Descendants( "minCoeffCostToGoal" ).First().Value );
			maxCoeffCostToGoal		= float.Parse( xdoc.Descendants( "maxCoeffCostToGoal" ).First().Value );

			Console.WriteLine();
			if ( keypressAdvance )	Console.WriteLine( "Keypress advance is ENABLED!" );
			else					Console.WriteLine( "Keypress advance is disabled." );
			Console.WriteLine();
			Console.WriteLine( "*** Configuration ***" );
			Console.WriteLine( "randomSeed:\t\t" + randomSeed );
			Console.WriteLine( "gridType:\t\t" + gridType.ToString() );
			Console.WriteLine( "columns:\t\t" + columns );
			Console.WriteLine( "rows:\t\t\t" + rows );
			Console.WriteLine( "agents:\t\t\t" + agents );
			Console.WriteLine( "iterations:\t\t" + iterations );
			Console.WriteLine( "minSimplexAmplitude:\t" + minSimplexAmplitude );
			Console.WriteLine( "maxSimplexAmplitude:\t" + maxSimplexAmplitude );
			Console.WriteLine( "minSimplexScale:\t" + minSimplexScale );
			Console.WriteLine( "maxSimplexScale:\t" + maxSimplexScale );
			Console.WriteLine( "minSimplexXOffset:\t" + minSimplexXOffset );
			Console.WriteLine( "maxSimplexXOffset:\t" + maxSimplexXOffset );
			Console.WriteLine( "minSimplexYOffset:\t" + minSimplexYOffset );
			Console.WriteLine( "maxSimplexYOffset:\t" + maxSimplexYOffset );
			Console.WriteLine( "minCoeffCostFromStart:\t" + minCoeffCostFromStart );
			Console.WriteLine( "maxCoeffCostFromStart:\t" + maxCoeffCostFromStart );
			Console.WriteLine( "minCoeffCostToGoal:\t" + minCoeffCostToGoal );
			Console.WriteLine( "maxCoeffCostToGoal:\t" + maxCoeffCostToGoal );

			// Build list of heuristics to use
			List<INaiveAStarHeuristic> heuristics = new List<INaiveAStarHeuristic>();
			foreach ( XElement heuristicTag in xdoc.Descendants( "heuristics" ) )
			{
				INaiveAStarHeuristic heuristic = null;
				if ( heuristicsMap.TryGetValue( heuristicTag.Value, out heuristic ) )
					heuristics.Add( heuristic );
			}

			Console.WriteLine();
			Console.WriteLine( "*** Heuristics ***" );
			foreach ( INaiveAStarHeuristic heuristic in heuristics )
			{
				Console.WriteLine( heuristic.GetType().Name );
			}

			#endregion


			///////////////////////////////////////////////////////////////////////////////////////
			#region Prepare output file

			string datetime = DateTime.Now.ToString( "yyyy-MM-dd_HH-mm-ss" );
			string filename = "results_" + datetime + ".csv";
			string output = String.Join( ",", 
				"Keypress Advance", 
				"Random Seed",
				"Grid Type",
				"Navmesh Columns",
				"Navmesh Rows",
				"Start Node",
				"Goal Node",
				"Noise Amplitude",
				"Noise Scale",
				"Noise Offset",
				"Heuristic",
				"Coeff From Start",
				"Coeff To Goal",
				"Total (ms)", 
				"Setup (ms)", 
				"Body (ms)", 
				"Find Min (ms)",
				"Backtrace (ms)", 
				"Closed Set (ms)", 
				"Open Set (ms)", 
				"Nodes (ms)", 
				"Total (ticks)", 
				"Setup (ticks)", 
				"Body (ticks)", 
				"Find Min (ticks)",
				"Backtrace (ticks)", 
				"Closed Set (ticks)", 
				"Open Set (ticks)", 
				"Nodes (ticks)", 
				"Nodes Considered", 
				"Max Closed Set", 
				"Max Open Set", 
				"Path Length", 
				"Path Nodes"
			);
			File.WriteAllText( filename, output, UTF8Encoding.UTF8 );

			Console.WriteLine();
			Console.WriteLine( "*** Output File ***" );
			Console.WriteLine( "Filename:\t" + filename );
			Console.WriteLine( "Encoding:\t" + UTF8Encoding.UTF8.ToString() );

			#endregion


			///////////////////////////////////////////////////////////////////////////////////////
			#region Set up the test

			// Set up the benckmarking tools
			AStarTestResult result = new AStarTestResult();
			Random random = new Random( randomSeed );
			Stopwatch stopwatch = new Stopwatch();

			// Create and size the navmesh -- It will not be resized after this
			NaiveAStarNavmesh navmesh = new NaiveAStarNavmesh( gridType, columns, rows );

			Console.WriteLine();
			Console.WriteLine( "*** Navmesh Nodes (10x10 Sample) ***" );
			for ( int x = 0; x < 10 && x < navmesh.Columns; x++ )
				for ( int y = 0; y < 10 && y < navmesh.Rows; y++ )
					Console.WriteLine( navmesh.Navmesh[ x, y ].ToString() );

			Console.WriteLine();
			Console.WriteLine( "*** Agents ***" );

			// Create a list of agents and randomize their configurations
			for ( int i = 0; i < agents; i++ )
			{
				INaiveAStarHeuristic heuristic = heuristics[ random.Next( 0, heuristics.Count ) ];
				float coeffCostFromStart = (float)( minCoeffCostFromStart + random.NextDouble() * ( maxCoeffCostFromStart - minCoeffCostFromStart ) );
				float coeffCostToGoal = (float)( minCoeffCostToGoal + random.NextDouble() * ( maxCoeffCostToGoal - minCoeffCostToGoal ) );

				NaiveAStarAgent agent = new NaiveAStarAgent( heuristic, coeffCostFromStart, coeffCostToGoal, keypressAdvance );

				int xStart = random.Next( 0, navmesh.Columns );
				int yStart = random.Next( 0, navmesh.Rows );
				agent.StartNode = navmesh.Navmesh[ xStart, yStart ];
				
				int xGoal = random.Next( 0, navmesh.Columns );
				int yGoal = random.Next( 0, navmesh.Rows );
				agent.GoalNode = navmesh.Navmesh[ xGoal, yGoal ];

				navmesh.AddAgent( agent );

				Console.WriteLine( agent );
			}

			#endregion


			// Wait for a keypress to begin testing
			Console.WriteLine();
			Console.WriteLine( "Press any key to begin benchmarks..." );
			Console.ReadKey();


			///////////////////////////////////////////////////////////////////////////////////////
			#region Run the benchmarks

			for ( int i = 0; i < iterations; i++ )
			{
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine( "Iteration " + ( i + 1 ) + " started!" );

				// Randomize terrain costs
				float simplexAmplitude = (float)( minSimplexAmplitude + random.NextDouble() * ( maxSimplexAmplitude - minSimplexAmplitude ) );
				float simplexScale = (float)( minSimplexScale + random.NextDouble() * ( maxSimplexScale - minSimplexScale ) );
				float simplexXOffset = (float)( minSimplexXOffset + random.NextDouble() * ( maxSimplexXOffset - minSimplexXOffset ) );
				float simplexYOffset = (float)( minSimplexYOffset + random.NextDouble() * ( maxSimplexYOffset - minSimplexYOffset ) );
				navmesh.GenerateResistanceNoise( simplexAmplitude, simplexScale, simplexXOffset, simplexYOffset );

#if ( DEBUG )	// Don't need to see raw move costs unless this is a debug build
				Console.WriteLine();
				Console.WriteLine( "*** Navmesh Move Costs ***" );
				for ( int x = 0; x < navmesh.Columns; x++ )
				{
					string navmeshRow = navmesh.Navmesh[ x, 0 ].MoveCost.ToString( "F2" );
					for ( int y = 1; y < navmesh.Rows; y++ )
					{
						navmeshRow += "\t" + navmesh.Navmesh[ x, y ].MoveCost.ToString( "F2" );
					}
					Console.WriteLine( navmeshRow );
				}
#endif

				Console.WriteLine();

				for ( int j = 0; j < agents; j++ )
				{
					NaiveAStarAgent peekAgent = navmesh.Agents.Peek();
					Console.WriteLine();
					Console.WriteLine( peekAgent );

					// Solve the A* path for this agent on the navmesh
					NaiveAStarAgent solvedAgent = navmesh.SolveNext();

					// Store the test parameters
					result.KeypressAdvance		= keypressAdvance;
					result.RandomSeed			= randomSeed;
					result.NavmeshGridType		= navmesh.GridType.ToString();
					result.NavmeshColumns		= navmesh.Columns;
					result.NavmeshRows			= navmesh.Rows;
					result.XStart				= solvedAgent.StartNode.Column;
					result.YStart				= solvedAgent.StartNode.Row;
					result.XGoal				= solvedAgent.GoalNode.Column;
					result.YGoal				= solvedAgent.GoalNode.Row;
					result.SimplexAmplitude		= simplexAmplitude;
					result.SimplexScale			= simplexScale;
					result.SimplexXOffset		= simplexXOffset;
					result.SimplexYOffset		= simplexYOffset;
					result.Heuristic			= solvedAgent.Heuristic.GetType().Name;
					result.CoeffCostFromStart	= solvedAgent.CoeffCostFromStart;
					result.CoeffCostToGoal		= solvedAgent.CoeffCostToGoal;

					// Store the test results
					result.MaxClosedSetCount	= solvedAgent.MaxClosedSetCount;
					result.MaxOpenSetCount		= solvedAgent.MaxOpenSetCount;
					result.NodesConsideredCount	= solvedAgent.NodesConsideredCount;
					result.PathLength			= solvedAgent.PathLength;
					result.PathString			= solvedAgent.PathString;
					result.MSBacktrace			= solvedAgent.SWBacktrace.ElapsedMilliseconds;
					result.TicksBacktrace		= solvedAgent.SWBacktrace.ElapsedTicks;
					result.MSBody				= solvedAgent.SWBody.ElapsedMilliseconds;
					result.TicksBody			= solvedAgent.SWBody.ElapsedTicks;
					result.MSClosedSet			= solvedAgent.SWClosedSet.ElapsedMilliseconds;
					result.TicksClosedSet		= solvedAgent.SWClosedSet.ElapsedTicks;
					result.MSFindMin			= solvedAgent.SWFindMin.ElapsedMilliseconds;
					result.TicksFindMin			= solvedAgent.SWFindMin.ElapsedTicks;
					result.MSNodes				= solvedAgent.SWNodes.ElapsedMilliseconds;
					result.TicksNodes			= solvedAgent.SWNodes.ElapsedTicks;
					result.MSOpenSet			= solvedAgent.SWOpenSet.ElapsedMilliseconds;
					result.TicksOpenSet			= solvedAgent.SWOpenSet.ElapsedTicks;
					result.MSSetup				= solvedAgent.SWSetup.ElapsedMilliseconds;
					result.TicksSetup			= solvedAgent.SWSetup.ElapsedTicks;
					result.MSTotal				= solvedAgent.SWTotal.ElapsedMilliseconds;
					result.TicksTotal			= solvedAgent.SWTotal.ElapsedTicks;

					// Write results to the output file
					File.AppendAllText( filename, "\n" + result, UTF8Encoding.UTF8 );

					Console.WriteLine( "Result " + ( j + 1 ) + " stored." );
				}

				Console.WriteLine();
				Console.WriteLine( "Iteration complete!" );
			}

			#endregion
			

			// Wait for a keypress to close
			Console.WriteLine();
			Console.WriteLine( "Press any key to exit..." );
			Console.ReadKey();
		}
	}
}

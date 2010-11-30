#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Angels_Vs_Demons.BoardObjects;
using Angels_Vs_Demons.GameObjects;
using Angels_Vs_Demons.GameObjects.Units;
using Angels_Vs_Demons.Util;
#endregion

namespace Angels_Vs_Demons.Players
{
    class ComputerPlayer : Player
    {
        // Board used for the computer moves
        private AvDGame currentBoard;

        // Used for generation of random numbers
        private Random random = new Random();

        /// <summary>
        /// Stores values of all units.
        /// </summary>
        private Dictionary<string, int> unitValues = new Dictionary<string, int>();

        /// <summary>
        /// Default weighting for board positions.
        /// </summary>
        private int[] DEFAULT_WEIGHTS ={1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
                                      1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
                                      1, 1, 2, 2, 3, 3, 2, 2, 1, 1, 
                                      1, 1, 2, 2, 3, 3, 2, 2, 1, 1, 
                                      1, 1, 2, 2, 3, 3, 2, 2, 1, 1, 
                                      1, 1, 2, 2, 3, 3, 2, 2, 1, 1, 
                                      1, 1, 2, 2, 3, 3, 2, 2, 1, 1, 
                                      1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
                                      1, 1, 1, 1, 1, 1, 1, 1, 1, 1};

        /// <summary>
        /// Weighting for champion board positions.
        /// </summary>
        private int[] CHAMPION_WEIGHTS = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
                                      1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
                                      1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
                                      1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
                                      1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
                                      1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
                                      1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
                                      1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
                                      1, 1, 1, 1, 1, 1, 1, 1, 1, 1};

        /// <summary>
        /// Weighting for board positions
        /// </summary>
        private int[] tableWeights;        
         
        internal AvDGame CurrentBoard
        {
          get { return currentBoard; }
          set { currentBoard = value; }
        }

        // Max depth used in the Min-Max algorithm
        private int maxDepth = 1;

        /// <summary>
        /// Creates a computer player
        /// </summary>
        /// <param name="faction">the faction to associate with this player</param>
        /// <param name="boardDepth">the maxdepth of the game tree</param>
        public ComputerPlayer(Faction faction, int boardDepth)
            : base(faction)
        {
            maxDepth = boardDepth;
            unitValues.Add("Champion", 10);
            unitValues.Add("Archer", 4);
            unitValues.Add("Guard", 4);
            unitValues.Add("Knight", 6);
            unitValues.Add("Mage", 8);
            unitValues.Add("Peon", 2);
        }

        /// <summary>
        /// Gets/sets the max depth of the min-max tree.
        /// </summary>
        public int depth
        {
            get
            {
                return maxDepth;
            }
            set
            {
                maxDepth = value;
            }
        }

        /// <summary>
        /// Interface used for the game with the AI.
        /// </summary>
        /// <param name="board">the game to make a move on</param>
        /// <returns>the turn to apply to the board</returns>
        public Turn getTurn(AvDGame board)
        {
            CurrentBoard = (AvDGame)board.Clone();

            Debug.WriteLine("******************************************************************");
            Debug.WriteLine("Computing AI move...");
            Debug.WriteLine("******************************************************************");

            return minimax(CurrentBoard);
        }

        /// <summary>
        /// Checks to see if there are any more moves. (says if the game move is valid)
        /// </summary>
        /// <param name="moves">The list of piece movements for the game move.</param>
        /// <returns>true if the game move is valid, false otherwise.</returns>
        private bool mayPlay(List moves)
        {
            return !moves.isEmpty();
                //&& !((List)moves.peek_head()).isEmpty();
        }

        /// <summary>
        /// Implements the Min-Max algorithm for selecting the computer move.
        /// </summary>
        /// <param name="board">The board that will be used as a starting point for generating the game movements</param>
        /// <returns>the computer's <code>Turn</code> to apply to the board</returns>
        private Turn minimax(AvDGame board)
        {
            List sucessors, bestMove = new List();
            Turn candidateTurn, bestTurn = null;
            AvDGame nextBoard;
            int totalMoves;
            int value, maxValue = Int32.MinValue;

            sucessors = board.getValidTurns();
            while (mayPlay(sucessors))
            {
                candidateTurn = (Turn)sucessors.pop_front();
                nextBoard = (AvDGame)board.Clone();

                Debug.WriteLine("******************************************************************");
                nextBoard.applyTurn(candidateTurn);
                nextBoard.endTurn();
                nextBoard.beginTurn();

                value = minMove(nextBoard, 1, maxValue, Int32.MaxValue);

                if (value > maxValue)
                {
                    Debug.WriteLine("Max value : " + value + " at depth : 0");
                    maxValue = value;
                    bestMove.clear();
                    bestMove.push_front(candidateTurn);
                } else if (value == maxValue) {
                    Debug.WriteLine("Max value (equal): " + value + " at depth : 0");
                    bestMove.push_front(candidateTurn);
                }
            }

            totalMoves = bestMove.length();

            Debug.WriteLine("Move value selected : " + maxValue + " at depth : 0");
            Debug.WriteLine("Total number of moves to select from: " + totalMoves);

            if (totalMoves > 1)
            {
                bestTurn = (Turn)bestMove.get(random.Next(totalMoves-1)); // Select Turn randomly from 0 to totalMoves.
            }

            return bestTurn;
        }

        /// <summary>
        /// Implements game move evaluation from the point of view of the MAX player.
        /// </summary>
        /// <param name="board">The board that will be used as a starting point for generating the game movements</param>
        /// <param name="depth">Current depth in the Min-Max tree</param>
        /// <param name="alpha">Current alpha value for the alpha-beta cutoff</param>
        /// <param name="beta">Current beta value for the alpha-beta cutoff</param>
        /// <returns>Move evaluation value</returns>
        private int maxMove(AvDGame board, int depth, int alpha, int beta)
        {
            if (cutOffTest(board, depth))
            {
                return eval(board);
            }

            List sucessors;
            Turn move;
            AvDGame nextBoard;
            int value;
            int maxValue = Int32.MinValue;

            Debug.WriteLine("Max node at depth : " + depth + " with alpha : " + alpha +
                                " beta : " + beta);

            sucessors = board.getValidTurns();
            while (mayPlay(sucessors))
            {
                move = (Turn)sucessors.pop_front();
                nextBoard = (AvDGame)board.Clone();
                nextBoard.applyTurn(move);
                nextBoard.endTurn();
                nextBoard.beginTurn();

                value = minMove(nextBoard, depth + 1, alpha, beta);

                //if (value > alpha)
                //{
                //    alpha = value;
                //    Debug.WriteLine("Max value : " + value + " at depth : " + depth);
                //}

                //if (alpha > beta)
                //{
                //    Debug.WriteLine("Max value with prunning : " + beta + " at depth : " + depth);
                //    Debug.WriteLine(sucessors.length() + " sucessors left");
                //    return beta;
                //}

                if (value > maxValue)
                {
                    maxValue = value;
                }
            }

            Debug.WriteLine("Max value selected : " + alpha + " at depth : " + depth);
            return alpha;
        }

        /// <summary>
        /// Implements game move evaluation from the point of view of the MIN player.
        /// </summary>
        /// <param name="board">The board that will be used as a starting point for generating the game movements</param>
        /// <param name="depth">Current depth in the Min-Max tree</param>
        /// <param name="alpha">Current alpha value for the alpha-beta cutoff</param>
        /// <param name="beta">Current beta value for the alpha-beta cutoff</param>
        /// <returns>Move evaluation value</returns>
        private int minMove(AvDGame board, int depth, int alpha, int beta)
        {
            if (cutOffTest(board, depth))
            {
                return eval(board);
            }

            List sucessors;
            Turn move;
            AvDGame nextBoard;
            int value;
            int maxValue = Int32.MinValue;

            Debug.WriteLine("Min node at depth : " + depth + " with alpha : " + alpha +
                                " beta : " + beta);

            sucessors = (List)board.getValidTurns();
            while (mayPlay(sucessors))
            {
                move = (Turn)sucessors.pop_front();
                nextBoard = (AvDGame)board.Clone();
                nextBoard.applyTurn(move);
                nextBoard.endTurn();
                nextBoard.beginTurn();

                value = maxMove(nextBoard, depth + 1, alpha, beta);

                //if (value < beta)
                //{
                //    beta = value;
                //    Debug.WriteLine("Min value : " + value + " at depth : " + depth);
                //}

                //if (beta < alpha)
                //{
                //    Debug.WriteLine("Min value with prunning : " + alpha + " at depth : " + depth);
                //    Debug.WriteLine(sucessors.length() + " sucessors left");
                //    return alpha;
                //}

                if (value > maxValue)
                {
                    maxValue = value;
                }
            }

            Debug.WriteLine("Min value selected : " + beta + " at depth : " + depth);
            return beta;
        }

        /// <summary>
        /// Evaluates the strength of the current player compared to the opossing player
        /// </summary>
        /// <param name="board">The board where the current player position will be evaluated.</param>
        /// <returns>the player strength</returns>
        private int eval(AvDGame board)
        {
            int colorForce = 0;
            int enemyForce = 0;
            int pos = 0;
            Unit unit;       

            for (int i = 0; i < board.Board.Grid.Length; i++)
            {
                foreach (Tile tile in board.Board.Grid[i])
                {
                    if (tile.IsOccupied)
                    {
                        unit = tile.Unit;
                        calculateTableWeights(unit);

                        if (unit.FactionType == this.Faction)
                        {
                            colorForce += calculateValue(unit, tableWeights[pos]);
                        }
                        else
                        {
                            enemyForce += calculateValue(unit, tableWeights[pos]);
                        }
                    }
                    pos++;
                }
            }

            return colorForce - enemyForce;
        }

        /// <summary>
        /// Evaluates the strength of a unit
        /// </summary>
        /// <param name="unit">the unit to calculate</param>
        /// <returns>the unit value</returns>
        private int calculateValue(Unit unit, int pos)
        {
            int value = unitValues[unit.GetType().Name];
            //Debug.WriteLine("Calculating value for unit: " + unit.Name);

            value = (value * unit.CurrHP) + pos;

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="board"></param>
        private void calculateTableWeights(Unit unit)
        {
            if (unit is Champion)
            {
                this.tableWeights = CHAMPION_WEIGHTS;
            }
            else
            {
                this.tableWeights = DEFAULT_WEIGHTS;
            }
        }

        /// <sumary> 
        ///   Verifies if the game tree can be prunned
        /// </sumary>
        /// <param name="board">
        ///   The board to evaluate
        /// </param>
        /// <param name="depth">
        ///   Current game tree depth
        /// </param>
        /// <value>
        ///  true if the tree can be prunned.
        /// </value>
        private bool cutOffTest(AvDGame board, int depth)
        {
            return depth >= maxDepth || board.IsOver;
        }

    }
}

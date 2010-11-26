#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Angels_Vs_Demons.BoardObjects;
using Angels_Vs_Demons.Screens.GameplayScreens;
using Angels_Vs_Demons.Util;
using Angels_Vs_Demons.GameObjects.Units;
using Angels_Vs_Demons.GameObjects;
#endregion

namespace Angels_Vs_Demons.Players
{
    class ComputerPlayer : Player
    {
        // Board used for the computer moves
        private Board currentBoard;

        // Used for generation of random numbers
        private Random random = new Random();

        /// <summary>
        /// Stores values of all units.
        /// </summary>
        private Dictionary<string, int> unitValues = new Dictionary<string, int>();

        internal Board CurrentBoard
        {
          get { return currentBoard; }
          set { currentBoard = value; }
        }

        // Max depth used in the Min-Max algorithm
        private int maxDepth = 1;

        public ComputerPlayer(Faction faction)
            : base(faction)
        {
            unitValues.Add("Champion", 100);
            unitValues.Add("Archer", 25);
            unitValues.Add("Guard", 20);
            unitValues.Add("Knight", 30);
            unitValues.Add("Mage", 30);
            unitValues.Add("Peon", 10);
        }

        /// <sumary> 
        ///   Allows the user to change the max depth of
        ///  the min-max tree
        /// </sumary>
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

        public Turn getTurn(Board board)
        {
            CurrentBoard = (Board)board.Clone();

            Debug.WriteLine("******************************************************************");
            Debug.WriteLine("Computing AI move...");
            Debug.WriteLine("******************************************************************");

            return minimax(CurrentBoard);
        }

        /// <sumary> 
        ///   Says if the game move is valid
        /// </sumary>
        /// <param name="moves">
        ///  The list of piece movements for the game move.
        /// </param>
        /// <value>
        ///  true if the game move is valid, false otherwise.
        /// </value>
        private bool mayPlay(List moves)
        {
            return !moves.isEmpty();
                //&& !((List)moves.peek_head()).isEmpty();
        }

        /// <sumary> 
        ///   Implements the Min-Max algorithm for selecting
        ///  the computer move
        /// </sumary>
        /// <param name="board">
        ///   The board that will be used as a starting point
        ///  for generating the game movements
        /// </param>
        /// <value>
        ///  A list with the computer game movements.
        /// </value>
        private Turn minimax(Board board)
        {
            List sucessors, bestMove = new List();
            Turn move, turn = null;
            Board nextBoard;
            int totalMoves;
            int value, maxValue = Int32.MinValue;

            sucessors = board.getValidTurns();
            while (mayPlay(sucessors))
            {
                move = (Turn)sucessors.pop_front();
                nextBoard = (Board)board.Clone();

                Debug.WriteLine("******************************************************************");
                nextBoard.applyTurn(move);
                nextBoard.endTurn();
                nextBoard.beginTurn();

                value = minMove(nextBoard, 1, maxValue, Int32.MaxValue);

                if (value > maxValue)
                {
                    Debug.WriteLine("Max value : " + value + " at depth : 0");
                    maxValue = value;
                    bestMove.clear();
                    bestMove.push_front(move);
                } else if (value == maxValue){
                    Debug.WriteLine("Max value (equal): " + value + " at depth : 0");
                    bestMove.push_front(move);
                }
            }

            totalMoves = bestMove.length();

            Debug.WriteLine("Move value selected : " + maxValue + " at depth : 0");
            Debug.WriteLine("Total number of moves to select from: " + totalMoves);

            if (totalMoves > 1)
            {
                turn = (Turn)bestMove.get(random.Next(totalMoves-1)); // Select Turn randomly from 0 to totalMoves.
            }

            return turn;
        }

        /// <sumary> 
        ///   Implements game move evaluation from the point of view of the
        ///  MAX player.
        /// </sumary>
        /// <param name="board">
        ///   The board that will be used as a starting point
        ///  for generating the game movements
        /// </param>
        /// <param name="depth">
        ///   Current depth in the Min-Max tree
        /// </param>
        /// <param name="alpha">
        ///   Current alpha value for the alpha-beta cutoff
        /// </param>
        /// <param name="beta">
        ///   Current beta value for the alpha-beta cutoff
        /// </param>
        /// <value>
        ///  Move evaluation value
        /// </value>
        private int maxMove(Board board, int depth, int alpha, int beta)
        {
            if (cutOffTest(board, depth))
            {
                return eval(board);
            }

            List sucessors;
            Turn move;
            Board nextBoard;
            int value;

            Debug.WriteLine("Max node at depth : " + depth + " with alpha : " + alpha +
                                " beta : " + beta);

            sucessors = board.getValidTurns();
            while (mayPlay(sucessors))
            {
                move = (Turn)sucessors.pop_front();
                nextBoard = (Board)board.Clone();
                nextBoard.applyTurn(move);
                nextBoard.endTurn();
                nextBoard.beginTurn();

                value = minMove(nextBoard, depth + 1, alpha, beta);

                if (value > alpha)
                {
                    alpha = value;
                    Debug.WriteLine("Max value : " + value + " at depth : " + depth);
                }

                if (alpha > beta)
                {
                    Debug.WriteLine("Max value with prunning : " + beta + " at depth : " + depth);
                    Debug.WriteLine(sucessors.length() + " sucessors left");
                    return beta;
                }

            }

            Debug.WriteLine("Max value selected : " + alpha + " at depth : " + depth);
            return alpha;
        }

        /// <sumary> 
        ///   Implements game move evaluation from the point of view of the
        ///  MIN player.
        /// </sumary>
        /// <param name="board">
        ///   The board that will be used as a starting point
        ///  for generating the game movements
        /// </param>
        /// <param name="depth">
        ///   Current depth in the Min-Max tree
        /// </param>
        /// <param name="alpha">
        ///   Current alpha value for the alpha-beta cutoff
        /// </param>
        /// <param name="beta">
        ///   Current beta value for the alpha-beta cutoff
        /// </param>
        /// <value>
        ///  Move evaluation value
        /// </value>
        private int minMove(Board board, int depth, int alpha, int beta)
        {
            if (cutOffTest(board, depth))
            {
                return eval(board);
            }

            List sucessors;
            Turn move;
            Board nextBoard;
            int value;

            Debug.WriteLine("Min node at depth : " + depth + " with alpha : " + alpha +
                                " beta : " + beta);

            sucessors = (List)board.getValidTurns();
            while (mayPlay(sucessors))
            {
                move = (Turn)sucessors.pop_front();
                nextBoard = (Board)board.Clone();
                nextBoard.applyTurn(move);
                nextBoard.endTurn();
                nextBoard.beginTurn();

                value = maxMove(nextBoard, depth + 1, alpha, beta);

                if (value < beta)
                {
                    beta = value;
                    Debug.WriteLine("Min value : " + value + " at depth : " + depth);
                }

                if (beta < alpha)
                {
                    Debug.WriteLine("Min value with prunning : " + alpha + " at depth : " + depth);
                    Debug.WriteLine(sucessors.length() + " sucessors left");
                    return alpha;
                }
            }

            Debug.WriteLine("Min value selected : " + beta + " at depth : " + depth);
            return beta;
        }

        /// <sumary> 
        ///   Evaluates the strength of the current player
        /// </sumary>
        /// <param name="board">
        ///   The board where the current player position will be evaluated.
        /// </param>
        /// <value>
        ///  Player strength
        /// </value>
        private int eval(Board board)
        {
            int colorForce = 0;
            int enemyForce = 0;
            Unit unit;

            for (int i = 0; i < board.Grid.Length; i++)
            {
                foreach (Tile tile in board.Grid[i])
                {
                    if (tile.IsOccupied)
                    {
                        unit = tile.Unit;

                        if (tile.Unit.FactionType == this.Faction)
                        {
                            colorForce += calculateValue(unit);
                        }
                        else
                        {
                            enemyForce += calculateValue(unit);
                        }
                    }
                }
            }

            return colorForce - enemyForce;
        }

        /// <sumary> 
        ///   Evaluates the strength of a piece
        /// </sumary>
        /// <param name="piece">
        ///   The type of piece
        /// </param>
        /// <param name="pos">
        ///   The piece position
        /// </param>
        /// <value>
        ///  Piece value
        /// </value>
        private int calculateValue(Unit unit)
        {
            int value = unitValues[unit.GetType().Name];
            Debug.WriteLine("Calculating value for unit: " +unit.Name);

            value = value + unit.CurrHP;

            return value;
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
        private bool cutOffTest(Board board, int depth)
        {
            return depth > maxDepth; 
                //TODO: check for win
                //|| board.hasEnded();
        }

    }
}

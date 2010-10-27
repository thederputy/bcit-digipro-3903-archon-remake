package ca.bcit.cst.comp2526.assign3.solution;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;
import java.util.logging.Level;
import java.util.logging.Logger;


public class ComputerTicTacToePlayer
    extends TicTacToePlayer
{

    private TicTacToeGame currentGame;
    private int maxDepth = 1;
    boolean winner = false;
    
    public ComputerTicTacToePlayer(final String name)
    {
        super(name);
    }

    public TicTacToeLocation getMove(final TicTacToeGame game)
    {
        TicTacToeLocation result;
        winner = false;
        currentGame = new TicTacToeGame(game);
        MutableTicTacToeBoard currentBoard = (MutableTicTacToeBoard)game.getBoard();
        currentBoard = (MutableTicTacToeBoard)currentBoard.clone();

        System.err.println("******************************************************************");
        System.err.println("Turn number: " + game.getTurnCount());
        System.err.println("******************************************************************");

        result = minimax(currentGame, currentBoard, 0);
        return (result);
    }

        /**
    * Implements the Min-Max algorithm for selecting
    * the computer move
    *
    * @param board The board that will be used as a starting point
    * for generating the game movements
    *
    * @return A list with the computer game movements.
    */
    private TicTacToeLocation minimaxTest(TicTacToeGame game, MutableTicTacToeBoard board, int depth) {
        List<TicTacToeLocation> sucessors;
        TicTacToeLocation move, bestMove = null;
        MutableTicTacToeBoard nextBoard;
        int value, maxValue = Integer.MIN_VALUE;

        bestMove = currentGame.getPossibleMoves(board).get(0);
        sucessors = currentGame.getPossibleMoves(board);
        while (mayPlay(sucessors) && !winner) {
            move = sucessors.get(0);
            sucessors.remove(0);
            nextBoard = (MutableTicTacToeBoard)board.clone();

            System.err.println("******************************************************************");
            try {
                nextBoard = game.performMove(move, nextBoard);
            } catch (InvalidMoveException ex) {
                Logger.getLogger(ComputerTicTacToePlayer.class.getName()).log(Level.SEVERE, null, ex);
            }

            bestMove = move;

            try {
                winner = game.checkForWinner(game.getCurrentPlayer(), nextBoard);
            } catch (InvalidLocationException ex) {
                Logger.getLogger(ComputerTicTacToePlayer.class.getName()).log(Level.SEVERE, null, ex);
            }

            if(winner) {
                System.err.println("Move value selected : " + move.toString() + " at depth : " + depth);
                break;
            } else if(depth >= maxDepth) {
                break;
            } else {
                depth++;
                minimax(game, nextBoard, depth);
            }




//            value = minMove(nextBoard, 1, maxValue, Int32.MaxValue);
//
//            if (value > maxValue) {
//                System.err.println("Max value : " + value + " at depth : 0");
//                maxValue = value;
//                bestMove = move;
//            }
        }

        return bestMove;
    }

    /**
     * Says if the game move is valid
     *
     * @param moves The list of piece movements for the game move.
     * @return true if the game move is valid, false otherwise.
     */
    private boolean mayPlay (List<TicTacToeLocation> moves) {
        return !moves.isEmpty() ;
                //&& (moves.size() > 1);
                //&& !((List) moves.peek_head ()).isEmpty ();
    }

    /**
    * Implements the Min-Max algorithm for selecting
    * the computer move
    *
    * @param board The board that will be used as a starting point
    * for generating the game movements
    *
    * @return A list with the computer game movements.
    */
    private TicTacToeLocation minimax(TicTacToeGame game, MutableTicTacToeBoard board, int depth) {
        List<TicTacToeLocation> sucessors;
        TicTacToeLocation move, bestMove = null;
        MutableTicTacToeBoard nextBoard;
        int value = Integer.MIN_VALUE;
        int maxValue = Integer.MIN_VALUE;

        bestMove = currentGame.getPossibleMoves(board).get(0);
        sucessors = currentGame.getPossibleMoves(board);
        nextBoard = (MutableTicTacToeBoard)board.clone();

        if(game.getTurnCount() == 1) {

            Random rand = new Random();
            List<TicTacToeLocation> firstMoveChoices = new ArrayList<TicTacToeLocation>();
            
            try {
                
                // If first move wasn't the middle, then move to the middle
                if(nextBoard.getSquare(new TicTacToeLocation(1, 1)).getPiece() == null) {

                    firstMoveChoices.add(new TicTacToeLocation(1,1));
                }

                // If first move was middle, then move to any corner
                else {
                    firstMoveChoices.add(new TicTacToeLocation(0,0));
                    firstMoveChoices.add(new TicTacToeLocation(0,2));
                    firstMoveChoices.add(new TicTacToeLocation(2,0));
                    firstMoveChoices.add(new TicTacToeLocation(2,2));
                }
            } catch (InvalidLocationException ex) {
                Logger.getLogger(ComputerTicTacToePlayer.class.getName()).log(Level.SEVERE, null, ex);
            }

            if(!firstMoveChoices.isEmpty()){
                return firstMoveChoices.get(rand.nextInt(firstMoveChoices.size()));
            }
        }

        try {
            Random rand = new Random();
            List<TicTacToeLocation> secondMoveChoices = new ArrayList<TicTacToeLocation>();
            if ((game.getTurnCount() == 3) &&
                (!currentGame.checkForBlockWin(currentGame.getFirstPlayer(), nextBoard))) {

                /**
                 * Check possible counters if computer is in middle.
                 */
                if(nextBoard.getSquare(new TicTacToeLocation(1,1)).getPiece()
                        .getOwner().getName().equalsIgnoreCase("O")) {

                    /* If current game state is any variation of:
                     * [ ][ ][X]
                     * [ ][O][ ]
                     * [X][ ][ ]
                     *
                     * then move vertically or horizontally adjacent to middle
                     */
                    if(nextBoard.getSquare(new TicTacToeLocation(0,0)).getPiece()!=null &&
                       nextBoard.getSquare(new TicTacToeLocation(2,2)).getPiece()!=null ||
                       nextBoard.getSquare(new TicTacToeLocation(2,0)).getPiece()!=null &&
                       nextBoard.getSquare(new TicTacToeLocation(0,2)).getPiece()!=null) {

                       secondMoveChoices.add(new TicTacToeLocation(0,1));
                       secondMoveChoices.add(new TicTacToeLocation(1,0));
                       secondMoveChoices.add(new TicTacToeLocation(1,2));
                       secondMoveChoices.add(new TicTacToeLocation(2,1));
                    } 
                    
                    /* If current game state is any variation of:
                     * [ ][X][ ]
                     * [ ][O][ ]
                     * [X][ ][ ]
                     *
                     * then move to the corner parallel to X in corner and 
                     * adjacent to other X
                     */
                    else if(nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!= null &&
                            nextBoard.getSquare(new TicTacToeLocation(2,0)).getPiece()!=null) {

                        secondMoveChoices.add(new TicTacToeLocation(0,0));
                    } 

                    else if(nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!=null &&
                            nextBoard.getSquare(new TicTacToeLocation(2,2)).getPiece()!=null) {

                        secondMoveChoices.add(new TicTacToeLocation(0,2));
                    } 

                    else if(nextBoard.getSquare(new TicTacToeLocation(0,2)).getPiece()!=null &&
                              nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null) {

                        secondMoveChoices.add(new TicTacToeLocation(2,2));
                    } 

                    else if(nextBoard.getSquare(new TicTacToeLocation(0,0)).getPiece()!=null &&
                              nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null) {

                        secondMoveChoices.add(new TicTacToeLocation(2,0));
                    } 

                    else if(nextBoard.getSquare(new TicTacToeLocation(0,0)).getPiece()!=null &&
                              nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null) {

                        secondMoveChoices.add(new TicTacToeLocation(0,2));
                    } 

                    else if(nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null &&
                              nextBoard.getSquare(new TicTacToeLocation(2,0)).getPiece()!=null) {

                        secondMoveChoices.add(new TicTacToeLocation(2,2));
                    } 

                    else if(nextBoard.getSquare(new TicTacToeLocation(0,2)).getPiece()!=null &&
                              nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null) {

                        secondMoveChoices.add(new TicTacToeLocation(0,0));
                    } 

                    else if(nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null &&
                              nextBoard.getSquare(new TicTacToeLocation(2,2)).getPiece()!=null) {

                        secondMoveChoices.add(new TicTacToeLocation(2,0));
                    }

                    /* If current game state is any variation of:
                     * [ ][X][ ]
                     * [X][O][ ]
                     * [ ][ ][ ]
                     *
                     * then move to the corner adjacent to both X's
                     */

                    // Move to top left
                    else if(nextBoard.getSquare(new TicTacToeLocation(1, 0)).getPiece() != null &&
                       nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece().getOwner().getName().equalsIgnoreCase("X") &&
                       nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!=null &&
                       nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece().getOwner().getName().equalsIgnoreCase("X")) {

                        secondMoveChoices.add(new TicTacToeLocation(0,0));
                    }
                    // Move to top right
                    else if(nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!=null &&
                            nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece().getOwner().getName().equalsIgnoreCase("X") &&
                            nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null &&
                            nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece().getOwner().getName().equalsIgnoreCase("X")) {

                        secondMoveChoices.add(new TicTacToeLocation(0,2));
                    }
                    // Move to bottom right
                    else if(nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null &&
                            nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece().getOwner().getName().equalsIgnoreCase("X") &&
                            nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null &&
                            nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece().getOwner().getName().equalsIgnoreCase("X")) {

                        secondMoveChoices.add(new TicTacToeLocation(2,2));
                    }
                    // Move to bottom left
                    else if(nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null &&
                            nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece().getOwner().getName().equalsIgnoreCase("X") &&
                            nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null &&
                            nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece().getOwner().getName().equalsIgnoreCase("X")) {

                        secondMoveChoices.add(new TicTacToeLocation(2,0));
                    }
                }

                /* If current game state is any variation of:
                 * [ ][ ][X]
                 * [ ][X][ ]
                 * [O][ ][ ]
                 *
                 * then move to either of the empty corners
                 */

                // x on top right
                else if(nextBoard.getSquare(new TicTacToeLocation(0,2)).getPiece()!=null &&
                        nextBoard.getSquare(new TicTacToeLocation(0,2)).getPiece().getOwner().getName().equalsIgnoreCase("X")){

                    secondMoveChoices.add(new TicTacToeLocation(0,0));
                    secondMoveChoices.add(new TicTacToeLocation(2,2));
                }
                // x on top left
                else if(nextBoard.getSquare(new TicTacToeLocation(0,0)).getPiece()!=null &&
                        nextBoard.getSquare(new TicTacToeLocation(0,0)).getPiece().getOwner().getName().equalsIgnoreCase("X")){

                    secondMoveChoices.add(new TicTacToeLocation(0,2));
                    secondMoveChoices.add(new TicTacToeLocation(2,0));
                }
                // x bottom right
                else if(nextBoard.getSquare(new TicTacToeLocation(2,2)).getPiece()!=null &&
                        nextBoard.getSquare(new TicTacToeLocation(2,2)).getPiece().getOwner().getName().equalsIgnoreCase("X")){

                    secondMoveChoices.add(new TicTacToeLocation(0,2));
                    secondMoveChoices.add(new TicTacToeLocation(2,0));
                }
                // x bottom left
                else if(nextBoard.getSquare(new TicTacToeLocation(2,0)).getPiece()!=null &&
                        nextBoard.getSquare(new TicTacToeLocation(2,0)).getPiece().getOwner().getName().equalsIgnoreCase("X")){

                    secondMoveChoices.add(new TicTacToeLocation(0,0));
                    secondMoveChoices.add(new TicTacToeLocation(2,2));
                }

//                /* If current game state is any variation of:
//                 * [ ][X][O]
//                 * [ ][ ][ ]
//                 * [X][ ][ ]
//                 *
//                 * then move to the middle
//                 */
//
//                //TODO: change to check first two locs as x
//                else if(nextBoard.getSquare(new TicTacToeLocation(0, 1)).getPiece() != null
//                        &&
//                        nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece().getOwner().getName().equalsIgnoreCase("X") &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,0)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,0)).getPiece().getOwner().getName().equalsIgnoreCase("X")) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,2)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,2)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,0)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,0)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,0)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,2)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,2)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//
//
//                /* If current game state is any variation of:
//                 * [ ][X][ ]
//                 * [O][ ][ ]
//                 * [X][ ][ ]
//                 *
//                 * then move to the middle
//                 */
//
//                //TODO: change to check first two locs as x
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,0)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,2)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,2)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,0)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,0)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,0)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,2)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,2)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(1,1));
//                }
//
//                /* If current game state is any variation of:
//                 * [ ][X][ ]
//                 * [ ][ ][ ]
//                 * [X][O][ ]
//                 *
//                 * then move to the corner
//                 */
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!= null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,0)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(0,0));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!=null &&
//                        nextBoard.getSquare(new TicTacToeLocation(2,2)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(0,2));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,2)).getPiece()!=null &&
//                          nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(2,2));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,0)).getPiece()!=null &&
//                          nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(2,0));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,0)).getPiece()!=null &&
//                          nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(0,2));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null &&
//                          nextBoard.getSquare(new TicTacToeLocation(2,0)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(2,2));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(0,2)).getPiece()!=null &&
//                          nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(0,0));
//                }
//
//                else if(nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null &&
//                          nextBoard.getSquare(new TicTacToeLocation(2,2)).getPiece()!=null) {
//
//                    secondMoveChoices.add(new TicTacToeLocation(2,0));
//                }


//                if(nextBoard.getSquare(new TicTacToeLocation(1, 1))
//                        .getPiece().getOwner().getName().equalsIgnoreCase("O")) {
                    
//

//                } else if((nextBoard.getSquare(new TicTacToeLocation(0,0)).getPiece()!=null &&
//                           nextBoard.getSquare(new TicTacToeLocation(2,2)).getPiece()!=null) ||
//                          (nextBoard.getSquare(new TicTacToeLocation(0,2)).getPiece()!=null &&
//                           nextBoard.getSquare(new TicTacToeLocation(2,0)).getPiece()!=null)) {
//
//                    Random rand = new Random();
//                    List<TicTacToeLocation> secondMoveChoices = new ArrayList<TicTacToeLocation>();
//                    secondMoveChoices.add(new TicTacToeLocation(0,1));
//                    secondMoveChoices.add(new TicTacToeLocation(1,0));
//                    secondMoveChoices.add(new TicTacToeLocation(1,2));
//                    secondMoveChoices.add(new TicTacToeLocation(2,1));
//
//                    return secondMoveChoices.get(rand.nextInt(secondMoveChoices.size()-1));
//                }
                if(!secondMoveChoices.isEmpty()){
                    return secondMoveChoices.get(rand.nextInt(secondMoveChoices.size()));
                }
            }

        } catch (InvalidLocationException ex) {
            Logger.getLogger(ComputerTicTacToePlayer.class.getName()).log(Level.SEVERE, null, ex);
        }

        try {
//            Random rand = new Random();
            List<TicTacToeLocation> thirdMoveChoices = new ArrayList<TicTacToeLocation>();
            if ((game.getTurnCount() == 5) &&
                (!currentGame.checkForBlockWin(currentGame.getFirstPlayer(), nextBoard))) {

                /* If current game state is any variation of:
                 * [ ][X][ ]
                 * [O][O][X]
                 * [X][ ][ ]
                 *
                 * then move to the corner adjacent to the X's that are
                 * adjacent to eachother
                 */

                // Move to top left
                if(nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null &&
                   nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece().getOwner().getName().equalsIgnoreCase("X") &&
                   nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!=null &&
                   nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece().getOwner().getName().equalsIgnoreCase("X")) {

                    thirdMoveChoices.add(new TicTacToeLocation(0,0));
                }
                // Move to top right
                else if(nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece()!=null &&
                        nextBoard.getSquare(new TicTacToeLocation(0,1)).getPiece().getOwner().getName().equalsIgnoreCase("X") &&
                        nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null &&
                        nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece().getOwner().getName().equalsIgnoreCase("X")) {

                    thirdMoveChoices.add(new TicTacToeLocation(0,2));
                }
                // Move to bottom right
                else if(nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece()!=null &&
                        nextBoard.getSquare(new TicTacToeLocation(1,2)).getPiece().getOwner().getName().equalsIgnoreCase("X") &&
                        nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null &&
                        nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece().getOwner().getName().equalsIgnoreCase("X")) {

                    thirdMoveChoices.add(new TicTacToeLocation(2,2));
                }
                // Move to bottom left
                else if(nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece()!=null &&
                        nextBoard.getSquare(new TicTacToeLocation(1,0)).getPiece().getOwner().getName().equalsIgnoreCase("X") &&
                        nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece()!=null &&
                        nextBoard.getSquare(new TicTacToeLocation(2,1)).getPiece().getOwner().getName().equalsIgnoreCase("X")) {

                    thirdMoveChoices.add(new TicTacToeLocation(2,0));
                }

                if(!thirdMoveChoices.isEmpty()){
                    return thirdMoveChoices.get(0);
                }
            }

        } catch (InvalidLocationException ex) {
            Logger.getLogger(ComputerTicTacToePlayer.class.getName()).log(Level.SEVERE, null, ex);
        }

        while (mayPlay(sucessors)) {
            move = sucessors.get(0);
            sucessors.remove(0);
            nextBoard = (MutableTicTacToeBoard)board.clone();
   
            System.err.println("******************************************************************");

            try {
                nextBoard = game.performMove(move, nextBoard);
                currentGame.switchPlayers();
            } catch (InvalidMoveException ex) {
                Logger.getLogger(ComputerTicTacToePlayer.class.getName()).log(Level.SEVERE, null, ex);
            }
            
            try {
                // If this move would cause human to win next turn do not consider it.
                // Depth is set to maxdepth so that the only other move computer will consider
                // other than a block, is a win.
                if(currentGame.checkForWinner(this, nextBoard)) {
                    value = eval(board, depth);
                } else if(currentGame.checkForBlockWin(currentGame.getFirstPlayer(), nextBoard)) {

//                    else if(currentGame.getCurrentPlayer().getName().equalsIgnoreCase("X")
//                      && currentGame.checkForBlockWin(currentGame.getCurrentPlayer(), nextBoard)) {

                    value = -100;
                    depth = maxDepth;
                    currentGame.switchPlayers();
//                } else {
//                    if (currentGame.checkForBlockWin(currentGame.getCurrentPlayer(), nextBoard)) {
//                        value = 10*((maxDepth+2)-(depth+1));
//                        currentGame.switchPlayers();
                } else {
                        value = minMove(nextBoard, 1);
                        currentGame.switchPlayers();
//                    }
                }
            } catch (InvalidLocationException ex) {
                Logger.getLogger(ComputerTicTacToePlayer.class.getName()).log(Level.SEVERE, null, ex);
            }

            System.err.println("Considered possible move: " + move.toString() +
                    " and found value: " + value);

            if (value > maxValue) {
//                System.err.println("Max value : " + value + " at depth : 0");
                maxValue = value;
                bestMove = move;
            }
        }
        return bestMove;
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
    private int minMove (MutableTicTacToeBoard board, int depth) {
        if (cutOffTest (board, depth))
            return eval(board, depth);

        List<TicTacToeLocation> sucessors;
        TicTacToeLocation move;
        MutableTicTacToeBoard nextBoard;
        int value = 0;
        int maxValue = Integer.MIN_VALUE;

        System.err.println("Min node at depth : " + depth);

        sucessors = currentGame.getPossibleMoves(board);
        
        while (mayPlay (sucessors)) {
            move = sucessors.get(0);
            sucessors.remove(0);
            nextBoard = (MutableTicTacToeBoard)board.clone();

            try {
                nextBoard = currentGame.performMove(move, nextBoard);
            } catch (InvalidMoveException ex) {
                Logger.getLogger(ComputerTicTacToePlayer.class.getName()).log(Level.SEVERE, null, ex);
            }

            try {
                if(currentGame.getCurrentPlayer().getName().equalsIgnoreCase("X") &&
                   currentGame.checkForBlockWin(currentGame.getCurrentPlayer(), nextBoard)) {
                    value = -100;
                    depth = maxDepth;
                    currentGame.switchPlayers();
                } else {
//                    if (currentGame.checkForBlockWin(currentGame.getCurrentPlayer(), nextBoard)) {
//                        value = 10*((maxDepth+2)-(depth+1));
//                    } else {
                    value = maxMove(nextBoard, depth + 1);
                    //}
                }
            } catch (InvalidLocationException ex) {
                Logger.getLogger(ComputerTicTacToePlayer.class.getName()).log(Level.SEVERE, null, ex);
            }

            if(value > maxValue) {
                maxValue = value;
            }
        }

        System.err.println("Min value selected : " + maxValue + " at depth : " + depth);
        //board.printBoard();
        return maxValue;
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
    private int maxMove (MutableTicTacToeBoard board, int depth) {
        if (cutOffTest (board, depth))
            return eval (board, depth);

        List<TicTacToeLocation> sucessors;
        TicTacToeLocation move;
        MutableTicTacToeBoard nextBoard;
        int value = 0;
        int maxValue = Integer.MIN_VALUE;

        System.err.println("Max node at depth : " + depth);

        sucessors = currentGame.getPossibleMoves(board);

        while (mayPlay (sucessors)) {
            move = sucessors.get(0);
            sucessors.remove(0);
            nextBoard = (MutableTicTacToeBoard)board.clone();

            try {
                nextBoard = currentGame.performMove(move, nextBoard);
            } catch (InvalidMoveException ex) {
                Logger.getLogger(ComputerTicTacToePlayer.class.getName()).log(Level.SEVERE, null, ex);
            }

            try {
                if(currentGame.getCurrentPlayer().getName().equalsIgnoreCase("X") &&
                   currentGame.checkForBlockWin(currentGame.getCurrentPlayer(), nextBoard)) {
                    value = -100;
                    depth = maxDepth;
                    currentGame.switchPlayers();
                } else {
//                    if (currentGame.checkForBlockWin(currentGame.getCurrentPlayer(), nextBoard)) {
//                        value = 10*((maxDepth+2)-(depth+1));
//                    } else {
                        value = minMove(nextBoard, depth + 1);
//                    }
                }
            } catch (InvalidLocationException ex) {
                Logger.getLogger(ComputerTicTacToePlayer.class.getName()).log(Level.SEVERE, null, ex);
            }

            if(value > maxValue) {
                maxValue = value;
            }
        }

        System.err.println("Max value selected : " + maxValue + " at depth : " + depth);
        //board.printBoard();
        return maxValue;
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
    private int eval (MutableTicTacToeBoard board, int depth) {
        //System.err.println("THIS SHOULD NEVER BE > than 5: " + depth);
        int offset = 1;
        int colorForce = 0;
        int enemyForce = 0;

        //board.printBoard();

        offset = (maxDepth+2) - depth;

        try {
            //if(currentGame.getCurrentPlayer().getName().equalsIgnoreCase("O")) {
                if(currentGame.checkForWinner(currentGame.getSecondPlayer(), board)) {
                    colorForce = (10 * offset);
                }
            //} else {
                else if(currentGame.checkForWinner(currentGame.getFirstPlayer(), board)) {
                    enemyForce = (10 * offset);
                }
            //}
        } catch (InvalidLocationException ex) {
            Logger.getLogger(ComputerTicTacToePlayer.class.getName()).log(Level.SEVERE, null, ex);
        }
        
        //turns control back to previous player
        //currentGame.switchPlayers();

        return colorForce - enemyForce;
    }

   /**
    * Verifies if the game tree can be pruned.
    *
    * @param board The board to evaluate
    * @param depth Current game tree depth
    * @return true if the tree can be pruned.
    */
  private boolean cutOffTest (MutableTicTacToeBoard board, int depth) {
        try {
            return depth >= maxDepth ||
                    (currentGame.checkForWinner(currentGame.getFirstPlayer(), board)) ||
                    (currentGame.checkForWinner(currentGame.getSecondPlayer(), board));
        } catch (InvalidLocationException ex) {
            Logger.getLogger(ComputerTicTacToePlayer.class.getName()).log(Level.SEVERE, null, ex);
        }

        return true;
  }
}

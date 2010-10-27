package ca.bcit.cst.comp2526.assign3.solution;


/**
 * A Tic Tac Toe Board made up of squares.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public interface TicTacToeBoard
{
    /**
     * Get the square at the specified location.
     *
     * @param location which square to get.
     * @return the square at the specified location.
     */
    TicTacToeSquare getSquare(TicTacToeLocation location);

    /**
     * The number of rows on the board.
     *
     * @return 3.
     */
    int getRows();

    /**
     * The number of columns on the board.
     *
     * @return 3.
     */
    int getCols();
}

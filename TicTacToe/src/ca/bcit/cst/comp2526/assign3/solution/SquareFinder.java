package ca.bcit.cst.comp2526.assign3.solution;

import java.util.List;



/**
 * Find squares of a given type.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public interface SquareFinder
{
    /**
     * Find all the squares of a given type on a Tic Tac Toe Board.
     *
     * @param board the board to check.
     * @return the Locations of the squares that were "accept"ed.
     */
    List<TicTacToeLocation> findSquares(TicTacToeBoard board);

    /**
     * Decide if a square meets the requirements to be found.
     *
     * @param square the square to check.
     * @return true if the square meets the requirements to be accepted.
     */
    boolean accept(TicTacToeSquare square);
}

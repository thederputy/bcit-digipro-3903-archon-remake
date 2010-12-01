package ca.bcit.cst.comp2526.assign3.solution;


/**
 * A Square on a Tic Tac Toe Board.  Each square has a location and an 
 * optional piece.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public interface TicTacToeSquare
{
    /**
     * Get the location of the square on the board.
     *
     * @return the location of the square.
     */
    TicTacToeLocation getLocation();

    /**
     * Get the piece that is on the square.
     *
     * @return null if there is no piece.
     */
    TicTacToePiece getPiece();
}

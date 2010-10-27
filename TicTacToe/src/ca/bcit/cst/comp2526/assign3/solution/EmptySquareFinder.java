package ca.bcit.cst.comp2526.assign3.solution;


/**
 * Find the squares that do not contain a piece.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public class EmptySquareFinder
    extends AbstractSquareFinder
{
    /**
     * See if the square is empty or not.
     *
     * @param square the square to check.
     * @return true of the square.getPiece is null.
     */
    public boolean accept(final TicTacToeSquare square)
    {
        final TicTacToePiece piece;

        if(square == null)
        {
            throw new IllegalArgumentException("square cannot be null");
        }
        
        piece = square.getPiece();

        return (piece == null);
    }
}

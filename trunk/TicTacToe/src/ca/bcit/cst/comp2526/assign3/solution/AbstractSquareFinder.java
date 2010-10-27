package ca.bcit.cst.comp2526.assign3.solution;

import java.util.ArrayList;
import java.util.List;



/**
 * Find the squares that match the requirements of the accpt method.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public abstract class AbstractSquareFinder
    implements SquareFinder
{
    /**
     * Check each square to see if it is "accept"ed or not.  If the accept method
     * returns true then the location of the square is added to the array that
     * is returned.
     * 
     * The squares are checked in the following order: left to right, top to bottom.
     *
     * @param board the board to check the squares of.
     * @return a (possibly empty) XList of the locations of the squares where accept returned true.
     */
    public final List<TicTacToeLocation> findSquares(final TicTacToeBoard board)
    {
        final List<TicTacToeLocation> found;

        if(board == null)
        {
            throw new IllegalArgumentException("board cannot be null");
        }

        found  = new ArrayList<TicTacToeLocation>();

        for(int row = 0; row < board.getRows(); row++)
        {
            for(int col = 0; col < board.getCols(); col++)
            {
                final TicTacToeSquare   square;
                final TicTacToeLocation location;

                try
                {
                    location = new TicTacToeLocation(row, col);
                }
                catch(final InvalidLocationException ex)
                {
                    throw new IllegalStateException("this can never happen");
                }

                square = board.getSquare(location);

                if(accept(square))
                {
                    found.add(square.getLocation());
                }
            }
        }

        return (found);
    }
}

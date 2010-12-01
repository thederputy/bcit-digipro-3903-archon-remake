package ca.bcit.cst.comp2526.assign3.solution;


import java.io.Serializable;


/**
 * A tic tac toe location class that supports rows and colums (x and y).
 * The row and column must be >= 0.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public class TicTacToeLocation
    implements Serializable
{
    /**
     * Serialization UID.
     */
    private static final long serialVersionUID = 1;
    
    /**
     * The x location.
     */
    private final int row;
    
    /**
     * The y location.
     */
    private final int col;

    /**
     * Constructs a Location with the specified row and column.
     *
     * @param r the row,
     * @param c the column.
     * @throws InvalidLocationException if either argument is < 0 or > 2.
     */
    public TicTacToeLocation(final int r,
                             final int c)
        throws InvalidLocationException
    {
        if(r < 0 || r > 2)
        {
            throw new InvalidLocationException("r", 0, 2, r);
        }

        if(c < 0 || c > 2)
        {
            throw new InvalidLocationException("c", 0, 2, c);
        }

        row = r;
        col = c;
    }


    /**
     * Get the row.
     *
     * @return the row.
     */
    public int getRow()
    {
        return (row);
    }

    /**
     * Get the column.
     *
     * @return the column.
     */
    public int getCol()
    {
        return (col);
    }

    @Override
    public boolean equals(final Object o)
    {
        final boolean retVal;

        if(o instanceof TicTacToeLocation)
        {
            final TicTacToeLocation other;

            other = (TicTacToeLocation)o;

            if(row == other.row)
            {
                retVal = col == other.col;
            }
            else
            {
                retVal = false;
            }
        }
        else
        {
            retVal = false;
        }

        return (retVal);
    }

    @Override
    public int hashCode()
    {
        int hash;

        hash = (row * 3) + col;

        return (hash);
    }

    /**
     * Generate a String representation of the location.
     *
     * @return (row, col)
     */
    @Override
    public String toString()
    {
        return ("(" + row + ", " + col + ")");
    }
}

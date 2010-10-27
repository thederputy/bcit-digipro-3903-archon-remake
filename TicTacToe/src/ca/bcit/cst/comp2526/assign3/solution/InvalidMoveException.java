package ca.bcit.cst.comp2526.assign3.solution;


/**
 * Thrown when a move is made to a square that is already occupied.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public class InvalidMoveException
    extends Exception
{
    /**
     * Serialization UID.
     */
    private static final long serialVersionUID = 1;

    /**
     * The location that was attempted to move to.
     */
    private final TicTacToeLocation location;

    /**
     * Constructs an InvalidMoveException with the specified location.
     *
     * @param loc the location that is already occupied on the board.
     */
    public InvalidMoveException(final TicTacToeLocation loc)
    {
        super("cannot move to: " + ((loc == null) ? "null" : (loc.getRow() + ", " + loc.getCol())));

        location = loc;
    }

    /**
     * Get the offending location.
     *
     * @return the offending location.
     */
    public TicTacToeLocation getLocation()
    {
        return (location);
    }
}

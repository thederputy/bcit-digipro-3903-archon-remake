package ca.bcit.cst.comp2526.assign3.solution;


/**
 * Find the squares that have a piece belonging to a specific player.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public class PlayerSquareFinder
    extends AbstractSquareFinder
{
    /**
     * The player to check for piece ownership.
     */
    private final TicTacToePlayer player;
    
    /**
     * Constructs a PlayerSquareFinder with the specified player.
     *
     * @param p the player used to accpet the square or not.
     * @throws IllegalArgumentException if p is null.
     */
    public PlayerSquareFinder(final TicTacToePlayer p)
    {
        if(p == null)
        {
            throw new IllegalArgumentException("p cannot be null");
        }
        
        player = p;
    }
    
    /**
     * See if the square has a piece belonging to a specific player.
     *
     * @param square the square to check.
     * @return true of the square.getPiece is not null and that the piece belongs the the specified player.
     */
    public boolean accept(final TicTacToeSquare square)
    {
        final TicTacToePiece  piece;
        final boolean         retVal;
                
        if(square == null)
        {
            throw new IllegalArgumentException("square cannot be null");
        }
        
        piece = square.getPiece();

        if(piece == null)
        {
            retVal = false;
        }
        else
        {
            final TicTacToePlayer owner;

            owner  = piece.getOwner();
            retVal = owner.equals(player);
        }
        
        return (retVal);
    }

    /**
     * Get the player used in the search.
     *
     * @return the player used in the search.
     */
    public TicTacToePlayer getPlayer()
    {
        return (player);
    }
}

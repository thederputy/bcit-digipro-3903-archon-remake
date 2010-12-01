package ca.bcit.cst.comp2526.assign3.solution;


/**
 * A Tic Tac Tow piece.  Each piece is owned by a player.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public class TicTacToePiece
{
    /**
     * The owner of the piece.
     */
    private final TicTacToePlayer owner;

    /**
     * Constructs a TicTacToePiece with the specified owner.
     *
     * @param player the owner of the piece.
     * @throws IllegalArgumentException if the player is null.
     */
    public TicTacToePiece(final TicTacToePlayer player)
    {
        if(player == null)
        {
            throw new IllegalArgumentException("player cannot be null");
        }
        
        owner = player;
    }

    /**
     * The player that owns the piece.
     *
     * @return the owner of the piece.
     */
    public TicTacToePlayer getOwner()
    {
        return (owner);
    }
}

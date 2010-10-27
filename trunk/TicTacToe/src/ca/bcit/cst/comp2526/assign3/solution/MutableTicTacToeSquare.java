package ca.bcit.cst.comp2526.assign3.solution;


/**
 * A Square on a Tic Tac Toe Board.  Each square has a location and an 
 * optional piece.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
final class MutableTicTacToeSquare
    implements TicTacToeSquare
{
    /**
     * The location of this square on the board.
     */
    private final TicTacToeLocation location;
    
    /**
     * The (possibly null) piece on the square.
     */
    private TicTacToePiece piece = null;

    /**
     * Construct a MutableTicTacToeSquare with the specified location.
     *
     * @param loc the location of the square on the board.
     * @throws IllegalArgumentException if loc is null.
     */
    MutableTicTacToeSquare(final TicTacToeLocation loc)
    {
        if(loc == null)
        {
            throw new IllegalArgumentException("loc cannot be null");
        }

        location = loc;
    }

    /**
     * Put the specified piece on the square.
     *
     * @param p the piece to put onto the square.
     * @throws IllegalArgumentException if p is null
     */
    public void setPiece(final TicTacToePiece p)
    {
//        if(p == null)
//        {
//            throw new IllegalArgumentException("p cannot be null");
//        }
        if(p==null) {
            piece = null;
        } else {
            piece = p;
        }
       
    }

    /**
     * Remove any pieces from the square
     */
    public void removePiece()
    {
       piece = null;
    }

    /**
     * Get the piece that is on the square.
     *
     * @return null if there is no piece.
     */
    public TicTacToePiece getPiece()
    {
        return (piece);
    }

    /**
     * Get the location of the square on the board.
     *
     * @return the location of the square.
     */
    public TicTacToeLocation getLocation()
    {
        return (location);
    }

    @Override
    public String toString() {
      return "Value of piece on square at " + location.toString() + ": " + piece;
    }
}

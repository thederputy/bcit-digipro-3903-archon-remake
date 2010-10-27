package ca.bcit.cst.comp2526.assign3.solution;



/**
 * A Tic Tac Toe board.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
final class MutableTicTacToeBoard
    implements TicTacToeBoard
{
    /**
     * The number of rows on teh board.
     */
    private static final int NUMBER_OF_ROWS = 3;

    /**
     * The number of columns on the board.
     */
    private static final int NUMBER_OF_COLS = 3;
    
    /**
     * The sqares on the board.
     */
    private final MutableTicTacToeSquare[][] squares;

    /**
     * Constructs a MutableTicTacToeBoard by creating a 3x3 group of TicTacToeSquares.
     */
    MutableTicTacToeBoard()
    {
        squares = new MutableTicTacToeSquare[NUMBER_OF_ROWS][NUMBER_OF_COLS];
        
        for(int row = 0; row < squares.length; row++)
        {
            for(int col = 0; col < squares[row].length; col++)
            {
                final TicTacToeLocation               location;
                final MutableTicTacToeSquare square;

                try
                {
                    location = new TicTacToeLocation(row, col);
                }
                catch(final InvalidLocationException ex)
                {
                    throw new IllegalStateException("this can never happen");
                }

                square            = new MutableTicTacToeSquare(location);
                squares[row][col] = square;
            }
        }
    }

    /**
     * Place a piace on the board at the specified location. The square at the
     * specified location may not have a piece on it already.
     * 
     * @param piece the piece to put on the board.
     * @param location the location of the square that the piece is to be placed on.
     * @throws InvalidMoveException if the square already has a piece on it.
     * @throws IllegalArgumentException if the location is null.
     */
    public void putPiece(final TicTacToePiece piece,
                         final TicTacToeLocation       location)
        throws InvalidMoveException
    {
        final MutableTicTacToeSquare square;

        if(piece == null)
        {
            throw new IllegalArgumentException("piece cannot be null");
        }

        if(location == null)
        {
            throw new IllegalArgumentException("location cannot be null");
        }
        
        square = squares[location.getRow()][location.getCol()];
        
        if(square.getPiece() != null)
        {
            throw new InvalidMoveException(location);
        }
        
        square.setPiece(piece);
    }

    public void printBoard() {
        for(int h = 0; h < (NUMBER_OF_ROWS); h++) {
            for(int w = 0; w < (NUMBER_OF_COLS); w++) {
                TicTacToePiece piece = squares[h][w].getPiece();
                if(piece != null) {
                    if(piece.getOwner().getName().equals("X"))
                        System.out.print("[X]");
                    else
                        System.out.print("[O]");
                }else {
                    System.out.print("[ ]");
                }
                
            }
            System.out.println();
        }
    }

    /**
     * Place a piace on the board at the specified location. The square at the
     * specified location may not have a piece on it already.
     *
     * @param piece the piece to put on the board.
     * @param location the location of the square that the piece is to be placed on.
     * @throws InvalidMoveException if the square already has a piece on it.
     * @throws IllegalArgumentException if the location is null.
     */
    public void removePiece(final TicTacToeLocation location)
    {
        final MutableTicTacToeSquare square;

        if(location == null)
        {
            throw new IllegalArgumentException("location cannot be null");
        }

        square = squares[location.getRow()][location.getCol()];
        
        square.removePiece();
    }

    /**
     * Get the square at the specified location.
     * 
     * @param location the location of the square to get.
     * @return the square at the specified location.
     * @throws IllegalArgumentException if the location is null.
     */
    public MutableTicTacToeSquare getSquare(final TicTacToeLocation location)
    {
        final MutableTicTacToeSquare square;
        final int             row;
        final int             col;

        if(location == null)
        {
            throw new IllegalArgumentException("location cannot be null");
        }
        
        row    = location.getRow();
        col    = location.getCol();
        square = squares[row][col];

        return (square);
    }

    /**
     * Get the number of rows on the board.
     * 
     * @return 3
     */
    public int getRows()
    {
        return (squares.length);
    }

    /**
     * Get the number of cols on the board.
     * 
     * @return 3
     */
    public int getCols()
    {
        return (squares[0].length);
    }

    /**
     * Get a String representation of the board.
     *
     * @return a String representaiton of the board.
     */
    @Override
    public String toString()
    {
        final StringBuilder builder;

        builder = new StringBuilder();

        for(int row = 0; row < squares.length; row++)
        {
            for(int col = 0; col < squares[row].length; col++)
            {
                final TicTacToeSquare square;
                final TicTacToePiece  piece;

                square = squares[row][col];
                piece  = square.getPiece();

                if(piece == null)
                {
                    builder.append(' ');
                }
                else
                {
                    final TicTacToePlayer owner;

                    owner = piece.getOwner();
                    builder.append(owner.getName());
                }
            }

            builder.append(System.getProperty("line.separator"));
        }

        return (builder.toString());
    }

    @Override
    public Object clone() {
        //MutableTicTacToeSquare[][] squaresClone = new MutableTicTacToeSquare[NUMBER_OF_ROWS][NUMBER_OF_COLS];
        MutableTicTacToeBoard board = new MutableTicTacToeBoard();

        for(int row = 0; row < squares.length; row++)
        {
            for(int col = 0; col < squares[row].length; col++)
            {
                final TicTacToeLocation location;
                final MutableTicTacToeSquare square;

                try
                {
                    location = new TicTacToeLocation(row, col);
                }
                catch(final InvalidLocationException ex)
                {
                    throw new IllegalStateException("this can never happen");
                }

//                System.out.println("Value of board: ");
//                board.printBoard();
                board.getSquare(location).setPiece(squares[row][col].getPiece());
//                board.getSquare(location).toString();
            }
        }

        return board;
    }
}

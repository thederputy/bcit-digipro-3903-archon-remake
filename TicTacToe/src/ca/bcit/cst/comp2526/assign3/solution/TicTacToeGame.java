package ca.bcit.cst.comp2526.assign3.solution;

import ca.bcit.cst.comp2526.assign3.solution.gui.TicTacToeFrame;
import java.util.ArrayList;
import java.util.List;



/**
 * A Tic Tac Toe game played with two players on a 3x3 board.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public class TicTacToeGame
{    
    /**
     * The player who is playing X.
     */
    private TicTacToePlayer firstPlayer;

    /**
     * The player who is playing O.
     */
    private TicTacToePlayer secondPlayer;

    /**
     * The board.
     */
    private final MutableTicTacToeBoard board;

    /**
     * The player who is currently moving.  null if the game is over.
     */
    private TicTacToePlayer currentPlayer;

    /**
     * The player who will go after this turn is completed.  null if the game is over.
     */
    private TicTacToePlayer nextPlayer;

    /**
     * The player that won the game.  null if there is no winner yet or if the game is tied.
     */
    private TicTacToePlayer winner;

    /**
     * Find the squares without pieces.
     */
    private final SquareFinder emptySquareFinder;

    /**
     * Find the squares that have the X players pieces on them.
     */
    private SquareFinder xSquareFinder;

    /**
     * Find the squares that have the O players pieces on them.
     */
    private SquareFinder oSquareFinder;

    private int turnCount;
    
    /**
     * Constructs a TicTacToeGame with the specified players.
     * 
     * @param first  the player to go first.
     * @param second the player to go second.
     * @throws IllegalArgumentException if either player is null or if both players are the same.
     */
    public TicTacToeGame()
    {
        board             = new MutableTicTacToeBoard();
        emptySquareFinder = new EmptySquareFinder();
        turnCount = 0;
    }

    public TicTacToeGame(TicTacToeGame game) {
        board = game.board;
        emptySquareFinder = game.emptySquareFinder;
        firstPlayer = game.firstPlayer;
        secondPlayer = game.secondPlayer;
        currentPlayer = game.currentPlayer;
        nextPlayer = game.nextPlayer;
        winner = game.winner;
        xSquareFinder = game.xSquareFinder;
        oSquareFinder = game.oSquareFinder;
        turnCount = game.turnCount;
    }

    /**
     * @return the turnCount
     */
    public int getTurnCount() {
        return turnCount;
    }

    /**
     * @param turnCount the turnCount to set
     */
    public void setTurnCount(int turnCount) {
        this.turnCount = turnCount;
    }
    
    /**
     * Get the player who is going first.
     * 
     * @return the player that is going first.
     */
    public TicTacToePlayer getFirstPlayer()
    {
        return (firstPlayer);
    }
    
    /**
     * Get the player who is going second.
     * 
     * @return the plaer that is going second.
     */
    public TicTacToePlayer getSecondPlayer()
    {
        return (secondPlayer);
    }

    /**
     * Get the game board.
     * 
     * @return the game board.
     */
    public TicTacToeBoard getBoard()
    {
        return (board);
    }
    
    /**
     * Get the player who is currently moving.
     * 
     * @return the player that is currently moving or null of the game is over.
     */
    public TicTacToePlayer getCurrentPlayer()
    {
        return (currentPlayer);
    }
    
    /**
     * Get the player who will go after the current player has moved.
     * 
     * @return the player who will go after the current player has moved or null of the game is over.
     */
    public TicTacToePlayer getNextPlayer()
    {
        return (nextPlayer);
    }

    /**
     * Get the player that won.
     * 
     * @return the player that won or null if the game is tied or not over yet.
     */
    public TicTacToePlayer getWinner()
    {
        return (winner);
    }

    public void play(final TicTacToePlayer first,
                     final TicTacToePlayer second,
                     final TicTacToeFrame  frame)
    {
        if(first.equals(second))
        {
            throw new IllegalArgumentException("first and second cannot be the same player");
        }

        if(first == null)
        {
            throw new IllegalArgumentException("first cannot be null");
        }

        if(second == null)
        {
            throw new IllegalArgumentException("second cannot be null");
        }

        firstPlayer       = first;
        secondPlayer      = second;
        currentPlayer     = firstPlayer;
        nextPlayer        = secondPlayer;
        xSquareFinder     = new PlayerSquareFinder(first);
        oSquareFinder     = new PlayerSquareFinder(second);

        while(winner == null)
        {
            final TicTacToeLocation       location;
            final List<TicTacToeLocation> possibleMoves;

            //print board

            location = currentPlayer.getMove(this);
            turnCount++;

            if(isValidMove(location))
            {
                try
                {
                    move(location);                    
                    possibleMoves = getPossibleMoves();
                    frame.repaint();

                    if(possibleMoves.isEmpty())
                    {
                        frame.gameOver();
                        break;
                    }
                }
                catch(final InvalidMoveException ex)
                {
                    ex.printStackTrace();
                }
            }
        }
    }

    private void printBoard() {
        board.printBoard();
    }

    /**
     * Get the empty squares on the board.  A square is empty if it does not have a piece on it.
     * 
     * @return the empty squares on the board.
     */
    public List<TicTacToeLocation> getPossibleMoves()
    {
        final List<TicTacToeLocation> locations;

        if(winner == null)
        {
            locations = emptySquareFinder.findSquares(board);
        }
        else
        {
            locations = new ArrayList<TicTacToeLocation>();
        }

        return (locations);
    }

    /**
     * Get the empty squares on the board.  A square is empty if it does not have a piece on it.
     * 
     * @return the empty squares on the board.
     */
    public List<TicTacToeLocation> getPossibleMoves(MutableTicTacToeBoard compBoard)
    {
        final List<TicTacToeLocation> locations;

        if(winner == null)
        {
            locations = emptySquareFinder.findSquares(compBoard);
        }
        else
        {
            locations = new ArrayList<TicTacToeLocation>();
        }

        return (locations);
    }
    
    /**
     * Check to see if a move is valid or not.
     * 
     * @param location the location of the square to check.
     * @return true if the square has no piece on it, false otherwise.
     * @throws IllegalArgumentException if location is null or off the board.
     */
    public boolean isValidMove(final TicTacToeLocation location)
    {
        final boolean retVal;
        
        if(location == null)
        {
            throw new IllegalArgumentException("location cannot be null");
        }
        
        if(getPossibleMoves().isEmpty())
        {
            retVal = false;
        }
        else
        {
            final TicTacToeSquare square;
            final TicTacToePiece  piece;

            square = board.getSquare(location);        
            piece  = square.getPiece();
            retVal = (piece == null);
        }
        
        return (retVal);
    }
    
    /**
     * The current player moves a piece to the specified square.
     * If the move is successful (isValidMove) then the current and next players
     * are swapped so that the next player gets a turn to move.
     * 
     * @param location the location of the square to move to.
     * @throws InvalidMoveException if the square is already occupied.
     * @throws IllegalArgumentException if location is null or off the board.
     */
    public void move(final TicTacToeLocation location)
        throws InvalidMoveException
    {
        if(location == null)
        {
            throw new IllegalArgumentException("location cannot be null");
        }
        
        // throws invalid move exception if it isn't valid
        performMove(location);

        try
        {
            if(checkForWinner(currentPlayer)) {
                winner = currentPlayer;
            }
        }
        catch(final InvalidLocationException ex)
        {
            throw new IllegalStateException("this can never happen");
        }
        
        if(winner != null || getPossibleMoves().isEmpty())
        {
            currentPlayer = null;
            nextPlayer    = null;
        }
        else
        {        
            final TicTacToePlayer temp;

            temp          = currentPlayer;
            currentPlayer = nextPlayer;
            nextPlayer    = temp;
        }
    }
    
    /**
     * Put a piece belonging to the currenntPlayer onto the board at the specified location.
     * 
     * @param location the location of the square to move to.
     * @throws InvalidMoveException if the square is already occupied.
     * @throws IllegalArgumentException if location is null or off the board.
     */
    public MutableTicTacToeBoard performMove(final TicTacToeLocation location,
                            MutableTicTacToeBoard compBoard)
                            throws InvalidMoveException
    {        
        final TicTacToePiece piece;

        if(location == null)
        {
            throw new IllegalArgumentException("location cannot be null");
        }
        
        piece = new TicTacToePiece(currentPlayer);
        compBoard.putPiece(piece, location);

        setTurnCount(getTurnCount() + 1);

        return compBoard;
    }

    public void switchPlayers() {
        final TicTacToePlayer temp;

        temp          = currentPlayer;
        currentPlayer = nextPlayer;
        nextPlayer    = temp;
    }

    /**
     * Performs a move by the specified player
     * @param location
     * @param compBoard
     * @param player
     * @return
     * @throws InvalidMoveException
     */
    public MutableTicTacToeBoard performMove(final TicTacToeLocation location,
                            MutableTicTacToeBoard compBoard, TicTacToePlayer player)
                            throws InvalidMoveException
    {
        final TicTacToePiece piece;

        if(location == null)
        {
            throw new IllegalArgumentException("location cannot be null");
        }

        piece = new TicTacToePiece(player);
        compBoard.putPiece(piece, location);
        return compBoard;
    }

    /**
     * Put a piece belonging to the currenntPlayer onto the board at the specified location.
     *
     * @param location the location of the square to move to.
     * @throws InvalidMoveException if the square is already occupied.
     * @throws IllegalArgumentException if location is null or off the board.
     */
    public void performMove(final TicTacToeLocation location)
        throws InvalidMoveException
    {
        final TicTacToePiece piece;

        if(location == null)
        {
            throw new IllegalArgumentException("location cannot be null");
        }

        piece = new TicTacToePiece(currentPlayer);
        board.putPiece(piece, location);
    }

    /**
     * Undo move on board at the specified location.
     *
     * @param location the location of the square to reset.
     * @throws InvalidMoveException if the square is already occupied.
     * @throws IllegalArgumentException if location is null or off the board.
     */
    public void undoMove(final TicTacToeLocation location)
    {
        if(location == null)
        {
            throw new IllegalArgumentException("location cannot be null");
        }

        board.removePiece(location);
    }
    
    /**
     * Check to see if the game has been won.  The game is won when a player gets
     * three pieces in a row, col, or diagonally.
     * 
     * @param player the player to check for winning.
     * @throws InvalidLocationException if the Location is invalid.
     */
    public boolean checkForWinner(final TicTacToePlayer player)
        throws InvalidLocationException
    {
        final SquareFinder             finder;
        final List<TicTacToeLocation> locations;

        if(currentPlayer.equals(firstPlayer))
        {
            finder = xSquareFinder;
        }
        else
        {
            finder = oSquareFinder;
        }

        locations = finder.findSquares(board);

//        checkHorizontal(locations, player);
//
//        if(winner == null)
//        {
//            checkVertical(locations, player);
//
//            if(winner == null)
//            {
//                checkDiagonal(locations, player);
//            }
//        }

        if(checkHorizontal(locations, player)) {
            return true;
        }else if(checkVertical(locations, player)) {
            return true;
        }else if(checkDiagonal(locations, player)) {
            return true;
        }else {
            return false;
        }
    }

    /**
     * Check to see if the game has been won.  The game is won when a player gets
     * three pieces in a row, col, or diagonally.
     *
     * @param player the player to check for winning.
     * @throws InvalidLocationException if the Location is invalid.
     */
    public boolean checkForWinner(final TicTacToePlayer player, MutableTicTacToeBoard compBoard)
        throws InvalidLocationException
    {
        final SquareFinder             finder;
        final List<TicTacToeLocation> locations;

        if(player.equals(firstPlayer))
        {
            finder = xSquareFinder;
        }
        else
        {
            finder = oSquareFinder;
        }

        locations = finder.findSquares(compBoard);

//        checkHorizontal(locations, player);
//
//        if(winner == null)
//        {
//            checkVertical(locations, player);
//
//            if(winner == null)
//            {
//                checkDiagonal(locations, player);
//            }
//        }

        if(checkHorizontal(locations, player)) {
            return true;
        }else if(checkVertical(locations, player)) {
            return true;
        }else if(checkDiagonal(locations, player)) {
            return true;
        }else {
            return false;
        }
    }

    /**
     * Check to see if the game can be won next turn
     *
     * @param player the player to check for winning.
     * @throws InvalidLocationException if the Location is invalid.
     */
    public boolean checkForBlockWin(final TicTacToePlayer player, MutableTicTacToeBoard compBoard)
        throws InvalidLocationException
    {
        final SquareFinder             finder;
        final List<TicTacToeLocation> locations;
        final List<TicTacToeLocation> emptyLocations;

        if(player.equals(firstPlayer))
        {
            finder = xSquareFinder;
        }
        else
        {
            finder = oSquareFinder;
        }

        locations = finder.findSquares(compBoard);
        emptyLocations = emptySquareFinder.findSquares(compBoard);

        if(checkHorizontal(locations, emptyLocations)) {
            return true;
        }else if(checkVertical(locations, emptyLocations)) {
            return true;
        }else if(checkDiagonal(locations, emptyLocations)) {
            return true;
        }else {
            return false;
        }
    }
    
    /**
     * Chrck to see if the player has a full row of pieces.
     * 
     * @param locations the locations belonging to the player.
     * @param player the player to declare the winner if there is a full row of pieces.
     * @throws InvalidLocationException if the Location is invalid.
     */
    private boolean checkHorizontal(final List<TicTacToeLocation> locations,
                                 final TicTacToePlayer          player)
        throws InvalidLocationException
    {
        if(checkFor(new TicTacToeLocation(0, 0), locations) &&
            checkFor(new TicTacToeLocation(0, 1), locations) &&
            checkFor(new TicTacToeLocation(0, 2), locations))
        {
            return true;
            //winner = player;
        }
        else if(checkFor(new TicTacToeLocation(1, 0), locations) &&
                checkFor(new TicTacToeLocation(1, 1), locations) &&
                checkFor(new TicTacToeLocation(1, 2), locations))
        {
            return true;
            //winner = player;
        }
        else if(checkFor(new TicTacToeLocation(2, 0), locations) &&
                checkFor(new TicTacToeLocation(2, 1), locations) &&
                checkFor(new TicTacToeLocation(2, 2), locations))
        {
            return true;
            //winner = player;
        }

        return false;
    }
    
    /**
     * Check to see if the player has a full column of pieces.
     * 
     * @param locations the locations belonging to the player.
     * @param player the player to declare the winner if there is a full column of pieces.
     * @throws InvalidLocationException if the Location is invalid.
     */
    private boolean checkVertical(final List<TicTacToeLocation> locations,
                               final TicTacToePlayer          player)
        throws InvalidLocationException
    {
        if(checkFor(new TicTacToeLocation(0, 0), locations) &&
            checkFor(new TicTacToeLocation(1, 0), locations) &&
            checkFor(new TicTacToeLocation(2, 0), locations))
        {
            return true;
            //winner = player;
        }
        else if(checkFor(new TicTacToeLocation(0, 1), locations) &&
                checkFor(new TicTacToeLocation(1, 1), locations) &&
                checkFor(new TicTacToeLocation(2, 1), locations))
        {
            return true;
            //winner = player;
        }
        else if(checkFor(new TicTacToeLocation(0, 2), locations) &&
                checkFor(new TicTacToeLocation(1, 2), locations) &&
                checkFor(new TicTacToeLocation(2, 2), locations))
        {
            return true;
            //winner = player;
        }

        return false;
    }
    
    /**
     * Check to see if the player has a full diagonal of pieces.
     * 
     * @param locations the locations belonging to the player.
     * @param player the player to declare the winner if there is a full diagonal of pieces.
     * @throws InvalidLocationException if the Location is invalid.
     */
    private boolean checkDiagonal(final List<TicTacToeLocation> locations,
                               final TicTacToePlayer          player)
        throws InvalidLocationException
    {
        if(checkFor(new TicTacToeLocation(0, 0), locations) &&
            checkFor(new TicTacToeLocation(1, 1), locations) &&
            checkFor(new TicTacToeLocation(2, 2), locations))
        {
            return true;
            //winner = player;
        }
        else if(checkFor(new TicTacToeLocation(0, 2), locations) &&
                checkFor(new TicTacToeLocation(1, 1), locations) &&
                checkFor(new TicTacToeLocation(2, 0), locations))
        {
            return true;
            //winner = player;
        }

        return false;
    }

    /**
     * Chrck to see if the player has a full row of pieces.
     *
     * @param locations the locations belonging to the player.
     * @param player the player to declare the winner if there is a full row of pieces.
     * @throws InvalidLocationException if the Location is invalid.
     */
    private boolean checkHorizontal(final List<TicTacToeLocation> locations,
                                    final List<TicTacToeLocation> emptyLocations)
        throws InvalidLocationException
    {
        if( checkFor(new TicTacToeLocation(0, 0), emptyLocations) &&
            checkFor(new TicTacToeLocation(0, 1), locations) &&
            checkFor(new TicTacToeLocation(0, 2), locations) ||
            checkFor(new TicTacToeLocation(0, 0), locations) &&
            checkFor(new TicTacToeLocation(0, 1), emptyLocations) &&
            checkFor(new TicTacToeLocation(0, 2), locations) ||
            checkFor(new TicTacToeLocation(0, 0), locations) &&
            checkFor(new TicTacToeLocation(0, 1), locations) &&
            checkFor(new TicTacToeLocation(0, 2), emptyLocations))
        {
            return true;
            //winner = player;
        }
        else if(checkFor(new TicTacToeLocation(1, 0), emptyLocations) &&
                checkFor(new TicTacToeLocation(1, 1), locations) &&
                checkFor(new TicTacToeLocation(1, 2), locations) ||
                checkFor(new TicTacToeLocation(1, 0), locations) &&
                checkFor(new TicTacToeLocation(1, 1), emptyLocations) &&
                checkFor(new TicTacToeLocation(1, 2), locations) ||
                checkFor(new TicTacToeLocation(1, 0), locations) &&
                checkFor(new TicTacToeLocation(1, 1), locations) &&
                checkFor(new TicTacToeLocation(1, 2), emptyLocations))
        {
            return true;
            //winner = player;
        }
        else if(checkFor(new TicTacToeLocation(2, 0), emptyLocations) &&
                checkFor(new TicTacToeLocation(2, 1), locations) &&
                checkFor(new TicTacToeLocation(2, 2), locations) ||
                checkFor(new TicTacToeLocation(2, 0), locations) &&
                checkFor(new TicTacToeLocation(2, 1), emptyLocations) &&
                checkFor(new TicTacToeLocation(2, 2), locations) ||
                checkFor(new TicTacToeLocation(2, 0), locations) &&
                checkFor(new TicTacToeLocation(2, 1), locations) &&
                checkFor(new TicTacToeLocation(2, 2), emptyLocations))
        {
            return true;
            //winner = player;
        }

        return false;
    }

    /**
     * Check to see if the player has a full column of pieces.
     *
     * @param locations the locations belonging to the player.
     * @param player the player to declare the winner if there is a full column of pieces.
     * @throws InvalidLocationException if the Location is invalid.
     */
    private boolean checkVertical(final List<TicTacToeLocation> locations,
                                  final List<TicTacToeLocation> emptyLocations)
        throws InvalidLocationException
    {
        if( checkFor(new TicTacToeLocation(0, 0), emptyLocations) &&
            checkFor(new TicTacToeLocation(1, 0), locations) &&
            checkFor(new TicTacToeLocation(2, 0), locations) ||
            checkFor(new TicTacToeLocation(0, 0), locations) &&
            checkFor(new TicTacToeLocation(1, 0), emptyLocations) &&
            checkFor(new TicTacToeLocation(2, 0), locations) ||
            checkFor(new TicTacToeLocation(0, 0), locations) &&
            checkFor(new TicTacToeLocation(1, 0), locations) &&
            checkFor(new TicTacToeLocation(2, 0), emptyLocations))
        {
            return true;
            //winner = player;
        }
        else if(checkFor(new TicTacToeLocation(0, 1), emptyLocations) &&
                checkFor(new TicTacToeLocation(1, 1), locations) &&
                checkFor(new TicTacToeLocation(2, 1), locations) ||
                checkFor(new TicTacToeLocation(0, 1), locations) &&
                checkFor(new TicTacToeLocation(1, 1), emptyLocations) &&
                checkFor(new TicTacToeLocation(2, 1), locations) ||
                checkFor(new TicTacToeLocation(0, 1), locations) &&
                checkFor(new TicTacToeLocation(1, 1), locations) &&
                checkFor(new TicTacToeLocation(2, 1), emptyLocations))
        {
            return true;
            //winner = player;
        }
        else if(checkFor(new TicTacToeLocation(0, 2), emptyLocations) &&
                checkFor(new TicTacToeLocation(1, 2), locations) &&
                checkFor(new TicTacToeLocation(2, 2), locations) ||
                checkFor(new TicTacToeLocation(0, 2), locations) &&
                checkFor(new TicTacToeLocation(1, 2), emptyLocations) &&
                checkFor(new TicTacToeLocation(2, 2), locations) ||
                checkFor(new TicTacToeLocation(0, 2), locations) &&
                checkFor(new TicTacToeLocation(1, 2), locations) &&
                checkFor(new TicTacToeLocation(2, 2), emptyLocations))
        {
            return true;
            //winner = player;
        }

        return false;
    }

    /**
     * Check to see if the player has a full diagonal of pieces.
     *
     * @param locations the locations belonging to the player.
     * @param player the player to declare the winner if there is a full diagonal of pieces.
     * @throws InvalidLocationException if the Location is invalid.
     */
    private boolean checkDiagonal(final List<TicTacToeLocation> locations,
                                  final List<TicTacToeLocation> emptyLocations)
        throws InvalidLocationException
    {
        if( checkFor(new TicTacToeLocation(0, 0), emptyLocations) &&
            checkFor(new TicTacToeLocation(1, 1), locations)      &&
            checkFor(new TicTacToeLocation(2, 2), locations)      ||
            checkFor(new TicTacToeLocation(0, 0), locations)      &&
            checkFor(new TicTacToeLocation(1, 1), emptyLocations) &&
            checkFor(new TicTacToeLocation(2, 2), locations)      ||
            checkFor(new TicTacToeLocation(0, 0), locations)      &&
            checkFor(new TicTacToeLocation(1, 1), locations)      &&
            checkFor(new TicTacToeLocation(2, 2), emptyLocations))
        {
            return true;
            //winner = player;
        }
        else if(checkFor(new TicTacToeLocation(0, 2), emptyLocations) &&
                checkFor(new TicTacToeLocation(1, 1), locations) &&
                checkFor(new TicTacToeLocation(2, 0), locations) ||
                checkFor(new TicTacToeLocation(0, 2), locations) &&
                checkFor(new TicTacToeLocation(1, 1), emptyLocations) &&
                checkFor(new TicTacToeLocation(2, 0), locations) ||
                checkFor(new TicTacToeLocation(0, 2), locations) &&
                checkFor(new TicTacToeLocation(1, 1), locations) &&
                checkFor(new TicTacToeLocation(2, 0), emptyLocations))
        {
            return true;
            //winner = player;
        }

        return false;
    }
    
    /**
     * Check to see if the location is in the given locations.
     * 
     * @param location  the location to look for.
     * @param locations the locations to look at.
     * @return true if the location is in the locations.
     */
    private boolean checkFor(final TicTacToeLocation        location,
                             final List<TicTacToeLocation> locations)
    {
        boolean retVal;

        retVal = false;

        for(int i = 0; i < locations.size(); i++)
        {
            final TicTacToeLocation loc;

            loc = locations.get(i);

            if(location.getRow() == loc.getRow() &&
                location.getCol() == loc.getCol())
            {
                retVal = true;
                break;
            }
        }

        return (retVal);
    }

}

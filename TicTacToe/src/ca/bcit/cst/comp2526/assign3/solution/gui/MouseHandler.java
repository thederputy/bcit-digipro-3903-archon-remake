package ca.bcit.cst.comp2526.assign3.solution.gui;


import ca.bcit.cst.comp2526.assign3.solution.TicTacToeGame;
import ca.bcit.cst.comp2526.assign3.solution.TicTacToeLocation;
import ca.bcit.cst.comp2526.assign3.solution.TicTacToePlayer;
import ca.bcit.cst.comp2526.assign3.solution.TicTacToeSquare;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;


/**
 * Handles mouse clicks on the TicTacToeViewSquares.  When the mouse is released
 * the game makes a (hopefully valid) move.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public class MouseHandler
    extends MouseAdapter
{
    /**
     * The frame that the game is being played on.
     */
    private final TicTacToeFrame frame;

    /**
     * The game being played.
     */
    private final TicTacToeGame game;

    /**
     * Constructs a new MuseHandler for the specified TicTacToeGameFrame.
     *
     * @param f the TicTacToeGameFrame that will be making moves on.
     * @throws IllegalArgumentException if f is null.
     */
    public MouseHandler(final TicTacToeFrame f)
    {
        if(f == null)
        {
            throw new IllegalArgumentException("f cannot be null");
        }

        frame = f;
        game  = frame.getGame();
    }

    /**
     * Make a move on the game associate with the TicTacToeGameFrame.
     *
     * @param evt the event.  Contains the TicTacToeSquareView that was clicked on.
     */
    @Override
    public void mouseReleased(final MouseEvent evt)
    {
        final TicTacToeSquareView view;
        final TicTacToeSquare     square;
        final TicTacToeLocation   location;

        view     = (TicTacToeSquareView)evt.getSource();
        square   = view.getSquare();
        location = square.getLocation();

        if(game.isValidMove(location))
        {
            final TicTacToePlayer player;

            player = game.getCurrentPlayer();
            player.setLocation(location);

            synchronized(player)
            {
                player.notifyAll();
            }
        }
    }
}

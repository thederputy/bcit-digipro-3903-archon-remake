package ca.bcit.cst.comp2526.assign3.solution.gui;


import ca.bcit.cst.comp2526.assign3.solution.InvalidLocationException;
import ca.bcit.cst.comp2526.assign3.solution.TicTacToeBoard;
import ca.bcit.cst.comp2526.assign3.solution.TicTacToeLocation;
import ca.bcit.cst.comp2526.assign3.solution.TicTacToeSquare;
import java.awt.GridLayout;
import java.awt.event.MouseListener;
import javax.swing.JPanel;


/**
 * A view of a TicTacToeBoard.
 *
 * @author D'Arcy Smith
 * @version 1.0
 */
public class TicTacToeBoardView
    extends JPanel
{
    /**
     * The board being played with.
     */
    private final TicTacToeBoard board;

    /**
     * Construct a TicTacToeBoardView with the specified TicTacToeBoard.
     *
     * @param b the TicTacToeBoard to display.
     * @throws IllegalArgumentException if b is null.
     */
    public TicTacToeBoardView(final TicTacToeBoard b)
    {
        if(b == null)
        {
            throw new IllegalArgumentException("b cannot be null");
        }

        board   = b;
    }

    /**
     * Initialize the view.
     * <p>
     * - set the layout to be a GridLayout
     * - add the TicTacToeSquareViews
     * - initialize the TicTacToeSquareViews
     *
     * @param squareListener the listener to add to each TicTacToeSquareView
     *                       to detect pieces being placed by the player.
     * @throws IllegalArgumentException if squareListener is null.
     */
    public void init(final MouseListener squareListener)
    {
        final GridLayout manager;

        if(squareListener == null)
        {
            throw new IllegalArgumentException("squareListener cannot be null");
        }

        manager = new GridLayout(board.getRows(), board.getCols());
        setLayout(manager);
        
        for(int row = 0; row < board.getRows(); row++)
        {
            for(int col = 0; col < board.getCols(); col++)
            {
                final TicTacToeLocation   location;
                final TicTacToeSquare     square;
                final TicTacToeSquareView view;

                try
                {
                    location = new TicTacToeLocation(row, col);
                }
                catch(final InvalidLocationException ex)
                {
                    throw new IllegalStateException("this can never happen");
                }

                square = board.getSquare(location);
                view   = new TicTacToeSquareView(square);
                view.init(squareListener);
                add(view);
            }
        }
    }
}
